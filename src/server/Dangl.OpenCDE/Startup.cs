using Dangl.AspNetCore.FileHandling;
using Dangl.OpenCDE.Core.Configuration;
using Dangl.OpenCDE.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dangl.OpenCDE
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var openCdeSettings = Configuration.Get<OpenCdeSettings>();
            if (openCdeSettings.StorageSettings == null)
            {
                openCdeSettings.StorageSettings = new StorageSettings();
            }

            var sqliteConnectionString = Configuration.GetConnectionString("Sqlite");
            openCdeSettings.Validate();
            services.AddOpenCdeServices(openCdeSettings);

            if (!string.IsNullOrWhiteSpace(openCdeSettings.ApplicationInsightsInstrumentationKey))
            {
                services.AddApplicationInsightsTelemetry();
            }

            services.AddDbContext<CdeDbContext>(sqlBuilder =>
                sqlBuilder.UseSqlite(sqliteConnectionString, options => options.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name)));

            services.AddDiskFileManager(openCdeSettings.StorageSettings.LocalDiskBasePath);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureOpenCdeApp(env);
        }
    }
}
