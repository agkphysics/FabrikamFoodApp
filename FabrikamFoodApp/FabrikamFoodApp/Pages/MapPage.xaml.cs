using FabrikamFoodApp.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FabrikamFoodApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        // Set initial centre to somewhere in Auckland
        private MapSpan lastRegion = MapSpan.FromCenterAndRadius(new Position(-36.888283, 174.814765), Distance.FromKilometers(13));

        public MapPage()
        {
            InitializeComponent();
            InitMap();
        }

        private async void InitMap()
        {
            map.MoveToRegion(lastRegion);

            var stores = await AzureManager.CurrentInstance.CurrentClient.GetTable<Stores>().ToEnumerableAsync();
            foreach (var s in stores)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Address = s.Address,
                    Label = s.Name,
                    Position = new Position(s.lat, s.lon)
                };
                map.Pins.Add(pin);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            map.MoveToRegion(lastRegion);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            lastRegion = map.VisibleRegion;
        }
    }
}
