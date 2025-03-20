namespace CatStealer.Application.DTOs
{
    public class CatWithTagsDTO
    {
        public int Id { get; set; }
        public string CatId { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string Image { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
