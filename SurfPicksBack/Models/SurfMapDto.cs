namespace SurfPicksBack.Models
{
    public class SurfMapDto
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public string ImageSrc { get; set; }
        public int Tier { get; set; }
        public SurfMapStatus Status { get; set; }
    }
}
