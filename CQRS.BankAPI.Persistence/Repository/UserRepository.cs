
using CleanArchitecture.Infrastructure.Repositories;
using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Users;
using CQRS.BankAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CQRS.BankAPI.Persistence.Repository;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(AppBankDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
        .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> IsUserExists(
        Email email,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<User>()
        .AnyAsync(x => x.Email == email);
    }
}