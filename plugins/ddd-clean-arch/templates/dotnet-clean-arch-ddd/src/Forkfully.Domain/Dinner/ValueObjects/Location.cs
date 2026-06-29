using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Dinner.ValueObjects;

public sealed class Location : ValueObject
{
    // Private setters + parameterless ctor so EF can materialize the owned value object.
    public string Name { get; private set; }
    public string Address { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    private Location(string name, string address, double latitude, double longitude)
    {
        Name = name;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

#pragma warning disable CS8618
    private Location()
    {
    }
#pragma warning restore CS8618

    public static Location Create(string name, string address, double latitude, double longitude)
    {
        return new Location(name, address, latitude, longitude);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Address;
        yield return Latitude;
        yield return Longitude;
    }
}
