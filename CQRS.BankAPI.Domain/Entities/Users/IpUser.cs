using System.Text.RegularExpressions;

namespace CQRS.BankAPI.Domain;

public partial record IpUser(string Value)
{
    const string Pattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";

    [GeneratedRegex(Pattern)]
    private static partial Regex IpUserRegex();

    public static IpUser? Create(string value)
    {
        if (string.IsNullOrEmpty(value) || !IpUserRegex().IsMatch(value))
        {
            return null;
        }

        return new IpUser(value);
    }

    public static implicit operator string(IpUser ipUser) => ipUser.Value;

    public static implicit operator IpUser(string ipUser) => Create(ipUser)!;

    public override string ToString() => Value;
}
