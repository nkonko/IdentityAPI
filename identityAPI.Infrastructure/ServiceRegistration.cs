using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using identityAPI.Infrastructure.Services;

namespace identityAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<Services.UserService>()
                .AddClasses(classes => classes.InNamespaceOf<Services.UserService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
            // Registrar AuthService
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
