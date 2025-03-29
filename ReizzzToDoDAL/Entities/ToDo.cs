namespace ReizzzToDo.DAL.Entities
{
    public class ToDo
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public float TimeToComplete { get; set; } = 0;
        public int? TimeUnitId { get; set; }
        public long? UserId { get; set; }
        public virtual TimeUnit? TimeUnit { get; set; }
        public virtual User? User { get; set; }
    }
}
