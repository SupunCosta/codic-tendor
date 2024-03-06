using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WH;

public class WHHistoryLog
{
    public string Id { get; set; }
    public WHHeader WHHeader { get; set; }
    [ForeignKey("WHHeader")] public string WareHouseId { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}