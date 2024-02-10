namespace CQRS.BankAPI.Application.DTOS.Request;

public record LoginUserRequest(string Email, string Password);