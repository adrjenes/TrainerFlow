using Microsoft.EntityFrameworkCore;
using TrainerFlow.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TrainerFlowDb")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:TrainerFlowDb");

builder.Services.AddDbContext<TrainerFlowDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

app.MapGet("/health", () => Results.Ok("OK"));

app.Run();

