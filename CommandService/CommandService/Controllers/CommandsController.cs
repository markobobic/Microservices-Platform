using AutoMapper;
using CommandService.DTO;
using CommandService.Models;
using CommandService.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetCommandsForPlatform(int platformId)
        {
            if (!await _repo.PlatformExistsAsync(platformId)) return NotFound();
            return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>
                (await _repo.GetCommandsForPlatformAsync(platformId)));
        }
        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<IActionResult> GetCommandForPlatform(int platformId, int commandId)
        {
            if (!await _repo.PlatformExistsAsync(platformId)) return NotFound();
            return Ok(_mapper.Map<CommandReadDTO>
                (await _repo.GetCommandAsync(platformId, commandId)));
        }
        [HttpPost]
        public async Task<IActionResult> CreateCommand(int platformId, CommandCreateDTO commandDTO)
        {
            if (!await _repo.PlatformExistsAsync(platformId)) return NotFound();
            
            var command = _mapper.Map<Command>(commandDTO);

            await _repo.CreateCommandAsync(platformId, command);
            await _repo.SaveChangesAsync();
            var commandReadDto = _mapper.Map<CommandReadDTO>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
    }
}
