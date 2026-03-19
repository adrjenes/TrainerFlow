using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Offers.DependencyInjection;
using TrainerFlow.Persistence;
using TrainerFlow.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TrainerFlowDb")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:TrainerFlowDb");

builder.Services.AddPersistence(connectionString);
builder.Services.AddOffersModule();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TrainerFlowDbContext>();
    db.Database.Migrate();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapGet("/health", () => Results.Ok("OK"));

app.Run();