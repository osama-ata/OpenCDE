using Dangl.OpenCDE.Shared.Configuration;
using System;

namespace Dangl.OpenCDE.Core.Configuration
{
    /// <summary>
    /// Replaces the previous DanglIdentitySettings: requests are authenticated by
    /// verifying a bearer JWT issued by a Supabase Auth project against its published
    /// JWKS, the same way BIM-Guard's own backend verifies it (see app/auth.py).
    /// </summary>
    public class SupabaseAuthSettings
    {
        /// <summary>
        /// The Supabase project's base URL, e.g. https://xxxxx.supabase.co
        /// </summary>
        public string Url { get; set; }

        public string JwksUrl => $"{Url.TrimEnd('/')}/auth/v1/.well-known/jwks.json";

        public string Issuer => $"{Url.TrimEnd('/')}/auth/v1";

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new InvalidConfigurationException($"{nameof(Url)} missing.");
            }

            if (!Uri.TryCreate(Url, UriKind.Absolute, out _))
            {
                throw new InvalidConfigurationException($"{nameof(Url)} must be an absolute url.");
            }
        }
    }
}
