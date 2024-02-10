namespace CQRS.BankAPI.Domain.Entities.Users;

public record UserStatus(string Value)
{
    public static UserStatus Active => new("Active");
    public static UserStatus Inactive => new("Inactive");
    public static UserStatus Blocked => new("Blocked");
    public static UserStatus Deleted => new("Deleted");

    public static implicit operator string(UserStatus userStatus) => userStatus.Value;
    public static implicit operator UserStatus(string userStatus) => new(userStatus);
    public override string ToString() => Value;
}
