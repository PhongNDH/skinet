namespace Api.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bab request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500
                    => "Errors are the path to dark side. Error leads to anger. Anger leads to hate. Hate leads to carreer change",
                _ => null
            };
        }
    }
}
