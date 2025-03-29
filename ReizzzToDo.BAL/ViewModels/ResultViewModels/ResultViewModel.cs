using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReizzzToDo.BAL.ViewModels.ResultViewModels
{
    public class ResultViewModel
    {
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
