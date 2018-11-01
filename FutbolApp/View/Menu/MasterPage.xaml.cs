using FutbolApp.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FutbolApp.View.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listview; } }
        public List<MasterMenuItem> items;
        public MasterPage()
        {
            InitializeComponent();
            SetItems();
        }

        void SetItems()
        {
            items = new List<MasterMenuItem>();
            items.Add(new MasterMenuItem("Edit", "icon.png", typeof(EditPlayersPage)));
            items.Add(new MasterMenuItem("Rate", "icon.png", typeof(RatePlayersPage)));
            items.Add(new MasterMenuItem("Top 10", "icon.png", typeof(TopTenPage)));
            items.Add(new MasterMenuItem("Manage", "icon.png", typeof(AddPlayerPage)));



            ListView.ItemsSource = items;

        }
    }
}