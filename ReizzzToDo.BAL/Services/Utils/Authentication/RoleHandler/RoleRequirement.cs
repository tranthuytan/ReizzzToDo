using Microsoft.AspNetCore.Authorization;

namespace ReizzzToDo.BAL.Services.Utils.Authentication.RoleHandler
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(string role)
        {
            Role = role;
        }

        public string Role { get; set; }
    }
}
