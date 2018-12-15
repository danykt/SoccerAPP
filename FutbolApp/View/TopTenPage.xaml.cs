using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Connectivity;
using FutbolApp.Helpers;

using FutbolApp.ViewModel;

/* Original author James Montemagno Coffe Cups License: MIT 
 * This class was modify from Coffee Cups project https://github.com/jamesmontemagno/app-coffeecups
 * The modification will be commented in each function of this class 
 * ALl sync and get functions are modified from coffee cups project to query information from
 * Futbol app database. All the structure follows coffee cups azure page.*/


namespace FutbolApp.View
{
    public partial class TopTenPage : ContentPage
    {
        TopTenModel vm;
        public TopTenPage()
        {
            InitializeComponent();
            BindingContext = vm = new TopTenModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            CrossConnectivity.Current.ConnectivityChanged += ConnecitvityChanged;
            OfflineStack.IsVisible = !CrossConnectivity.Current.IsConnected;


            if (vm.playersCollection.Count == 0 && Settings.IsLoggedIn)
                vm.FetchPlayersOperation.Execute(null);
            else
            {
                await vm.LoginAsync();
                if (Settings.IsLoggedIn)
                    vm.FetchPlayersOperation.Execute(null);
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
