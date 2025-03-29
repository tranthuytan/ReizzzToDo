namespace ReizzzToDo.DAL.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int PreferredTimeUnitId { get; set; } = (int) ReizzzToDo.DAL.Enums.TimeUnit.Minutes;
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<ToDo>? ToDos { get; set; }
    }
}
