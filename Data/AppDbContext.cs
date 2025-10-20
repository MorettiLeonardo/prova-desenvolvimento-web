using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Consumo> Consumos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=leonardo_guilherme.db");
    }
}