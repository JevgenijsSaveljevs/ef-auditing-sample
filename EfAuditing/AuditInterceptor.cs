using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EfAuditing;

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