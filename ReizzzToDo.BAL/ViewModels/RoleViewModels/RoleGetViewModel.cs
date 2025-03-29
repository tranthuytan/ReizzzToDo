using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.ViewModels.RoleViewModels
{
    public class RoleGetViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public RoleGetViewModel FromRole(Role role)
        {
            return new RoleGetViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}
