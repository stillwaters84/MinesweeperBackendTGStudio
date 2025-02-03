using StudioTGMinesweeperService.Models;
using StudioTGMinesweeperService.Models.DbModels;

namespace StudioTGMinesweeperService.Interfaces
{
    public interface INewGameRepository
    {
        Task<List<NewGameModel>> GetAsync();

        Task<NewGameModel?> GetByIdAsync(Guid id);

        Task UpdateAsync(NewGameModel updatedClient);

        Task Create(NewGameModel client);
    }
}
