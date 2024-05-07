namespace EM.Common.Core.Domain;

public sealed class AssertionConcern
{
    public static void ValidateNullOrEmpty(string value, string message)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateNullOrDefault<T>(T value, string message)
    {
        if (value is null || EqualityComparer<T>.Default.Equals(value, default(T)))
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

    public static void ValidateLessThanEqualToMinimum(long valor, long minimum, string message)
    {
        if (valor <= minimum)
        {
            throw new DomainException(message);
        }
    }
}
