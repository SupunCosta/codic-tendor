using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PL;

public class PLMarketType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string DisplayOrder { get; set; }

    public List<PLItem> PLItem { get; set; }
}