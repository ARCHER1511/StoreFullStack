namespace Application.Common
{
    public class GeneralResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static GeneralResponse<T> Ok(T data, string? message = null) =>
            new()
            {
                Success = true,
                Data = data,
                Message = message ?? "Success",
            };

        public static GeneralResponse<T> Fail(string message) =>
            new() { Success = false, Message = message };
    }
}
