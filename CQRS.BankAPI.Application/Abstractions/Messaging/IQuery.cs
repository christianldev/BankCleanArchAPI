using CQRS.BankAPI.Domain.Abstractions;
using MediatR;

namespace CQRS.BankAPI.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}