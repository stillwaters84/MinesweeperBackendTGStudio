using StudioTGMinesweeperService.Additional;
using StudioTGMinesweeperService.Interfaces;
using StudioTGMinesweeperService.Models;
using StudioTGMinesweeperService.Models.DbModels;

namespace StudioTGMinesweeperService.Services
{
    public class GameTurnRequestService : IGameTurnRequestService
    {
        private readonly INewGameRepository _newGameRepository;

        private readonly IGameTurnRepository _gameTurnRepository;

        public GameTurnRequestService(INewGameRepository requestRepository, IGameTurnRepository gameTurnRepository)
        {
            _newGameRepository = requestRepository;
            _gameTurnRepository = gameTurnRepository;
        }

        public async Task<GameInfoResponse> MakeTurn(GameTurnRequest request)
        {
            var gameId = request.game_id;
            var rowIndex = request.row;
            var colIndex = request.col;

            //suppose we can't have null initGame
            var initGame = await _newGameRepository.GetByIdAsync(gameId);
            
            var initField = ArrayMethods.ConvertTo2DCharArray(initGame.width, initGame.height, initGame.field);
            var initCell = initField[rowIndex, colIndex];

            var currentGame = await _gameTurnRepository.GetByIdAsync(gameId);
            if (currentGame.completed)
            {
                throw new ArgumentException("больше нельзя делать ходы");
            }

            var currentField = ArrayMethods.ConvertTo2DCharArray(currentGame.width, currentGame.height, currentGame.field);

            if (currentField[rowIndex,colIndex] != ' ')
            {
                throw new ArgumentException("уже открытая ячейка");
            }

            //uncovering algo
            if (initCell == 'X')
            {
                for (int i = 0; i < initGame.width; i++)
                {
                    var row = initGame.field[i].ToCharArray(0, initGame.field[i].Length);
                    for (int j = 0; j < row.Length; j++)
                    {
                        currentField[i, j] = row[j];
                    }
                }
            }
            else if(initCell >= '0' && initCell <= '8')
            {
                UncoverField(rowIndex, colIndex, currentField, initField, initGame.width, initGame.height);
            }

            currentGame.field = ArrayMethods.CreateStringArrayField(currentField, currentGame.width, currentGame.height);

            //if we find any closed field which is not X than continue game
            //else change all X to M and uncover all field
            bool completed = true;
            
            for(int i = 0; i < initGame.width; i++)
            {
                for(int j = 0;j < initGame.height; j++)
                {
                    if(currentField[i, j] == ' ')
                    {
                        if (initField[i,j] != 'X')
                        {
                            completed = false;
                        }
                    }
                }
            }

            if (completed)
            {
                currentGame.completed = true;
                ReplaceWithMSymbol(initGame, initField, currentField);
            }

            await _gameTurnRepository.UpdateAsync(currentGame);

            GameInfoResponse response = new GameInfoResponse { game_id = gameId, width = initGame.width, height = initGame.height, mines_count = initGame.mines_count, completed = false, field = currentField };
            return response;
        }

        public void UncoverField(int row, int col, char[,] currentField, char[,] gameField, int width, int height)
        {
            if(row >= 0 && col >= 0 && row < width && col < height)
            {
                if (gameField[row, col] == '0' && currentField[row, col] == ' ')
                {
                    currentField[row, col] = gameField[row, col];
                    for(int i = -1; i <= 1; i++)
                    {
                        for(int j = -1; j <= 1; j++)
                        {
                            UncoverField(row + i, col + j, currentField, gameField, width, height);
                        }
                    }
                }
                else if (gameField[row,col] > '0' && gameField[row,col] <= '8')
                {
                    currentField[row, col] = gameField[row, col];
                }
            }
        }

        public void ReplaceWithMSymbol(NewGameModel initGame, char[,] initField, char[,] currentField)
        {
            for (int i = 0; i < initGame.width; i++)
            {
                for (int j = 0; j < initGame.height; j++)
                {
                    if (currentField[i, j] == ' ')
                    {
                        if (initField[i, j] == 'X')
                        {
                            currentField[i, j] = 'M';
                        }
                    }
                }
            }
        }
    }
}
