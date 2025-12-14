using Microsoft.EntityFrameworkCore;

namespace api.Data.Config;

public class BonusHackerDbContext : DbContext
{
    private IConfiguration _configuration;

    public BonusHackerDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("Postgres");
        optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
    }
    public DbSet<ScrapedProduct> ScrapedProducts { get; set; }
}   