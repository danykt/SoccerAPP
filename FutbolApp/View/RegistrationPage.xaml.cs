using System;
using System.Collections.Generic;
using FutbolApp.Modelo;
using FutbolApp.ViewModel;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace FutbolApp.View
{
    public partial class RegistrationPage : ContentPage
    {


        RegistrationPageModel vm;
        public RegistrationPage()
        {
            InitializeComponent();
            SetComponents();
            BindingContext = vm = new RegistrationPageModel();
        }

        void SetComponents()
        {
            BackgroundColor = Tools.BackgroundColor;
            Lbl_Username.TextColor = Tools.TextColor;
            Lbl_Password.TextColor = Tools.TextColor;
            Lbl_fname.TextColor = Tools.TextColor;
            Lbl_ConfirmPassword.TextColor = Tools.TextColor;


            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Tools.LoginIconSize;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //  CrossConnectivity.Current.ConnectivityChanged += ConnecitvityChanged;
            // OfflineStack.IsVisible = !CrossConnectivity.Current.IsConnected;


            // if (vm.playersCollection.Count == 0 && Settings.IsLoggedIn)
            //  vm.FetchPlayersOperation.Execute(null);
            // else
            // {
            //  await vm.LoginAsync();
            //if (Settings.IsLoggedIn)
            // vm.FetchPlayersOperation.Execute(null);
            //}
        }

        async void OnPreviousPageButtonClicked(Object sender, EventArgs eventArgs)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CrossConnectivity.Current.ConnectivityChanged -= ConnecitvityChanged;
        }



        void ConnecitvityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //  OfflineStack.IsVisible = !e.IsConnected;
            });
        }
    }
}
