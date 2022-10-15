using ChatBot;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using ChatBot.Auth;
using ChatBot.Auth.Helpers;
using ChatBot.Auth.Repository;
using ChatBot.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AppSettings>();
builder.Services.AddSingleton<TicketService>();
builder.Services.AddSingleton<UserService>(); 
builder.Services.AddSingleton<JwtUtils>(); //inject utils
builder.Services.AddSingleton<IUserRepository, UserListRepository>();
builder.Services.AddCors(); //add cors
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(x => x  //cors settings, can be changed to only allow specific origins
    .WithOrigins("http://localhost:5000") //only localhost
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
});

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(60),
});


app.UseMiddleware<ErrorHandlerMiddleware>(); //custom global error handler
app.UseMiddleware<JwtMiddleware>(); //custom jwt auth middleware

app.UseMiddleware<ConversationMiddleware>();

app.Run();
