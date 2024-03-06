using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.ProjectLocationDetails;

namespace UPrinceV4.Web.Data;

public class ProjectDefinitionDbContext : DbContext
{
    private readonly string _connectionString;

    public ProjectDefinitionDbContext(DbContextOptions<ProjectDefinitionDbContext> options, ITenantProvider tenant)
    {
        _connectionString = tenant.GetTenant().ConnectionString;
    }


    public DbSet<ProjectDefinition> ProjectDefinition { get; set; }
    public DbSet<MapLocation> MapLocation { get; set; }
    
    public DbSet<ProjectCostConversion> ProjectCostConversion { get; set; }
    
    public DbSet<ProjectFinance> ProjectFinance { get; set; }
    
    
    public DbSet<Position> Position { get; set; }
    
    public DbSet<BoundingPoint> BoundingPoint { get; set; }
    
    public DbSet<Address> Address { get; set; }
    public DbSet<Geometry> Geometry { get; set; }
    
    public DbSet<DataSources> DataSources { get; set; }
    
    public DbSet<CalendarTemplate> CalendarTemplate { get; set; }
    
    public DbSet<ProjectTime> ProjectTime { get; set; }
    
    public DbSet<ProjectTeamRole> ProjectTeamRole { get; set; }
    
    public DbSet<ProjectType> ProjectType { get; set; }
    public DbSet<ProjectManagementLevel> ProjectManagementLevel { get; set; }
    
    public DbSet<ProjectToleranceState> ProjectToleranceState { get; set; }
    
    public DbSet<ProjectDefinitionHistoryLog> ProjectDefinitionHistoryLog { get; set; }
    
    public DbSet<ProjectKPI> ProjectKPI { get; set; }
    
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}