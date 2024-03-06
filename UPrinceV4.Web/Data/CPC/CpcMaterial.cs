using System.Collections.Generic;

namespace UPrinceV4.Web.Data.CPC;

public class CpcMaterial
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LocaleCode { get; set; }
    public int DisplayOrder { get; set; }
    public IList<CpcMaterialLocalizedData> CpcMaterialLocalizedData { get; set; }
}

public class CpcMaterialDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}