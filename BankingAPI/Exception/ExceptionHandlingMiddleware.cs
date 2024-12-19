using BankingAPI.Exception;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CustomException ex)
        {
            var errorResponse = new ErrorResponse
            {
                Status = "error",
                Message = ex.Message,
                Code = ex.Code,
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                ErrorDetails = new ErrorDetails
                {
                    Field = "customerId", // Örneğin, müşteri ID'si ile ilgili bir hata
                    Reason = "The provided customer ID does not exist in the database."
                },
                DeveloperMessage = ex.DeveloperMessage,
                UserMessage = ex.UserMessage
            };

            context.Response.StatusCode = 400; // İlgili HTTP durumu
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
        catch (Exception ex)
        {
            // Genel hata durumu
            _logger.LogError(ex, "An unexpected error occurred");
            var errorResponse = new ErrorResponse
            {
                Status = "error",
                Message = "An unexpected error occurred.",
                Code = "ERR_GENERIC",
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                DeveloperMessage = "An unexpected error occurred in the system.",
                UserMessage = "An error occurred. Please try again later."
            };

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}