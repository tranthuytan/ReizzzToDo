using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using ReizzzToDo.BAL.Services.UserServices;

namespace ReizzzToDo.BAL.Services.Utils.Authentication.RoleHandler
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RoleAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            // get current userId from context.User.Claims
            string? userId = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!long.TryParse(userId, out var parsedUserId))
            {
                return;
            }
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                IUserService _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                HashSet<string> roles = await _userService.GetUserRoles(parsedUserId);
                if (roles.Contains(requirement.Role))
                {
                    context.Succeed(requirement);
                }
            }
            return;
        }
    }
}
