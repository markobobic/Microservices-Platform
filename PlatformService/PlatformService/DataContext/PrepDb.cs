using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.DataContext
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                context.Platforms.AddRange(new List<Platform>()
                {
                    new Platform() { Cost = "Free", Name = "Dot Net", Publisher = "Microsoft"},
                    new Platform() { Cost = "Free", Name = "Sql Server", Publisher = "Microsoft"},
                    new Platform() { Cost = "Free", Name = "Github", Publisher = "Microsoft"},
                });

                context.SaveChanges();
            }
        }
    }
}
