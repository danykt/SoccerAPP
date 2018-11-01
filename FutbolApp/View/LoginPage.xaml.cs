using System;
using System.Collections.Generic;
using FutbolApp.Modelo;
using Xamarin.Forms;
using FutbolApp.View.Menu;

namespace FutbolApp.View
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            SetComponents();
        }

        void SetComponents()
        {
            BackgroundColor = Tools.BackgroundColor;
            Label_Pass.TextColor = Tools.TextColor;
            Label_User.TextColor = Tools.TextColor;

            Spinner.IsVisible = false;
            LoginIcon.HeightRequest = Tools.LoginIconSize;
        }

        async void Signin_Async(object sender, EventArgs e)
        {
            if(Device.OS == TargetPlatform.Android)
            {
                Application.Current.MainPage = new NavigationPage(new MasterDetail());
            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                await Navigation.PushModalAsync(new NavigationPage(new MasterDetail()));
            }
        }

        async void Register_Async(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.Android)
            {
                Application.Current.MainPage = new RegistrationPage();
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
                Application.Current.MainPage = new AddPlayerPage();
            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                await Navigation.PushModalAsync(new AddPlayerPage());
            }
        }
    }
}
