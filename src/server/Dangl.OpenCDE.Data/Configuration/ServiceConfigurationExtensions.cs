using Dangl.OpenCDE.Data.IO;
using Dangl.OpenCDE.Data.Repository;
using Dangl.OpenCDE.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dangl.OpenCDE.Data.Configuration
{
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection AddOpenCdeDataServices(this IServiceCollection services)
        {
            // Repositories
            services.AddTransient<IProjectsRepository, ProjectsRepository>();
            services.AddTransient<IDocumentsRepository, DocumentsRepository>();
            services.AddTransient<IOpenCdeDocumentSelectionRepository, OpenCdeDocumentSelectionRepository>();

            // File Service
            services.AddTransient<ICdeAppFileHandler, CdeAppFileHandler>();

            // Identity of the caller, resolved from the verified Supabase JWT
            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
