using Microsoft.Extensions.DependencyInjection;
using identityAPI.Infrastructure.Services;
using identityAPI.Infrastructure.Services.Interfaces;

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
    }
}
