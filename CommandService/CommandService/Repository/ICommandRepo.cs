using CommandService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandService.Repository
{
    public interface ICommandRepo : IPlatformRepo
    {
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId);
        Task<Command> GetCommand(int platformId, int commandId);
        Task CreateCommand(int platformId, Command command);

    }
}
