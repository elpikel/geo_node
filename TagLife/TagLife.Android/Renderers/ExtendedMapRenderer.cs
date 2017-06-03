using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using TagLife;
using TagLife.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using System.IO;
using System.Linq;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using TagLife.Controls;
using TagLife.Droid.Services;
using Console = System.Console;

[assembly: ExportRenderer(typeof(CustomMap), typeof(ExtendedMapRenderer))]
namespace TagLife.Droid.Renderers
{
    public class ExtendedMapRenderer : MapRenderer, GoogleMap.IOnMarkerClickListener
    {
        private readonly List<Marker> _customPinsOnMap = new List<Marker>();

        private bool _isMapInitialized;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                // todo: unsubscribe from event
                var formsMap = (CustomMap)e.NewElement;
                formsMap.CustomPins.CollectionChanged += CustomPins_CollectionChanged;
            }
        }

        public bool OnMarkerClick(Marker marker)
        {
            // todo: invode marker's action
            marker.Remove();
            return true;
        }

        private void CustomPins_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var newPins = e.NewItems?.Cast<CustomPin>();
            var toBeRemovedPins = e.OldItems?.Cast<CustomPin>();

            if (_isMapInitialized)
            {
                AddNewPins(newPins);
                RemovePins(toBeRemovedPins);
            }
            else
            {
                throw new Exception("Pin changed before map initialization");
            }
        }

        private void AddNewPins(IEnumerable<CustomPin> newPins)
        {
            if (newPins == null)
            {
                return;
            }

            foreach (var newItem in newPins)
            {
                var markerDescription = new MarkerCreator().CreateMarker(newItem);
                var markerr = NativeMap.AddMarker(markerDescription);
                _customPinsOnMap.Add(markerr);
            }
        }

        private void RemovePins(IEnumerable<CustomPin> toBeRemovedPins)
        {
            if (toBeRemovedPins == null)
            {
                return;
            }

            foreach (var oldItem in toBeRemovedPins)
            {
                // todo: verify if marker has correct equals and removes correctly
                var marker = _customPinsOnMap.First(m => m.Snippet == oldItem.Id);
                marker.Remove();
                _customPinsOnMap.Remove(marker);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Console.WriteLine(e.PropertyName);

            if (e.PropertyName.Equals("VisibleRegion"))
            {
                if (NativeMap != null && !_isMapInitialized)
                {
                    NativeMap.SetOnMarkerClickListener(this);
                    _isMapInitialized = true;
                }
            }
        }

        

        void ExportBitmapAsPNG(Bitmap bitmap)
        {
            var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = System.IO.Path.Combine(sdCardPath, "test.png");
            var stream = new FileStream(filePath, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();
        }
    }
}