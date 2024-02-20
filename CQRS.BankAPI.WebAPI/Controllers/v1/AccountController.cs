using CQRS.BankAPI.Application.DTOS.Request;
using CQRS.BankAPI.Application.Features.Authenticate.Command.AuthenticateCommand;
using CQRS.BankAPI.Application.Features.Authenticate.Command.RegisterCommand;
using CQRS.BankAPI.Application.Features.Authenticate.Command.RegisterCommand.WithoutIdentity;
using CQRS.BankAPI.Application.Features.Users.Commands.AuthenticateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.BankAPI.WebAPI.Controllers.v1
{
    [ApiVersion("1")]

    public class AccountController : BaseApiController
    {

        private readonly ISender _sender;

        public AccountController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest account)
        {

            return Ok(await Mediator!.Send(new AuthenticateCommand
            {
                Email = account.Email,
                Password = account.Password,
                IpAddress = GenerateIpAddress()
            }));

        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest account)
        {
            return Ok(await Mediator!.Send(new RegisterCommand
            {
                Email = account.Email,
                Password = account.Password,
                ConfirmPassword = account.ConfirmPassword,
                FirstName = account.FirstName,
                LastName = account.LastName,
                UserName = account.UserName,
                Origin = Request.Headers["Origin"]
            }));
        }


        private string? GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {

                return Request.Headers["X-Forwarded-For"];
            }
            return HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString();

        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }



        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserRequest request,
            CancellationToken cancellationToken
        )
        {
            var command = new RegisterUserCommand(
                request.Name,
                request.LastName,
                request.Dni,
                request.PhoneNumber,
                request.Province,
                request.City,
                request.District,
                request.Email,
                request.PasswordHash,
                request.ConfirmPassword,
                request.IpUser,
                request.UserStatus
            );

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);

        }

        [Authorize]
        [HttpPost("me")]
        public async Task<IActionResult> Me(
            [FromBody] MeTokenRequest request,
            CancellationToken cancellationToken
        )
        {
            var command = new GetUserByTokenCommand(request.AuthToken);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return Unauthorized(result.Error);
            }

            return Ok(result.Value);
        }

    }
}
