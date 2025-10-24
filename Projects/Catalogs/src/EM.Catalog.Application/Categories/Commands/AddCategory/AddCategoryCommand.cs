using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed record AddCategoryCommand(short Code, string Name, string Description) : ICommand;
