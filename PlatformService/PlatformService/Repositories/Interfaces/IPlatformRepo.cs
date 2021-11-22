using PlatformService.Models;
using System.Collections.Generic;

namespace PlatformService.Repositories.Interfaces
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform platform);
        
    }
}
