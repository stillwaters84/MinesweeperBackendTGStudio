using Microsoft.EntityFrameworkCore;
using StudioTGMinesweeperService.Contexts;
using StudioTGMinesweeperService.Interfaces;
using StudioTGMinesweeperService.Models.DbModels;

namespace StudioTGMinesweeperService.Repositories
{
    public class GameTurnRepository : IGameTurnRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<GameTurnModel> _dbSet;

        public GameTurnRepository(MinesweeperContext minesweeperContext)
        {
            _dbContext = minesweeperContext;
            _dbSet = _dbContext.Set<GameTurnModel>();
        }

        public async Task<List<GameTurnModel>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<GameTurnModel?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(GameTurnModel updatedClient)
        {
            _dbSet.Update(updatedClient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Create(GameTurnModel client)
        {
            _dbSet.AddAsync(client);
            await _dbContext.SaveChangesAsync();
        }
    }
}
