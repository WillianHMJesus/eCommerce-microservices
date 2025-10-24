using AutoFixture;
using EM.Catalog.API.Models;

namespace EM.Catalog.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(CategoryCollection))]
public class CategoryCollection : ICollectionFixture<CategoryFixture>
{ }

public class CategoryFixture
{
    private CategoryRequest? _categoryRequest;

    public Guid? CategoryId { get; set; }

    public CategoryRequest GenerateValidCategoryRequestWithTheSameValue()
    {
        if (_categoryRequest is null)
            _categoryRequest = new Fixture().Create<CategoryRequest>();

        return _categoryRequest;
    }
}
