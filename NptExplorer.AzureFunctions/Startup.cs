using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NptExplorer.AzureFunctions.Context;
using NptExplorer.AzureFunctions.Repositories;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using System;
using NptExplorer.AzureFunctions.Services.Abstract;
using NptExplorer.AzureFunctions.Services.Concrete;

[assembly: FunctionsStartup(typeof(NptExplorer.AzureFunctions.Startup))]
namespace NptExplorer.AzureFunctions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

        if (string.IsNullOrEmpty(connectionString)) return;
        builder.Services.AddDbContext<NptExplorerContext>(
            options => options.UseSqlServer(connectionString));
        builder.Services.AddScoped(typeof(DbContext), sp => sp.GetService<NptExplorerContext>());

        // Unit of work
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork<NptExplorerContext>>();

        builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
        builder.Services.AddScoped<IChallengeRepository, ChallengeRepository>();
        builder.Services.AddScoped<ILocationRepository, LocationRepository>();
        builder.Services.AddScoped<ITrailRepository, TrailRepository>();
        builder.Services.AddScoped<IUserBadgeRepository, UserBadgeRepository>();
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();

        builder.Services.AddScoped<IGraphService, GraphService>();
        builder.Services.AddScoped<IRequestProviderService, RequestProviderService>();

        builder.Services.AddAutoMapper(typeof(Startup).Assembly);
    }
}