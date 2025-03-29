using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Repositories.BaseRepository;

namespace ReizzzToDo.DAL.Repositories.UserRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<HashSet<string>> GetUserWithRole(long userId);
    }
}
