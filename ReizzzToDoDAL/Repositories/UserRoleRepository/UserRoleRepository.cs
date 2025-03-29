using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Repositories.BaseRepository;

namespace ReizzzToDo.DAL.Repositories.UserRoleRepository
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ReizzzToDoContext context) : base(context)
        {
        }
    }
}
