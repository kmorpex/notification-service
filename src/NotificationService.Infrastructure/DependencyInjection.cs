using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Core.Enum;
using NotificationService.Core.Providers;
using NotificationService.Infrastructure.Background;
using NotificationService.Infrastructure.Background.Configuration;
using NotificationService.Infrastructure.Extensions;
using NotificationService.Infrastructure.Notification.Providers.Email.Fake;
using NotificationService.Infrastructure.Notification.Providers.Email.SendGrid;
using NotificationService.Infrastructure.Notification.Providers.SMS.Fake;
using NotificationService.Infrastructure.Notification.Services;

namespace NotificationService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastricture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new WorkerConfiguration
        {
            WorkerCount = Environment.ProcessorCount // Use the number of processor cores as worker count
        });
        services.AddSingleton(new RetryPolicy
        {
            MaxRetries = 3,
            DelayBetweenRetries = TimeSpan.FromSeconds(5)
        });
        
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        var workerConfiguration = services.BuildServiceProvider().GetRequiredService<WorkerConfiguration>();
        for (int i = 0; i < workerConfiguration.WorkerCount; i++)
        {
            services.AddHostedService<BackgroundTaskService>();
        }

        services.AddEmailService(configuration);
        services.AddSMSService(configuration);
        
        services.AddScoped<INotificationServiceFactory, NotificationServiceFactory>();
        services.Decorate<INotificationServiceFactory, BackgroundNotificationServiceFactory>();
    }
    
    private static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        EmailProviderEnum? provider = configuration.GetValue<EmailProviderEnum?>("NotificationSettings:Email:Provider");
        
        if (provider == null)
            throw new NullReferenceException("The email provider is not registered in the configuration file.");

        switch (provider)
        {
            case EmailProviderEnum.SendGrid:
                services.AddSingleton<SendGridOptions>(configuration.GetSection("NotificationSettings:Email:SendGrid").Get<SendGridOptions>());
                services.AddScoped<IEmailProvider, SendGridEmailProvider>();
                break;
            case EmailProviderEnum.Fake:
                services.AddTransient<IEmailProvider, FakeEmailProvider>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private static void AddSMSService(this IServiceCollection services, IConfiguration configuration)
    {
        SMSProviderEnum? provider = configuration.GetValue<SMSProviderEnum?>("NotificationSettings:SMS:Provider");

        if (provider == null)
            throw new NullReferenceException("The SMS provider is not registered in the configuration file.");

        switch (provider)
        {
            case SMSProviderEnum.Fake:
                services.AddScoped<ISMSProvider, FakeSMSProvider>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}