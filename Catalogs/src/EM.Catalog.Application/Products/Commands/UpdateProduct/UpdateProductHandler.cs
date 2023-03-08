using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand>
{
    public Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
