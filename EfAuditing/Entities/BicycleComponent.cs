namespace EfAuditing.Entities;

public class BicycleComponent
{
    public int Id { get; set; }
    public int BicycleId { get; set; }
    public int ComponentId { get; set; }
    public Component Component { get; set; } = null!;
    public Bicycle Bicycle { get; set; } = null!;
}
