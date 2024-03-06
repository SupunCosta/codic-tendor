using System.Collections.Generic;

namespace UPrinceV4.Web.Data.CPC;

public class CpcResourceType
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LocaleCode { get; set; }
    public int DisplayOrder { get; set; }
    public IList<CpcResourceTypeLocalizedData> CpcResourceTypeLocalizedData { get; set; }
}

public class CpcResourceTypeDto
{
    public string key { get; set; }
    public string Text { get; set; }
}