using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using NotificationService.Application.Abstractions.Services;
using NotificationService.Application.Configuration.Behaviors;
using NotificationService.Application.Notification.Services;

namespace NotificationService.Application;

public static class ModuleApplication
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<INotificationAppService, NotificationAppService>();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleApplication).Assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(ModuleApplication).Assembly);
    }
}