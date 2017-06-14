using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using TagLife.Controls;

namespace TagLife.Services
{
    public class PlacesService
    {
        private static HttpClient _httpClient;

        public PlacesService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Place>> GetPlaces()
        {
            var places = await _httpClient.GetAsync(@"https://young-thicket-35712.herokuapp.com/places");

            // todo: one day change it to throw correct exception
            if (!places.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            var readAsStringAsync = await places.Content.ReadAsStringAsync();

            // todo: serialization exception
            return JsonConvert.DeserializeObject<List<Place>>(readAsStringAsync);
        }
    }

    public class Place
    {
        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

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