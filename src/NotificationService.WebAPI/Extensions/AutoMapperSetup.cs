using System.Reflection;
using NotificationService.Application.Configuration.Mapper;

namespace NotificationService.WebAPI.Extensions;

public static class AutoMapperSetup
{
    public static void AddAutoMapperSetup(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddAutoMapper(cfg =>
            {
                cfg.RegisterAppMappings();
            },
            Assembly.GetExecutingAssembly()
        );
    }
}