
namespace CQRS.BankAPI.Application.Features.Users.Commands.AuthenticateUser
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading;
    using System.Threading.Tasks;
    using CQRS.BankAPI.Application.DTOS.Response;
    using CQRS.BankAPI.Application.Interfaces;
    using CQRS.BankAPI.Domain.Abstractions;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class GetUserByTokenCommandHandler : ITokenHandler<Result<UserResponse>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserByTokenCommandHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Result<UserResponse>> Handle(CancellationToken cancellationToken)
        {
            var jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userId = token.Claims.First(claim => claim.Type == "sub").Value;
            var email = token.Claims.First(claim => claim.Type == "email").Value;

            return Result.Success(new UserResponse { UserId = userId, Email = email });

        }
    }
}
