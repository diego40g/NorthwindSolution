using Microsoft.IdentityModel.Tokens;
using Northwind.Models;

namespace Northwind.WebApi.Authentication
{
    public interface ITokenProvider
    {
        string CreateToken(User user, DateTime expiry);
        TokenValidationParameters GetValidationParameters();
    }
}
