using System;
using Plugin.Geolocator;
using TagLife.Controls;
using TagLife.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TagLife.Pages
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
            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(e.Position.ToXamarinPosition(), Distance.FromKilometers(2)));
        }

        private async void OnAddTagClicked(object sender, EventArgs e)
        {
            if (TagDescription.Text.IsNullOrWhitespace())
            {
                return;
            }

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync();

            MainMap.CustomPins.Add(new CustomPin(position, Guid.NewGuid().ToString(), TagDescription.Text));

            TagDescription.Text = "";
        }
    }
}
