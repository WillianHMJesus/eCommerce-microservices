﻿using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoMapper;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Validations;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Validations;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using Moq;

namespace EM.Catalog.UnitTests.CustomAutoData;

public class AutoProductDataAttribute : AutoDataAttribute
{
    public AutoProductDataAttribute()
        : base(CreateFixture)
    { }

    private static IFixture CreateFixture()
    {
        IFixture fixture = new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true });

        Product product = fixture.Create<Product>();
        Category category = fixture.Create<Category>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>()))
            .Returns(product);

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Product>(It.IsAny<UpdateProductCommand>()))
            .Returns(product);

        fixture.Freeze<Mock<ICategoryValidations>>()
            .Setup(x => x.ValidateCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<IProductValidations>>()
            .Setup(x => x.ValidateDuplicityAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<IProductValidations>>()
            .Setup(x => x.ValidateDuplicityAsync(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<IProductValidations>>()
            .Setup(x => x.ValidateProductIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<IUnitOfWork>>()
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<IReadRepository>>()
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        IEnumerable<Error> errors = new List<Error> { new Error(Key.ProductAnErrorOccorred, "") };
        fixture.Freeze<Mock<IResourceManager>>()
            .Setup(x => x.GetErrorsByKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.CreateResponseWithErrors(errors));

        fixture.Customize<Product>(x => x.FromFactory(() => product));

        return fixture;
    }
}
