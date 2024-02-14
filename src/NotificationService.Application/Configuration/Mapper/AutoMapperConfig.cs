using AutoMapper;
using NotificationService.Application.Configuration.Mapper.Notification;

namespace NotificationService.Application.Configuration.Mapper;

public static class AutoMapperConfig
{
    public static void RegisterAppMappings(this IMapperConfigurationExpression config)
    {
        config.AddProfile(new NotificationMapper());
    }
}