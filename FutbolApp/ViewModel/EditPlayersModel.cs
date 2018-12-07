using System;
using FutbolApp.Modelo;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using FutbolApp.Helpers;
using Xamarin.Auth;
using FutbolApp.View;


/* Edit players page renders all players chosen from the database by the user first we query
 all players then we relate which players are used by using our stored account userID which is 
 set during login. The ratings are also related to each players info in order to display everything
 in the ratePlayer page */


namespace FutbolApp.ViewModel
{
    public class EditPlayersModel: BaseViewModel
    {
        AzureService azureService;

        public INavigation Navigation { get; set; }

        private PlayerInfo _selectedPlayer { get; set; }
        public PlayerInfo SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                if (_selectedPlayer != value)
                {
                    _selectedPlayer = value;
                }
            }

        }


        public EditPlayersModel(INavigation navigation)
        {
            this.Navigation = navigation;

            azureService = DependencyService.Get<AzureService>();

        }


        public ObservableRangeCollection<PlayerInfo> playersCollection { get; } = new ObservableRangeCollection<PlayerInfo>();
        public ObservableRangeCollection<Grouping<string, PlayerInfo>> playersGrouped { get; } = new ObservableRangeCollection<Grouping<string, PlayerInfo>>();

        public ObservableRangeCollection<RATING> ratings { get; } = new ObservableRangeCollection<RATING>();
        public ObservableRangeCollection<Grouping<string, RATING>> ratingsGrouped { get; } = new ObservableRangeCollection<Grouping<string, RATING>>();

        string loadingLabel;
        public string LoadingLabel
        {
            get { return loadingLabel; }
            set { SetProperty(ref loadingLabel, value); }
        }

        bool deleted;
        public bool Deleted
        {
            get => deleted;
            set => SetProperty(ref deleted, value);
        }

        string firtsName;
        public string FirstName
        {
            get => firtsName;
            set => SetProperty(ref firtsName, value);

        }


        ICommand ratePlayersOperation;
        public ICommand RatePlayersOperation =>
        ratePlayersOperation ?? (ratePlayersOperation = new Command(async () => await RatePlayersOperationAsync()));

        async Task RatePlayersOperationAsync()
        {
            if (IsBusy)
                return;

            if(SelectedPlayer == null)
            {
                await Application.Current.MainPage.DisplayAlert("Select Player", "choose a player to rate", "ok");
                return;

            }

            try
            {
                var rating = ratings.FirstOrDefault(c => c.PlayerInfoID == SelectedPlayer.id);

                await Navigation.PushModalAsync(new RatePlayersPage(SelectedPlayer,rating));

               //Application.Current.MainPage = new NavigationPage(new RatePlayersPage(SelectedPlayer, rating));

            }
            catch (Exception ex)
            {
                Debug.Write("Something went wrong" + ex);
                await Application.Current.MainPage.DisplayAlert("Fetching Error", "Unable to fetch ratings, you may be offline", "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        ICommand fetchRatingsOperation;
        public ICommand FetchRatingsOperation =>
        fetchRatingsOperation ?? (fetchRatingsOperation = new Command(async () => await ExecuteFetchRatingsOperationAsync()));

        async Task ExecuteFetchRatingsOperationAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingLabel = "Fetching Ratings...";
                IsBusy = true;
                var ratingInDB = await azureService.GetRatings();
                ratings.ReplaceRange(ratingInDB.Where(c => c.UserID == UserId ));

                SetUserPlayers();
            }
            catch (Exception ex)
            {
                Debug.Write("Something went wrong" + ex);
                await Application.Current.MainPage.DisplayAlert("Fetching Error", "Unable to fetch ratings, you may be offline", "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }


        ICommand fetchPlayersOperation;
        public ICommand FetchPlayersOperation =>
        fetchPlayersOperation ?? (fetchPlayersOperation = new Command(async () => await ExecuteFetchPlayersOperationAsync()));

        async Task ExecuteFetchPlayersOperationAsync()
        {
            if (IsBusy)
                return;

            try
            {

               // await Application.Current.MainPage.DisplayAlert("Fetching Error", "User ID: " + UserId, "ok");

                LoadingLabel = "Fetching Players...";
                IsBusy = true;
                var players = await azureService.GetPlayersInfo();
                playersCollection.ReplaceRange(players);

                var ratingInDB = await azureService.GetRatings();
                ratings.ReplaceRange(ratingInDB.Where(c => c.UserID == UserId));

                SetUserPlayers();


            }
            catch (Exception ex)
            {
                Debug.Write("Something went wrong" + ex);
                await Application.Current.MainPage.DisplayAlert("Fetching Error", "Unable to fetch players, you may be offline", "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        ICommand removePlayerOperation;
        public ICommand RemovePlayerOperation =>
        removePlayerOperation ?? (removePlayerOperation = new Command(async () => await ExecuteRemovePlayerOperationAsync()));

        async Task ExecuteRemovePlayerOperationAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;

            try
            {

                if (SelectedPlayer == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Select Order", "Please select a order.", "OK");
                    return;
                }
                LoadingLabel = "Removing Order...";
                IsBusy = true;

                var player = await azureService.RemovePlayer(SelectedPlayer);

                playersCollection.Remove(player);
                RankPlayers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);
            }
            finally
            {
                LoadingLabel = string.Empty;
                IsBusy = false;
            }

        }

        void SetUserPlayers()
        {
            var playersInDB = playersCollection.ToList();

            playersCollection.Clear();


            var userRatings = ratings.ToList();


            foreach(PlayerInfo player in playersInDB)
            {
                foreach (RATING rating in ratings)
                {
                    if (player.id == rating.PlayerInfoID)
                    {
                        playersCollection.Add(player);
                    }
                }

            }
            

            RankPlayers();
        }
     


        void RankPlayers()
        {
            var set = from player in playersCollection
                      orderby player.DateUtc descending
                      group player by player.DateDisplay
                      into PlayersGroup
                      select new Grouping<string, PlayerInfo>($"{PlayersGroup.Key} ({PlayersGroup.Count()})", PlayersGroup);


            playersGrouped.ReplaceRange(set);
           // SavePlayersCount(playersGrouped.Count.ToString());


        }


        public void SavePlayersCount(string count)
        {



            var account = AccountStore.Create().FindAccountsForService(App.Name).FirstOrDefault();

            account.Properties.Add("PlayerCount", count);

            

            AccountStore.Create().Save(account, App.Name);


        }

        public Task<bool> LoginAsync()
        {
            if (Settings.IsLoggedIn)
                return Task.FromResult(true);


            return azureService.LoginAsync();
        }


        public string UserName
        {
            get{
                var account = AccountStore.Create().FindAccountsForService(App.Name).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }


        public string Password
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.Name).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }

        public string UserId
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.Name).FirstOrDefault();
                return (account != null) ? account.Properties["UserId"] : null;
            }
        }

       
    }
}








