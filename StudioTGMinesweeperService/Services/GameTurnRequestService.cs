using Microsoft.AspNetCore.Http.HttpResults;
using StudioTGMinesweeperService.Interfaces;
using StudioTGMinesweeperService.Models;
using StudioTGMinesweeperService.Models.DbModels;
using System.Diagnostics.Eventing.Reader;

namespace StudioTGMinesweeperService.Services
{
    public class GameTurnRequestService : IGameTurnRequestService
    {
        //need to check if this cell was already checked and send error

        //get point and game id +
        //get initialized field from NewGame table +
        //checking what cell it is +
        //write that cell value +
        //if x than open full field and send where completed true
        //if not than algorhytm of opening nearby cells
        //after that update field in GameTurn table
        //create new GameInfo
        private readonly INewGameRepository _newGameRepository;

        private readonly IGameTurnRepository _gameTurnRepository;

        public GameTurnRequestService(INewGameRepository requestRepository, IGameTurnRepository gameTurnRepository)
        {
            _newGameRepository = requestRepository;
            _gameTurnRepository = gameTurnRepository;
        }

        //if cell is X then send completed game +
        //else cell is not X then open nearby cells +
        //if 0 then recursive opening till it's another number +
        //at the end checking if all cells are opened for "completed" flag
        public async Task<GameInfoResponse> MakeTurn(GameTurnRequest request)
        {
            var gameId = request.game_id;
            var rowIndex = request.row;
            var colIndex = request.col;

            var initGame = await _newGameRepository.GetByIdAsync(gameId);
            
            var initCharArray = ConvertTo2DCharArray(initGame.width, initGame.height, initGame.field);
            var initCell = initCharArray[rowIndex, colIndex];

            var currentGame = await _gameTurnRepository.GetByIdAsync(gameId);
            if (currentGame.completed)
            {
                throw new ArgumentException("больше нельзя делать ходы");
            }

            var currentField = ConvertTo2DCharArray(currentGame.width, currentGame.height, currentGame.field);

            if (currentField[rowIndex,colIndex] != ' ')
            {
                throw new ArgumentException("уже открытая ячейка");
            }
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
                UncoverField(rowIndex, colIndex, currentField, initCharArray, initGame.width, initGame.height);
            }

            currentGame.field = CreateStringArrayField(currentField, currentGame.width, currentGame.height);

            //if we find any closed field which is not X than continue game
            //else change all X to M and uncover all field
            bool completed = true;
            
            for(int i = 0; i < initGame.width; i++)
            {
                for(int j = 0;j < initGame.height; j++)
                {
                    if(currentField[i, j] == ' ')
                    {
                        if (initCharArray[i,j] != 'X')
                        {
                            completed = false;
                        }
                    }
                }
            }

            if (completed)
            {
                currentGame.completed = true;
                for (int i = 0; i < initGame.width; i++)
                {
                    for (int j = 0; j < initGame.height; j++)
                    {
                        if (currentField[i, j] == ' ')
                        {
                            if (initCharArray[i, j] == 'X')
                            {
                                currentField[i, j] = 'M';
                            }
                        }
                    }
                }
            }

            await _gameTurnRepository.UpdateAsync(currentGame);

            GameInfoResponse response = new GameInfoResponse { game_id = gameId, width = initGame.width, height = initGame.height, mines_count = initGame.mines_count, completed = false, field = currentField };
            return response;
        }

        public char[,] ConvertTo2DCharArray(int width, int height, string[] field)
        {
            var charArray = new char[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    charArray[i, j] = field[i][j];
                }
            }
            return charArray;
        }

        public string[] CreateStringArrayField(char[,] fieldChars, int width, int height)
        {
            string[] gameFieldString = new string[width];
            for (int i = 0; i < width; i++)
            {
                string row = string.Empty;
                for (int j = 0; j < height; j++)
                {
                    row += fieldChars[i, j];
                }
                gameFieldString[i] = row;
            }
            return gameFieldString;
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
    }
}
