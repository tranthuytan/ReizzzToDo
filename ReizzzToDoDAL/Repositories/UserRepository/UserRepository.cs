using Microsoft.EntityFrameworkCore;
using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Migrations;
using ReizzzToDo.DAL.Repositories.BaseRepository;

namespace ReizzzToDo.DAL.Repositories.UserRepository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ReizzzToDoContext context) : base(context)
        {
        }

        public async Task<HashSet<string>> GetUserWithRole(long userId)
        {
            ICollection<UserRole>[] userRoles = await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.Id == userId)
                .Select(u => u.UserRoles)
                .ToArrayAsync();

            return userRoles
                    .SelectMany(ur => ur)
                    .Select(ur => ur.Role)
                    .Select(r => r.Name).ToHashSet();
        }
    }
}
