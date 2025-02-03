using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StudioTGMinesweeperService.Interfaces;
using StudioTGMinesweeperService.Models;

namespace StudioTGMinesweeperService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinesweeperController : ControllerBase
    {
        //IGameService which takes two separate services in it
        private readonly INewGameRequestService _requestService;

        private readonly IGameTurnRequestService _turnRequestService;

        public MinesweeperController(INewGameRequestService requestService, IGameTurnRequestService turnRequestService) 
        {
            _turnRequestService = turnRequestService;
            _requestService = requestService;
        }
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost("new")]
        public async Task<ActionResult<GameInfoResponse>> New([FromBody]NewGameRequest request)
        {
            if (request.width <= 30 && request.height <= 30 && request.mines_count < request.width * request.height - 1)
            {
                var result = await _requestService.CreateNewGame(request);
                return result;
            }
            else
            {
                return BadRequest($"ширина поля должна быть не менее 2 и не более 30, количество мин не более {request.width * request.height - 2}");
            }
        }

        [HttpPost("turn")]
        public async Task<ActionResult<GameInfoResponse>> Turn([FromBody]GameTurnRequest request)
        {
            GameInfoResponse result;
            try
            {
                result = await _turnRequestService.MakeTurn(request);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            return result;
        }
    }
}
