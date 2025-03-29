using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.Filters
{
    public class ToDoFilter
    {
        public string? Name { get; set; } = string.Empty;
        public bool? IsCompleted { get; set; } = false;
    }
}
