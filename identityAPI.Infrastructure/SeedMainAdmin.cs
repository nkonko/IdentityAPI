using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using identityAPI.Core.Entities;

namespace identityAPI.Infrastructure
{
    public static class SeedMainAdmin
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminUserName = "admin";
            var adminEmail = "admin@demo.com";
            var adminPassword = "Admin123!"; // Cambia esto en producción

            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                // Puedes loggear errores si lo deseas
            }
        }
    }
}
