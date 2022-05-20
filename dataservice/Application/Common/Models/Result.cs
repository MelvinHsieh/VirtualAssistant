namespace Application.Common.Models
{
    public class Result
    {
        public Result(bool success, string? message = null)
        {
            Success = succes;
            ResponseData = null;
            Message = null;
        }

        public Result(bool success, string? message = null)
        {
            Success = success;
            ResponseData = null;
            Message = message;
        }

        public Result(bool success, string? message = null, object? response = null)
        {
            Success = success;
            ResponseData = response; 
            Message = message;
        }


        public bool Success { get; set; }
        public string? Message { get; set; }

        public object? ResponseData { get; set; }
    }
}
