using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.DataContext
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
        }

        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                try
                {
                    context.Database.Migrate();

                }
                catch (Exception ex)
                {

                    Console.WriteLine("MIGRATE ERROR:"  + ex.Message);
                }
            }
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
