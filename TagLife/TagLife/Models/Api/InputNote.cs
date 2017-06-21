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
        public float Longitude { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }
    }
}