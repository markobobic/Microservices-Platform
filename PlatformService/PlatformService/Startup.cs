using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PlatformService.DataContext;
using PlatformService.Repositories;
using PlatformService.Repositories.Interfaces;
using PlatformService.SyncDataServices.Http;
using PlatformService.SyncDataServices.Http.Interfaces;
using System;

namespace PlatformService
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("===> Using Sql Server");
                services.AddDbContext<AppDbContext>(opt =>
                {
                    opt.UseSqlServer(Configuration.GetConnectionString("PlatformsConn"));
                });
            }
            else
            {
                services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));
            }
            services.AddScoped<IPlatformRepo,PlatformRepo>();
            services.AddHttpClient<ICommandDataClient, CommandDataClient>();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            PrepDb.PrepPopulation(app, _env.IsProduction());
        }
    }
}
