namespace SurfPicksBack.Models
{
    public class DuelDto
    {
        public Guid LobbyId { get; set; }
        public string Player1 { get; set; } = "";
        public string Player2 { get; set; } = "";
        public bool IsPlayer1WantReroll { get; set; } = false;
        public bool IsPlayer2WantReroll { get; set; } = false;
        public List<SurfMapDto> MapsInLobby { get; set; } = new List<SurfMapDto>();
        public LobbyInfoStage Stage { get; set; } = LobbyInfoStage.None;
        public bool Rerollable { get; set; } = true;
        public DateTime ExpirationDate { get; set; } = DateTime.Now + new TimeSpan(14,0,0,0);
        public PredicateDto Predicate { get; set; } = new PredicateDto();

        public bool VoteForReroll(int id, SurfMapService surfMapService)
        {
            if (id == 1)
            {
                IsPlayer1WantReroll = true;
            }
            if (id == 2)
            {
                IsPlayer2WantReroll = true;
            }
            if (IsPlayer1WantReroll && IsPlayer2WantReroll)
            {
                var tmp = surfMapService.GetSurfMaps(x => Predicate.IsFits(x), MapsInLobby.Count);
                MapsInLobby = tmp;
                return true;
            }
            return false;
        }
        public bool VoteForUnreroll(int id)
        {
            if (id == 1)
            {
                IsPlayer1WantReroll = false;
                return true;
            }
            if (id == 2)
            {
                IsPlayer2WantReroll = false;
                return true;
            }
            return false;
        }
        public bool NextStage(string mapName, int id)
        {
            SurfMapDto? pickedMap = this.MapsInLobby.Where(x => x.Status == SurfMapStatus.None).FirstOrDefault(x => x?.Name == mapName, null);
            if (pickedMap == null)
                return false;
            Rerollable = false;
            if (this.Stage == LobbyInfoStage.FirstPlayerPick || this.Stage == LobbyInfoStage.SecondPlayerPick)
                return NextPickStage(pickedMap, id);
            if (this.Stage == LobbyInfoStage.FirstPlayerBan && id == 1)
            {
                this.Stage = LobbyInfoStage.SecondPlayerBan;
                pickedMap.Status = SurfMapStatus.Banned;
                pickedMap.Player = this.Player1;
                if (BanTranlationToPickLogic())
                    this.Stage = LobbyInfoStage.SecondPlayerPick;
                LastAutoDecider();
                return true;
            }
            if (this.Stage == LobbyInfoStage.SecondPlayerBan && id == 2)
            {
                this.Stage = LobbyInfoStage.FirstPlayerBan;
                pickedMap.Status = SurfMapStatus.Banned;
                pickedMap.Player = this.Player2;
                if (BanTranlationToPickLogic())
                    this.Stage = LobbyInfoStage.FirstPlayerPick;
                LastAutoDecider();
                return true;
            }
            return false;
        }
        protected virtual bool BanTranlationToPickLogic()
        {
            return this.MapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList().Count == 3;
        }
        protected virtual bool PickTranlationToBanLogic()
        {
            return false;
        }
        private bool NextPickStage(SurfMapDto pickedMap, int id)
        {
            if (this.Stage == LobbyInfoStage.FirstPlayerPick && id == 1)
            {
                this.Stage = LobbyInfoStage.SecondPlayerPick;
                pickedMap.Status = SurfMapStatus.Picked;
                pickedMap.Player = this.Player1;
                if (PickTranlationToBanLogic())
                    this.Stage = LobbyInfoStage.SecondPlayerBan;
                LastAutoDecider();
                return true;
            }
            if (this.Stage == LobbyInfoStage.SecondPlayerPick && id == 2)
            {
                this.Stage = LobbyInfoStage.FirstPlayerPick;
                pickedMap.Status = SurfMapStatus.Picked;
                pickedMap.Player = this.Player2;
                if (PickTranlationToBanLogic())
                    this.Stage = LobbyInfoStage.FirstPlayerBan;
                LastAutoDecider();
                return true;
            }
            return false;
        }

        private void LastAutoDecider()
        {
            if (this.MapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList().Count == 1)
            {
                this.Stage = LobbyInfoStage.None;
                this.MapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList()[0].Status = SurfMapStatus.Decider;
            }
        }
    }
}
