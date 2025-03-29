using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Repositories.RoleRepository;
using ReizzzToDo.DAL.Repositories.ToDoRepository;
using ReizzzToDo.DAL.Repositories.UserRepository;
using ReizzzToDo.DAL.Repositories.UserRoleRepository;

namespace ReizzzToDo.DAL
{
    public static class DALDependencyInjection
    {
        public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            AddDatabase(services, configuration);
            AddRepositories(services);
            return services;
        }

        private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReizzzToDoContext>(options => options.UseSqlServer(configuration.GetConnectionString("LocalDb")));
        }
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IToDoRepository, ToDoRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        }
    }
}
