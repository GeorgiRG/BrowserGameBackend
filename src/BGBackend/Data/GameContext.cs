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
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<UserSkills> UserSkills { get; set; }
        public DbSet<StarSystem> StarSystems { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Bot> Bots { get; set; }

        public GameContext (IConfiguration config) 
        {
            _config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(_config["ConnectionStrings:DbConnectionLocal"]);


    }
}
    