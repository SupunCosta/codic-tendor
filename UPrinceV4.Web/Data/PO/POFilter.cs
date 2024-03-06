using System;

namespace UPrinceV4.Web.Data.PO;

public class POFilter
{
    public string Title { get; set; }
    public string StatusId { get; set; }
    public string ProjectId { get; set; }
    public string PurchaseOrderTitle { get; set; }
    public string ThirdParty { get; set; }
    public string Ammount { get; set; }
    public string RequestTypeId { get; set; }

    public string TotalAmount { get; set; }


    public Sorter Sorter { get; set; }

    public string Customer { get; set; }
    public string Supplier { get; set; }

    public DateTime? LastModifiedDate { get; set; }
    public string Offset { get; set; }
    public DateTime LocalDate { get; set; }
    public int? Date { get; set; }

    public string ModifiedBy { get; set; }
    public string Resource { get; set; }

    public string ProjectSequenceCode { get; set; }
}

public class POFilterDto
{
    public string Status { get; set; }
    public string ProjectTitle { get; set; }
    public string PurchaseOrderTitle { get; set; }
    public string ThirdParty { get; set; }
    public string Ammount { get; set; }

    public string Customer { get; set; }
    public string Suplier { get; set; }
}

public class POBorResourceFilter
{
    public string BorTitle { get; set; }
    public string PbsTitle { get; set; }
    public string ResourceTitle { get; set; }
    public string ResourceFamily { get; set; }
    public string ResourceTypeId { get; set; }

    public Sorter Sorter { get; set; }

    public string ProjectSequenceCode { get; set; }
}

public class POPbsResourceFilter
{
    public string Title { get; set; }
    public string PbsProductItemTypeId { get; set; }
    public string PbsProductStatusId { get; set; }
    public string PbsToleranceStateId { get; set; }
    public string Scope { get; set; }
    public string Taxonomy { get; set; }
    public Sorter Sorter { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string Type { get; set; }
}