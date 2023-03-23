using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Application.DTOs;

public sealed record CategoryDTO(Guid Id, short Code, string Name, string Description)
{
    public static explicit operator CategoryDTO(Category category)
    {
        return new CategoryDTO(
            category.Id,
            category.Code,
            category.Name,
            category.Description);
    }
}

