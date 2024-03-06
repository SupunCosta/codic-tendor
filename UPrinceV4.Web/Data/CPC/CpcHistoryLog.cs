using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CPC;

public class CpcHistoryLog
{
    public string Id { get; set; }
    public string HistoryLog { get; set; }

    [ForeignKey("CorporateProductCatalog")]
    public string CoperateProductCatalogId { get; set; }

    public virtual CorporateProductCatalog CorporateProductCatalog { get; set; }

    public string ChangedByUserId { get; set; }

    //public virtual ApplicationUser ChangedByUser { get; set; }
    public string Action { get; set; }
    public DateTime ChangedTime { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RevisionNumber { get; set; }
}