using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Repositories.BaseRepository;

namespace ReizzzToDo.DAL.Repositories.ToDoRepository
{
    public class ToDoRepository : BaseRepository<ToDo>, IToDoRepository
    {
        public ToDoRepository(ReizzzToDoContext context) : base(context)
        {
        }
    }
}
