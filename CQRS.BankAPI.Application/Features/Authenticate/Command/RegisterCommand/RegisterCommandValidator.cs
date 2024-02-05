using FluentValidation;

namespace CQRS.BankAPI.Application.Features.Authenticate.Command.RegisterCommand
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(p => p.FirstName)
               .NotEmpty()
               .MaximumLength(80);

            RuleFor(p => p.LastName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(p => p.Email)
               .NotEmpty()
               .EmailAddress()
               .MaximumLength(100);

            RuleFor(p => p.UserName)
                .NotEmpty()
               .MaximumLength(100);

            RuleFor(p => p.Password)
             .NotEmpty()
            //ASP.NET Core identity default password policy with message customization
            .MinimumLength(6)
            .Must(p => p.Any(char.IsDigit)).WithMessage("Contrasena debe contener al menos un digito.")
            .Must(p => p.Any(char.IsLower)).WithMessage("Contrasena debe contener al menos una letra minuscula.")
            .Must(p => p.Any(char.IsUpper)).WithMessage("Contrasena debe contener al menos una letra mayuscula.")
            .Matches("[^A-Za-z0-9]+.{8}").WithMessage("Contrasena debe contener al menos un caracter especial.")
            .MaximumLength(20);

            RuleFor(p => p.ConfirmPassword)
           .Equal(p => p.Password);
        }
    }
}
