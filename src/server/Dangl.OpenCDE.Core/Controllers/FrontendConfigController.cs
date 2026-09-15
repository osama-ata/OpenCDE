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
                // Dangl-IT's own hosted identity server and is out of scope for this
                // Supabase-authenticated deployment -- left blank rather than wired to
                // a provider that can't issue a token this server would accept.
                DanglIdentityClientId = string.Empty,
                DanglIdentityUrl = string.Empty,
                DanglIconsBaseUrl = string.Empty,
                ApplicationInsightsInstrumentationKey = _settings.ApplicationInsightsInstrumentationKey,
                Environment = _environment.EnvironmentName,
                RequiredScope = "authenticated"
            };
        }
    }
}
