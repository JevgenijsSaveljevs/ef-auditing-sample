using EfAuditing.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfAuditing.EntityConfigurations;

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
