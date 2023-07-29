using Microsoft.AspNetCore.Mvc;
using SurfPicksBack.Models;

namespace SurfPicksBack.Controllers
{
    [ApiController]
    [Route("api")]
    public class SurfController : ControllerBase
    {
        private LobbyService lobbyService;
        private SurfMapService mapService;
        private readonly IWebHostEnvironment appEnvironment;
        public SurfController(LobbyService lobbyService, IWebHostEnvironment webHostEnvironment, SurfMapService surfMapService)
        {
            appEnvironment = webHostEnvironment;
            this.lobbyService = lobbyService;
            mapService = surfMapService;    

        }
        [HttpGet("map/{imgSrc}")]
        public ActionResult GetMapImage(string? imgSrc)
        {
            if (imgSrc == null)
                return BadRequest();
            string filePath = Path.Combine(appEnvironment.ContentRootPath, "Assets/Images/"+ imgSrc);
            string fileType = "application/jpg";
            if (System.IO.File.Exists(filePath))
            {
                return PhysicalFile(filePath,fileType);
                
            }
            return BadRequest(filePath);
        }
        [HttpGet("predicates")]
        public ActionResult<AvailablePredicatesDto> GetAvailablePredicates()
        {
            return Ok(mapService.GetAvailablePredicates());
        }
        [HttpPost("lobby/duel")]
        public ActionResult<DuelDto> CreateDuel(PredicateDto predicateDto)
        {
            if(predicateDto == null)
                return BadRequest();
            return Ok(lobbyService.InitLobby(x => predicateDto.IsFits(x),new DuelDto() {Player1=predicateDto.Player1,Player2=predicateDto.Player2 ,Predicate = predicateDto},9));
        }
        [HttpPost("lobby/tournamentbo3")]
        public ActionResult<DuelDto> TournamentBO3(PredicateDto predicateDto)
        {
            if (predicateDto == null)
                return BadRequest();
            return Ok(lobbyService.InitLobby(x => predicateDto.IsFits(x), new TournamentDto() { Player1 = predicateDto.Player1, Player2 = predicateDto.Player2, Predicate = predicateDto }, 7));
        }
        [HttpPost("lobby/tournamentbo5")]
        public ActionResult<DuelDto> TournamentBO5(PredicateDto predicateDto)
        {
            if (predicateDto == null)
                return BadRequest();
            return Ok(lobbyService.InitLobby(x => predicateDto.IsFits(x), new TournamentDto() { Player1 = predicateDto.Player1, Player2 = predicateDto.Player2 , Predicate = predicateDto }, 11));
        }
        [HttpPost("lobby/tournamentbo5withoutreroll")]
        public ActionResult<DuelDto> TournamentBO5WithoutReroll(PredicateDto predicateDto)
        {
            if (predicateDto == null)
                return BadRequest();
            return Ok(lobbyService.InitLobby(x => predicateDto.IsFits(x), new TournamentDto() { Player1 = predicateDto.Player1, Player2 = predicateDto.Player2, Predicate = predicateDto,Rerollable=false }, 11));
        }
        [HttpPost("lobby/reroll")]
        public ActionResult<bool> VoteForReroll(string? lobbyId, string? id)
        {
            if (Guid.TryParse(lobbyId, out Guid lobbyID) && int.TryParse(id, out int userId))
                return Ok(lobbyService.VoteForReroll(lobbyID, userId));
            return BadRequest();
        }
        [HttpPost("lobby/unreroll")]
        public ActionResult<bool> VoteForUnreroll(string? lobbyId, string? id)
        {
            if (Guid.TryParse(lobbyId, out Guid lobbyID) && int.TryParse(id, out int userId))
                return Ok(lobbyService.VoteForUnreroll(lobbyID, userId));
            return BadRequest();
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
