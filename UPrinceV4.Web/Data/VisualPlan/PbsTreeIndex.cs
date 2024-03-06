using System.Collections.Generic;

namespace UPrinceV4.Web.Data.VisualPlan;

public class PbsTreeIndex
{
    public string Id { get; set; }
    public string PbsProductId { get; set; }
    public int TreeIndex { get; set; }
}

public class PbsTreeIndexDto
{
    public List<PbsTreeIndex> PbsTreeIndex { get; set; }
}

public class PbsTreeIndexSibling
{
    public string PbsProductId { get; set; }
    public int TreeIndex { get; set; }
}