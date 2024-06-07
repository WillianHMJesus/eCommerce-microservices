using EM.Catalog.Application.Categories.Commands.AddCategory;
using AutoFixture;

namespace EM.Catalog.IntegrationTests.Fixture;

[CollectionDefinition(nameof(AddCategoryCommandCollection))]
public class AddCategoryCommandCollection : ICollectionFixture<AddCategoryCommandFixture>
{ }

public class AddCategoryCommandFixture
{
    private AddCategoryCommand? _addCategoryCommand;

    public Guid? CategoryId { get; set; }

    public AddCategoryCommand GenerateValidAddCategoryCommandWithTheSameValue()
    {
        if (_addCategoryCommand == null)
            _addCategoryCommand = new AutoFixture.Fixture().Create<AddCategoryCommand>();

        return _addCategoryCommand;
    }
}
