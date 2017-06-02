using Android.Gms.Maps.Model;
using Android.Views;
using Android.Widget;

namespace TagLife.Droid.Services
{
    public class MarkerCreator
    {
        public MarkerOptions CreateMarker(CustomPin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            // todo: verify if this is a fine way of saving additional data
            marker.SetSnippet(pin.Id);

            var inflater = Android.App.Application.Context.GetSystemService("layout_inflater") as Android.Views.LayoutInflater;


            var control = inflater.Inflate(Resource.Layout.Pinlayout, null);

            var textControl = control.FindViewById<TextView>(Resource.Id.jols);

            var userComment = pin.Text;

            ShrinkTextViewWhenCommentIsShort(userComment, textControl);
            SetTextAndShortenItWhenTooLong(userComment, textControl);

            control.Measure(View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
                View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));

            control.Layout(0, 0, control.MeasuredWidth, control.MeasuredHeight);

            // todo: optimize memory usage
            control.DrawingCacheEnabled = true;
            control.BuildDrawingCache(true);
            var drawingCache = control.GetDrawingCache(true);


            // ExportBitmapAsPNG(drawingCache);
            marker.SetIcon(BitmapDescriptorFactory.FromBitmap(drawingCache));
            return marker;
        }

        private static void SetTextAndShortenItWhenTooLong(string userComment, TextView textControl)
        {
            string toBeWritten = userComment;
            const int numberOfLetters = 50;
            if (userComment.Length > numberOfLetters)
                toBeWritten = userComment.Substring(0, numberOfLetters) + "...";

            textControl.Text = toBeWritten;
        }

        private static void ShrinkTextViewWhenCommentIsShort(string userComment, TextView textControl)
        {
            if (userComment.Length <= 19)
            {
                var layoutParameters = textControl.LayoutParameters;
                layoutParameters.Width = ViewGroup.LayoutParams.WrapContent;
                textControl.LayoutParameters = layoutParameters;
            }
        }
    }
}