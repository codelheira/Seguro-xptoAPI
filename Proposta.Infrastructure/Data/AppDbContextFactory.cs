using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Proposta.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseSqlite("Data Source=proposta.db",
            x => x.MigrationsAssembly("Proposta.Infrastructure"));

        return new AppDbContext(optionsBuilder.Options);
    }
}