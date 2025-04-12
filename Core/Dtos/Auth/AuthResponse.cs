using Domain.Dtos.Auth;

public class AuthResponse : AuthDto
{
    public bool IsSuccess { get; set; }

    public AuthResponse()
    {
    }

    public AuthResponse(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
        IsAuthenticated = isSuccess;
    }

    public AuthResponse(bool isSuccess, string message, string token, string username)
    {
        IsSuccess = isSuccess;
        Message = message;
        Token = token;
        Username = username;
        IsAuthenticated = isSuccess;
    }

    // Create success response
    public static AuthResponse Success(string message, string token = "", string username = "")
    {
        return new AuthResponse(true, message, token, username);
    }

    // Create failure response
    public static AuthResponse Failure(string message)
    {
        return new AuthResponse(false, message);
    }
}