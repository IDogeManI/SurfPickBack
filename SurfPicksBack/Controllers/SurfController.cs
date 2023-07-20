using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurfPicksBack.Models;

namespace SurfPicksBack.Controllers
{
    [ApiController]
    [Route("api")]
    public class SurfController : ControllerBase
    {
        private LobbyService lobbyService;
        public SurfController(LobbyService lobbyService)
        {

            this.lobbyService = lobbyService;

        }

        [HttpPost("lobby/duel")]
        public ActionResult<DuelDto> CreateDuel(PredicateDto predicateDto)
        {
            if(predicateDto == null)
                return BadRequest();
            return Ok(lobbyService.InitLobby(x => predicateDto.IsFits(x),new DuelDto() {Player1=predicateDto.Player1,Player2=predicateDto.Player2 },9));
        }
        [HttpPost("lobby/tournamentbo3")]
        public ActionResult<DuelDto> TournamentBO3(PredicateDto predicateDto)
        {
            if (predicateDto == null)
                return BadRequest();
            return Ok(lobbyService.InitLobby(x => predicateDto.IsFits(x), new TournamentDto() { Player1 = predicateDto.Player1, Player2 = predicateDto.Player2 }, 7));
        }
        [HttpPost("lobby/tournamentbo5")]
        public ActionResult<DuelDto> TournamentBO5(PredicateDto predicateDto)
        {
            if (predicateDto == null)
                return BadRequest();
            return Ok(lobbyService.InitLobby(x => predicateDto.IsFits(x), new TournamentDto() { Player1 = predicateDto.Player1, Player2 = predicateDto.Player2 }, 11));
        }
        [HttpPatch("lobby")]
        public ActionResult<bool> NextStage(string? lobbyId,string? id, string? mapName)
        {
            if (Guid.TryParse(lobbyId, out Guid lobbyID) && int.TryParse(id, out int userId) && mapName != null)
                return Ok(lobbyService.NextStage(lobbyID, mapName, userId));
            return BadRequest();
        }
        [HttpPut("lobby")]
        public ActionResult<DuelDto> PingLobby(string? lobbyId)
        {
            if (Guid.TryParse(lobbyId, out Guid lobbyID)) 
                return Ok(lobbyService.PingLobby(lobbyID));
            return BadRequest();
        }
        [HttpDelete("lobby")]
        public ActionResult<bool> DeleteLobby(string? lobbyId)
        {
            if (Guid.TryParse(lobbyId, out Guid lobbyID))
                return Ok(lobbyService.DeleteLobby(lobbyID));
            return BadRequest();
        }
    }
}
