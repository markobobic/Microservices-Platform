using PlatformService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Repositories.Interfaces
{
    public interface IPlatformRepo
    {
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Platform>> GetAllPlatformsAsync();
        Task<Platform> GetPlatformByIdAsync(int id);
        Task CreatePlatformAsync(Platform platform);
        
    }
}
