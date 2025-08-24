using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using identityAPI.Core.Entities;
using identityAPI.Infrastructure.Persistence;
using Npgsql;

namespace identityAPI.Infrastructure
{
    public static class SeedRoles
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, int maxRetries = 10, int delaySeconds = 5)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Espera a que la base de datos esté lista con retry
            int retries = 0;
            while (true)
            {
                try
                {
                    await context.Database.OpenConnectionAsync();
                    await context.Database.CloseConnectionAsync();
                    break;
                }
                catch (NpgsqlException)
                {
                    retries++;
                    if (retries >= maxRetries)
                        throw;
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }

            // Aplica migraciones
            await context.Database.MigrateAsync();

            // Seed de roles
            string[] roles = new[] { "User", "Admin" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
