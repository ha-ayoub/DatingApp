using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Interfaces;
using DatingApp.Infrastructure.Hubs;
using DatingApp.Infrastructure.Identity;
using DatingApp.Infrastructure.Persistence;
using DatingApp.Infrastructure.Persistence.Repositories;
using DatingApp.Infrastructure.Persistence.Seeders;
using DatingApp.Infrastructure.Services;
using DatingApp.Infrastructure.Services.Cache;
using DatingApp.Infrastructure.Services.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DatingApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<ISwipeRepository, SwipeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<DataSeeder>();
        services.AddSignalR();

        return services;
    }
}
