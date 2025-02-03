using StudioTGMinesweeperService.Models.DbModels;

namespace StudioTGMinesweeperService.Interfaces
{
    public interface IGameTurnRepository
    {
        Task<List<GameTurnModel>> GetAsync();

        Task<GameTurnModel?> GetByIdAsync(Guid id);

        Task UpdateAsync(GameTurnModel updatedClient);

        Task Create(GameTurnModel client);
    }
}
