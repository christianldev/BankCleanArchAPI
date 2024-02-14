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

    // validation of verficator digit of DNI number that contains 10 digits
    public static bool IsValid(string dni)
    {
        if (dni.Length != 10)
        {
            return false;
        }

        int[] coefficients = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

        int sum = 0;
        for (int i = 0; i < coefficients.Length; i++)
        {
            int value = int.Parse(dni[i].ToString()) * coefficients[i];
            sum += value > 9 ? value - 9 : value;
        }

        int checkDigit = 10 - sum % 10;
        checkDigit = checkDigit == 10 ? 0 : checkDigit;

        return checkDigit == int.Parse(dni[9].ToString());
    }
    public static bool IsInvalid(string dni) => !IsValid(dni);

    public static implicit operator string(Dni dni) => dni.Value;

    public static implicit operator Dni(string dni) => Create(dni)!;

    public override string ToString() => Value;
}

