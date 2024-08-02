using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using task_management.Models;

namespace task_management.Services
{
    public static class JWTTokenUtil
    {
        private static readonly IConfigurationSection _jWTSettings = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build().GetSection("Jwt");



        public static ClaimsPrincipal GetPrincipalFromToken(string refreshToken, out bool isExpired)
        {
            isExpired = false;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = _jWTSettings["Audience"],
                ValidIssuer = _jWTSettings["Issuer"],
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings["Key"])),
            ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = new ClaimsPrincipal();
            try
            {
                principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
                var ticks = long.Parse(tokenExp);
                var tokenDate = DateTimeOffset.FromUnixTimeSeconds(ticks).LocalDateTime;

                if (tokenDate <= DateTime.Now)
                {
                    isExpired = true;
                }

            }
            catch
            {
                isExpired = true;
                throw;
            }
            return principal;

        }

    }
}
