namespace CQRS.BankAPI.Application.Features.Authenticate.Command.RegisterCommand.WithoutIdentity;

public record RegisterUserRequest(string Name, string LastName, string Dni, string PhoneNumber, string Province, string City, string District, string Email, string PasswordHash, string ConfirmPassword, string IpUser, string UserStatus);