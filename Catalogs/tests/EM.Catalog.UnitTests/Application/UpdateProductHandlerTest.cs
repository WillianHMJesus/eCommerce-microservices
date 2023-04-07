using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.Fixtures;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public class UpdateProductHandlerTest
{
    private readonly CategoryFixture _categoryFixture;

    public UpdateProductHandlerTest()
        => _categoryFixture = new CategoryFixture();

    [Fact]
    public async Task Handle_ValidRequest_MustReturnWithSuccess()
    {
        Mock<IProductRepository> mockProductRepository = new();
        Category? category = _categoryFixture.GenerateCategory();
        mockProductRepository.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Category?>(category));
        UpdateProductHandler addProductHandler = new(mockProductRepository.Object);

        Result result = await addProductHandler.Handle(new UpdateProductCommand(Guid.NewGuid(), "iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro", true, category.Id), CancellationToken.None);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task Handle_InvalidRequest_MustReturnWithFailed()
    {
        Mock<IProductRepository> mockProductRepository = new();
        UpdateProductHandler addProductHandler = new(mockProductRepository.Object);

        Result result = await addProductHandler.Handle(new UpdateProductCommand(Guid.NewGuid(), "iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro", true, Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.Success);
        Assert.True(result.Errors?.Any(x => x.Message == ErrorMessage.ProductCategoryNotFound));
    }
}
