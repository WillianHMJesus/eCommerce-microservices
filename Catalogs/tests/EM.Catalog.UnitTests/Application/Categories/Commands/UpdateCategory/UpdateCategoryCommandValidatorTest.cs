using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidatorTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly UpdateCategoryCommandValidator _validator;
    private readonly UpdateCategoryCommand _updateCategoryCommand;

    public UpdateCategoryCommandValidatorTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = _fixture.Freeze<Mock<IReadRepository>>();
        _validator = _fixture.Create<UpdateCategoryCommandValidator>();
        _updateCategoryCommand = _fixture.Create<UpdateCategoryCommand>();
    }

    [Fact]
    public async Task Constructor_ValidUpdateCategoryCommand_ShouldReturnValidResult()
    {
        ValidationResult result = await _validator.ValidateAsync(_updateCategoryCommand);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Constructor_InvalidUpdateCategoryCommandId_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Id, Guid.Empty)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryInvalidId);
    }

    [Fact]
    public async Task Constructor_ZeroUpdateCategoryCommandCode_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Code, 0)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryCodeLessThanEqualToZero);
    }

    [Fact]
    public async Task Constructor_EmptyUpdateCategoryCommandName_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Name, "")
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullUpdateCategoryCommandName_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Name, null as string)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryNameNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_EmptyUpdateCategoryCommandDescription_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Description, "")
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryDescriptionNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_NullUpdateCategoryCommandDescription_ShouldReturnInvalidResult()
    {
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Description, null as string)
            .Create();

        ValidationResult result = await _validator.ValidateAsync(updateCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryDescriptionNullOrEmpty);
    }

    [Fact]
    public async Task Constructor_DuplicityUpdateCategoryCommand_ShouldReturnInvalidResult()
    {
        IEnumerable<Category> categories = new List<Category>() { _fixture.Create<Category>() };

        _repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(categories));

        ValidationResult result = await _validator.ValidateAsync(_updateCategoryCommand);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == ErrorMessage.CategoryRegisterDuplicity);
    }

    [Fact]
    public async Task Constructor_NotDuplicityUpdateCategoryCommand_ShouldReturnValidResult()
    {
        IEnumerable<Category> categories = new List<Category>() { _fixture.Create<Category>() };
        UpdateCategoryCommand updateCategoryCommand = _fixture.Build<UpdateCategoryCommand>()
            .With(x => x.Id, categories.First().Id)
            .Create();

        _repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(categories));

        ValidationResult result = await _validator.ValidateAsync(updateCategoryCommand);

        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
