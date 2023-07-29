namespace SurfPicksBack.Models
{
    public class PredicateDto
    {
        public string? Pool { get; set; }
        public List<string>? Tiers { get; set; }
        public List<string>? Styles { get; set; }
        public string Player1 { get; set; } = "";
        public string Player2 { get; set; } = "";
        public bool IsFits(SurfMapDto map)
        {
            return Pool != null && Tiers != null&& Styles!=null && Pool.Equals(map.Pool) && Tiers.Contains(map.Tier) && Styles.Contains(map.Style);
        }
    }
}
