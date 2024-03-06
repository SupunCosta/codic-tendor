using System.Collections.Generic;

namespace UPrinceV4.Web.Data.CPC;

public class CpcBasicUnitOfMeasure
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int DisplayOrder { get; set; }
    public string LocaleCode { get; set; }
    public IList<CpcBasicUnitOfMeasureLocalizedData> CpcBasicUnitOfMeasureLocalizedData { get; set; }
}

public class CpcBasicUnitOfMeasureDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}