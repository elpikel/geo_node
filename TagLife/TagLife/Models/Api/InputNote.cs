using Newtonsoft.Json;

namespace TagLife.Models.Api
{
    public class InputNote
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }
    }
}