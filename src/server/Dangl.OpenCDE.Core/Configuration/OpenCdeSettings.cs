using Dangl.OpenCDE.Shared.Configuration;
using System;

namespace Dangl.OpenCDE.Core.Configuration
{
    public class OpenCdeSettings
    {
        public SupabaseAuthSettings Supabase { get; set; }

        public StorageSettings StorageSettings { get; set; }

        public string ApplicationInsightsInstrumentationKey { get; set; }

        public string AppBaseUrl { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(AppBaseUrl))
            {
                throw new InvalidConfigurationException($"{nameof(AppBaseUrl)} missing.");
            }

            if (!AppBaseUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase)
                && !AppBaseUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidConfigurationException($"{nameof(AppBaseUrl)} must be an absolute uri and use http or https");
            }

            if (Supabase == null)
            {
                throw new InvalidConfigurationException($"{nameof(Supabase)} missing.");
            }

            Supabase?.Validate();
            if (StorageSettings == null)
            {
                throw new InvalidConfigurationException($"{nameof(StorageSettings)} missing.");
            }

            StorageSettings?.Validate();
        }
    }
}
