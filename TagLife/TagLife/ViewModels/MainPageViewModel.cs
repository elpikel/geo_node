using System.Collections.Generic;
using PropertyChanged;
using Xamarin.Forms.Maps;

namespace TagLife.ViewModels
{
    [ImplementPropertyChanged]
    public class MainPageViewModel
    {
        public List<Pin> Pins { get; set; }

        public MainPageViewModel()
        {
            Pins = new List<Pin>()
        {
            new Pin()
            {
                Position = new Position(0,0),
                Label = "hehe"
            }
        };
        }

        public bool IsShowingUser { get; set; } = true;
    }
}