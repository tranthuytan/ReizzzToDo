using ReizzzToDo.DAL.Entities;

namespace ReizzzToDo.BAL.ViewModels.UserRoleViewModels
{
    public class UserRoleViewModel
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public UserRole ToUserRole(UserRoleViewModel userRoleVM)
        {
            return new UserRole
            {
                UserId = userRoleVM.UserId,
                RoleId = userRoleVM.RoleId
            };
        }
    }
}
