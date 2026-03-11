using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using identityAPI.Infrastructure.Services;
using identityAPI.Infrastructure.Services.Interfaces;
using identityAPI.Infrastructure.Services.FileStorage;

namespace identityAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<UserService>()
                .AddClasses(classes => classes.InNamespaceOf<UserService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
            services.AddScoped<IAuthService, AuthService>();
        }

        public static void AddFileStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            var fileStorageSection = configuration.GetSection("FileStorage");
            services.Configure<FileStorageSettings>(fileStorageSection);

            var settings = fileStorageSection.Get<FileStorageSettings>() ?? new FileStorageSettings();

            switch (settings.Provider?.ToLowerInvariant())
            {
                case "cloudinary":
                    services.AddScoped<IFileStorageService, CloudinaryStorageService>();
                    break;
                case "local":
                default:
                    services.AddScoped<IFileStorageService, LocalFileStorageService>();
                    break;
            }
        }
    }
}
