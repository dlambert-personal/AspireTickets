//using MediatR;

namespace TodoStates.Shared.CQRS;

public interface IQuery<out TResponse>;
//public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull;
