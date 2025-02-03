namespace StudioTGMinesweeperService.Models
{
    public class NewGameRequest
    {
        public required int width { get; set; }

        public required int height { get; set; }

        public required int mines_count { get; set; }
    }
}
