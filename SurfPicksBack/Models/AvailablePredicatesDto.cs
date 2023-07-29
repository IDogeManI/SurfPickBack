namespace SurfPicksBack.Models
{
    public class AvailablePredicatesDto
    {
        public List<string>? Pools { get; set; }
        public List<List<string>>? Styles { get; set; } = new List<List<string>>();
        public List<List<string>>? Tiers { get; set; } = new List<List<string>>();
    }
}
