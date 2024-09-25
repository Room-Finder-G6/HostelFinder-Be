using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RoomFinder.Domain.Common.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HostelFinder.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jwtSettings;
        public TokenService(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public string GenerateJwtToken(User user, UserRole role)
        {
            var claims = new List<Claim>()
            {
                new Claim ("UserId" , user.Id.ToString()),
                new Claim (ClaimTypes.Name, user.Username),
                new Claim (ClaimTypes.Email, user.Email),
                new Claim (ClaimTypes.Role, role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken (
                    _jwtSettings.Issuer,
                    _jwtSettings.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryInMinutes),
                    signingCredentials: credentials
                    );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int? ValidateToken(string token)
        {
            if (token == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}
