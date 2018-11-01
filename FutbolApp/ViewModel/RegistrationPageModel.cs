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
    public class RegistrationPageModel : BaseViewModel
    {
        AzureService azureService;

        private USUARIO _selectedUser { get; set; }
        public USUARIO SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if(_selectedUser != value)
                {
                    _selectedUser = value;
                }
            }

        }


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

        string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        ICommand loadUsersCommand;
        public ICommand LoadUsersCommand =>
            loadUsersCommand ?? (loadUsersCommand = new Command(async () => await ExecuteLoadUsersCommandAsync()));

        async Task ExecuteLoadUsersCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;


            try
            {
                LoadingMessage = "Loading Users...";
                IsBusy = true;
                var users = await azureService.GetUsers();
                Users.ReplaceRange(users);


                SortUsers();


            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync users, you may be offline", "OK");
            }
            finally
            {
                IsBusy = false;
            }


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

        ICommand addUserCommand;
        public ICommand AddUserCommand =>
            addUserCommand ?? (addUserCommand = new Command(async () => await ExecuteAddUserCommandAsync()));

        async Task ExecuteAddUserCommandAsync()
        {
            //if (IsBusy || !(await LoginAsync()))
            //return;
            try
            {

                if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Needs Name", "Please enter username and password.", "OK");
                    return;
                }
                else if (Password != ConfirmPassword)
                {
                    await Application.Current.MainPage.DisplayAlert("Passwords dont match", "Please confirm your password", "OK");
                    return;

                }
                else if (string.IsNullOrEmpty(Email))
                {
                    await Application.Current.MainPage.DisplayAlert("Passwords dont match", "An Email is required in order to sign up", "OK");
                    return;

                }

                // await Application.Current.MainPage.DisplayAlert("Needs Name", "This is after checking input", "OK");

                LoadingMessage = "Adding User...";
                IsBusy = true;


                var user = await azureService.AddUser(Username, Password, "Cesar Labastida", "Developer", Email);


                Username = string.Empty;
                Password = string.Empty;
                ConfirmPassword = string.Empty;
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

        ICommand removeUserCommand;
        public ICommand RemoveUserCommand =>
            removeUserCommand ?? (removeUserCommand = new Command(async () => await ExecuteRemoveUserCommandAsync()));

        async Task ExecuteRemoveUserCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;

            try
            {

                if (SelectedUser == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Select User", "Please select a user.", "OK");
                    return;
                }
                LoadingMessage = "Removing Customer...";
                IsBusy = true;

                var user = await azureService.RemoveUser(SelectedUser);

                Users.Remove(user);
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
