using System.ComponentModel.DataAnnotations;

namespace Dangl.OpenCDE.Shared.Models.Controllers.FrontendConfig
{
    public class FrontendConfigGet
    {
        [Required]
        public string DanglIconsBaseUrl { get; set; }

        [Required]
        public string DanglIdentityUrl { get; set; }

        public string ApplicationInsightsInstrumentationKey { get; set; }

        [Required]
        public string Environment { get; set; }

        [Required]
        public string DanglIdentityClientId { get; set; }

        [Required]
        public string RequiredScope { get; set; }

        /// <summary>
        /// The Supabase project's base URL, used by the UI to sign users in directly
        /// against Supabase Auth. Empty when no <c>Supabase:AnonKey</c> is configured.
        /// </summary>
        public string SupabaseUrl { get; set; }

        /// <summary>
        /// The Supabase project's publishable/anon key, safe to expose to the browser.
        /// Empty when no <c>Supabase:AnonKey</c> is configured, in which case the UI's
        /// login screen is disabled.
        /// </summary>
        public string SupabaseAnonKey { get; set; }
    }
}
