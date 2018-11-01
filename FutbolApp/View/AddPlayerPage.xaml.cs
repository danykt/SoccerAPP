using System;
using System.Collections.Generic;
using FutbolApp.Modelo;
using FutbolApp.ViewModel;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace FutbolApp.View
{
    public partial class AddPlayerPage : ContentPage
    {
        AddPlayerModel vm;
        public AddPlayerPage()
        {
            InitializeComponent();
            BindingContext = vm = new AddPlayerModel();
        }

        void SetComponents()
        {
            BackgroundColor = Tools.BackgroundColor;
            Lbl_Team.TextColor = Tools.TextColor;
            Lbl_LastName.TextColor = Tools.TextColor;
            Lbl_FirstName.TextColor = Tools.TextColor;


            ActivitySpinner.IsVisible = false;
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
