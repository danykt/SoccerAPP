using System;
using System.Collections.Generic;
using FutbolApp.Modelo;
using Xamarin.Forms;

namespace FutbolApp.View
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            SetComponents();
        }

        void SetComponents()
        {
            BackgroundColor = Tools.BackgroundColor;
            Lbl_Username.TextColor = Tools.TextColor;
            Lbl_Password.TextColor = Tools.TextColor;
            Lbl_ConfirmPassword.TextColor = Tools.TextColor;
            Lbl_email.TextColor = Tools.TextColor;


            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Tools.LoginIconSize;
        }
    }
}
