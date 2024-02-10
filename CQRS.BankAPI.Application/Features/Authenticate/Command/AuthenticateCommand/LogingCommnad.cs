using System.Windows.Input;
using CQRS.BankAPI.Application.Abstractions.Messaging;

namespace CQRS.BankAPI.Application.Features.Authenticate.Command.AuthenticateCommand;

public record LoginCommand(string Email, string Password) : ICommand<string>;