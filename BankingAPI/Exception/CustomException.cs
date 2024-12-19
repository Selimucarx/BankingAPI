namespace BankingAPI.Exception;

public class CustomException : System.Exception
{
    public CustomException(string message, string code, string developerMessage, string userMessage)
        : base(message)
    {
        Code = code;
        DeveloperMessage = developerMessage;
        UserMessage = userMessage;
    }

    public string Code { get; }
    public string DeveloperMessage { get; }
    public string UserMessage { get; }
}