using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class UtcDateInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ConvertDatesToUtc(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ConvertDatesToUtc(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ConvertDatesToUtc(DbContext context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                var properties = entry.Properties;

                foreach (var property in properties)
                {
                    if (property.Metadata.ClrType == typeof(DateTime))
                    {
                        if (property.CurrentValue is DateTime dateTime)
                        {
                            if (dateTime.Kind == DateTimeKind.Unspecified || dateTime.Kind == DateTimeKind.Local)
                            {
                                property.CurrentValue = dateTime.ToUniversalTime();
                            }
                        }
                    }
                    else if (property.Metadata.ClrType == typeof(DateTime?))
                    {
                        if (property.CurrentValue is DateTime dateTime)
                        {
                            if (dateTime.Kind == DateTimeKind.Unspecified || dateTime.Kind == DateTimeKind.Local)
                            {
                                property.CurrentValue = (DateTime?)dateTime.ToUniversalTime();
                            }
                        }
                    }
                }
            }
        }
    }
}
