using Microsoft.EntityFrameworkCore;

namespace ReizzzToDo.DAL.Entities
{
    public class ReizzzToDoContext : DbContext
    {
        public ReizzzToDoContext()
        {
        }
        public ReizzzToDoContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TimeUnit> TimeUnits { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ToDo>(entity =>
            {
                entity.HasKey("Id");
                entity.ToTable("to_dos");
                entity.HasOne<User>(td => td.User)
                    .WithMany(u => u.ToDos)
                    .HasForeignKey(td => td.UserId)
                    .HasConstraintName("FK_todo_user");
                entity.HasOne<TimeUnit>(td => td.TimeUnit)
                    .WithMany(tu => tu.ToDos)
                    .HasForeignKey(td => td.TimeUnitId)
                    .HasConstraintName("FK_todo_time_unit");
            });
            builder.Entity<TimeUnit>(entity =>
            {
                List<TimeUnit> timeUnits = Enum.GetValues<Enums.TimeUnit>().Select(tu => new TimeUnit
                {
                    Id = (int)tu,
                    Name = tu.ToString()
                }).ToList();
                entity.HasKey("Id");
                entity.HasData(timeUnits);
                entity.ToTable("time_units");
            });
            builder.Entity<User>(entity =>
            {
                entity.HasKey("Id");
                entity.ToTable("users");
            });
            builder.Entity<Role>(entity =>
            {
                List<Role> roles = Enum.GetValues<Enums.Role>().Select(r => new Role
                {
                    Id = (long)r,
                    Name = r.ToString()
                }).ToList();
                entity.HasKey("Id");
                entity.HasData(roles);
                entity.ToTable("roles");
            });
            builder.Entity<UserRole>(entity =>
            {
                entity.HasKey("UserId", "RoleId");
                entity.ToTable("user_roles");
            });

        }
    }
}
