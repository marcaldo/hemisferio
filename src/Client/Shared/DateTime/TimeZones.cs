using NodaTime;
using TimeZoneNames;

namespace Hemisferio.Client.Shared.DateTime
{
    public sealed class TimeZones : ITimeZones
    {
        public IDictionary<string, string> GetCountriesByCulture(string cultureName)
        {
            var countries = TZNames.GetCountryNames(cultureName);
            return countries;
        }

        public IDictionary<string, string> GetTimeZonesForCountry(string countryKey, string cultureName)
        {
            var zones = TZNames.GetTimeZonesForCountry(countryKey, cultureName,
                new System.DateTime(System.DateTime.Now.Year, 1, 1));
            return zones;
        }

        public LocalDateTime GetLocalNodaDateTimeNow(string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
            {
                throw new ArgumentException("A timezone Id must be provided.");
            }

            // get the current utc time from the system clock
            Instant instantUtc = SystemClock.Instance.GetCurrentInstant();

            DateTimeZone dateTimeZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            var nowLocal = instantUtc.InZone(dateTimeZone).LocalDateTime;

            return nowLocal;
        }

        public DateTimeOffset GetLocalDateTimeNow(string timeZoneId)
        {
            if (string.IsNullOrEmpty(timeZoneId))
            {
                throw new ArgumentException("A timezone Id must be provided.");
            }

            Instant instantUtc = SystemClock.Instance.GetCurrentInstant();

            DateTimeZone dateTimeZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            var localNow = instantUtc.InZone(dateTimeZone).ToDateTimeOffset();

            return localNow;
        }
    }
}
