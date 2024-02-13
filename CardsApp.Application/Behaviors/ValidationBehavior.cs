using MediatR;

namespace CardsApp.Application.Behaviors;

public class ValidationBehavior<TRequest,TResponse>: IPipelineBehavior<TRequest, TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //todo complete
        throw new NotImplementedException();
    }
}