using System;
using FutbolApp.Modelo;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using FutbolApp.Helpers;


/* Original author James Montemagno Coffe Cups License: MIT 
 * This class was modify from Coffee Cups project https://github.com/jamesmontemagno/app-coffeecups
 * The modification will be commented in each function of this class 
 * Futbol app database. All the structure follows coffee cups azure page.*/

namespace FutbolApp.ViewModel
{
    public class RegistrationPageModel : BaseViewModel
    {
        AzureService azureService;

     


        public RegistrationPageModel()
        {
            azureService = DependencyService.Get<AzureService>();
        }

        public ObservableRangeCollection<USUARIO> Users { get; } = new ObservableRangeCollection<USUARIO>();
        public ObservableRangeCollection<Grouping<string, USUARIO>> UsersGrouped { get; } = new ObservableRangeCollection<Grouping<string, USUARIO>>();


        string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { SetProperty(ref loadingMessage, value); }
        }


        bool borrado;
        public bool Borrado
        {
            get => borrado;
            set => SetProperty(ref borrado, value);
        }



        string username;
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        string confirmpassword;
        public string ConfirmPassword
        {
            get => confirmpassword;
            set => SetProperty(ref confirmpassword, value);
        }

      
        string fullName;
        public string FullName
        {
            get => fullName;
            set => SetProperty(ref fullName, value);
        }





        void SortUsers()
        {
            var groups = from user in Users
                         orderby user.DateUtc descending
                                     group user by user.DateLabel
                into usersGroup
                         select new Grouping<string, USUARIO>($"{usersGroup.Key} ({usersGroup.Count()})", usersGroup);


            UsersGrouped.ReplaceRange(groups);
        }

        ICommand registerUserOperation;
        public ICommand RegisterUserOperation =>
        registerUserOperation ?? (registerUserOperation = new Command(async () => await RegisterUserOperationAsync()));

        async Task RegisterUserOperationAsync()
        {
            //if (IsBusy || !(await LoginAsync()))
            //return;
            try
            {

                if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Needs Username or Password", "Please enter username and password.", "OK");
                    return;
                }
                else if (Password != ConfirmPassword)
                {
                    await Application.Current.MainPage.DisplayAlert("Passwords dont match", "Please confirm your password", "OK");
                    return;

                }
                else if (string.IsNullOrEmpty(FullName))
                {
                    await Application.Current.MainPage.DisplayAlert("Full Name required", "Full Name is required in order to sign up", "OK");
                    return;

                }



                LoadingMessage = "Registering User...";
                IsBusy = true;


                var user = await azureService.AddUser(Username, FullName, Password);

                if(user != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Congratulations you have successfully signed up", "OK");
                    return;
                }


                Username = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;
                FullName = string.Empty;
                Borrado = false;
                // Customers.Add(customer);
                Users.Add(user);
                SortUsers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;
            }

        }

  

        public Task<bool> LoginAsync()
        {
            if (Settings.IsLoggedIn)
                return Task.FromResult(true);


            return azureService.LoginAsync();
        }

    }
}
