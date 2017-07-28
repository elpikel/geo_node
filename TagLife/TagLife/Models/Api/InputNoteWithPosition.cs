using Newtonsoft.Json;

namespace TagLife.Models.Api
{
    public class InputNoteWithPosition
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("place_id")]
        public int Place { get; set; }
    }
}