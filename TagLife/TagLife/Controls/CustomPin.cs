

namespace TagLife.Controls
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

        protected bool Equals(CustomPin other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CustomPin) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}