using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReizzzToDo.DAL.Entities
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
