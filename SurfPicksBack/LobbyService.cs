using SurfPicksBack.Models;

namespace SurfPicksBack
{
    public class LobbyService
    {
        private List<DuelDto> allLobbies = new List<DuelDto>();

        private SurfMapService mapService;

        private Timer timer = null;
        public LobbyService(SurfMapService mapService)
        {

            this.mapService = mapService;
            StartTimer();

        }
        public void StartTimer()
        {
            this.timer = new Timer(new TimerCallback(DeleteExpiredLobbiesAndInitMaps),null,0, 86400000); //1 day = 86400000 ms
        }
        public void DeleteExpiredLobbiesAndInitMaps(object state)
        {
            foreach(var lobby in allLobbies)
            {
                if (lobby != null&&lobby.ExpirationDate<=DateTime.Now)
                {
                    allLobbies.Remove(lobby);
                }
            }
            mapService.InitMaps();
        }
        public DuelDto InitLobby(Func<SurfMapDto, bool> predicate, DuelDto lobby, int countOfMaps)
        {
            Random rand = new Random();
            int whoStart = rand.Next(1, 3);
            lobby.LobbyId = Guid.NewGuid();
            lobby.MapsInLobby = mapService.GetSurfMaps(predicate, countOfMaps);
            lobby.Stage = (LobbyInfoStage)whoStart;
            allLobbies.Add(lobby);
            return lobby;
        }
        public bool DeleteLobby(Guid id)
        {
            DuelDto? lobby = allLobbies.FirstOrDefault(x => x.LobbyId == id);
            if (lobby != null)
            {
                allLobbies.Remove(lobby);
                return true;
            }
            return false;
        }
        public bool NextStage(Guid lobbyId, string mapName, int id)
        {
            DuelDto? lobby = allLobbies.FirstOrDefault(x => x?.LobbyId.Equals(lobbyId) ?? false, null);
            if (lobby == null)
                return false;
            return lobby.NextStage(mapName, id);
        }
        public bool VoteForReroll(Guid lobbyId, int id)
        {
            DuelDto? lobby = allLobbies.FirstOrDefault(x => x?.LobbyId.Equals(lobbyId) ?? false, null);
            if (lobby == null)
                return false;
            return lobby.VoteForReroll(id, mapService);
        }
        public bool VoteForUnreroll(Guid lobbyId, int id)
        {
            DuelDto? lobby = allLobbies.FirstOrDefault(x => x?.LobbyId.Equals(lobbyId) ?? false, null);
            if (lobby == null)
                return false;
            return lobby.VoteForUnreroll(id);
        }
        public DuelDto? PingLobby(Guid id)
        {
            DuelDto? lobbyInfo = allLobbies.FirstOrDefault(x => x?.LobbyId.Equals(id) ?? false, null);
            return lobbyInfo;
        }

    }
}
