using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Geolocator;
using PropertyChanged;
using TagLife.Controls;
using TagLife.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TagLife.ViewModels
{
    [ImplementPropertyChanged]
    public class MainPageViewModel
    {
        public ImmutableList<CustomPin> Pins { get; set; } = ImmutableList<CustomPin>.Empty;

//        public MapSpan View { get; set; }

        public bool IsShowingUser { get; set; } = true;

        public string Comment { get; set; }

        public MainPageViewModel()
        {
//            View = MapSpan.FromCenterAndRadius(new Position(), Distance.FromKilometers(9999));
        }

        public async Task StartTracking()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            // todo: unpin
            locator.PositionChanged += Locator_PositionChanged;
            await locator.StartListeningAsync(1000, 10);
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
//            View = MapSpan.FromCenterAndRadius(e.Position.ToXamarinPosition(), Distance.FromMeters(500));
        }

        public ICommand AddComment
        {
            get
            {
                return new Command(async () =>
                {
                    await Task.Delay(0);

                    if (Comment.IsNullOrWhitespace())
                    {
                        return;
                    }

                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 300;
                    var position = await locator.GetPositionAsync();

                    Pins = Pins.Add(new CustomPin(position, Guid.NewGuid().ToString(), Comment));
                    Comment = "";
                });
            }
        }
    }
}