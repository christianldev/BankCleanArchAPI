
namespace CQRS.BankAPI.Application.Features.Users.Commands.AuthenticateUser
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading;
    using System.Threading.Tasks;
    using CQRS.BankAPI.Application.Abstractions.Messaging;
    using CQRS.BankAPI.Application.Authentication;
    using CQRS.BankAPI.Application.DTOS.Request;
    using CQRS.BankAPI.Application.DTOS.Response;
    using CQRS.BankAPI.Domain.Abstractions;
    using MediatR;

    public class GetUserByTokenCommandHandler : IRequestHandler<GetUserByTokenCommand, Result<UserResponse>>
    {
        private readonly IJwtProvider _jwtProvider;

        public GetUserByTokenCommandHandler(IJwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByTokenCommand request, CancellationToken cancellationToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(request.AuthToken);

            var userId = jwtToken.Claims.First(claim => claim.Type == "sub").Value;
            var email = jwtToken.Claims.First(claim => claim.Type == "email").Value;

            return Result.Success(new UserResponse { UserId = userId, Email = email });
        }
    }
}
