using Microsoft.Extensions.Configuration;
using PlatformService.DTOs;
using PlatformService.SyncDataServices.Http.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        public IConfiguration Configuration { get; }


        public CommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            Configuration = configuration;
        }


        public async Task SendPlatformToCommandAsync(PlatformReadDTO platformReadDTO)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(platformReadDTO),
                                            Encoding.UTF8,
                                            "application/json");
            var response = await _httpClient.PostAsync(Configuration["CommandServiceAPI"], httpContent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync post to command service was okay!");
                await Task.CompletedTask;

            }
            Console.WriteLine("--> Sync post was not okay!");
        }
    }
}
