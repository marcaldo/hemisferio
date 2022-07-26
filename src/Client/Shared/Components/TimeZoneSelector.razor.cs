using Hemisferio.Client.Shared.DateTime;
using Hemisferio.Client.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using NodaTime;
using System.Globalization;

namespace Hemisferio.Client.Shared.Components
{
    public partial class TimeZoneSelector : ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ITimeZones TimeZone { get; set; }

        [Parameter] public string UserId { get; set; }
        [Parameter] public bool ShowContinueSetup { get; set; }
        public CultureInfo[] Languages { get => GetSupportedLanguages(); }
        protected string SelectedCultureName { get; set; }

        protected CultureInfo[] CultureList { get; set; }
        protected IDictionary<string, string> Countries { get; set; } = new Dictionary<string, string>();
        protected IDictionary<string, string> TimeZones { get; set; } = new Dictionary<string, string>();
        protected LocalDateTime LocalDateTime { get; set; }
        string userCountryCode = "AR";
        string userCulture = "es-AR";
        string userTimeZoneId = "";

        protected override async Task OnInitializedAsync()
        {
            await InitializeValues();
        }

        private async Task InitializeValues()
        {
            CultureList = GetCultures(userCulture);
            Countries = TimeZone.GetCountriesByCulture(userCulture);
            bool saveUserConfiguration = false;

            if (userCountryCode == null)
            {
                string countryCode = null;
                KeyValuePair<string, string> country = new();

                if (userCulture.Length > 2)
                {
                    countryCode = userCulture[3..];
                    country = Countries.SingleOrDefault(c => c.Key.Equals(countryCode, StringComparison.InvariantCultureIgnoreCase));
                }
                else if (userCulture.Length == 2)
                {
                    var userCultureNativeName = CultureInfo.CreateSpecificCulture(userCulture).NativeName;
                    country = Countries.SingleOrDefault(c => userCultureNativeName.ToLowerInvariant().Contains(c.Value.ToLowerInvariant()));
                }

                if (country.Value != null)
                {
                    userCountryCode = country.Key;
                    saveUserConfiguration = true;
                }
            };

            TimeZones = TimeZone.GetTimeZonesForCountry(userCountryCode, userCulture);

            if (saveUserConfiguration)
            {
                var defaultTimezone = TimeZones.FirstOrDefault();
                userTimeZoneId = defaultTimezone.Key;

                //await SaveUserConfiguration();
            }
        }

        protected void SetLanguage(ChangeEventArgs e)
        {
            CultureList = GetCultures(e.Value.ToString());
            //SaveCulture(e.Value.ToString());
        }

        protected async Task SetTimeZone(ChangeEventArgs e)
        {
            userTimeZoneId = e.Value.ToString();

            //await SaveUserConfiguration();
        }

        protected void SetCulture(ChangeEventArgs e)
        {
            //SaveCulture(e.Value.ToString());
        }

        protected async Task SetCountry(ChangeEventArgs e)
        {
            userCountryCode = e.Value.ToString();
            TimeZones = TimeZone.GetTimeZonesForCountry(userCountryCode, userCulture);

            userTimeZoneId = TimeZones.FirstOrDefault().Key;

            //await SaveUserConfiguration();

        }

        protected CultureInfo[] GetSupportedLanguages()
        {
            return AppLocalization.SupportedCultures
                .Where(c => c.Name.Length == 2)
                .ToArray();
        }

        protected CultureInfo[] GetCultures(string cultureName)
        {
            var culture = CultureInfo.CreateSpecificCulture(cultureName);

            var cultures = AppLocalization.SupportedCultures
                .Where(c => c.Name.Length > 2 && c.Parent.Name == culture.Parent.Name)
                .ToArray();

            if (!cultures.Any())
            {
                SelectedCultureName = culture.NativeName;
                cultures = AppLocalization.PredefinedCultures();
                StateHasChanged();
            }

            return cultures;
        }

        //private async Task SaveUserConfiguration()
        //{
        //    await UserProfileManager.UpdateClaimAsync(User, CustomClaimTypes.TimeZone, userTimeZoneId);

        //    var workflowState = User.WorkflowState &= ~WorkflowStateType.TimeZoneSetupRequired;
        //    await UserProfileManager.UpdateClaimAsync(User, CustomClaimTypes.WorkflowState, ((int)workflowState).ToString());

        //    await UserProfileManager.UpdateUserAsync(User);
        //}

        //private void SaveCulture(string cultureName)
        //{
        //    _ = UserProfileManager.SetUserCultureAsync(UserId, cultureName);

        //    var backUrl = NavigationManager.Uri.Replace(NavigationManager.BaseUri, string.Empty);
        //    NavigationManager.NavigateTo(
        //        $"/UserSettings/SetCulture?culture={cultureName}&redirectUri=/{backUrl}",
        //        forceLoad: true);

        //}

    }
}
