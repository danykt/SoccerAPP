using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FutbolApp.Modelo;
using MvvmHelpers;
using Xamarin.Forms;

namespace FutbolApp.ViewModel
{
    public class RatePlayersModel : BaseViewModel
    {
      

        public RatePlayersModel(PlayerInfo playerinfo, RATING rating)
        {



        }



        


        string loadingLabel;
        public string LoadingLabel
        {
            get { return loadingLabel; }
            set { SetProperty(ref loadingLabel, value); }
        }

       


    }
}
