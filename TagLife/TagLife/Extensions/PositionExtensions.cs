using Xamarin.Forms.Maps;

namespace TagLife.Extensions
{
    public static class PositionExtensions
    {
        public static Position ToXamarinPosition(this Plugin.Geolocator.Abstractions.Position position)
        {
            return new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);  
        }
    }
}