using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Data.DbInitializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Extensions
{
    public static class DatabaseInitializeExtension
    {
        public static IWebHost InitializeDatabase(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<MSMContext>();
                    DbInitializer.Seed(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogCritical($"{ex}, An Error Occured while seeding database");
                }
            }
            return host;
        }
    }
}
