using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReizzzToDo.BAL.Enums;
using ReizzzToDo.BAL.Filters;
using ReizzzToDo.BAL.Services.ToDoServices;
using ReizzzToDo.BAL.Services.Utils.Authentication.RoleHandler;
using ReizzzToDo.BAL.Validators.ToDoValidator;
using ReizzzToDo.BAL.ViewModels.ResultViewModels;
using ReizzzToDo.BAL.ViewModels.ToDoViewModels;

namespace ReizzzToDo.Controllers
{
    [Route("api/to-dos")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        /// <summary>
        /// User create a new ToDo
        /// </summary>
        /// <param name="toDoVM">Object Type: ToDoAddViewModel</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Post:
        ///     {
        ///         "name":"Complete the X module",
        ///     }
        ///     
        /// Return status code and object type: ResultViewModel
        /// 
        ///     {
        ///         "isSuccess": bool,
        ///         "Message": string,
        ///         "Error": List &lt;string&gt;
        ///     }
        /// </remarks>
        /// <response code = "200">ToDo created successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpPost()]
        [HasRole(Role.Registered)]
        public async Task<IActionResult> AddToDo(ToDoAddViewModel toDoVM)
        {
            var result = new ResultViewModel();

            var validator = new ToDoAddViewModelValidator();
            var validation = validator.Validate(toDoVM);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    result.Errors.Add(error.ErrorMessage);
                }
                return BadRequest(result);
            }
            result = await _toDoService.Add(toDoVM);
            return Ok(result);
        }

        /// <summary>
        /// User get all their ToDo
        /// </summary>
        /// <param name="filter">Object Type: ToDoFilter</param>
        /// <remarks>
        /// ToDoFilter:
        /// 
        ///     {
        ///         "name": string?, (to get all ToDo that contains the keyword)
        ///         "isCompleted": bool? (to get all ToDo that equal the value)
        ///     }
        /// 
        /// Sample request:
        /// 
        ///     Get:
        ///     {
        ///         "name":null,
        ///         "isCompleted": null
        ///     }
        ///     
        /// Return status code and object type: ToDoGetAllViewModel
        /// 
        ///     {
        ///         "isPaginated": bool,
        ///         "page": int,
        ///         "pageSize": int,
        ///         "pageCount": int,
        ///         "Data": List &lt;ToDoGetViewModel&gt;
        ///     }
        /// 
        /// ToDoGetViewModel:
        ///     
        ///     {
        ///         "id": long,
        ///         "name": string,
        ///         "isCompleted": bool,
        ///         "TimeToComplete": float,
        ///         "TimeUnit": string
        ///     }
        ///     
        /// </remarks>
        /// <response code = "200">Get all ToDo successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpGet()]
        [HasRole(Role.Registered)]
        public async Task<IActionResult> GetAllToDo([FromQuery]ToDoFilter filter)
        {
            var result = new ToDoGetAllViewModel();

            result = await _toDoService.GetAll(filter);
            if (result.Data.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// User get all their own ToDo
        /// </summary>
        /// <param name="toDoId">Object Type: long</param>
        /// <remarks>
        ///     
        /// Return status code and object type: ToDoGetViewModel
        /// 
        ///     {
        ///         "id": long,
        ///         "name": string,
        ///         "isCompleted": bool,
        ///         "timeToComplete": float,
        ///         "TimeUnit": string;
        ///         "Error": string
        ///     }
        /// </remarks>
        /// <response code = "200">Get all ToDo successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpGet("{toDoId}")]
        [HasRole(Role.Registered)]
        public async Task<IActionResult> GetToDoById(long toDoId)
        {
            var result = new ToDoGetViewModel();

            result = await _toDoService.GetById(toDoId);
            if (result == null)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// User update their own ToDo
        /// </summary>
        /// <param name="toDoUpdateVM">Object Type: ToDoUpdateViewModel</param>
        /// <remarks>
        ///     
        /// ToDoUpdateViewModel:
        /// 
        ///     {
        ///         "id": string,
        ///         "name": string?,
        ///         "isCompleted": bool?
        ///     }
        /// 
        /// Sample request:
        /// 
        ///     Put:
        ///     {
        ///         "name": "Updated name",
        ///         "isCompleted": null
        ///     }
        ///     
        ///     {
        ///         "name": null,
        ///         "isCompleted": true
        ///     }
        /// Return status code and object type: ResultViewModel
        /// 
        ///     {
        ///         "isSuccess": bool,
        ///         "message": string,
        ///         "errors": List&lt;string&gt;,
        ///     }
        /// </remarks>
        /// <response code = "200">Update ToDo successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpPut()]
        [HasRole(Role.Registered)]
        public async Task<IActionResult> UpdateToDo(ToDoUpdateViewModel toDoUpdateVM)
        {
            var result = new ResultViewModel();
            var validator = new ToDoUpdateViewModelValidator();
            var validation = validator.Validate(toDoUpdateVM);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    result.Errors.Add(error.ErrorMessage);
                }
                return BadRequest(result);
            }
            result = await _toDoService.Update(toDoUpdateVM);
            return Ok(result);
        }

        /// <summary>
        /// User delete their own ToDo
        /// </summary>
        /// <param name="toDoId">Object Type: long</param>
        /// <remarks>
        /// 
        /// Return status code and object type: ResultViewModel
        /// 
        ///     {
        ///         "isSuccess": bool,
        ///         "message": string,
        ///         "errors": List&lt;string&gt;,
        ///     }
        /// </remarks>
        /// <response code = "200">Update ToDo successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpDelete("{toDoId}")]
        [HasRole(Role.Registered)]
        public async Task<IActionResult> DeleteToDoById(long toDoId)
        {
            var result = await _toDoService.DeleteById(toDoId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        /// <summary>
        /// User create a new ToDo (the user must be logged in)
        /// </summary>
        /// <param name="toDoVM">Object Type: ToDoAddViewModel</param>
        /// <remarks>
        /// Sample request:
        /// 
        ///     Post:
        ///     {
        ///         "name":"Complete the X module",
        ///     }
        ///     
        /// Return status code and object type: ResultViewModel
        /// 
        ///     {
        ///         "isSuccess": bool,
        ///         "Message": string,
        ///         "Error": List &lt;string&gt;
        ///     }
        /// </remarks>
        /// <response code = "200">ToDo created successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpPost("add-to-do-with-authorize()")]
        [Authorize()]
        public async Task<IActionResult> AddToDoWithAuthorize(ToDoAddViewModel toDoVM)
        {
            var result = new ResultViewModel();

            var validator = new ToDoAddViewModelValidator();
            var validation = validator.Validate(toDoVM);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    result.Errors.Add(error.ErrorMessage);
                }
                return BadRequest(result);
            }
            result = await _toDoService.Add(toDoVM);
            return Ok(result);
        }

        /// <summary>
        /// User get all their ToDo (the user must have 2 role: Admin and Registered)
        /// </summary>
        /// <param name="filter">Object Type: ToDoFilter</param>
        /// <remarks>
        /// ToDoFilter:
        /// 
        ///     {
        ///         "name": string?, (to get all ToDo that contains the keyword)
        ///         "isCompleted": bool? (to get all ToDo that equal the value)
        ///     }
        /// 
        /// Sample request:
        /// 
        ///     Get:
        ///     {
        ///         "name":null,
        ///         "isCompleted": null
        ///     }
        ///     
        /// Return status code and object type: ToDoGetAllViewModel
        /// 
        ///     {
        ///         "isPaginated": bool,
        ///         "page": int,
        ///         "pageSize": int,
        ///         "pageCount": int,
        ///         "Data": List &lt;ToDoGetViewModel&gt;
        ///     }
        /// 
        /// ToDoGetViewModel:
        ///     
        ///     {
        ///         "id": long,
        ///         "name": string,
        ///         "isCompleted": bool,
        ///         "TimeToComplete": float,
        ///         "TimeUnit": string
        ///     }
        ///     
        /// </remarks>
        /// <response code = "200">Get all ToDo successful</response>
        /// <response code = "400">Invalid input</response>
        [HttpGet("with-2-roles")]
        [HasRole(Role.Registered)]
        [HasRole(Role.Admin)]
        public async Task<IActionResult> GetAllToDoWithMoreAuthorization(ToDoFilter filter)
        {
            var result = new ToDoGetAllViewModel();

            result = await _toDoService.GetAll(filter);
            if (result.Data.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
