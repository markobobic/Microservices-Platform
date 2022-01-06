using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Repositories.Interfaces;
using PlatformService.SyncDataServices.Http.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private const string Platform_Published = "Platform_Published";
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo platformRepo,
                                   IMapper mapper,
                                   ICommandDataClient commandDataClient,
                                   IMessageBusClient messageBusClient)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<IActionResult> GetPlatformById(int id) =>
            _mapper.Map<PlatformReadDTO>(await _platformRepo.GetPlatformByIdAsync(id)) == null ? NotFound()
            : Ok(_mapper.Map<PlatformReadDTO>(await _platformRepo.GetPlatformByIdAsync(id)));


        [HttpGet]
        public async Task<IActionResult> GetPlatforms() =>
            Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(await _platformRepo.GetAllPlatformsAsync()));

        [HttpPost("createPlatform")]
        public async Task<IActionResult> CreatePlatform(PlatformCreateDTO platformDto)
        {
            var platform = _mapper.Map<Platform>(platformDto);
            await _platformRepo.CreatePlatformAsync(platform);
            await _platformRepo.SaveChangesAsync();
            var platformRead = _mapper.Map<PlatformReadDTO>(platform);
            try
            {
                await _commandDataClient.SendPlatformToCommandAsync(platformRead);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not send sync: " + e.Message);
            }
            try
            {
                var platformPublishDTO = _mapper.Map<PlatformPublishedDTO>(platformRead);
                platformPublishDTO.Event = Platform_Published;
                await _messageBusClient.PublishNewPlatformAsync(platformPublishDTO);
            }
            catch (Exception e)
            {

                Console.WriteLine("Could not send async: " + e.Message);
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { platformRead.Id }, platformRead);
        }

    }
}
