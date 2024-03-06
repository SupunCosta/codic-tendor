using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.PO;

public class PODocument
{
    public string Id { get; set; }
    public string Link { get; set; }
    public POHeader POHeader { get; set; }
    [ForeignKey("POHeader")] public string POHeaderId { get; set; }
}