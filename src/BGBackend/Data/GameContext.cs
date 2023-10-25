using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BrowserGameBackend.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace BrowserGameBackend.Data
{
    public class GameContext : DbContext
    {
        private readonly IConfiguration _config;
        public DbSet<Bot> Bots { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<Fleet> Fleets { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Population> Populations { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceProduction> ResourcesProductions { get; set; }
        public DbSet<ResourceRequirements> ResourcesRequirements { get; set; }
        public DbSet<ResourceStorage> ResourcesStorages { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<StarSystem> StarSystems { get; set; }
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<UserSkills> UserSkills { get; set; }

        public GameContext (IConfiguration config) 
        {
            _config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(_config["ConnectionStrings:DbConnectionLocal"]);
    }
}
    