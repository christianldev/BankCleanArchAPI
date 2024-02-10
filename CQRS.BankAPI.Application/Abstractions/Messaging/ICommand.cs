using CQRS.BankAPI.Domain.Abstractions;
using MediatR;

namespace CQRS.BankAPI.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result>, IBaseCommand
    {

    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {

    }

    public interface IBaseCommand
    {
    }
}
