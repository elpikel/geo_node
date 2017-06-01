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
                var markerDescription = CreateMarker(newItem);
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

        private MarkerOptions CreateMarker(CustomPin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            // todo: verify if this is a fine way of saving additional data
            marker.SetSnippet(pin.Id);

            var inflater = Android.App.Application.Context.GetSystemService("layout_inflater") as Android.Views.LayoutInflater;


            var inflate = inflater.Inflate(Resource.Layout.Pinlayout, null);


            var findViewById = inflate.FindViewById<TextView>(Resource.Id.jols);
            findViewById.Text = pin.Text;

            inflate.Measure(MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
                MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));

            inflate.Layout(0, 0, inflate.MeasuredWidth, inflate.MeasuredHeight);

            // todo: optimize memory usage
            inflate.DrawingCacheEnabled = true;
            inflate.BuildDrawingCache(true);
            var drawingCache = inflate.GetDrawingCache(true);


            // ExportBitmapAsPNG(drawingCache);
            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(drawingCache));
            return marker;
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