using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid Id, short Code, string Name, string Description) 
    : ICommand
{ }
