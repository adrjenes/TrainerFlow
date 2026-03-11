using Microsoft.EntityFrameworkCore;
using TrainerFlow.Modules.Identity.Domain;
using TrainerFlow.Modules.Offers.Domain;
using TrainerFlow.Modules.Orders.Domain;

namespace TrainerFlow.Persistence;

public sealed class TrainerFlowDbContext : DbContext
{
    public TrainerFlowDbContext(DbContextOptions<TrainerFlowDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<PasswordSetupToken> PasswordSetupTokens => Set<PasswordSetupToken>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrainerFlowDbContext).Assembly);
    }
}