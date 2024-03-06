using UPrinceV4.Web.Data.PO;

namespace UPrinceV4.Web.Data.CPC;

public class POCpcAverage : POMetaData
{
    public string Id { get; set; }
    public string CpcId { get; set; }
    public string PoId { get; set; }
    public string Price { get; set; }
}

public class POCpcAverageDto
{
    public string Id { get; set; }
    public string CpcId { get; set; }
    public string PoId { get; set; }
    public string Price { get; set; }
}