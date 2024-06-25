using EfAuditing.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace EfAuditing;

public class BicycleAppContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new AuditInterceptor());
        optionsBuilder
            .UseSqlServer("Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=P@ssw0rd123#;Trust Server Certificate=True;Encrypt=False");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BicycleTypeConfiguration).Assembly);
    }
}
