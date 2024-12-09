using DataAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DataAPI.Services.MigrationServices
{
    public class MigrationService
    {
        public static void InitializeMigration(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            serviceScope.ServiceProvider.GetService<DotnetCurrencyProjectContext>()!.Database.Migrate();
        }
    }
}
