using OfflineTicketingSystem.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OfflineTicketingSystem.Services
{
    public interface ITokenService
    {
        string CreateToken(UserEntity user);
    }

    public class AuthService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public AuthService(IConfiguration config)
        {
            _config = config;

            var secretKey = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JWT Key not found in configuration.");

            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            
            if (keyBytes.Length < 64)
                throw new InvalidOperationException(
                    $"JWT Key must be at least 64 bytes for HMAC-SHA512. Current: {keyBytes.Length} bytes.");

            _key = new SymmetricSecurityKey(keyBytes);
        }

        public string CreateToken(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
