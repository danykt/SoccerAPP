using System;
using System.Collections.Generic;
using System.Diagnostics;
using FutbolApp.Modelo;
using Xamarin.Forms;



/* Original author James Montemagno Coffe Cups License: MIT 
 * This class was modify from Coffee Cups project https://github.com/jamesmontemagno/app-coffeecups
 * All the structure follows coffee cups azure page.*/


namespace FutbolApp.View
{
    public partial class RatePlayersPage : ContentPage
    {

        AzureService azureService;
        RATING current;
        public RatePlayersPage()
        {
            InitializeComponent();
            SetComponents();
        }

        public RatePlayersPage(PlayerInfo info, RATING rate)
        {
            InitializeComponent();
            SetComponents();

            current = rate;

            azureService = DependencyService.Get<AzureService>();

            setGeneralRating(rate.getGeneralRating());

            PlayerName.Text = info.FirstName + " " + info.LastName;

            SpeedPicker.SelectedItem = rate.Speed;
            PassPicker.SelectedItem = rate.Pass;
            DribbingPicker.SelectedItem = rate.Dribbling;
            DefensePicker.SelectedItem = rate.Defense;
            ShotPicker.SelectedItem = rate.Shot;



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




        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                double genRating = (Int32.Parse(SpeedPicker.SelectedItem.ToString()) + Int32.Parse(PassPicker.SelectedItem.ToString())
                                        + Int32.Parse(DribbingPicker.SelectedItem.ToString()) + Int32.Parse(DefensePicker.ToString())
                                    + Int32.Parse(ShotPicker.ToString()))/5.0;

                setGeneralRating(genRating);


            }
            catch (Exception ex)
            {
                setGeneralRating(0.0);

            }
        }

        void setGeneralRating(double rate)
        {
            if(rate >= 4.0)
            {
                PlayerGeneralRating.TextColor = Color.Green;
                PlayerGeneralRating.Text = rate.ToString();
            }
            else if(rate < 4.0 && rate >= 2.0)
            {
                PlayerGeneralRating.TextColor = Color.Yellow;
                PlayerGeneralRating.Text = rate.ToString();
            }
            else
            {
                PlayerGeneralRating.TextColor = Color.Red;
                PlayerGeneralRating.Text = rate.ToString();
            }
        }

        async void OnPreviousPageButtonClicked(Object sender, EventArgs eventArgs)
        {
            await Navigation.PopModalAsync();
        }
        async void UpdatePlayerAsyncProcedure(object sender, EventArgs e)
        {

            current.Defense = DefensePicker.SelectedItem.ToString();
            current.Dribbling = DribbingPicker.SelectedItem.ToString();
            current.Speed = SpeedPicker.SelectedItem.ToString();
            current.Pass = PassPicker.SelectedItem.ToString();
            current.Shot = ShotPicker.SelectedItem.ToString();


            try
            {
               

                IsBusy = true;



                var playerRate = await azureService.UpadteRating(current);



            }
            catch (Exception ex)
            {
                Debug.WriteLine("Something went wrong" + ex);

            }
            finally
            {
                IsBusy = false;
            }

            await DisplayAlert("Changes Saved", "Your Ratings have been updated", "OK");

        }
    }
}
