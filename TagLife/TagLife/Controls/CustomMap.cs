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
    }
}