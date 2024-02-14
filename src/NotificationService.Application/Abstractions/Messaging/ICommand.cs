using MediatR;

namespace NotificationService.Application.Abstractions.Messaging;

public interface ICommand : IRequest, IBaseCommand
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>, IBaseCommand
{
}

public interface IBaseCommand
{
}