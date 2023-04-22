namespace EM.Catalog.Domain;

public sealed class AssertionConcern
{
    public static void ValidateNullOrEmpty(string value, string message)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateLessThanEqualToMinimum(long valor, long minimum, string message)
    {
        if (valor <= minimum)
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateLessThanMinimum(long valor, long minimum, string message)
    {
        if (valor < minimum)
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateLessThanEqualToMinimum(decimal valor, decimal minimum, string message)
    {
        if (valor <= minimum)
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateNull(object value, string message)
    {
        if (value == null)
        {
            throw new DomainException(message);
        }
    }
}
