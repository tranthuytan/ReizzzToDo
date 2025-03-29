using ReizzzToDo.BAL.Filters;
using ReizzzToDo.BAL.ViewModels.Commons;
using ReizzzToDo.BAL.ViewModels.ResultViewModels;
using ReizzzToDo.BAL.ViewModels.ToDoViewModels;

namespace ReizzzToDo.BAL.Services.ToDoServices
{
    public interface IToDoService
    {
        public Task<ResultViewModel> Add(ToDoAddViewModel toDoVM);
        public Task<ToDoGetViewModel?> GetById(long id);
        public Task<ToDoGetAllViewModel> GetAll(ToDoFilter filter);
        public Task<ToDoGetAllViewModel> GetPagination(ToDoFilter filter, GetRequestViewModel request);
        public Task<ResultViewModel> DeleteById(long id);
        public Task<ResultViewModel> Update(ToDoUpdateViewModel toDoVM);

    }
}
