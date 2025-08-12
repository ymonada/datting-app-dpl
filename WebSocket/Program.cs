using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebSocket.db;
using WebSocket.Domain.Validators;
using WebSocket.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddServices()
    .AddAuth();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddValidators();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();
app.UseCors("AllowFrontend");
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

// app.MapHub<ChatHub>("/chat");

app.Run();

