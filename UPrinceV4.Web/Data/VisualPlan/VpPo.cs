using System;
using System.Collections.Generic;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.VisualPlan;

namespace UPrinceV4.Web.Data.VisualPlaane;

public class VpPo
{
    public string Id { get; set; }
    public string ParentId { get; set; }
    public string Title { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string LinkId { get; set; }
    public string Project { get; set; }
    public string BorId { get; set; }
    public string CCPCId { get; set; }
    public string PoId { get; set; }
    public virtual ICollection<VpPo> Children { get; set; }
}

public class GetVpPo
{
    public string Id { get; set; }
    public string ParentId { get; set; }
    public string Title { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string LinkId { get; set; }
    public string Project { get; set; }
    public string Quantity { get; set; }
    public string BorId { get; set; }
    public string CCPCId { get; set; }
    public string PoId { get; set; }
    public string ResourceType { get; set; }
    public virtual List<GetVpPo> Resources { get; set; }
    public IEnumerable<GetPOLabourTeam> Teams { get; set; }
    public IEnumerable<GetPOToolPool> ToolsPool { get; set; }
    public string RequestTypeId { get; set; }
    public string RequestTypeName { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? RequestedDate { get; set; }
    public DateTime? ExpectedDate { get; set; }

    public string OrderStatus { get; set; }
    public string IsWarfAvailable { get; set; }
    public bool? IsStock { get; set; }
    public bool IsPoApproved { get; set; }
}

public class VpPoFilter
{
    public List<string> RequestType { get; set; }
    public List<string> ResourceType { get; set; }
    public List<string> PoId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string Date { get; set; }
    public bool IsAllProjects { get; set; } = false;
}

public class PbsTreeStructureFilter
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string Title { get; set; }
    public string ProjectTitle { get; set; }
    public string productStatus { get; set; }
    public List<string> BusinessUnit { get; set; }
}

public class StockVp
{
    public string CPCId { get; set; }
    public string WareHouseTaxonomyId { get; set; }
}

public class VpWHTaxonomyListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string WareHouseId { get; set; }
    public string ParentId { get; set; }
    public string WHTaxonomyLevelId { get; set; }
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

public class UpdatePersonsDate
{
    public List<GetPOLabourTeam> PersonId { get; set; }
    public List<GetVpWHDto> TaxonomyId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetByDate
{
    public List<GetVpHR> LabourTeam { get; set; }
    public List<GetVpWHDto> Tools { get; set; }
}

public class DateFilter
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class AddPmolPlannedWork
{
    public string Id { get; set; }
    public string CoperateProductCatalogId { get; set; }
    public double? RequiredQuantity { get; set; }
    public double? ConsumedQuantity { get; set; }
    public string CpcBasicUnitofMeasureId { get; set; }
    public string PmolId { get; set; }
    public string Type { get; set; }
    public string PersonId { get; set; }
    public string PmolLabourId { get; set; }
    public string RoleId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class VpProjectWithPm
{
    public string ProjectId { get; set; }

    public string ProjectTitle { get; set; }

    public string ContractingUnitId { get; set; }
    public string CabPersonId { get; set; }
    public string SequenceCode { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }

    public string ProjectConnectionString { get; set; }

    public List<PmolListDtoForMobile> Pmols { get; set; }

    public List<BorListDto> BorList { get; set; }

    public List<Pbs> PbsList { get; set; }
    public string Oid { get; set; }
}

public class UpdateBorPbsForVp
{
    public string Type { get; set; }
    public string SequenceId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class PbsProductTaxonomyTree
{
    public string ParentPbsSequenceId { get; set; }
    public string ChildPbsSequenceId { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ContractingUnit { get; set; }
}

public class TestVp
{
    public string Link { get; set; }
    public string PdfType { get; set; }
}

public class BorVpFilter
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class CreatePr
{
    public string BorId { get; set; }
}