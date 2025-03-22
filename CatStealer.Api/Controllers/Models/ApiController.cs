namespace CatStealer.Api.Controllers.Models
{
    public class ApiResponse
    {
        public IEnumerable<ErrorOr.Error> Errors { get; set; } = [];

        public bool Success { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }
}
