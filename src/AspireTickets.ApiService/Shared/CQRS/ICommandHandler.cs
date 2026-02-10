//using MediatR;
namespace TodoStates.Shared.CQRS;

//todo: look at this interface
//public interface ICommandHandler<in TCommand>
//    : ICommandHandler<TCommand, Result>
//    where TCommand : ICommand<Result>
//{
//    Task<Result> Handle(TCommand request, CancellationToken cancellationToken);
//}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken = default);
}


public interface ICommandHandler<in TCommand, TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken = default);
}
//public interface ICommandHandler<in TCommand, TResponse>
//    //: IRequestHandler<TCommand, TResponse>
//    where TCommand : ICommand<TResponse>
//    where TResponse : notnull
//{
//    Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
//}
