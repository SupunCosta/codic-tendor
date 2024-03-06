using System.Collections.Generic;
using UPrinceV4.Web.Data.WF;
using UPrinceV4.Web.Data.WH;

namespace UPrinceV4.Web.Data.Stock;

public class StockShortCutPane
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }

    public string TypeId { get; set; }
}

public class StockShortCutPaneCommon
{
    public IEnumerable<StockShortCutPane> Stock { get; set; }
    public IEnumerable<WFShortCutPane> WorkFlow { get; set; }
    public IEnumerable<WHShortCutPane> Warehouse { get; set; }
}

public class StockShortCutPaneDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}