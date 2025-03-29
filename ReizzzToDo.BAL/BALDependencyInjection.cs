using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using ReizzzToDo.BAL.Services.RoleService;
using ReizzzToDo.BAL.Services.ToDoServices;
using ReizzzToDo.BAL.Services.UserServices;
using ReizzzToDo.BAL.Services.Utils.Authentication;
using ReizzzToDo.BAL.Services.Utils.Authentication.RoleHandler;
using ReizzzToDo.BAL.Services.Utils.PasswordHasher;

namespace ReizzzToDo.BAL
{
    public static class BALDependencyInjection
    {
        public static IServiceCollection AddBAL(this IServiceCollection services)
        {
            AddServices(services);
            AddPolicy(services);
            return services;
        }

        public static void AddPolicy(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, RoleAuthorizationPolicyProvider>();
        }
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IToDoService, ToDoService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddSingleton<JwtProvider>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            // add HttpContextAccessor to have access to the jwt claims
            services.AddHttpContextAccessor();

            // add NewtonsoftJson to support serialize object that reference to another object
        }
    }
}
