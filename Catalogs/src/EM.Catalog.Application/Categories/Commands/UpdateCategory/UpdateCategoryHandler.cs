using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IMapper _mapper;

    public UpdateCategoryHandler(
        IWriteRepository writeRepository,
        IMapper mapper)
    {
        _writeRepository = writeRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = _mapper.Map<Category>(command);
        await _writeRepository.UpdateCategoryAsync(category, cancellationToken);

        return new Result();
    }
}
