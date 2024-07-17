using Microsoft.EntityFrameworkCore;
using RPG;
using RPG.Entities;
using RPG.Entities.Database;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RPG.Data
{
    public class GameContext : DbContext
    {
        public DbSet<GameLog> GameLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=RPG;Integrated Security=True;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
