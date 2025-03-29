using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReizzzToDo.BAL.ViewModels.ResultViewModels
{
    public class LoginResultViewModel : ResultViewModel
    {
        public string Jwt { get; set; } = string.Empty;
    }
}
