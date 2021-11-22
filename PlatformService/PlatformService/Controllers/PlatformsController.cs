using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Repositories.Interfaces;
using System.Collections.Generic;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        public PlatformsController(IPlatformRepo platformRepo, IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public IActionResult GetPlatformById(int id) =>
            _mapper.Map<PlatformReadDTO>(_platformRepo.GetPlatformById(id)) == null ? NotFound()
            : Ok(_mapper.Map<PlatformReadDTO>(_platformRepo.GetPlatformById(id)));
        

        [HttpGet]
        public IActionResult GetPlatforms() =>
            Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(_platformRepo.GetAllPlatforms()));

        [HttpPost("createPlatform")]
        public IActionResult CreatePlatform(PlatformCreateDTO platformDto)
        {
            var platform = _mapper.Map<Platform>(platformDto);
            _platformRepo.CreatePlatform(platform);
            _platformRepo.SaveChanges();
            var platformRead = _mapper.Map<PlatformReadDTO>(platform);
            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformRead.Id }, platformRead);
        }

    }
}
