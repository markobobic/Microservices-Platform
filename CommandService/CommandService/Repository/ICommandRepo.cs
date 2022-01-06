using CommandService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandService.Repository
{
    public interface ICommandRepo : IPlatformRepo
    {
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId);
        Task<Command> GetCommandAsync(int platformId, int commandId);
        Task CreateCommandAsync(int platformId, Command command);

    }
}
