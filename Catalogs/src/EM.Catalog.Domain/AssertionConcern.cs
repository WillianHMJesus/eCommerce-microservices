namespace EM.Catalog.Domain;

internal class AssertionConcern
{
    internal static void ValidateNullOrEmpty(string value, string message)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainException(message);
        }
    }

    internal static void ValidateLessThanEqualToMinimum(int valor, int minimum, string message)
    {
        if (valor <= minimum)
        {
            throw new DomainException(message);
        }
    }

    internal static void ValidateLessThanMinimum(int valor, int minimum, string message)
    {
        if (valor < minimum)
        {
            throw new DomainException(message);
        }
    }

    internal static void ValidateLessThanEqualToMinimum(decimal valor, decimal minimum, string message)
    {
        if (valor <= minimum)
        {
            throw new DomainException(message);
        }
    }
}
