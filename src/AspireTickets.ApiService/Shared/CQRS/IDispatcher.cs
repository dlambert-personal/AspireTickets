namespace TodoStates.Shared.CQRS;

public interface IDispatcher
{
    Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : ICommand<TResponse>;
}