namespace BankingAPI.Exception;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    // `RequestDelegate` middleware pipeline tarafından sağlanır
    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
    {
        // İstek yolunu loglayın
        Console.WriteLine($"Request Path: {context.Request.Path}");
        Console.WriteLine($"Request Method: {context.Request.Method}");

        var requestPath = context.Request.Path.ToString().TrimEnd('/');

        if (requestPath.Equals("/api/customers", StringComparison.OrdinalIgnoreCase) ||
            requestPath.Equals("/api/customers/token", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context); // Token doğrulama yapılmadan geç
            return;
        }

        // Token doğrulama işlemi
        var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token) || !jwtService.ValidateToken(token))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Invalid or missing token.");
            return;
        }

        await _next(context);
    }
}