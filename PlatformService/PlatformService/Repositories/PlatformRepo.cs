using Microsoft.EntityFrameworkCore;
using PlatformService.DataContext;
using PlatformService.Models;
using PlatformService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Repositories
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _db;
        public PlatformRepo(AppDbContext db)
        {
            _db = db;
        }
        public async Task CreatePlatformAsync(Platform platform) => await _db.Platforms.AddAsync(platform);
        public async Task<IEnumerable<Platform>> GetAllPlatformsAsync() => await _db.Platforms?.ToListAsync();
        public async Task<Platform> GetPlatformByIdAsync(int id) => await _db.Platforms?.SingleOrDefaultAsync(x => x.Id == id);
        public async Task<bool> SaveChangesAsync() => await _db?.SaveChangesAsync() >= 0;

    }
}
