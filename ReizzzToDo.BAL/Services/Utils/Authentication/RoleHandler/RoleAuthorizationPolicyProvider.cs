using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ReizzzToDo.BAL.Services.Utils.Authentication.RoleHandler
{
    public class RoleAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public RoleAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);
            if (policy !=null)
            {
                return policy;
            }
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new RoleRequirement(policyName))
                .Build();
        }
    }
}
