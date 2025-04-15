using Application.Dtos.Auth;

public class AuthResponse : AuthDto
{

    public AuthResponse()
    {
    }

    public AuthResponse( string message)
    {
        Message = message;
    }

    public AuthResponse( string token, string username)
    {
        Token = token;
        Username = username;


    }

    public static AuthResponse Success( string token = "", string username = "")
    {
        return new AuthResponse( token, username);
    }

    public static AuthResponse Failure(string message)
    {
        return new AuthResponse(message);
    }
}