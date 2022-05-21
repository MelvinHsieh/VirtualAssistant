namespace Application.Common.Models
{
    public class Result
    {
        public Result(bool success, string? message = null)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
