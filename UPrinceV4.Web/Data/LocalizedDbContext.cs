using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Data;

public class LocalizedDbContext : DbContext
{
    private readonly string _connection;

    public LocalizedDbContext(DbContextOptions<LocalizedDbContext> options, string connection, ITenantProvider tenant) :
        base(options)
    {
        _connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connection);
    }


    public DbSet<ErrorMessage> ErrorMessage { get; set; }
    
    public DbSet<LocalizedData> LocalizedData { get; set; }
    
    
}