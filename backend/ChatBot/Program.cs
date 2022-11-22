using ChatBot.Auth.Jwt;
using ChatBot.Extensions;
using ChatBot.Http;
using ChatBot.Middlewares;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Development",
        policy => { policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod(); });
});
builder.Services.AddControllers();
builder.Services.AddHttpClient<IUserHttpClient, UserHttpClient>();

var app = builder.Build();

app.Services.GetService<IAiClientService>();

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
