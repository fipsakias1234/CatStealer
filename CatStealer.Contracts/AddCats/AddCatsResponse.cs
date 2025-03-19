namespace CatStealer.Contracts.AddCats
{
    public class AddCatsResponse
    {
        List<AddCatDescriptionResponse> addedCats { get; set; }
    }

    public class AddCatDescriptionResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
