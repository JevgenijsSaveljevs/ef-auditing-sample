using EfAuditing.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfAuditing;

internal class Program
{
    private static void PrintSampleText(string text) { Console.WriteLine(); Console.WriteLine(new string('=', text.Length + 6)); Console.WriteLine(text); Console.WriteLine(new string('=', 20)); Console.WriteLine(); }
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
