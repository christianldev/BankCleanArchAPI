using CQRS.BankAPI.Application.DTOS.Request;
using CQRS.BankAPI.Application.DTOS.Response;
using CQRS.BankAPI.Domain.Users;

namespace CQRS.BankAPI.Domain.Entities.Users;

public interface IUserRepository
{

    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    void Add(User user);

    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> IsUserExists(
        Email email,
        CancellationToken cancellationToken = default
    );

    Task<bool> IsDniExists(
        Dni dni,
        CancellationToken cancellationToken = default
    );


}