using Microsoft.EntityFrameworkCore;
using StudioTGMinesweeperService.Contexts;
using StudioTGMinesweeperService.Interfaces;
using StudioTGMinesweeperService.Models.DbModels;

namespace StudioTGMinesweeperService.Repositories
{
    public class NewGameRepository : INewGameRepository
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<NewGameModel> _dbSet;

        public NewGameRepository(MinesweeperContext minesweeperContext) 
        { 
            _dbContext = minesweeperContext;
            _dbSet = _dbContext.Set<NewGameModel>();
        }

        public async Task<List<NewGameModel>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<NewGameModel?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(NewGameModel updatedClient)
        {
            _dbSet.Update(updatedClient);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Create(NewGameModel client)
        {
            _dbSet.AddAsync(client);
            await _dbContext.SaveChangesAsync();
        }
    }
}
