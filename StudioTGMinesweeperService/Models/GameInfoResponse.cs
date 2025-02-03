namespace StudioTGMinesweeperService.Models
{
    public class GameInfoResponse
    {
        public required Guid game_id { get; set; } //uuid

        public required int width { get; set; }

        public required int height { get; set; }

        public required int mines_count { get; set; }

        public bool completed { get; set; }

        public required char[,] field { get; set; }
    }
}
