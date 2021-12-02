using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo platformRepo,
                                   IMapper mapper,
                                   ICommandDataClient commandDataClient)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public IActionResult GetPlatformById(int id) =>
            _mapper.Map<PlatformReadDTO>(_platformRepo.GetPlatformById(id)) == null ? NotFound()
            : Ok(_mapper.Map<PlatformReadDTO>(_platformRepo.GetPlatformById(id)));


        [HttpGet]
        public IActionResult GetPlatforms() =>
            Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(_platformRepo.GetAllPlatforms()));

        [HttpPost("createPlatform")]
        public async Task<IActionResult> CreatePlatform(PlatformCreateDTO platformDto)
        {
            var platform = _mapper.Map<Platform>(platformDto);
            _platformRepo.CreatePlatform(platform);
            _platformRepo.SaveChanges();
            var platformRead = _mapper.Map<PlatformReadDTO>(platform);
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformRead);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformRead.Id }, platformRead);
        }

    }
}
