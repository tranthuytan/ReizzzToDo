using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.ViewModels.ToDoViewModels
{
    public class ToDoUpdateViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
