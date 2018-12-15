using System;
using System.Collections.Generic;
using FutbolApp.Helpers;
using FutbolApp.ViewModel;
using Plugin.Connectivity;
using Xamarin.Forms;

/* Original author James Montemagno Coffe Cups License: MIT 
 * This class was modify from Coffee Cups project https://github.com/jamesmontemagno/app-coffeecups
 * Futbol app database. All the structure follows coffee cups azure page.*/

namespace FutbolApp.View
{
    public partial class EditPlayersPage : ContentPage
    {

        EditPlayersModel vm;
        public EditPlayersPage()
        {
            InitializeComponent();
            BindingContext = vm = new EditPlayersModel(Navigation); // modified
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            CrossConnectivity.Current.ConnectivityChanged += ConnecitvityChanged;
            OfflineStack.IsVisible = !CrossConnectivity.Current.IsConnected;


            if (vm.playersCollection.Count == 0 && Settings.IsLoggedIn)
            {
                vm.FetchPlayersOperation.Execute(null);
                vm.FetchRatingsOperation.Execute(null);
            }
            else
            {
                await vm.LoginAsync();
                if (Settings.IsLoggedIn)
                {
                    vm.FetchPlayersOperation.Execute(null);
                    vm.FetchRatingsOperation.Execute(null);
                }

            }
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
                OfflineStack.IsVisible = !e.IsConnected;
            });
        }
    }
}
