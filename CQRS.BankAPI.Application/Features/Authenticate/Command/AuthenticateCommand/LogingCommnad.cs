using System.Windows.Input;
using CQRS.BankAPI.Application.Abstractions.Messaging;
using CQRS.BankAPI.Application.DTOS.Response;

namespace CQRS.BankAPI.Application.Features.Authenticate.Command.AuthenticateCommand;

public record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;