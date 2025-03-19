namespace CatStealer.Application.DTOs
{
    public class AddCatsDTO
    {
        public List<AddCatDescriptionDTO> addedCats { get; set; }
    }

    public class AddCatDescriptionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
