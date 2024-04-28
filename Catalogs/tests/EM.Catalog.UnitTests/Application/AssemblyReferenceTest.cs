using EM.Catalog.Application;
using System.Reflection;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public sealed class AssemblyReferenceTest
{
    [Fact]
    public void Assembly_ValidAssembly_MustReturnValidAssembly()
    {
        Assembly assembly = AssemblyReference.Assembly;
        
        string? assemblyName = assembly.GetName()?.Name;

        Assert.NotNull(assembly);
        Assert.NotNull(assemblyName);
        Assert.Equal("EM.Catalog.Application", assemblyName);
    }
}
