using Microsoft.AspNetCore.Localization;
using System.Globalization;


namespace Hemisferio.Client.Shared.Extensions
{
    public static class AppLocalization
    {
        public static RequestCulture DefaultCulture { get => new(""); }
        public static CultureInfo[] SupportedCultures { get => GetSupportedCultures(); }

        public static CultureInfo[] PredefinedCultures()
        {
            return new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("en-GB"),
                new CultureInfo("en-CA"),
                new CultureInfo("es-ES"),
                new CultureInfo("es-AR"),
                new CultureInfo("es-MX")
            };
        }

        private static CultureInfo[] GetSupportedCultures()
        {
            var cultures = new List<CultureInfo>();

            var parentCultures = new List<CultureInfo>
            {
                    new CultureInfo("en"),
                    new CultureInfo("es")
            };

            foreach (var culture in PredefinedCultures())
            {
                cultures.Add(culture);
            }

            foreach (var parentCulture in parentCultures)
            {
                cultures.Add(parentCulture);

                var specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                    .Where(c => c.Parent.Name == parentCulture.Name && cultures.All(cults => cults.Name != c.Name));

                foreach (var culture in specificCultures)
                {
                    cultures.Add(culture);
                }
            }

            return cultures.ToArray();
        }
    }
}
