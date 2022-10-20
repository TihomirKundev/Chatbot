using ChatBot.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using ChatBot.Auth;
using ChatBot.Auth.Helpers;
using ChatBot.Extensions;
using ChatBot.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Development",
        policy => { policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseWebSockets(new WebSocketOptions {KeepAliveInterval = TimeSpan.FromSeconds(60),});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoint => { endpoint.MapControllers(); });

app.UseWebSockets(new WebSocketOptions {KeepAliveInterval = TimeSpan.FromSeconds(60),});

//Middlewares (interceptors)
app.UseMiddleware<ErrorHandlerMiddleware>(); //custom global error handler
app.UseMiddleware<JwtMiddleware>(); //custom jwt auth middleware
app.UseMiddleware<ConversationMiddleware>();

app.Run();
