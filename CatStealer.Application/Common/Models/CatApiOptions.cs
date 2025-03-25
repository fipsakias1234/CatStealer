namespace CatStealer.Application.Common.Models
{
    public class CatApiOptions
    {
        public const string SectionName = "CatApi";
        public string BaseUrl { get; set; } = "https://api.thecatapi.com/v1/";
        public string ApiKey { get; set; } = string.Empty;
    }
}
