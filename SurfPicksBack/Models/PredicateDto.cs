namespace SurfPicksBack.Models
{
    public class PredicateDto
    {
        public List<string>? servers { get; set; }
        public List<int>? tiers { get; set; }
        public string Player1 { get; set; } = "";
        public string Player2 { get; set; } = "";
        public bool IsFits(SurfMapDto map)
        {
            return servers != null && tiers != null && servers.Contains(map.Server) && tiers.Contains(map.Tier);
        }
    }
}
