namespace EfAuditing.Entities;

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
