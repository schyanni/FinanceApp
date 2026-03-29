using FinancingApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace FinancingApp.Persistence;

public class FinancingAppContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<Category> Categories { get; set; }

    private const string Path = "financing.db";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite($"Data Source={Path}");
    }
}