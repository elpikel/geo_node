using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using PropertyChanged;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TagLife.Controls
{
    [ImplementPropertyChanged]
    public class CustomMap : Map
    {
        public static readonly BindableProperty CustomPinsProperty = BindableProperty.Create(
             nameof(CustomPins),
             typeof(ImmutableList<CustomPin>),
             typeof(CustomMap),
             ImmutableList<CustomPin>.Empty,
             BindingMode.OneWay,
             null,
             (bindable, value, newValue) =>
             {
                 var customMap = (CustomMap) bindable;
                 customMap.OnPropertyChanged(nameof(CustomPins));
             });

        public ImmutableList<CustomPin> CustomPins
        {
            get { return (ImmutableList<CustomPin>)GetValue(CustomPinsProperty); }
            set { SetValue(CustomPinsProperty, value); }
        }

        public static readonly BindableProperty RegionProperty = BindableProperty.Create(
            nameof(Region),
            typeof(MapSpan),
            typeof(CustomMap),
            MapSpan.FromCenterAndRadius(new Position(54.37, 18.62), Distance.FromKilometers(30)),
            BindingMode.OneWay,
            null,
            (bindable, value, newValue) =>
            {
                if (value == null)
                {
                    return;
                }

                var customMap = (CustomMap)bindable;
                customMap.MoveToRegion((MapSpan)value);
            });

        public MapSpan Region
        {
            get { return (MapSpan)GetValue(CustomPinsProperty); }
            set { SetValue(CustomPinsProperty, value); }
        }
    }
}