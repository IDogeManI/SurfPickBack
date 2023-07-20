namespace SurfPicksBack.Models
{
    public class TournamentDto : DuelDto
    {
        protected override bool BanTranlationToPickLogic()
        {
            return this.MapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList().Count % 2 == 1;
        }
        protected override bool PickTranlationToBanLogic()
        {
            return this.MapsInLobby.Where(x => x.Status == SurfMapStatus.None).ToList().Count % 2 == 1;
        }
    }
}
