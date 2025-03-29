using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.ViewModels.ToDoViewModels
{
    public class ToDoAddViewModel
    {
        public string Name { get; set; } = string.Empty;
        public ToDo ToToDo(ToDoAddViewModel toDoVM)
        {
            return new ToDo
            {
                Name = toDoVM.Name
            };
        }
    }
}
