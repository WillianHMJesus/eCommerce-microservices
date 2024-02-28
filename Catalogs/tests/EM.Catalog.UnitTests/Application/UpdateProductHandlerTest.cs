using AutoMapper;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.Fixtures;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application;

public sealed class UpdateProductHandlerTest
{
    private readonly Mock<IWriteRepository> _mockWriteRepository;
    private readonly Mock<IMapper> _mockMapper;

    public UpdateProductHandlerTest()
    {
        _mockWriteRepository = new();
        _mockMapper = new();
    }

    [Fact]
    public async Task Handle_ValidRequest_MustReturnWithSuccess()
    {
        Category? category = new CategoryFixture().GenerateCategory();
        UpdateProductHandler addProductHandler = new(_mockWriteRepository.Object, _mockMapper.Object);
        _mockWriteRepository.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<Category?>(category));
        _mockMapper.Setup(x => x.Map<Product>(It.IsAny<UpdateProductCommand>()))
            .Returns(new ProductFixture().GenerateProduct());

        Result result = await addProductHandler.Handle(new UpdateProductCommand(Guid.NewGuid(), "iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro", true, category.Id), 
            CancellationToken.None);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task Handle_InvalidRequest_MustReturnWithFailed()
    {
        UpdateProductHandler addProductHandler = new(_mockWriteRepository.Object, _mockMapper.Object);

        Result result = await addProductHandler.Handle(new UpdateProductCommand(Guid.NewGuid(), "iPhone 14 Pro", "iPhone 14 Pro 128GB Space Black", 999, 1, "Image iPhone 14 Pro", true, Guid.NewGuid()), 
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.True(result.Errors?.Any(x => x.Message == ErrorMessage.ProductCategoryNotFound));
    }
}
