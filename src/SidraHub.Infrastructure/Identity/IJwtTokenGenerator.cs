using SidraHub.Application.Services.Auth.Models;

namespace SidraHub.Infrastructure.Identity;

public interface IJwtTokenGenerator
{
    AuthResponse GenerateToken(ApplicationUser user, IReadOnlyCollection<string> roles);
}
