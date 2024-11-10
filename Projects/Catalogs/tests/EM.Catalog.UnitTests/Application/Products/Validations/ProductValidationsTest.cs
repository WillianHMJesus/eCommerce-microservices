using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Validations;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Validations;

public sealed class ProductValidationsTest
{
    [Theory, AutoProductData]
    public async Task ValidateDuplicityAddAsync_ProductsEmpty_ShouldReturnTrue(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        ProductValidations sut,
        string name)
    {
        repositoryMock
            .Setup(x => x.GetProductsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        bool result = await sut.ValidateDuplicityAsync(name, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoProductData]
    public async Task ValidateDuplicityAddAsync_ProductsNotEmpty_ShouldReturnFalse(
        ProductValidations sut,
        string name)
    {
        bool result = await sut.ValidateDuplicityAsync(name, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoProductData]
    public async Task ValidateDuplicityUpdateAsync_ProductsEmpty_ShouldReturnTrue(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        ProductValidations sut,
        UpdateProductCommand command)
    {
        repositoryMock
            .Setup(x => x.GetProductsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        bool result = await sut.ValidateDuplicityAsync(command, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoProductData]
    public async Task ValidateDuplicityUpdateAsync_ProductsNotEmpty_ShouldReturnFalse(
        ProductValidations sut,
        UpdateProductCommand command)
    {
        bool result = await sut.ValidateDuplicityAsync(command, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoProductData]
    public async Task ValidateDuplicityUpdateAsync_ProductsNotEmptyButNotDuplicity_ShouldReturnTrue(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        ProductValidations sut,
        Product product)
    {
        IEnumerable<Product> products = new List<Product>() { product };
        repositoryMock
            .Setup(x => x.GetProductsByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        UpdateProductCommand command = new Fixture().Build<UpdateProductCommand>()
            .With(x => x.Id, product.Id)
            .Create();

        bool result = await sut.ValidateDuplicityAsync(command, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoProductData]
    public async Task ValidateProductIdAsync_ProductFound_ShouldReturnTrue(
        ProductValidations sut,
        Guid productId)
    {
        bool result = await sut.ValidateProductIdAsync(productId, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoProductData]
    public async Task ValidateProductIdAsync_ProductNotFound_ShouldReturnFalse(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        ProductValidations sut,
        Guid productId)
    {
        Product? product = null;
        repositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        bool result = await sut.ValidateProductIdAsync(productId, CancellationToken.None);

        result.Should().BeFalse();
    }
}
