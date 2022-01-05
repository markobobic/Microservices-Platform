using CommandService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommandService.Repository
{
    public interface IPlatformRepo
    {
        Task<IEnumerable<Platform>> GetAllPlatforms();
        Task CreatePlatform(Platform platform);
        Task<bool> PlatformExists(int platformId);
    }
}
