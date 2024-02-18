using CQRS.BankAPI.Domain.Abstractions;
using MediatR;

namespace CQRS.BankAPI.Application.Abstractions.Messaging;


public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
where TCommand : ICommand
{

}

// public interface ICommandHandler<TCommand, TResponse>
// : IRequestHandler<TCommand, Result<TResponse>>
// where TCommand : ICommand<TResponse>
// {

// }

public interface ICommandHandler<TCommand, TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}