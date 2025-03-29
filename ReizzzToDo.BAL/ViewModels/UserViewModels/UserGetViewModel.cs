using ReizzzToDo.BAL.ViewModels.RoleViewModels;
using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.ViewModels.UserViewModels
{
    public class UserGetViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int PreferredTimeUnitId { get; set; } = (int)ReizzzToDo.DAL.Enums.TimeUnit.Minutes;
        public List<RoleGetViewModel> Roles { get; set; } = new List<RoleGetViewModel>();
        public UserGetViewModel FromUser(User user)
        {
            var result = new UserGetViewModel
            {
                Id = user.Id,
                UserName = user.Username,
                Name = user.Name,
                PreferredTimeUnitId = user.PreferredTimeUnitId
            };
            RoleGetViewModel roleVM = new();
            foreach (var userRole in user.UserRoles)
            {
                roleVM = roleVM.FromRole(userRole.Role!);
                result.Roles.Add(roleVM);
            }
            return result;
        }
    }
}
