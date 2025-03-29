using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReizzzToDo.BAL.Services.UserServices;
using ReizzzToDo.BAL.Validators.UserValidator;
using ReizzzToDo.BAL.ViewModels.ResultViewModels;
using ReizzzToDo.BAL.ViewModels.UserViewModels;

namespace ReizzzToDo.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// User register an account
        /// </summary>
        /// <param name="userVM">Object Type: UserAddViewModel</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Post:
        ///     {
        ///         "username":"admin",
        ///         "password":"hehe123",
        ///         "name":"The name is Admin",
        ///         "preferredTimeUnit": null
        ///     }
        ///     
        ///     preferredTimeUnit is an int with value = 2 (Minutes) if not specify
        ///     1: Seconds
        ///     2: Minutes
        ///     3: Hours
        ///     
        /// Return status code and object type: ResultViewModel
        /// 
        ///     {
        ///         "isSuccess": bool,
        ///         "Message": string,
        ///         "Error": List &lt;string&gt;
        ///     }
        /// </remarks>
        /// <response code = "200">User registered successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserAddViewModel userVM)
        {
            var result = new ResultViewModel();

            UserAddViewModelValidator validator = new();
            var validation = validator.Validate(userVM);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    result.Errors.Add(error.ErrorMessage);
                    return BadRequest(result);
                }
            }
            result = await _userService.Register(userVM);
            if (result.IsSuccess) 
                return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// User login to the app
        /// </summary>
        /// <param name="userVM">Object Type: UserLoginViewModel</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Post:
        ///     {
        ///         "username":"admin",
        ///         "password":"hehe123"
        ///     }
        /// Return status code and object type: LoginResultViewModel
        /// 
        ///     {
        ///         "jwt": string
        ///         "isSuccess": bool,
        ///         "Message": string,
        ///         "Error": List &lt;string&gt;
        ///     }
        /// </remarks>
        /// <returns>Return action result and error message (if any)</returns>
        /// <response code = "200">User login successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginViewModel userVM)
        {
            var result = new LoginResultViewModel();
            result = await _userService.Login(userVM);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
