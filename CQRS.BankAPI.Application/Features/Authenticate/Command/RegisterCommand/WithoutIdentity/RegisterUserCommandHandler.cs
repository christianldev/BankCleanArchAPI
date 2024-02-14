

using CQRS.BankAPI.Application.Abstractions.Messaging;
using CQRS.BankAPI.Application.Helpers;
using CQRS.BankAPI.Domain;
using CQRS.BankAPI.Domain.Abstractions;
using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Users;

namespace CQRS.BankAPI.Application.Features.Authenticate.Command.RegisterCommand.WithoutIdentity;

internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
        )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
        )
    {

        //1. Validar que el usuario no exista en la base de datos
        var email = new Email(request.Email);
        var userExists = await _userRepository.IsUserExists(email);

        //2. Validar que el DNI sea valido y no exista en la base de datos
        var dni = new Dni(request.Dni);
        var dniExists = await _userRepository.IsDniExists(dni);

        if (dniExists)
        {
            return Result.Failure<Guid>(UserErrors.DniAlreadyExists);
        }

        if (userExists)
        {
            return Result.Failure<Guid>(UserErrors.AlreadyExists);
        }


        //2. Encriptar el password plano del usuario que envio el cliente
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
        var ipAddress = IpHelper.GetIpAddress();

        //3. Crear un objeto de tipo user
        var user = User.Create(
            request.Name,
            request.LastName,
            new Dni(request.Dni),
            new Address(request.Province, request.City, request.District),
            new PhoneNumber(request.PhoneNumber),
            new Email(request.Email),
            new PasswordHash(passwordHash),
            new IpUser(ipAddress),
            new UserStatus(UserStatus.Active)
        );


        //4. Insertar el usuario a la base de datos
        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync();

        // return a message with status 200 OK and the user id
        return Result.Success<Guid>(user.Id.Value);
    }
}