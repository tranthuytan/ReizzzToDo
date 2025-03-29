using Microsoft.EntityFrameworkCore;
using ReizzzToDo.BAL.Services.Utils.Authentication;
using ReizzzToDo.BAL.Services.Utils.PasswordHasher;
using ReizzzToDo.BAL.ViewModels.Commons;
using ReizzzToDo.BAL.ViewModels.ResultViewModels;
using ReizzzToDo.BAL.ViewModels.UserRoleViewModels;
using ReizzzToDo.BAL.ViewModels.UserViewModels;
using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Repositories.UserRepository;
using ReizzzToDo.DAL.Repositories.UserRoleRepository;

namespace ReizzzToDo.BAL.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtProvider _jwtProvider;
        private readonly IUserRoleRepository _userRoleRepository;
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher,
            JwtProvider jwtProvider,
            IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<GetAllResponseViewModel<UserGetViewModel>> GetAllUsers()
        {
            var result = new GetAllResponseViewModel<UserGetViewModel>();
            var users = await _userRepository.GetAll(includeFunc: u => u.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));
            UserGetViewModel userVM = new();
            foreach (var user in users)
            {
                result.Data.Add(userVM.FromUser(user));
            }
            result.Message = string.Format("Get {0} users", result.Data.Count());
            result.IsPaginated = false;
            result.Page = 1;
            result.PageSize = result.Data.Count();
            return result;
        }
        public async Task<HashSet<string>> GetUserRoles(long userId)
        {
            var result = await _userRepository.GetUserWithRole(userId);
            return result;
        }
        public async Task<LoginResultViewModel> Login(UserLoginViewModel userVM)
        {
            var result = new LoginResultViewModel();
            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(u => u.Username == userVM.UserName);
                if (user == null)
                {
                    result.Message = "There's no user with that username";
                    return result;
                }

                bool isPasswordCorrect = _passwordHasher.Verify(userVM.Password, user.Password);
                if (!isPasswordCorrect)
                {
                    result.Message = "Wrong password";
                    return result;
                }
                // create a jwt and return it
                result.Jwt = _jwtProvider.Generate(user);
                result.IsSuccess = true;
                result.Message = "Login successfully";
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }

            return result;
        }

        public async Task<ResultViewModel> Register(UserAddViewModel userVM)
        {
            var result = new ResultViewModel();
            try
            {
                // check for duplicated userName
                var existedUser = await _userRepository.FirstOrDefaultAsync(u => u.Username == userVM.Username);
                if (existedUser != null)
                {
                    result.Message = "Duplicated username, please choose another one";
                    return result;
                }

                // hash password
                userVM.Password = _passwordHasher.Hash(userVM.Password);

                // add user to the database
                var userToAdd = userVM.ToUser(userVM);
                await _userRepository.AddAsync(userToAdd);
                await _userRepository.SaveChangesAsync();
                result.IsSuccess = true;
                result.Message = "User created successfully";
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }
            return result;


        }

        public async Task<ResultViewModel> AddUserRole(UserRoleViewModel userRoleVM)
        {
            var result = new ResultViewModel();
            try
            {
                UserRole? existedUserRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.RoleId == userRoleVM.RoleId && ur.UserId == userRoleVM.UserId);
                if (existedUserRole != null)
                {
                    result.Errors.Add("This user has that role already");
                    return result;
                }
                UserRole userRoleToAdd = userRoleVM.ToUserRole(userRoleVM);
                await _userRoleRepository.AddAsync(userRoleToAdd);
                await _userRoleRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
            result.IsSuccess = true;
            result.Message = "Add role for user successful";
            return result;
        }
        public async Task<ResultViewModel> DeleteUserRole(UserRoleViewModel userRoleVM)
        {
            var result = new ResultViewModel();
            try
            {
                UserRole? userRoleToDelete = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.RoleId == userRoleVM.RoleId && ur.UserId == userRoleVM.UserId);
                if (userRoleToDelete == null)
                {
                    result.Errors.Add("This user doesn't have that role");
                    return result;
                }
                _userRoleRepository.Delete(userRoleToDelete);
                await _userRoleRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
            result.IsSuccess = true;
            result.Message = "Delete user role successfully";
            return result;
        }
    }
}
