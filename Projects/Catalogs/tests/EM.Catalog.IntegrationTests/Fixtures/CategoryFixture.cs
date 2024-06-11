using EM.Catalog.Application.Categories.Commands.AddCategory;
using AutoFixture;

namespace EM.Catalog.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(CategoryCollection))]
public class CategoryCollection : ICollectionFixture<CategoryFixture>
{ }

public class CategoryFixture
{
    private AddCategoryCommand? _addCategoryCommand;

    public Guid? CategoryId { get; set; }

    public AddCategoryCommand GenerateValidAddCategoryCommandWithTheSameValue()
    {
        if (_addCategoryCommand is null)
            _addCategoryCommand = new Fixture().Create<AddCategoryCommand>();

        return _addCategoryCommand;
    }
}
