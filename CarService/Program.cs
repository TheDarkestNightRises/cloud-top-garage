using System.Reflection;
using Application.LogicContracts;
using CarService.Application.LogicContracts;
using CarService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilterAttribute>();
});

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
        cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("CarService", false));
    });
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("CarsConn")));
}
else
{
    // Console.WriteLine("--> Using SqlServer Db");
    // builder.Services.AddDbContext<AppDbContext>(opt =>
    //     opt.UseSqlServer(builder.Configuration.GetConnectionString("CarsConn")));
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
         opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IGarageRepository, GarageRepository>();
builder.Services.AddScoped<IGarageLogic, GarageLogic>();
builder.Services.AddScoped<ICarLogic, CarLogic>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

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
app.Run();
