using Microsoft.AspNetCore.Authorization;
using ReizzzToDo.BAL.Enums;

namespace ReizzzToDo.BAL.Services.Utils.Authentication.RoleHandler
{
    public sealed class HasRoleAttribute : AuthorizeAttribute
    {
        public HasRoleAttribute(Role role) : base(policy: role.ToString())
        {
        }
    }
}
