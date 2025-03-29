using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using ReizzzToDo.BAL.Common;

namespace ReizzzToDo.BAL.Extensions
{
    public static class HttpContextAccessorExtension
    {
        public static long GetJwtSubValue(this IHttpContextAccessor httpContextAccessor)
        {
            string? userIdString = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
            bool isUserIdStringNull = long.TryParse(userIdString, out long userId);
            if (isUserIdStringNull == false)
            {
                throw new ApplicationException(ErrorMessage.JwtClaimCantReached);
            }
            return userId;
        }
    }
}
