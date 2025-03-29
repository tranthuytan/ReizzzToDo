using FluentValidation;
using ReizzzToDo.BAL.ViewModels.ToDoViewModels;
namespace ReizzzToDo.BAL.Validators.ToDoValidator
{
    public class ToDoAddViewModelValidator : AbstractValidator<ToDoAddViewModel>
    {
        public ToDoAddViewModelValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(td => td.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2);
        }
    }
}
