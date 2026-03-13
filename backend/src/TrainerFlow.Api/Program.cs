using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Offers.Features.GetOffers;
using TrainerFlow.Persistence;
using TrainerFlow.Persistence.Features.Offers.GetOffers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TrainerFlowDb")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:TrainerFlowDb");

builder.Services.AddDbContext<TrainerFlowDbContext>(options =>
    options.UseNpgsql(connectionString)
           .UseTrainerFlowSeeding());

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GetOffersHandler>();
builder.Services.AddScoped<IOffersReadRepository, OffersReadRepository>();

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