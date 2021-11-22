using PlatformService.Data;
using PlatformService.Models;
using PlatformService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.Repositories
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _db;
        public PlatformRepo(AppDbContext db)
        {
            _db = db;
        }
        public void CreatePlatform(Platform platform) => _db.Platforms?.Add(platform ?? throw new ArgumentException());
        public IEnumerable<Platform> GetAllPlatforms() => _db.Platforms?.ToList();
        public Platform GetPlatformById(int id) => _db.Platforms?.SingleOrDefault(x => x.Id == id);
        public bool SaveChanges() => _db?.SaveChanges() >= 0;

    }
}
