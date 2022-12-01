using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MockAPI.Config;
using MockAPI.DAL.Repository;
using MockAPI.DAL.Repository.EFC;
using MockAPI.DAL.Repository.EFC.Options;
using MockAPI.DAL.Repository.Impl;
using MockAPI.Service;
using MockAPI.Service.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddCors(options => {
//     options.AddPolicy(name: "Development",
//         policy  =>
//         {
//             policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
//         });
// });

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services.AddDbContext<DatabaseContext>(
    (serviceProvider, dbContextOptionsBuilder) =>
{
    var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
    dbContextOptionsBuilder.UseSqlServer(databaseOptions.ConnectionString);
    dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
    dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
});

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Development",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Development");
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.Run();
