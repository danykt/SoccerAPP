using System;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using FutbolApp.Helpers;
using FutbolApp.Modelo;

namespace FutbolApp.ViewModel
{
    public class TopTenModel: BaseViewModel
    {
        AzureService azureService;

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

     //   ICommand addPlayerCommand;
     //   public ICommand AddPlayerCommand =>
     //   addPlayerCommand ?? (addPlayerCommand = new Command(async () => await ExecuteFetchPlayersOperationAsync()));

      //  async Task ExecuteFetchPlayersOperationAsync()
      //  {
       //     if (IsBusy)
       //         return;

          //  try
         //   {
          //      if(string.IsNullOrWhiteSpace())
         //  }
       // }


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
    }
}
