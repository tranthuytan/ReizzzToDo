using Microsoft.IdentityModel.Tokens;
using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.ViewModels.UserViewModels
{
    public class UserAddViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? PreferredTimeUnitId { get; set; }

        public User ToUser(UserAddViewModel userVM)
        {
            var result = new User
            {
                Username = userVM.Username,
                Password = userVM.Password,
                Name = userVM.Name,
            };
            if (PreferredTimeUnitId != null)
            {
                result.PreferredTimeUnitId = (int)userVM.PreferredTimeUnitId!;
            }
            return result;
        }
    }
}
