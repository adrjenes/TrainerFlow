using Microsoft.EntityFrameworkCore;
using TrainerFlow.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TrainerFlowDb")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:TrainerFlowDb");

builder.Services.AddDbContext<TrainerFlowDbContext>(options =>
    options.UseNpgsql(connectionString)
           .UseTrainerFlowSeeding());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TrainerFlowDbContext>();
    db.Database.Migrate();
}

app.MapGet("/health", () => Results.Ok("OK"));

app.Run();

