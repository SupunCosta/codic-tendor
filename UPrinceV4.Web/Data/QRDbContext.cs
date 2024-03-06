using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.CAB;


namespace UPrinceV4.Web.Data;

public class QRDbContext : DbContext
{
    private readonly string _connection;

    public QRDbContext(DbContextOptions<QRDbContext> options, string connection, ITenantProvider tenant) :
        base(options)
    {
        _connection = connection;
    }


    public DbSet<TimeClockActivityType> TimeClockActivityType { get; set; }
    
    public DbSet<QRCode> QRCode { get; set; }
    
    public DbSet<QrHistoryLog> QrHistoryLog { get; set; }
  
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connection);
    }
}