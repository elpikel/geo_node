

namespace TagLife
{
    public class CustomPin
    {
        public CustomPin(Plugin.Geolocator.Abstractions.Position position, string id, string text)
        {
            Position = position;
            Id = id;
            Text = text;
        }

        public Plugin.Geolocator.Abstractions.Position Position { get; }

        public string Id { get; }

        public string Text { get; }
    }
}