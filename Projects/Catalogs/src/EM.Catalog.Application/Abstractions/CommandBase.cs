using WH.SharedKernel;
using WH.SharedKernel.Abstractions;
using WH.SharedKernel.Mediator;
using WH.SharedKernel.ResourceManagers;

namespace EM.Catalog.Application.Abstractions;
public abstract class CommandBase
    (IUnitOfWork unitOfWork,
    IMediator mediator)
{
    public async Task<Result> CommitAndPublishAsync(
        CancellationToken cancellationToken,
        Func<IEvent> eventFactory,
        string errorSavingMessage,
        Result? successResult = null)
    {
        if (!await unitOfWork.CommitAsync(cancellationToken))
        {
            return Result.CreateResponseWithErrors([new Error("Application", errorSavingMessage)]);
        }

        await mediator.Publish(eventFactory(), cancellationToken);

        return successResult ?? Result.CreateResponseWithData();
    }
}
