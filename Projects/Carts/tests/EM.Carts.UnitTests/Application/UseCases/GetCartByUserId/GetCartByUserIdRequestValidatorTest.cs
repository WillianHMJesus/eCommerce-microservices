using AutoFixture;
using EM.Carts.Application.UseCases.GetCartByUserId;
using EM.Carts.UnitTests.Application.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using FluentValidation.Results;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application.UseCases.GetCartByUserId;

public sealed class GetCartByUserIdRequestValidatorTest
{
    [Theory, AutoCartData]
    public async Task Constructor_ValidGetCartByUserId_ShouldReturnValidResult(
        GetCartByUserIdRequestValidator sut,
        GetCartByUserIdRequest request)
    {
        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCartData]
    public async Task Constructor_DefaultUserId_ShouldReturnInvalidResult(
        GetCartByUserIdRequestValidator sut)
    {
        GetCartByUserIdRequest request = new Fixture().Build<GetCartByUserIdRequest>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        ValidationResult result = await sut.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Key.UserIdInvalid);
    }
}
