using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class Product
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [ForeignKey("ProductCategory")] public int CategoryId { get; set; }
    public virtual ProductCategory Category { get; set; }
}