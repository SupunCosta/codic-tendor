using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PL;

public class PLType
{
    public string Id { get; set; }
    public string TypeId { get; set; }
    public string Name { get; set; }
    public string DisplayOrder { get; set; }
    public string Type { get; set; }


    public PLPriceList PriceList { get; set; }
    [ForeignKey("PriceListId")] public string PriceListId { get; set; }
}