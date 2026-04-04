namespace SidraHub.Application.Services.Auth.Models;

public sealed class AuthResult
{
    public bool Succeeded { get; init; }
    public AuthResponse? Data { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

    public static AuthResult Success(AuthResponse data) => new()
    {
        Succeeded = true,
        Data = data
    };

    public static AuthResult Failure(params string[] errors) => new()
    {
        Succeeded = false,
        Errors = errors
    };
}
