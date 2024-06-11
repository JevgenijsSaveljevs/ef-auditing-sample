using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfAuditing;


//docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=P@ssw0rd123#" -p 1433:1433 -d --rm mcr.microsoft.com/mssql/server:2022-latest
internal class Program
{
    private static void PrintSampleText(string text) { Console.WriteLine(); Console.WriteLine(new string('=', 20)); Console.WriteLine(text); Console.WriteLine(new string('=', 20)); Console.WriteLine(); }
    static async Task Main(string[] args)
    {
        await AddDiverge();
        await UpdateDivergeModel();
        await UpdateSuspensionComponent();
    }

    private static async Task UpdateSuspensionComponent()
    {
        PrintSampleText("add collection item");
        var ctx = new BicycleAppContext();
        var diverge = ctx.Set<Bicycle>().Include(x => x.BicycleComponents).First(x => x.Model == "2022 Diverge Comp E5");
        var component = new Component() { Name = "Future Shock 1.5 w/Smooth Boot, FACT carbon, 12x100 mm thru-axle, flat-mount" };
        diverge.BicycleComponents.Add(new() { Component = component, BicycleId = diverge.Id });

        await ctx.SaveChangesAsync();
    }

    private static async Task UpdateDivergeModel()
    {
        PrintSampleText("Update Model name");
        var ctx = new BicycleAppContext();
        var diverge = ctx.Set<Bicycle>().First(x => x.Model == "Diverge");
        diverge.Model = "2022 Diverge Comp E5";

        await ctx.SaveChangesAsync();
    }

    private static async Task AddDiverge()
    {
        PrintSampleText("Add new entry");
        var ctx = new BicycleAppContext();

        if (ctx.Set<Bicycle>().Any(x => x.Model == "Diverge"))
        {
            return;
        }

        var component = ctx.Set<Component>()
            .FirstOrDefault(x => x.Name == "Shimano GRX") ?? new Component() { Name = "Shimano GRX" };

        var bike = new Bicycle()
        {
            Year = new DateTime(2024, 10, 1),
            Model = "Diverge",
            BrandId = 1,
            BicycleTypeCode = "Gravel"
        };

        bike.BicycleComponents = new()
        {
            new() { Bicycle = bike, Component = component },
        };
        var bikes = new List<Bicycle>() { bike };

        
        ctx.Add(bike);

        await ctx.SaveChangesAsync();
    }
}

public class BicycleAppContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new AuditInterceptor());
        optionsBuilder
            .UseSqlServer("Data Source=localhost,1433;Initial Catalog=master;User ID=sa;Password=P@ssw0rd123#;Trust Server Certificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BicycleTypeConfiguration).Assembly);
    }
}

public class BicycleTypeConfiguration : IEntityTypeConfiguration<BicycleType>
{
    public void Configure(EntityTypeBuilder<BicycleType> builder)
    {
        builder.ToTable("BicycleType", "dbo");
        builder.HasKey(x => x.Code);
        //builder.Property(x => x.Code);
    }
}

public class BicycleComponentConfiguration : IEntityTypeConfiguration<BicycleComponent>
{
    public void Configure(EntityTypeBuilder<BicycleComponent> builder)
    {
        builder.ToTable("BicycleComponent", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Component).WithMany().HasForeignKey(nameof(BicycleComponent.ComponentId));
        builder.HasOne(x => x.Bicycle).WithMany().HasForeignKey(nameof(BicycleComponent.BicycleId));
    }
}

public class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        builder.ToTable("Component", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brand", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}

public class BicycleConfiguration : IEntityTypeConfiguration<Bicycle>
{
    public void Configure(EntityTypeBuilder<Bicycle> builder)
    {
        builder.ToTable("Bicycle", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Brand).WithMany();
        builder.HasOne(x => x.BicycleType).WithMany();
        builder.HasMany(x => x.BicycleComponents).WithOne(x => x.Bicycle).HasForeignKey(x => x.BicycleId);
    }
}


public class BicycleType
{
    public required string Code { get; set; }
    public required string Name { get; set; }
}

public class BicycleComponent
{
    public int Id { get; set; }
    public int BicycleId { get; set; }
    public int ComponentId { get; set; }
    public Component Component { get; set; } = null!;
    public Bicycle Bicycle { get; set; } = null!;
}

public class Component
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class Brand
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

public class Bicycle
{
    public int Id { get; set; }
    public required string Model { get; set; }
    public required DateTime Year { get; set; }
    public Brand Brand { get; set; } = null!;
    public int BrandId { get; set; }
    public BicycleType BicycleType { get; set; } = null!;
    public required string BicycleTypeCode { get; set; }
    public List<BicycleComponent> BicycleComponents { get; set; } = null!;
}

public class AuditInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
     {
        var entries = eventData.Context.ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            Console.WriteLine(entry.Metadata.Name);
            Console.WriteLine(entry.State);
            Console.WriteLine("PROPERTIES:");
            var props = entry.Properties;
            foreach (var property in props)
            {
                Console.WriteLine($"\tprop: {property.Metadata.Name}; is modified {property.IsModified}; [old.val: {property.OriginalValue}, new val: {property.CurrentValue}]");
            }
            Console.WriteLine();
        }

        return new(result);
     }
}