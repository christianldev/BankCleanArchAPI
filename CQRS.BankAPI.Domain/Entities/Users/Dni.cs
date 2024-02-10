namespace CQRS.BankAPI.Domain;

public sealed record Dni(string Value)
{
    private const int DefaultLenght = 10;
    private const int MaxLenght = 10;

    public static Dni? Create(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length != DefaultLenght)
        {
            return null;
        }

        return new Dni(value);
    }

    // validation of verficator digit of DNI number
    public static bool IsValid(string dni)
    {
        if (string.IsNullOrEmpty(dni) || dni.Length != MaxLenght)
        {
            return false;
        }

        var number = int.Parse(dni.Substring(0, 8));
        var letter = dni.Substring(8, 1);
        var letters = "TRWAGMYFPDXBNJZSQVHLCKE";
        var index = number % 23;
        var expectedLetter = letters[index];
        return expectedLetter == letter[0];
    }

    public static bool IsInvalid(string dni) => !IsValid(dni);

    public static implicit operator string(Dni dni) => dni.Value;

    public static implicit operator Dni(string dni) => Create(dni)!;

    public override string ToString() => Value;
}

