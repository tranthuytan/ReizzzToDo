using ReizzzToDo.BAL.ViewModels.Commons;
using ReizzzToDo.BAL.ViewModels.RoleViewModels;
using ReizzzToDo.DAL.Repositories.RoleRepository;

namespace ReizzzToDo.BAL.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<GetAllResponseViewModel<RoleGetViewModel>> GetAllRoles()
        {
            var result = new GetAllResponseViewModel<RoleGetViewModel>();
            var roles = await _roleRepository.GetAll();
            foreach (var role in roles)
            {
                RoleGetViewModel roleVM = new();
                result.Data.Add(roleVM.FromRole(role));
            }
            result.IsPaginated = false;
            result.Message = string.Format("Get {0} roles", result.Data.Count);
            result.Page = 1;
            result.PageSize = result.Data.Count();
            return result;
        }
    }
}
