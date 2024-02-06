using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Users;

namespace CQRS.BankAPI.Domain;

public interface IUserRepository
{

    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    void Add(User user);

    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> IsUserExists(
        Email email,
        CancellationToken cancellationToken = default
    );

}