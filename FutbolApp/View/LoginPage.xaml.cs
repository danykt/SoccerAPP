using System;
using System.Collections.Generic;
using FutbolApp.Modelo;
using Xamarin.Forms;
using FutbolApp.View.Menu;
using Xamarin.Auth;
using System.Linq;

namespace FutbolApp.View
{
    public partial class LoginPage : ContentPage
    {

        AzureService azureService;
        public LoginPage()
        {
            InitializeComponent();
            SetComponents();
        }

        void SetComponents()
        {
            azureService = DependencyService.Get<AzureService>();


            BackgroundColor = Tools.BackgroundColor;
            Label_Pass.TextColor = Tools.TextColor;
            Label_User.TextColor = Tools.TextColor;

            Spinner.IsVisible = false;
            LoginIcon.HeightRequest = Tools.LoginIconSize;
        }

        async void Signin_Async(object sender, EventArgs e)
        {


            string setUsername = Entry_User.Text;
            string setPassword = Entry_Pass.Text;


            if (String.IsNullOrWhiteSpace(setUsername) || String.IsNullOrWhiteSpace(setPassword))
            {
                await DisplayAlert("Need login credentials", "Please login with your credentials or Register", "OK");
                return;
            }

            if (IsBusy)
                return;

          try
           {
                Spinner.IsVisible = true;

                IsBusy = true;

                var usersInDB = await azureService.GetUsers();

                var accessUser = usersInDB.SingleOrDefault(x => x.Username == setUsername && x.Password == setPassword);


                if(accessUser != null)
                {
                    await DisplayAlert("Login", "Login Success", "OK");
                    SaveCredentials(accessUser.Username, accessUser.Password, accessUser.Id);
                    if (Device.OS == TargetPlatform.Android)
                    {
                        Application.Current.MainPage = new NavigationPage(new MasterDetail());
                    }
                    else if (Device.OS == TargetPlatform.iOS)
                    {
                        await Navigation.PushModalAsync(new NavigationPage(new MasterDetail()));
                    }
                }
                else
                {
                    await DisplayAlert("Login", "Login failed incorrect password/username combination", "OK");
                }
                                                          
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                await DisplayAlert("Login", "Server Error" + ex, "OK");
            }
            finally
            {
                IsBusy = false;
                Spinner.IsVisible = false;
            }


        }

        async void Register_Async(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.Android)
            {
              //Application.Current.MainPage = new NavigationPage(new RegistrationPage());
                await Navigation.PushModalAsync(new RegistrationPage());

            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                await Navigation.PushModalAsync(new RegistrationPage());
            }
        }

    
        async void AddPlayer_Async(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.Android)
            {
              // Application.Current.MainPage = new NavigationPage(new AddPlayerPage());
                await Navigation.PushAsync(new AddPlayerPage());

            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                await Navigation.PushModalAsync(new AddPlayerPage());
            }
        }


        public void SaveCredentials(string username, string password, string userid)
        {
            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = username
                };

                account.Properties.Add("Password", password);
                account.Properties.Add("UserId", userid);
                AccountStore.Create().Save(account, App.Name);

            }
        }
    }
}
