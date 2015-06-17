namespace Gwent.NET.DTOs
{
    public class GameHubResult<T>
    {
        public string Error { get; set; }

        public T Data { get; set; }
    }
}