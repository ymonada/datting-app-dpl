using WebSocket.Domain.Common;

namespace WebSocket.Domain.UserAggregate;

public class Location : ValueObject
{
    public string Country { get; init; } = string.Empty;
    public string CityOrRegion { get; init; } = string.Empty;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return CityOrRegion;
    }
    public static Location CreateEmpty() => new  Location()
    {
        Country = string.Empty,
        CityOrRegion = string.Empty
    };
};