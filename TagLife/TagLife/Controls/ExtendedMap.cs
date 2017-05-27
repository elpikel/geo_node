using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TagLife.Controls
{
    public class ExtendedMap : Map
    {
        public readonly BindableProperty BindableProperty = BindableProperty.Create(
            nameof(Pinss),
            typeof(List<Pin>),
            typeof(ExtendedMap),
            null,
            BindingMode.OneWay,
            null,
            (bindable, value, newValue) =>
            {
                ReplacePins(bindable, newValue);
            });

        public List<Pin> Pinss
        {
            get { return (List<Pin>)GetValue(BindableProperty); }
            set { SetValue(BindableProperty, value); }
        }

        // yep, thats dirty. implement smart removing later
        private static void ReplacePins(BindableObject bindable, object newValue)
        {
            var extendedMap = (ExtendedMap)bindable;

            var numberOfPins = extendedMap.Pins.Count;

            for (int i = numberOfPins - 1; i >= 0; i--)
            {
                extendedMap.Pins.RemoveAt(i);
            }

            var newPins = (List<Pin>)newValue;

            foreach (var pin in newPins)
            {
                extendedMap.Pins.Add(pin);
            }
        }
    }
}