using System.Collections.Generic;

namespace UPrinceV4.Web.Data.DD;

public class DropDownList
{
    public CoporateProductCatalog CoporateProductCatalog { get; set; }
    public Project Project { get; set; }
    public ProjectBreakDownStructure ProjectBreakDownStructure { get; set; }

    public ProjectMolecule ProjectMolecule { get; set; }
    //public BillOfResource BillOfResource { get; set; }
}

public class CoporateProductCatalog
{
    public IEnumerable<CpcBasicUnitOfMeasure> CpcBasicUnitOfMeasure { get; set; }
    public IEnumerable<CpcResourceFamily> CpcResourceFamily { get; set; }
    public IEnumerable<CpcResourceType> CpcResourceType { get; set; }

    public IEnumerable<CpcMaterial> CpcMaterial { get; set; }

    //public IEnumerable<CpcPressureClass> CpcPressureClass { get; set; }
    //public List<CpcUnitOfSizeMeasure> CpcUnitOfSizeMeasure { get; set; }
    public IEnumerable<CpcBrand> CpcBrand { get; set; }
}

public class CpcBasicUnitOfMeasure
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class CpcResourceFamily
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class CpcResourceType
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class CpcMaterial
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}
//public class CpcPressureClass
//{
//    public string Country { get; set; }
//    public string Language { get; set; }
//    public string LanguageCode { get; set; }

//}
//public class CpcUnitOfSizeMeasure
//{
//    public string Country { get; set; }
//    public string Language { get; set; }
//    public string LanguageCode { get; set; }

//}   
public class CpcBrand
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class Project
{
    public IEnumerable<MagementLevel> MagementLevel { get; set; }

    //public List<Currncies> Currncies { get; set; }
    public IEnumerable<ToleranceState> ToleranceState { get; set; }
    public IEnumerable<Types> Types { get; set; }

    public IEnumerable<Templates> Templates { get; set; }

    //public List<GeneralLedgerNumber> GeneralLedgerNumber { get; set; }
    public IEnumerable<States> States { get; set; }
}

public class MagementLevel
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}
//public class Currncies
//{
//    public string Id { get; set; }

//}
public class ToleranceState
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class Types
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class Templates
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}
//public class GeneralLedgerNumber
//{
//    public string Id { get; set; }

//}
public class States
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class ProjectBreakDownStructure
{
    public IEnumerable<ProjectBreakDownStructureToleranceState> ProjectBreakDownStructureToleranceState { get; set; }

    public IEnumerable<ProductState> ProductState { get; set; }
    public IEnumerable<ItemTypes> ItemTypes { get; set; }
    public IEnumerable<Experions> Experions { get; set; }
    public IEnumerable<Skills> Skills { get; set; }
}

public class ProductState
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class ProjectBreakDownStructureToleranceState
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class ItemTypes
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class Experions
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class Skills
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class ProjectMolecule
{
    public IEnumerable<ProjectMoleculeStaus> Staus { get; set; }
    public IEnumerable<ProjectMoleculeType> Type { get; set; }
}

public class ProjectMoleculeStaus
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class ProjectMoleculeType
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}

public class BillOfResource
{
    public IEnumerable<BorStatus> BorStatus { get; set; }
}

public class BorStatus
{
    public string Country { get; set; }
    public string Language { get; set; }
    public string LanguageCode { get; set; }
}