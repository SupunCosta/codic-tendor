using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PL;

public class PLStatus
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public string StatusId { get; set; }
    public string DisplayOrder { get; set; }

    public List<PLPriceList> PLPriceList { get; set; }
}