using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.CPC;

public class CpcResourceFamily
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string LocaleCode { get; set; }

    //[ForeignKey("CpcResourceFamily")]
    public string ParentId { get; set; }
    //public virtual CpcResourceFamily Parent { get; set; }

    public int DisplayOrder { get; set; }
    public IList<CpcResourceFamilyLocalizedData> CpcResourceFamilyLocalizedData { get; set; }
}

public class CpcResourceFamilyDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsChildren { get; set; }
    public string CPCId { get; set; }
    public string ResourceTypeId { get; set; }
    public string PoId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public double Percentage { get; set; }
}

public class CpcResourceFamilyDtoDapper
{
    public string Key { get; set; }
    public string Text { get; set; }
}