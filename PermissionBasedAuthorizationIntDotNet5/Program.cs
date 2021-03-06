using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationIntDotNet5
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerProvider>();
            var logger = loggerFactory.CreateLogger("app");

            try
            {
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await Seeds.DefaultRoles.SeedAsync(roleManager);
                await Seeds.DefaultUsers.SeedBasicUserAsync(userManager);
                await Seeds.DefaultUsers.SeedSuperAdminUserAsync(userManager, roleManager);

                logger.LogInformation("Data seeded");
                logger.LogInformation("Application Started");
            }
            catch (System.Exception ex)
            {
                logger.LogWarning(ex, "An error occurred while seeding data");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
