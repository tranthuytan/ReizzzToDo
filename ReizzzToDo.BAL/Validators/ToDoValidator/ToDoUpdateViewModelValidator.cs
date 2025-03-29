

using FluentValidation;
using ReizzzToDo.BAL.ViewModels.ToDoViewModels;

namespace ReizzzToDo.BAL.Validators.ToDoValidator
{
    public class ToDoUpdateViewModelValidator : AbstractValidator<ToDoUpdateViewModel>
    {
        public ToDoUpdateViewModelValidator()
        {
            RuleFor(td => td.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2);
            RuleFor(td => td.IsCompleted)
                .NotNull();
        }
    }
}
