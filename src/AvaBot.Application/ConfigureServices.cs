using AvaBot.Application.Common.Behaviours;
using AvaBot.Application.Common.Middlewares;
using AvaBot.Application.Features.Instructions.Commands.CreateInstruction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
namespace AvaBot.Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CreateInstructionCommandHandler>();
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddTransient<ExceptionHandlingMiddleware>();
        return services;
    }
}
