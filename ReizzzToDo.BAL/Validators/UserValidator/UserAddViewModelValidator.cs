using FluentValidation;
using ReizzzToDo.BAL.ViewModels.UserViewModels;

namespace ReizzzToDo.BAL.Validators.UserValidator
{
    public class UserAddViewModelValidator : AbstractValidator<UserAddViewModel>
    {
        public UserAddViewModelValidator()
        {
            RuleFor(u => u.Username)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(16);
            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(15);
        }
    }
}
