using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReizzzToDo.BAL.Services.RoleService;
using ReizzzToDo.BAL.Services.UserServices;
using ReizzzToDo.BAL.ViewModels.UserRoleViewModels;

namespace ReizzzToDo.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AdminController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        /// <summary>
        /// Get all Username and Name in the database
        /// </summary>
        /// <response code = "200">Return list of users</response>
        /// <response code = "404">No user in the database</response>
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUser()
        {
            var result = await _userService.GetAllUsers();
            if (result.Data.Any())
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        /// <summary>
        /// Get all Role in the database
        /// </summary>
        /// <response code = "200">Return list of roles</response>
        /// <response code = "404">No role in the database</response>
        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();
            if (result.Data.Any())
            {
                return Ok(result);

            }
            return NotFound(result);
        }
        /// <summary>
        /// Add role for user
        /// </summary>
        /// <param name="userRoleVM">Object type: UserRoleViewModel</param>
        /// <remarks>
        /// UserRoleViewModel:
        /// 
        ///     {
        ///         "userId": long,
        ///         "roleId": long
        ///     }
        /// 
        /// Sample request:
        /// 
        ///     Post:
        ///     {
        ///         "userId":1,
        ///         "roleId":2
        ///     }
        /// </remarks>
        /// <response code = "200">Add user role successfully</response>
        /// <response code = "400">Can't add user role</response>
        [HttpPost("add-user-role")]
        public async Task<IActionResult> AddUserRole([FromBody] UserRoleViewModel userRoleVM)
        {
            var result = await _userService.AddUserRole(userRoleVM);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Delete role for user
        /// </summary>
        /// <param name="userRoleVM">Object type: UserRoleViewModel</param>
        /// <remarks>
        /// UserRoleViewModel:
        /// 
        ///     {
        ///         "userId": long,
        ///         "roleId": long
        ///     }
        /// 
        /// Sample request:
        /// 
        ///     Delete:
        ///     {
        ///         "userId":1,
        ///         "roleId":2
        ///     }
        /// </remarks>
        /// <response code = "200">Delete user role successfully</response>
        /// <response code = "400">Can't delete user role</response>
        [HttpDelete("delete-user-role")]
        public async Task<IActionResult> DeleteUserRole([FromBody] UserRoleViewModel userRoleVM)
        {
            var result = await _userService.DeleteUserRole(userRoleVM);
            if (result.IsSuccess)
            {
                return Ok(result);

            }
            return BadRequest(result);
        }
    }
}
