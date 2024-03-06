using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Data;

public class LocationDbContext : DbContext
{
    private readonly string _connection;

    public LocationDbContext(DbContextOptions<LocationDbContext> options, string connection, ITenantProvider tenant) :
        base(options)
    {
        _connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connection);
    }


    public DbSet<Location> Location { get; set; }
    
}