using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand;
