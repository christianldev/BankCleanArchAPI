
using CQRS.BankAPI.Application.Abstractions.Messaging;

namespace CQRS.BankAPI.Application.Features.Authenticate.Command.RegisterCommand.WithoutIdentity;

public sealed record RegisterUserCommand(
    string Name,
    string LastName,
    string Dni,
    string PhoneNumber,
    string Province,
    string City,
    string District,
    string Email,
    string PasswordHash,
    string ConfirmPassword,
    string IpUser,
    string UserStatus) : ICommand<Guid>;
