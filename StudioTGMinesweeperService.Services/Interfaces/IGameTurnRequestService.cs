using StudioTGMinesweeperService.Models;

namespace StudioTGMinesweeperService.Interfaces
{
    public interface IGameTurnRequestService
    {
        Task<GameInfoResponse> MakeTurn(GameTurnRequest request);
    }
}
