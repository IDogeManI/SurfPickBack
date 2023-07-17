namespace SurfPicksBack.Models
{
    public class LobbyInfoDto
    {
        public Guid LobbyId { get; set; }
        public List<SurfMapDto> mapsInLobby { get; set; } = new List<SurfMapDto>();
        public LobbyInfoStage Stage { get; set; } = LobbyInfoStage.None;
    }
}
