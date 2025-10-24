using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid Id, short Code, string Name, string Description) : ICommand;
