using FoodDeliveryService.Application.Services;
using FoodDeliveryService.Infrastructure.Persistence.Data;
using FoodDeliveryService.Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<FoodDeliveryDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .CreateLogger();
    builder.Host.UseSerilog();

    builder.Services.AddScoped<IRestaurantService, RestaurantService>();
    builder.Services.AddScoped<IMealService, MealService>();
    builder.Services.AddScoped<IOrderService, OrderService>();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });
}

var app = builder.Build();
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<FoodDeliveryDbContext>();
        dbContext.Database.EnsureCreated();
    }

    if (app.Environment.EnvironmentName.Equals("Development"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}
