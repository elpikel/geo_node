using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using Xamarin.Forms.Maps;

namespace TagLife
{
    [ImplementPropertyChanged]
    public class CustomMap : Map
    {
        private ObservableCollection<CustomPin> _customPins = new ObservableCollection<CustomPin>();
        public ObservableCollection<CustomPin> CustomPins
        {
            get
            {
                return _customPins;
            }
            set
            {
                _customPins = value;
                OnPropertyChanged();
            }
        }

        public CustomMap()
        {
            _customPins.CollectionChanged += _customPins_CollectionChanged;
        }

        private void _customPins_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CustomPins));
        }
    }
}