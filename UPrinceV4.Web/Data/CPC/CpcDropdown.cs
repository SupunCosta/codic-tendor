using System.Collections.Generic;

namespace UPrinceV4.Web.Data.CPC;

public class CpcDropdown
{
    public IEnumerable<CpcBasicUnitOfMeasure> CpcBasicUnitOfMeasure { get; set; }
    public IEnumerable<CpcResourceFamily> CpcResourceFamily { get; set; }
    public IEnumerable<CpcResourceType> CpcResourceType { get; set; }
    public IEnumerable<CpcMaterial> CpcMaterial { get; set; }
    public IEnumerable<CpcPressureClass> CpcPressureClass { get; set; }
    public IEnumerable<CpcUnitOfSizeMeasure> CpcUnitOfSizeMeasure { get; set; }
    public IEnumerable<CpcBrand> CpcBrand { get; set; }
}