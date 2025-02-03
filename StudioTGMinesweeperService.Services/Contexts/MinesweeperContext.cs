using Microsoft.EntityFrameworkCore;
using StudioTGMinesweeperService.Models.DbModels;

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
    }
}
