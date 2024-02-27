
namespace CQRS.BankAPI.Application.Interfaces
{
    public interface ITokenHandler<TResponse>
    {
        Task<TResponse> Handle(CancellationToken cancellationToken);
    }
}