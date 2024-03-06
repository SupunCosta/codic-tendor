using System.Collections.Generic;
using UPrinceV4.Web.Data.Stock;

namespace UPrinceV4.Web.Data.WF;

public class WFDropDownData
{
    public IEnumerable<WFTypeDto> Types { get; set; }

    public IEnumerable<WFActivityStatusDto> ActivityStatus { get; set; }
    public IEnumerable<StockTypeDto> ResourceType { get; set; }
}