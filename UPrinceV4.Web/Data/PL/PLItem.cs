using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Data.PL;

public class PLItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ItemPrise { get; set; }

    public PbsProduct PbsProduct { get; set; }
    [ForeignKey("PbsProduct")] public string ProductId { get; set; }

    public CorporateProductCatalog CorporateProductCatalog { get; set; }

    [ForeignKey("CorporateProductCatalog")]
    public string CorporateProductCatalogId { get; set; }

    public CpcBasicUnitOfMeasure CpcBasicUnitOfMeasure { get; set; }
    [ForeignKey("CpcBasicUnitOfMeasure")] public string CpcBasicUnitOfMeasureId { get; set; }

    public PLMarketType MarketType { get; set; }
    public string MarketTypeId { get; set; }

    public List<PLListsItems> PLListItems { get; set; }
}