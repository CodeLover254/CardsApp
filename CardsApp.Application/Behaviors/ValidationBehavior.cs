using FluentValidation;
using MediatR;

namespace CardsApp.Application.Behaviors;

public class ValidationBehavior<TRequest,TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest: class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var validationCtx = new ValidationContext<TRequest>(request);

        var errors = _validators.Select(v => v.Validate(validationCtx))
            .SelectMany(x => x.Errors)
            .Where(f => f != null)
            .ToList();

        if (errors.Any()) throw new ValidationException("Validation failure", errors);

        return await next();
    }
}