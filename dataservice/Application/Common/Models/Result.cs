namespace Application.Common.Models
{
    public class Result
    {
        public Result(bool succes)
        {
            Success = succes;
            Message = null;
        }

        public Result(bool success, string? message)
        {
            Success = success;
            Message = message;
        }


        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
