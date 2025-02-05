using Microsoft.EntityFrameworkCore;
using WebSocket.db;
using WebSocket.Extensions;
using WebSocket.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddServices()
    .AddAuth();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:5173")); // Додай адресу клієнта
});

var app = builder.Build();
app.UseCors();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<FinderHub>("/chats");
app.Run();

