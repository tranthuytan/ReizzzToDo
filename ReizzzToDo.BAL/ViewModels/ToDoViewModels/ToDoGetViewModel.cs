using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.ViewModels.ToDoViewModels
{
    public class ToDoGetViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public float TimeToComplete { get; set; }
        public string? TimeUnit { get; set; }
        public string Error { get; set; } = string.Empty;

        public ToDoGetViewModel FromToDo(ToDo toDo)
        {
            var toDoVM = new ToDoGetViewModel
            {
                Id = toDo.Id,
                Name = toDo.Name,
                IsCompleted = toDo.IsCompleted,
                TimeToComplete = toDo.TimeToComplete,
                TimeUnit = toDo.TimeUnit?.Name
            };
            return toDoVM;
        }
    }
}
