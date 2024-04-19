using Microsoft.EntityFrameworkCore;

namespace cp2.Extensions;

public static class MigrationExtension
{
    public static void AplicarMigrations(this WebApplication app)
    {
        using var escopo = app.Services.CreateScope();

        var dbContext = escopo.ServiceProvider.GetRequiredService<EncurtadorDbContext>();
        
        dbContext.Database.Migrate();
    }
}