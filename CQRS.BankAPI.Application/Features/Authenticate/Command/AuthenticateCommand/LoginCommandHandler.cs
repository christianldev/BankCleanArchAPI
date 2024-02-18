
using CQRS.BankAPI.Application.Abstractions.Messaging;
using CQRS.BankAPI.Application.Authentication;
using CQRS.BankAPI.Application.DTOS.Response;
using CQRS.BankAPI.Domain.Abstractions;
using CQRS.BankAPI.Domain.Entities.Users;
using MediatR;

namespace CQRS.BankAPI.Application.Features.Authenticate.Command.AuthenticateCommand;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokenResponse>>
{
    private readonly IUserRepository _userRepository;

    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        //1. Verificar que el usuario exista en la base de datos
        // buscar un usuario en la bd por el email
        var user = await _userRepository.GetByEmailAsync(new Email(request.Email), cancellationToken);
        if (user is null)
        {
            return Result.Failure<TokenResponse>(UserErrors.NotFound);
        }


        //2. Validar que el password es correcto
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash!.Value))
        {
            return Result.Failure<TokenResponse>(UserErrors.InvalidCredentials);
        }

        //3. Generar el JWT
        var token = await _jwtProvider.Generate(user);

        //4. Return jwt;
        return Result.Success(new TokenResponse { AccessToken = token });
    }
}