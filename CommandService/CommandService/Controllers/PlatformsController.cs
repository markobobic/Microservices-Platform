using AutoMapper;
using CommandService.Models;
using CommandService.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatforms() => 
            Ok(_mapper.Map<IEnumerable<Platform>>(await _repo.GetAllPlatformsAsync()));

        [HttpPost]
        public IActionResult Test123()
        {
            Console.WriteLine("Test");
            return Ok("Test");
        }
        
    }
}
