using Dangl.OpenCDE.Core.Configuration;
using Dangl.OpenCDE.Shared;
using Dangl.OpenCDE.Shared.Models.Controllers.FrontendConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;

namespace Dangl.OpenCDE.Core.Controllers
{
    [Route("api/frontend-config")]
    public class FrontendConfigController : CdeAppControllerBase
    {
        public FrontendConfigController(OpenCdeSettings settings,
            IWebHostEnvironment environment)
        {
            _settings = settings;
            _environment = environment;
        }

        private static string _frontendConfig;
        private readonly OpenCdeSettings _settings;
        private readonly IWebHostEnvironment _environment;

        [HttpGet("")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FrontendConfigGet), (int)HttpStatusCode.OK)]
        public IActionResult GetFrontendConfig()
        {
            return Ok(GetFrontendConfigModel());
        }

        [HttpGet("config.js")]
        [AllowAnonymous]
        public IActionResult GetFrontendConfigScript([FromQuery] string timestamp)
        {
            if (!string.IsNullOrWhiteSpace(timestamp))
            {
                HttpContext.Response
                    .GetTypedHeaders()
                    .CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(365)
                    };
            }

            return GetContentResultForFrontendConfig();
        }

        private ContentResult GetContentResultForFrontendConfig()
        {
            if (_frontendConfig == null)
            {
                InitializeFrontendConfig();
            }

            return Content(_frontendConfig, "application/javascript");
        }

        private void InitializeFrontendConfig()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var frontendConfigObject = GetFrontendConfigModel();
            var serializedConfig = JsonConvert.SerializeObject(frontendConfigObject, serializerSettings);
            _frontendConfig = @"(function() {
window.danglOpenCdeFrontendConfig = "
+ serializedConfig
+ @";
})();";
        }

        private FrontendConfigGet GetFrontendConfigModel()
        {
            return new FrontendConfigGet
            {
                // The standalone "log into the CDE UI directly" screen these previously
                // fed (AuthenticationService.initiateOpenIdImplicitLogin) pointed at
                // Dangl-IT's own hosted identity server, which no longer issues tokens
                // this server accepts -- left blank permanently. The UI now signs
                // users in directly against Supabase Auth via SupabaseUrl/SupabaseAnonKey
                // below instead.
                DanglIdentityClientId = string.Empty,
                DanglIdentityUrl = string.Empty,
                DanglIconsBaseUrl = string.Empty,
                ApplicationInsightsInstrumentationKey = _settings.ApplicationInsightsInstrumentationKey,
                Environment = _environment.EnvironmentName,
                RequiredScope = "authenticated",
                // Empty when Supabase:AnonKey isn't configured, in which case the UI
                // disables its own login form (server-to-server bearer-token callers,
                // e.g. BIM-Guard's sync client, are unaffected either way).
                SupabaseUrl = _settings.Supabase?.Url ?? string.Empty,
                SupabaseAnonKey = _settings.Supabase?.AnonKey ?? string.Empty
            };
        }
    }
}
