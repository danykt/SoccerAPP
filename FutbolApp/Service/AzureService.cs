using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;
using Xamarin.Forms;
using FutbolApp.Helpers;
using FutbolApp.Authentication;
using FutbolApp;
using System.IO;
using Plugin.Connectivity;
using FutbolApp.Modelo;

[assembly: Dependency(typeof(AzureService))]
namespace FutbolApp
{
    public class AzureService
    {

        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<USUARIO> userTable;
        IMobileServiceSyncTable<PlayerInfo> playerInfoTable;
        IMobileServiceSyncTable<RATING> ratingsTable;


#if AUTH
        public static bool UseAuth { get; set; } = true;
#else
        public static bool UseAuth { get; set; } = false;
#endif

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;


            var appUrl = "https://edfrydelivery.azurewebsites.net";

#if AUTH
            Client = new MobileServiceClient(appUrl, new AuthHandler());

            if (!string.IsNullOrWhiteSpace (Settings.AuthToken) && !string.IsNullOrWhiteSpace (Settings.UserId)) {
                Client.CurrentUser = new MobileServiceUser (Settings.UserId);
                Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }
#else
            //Create our client

            Client = new MobileServiceClient(appUrl);

#endif

            //InitialzeDatabase for path
            var path = "syncstore.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<USUARIO>();
            store.DefineTable<PlayerInfo>();
            store.DefineTable<RATING>();


            //Initialize SyncContext
            await Client.SyncContext.InitializeAsync(store);

            //Get our sync table that will call out to azure
            userTable = Client.GetSyncTable<USUARIO>();
            playerInfoTable = Client.GetSyncTable<PlayerInfo>();
            ratingsTable = Client.GetSyncTable<RATING>();


        }

    

        public async Task SyncUsers()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await userTable.PullAsync("allUsers", userTable.CreateQuery());

                await Client.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync users, that is alright as we have offline capabilities: " + ex);
            }
        }

        public async Task <IEnumerable<PlayerInfo>> GetPlayersInfo()
        {
            await Initialize();
            await SyncPlayerInfo();

            return await playerInfoTable.OrderBy(c => c.DateUtc).ToEnumerableAsync();;

        }


        public async Task SyncPlayerInfo()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await playerInfoTable.PullAsync("allPlayers", playerInfoTable.CreateQuery());

                await Client.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync users, that is alright as we have offline capabilities: " + ex);
            }
        }



        public async Task SyncRatings()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await ratingsTable.PullAsync("allRatings", ratingsTable.CreateQuery());

                await Client.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync users, that is alright as we have offline capabilities: " + ex);
            }
        }
        public async Task<IEnumerable<RATING>> GetRatings()
        {
            await Initialize();
            await SyncRatings();

            return await ratingsTable.OrderBy(c => c.DateUtc).ToEnumerableAsync(); ;

        }

        public async Task<IEnumerable<USUARIO>> GetUsers()
        {
            await Initialize();
            await SyncUsers();

            return await userTable.OrderBy(c => c.DateUtc).ToEnumerableAsync();
        }


        public async Task<USUARIO> AddUser(string username, string fullname, string password)
        {
            await Initialize();
            var user = new USUARIO
            {
                DateUtc = DateTime.UtcNow,
                FullName = fullname,
                Username = username,
                Password = password
            };

            await userTable.InsertAsync(user);

            await SyncUsers();

            return user;
        }

        public async Task<PlayerInfo> AddPlayerInfo(string fname, string lname, string team)
        {
            await Initialize();

            var playerinfo = new PlayerInfo
            {
                FirstName = fname,
                LastName = lname,
                Team = team,
                DateUtc = DateTime.UtcNow,
                OS = Device.RuntimePlatform

            };

            await playerInfoTable.InsertAsync(playerinfo);

            await SyncPlayerInfo();
            //return coffee
            return playerinfo;
        }

        public async Task<RATING> AddRating(string userId,string playerId, string shot, string pass, string dribb, string defence, string speed)
        {
            await Initialize();

           

            var rating = new RATING
            {
                UserID = userId,
                PlayerInfoID = playerId,
                Shot = shot,
                Pass = pass,
                Dribbling = dribb,
                Defense = defence,
                Speed = speed,
                DateUtc = DateTime.UtcNow
            };


            await ratingsTable.InsertAsync(rating);

            await SyncRatings();
            //return coffee
            return rating;
        }



        public async Task<USUARIO> RemoveUser(USUARIO selected)
        {
            await Initialize();
            await userTable.DeleteAsync(selected);
            
            await SyncUsers();
            return selected;
        }

        public async Task<RATING> UpadteRating(RATING selected)
        {
            await Initialize();
            await ratingsTable.UpdateAsync(selected);

            await SyncRatings();
            return selected;
        }


        public async Task<PlayerInfo> RemovePlayer(PlayerInfo selected)
        {
            await Initialize();
            await playerInfoTable.DeleteAsync(selected);
            await SyncPlayerInfo();
            return selected;
        }


        public async Task<bool> LoginAsync()
        {

            await Initialize();

            var provider = MobileServiceAuthenticationProvider.Twitter;
            var uriScheme = "coffeecups";


#if __ANDROID__
            var user = await Client.LoginAsync(Forms.Context, provider, uriScheme);

#elif __IOS__
            CoffeeCups.iOS.AppDelegate.ResumeWithURL = url => url.Scheme == uriScheme && Client.ResumeWithURL(url);
            var user = await Client.LoginAsync(GetController(), provider, uriScheme);
            
#else
            var user = await Client.LoginAsync(provider, (Newtonsoft.Json.Linq.JObject)uriScheme);

#endif
            if (user == null)
            {
                Settings.AuthToken = string.Empty;
                Settings.UserId = string.Empty;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Login Error", "Unable to login, please try again", "OK");
                });
                return false;
            }
            else
            {
                Settings.AuthToken = user.MobileServiceAuthenticationToken;
                Settings.UserId = user.UserId;
            }

            return true;
        }


#if __IOS__
         UIKit.UIViewController GetController()
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            var root = window.RootViewController;
            if (root == null)
                return null;

            var current = root;
            while (current.PresentedViewController != null)
            {
                current = current.PresentedViewController;
            }

            return current;
        }
#endif
    }
}
