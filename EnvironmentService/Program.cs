using System.Reflection;
using EnvironmentService.Application.Logic;
using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Controllers;
using EnvironmentService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Setup Mass Transit with RabbitMQ
var configuration = builder.Configuration;
var rabbitMqConfig = configuration.GetSection("RabbitMQ");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(Assembly.GetEntryAssembly());
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host($"{rabbitMqConfig["Host"]}", $"/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("EnvironmentService", false));
    });
});

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilterAttribute>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("EnvironmentsConn")));
}
else
{
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
         opt.UseInMemoryDatabase("InMem"));
    // Console.WriteLine("--> Using SqlServer Db");
    // builder.Services.AddDbContext<AppDbContext>(opt =>
    //     opt.UseSqlServer(builder.Configuration.GetConnectionString("EnvironmentsConn")));
}
builder.Services.AddScoped<IStatRepository, StatRepository>();
builder.Services.AddScoped<IIndoorEnvironmentRepository, IndoorEnvironmentRepository>();
builder.Services.AddScoped<IIndoorEnvironmentLogic, IndoorEnvironmentLogic>();
builder.Services.AddScoped<IStatLogic, StatLogic>();
builder.Services.AddScoped<IGarageLogic, GarageLogic>();
builder.Services.AddScoped<IGarageRepository, GarageRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());
builder.Services.AddHealthChecks();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz");
PrepDb.PrepPopulation(app, app.Environment.IsProduction());
// Build the service provider
var serviceProvider = builder.Services.BuildServiceProvider();

// Resolve the logic class
var indoorEnvironmentLogic = serviceProvider.GetService<IIndoorEnvironmentLogic>();

// Call the initialization method
indoorEnvironmentLogic.InitializeWebSockets();
app.Run();
