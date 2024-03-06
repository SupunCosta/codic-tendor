using System.Collections.Generic;

namespace UPrinceV4.Web.Data.Stock;

public class StockDropDownData
{
    public IEnumerable<StockTypeDto> ResourceTypes { get; set; }
    public IEnumerable<StockStatusDto> Status { get; set; }
    public IEnumerable<StockActiveTypeDto> Types { get; set; }
}