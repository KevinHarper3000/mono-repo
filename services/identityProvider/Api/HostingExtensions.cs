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

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        //app.UseIdentityServer();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        // uncomment if you want to add a UI
        //app.UseAuthorization();
        //app.MapRazorPages().RequireAuthorization();

        return app;
    }

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder,
        IConfiguration configuration, Container container)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddIdentityServer()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        //builder.Services.AddScoped<ILocalUserService, LocalUserService>();

        MySqlServerVersion serverVersion = new(new Version(8, 0, 27));
        string connection = GetConnectionString(configuration);

        builder.Services.AddDbContext<IdpContext>(options =>
            options.UseMySql(connection, serverVersion).EnableSensitiveDataLogging());

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

        //uncomment if you want to add a UI
        //builder.Services.AddRazorPages();

        builder.Services.AddSimpleInjector(container, options =>
        {
            options.AddAspNetCore()
                .AddControllerActivation();
        });

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


        //builder.AddProfileService<>();

        //builder.Services.AddAuthentication()
        //    .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
        //    {
        //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        //        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
        //        options.SaveTokens = true;

        //        options.Authority = "https://localhost:5001";
        //        options.ClientId = "web";
        //        options.ClientSecret = "secret";
        //        options.ResponseType = "code";

        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            NameClaimType = "name",
        //            RoleClaimType = "role"
        //        };
        //    });

        InitializeContainer(container);

        return builder.Build();
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
            .RegisterConditional<IRepository<UserViewModel>, BaseRepository<UserViewModel, User>>(
                x => x.Consumer.ImplementationType == typeof(UsersController));
        container
            .RegisterConditional<IRepository<UserClaimViewModel>,
                BaseRepository<UserClaimViewModel, UserClaim>>(
                x => x.Consumer.ImplementationType == typeof(UserClaimsController));
    }
}