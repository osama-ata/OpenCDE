using Dangl.Data.Shared.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Dangl.OpenCDE.Core.Configuration
{
    public static class AppConfigurationExtensions
    {
        public static IApplicationBuilder ConfigureOpenCdeApp(this IApplicationBuilder app,
            IWebHostEnvironment environment)
        {
            app.UseForwardedHeaders();

            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials()
                .WithExposedHeaders("*"));

            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseOpenCdeVersionHeader();

            app.UseStaticFiles();

            app.UseOpenCdeSwaggerUi();

            app.UseHttpHeadToGetTransform();

            app.UseClientCompressionSupport();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                if (environment.IsDevelopment())
                {
                    spa.Options.SourcePath = "../dangl-opencde-ui";
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
                else
                {
                    spa.Options.DefaultPage = "/dist/browser/index.html";
                }
            });

            return app;
        }
    }
}
