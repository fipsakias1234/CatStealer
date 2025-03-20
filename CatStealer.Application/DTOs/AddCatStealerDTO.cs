namespace CatStealer.Application.DTOs
{
    public class AddCatsDTO
    {
        public List<AddCatDescriptionDTO> AddedCats { get; set; } = new();
    }

    public class AddCatDescriptionDTO
    {
        public string CatId { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
