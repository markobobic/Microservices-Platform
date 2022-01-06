using AutoMapper;
using CommandService.DTO;
using CommandService.Enums;
using CommandService.Models;
using CommandService.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommandService.EventsProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public async Task ProcessEvent(string message)
        {
            var eventType = await DetermineEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    await AddPlatform(message);
                    break;
                default:
                    break;
            }
        }
        private async Task<EventType> DetermineEvent(string notificationMessage)
        {
            Debug.WriteLine("---> Determing event");
            return await Task.Run(() =>
            {
                var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);
                switch (eventType.Event)
                {
                    case "Platform_Published":
                        Debug.WriteLine("Platform published event detected!");
                        return EventType.PlatformPublished;
                    default:
                        Debug.WriteLine("Undetermined event detected!");
                        return EventType.Undetermined;
                }
            });
            
        }
        
        private async Task AddPlatform(string platformPublishedMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var platformPublishedDTO = JsonSerializer.Deserialize<PlatformPublishedDTO>(platformPublishedMessage);
            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDTO);
                if (!await repo.ExternalPlatformExistsAsync(platform.ExternalId))
                {
                   await repo.CreatePlatformAsync(platform);
                   await repo.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine($"Could not add platform to db {ex.Message}");
            }
        }
    }
}
