using NodaTime;

namespace Hemisferio.Client.Shared.DateTime
{
    public interface ITimeZones
    {
        IDictionary<string, string> GetCountriesByCulture(string cultureName);
        DateTimeOffset GetLocalDateTimeNow(string timeZoneId);
        LocalDateTime GetLocalNodaDateTimeNow(string timeZoneId);
        IDictionary<string, string> GetTimeZonesForCountry(string countryKey, string cultureName);
    }
}
