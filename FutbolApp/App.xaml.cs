using System;
using FutbolApp.View;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FutbolApp
{
    public partial class App : Application
    {

        public static string Name { get; internal set; }
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
