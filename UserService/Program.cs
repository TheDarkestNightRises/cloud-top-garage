using System.Reflection;
using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.AuthPolicies;
using UserService.Data;
using UserService.Logic;


//Setup Mass Transit with RabbitMQ

var builder = WebApplication.CreateBuilder(args);

//Setting up MassTransit
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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
AuthorizationPolicies.AddPolicies(builder.Services);

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("UsersConn")));

}
else
{
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
    // Console.WriteLine("--> Using SqlServer Db locally");
    //  builder.Services.AddDbContext<AppDbContext>(opt =>
    //    opt.UseSqlServer(builder.Configuration.GetConnectionString("UsersConn")));
}

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
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
