using StudioTGMinesweeperService.Models;

namespace StudioTGMinesweeperService.Interfaces
{
    public interface INewGameRequestService
    {
         Task<GameInfoResponse> CreateNewGame(NewGameRequest request);
    }
}
