using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Dangl.OpenCDE.Core.Configuration
{
    /// <summary>
    /// Fetches and caches a Supabase project's JWKS for validating bearer tokens, the
    /// same JWKS-backed verification BIM-Guard's own backend performs via PyJWKClient
    /// (see app/auth.py). Used as a JwtBearerOptions.TokenValidationParameters
    /// IssuerSigningKeyResolver, which is a synchronous callback, so the fetch below
    /// blocks -- acceptable here since results are cached for CacheDuration.
    /// </summary>
    public class SupabaseJwksProvider
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

        private readonly string _jwksUrl;
        private readonly object _lock = new object();
        private IList<SecurityKey> _cachedKeys;
        private DateTimeOffset _cachedAtUtc;

        public SupabaseJwksProvider(string jwksUrl)
        {
            _jwksUrl = jwksUrl;
        }

        public IEnumerable<SecurityKey> GetSigningKeys(string kid)
        {
            EnsureKeysLoaded();
            return _cachedKeys;
        }

        private void EnsureKeysLoaded()
        {
            if (_cachedKeys != null && DateTimeOffset.UtcNow - _cachedAtUtc < CacheDuration)
            {
                return;
            }

            lock (_lock)
            {
                if (_cachedKeys != null && DateTimeOffset.UtcNow - _cachedAtUtc < CacheDuration)
                {
                    return;
                }

                var json = _httpClient.GetStringAsync(_jwksUrl).GetAwaiter().GetResult();
                var jwks = new JsonWebKeySet(json);
                _cachedKeys = jwks.Keys.Cast<SecurityKey>().ToList();
                _cachedAtUtc = DateTimeOffset.UtcNow;
            }
        }
    }
}
