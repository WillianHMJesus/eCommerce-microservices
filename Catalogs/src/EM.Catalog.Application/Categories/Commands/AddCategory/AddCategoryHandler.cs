using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryHandler : ICommandHandler<AddCategoryCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IMapper _mapper;

    public AddCategoryHandler(
        IWriteRepository writeRepository,
        IMapper mapper)
    {
        _writeRepository = writeRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = _mapper.Map<Category>(command);
        await _writeRepository.AddCategoryAsync(category, cancellationToken);

        return Result.CreateResponseWithData(category.Id);
    }
}
