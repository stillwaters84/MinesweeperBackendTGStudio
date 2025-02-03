using Microsoft.EntityFrameworkCore;

namespace StudioTGMinesweeperService.Models.DbModels
{
    public class NewGameModel //creating game - saving new game with field in db - 1 game 1 field - GAMEFIELDMODEL
    {
        public required Guid Id { get; set; } //uuid

        public required int width { get; set; }

        public required int height { get; set; }

        public required int mines_count { get; set; }

        public bool completed { get; set; }

        //EF CORE CAN'T DO SHIT
        public required string[] field { get; set; }
    }
}
