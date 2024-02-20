using CQRS.BankAPI.Application.Abstractions.Messaging;
using CQRS.BankAPI.Application.DTOS.Response;

namespace CQRS.BankAPI.Application.Features.Users.Commands.AuthenticateUser;

public record GetUserByTokenCommand(string AuthToken) : ICommand<UserResponse>;