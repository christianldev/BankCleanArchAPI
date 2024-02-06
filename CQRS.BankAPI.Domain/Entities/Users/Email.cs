using System.Text.RegularExpressions;

namespace CQRS.BankAPI.Domain.Entities.Users;

public record Email(string Value)
{
    const string RegexPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        if (!Regex.IsMatch(email, RegexPattern))
        {
            throw new ArgumentException("Email is invalid", nameof(email));
        }

        return new Email(email);
    }

    public static implicit operator string(Email email) => email.Value;

    public static implicit operator Email(string email) => Create(email);

    public override string ToString() => Value;

}
