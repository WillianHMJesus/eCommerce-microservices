using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.Fixtures;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public class AddProductHandlerTest
{
    private readonly CategoryFixture _categoryFixture;

    public AddProductHandlerTest()
        => _categoryFixture = new CategoryFixture();

    [Fact]
    public async Task Handle_ValidRequest_MustReturnWithSuccess()
    {
        Mock<IProductRepository> mockProductRepository = new();
        Mock<ICategoryRepository> mockCategoryRepository = new();
        Category? category = _categoryFixture.GenerateCategory();

        mockCategoryRepository.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Category?>(category));
        AddProductHandler addProductHandler = new(mockProductRepository.Object, mockCategoryRepository.Object);

        Result result = await addProductHandler.Handle(new AddProductCommand("iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro", category.Id), CancellationToken.None);

        Assert.True(result.Success);
        Assert.IsType<Guid>(result.Data);
    }

    [Fact]
    public async Task Handle_InvalidRequest_MustReturnWithFailed()
    {
        Mock<IProductRepository> mockProductRepository = new();
        Mock<ICategoryRepository> mockCategoryRepository = new();

        mockCategoryRepository.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Category?>(null));
        AddProductHandler addProductHandler = new(mockProductRepository.Object, mockCategoryRepository.Object);

        Result result = await addProductHandler.Handle(new AddProductCommand("iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro", Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.Success);
        Assert.True(result.Errors?.Any(x => x.Message == ErrorMessage.ProductCategoryNotFound));
    }
}
