using CommandService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandService.Repository
{
    public interface IPlatformRepo
    {
        Task<IEnumerable<Platform>> GetAllPlatformsAsync();
        Task CreatePlatformAsync(Platform platform);
        Task<bool> PlatformExistsAsync(int platformId);
    }
}
