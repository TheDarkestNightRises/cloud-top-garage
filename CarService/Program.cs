using Application.LogicContracts;
using CarService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("CarsConn")));

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
    builder.Services.AddScoped<ICarLogic, CarLogic>();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    PrepDb.PrepPopulation(app, app.Environment.IsProduction());
    app.Run();
