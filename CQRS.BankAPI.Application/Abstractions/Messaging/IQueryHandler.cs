using CQRS.BankAPI.Domain.Abstractions;
using MediatR;

namespace CQRS.BankAPI.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
: IRequestHandler<TQuery, Result<TResponse>>
where TQuery : IQuery<TResponse>
{

}