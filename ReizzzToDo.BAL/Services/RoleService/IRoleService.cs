using ReizzzToDo.BAL.ViewModels.Commons;
using ReizzzToDo.BAL.ViewModels.RoleViewModels;

namespace ReizzzToDo.BAL.Services.RoleService
{
    public interface IRoleService
    {
        public Task<GetAllResponseViewModel<RoleGetViewModel>> GetAllRoles();
    }
}
