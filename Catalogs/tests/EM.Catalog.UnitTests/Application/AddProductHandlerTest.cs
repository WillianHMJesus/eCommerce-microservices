using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public class AddProductHandlerTest
{
    [Fact]
    public async Task Handle_ValidRequest_MustInvokeAddProduct()
    {
        Mock<IProductRepository> mockProductRepository = new();
        AddProductHandler addProductHandler = new(mockProductRepository.Object);

        await addProductHandler.Handle(new AddProductCommand("iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro"), CancellationToken.None);

        mockProductRepository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
    }
}
