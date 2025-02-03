using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudioTGMinesweeperService.Interfaces;
using StudioTGMinesweeperService.Models;
using StudioTGMinesweeperService.Models.DbModels;
using StudioTGMinesweeperService.Repositories;

namespace StudioTGMinesweeperService.Services
{
    public class NewGameRequestService : INewGameRequestService
    {
        private readonly INewGameRepository _newGameRepository;

        private readonly IGameTurnRepository _gameTurnRepository;

        public NewGameRequestService(INewGameRepository requestRepository, IGameTurnRepository gameTurnRepository)
        {
            _newGameRepository = requestRepository;
            _gameTurnRepository = gameTurnRepository;
        }

        public async Task<GameInfoResponse> CreateNewGame(NewGameRequest request) //3 parameters (maybe newgamerequest) - NewGameRequest
        {
            //checking if width or height <= 30, checking mine_count
            //create array [width][height]
            var initializedGame = CreateGame(request);
            await _newGameRepository.Create(initializedGame);

            //after db save we are hiding the field
            char[,] emptyGameFieldArray = new char[request.width, request.height];
            string[] emptyGameFieldString = CreateEmptyField(request.width, request.height);
            //FOR WITH ' ' SYMBOLS
            //initializedGame.field = emptyGameField;

            GameTurnModel gameTurnZero = new GameTurnModel { Id = initializedGame.Id, width = request.width, height = request.height, mines_count = request.mines_count, completed = false, field = emptyGameFieldString };
            await _gameTurnRepository.Create(gameTurnZero);

            //automapper but better
            for(int i = 0; i < emptyGameFieldString.Length; i++)
            {
                var row = emptyGameFieldString[i].ToCharArray(0, emptyGameFieldString[i].Length);
                for(int j = 0; j <  row.Length; j++)
                {
                    emptyGameFieldArray[i,j] = row[j];
                }
            }
            GameInfoResponse response = new GameInfoResponse { game_id = gameTurnZero.Id, width = gameTurnZero.width, height = gameTurnZero.height, mines_count = gameTurnZero.mines_count, completed = false, field = emptyGameFieldArray };
            return response;
        }

        public NewGameModel CreateGame(NewGameRequest request)
        {
            var gameId = new Guid();
            var gameFieldArray = CreateCompletedField(request.width, request.height, request.mines_count);
            var gameFieldString = CreateStringArrayField(gameFieldArray, request.width, request.height);

            var gameInfo = new NewGameModel { Id = gameId, width = request.width, height = request.height, mines_count = request.mines_count, completed = false, field = gameFieldString };
            return gameInfo;
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

        //need to improve random numbers
        public char[,] CreateCompletedField(int width, int height, int minesCount)
        {
            char[,] fieldArray = new char[width, height];
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    fieldArray[i, j] = '0';
                }
            }
            int MinesCount = minesCount;

            //algorithm from github
            int mineIndex = 0;
            Random rnd = new Random();

            while (mineIndex < MinesCount)
            {
                int firstRndNum = rnd.Next(0, width-1);
                int secondRndNum = rnd.Next(0, height-1);

                if (fieldArray[firstRndNum, secondRndNum] != 'X')//Field.FailedGameMine) //X symbol
                {
                    fieldArray[firstRndNum, secondRndNum] = 'X'; //Field.FailedGameMine;

                    //Increases the cells value around the current mine.
                    for (int aroundMineI = firstRndNum - 1; aroundMineI <= firstRndNum + 1; aroundMineI++)
                    {
                        for (int aroundMineJ = secondRndNum - 1; aroundMineJ <= secondRndNum + 1; aroundMineJ++)
                        {
                            if (aroundMineI >= 0 && aroundMineJ >= 0 && aroundMineI < width && aroundMineJ < height)
                            {
                                var value = fieldArray[aroundMineI, aroundMineJ];
                                if (value != 'X')
                                {
                                    fieldArray[aroundMineI, aroundMineJ] = (char)(Convert.ToInt32(value) + 1);
                                }
                            }
                        }
                    }
                    mineIndex++;
                }
            }
            return fieldArray;
        }

        public string[] CreateEmptyField(int width, int height)
        {
            string[] array = new string[height];
            for (int i = 0; i < height; i++)
            {
                array[i] = new string(' ', width);
            }

            return array;
        }
    }
}
