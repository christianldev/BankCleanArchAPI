using CQRS.BankAPI.Domain.Abstractions;
using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Roles;
using CQRS.BankAPI.Domain.Users.Events;

namespace CQRS.BankAPI.Domain.Users;

public class User : Entity<UserId>
{
    private User()
    {

    }

    private User(
        UserId id,
        string name,
        string lastname,
        Dni dni,
        Address address,
        PhoneNumber phoneNumber,
        Email email,
        PasswordHash passwordHash,
        IpUser ipUser,
        UserStatus userstatus
        ) : base(id)
    {
        Name = name;
        LastName = lastname;
        Dni = dni;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        PasswordHash = passwordHash;
        IpUser = ipUser;
        UserStatus = userstatus;
    }

    public string? Name { get; private set; }
    public string? LastName { get; private set; }
    public Dni Dni { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PasswordHash? PasswordHash { get; private set; } = null!;
    public IpUser? IpUser { get; private set; }
    public UserStatus UserStatus { get; private set; }

    public static User Create(
        string name,
        string lastname,
        Dni dni,
        Address address,
        PhoneNumber phoneNumber,
        Email email,
        PasswordHash passwordHash,
        IpUser ipUser,
        UserStatus userstatus
    )
    {
        var user = new User(UserId.New(), name, lastname, dni, address, phoneNumber, email, passwordHash, ipUser, userstatus);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id!));
        return user;
    }

    public ICollection<Role>? Roles { get; set; }

}
