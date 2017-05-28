using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using TagLife.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TagLife
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            // sorry no binding pushpins to map control in forms - we'll do it later with workaround
            //            BindingContext = new MainPageViewModel();
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            locator.PositionChanged += Locator_PositionChanged;
            await locator.StartListeningAsync(1000, 10);
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
//            MainMap.Pins.Add(new Pin()
//            {
//                Label = "sdfsdf",
//                Position = new Position(e.Position.Latitude, e.Position.Longitude)
//            });
        }

        private async void OnAddTagClicked(object sender, EventArgs e)
        {
            if (TagDescription.Text.IsNullOrWhitespace())
                return;

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();
            var item = new Pin()
            {
                Label = TagDescription.Text,
                Position = new Position(position.Latitude, position.Longitude),
            };

            item.Clicked += (sender1, e1) =>
            {
                var pin = (Pin)sender1;

                Application.Current.MainPage =new NavigationPage(new LocationPage(pin.Label));
            };
            MainMap.Pins.Add(item);

            // todo: call api

            TagDescription.Text = "";
        }
    }
}
