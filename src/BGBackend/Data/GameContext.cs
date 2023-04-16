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
        public DbSet<Base> Bases { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        public GameContext (IConfiguration config) 
        {
            _config = config;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql(_config["ConnectionStrings:DbConnectionLocal"]);


    }
}
    