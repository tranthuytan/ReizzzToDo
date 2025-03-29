using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReizzzToDo.BAL.Common;
using ReizzzToDo.BAL.Extensions;
using ReizzzToDo.BAL.Filters;
using ReizzzToDo.BAL.ViewModels.Commons;
using ReizzzToDo.BAL.ViewModels.ResultViewModels;
using ReizzzToDo.BAL.ViewModels.ToDoViewModels;
using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Filter;
using ReizzzToDo.DAL.Repositories.ToDoRepository;
using ReizzzToDo.DAL.Repositories.UserRepository;

namespace ReizzzToDo.BAL.Services.ToDoServices
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public ToDoService(IToDoRepository toDoRepository,
                            IHttpContextAccessor httpContextAccessor,
                            IUserRepository userRepository)
        {
            _toDoRepository = toDoRepository;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<ResultViewModel> Add(ToDoAddViewModel toDoVM)
        {
            ResultViewModel result = new();
            try
            {
                ToDo toDoToAdd = toDoVM.ToToDo(toDoVM);

                long creatorId = _httpContextAccessor.GetJwtSubValue();

                toDoToAdd.UserId = creatorId;

                User? creator = await _userRepository.FirstOrDefaultAsync(u => u.Id == creatorId);
                if (creator == null)
                {
                    throw new ApplicationException(ErrorMessage.UserIdNotMatchWithAnyUser);
                }
                toDoToAdd.TimeUnitId = creator.PreferredTimeUnitId;

                await _toDoRepository.AddAsync(toDoToAdd);
                await _toDoRepository.SaveChangesAsync();

                result.IsSuccess = true;
                result.Message = ToDoMessage.AddToDoSuccess;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }
            return result;
        }
        public async Task<ResultViewModel> DeleteById(long id)
        {
            ResultViewModel result = new();
            try
            {
                long userId = _httpContextAccessor.GetJwtSubValue();
                var toDoToDelete = await _toDoRepository.GetById(id);
                if (toDoToDelete == null)
                {
                    result.Errors.Add(string.Format(ErrorMessage.NoToDoWithThatId, id));
                    return result;
                }
                if (toDoToDelete.UserId != userId)
                {
                    result.Errors.Add(ErrorMessage.WrongUserAccessToDo);
                    return result;
                }
                _toDoRepository.Delete(toDoToDelete);
                await _toDoRepository.SaveChangesAsync();
                result.IsSuccess = true;
                result.Message = ToDoMessage.DeleteToDoSuccess;

            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }
            return result;
        }
        public async Task<ToDoGetAllViewModel> GetAll(ToDoFilter filter)
        {
            var result = new ToDoGetAllViewModel();
            var filterList = new Filter<ToDo>();
            try
            {
                long userId = _httpContextAccessor.GetJwtSubValue();

                filterList.Conditions.Add(td => td.UserId == userId);
                if (!filter.Name.IsNullOrEmpty())
                {
                    filterList.Conditions.Add(td => td.Name.ToLower().Contains(filter.Name!.ToLower()));
                }
                if (filter.IsCompleted != null)
                {
                    filterList.Conditions.Add(td => td.IsCompleted == filter.IsCompleted);
                }
                var toDos = await _toDoRepository.GetAll(filterList, includeFunc: td => td.Include(td => td.TimeUnit));

                List<ToDoGetViewModel> resultData = new List<ToDoGetViewModel>();
                foreach (var toDo in toDos)
                {
                    ToDoGetViewModel toDoVM = new();
                    resultData.Add(toDoVM.FromToDo(toDo));
                }
                result.Data = resultData;

                // GetAll() doesn't send Pagination information
                result.IsPaginated = false;
                result.Page = 1;
                result.PageSize = resultData.Count;
                result.Message = string.Format(ToDoMessage.GetAllToDo, resultData.Count);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<ToDoGetAllViewModel> GetPagination(ToDoFilter filter, GetRequestViewModel request)
        {

            var result = new ToDoGetAllViewModel();
            var filterList = new Filter<ToDo>();
            try
            {
                long userId = _httpContextAccessor.GetJwtSubValue();

                filterList.Conditions.Add(td => td.UserId == userId);
                if (!filter.Name.IsNullOrEmpty())
                {
                    filterList.Conditions.Add(td => td.Name.Contains(filter.Name!));
                }
                if (filter.IsCompleted != null)
                {
                    filterList.Conditions.Add(td => td.IsCompleted == filter.IsCompleted);
                }

                var toDos = await _toDoRepository.Pagination(filterList,
                    includeFunc: td => td.Include(td => td.TimeUnit),
                    request.Page,
                    request.PageSize);

                List<ToDoGetViewModel> resultData = new List<ToDoGetViewModel>();
                foreach (var toDo in toDos)
                {
                    ToDoGetViewModel toDoVM = new();
                    resultData.Add(toDoVM.FromToDo(toDo));
                }
                result.Data = resultData;

                // GetAll() doesn't send Pagination information
                result.IsPaginated = request.IsPaginated;
                result.Page = request.Page;
                result.PageSize = request.PageSize;
                result.Message = string.Format(ToDoMessage.GetAllToDo, resultData.Count);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<ToDoGetViewModel?> GetById(long id)
        {
            ToDoGetViewModel result = new();
            try
            {
                long userId = _httpContextAccessor.GetJwtSubValue();
                var toDo = await _toDoRepository.GetById(id);
                if (toDo == null)
                {
                    return null;
                }
                if (toDo.UserId != userId)
                {
                    throw new ArgumentException(ErrorMessage.WrongUserAccessToDo);
                }
                result = result.FromToDo(toDo);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<ResultViewModel> Update(ToDoUpdateViewModel toDoVM)
        {
            ResultViewModel result = new();
            try
            {
                long userId = _httpContextAccessor.GetJwtSubValue();
                var toDoToUpdate = await _toDoRepository.GetById(toDoVM.Id);
                if (toDoToUpdate == null)
                {
                    result.Errors.Add(ErrorMessage.NoToDoWithThatId);
                    return result;
                }
                if (toDoToUpdate.UserId != userId)
                {
                    result.Errors.Add(ErrorMessage.WrongUserAccessToDo);
                    return result;
                }
                if ((!toDoToUpdate.Name.Equals(toDoVM.Name) && !toDoVM.Name.IsNullOrEmpty()) && (toDoToUpdate.IsCompleted != toDoVM.IsCompleted && toDoVM.IsCompleted!=null))
                {
                    result.Errors.Add(ErrorMessage.ToDoCanBeUpdatedWithNameOrCompleteStateChanged);
                    return result;
                }
                toDoToUpdate.Name = toDoVM.Name ?? toDoToUpdate.Name;
                toDoToUpdate.IsCompleted = toDoVM.IsCompleted ?? toDoToUpdate.IsCompleted;
                _toDoRepository.Update(toDoToUpdate);
                await _toDoRepository.SaveChangesAsync();
                result.IsSuccess = true;
                result.Message = ToDoMessage.UpdateToDoSuccess;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }
            return result;
        }
    }
}
