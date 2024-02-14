using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using AutoWrapper;
using FluentValidation.AspNetCore;
using NotificationService.Application;
using NotificationService.Infrastructure;
using NotificationService.WebAPI.Extensions;
using NotificationService.WebAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers(option =>
    {
        option.Filters.Add(typeof(ValidationActionFilter));
    })
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();

// ----- AutoMapper -----
builder.Services.AddAutoMapperSetup();

// ----- ApiVersioning -----
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastricture(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions
{
    LogRequestDataOnException = false,
    ShouldLogRequestData = false,
    UseApiProblemDetailsException = true,
    EnableExceptionLogging = app.Environment.IsDevelopment(),
    EnableResponseLogging = app.Environment.IsDevelopment(),
    IsDebug = app.Environment.IsDevelopment()
});
app.UseHttpsRedirection();
app.MapControllers(); 

app.Run();