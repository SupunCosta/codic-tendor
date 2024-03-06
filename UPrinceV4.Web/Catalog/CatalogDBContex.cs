using Microsoft.EntityFrameworkCore;

namespace UPrinceV4.Web.Catalog;

public class CatalogDBContex : DbContext
{
    public CatalogDBContex(DbContextOptions<CatalogDBContex> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenant { get; set; }
}