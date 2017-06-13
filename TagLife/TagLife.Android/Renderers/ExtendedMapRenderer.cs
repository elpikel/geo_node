using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using TagLife.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using System.IO;
using System.Linq;
using Android.Graphics;
using TagLife.Controls;
using TagLife.Droid.Services;
using TagLife.Helpers;
using Console = System.Console;

[assembly: ExportRenderer(typeof(CustomMap), typeof(ExtendedMapRenderer))]
namespace TagLife.Droid.Renderers
{
    public class ExtendedMapRenderer : MapRenderer, GoogleMap.IOnMarkerClickListener
    {
        private readonly List<Marker> _customPinsOnMap = new List<Marker>();
        private ImmutableList<CustomPin> _oldCustomPins = ImmutableList<CustomPin>.Empty;
        private bool _isMapInitialized;

        private CustomMap _map;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                _map = (CustomMap)e.NewElement;
            }
        }

        public bool OnMarkerClick(Marker marker)
        {
            // todo: invode marker's action
            marker.Remove();
            return true;
        }

        private void AddNewPins(IEnumerable<CustomPin> newPins)
        {
            foreach (var newItem in newPins)
            {
                var markerDescription = new MarkerCreator().CreateMarker(newItem);
                var markerr = NativeMap.AddMarker(markerDescription);
                _customPinsOnMap.Add(markerr);
            }
        }

        private void RemovePins(IEnumerable<CustomPin> toBeRemovedPins)
        {
            foreach (var oldItem in toBeRemovedPins)
            {
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

            if (e.PropertyName.Equals(nameof(CustomMap.CustomPins)))
            {
                var newCustomPins = _map.CustomPins;
                var news = _oldCustomPins.GetNews(newCustomPins);
                var olds = _oldCustomPins.GetMissings(newCustomPins);

                AddNewPins(news);
                RemovePins(olds);
                _oldCustomPins = newCustomPins;
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