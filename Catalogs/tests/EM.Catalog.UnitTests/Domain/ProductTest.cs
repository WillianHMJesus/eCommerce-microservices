using EM.Catalog.Domain.Entities;
using Xunit;

namespace EM.Catalog.UnitTests.Domain;

public class ProductTest
{
    [Fact]
    public void DebitQuantity_QuantityValid_MustDebitProductQuantity()
    {
        Product product = new("Maquina de Lavar", "Máquina de lavar Brastemp 15kg", 3100.99M, 10, "");

        product.DebitQuantity(product.Quantity);

        Assert.Equal(0, product.Quantity);
    }
}
