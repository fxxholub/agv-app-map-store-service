using AgvAppMapStoreService.Core;
using Microsoft.EntityFrameworkCore;

namespace AgvAppMapStoreService.Data;

public class MapDbContext : DbContext
{
    public MapDbContext(DbContextOptions<MapDbContext> options) : base(options)
    {
    }

    public DbSet<Map> Maps { get; set; }
}