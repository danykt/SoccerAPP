using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Connectivity;
using FutbolApp.Helpers;

using FutbolApp.ViewModel;

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
