using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FutbolApp.View
{
    public partial class RatePlayersPage : ContentPage
    {
        public RatePlayersPage()
        {
            InitializeComponent();
            SetComponents();
        }

        void SetComponents()
        {
            for (int i = 1; i <= 5; i++)
            {
                SpeedPicker.Items.Add(i.ToString());
                PassPicker.Items.Add(i.ToString());
                DribbingPicker.Items.Add(i.ToString());
                DefensePicker.Items.Add(i.ToString());
                ShotPicker.Items.Add(i.ToString());

            }
        }
    }
}
