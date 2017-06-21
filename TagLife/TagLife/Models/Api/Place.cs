using Plugin.Geolocator.Abstractions;
using TagLife.Controls;

namespace TagLife.Models.Api
{
    public class Place
    {
        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }
        public string Username { get; set; }

        public CustomPin ToCustomPin()
        {
            return new CustomPin(
                position: new Position()
                {
                    Latitude = Latitude,
                    Longitude = Longitude,
                },
                id: Id,
                text: Description);
        }
    }
}