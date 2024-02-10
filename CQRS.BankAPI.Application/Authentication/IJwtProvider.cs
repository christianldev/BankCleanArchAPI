using CQRS.BankAPI.Domain.Users;

namespace CQRS.BankAPI.Application.Authentication;

public interface IJwtProvider
{

    Task<string> Generate(User user);

}