namespace ReizzzToDo.DAL.Entities
{
    public class TimeUnit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<ToDo>? ToDos { get; set; }
    }
}
