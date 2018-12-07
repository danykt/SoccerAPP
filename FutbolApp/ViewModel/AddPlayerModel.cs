using System;
using FutbolApp.Modelo;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using FutbolApp.Helpers;


/* AddPlayerModel @Cesar Labastida
AddplayerModel class handles backend functionality for Addplayer.xml page
This class is binded to codebehind class Addplayer.cs. The labels of the pages
are set here in order to change the state of the page once players are queried.


 */


namespace FutbolApp.ViewModel
{
    public class AddPlayerModel : BaseViewModel
    {

        /* Labels to display state of the page loading and input labels to 
        add new players into the database*/

        AzureService azureService;
        public AddPlayerModel()
        {
            azureService = DependencyService.Get<AzureService>(); // initialize azure service conection
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


        /* add player command validates input from the addplayers page once validated the azure service 
        page adds this new player locally and to azure service db */

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
