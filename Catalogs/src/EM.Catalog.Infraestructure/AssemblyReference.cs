using System.Reflection;

namespace EM.Catalog.Infraestructure;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
