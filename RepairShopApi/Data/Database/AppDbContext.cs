using Microsoft.EntityFrameworkCore;

namespace RepairShopApi.Data.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Request> Requests { get; set; } = null!;
    public virtual DbSet<Device> Devices { get; set; } = null!;
    public virtual DbSet<RepairInfo> Repairs { get; set; } = null!;
    public virtual DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
