using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Data;

public class ShiftDbContext : DbContext
{
    private readonly string _connection;

    public ShiftDbContext(DbContextOptions<ShiftDbContext> options, string connection, ITenantProvider tenant) :
        base(options)
    {
        _connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connection);
    }


    public DbSet<Shift> Shifts { get; set; }
    
    public DbSet<WorkflowState> WorkflowState { get; set; }
    
    
}