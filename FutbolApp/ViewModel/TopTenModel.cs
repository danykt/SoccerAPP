using System;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using FutbolApp.Helpers;
using FutbolApp.Modelo;
using Xamarin.Auth;

namespace FutbolApp.ViewModel
{
    public class TopTenModel: BaseViewModel
    {
        AzureService azureService;
        int playerCount;

        private PlayerInfo _selectedPlayer { get; set; }
        public PlayerInfo SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                if(_selectedPlayer != value)
                {
                    _selectedPlayer = value;
                }
            }

        }


        public TopTenModel()
        {
            
           // playerCount = Int16.Parse(PlayerCount);
            azureService = DependencyService.Get<AzureService>();

        }

        public ObservableRangeCollection<PlayerInfo> playersCollection { get; } = new ObservableRangeCollection<PlayerInfo>();
        public ObservableRangeCollection<Grouping<string, PlayerInfo>> playersGrouped { get; } = new ObservableRangeCollection<Grouping<string, PlayerInfo>>();


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

        ICommand fetchPlayersOperation;
        public ICommand FetchPlayersOperation =>
        fetchPlayersOperation ?? (fetchPlayersOperation = new Command(async () => await ExecuteFetchPlayersOperationAsync()));

        async Task ExecuteFetchPlayersOperationAsync()
        {
            if (IsBusy)
                return;

            try
            {
                LoadingLabel = "Fetching Players...";
                IsBusy = true;
                var players = await azureService.GetPlayersInfo();
                playersCollection.ReplaceRange(players);

                RankPlayers();
            }catch(Exception ex)
            {
                Debug.Write("Something went wrong" + ex);
                await Application.Current.MainPage.DisplayAlert("Fetching Error", "Unable to fetch players, you may be offline", "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        ICommand removePlayerCommand;
        public ICommand RemovePlayerCommand =>
        removePlayerCommand ?? (removePlayerCommand = new Command(async () => await ExecuteRemovePlayerCommandAsync()));

        async Task ExecuteRemovePlayerCommandAsync()
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

                playersCollection.Remove(SelectedPlayer);
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

        ICommand addRatingOperation;
        public ICommand AddRatingOperation =>
        addRatingOperation ?? (addRatingOperation = new Command(async () => await ExecuteAddRateOperationAsync()));

        async Task ExecuteAddRateOperationAsync()
        {
            try
            {
                if (SelectedPlayer == null)
                    return;

            /*    if(playerCount == 10)
                {
                    await Application.Current.MainPage.DisplayAlert("Limit Reach", "Your top 10 list is now completed go to edit players page to remove players", "OK");

                }
            */


                LoadingLabel = "Adding Player Information..";
                IsBusy = true;



                var playerRate = await azureService.AddRating(UserId, SelectedPlayer.id,"0","0","0","0","0");

               

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Something went wrong" + ex);

            }
            finally
            {
                LoadingLabel = string.Empty;
                IsBusy = false;
            }
            playerCount++;
            await Application.Current.MainPage.DisplayAlert("Player Added", "Go to Top 10 page to rate your player", "OK");

        }

        void RankPlayers()
        {
            var set = from player in playersCollection
                orderby player.DateUtc descending
                      group player by player.DateDisplay
                      into PlayersGroup
                      select new Grouping<string, PlayerInfo>($"{PlayersGroup.Key} ({PlayersGroup.Count()})", PlayersGroup);


            playersGrouped.ReplaceRange(set);

        }

        public Task<bool> LoginAsync()
        {
            if (Settings.IsLoggedIn)
                return Task.FromResult(true);


            return azureService.LoginAsync();
        }

        public string UserName
        {
            get
            {
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

        public string PlayerCount
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(App.Name).FirstOrDefault();
                return (account != null) ? account.Properties["PlayerCount"] : null;
            }
        }
    }
}
