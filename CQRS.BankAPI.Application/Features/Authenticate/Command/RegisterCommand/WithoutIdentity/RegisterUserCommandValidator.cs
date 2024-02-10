using CQRS.BankAPI.Domain;
using FluentValidation;

namespace CQRS.BankAPI.Application.Features.Authenticate.Command.RegisterCommand.WithoutIdentity;

internal sealed class RegisterUserCommandValidator
    : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.Name)
        .NotEmpty().WithMessage("El nombre no puede ser nulo");
        RuleFor(c => c.LastName)
        .NotEmpty().WithMessage("El apellido no puede ser nulo");
        RuleFor(c => c.Dni)
        .NotEmpty().WithMessage("El DNI no puede ser nulo");
        RuleFor(c => c.Dni).Must(Dni.IsValid).WithMessage("El DNI no es válido");
        RuleFor(c => c.PhoneNumber)
        .NotEmpty().WithMessage("El número de teléfono no puede ser nulo");
        RuleFor(c => c.Province).NotEmpty().WithMessage("La provincia no puede ser nula");
        RuleFor(c => c.City).NotEmpty().WithMessage("La ciudad no puede ser nula");
        RuleFor(c => c.District).NotEmpty().WithMessage("El distrito no puede ser nulo");
        RuleFor(c => c.Email).EmailAddress();
        RuleFor(c => c.PasswordHash).NotEmpty().MinimumLength(6).WithMessage("La contraseña no puede ser nula");
        RuleFor(c => c.IpUser).NotEmpty().WithMessage("La dirección IP no puede ser nula");
        RuleFor(c => c.UserStatus).NotEmpty().WithMessage("El estado del usuario no puede ser nulo");
    }
}