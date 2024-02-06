using CQRS.BankAPI.Domain.Abstractions;
using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Users.Events;

namespace CQRS.BankAPI.Domain.Users;

public class User : Entity<UserId>
{
    private User()
    {

    }

    private User(
        UserId id,
        string nombre,
        string apellido,
        Dni dni,
        Address address,
        PhoneNumber phoneNumber,
        Email email,
        PasswordHash passwordHash,
        IpUser ipUser,
        UserStatus userstatus
        ) : base(id)
    {
        Nombre = nombre;
        Apellido = apellido;
        Dni = dni;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        PasswordHash = passwordHash;
        IpUser = ipUser;
        UserStatus = userstatus;
    }

    public string? Nombre { get; private set; }
    public string? Apellido { get; private set; }
    public Dni Dni { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PasswordHash? PasswordHash { get; private set; } = null!;
    public IpUser? IpUser { get; private set; }
    public UserStatus UserStatus { get; private set; }

    public static User Create(
        string nombre,
        string apellido,
        Dni dni,
        Address address,
        PhoneNumber phoneNumber,
        Email email,
        PasswordHash passwordHash,
        IpUser ipUser,
        UserStatus userstatus
    )
    {
        var user = new User(UserId.New(), nombre, apellido, dni, address, phoneNumber, email, passwordHash, ipUser, userstatus);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id!));
        return user;
    }

}
