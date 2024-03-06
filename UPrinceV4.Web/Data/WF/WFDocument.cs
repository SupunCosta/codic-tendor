using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.WF;

public class WFDocument
{
    public string Id { get; set; }
    public string Link { get; set; }
    public WFHeader WFHeader { get; set; }
    [ForeignKey("WFHeader")] public string WFHeaderId { get; set; }
}

public class WFDocumentDto
{
    public string Link { get; set; }
}