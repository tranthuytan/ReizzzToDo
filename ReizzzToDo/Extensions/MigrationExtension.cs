using Microsoft.EntityFrameworkCore;
using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ReizzzToDoContext context = scope.ServiceProvider.GetRequiredService<ReizzzToDoContext>();
            context.Database.Migrate();
        }
    }
}
