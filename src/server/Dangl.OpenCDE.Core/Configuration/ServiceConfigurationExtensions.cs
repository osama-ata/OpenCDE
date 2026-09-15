using AutoMapper;
using Dangl.Data.Shared.AspNetCore;
using Dangl.Data.Shared.AspNetCore.Validation;
using Dangl.Data.Shared.Json;
using Dangl.OpenCDE.Core.Filters;
using Dangl.OpenCDE.Core.Mapping;
using Dangl.OpenCDE.Data.Configuration;
using Dangl.OpenCDE.Data.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Dangl.OpenCDE.Core.Configuration
{
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection AddOpenCdeServices(this IServiceCollection services,
            OpenCdeSettings openCdeSettings)
        {
            openCdeSettings.Validate();
            services.AddTransient(_ => openCdeSettings);

            var jwksProvider = new SupabaseJwksProvider(openCdeSettings.Supabase.JwksUrl);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Keep raw claim names ("sub", "exp", "email") instead of ASP.NET's
                    // default ClaimTypes remapping -- controllers read them directly,
                    // the same shape BIM-Guard's own backend sees them in (app/auth.py).
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = openCdeSettings.Supabase.Issuer,
                        ValidateAudience = true,
                        ValidAudience = "authenticated",
                        ValidateLifetime = true,
                        IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                            jwksProvider.GetSigningKeys(kid)
                    };
                });
            services.AddAuthorization();

            services.AddControllers(mvcOptions =>
                {
                    mvcOptions.Filters.Add(typeof(ModelStateValidationFilter));
                    mvcOptions.Filters.Add(typeof(RequiredFormFileValidationFilter));
                    mvcOptions.Filters.Add(typeof(ApiErrorLoggingActionFilter));
                    mvcOptions.AddEmptyFormFileValidator();
                })
                // To ensure it's using it's own assembly in addition to the startup one
                .AddApplicationPart(typeof(ServiceConfigurationExtensions).Assembly)
                .AddNewtonsoftJson(jsonOptions =>
                {
                    jsonOptions.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    jsonOptions.SerializerSettings.ConfigureDefaultJsonSerializerSettings(true);
                });

            services.AddOpenCdeDataServices();

            services.AddResponseCompression(o => o.EnableForHttps = true);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);

            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);

            services.AddAutoMapper(c => c.AddEnumCompatibilityValidator(),
                typeof(CdeAutoMapperProfile),
                typeof(CdeDataAutoMapperProfile));

            services.AddOpenCdeSwaggerServices(openCdeSettings);

            services.AddHsts(o => o.MaxAge = TimeSpan.FromDays(365));

            // Forwarded headers are important when running e.g. in Azure App Service on Docker, since that will terminate
            // https and otherwise the app would be stuck in an infinite redirect loop, attempting to redirect all requests
            // to Https since it would assume that requests are incoming via plain http
            services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.All);

            services.AddHttpClient();

            return services;
        }
    }
}
