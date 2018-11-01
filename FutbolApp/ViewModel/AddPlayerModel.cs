using System;
using FutbolApp.Modelo;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using FutbolApp.Helpers;


namespace FutbolApp.ViewModel
{
    public class AddPlayerModel : BaseViewModel
    {
        AzureService azureService;
        public AddPlayerModel()
        {
            azureService = DependencyService.Get<AzureService>();
        }

        string loadingLabel;
        public string LoadingLabel
        {
            get { return loadingLabel; }
            set { SetProperty(ref loadingLabel, value); }
        }

      
        string firstName;
        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);

        }

        string lastName;
        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);

        }

        string team;
        public string Team
        {
            get => team;
            set => SetProperty(ref team, value);

        }


        ICommand addPlayerInfoOperation;
        public ICommand AddPlayerInfoOperation =>
        addPlayerInfoOperation ?? (addPlayerInfoOperation = new Command(async () => await ExecuteAddPlayerOperationAsync()));

        async Task ExecuteAddPlayerOperationAsync()
        {
            try
            {
                if(string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Team))
                {
                    await Application.Current.MainPage.DisplayAlert("Some fields are required", "Please fill all fields", "Ok");
                    return;
                }

                LoadingLabel = "Adding Player Information..";
                IsBusy = true;



                var playerInfo = await azureService.AddPlayerInfo(FirstName, LastName, Team);

                FirstName = string.Empty;
                LastName = string.Empty;
                Team = string.Empty;

            }
            catch(Exception ex)
            {
                Debug.WriteLine("Something went wrong" + ex);

            }
            finally
            {
                LoadingLabel = string.Empty;
                IsBusy = false;
            }
            
        }

    }
}
