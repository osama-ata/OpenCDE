using Dangl.OpenCDE.Core.Configuration;
using Dangl.OpenCDE.Shared.Models.Foundations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Dangl.OpenCDE.Core.Controllers.Foundations
{
    [Route("foundation/1.0/auth")]
    [AllowAnonymous]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly OpenCdeSettings _settings;

        public AuthenticationController(OpenCdeSettings settings)
        {
            _settings = settings;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(AuthGet), (int)HttpStatusCode.OK)]
        public IActionResult GetAuthenticationMetadataAsync()
        {
            // Tokens come from Supabase Auth (see app/auth.py in BIM-Guard's own
            // backend for the matching verification side) rather than a Dangl Identity
            // OIDC redirect this server itself brokers, so these are static endpoints
            // instead of a fetched discovery document.
            var supabaseAuthBaseUrl = _settings.Supabase.Url.TrimEnd('/') + "/auth/v1";
            var authenticationMetadata = new AuthGet
            {
                OAuth2AuthUrl = $"{supabaseAuthBaseUrl}/authorize",
                OAuth2TokenUrl = $"{supabaseAuthBaseUrl}/token",
                OAuth2DynamicClientRegistrationUrl = null,
                HttpBasicSupported = false,
                SupportedOAuth2Flows = new List<string>
                {
                    "authorization_code_grant",
                    "password_grant"
                },
                OAuth2RequiredScopes = "authenticated"
            };

            return Ok(authenticationMetadata);
        }
    }
}
