namespace SurfPicksBack.Models
{
    public class SurfMapDto
    {
        public string Name { get; set; } = "";
        public string Pool { get; set; } = "";
        public string Style { get; set; } = "";
        public string ImageSrc { get; set; } = "";
        public string Tier { get; set; } = "";
        public SurfMapStatus Status { get; set; }
        public string Player { get; set; } = "";
    }
}
