using Microsoft.EntityFrameworkCore;
using Infra.Context;
using Infra.Contracts;
using Infra.Repositories;
using Services.CourierService;
using Services.MotorcycleService;
using Services.ICourierService;
using Services.IMotorcycleService;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connStr, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

var apiPort = builder.Configuration.GetValue<string>("ApiRentalConfiguration:Port");

if (!string.IsNullOrWhiteSpace(apiPort))
{
    builder.WebHost.UseUrls(new string[] { $"http://+:{apiPort}" });
}

//REPOSITORIES
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

//SERVICES
builder.Services.AddScoped<ICourierService, CourierService>();
builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Rental API",
        Version = "v1",
        Description = "API para gestão de motos, entregadores e locações"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Rental API v1");
        options.RoutePrefix = string.Empty; // Swagger abre direto em "/"
    });
}
else
{
    app.UseSwagger();
}

app.UseSwaggerUI();

app.MapControllers();
app.Run();



