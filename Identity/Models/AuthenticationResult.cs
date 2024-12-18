namespace Identity.Models;

public class AuthenticationResult
{
    public string UserId { get; set; }

    public bool Succeeded { get; private init; }

    public string ErrorMessage { get; private init; }

    public static AuthenticationResult Success(string userId) => new() { Succeeded = true, UserId = userId };

    public static AuthenticationResult Failed(string error) => new() { Succeeded = false, ErrorMessage = error };
}
