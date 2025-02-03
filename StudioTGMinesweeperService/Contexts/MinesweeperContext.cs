using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudioTGMinesweeperService.Models;
using StudioTGMinesweeperService.Models.DbModels;
using System.Collections;
using System.Reflection.Metadata;

namespace StudioTGMinesweeperService.Contexts
{
    public class MinesweeperContext : DbContext
    {
        public DbSet<NewGameModel> NewGame { get; set; }

        public DbSet<GameTurnModel> GameTurn { get; set; }

        public MinesweeperContext()
        {
            Database.EnsureCreated();
        }
        public MinesweeperContext(DbContextOptions<MinesweeperContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewGameModel>().Property(b => b.field)
                .Metadata
                .SetValueComparer(new ValueComparer<Field[,]>(
                    (a, b) => StructuralComparisons.StructuralEqualityComparer.Equals(a, b),
                    a => a.GetHashCode()));

            modelBuilder.Entity<GameTurnModel>().Property(b => b.field)
                .Metadata
                .SetValueComparer(new ValueComparer<Field[,]>(
                    (a, b) => StructuralComparisons.StructuralEqualityComparer.Equals(a, b),
                    a => a.GetHashCode()));
        }*/
    }
}
