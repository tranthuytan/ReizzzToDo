using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Repositories.BaseRepository;

namespace ReizzzToDo.DAL.Repositories.RoleRepository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ReizzzToDoContext context) : base(context)
        {
        }
    }
}
