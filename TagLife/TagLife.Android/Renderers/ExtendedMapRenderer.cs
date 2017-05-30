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
using System.Diagnostics;

[assembly: ExportRenderer(typeof(CustomMap), typeof(ExtendedMapRenderer))]
namespace TagLife.Droid.Renderers
{
    public class ExtendedMapRenderer : MapRenderer, GoogleMap.IOnMarkerClickListener, IOnMapReadyCallback
    {
        GoogleMap map;
        List<CustomPin> customPins;
        bool isDrawn;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                formsMap.PropertyChanged += FormsMap_PropertyChanged;
                //                customPins = formsMap.CustomPins;
                ((MapView)Control).GetMapAsync(this);
            }
        }

        private void FormsMap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.PropertyName);
        }


        //public new void OnCameraChange(CameraPosition position)
        //{
        //    map.Clear();

        //    foreach (var pin in customPins)
        //    {
        //        var marker = new MarkerOptions();
        //        marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
        //        marker.SetTitle(pin.Pin.Label);
        //        marker.SetSnippet(pin.Pin.Address);
        //        //                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));

        //        map.AddMarker(marker);
        //    }
        //}

//        public new void OnMapReady(GoogleMap googleMap)
//        {
////           
//
//            map = googleMap;
//            map.SetOnMarkerClickListener(this);
//            map.MyLocationEnabled = true;
//            //                    map.SetOnCameraChangeListener(this);
//        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Console.WriteLine(e.PropertyName);
            map = NativeMap;

            if (map != null)
            {
                map.SetOnMarkerClickListener(this);
            }

            if ((e.PropertyName.Equals("VisibleRegion") || e.PropertyName.Contains("Pin")) && !isDrawn && map != null)
            {
                map.Clear();

                foreach (var pin in (sender as CustomMap).CustomPins)
                {
                    var marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
                    marker.SetTitle(pin.Pin.Label);
                    marker.SetSnippet(pin.Pin.Address);
                    //                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));

                    map.AddMarker(marker);

                }
               // isDrawn = true;
            }
        }

        //        public bool OnMarkerClick(Marker marker)
        //        {
        //
        //
        //            return true;
        //        }
        public bool OnMarkerClick(Marker marker)
        {
            throw new NotImplementedException();
        }
    }
}