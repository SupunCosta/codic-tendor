using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Data;

public class TimeClockDbContext : DbContext
{
    private readonly string _connection;

    public TimeClockDbContext(DbContextOptions<TimeClockDbContext> options, string connection, ITenantProvider tenant) :
        base(options)
    {
        _connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connection);
    }


    public DbSet<Shift> Shifts { get; set; }

    public DbSet<QRCode> QRCode { get; set; }
    
    public DbSet<TimeClock> TimeClock { get; set; }
    
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    
}