using ChatBot.Auth.Jwt;
using ChatBot.Extensions;
using ChatBot.Http;
using ChatBot.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ChatBot.Repositories.EFC;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Pomelo.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);




var serverVersion = new MariaDbServerVersion(new Version(10, 4, 24));
builder.Services.AddDbContext<DatabaseContext>(options=>options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),serverVersion));  
builder.Services.RegisterServices();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Development",
        policy => { policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod(); });
});
builder.Services.AddControllers();

builder.Services.AddHttpClient<IUserHttpClient, UserHttpClient>();

//builder.Services.ConfigureOptions<DatabaseOptionsSetup>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(60), });

app.UseStaticFiles();
app.UseRouting();
app.UseCors("Development");
app.UseMiddleware<JwtMiddleware>();


//app.UseAuthorization();

app.UseEndpoints(endpoint => { endpoint.MapControllers(); });

app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(60), });


app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<ConversationMiddleware>();

app.Run();
