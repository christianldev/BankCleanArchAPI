using FluentValidation;

namespace CQRS.BankAPI.Application.Features.Authenticate.Command.AuthenticateCommand
{
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {

            RuleFor(p => p.Email)
               .NotEmpty()
               .EmailAddress()
               .MaximumLength(100);

            RuleFor(p => p.Password)
             .NotEmpty()
            //ASP.NET Core identity default password policy with message customization
            .MinimumLength(6)
            .MaximumLength(20);

        }
    }
}
