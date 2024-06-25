using EfAuditing.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfAuditing.EntityConfigurations;

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
