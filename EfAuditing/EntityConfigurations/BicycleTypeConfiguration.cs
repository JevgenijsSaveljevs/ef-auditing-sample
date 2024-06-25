using EfAuditing.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfAuditing.EntityConfigurations;

public class BicycleTypeConfiguration : IEntityTypeConfiguration<BicycleType>
{
    public void Configure(EntityTypeBuilder<BicycleType> builder)
    {
        builder.ToTable("BicycleType", "dbo");
        builder.HasKey(x => x.Code);
        //builder.Property(x => x.Code);
    }
}
