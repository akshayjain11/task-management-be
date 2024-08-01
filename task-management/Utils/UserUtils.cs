using System.Security.Claims;
using task_management.Services;

namespace task_management.Utils
{
    public class UserUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserUtils(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal GetUserDetailsFromToken()
        {
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (accessToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                accessToken = accessToken.Substring("Bearer ".Length).Trim();
            }
            ClaimsPrincipal claimsPrincipal = JWTTokenUtil.GetPrincipalFromToken(accessToken, out bool isExpired);
            return claimsPrincipal;
        }
    }
}
