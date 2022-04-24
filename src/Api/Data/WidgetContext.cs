using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Refactoring.Data;

#nullable disable

public class WidgetContext : DbContext
{
    public string DbPath { get; }

    public DbSet<Widget> Widgets { get; set; }

    public WidgetContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "widgets.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Widget>(e =>
        {
            e.HasMany(p => p.Properties).WithOne(p => p.Widget);
        });
    }
}
