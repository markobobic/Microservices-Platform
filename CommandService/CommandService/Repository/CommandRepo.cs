using CommandService.Data;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandService.Repository
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;
        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateCommandAsync(int platformId, Command command)
        {
            if (command == null) return;
            command.PlatformId = platformId;
            _context.Entry<Command>(command).DetectChanges();
            await _context.Commands.AddAsync(command);
        }

        public async Task CreatePlatformAsync(Platform platform)
        {
            if (platform == null) return;
            await _context.AddAsync(platform);
        }

        public async Task<bool> ExternalPlatformExistsAsync(int externalPlatformId) =>
            await _context.Platforms.AnyAsync(x => x.Id == externalPlatformId);

        public async Task<IEnumerable<Platform>> GetAllPlatformsAsync() => 
            await _context.Platforms.ToListAsync();

        public async Task<Command> GetCommandAsync(int platformId, int commandId) => 
            await _context.Commands.SingleOrDefaultAsync(x => x.Id == commandId
            && x.PlatformId == platformId);

        public async Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId)
        {
            var platform = await _context.Platforms
                .SingleOrDefaultAsync(x => x.Id == platformId);
            if(platform == null) return new List<Command>();
            return platform.Commands;
        }

        public async Task<bool> PlatformExistsAsync(int platformId)
        {
            return await _context.Platforms.AnyAsync(x => x.Id == platformId);
        }

        public async Task<bool> SaveChangesAsync()
        {
           var result = await _context.SaveChangesAsync();
            var result1 = await _context.Commands.ToListAsync();
           return result >= 0; 
        }
    }
}
