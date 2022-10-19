using Fake_API.Service;

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
builder.Services.AddSingleton<UserService>();



builder.Services.AddCors(options => {
    options.AddPolicy(name: "Development",
        policy  =>
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

app.Run();
