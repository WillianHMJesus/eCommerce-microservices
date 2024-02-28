using AutoMapper;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.Fixtures;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public sealed class AddProductHandlerTest
{
    private readonly Mock<IWriteRepository> _mockWriteRepository;
    Mock<IMapper> _mockMapper = new();

    public AddProductHandlerTest()
    {
        _mockWriteRepository = new();
        _mockMapper = new();
    }

    [Fact]
    public async Task Handle_ValidRequest_MustReturnWithSuccess()
    {
        Category? category = new CategoryFixture().GenerateCategory();
        AddProductHandler addProductHandler = new(_mockWriteRepository.Object, _mockMapper.Object);
        _mockWriteRepository.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Category?>(category));
        _mockMapper.Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>()))
            .Returns(new ProductFixture().GenerateProduct());

        Result result = await addProductHandler.Handle(new AddProductCommand("iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro", category.Id), 
            CancellationToken.None);

        Assert.True(result.Success);
        Assert.IsType<Guid>(result.Data);
    }

    [Fact]
    public async Task Handle_InvalidRequest_MustReturnWithFailed()
    {
        AddProductHandler addProductHandler = new(_mockWriteRepository.Object, _mockMapper.Object);

        Result result = await addProductHandler.Handle(new AddProductCommand("iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro", Guid.NewGuid()), 
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.True(result.Errors?.Any(x => x.Message == ErrorMessage.ProductCategoryNotFound));
    }
}
