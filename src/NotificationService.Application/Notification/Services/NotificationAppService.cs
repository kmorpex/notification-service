using AutoMapper;
using ErrorOr;
using MediatR;
using NotificationService.Application.Abstractions.Services;
using NotificationService.Application.Notification.Commands.SendEmail;
using NotificationService.Application.Notification.Commands.SendSMS;
using NotificationService.Application.Notification.Contracts.SendEmail;
using NotificationService.Application.Notification.Contracts.SendSMS;

namespace NotificationService.Application.Notification.Services;

public class NotificationAppService : INotificationAppService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    public NotificationAppService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    public async Task<ErrorOr<Success>> SendEmailAsync(SendEmailRequestModel model, CancellationToken ct = default)
    {
        var command = _mapper.Map<SendEmailCommand>(model);
        
        var result = await _mediator.Send(command, ct);
        if (result.IsError)
        {
            return result.ErrorsOrEmptyList;
        }
        
        return Result.Success;
    }

    public async Task<ErrorOr<Success>> SendSMSAsync(SendSMSRequestModel model, CancellationToken ct = default)
    {
        var command = _mapper.Map<SendSMSCommand>(model);
        
        var result = await _mediator.Send(command, ct);
        if (result.IsError)
        {
            return result.ErrorsOrEmptyList;
        }
        
        return Result.Success;
    }
}