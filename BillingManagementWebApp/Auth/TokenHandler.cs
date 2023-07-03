using BillingManagementWebApp.Auth.Model;
using BillingManagementWebApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BillingManagementWebApp.Auth
{
    public class TokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Token CreateAccessToken(User user)
        {
            Token token = new Token();
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.isAdmin ? "Admin" : "User")
            };
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            token.ExpirationDate = DateTime.Now.AddMinutes(15);
            JwtSecurityToken securityToken = new(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                claims: claims,
                expires: token.ExpirationDate,
                signingCredentials: signingCredentials);

            JwtSecurityTokenHandler tokenHandler = new();

            token.AccessToken = tokenHandler.WriteToken(securityToken);
            token.RefreshToken = CreateRefreshToken();

            return token;
        }
        public string CreateRefreshToken() { return Guid.NewGuid().ToString();}
    }
}
