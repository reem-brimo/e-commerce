using System.Security.Claims;

namespace E_Commerce.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(List<Claim> claims, string email, string username);
    }
}
