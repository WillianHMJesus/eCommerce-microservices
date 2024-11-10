using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EM.Carts.Application;

[ExcludeFromCodeCoverage]
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
