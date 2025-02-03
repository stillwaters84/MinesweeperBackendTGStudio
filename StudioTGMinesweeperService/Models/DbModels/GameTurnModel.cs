namespace StudioTGMinesweeperService.Models.DbModels
{
    public class GameTurnModel
    {
        public required Guid Id { get; set; } //uuid

        public required int width { get; set; }

        public required int height { get; set; }

        public required int mines_count { get; set; }

        public bool completed { get; set; }

        public required string[] field { get; set; }
    }
}
