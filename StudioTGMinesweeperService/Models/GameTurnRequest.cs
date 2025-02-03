namespace StudioTGMinesweeperService.Models
{
    public class GameTurnRequest
    {
        public required Guid game_id { get; set; } //$uuid

        public required int col { get; set; }

        public required int row { get; set; }
    }
}
