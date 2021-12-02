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
        public IConfiguration _configuration { get; }


        public CommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task SendPlatformToCommand(PlatformReadDTO platformReadDTO)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(platformReadDTO),
                                            Encoding.UTF8,
                                            "application/json");
            var response = await _httpClient.PostAsync(@"http://localhost:6000/api/c/platforms/",httpContent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync post to command service was okay!");
                await Task.CompletedTask;

            }
            Console.WriteLine("--> Sync post was not okay!");
        }
    }
}
