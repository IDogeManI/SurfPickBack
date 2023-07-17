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

        [HttpPost("lobby")]
        public ActionResult<LobbyInfoDto> CreateLobby(PredicateDto predicateDto)
        {
            if(predicateDto == null)
                return BadRequest();
            return Ok(lobbyService.CreateLobby(x => predicateDto.IsFits(x)));
        }
        [HttpPatch("lobby")]
        public ActionResult<bool> NextStage(string? lobbyId,string? id, string? mapName)
        {
            if (Guid.TryParse(lobbyId, out Guid lobbyID) && int.TryParse(id, out int userId) && mapName != null)
                return Ok(lobbyService.NextStage(lobbyID, mapName, userId));
            return BadRequest();
        }
        [HttpPut("lobby")]
        public ActionResult<LobbyInfoDto> PingLobby(string? lobbyId)
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
