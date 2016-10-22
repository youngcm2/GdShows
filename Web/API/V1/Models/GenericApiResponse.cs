namespace GdShows.API.V1.Models
{
    public class GenericApiResponse
    {
        public GenericApiResponse()
        {
            Success = true;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
