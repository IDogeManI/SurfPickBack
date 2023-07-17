using SurfPicksBack.Models;

namespace SurfPicksBack
{
    public class LobbyService
    {
        private List<LobbyInfoDto> allLobbies = new List<LobbyInfoDto>();

        private SurfMapService mapService;
        public LobbyService(SurfMapService mapService)
        {

            this.mapService = mapService;

        }
        public LobbyInfoDto CreateLobby(Func<SurfMapDto, bool> predicate)
        {
            Random rand = new Random();
            int whoStart = rand.Next(1, 3);
            LobbyInfoDto lobby = new LobbyInfoDto();
            lobby.LobbyId = Guid.NewGuid();
            lobby.mapsInLobby = mapService.GetSurfMaps(predicate);
            lobby.Stage = (LobbyInfoStage)whoStart;
            allLobbies.Add(lobby);
            return lobby;
        }
        public bool DeleteLobby(Guid id)
        {
            LobbyInfoDto? lobby = allLobbies.FirstOrDefault(x => x.LobbyId == id);
            if (lobby != null)
            {
                allLobbies.Remove(lobby);
                return true;
            }
            return false;
        }
        public bool NextStage(Guid lobbyId, string name, int id)
        {
            LobbyInfoDto? lobby = allLobbies.FirstOrDefault(x => x?.LobbyId.Equals(lobbyId) ?? false, null);
            if (lobby == null)
                return false;
            SurfMapDto? pickedMap = lobby.mapsInLobby.Where(x => x.Status == SurfMapStatus.None).FirstOrDefault(x => x?.Name == name, null);
            if (pickedMap == null)
                return false;
            if (lobby.Stage == LobbyInfoStage.FirstPlayerPick || lobby.Stage == LobbyInfoStage.SecondPlayerPick)
                return NextPickStage(lobby, pickedMap, id);
            if (lobby.Stage == LobbyInfoStage.FirstPlayerBan && id == 1)
            {
                lobby.Stage = LobbyInfoStage.SecondPlayerBan;
                pickedMap.Status = SurfMapStatus.Banned;
                if (lobby.mapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList().Count == 3)
                    lobby.Stage = LobbyInfoStage.SecondPlayerPick;
                return true;
            }
            if (lobby.Stage == LobbyInfoStage.SecondPlayerBan && id == 2)
            {
                lobby.Stage = LobbyInfoStage.FirstPlayerBan;
                pickedMap.Status = SurfMapStatus.Banned;
                if (lobby.mapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList().Count == 3)
                    lobby.Stage = LobbyInfoStage.FirstPlayerPick;
                return true;
            }
            return false;
        }
        private bool NextPickStage(LobbyInfoDto lobby, SurfMapDto pickedMap, int id)
        {
            if (lobby.Stage == LobbyInfoStage.FirstPlayerPick && id == 1)
            {
                lobby.Stage = LobbyInfoStage.SecondPlayerPick;
                pickedMap.Status = SurfMapStatus.Picked;
                if (lobby.mapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList().Count == 1)
                {
                    lobby.Stage = LobbyInfoStage.None;
                    lobby.mapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList()[0].Status = SurfMapStatus.Decider;
                }
                return true;
            }
            if (lobby.Stage == LobbyInfoStage.SecondPlayerPick && id == 2)
            {
                lobby.Stage = LobbyInfoStage.FirstPlayerPick;
                pickedMap.Status = SurfMapStatus.Picked;
                if (lobby.mapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList().Count == 1)
                {
                    lobby.Stage = LobbyInfoStage.None;
                    lobby.mapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList()[0].Status = SurfMapStatus.Decider;
                }
                return true;
            }
            return false;
        }
        public LobbyInfoDto? PingLobby(Guid id)
        {
            LobbyInfoDto? lobbyInfo = allLobbies.FirstOrDefault(x => x?.LobbyId.Equals(id)??false, null);
            return lobbyInfo;
        }

    }
}
