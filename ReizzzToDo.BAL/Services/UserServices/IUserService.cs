using ReizzzToDo.BAL.ViewModels.Commons;
using ReizzzToDo.BAL.ViewModels.ResultViewModels;
using ReizzzToDo.BAL.ViewModels.UserRoleViewModels;
using ReizzzToDo.BAL.ViewModels.UserViewModels;

namespace ReizzzToDo.BAL.Services.UserServices
{
    public interface IUserService
    {
        public Task<ResultViewModel> Register(UserAddViewModel userVM);
        public Task<LoginResultViewModel> Login(UserLoginViewModel userVM);
        public Task<HashSet<string>> GetUserRoles(long userId);
        public Task<GetAllResponseViewModel<UserGetViewModel>> GetAllUsers();
        public Task<ResultViewModel> AddUserRole(UserRoleViewModel userRoleVM);
        public Task<ResultViewModel> DeleteUserRole(UserRoleViewModel userRoleVM);
    }
}
