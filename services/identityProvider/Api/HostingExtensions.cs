using System.Text;
using Api.Controllers;
using AutoMapper;
using Data.Contexts;
using Data.Entities;
using Data.MappingProfiles;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using SimpleInjector;
using SvcCommon.Abstract;
using SvcCommon.Concrete;

namespace Api;

internal static class HostingExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        ConfigureForDev(app);
        
        app.UseStaticFiles();
        app.UseRouting();

        //app.UseIdentityServer();

        app.MapControllers();

        return app;
    }

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder,
        IConfiguration configuration, Container container)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        AddIdentityConfiguration(builder);
        ConfigurePersistence(builder, configuration);

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

        //builder.Services.AddScoped<ILocalUserService, LocalUserService>();

        builder.Services.AddSimpleInjector(container, options =>
        {
            options.AddAspNetCore()
                .AddControllerActivation();
        });

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        InitializeContainer(container);

        return builder.Build();
    }

    private static void ConfigureForDev(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }

    private static void AddIdentityConfiguration(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityServer()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);
    }

    private static void ConfigurePersistence(WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        MySqlServerVersion serverVersion = new(new Version(8, 0, 27));
        string connection = GetConnectionString(configuration);

        builder.Services.AddDbContext<IdpContext>(options =>
            options.UseMySql(connection, serverVersion).EnableSensitiveDataLogging());
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        StringBuilder connection = new();
        connection.Append(configuration["server"]);
        connection.Append(configuration["port"]);
        connection.Append(configuration["database"]);
        connection.Append(configuration["UID"]);
        connection.Append(configuration["dbPassword"]);
        connection.Append(configuration["SSLMode"]);
        return connection.ToString();
    }

    private static void InitializeContainer(Container container)
    {
        container.Register<IBaseContext, IdpContext>(Lifestyle.Scoped);
        container.Register<Profile, IdentityProfiles>(Lifestyle.Scoped);

        container
            .RegisterConditional<IRepository<UserViewModel>,
            BaseRepository<UserViewModel, User>>(
                x => x.Consumer.ImplementationType == typeof(UsersController));
        container
            .RegisterConditional<IRepository<UserClaimViewModel>,
                BaseRepository<UserClaimViewModel, UserClaim>>(
                x => x.Consumer.ImplementationType == typeof(UserClaimsController));
    }
}
