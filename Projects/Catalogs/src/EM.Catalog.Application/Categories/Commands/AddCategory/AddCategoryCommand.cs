using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed record AddCategoryCommand(short Code, string Name, string Description) 
    : ICommand
{ }
