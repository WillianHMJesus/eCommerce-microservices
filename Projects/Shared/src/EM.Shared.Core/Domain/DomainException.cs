namespace EM.Shared.Core;

public sealed class DomainException : Exception
{
    public DomainException(string message)
        : base(message) { }
}