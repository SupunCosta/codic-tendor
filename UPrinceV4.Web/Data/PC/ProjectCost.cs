using System;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.PMOL;

namespace UPrinceV4.Web.Data.PC;

public class ProjectCost
{
    public string Id { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string ProductId { get; set; }
    public string ProductTitle { get; set; }
    public string BorId { get; set; }
    public string BorTitle { get; set; }
    [ForeignKey("Pmol")] public string PmolId { get; set; }
    public virtual Pmol Pmol { get; set; }
    public string PmolTitle { get; set; }
    public string PmolTypeId { get; set; }
    public string PmolType { get; set; }
    public bool? IsPlannedResource { get; set; }
    public DateTime? Date { get; set; }
    public string ResourceTypeId { get; set; }
    public string ResourceType { get; set; }
    public string OriginalPmolTypeId { get; set; }
    public string OriginalPmolType { get; set; }

    public string NewPmolTypeId { get; set; }
    public string NewPmolType { get; set; }

    public string ResourceNumber { get; set; }
    public string BusinessId { get; set; }
    public string PcTitle { get; set; }
    public string PcStatus { get; set; } //pmolstate
    public double? ConsumedQuantity { get; set; }
    public string Mou { get; set; }
    public double? CostMou { get; set; }
    public double? TotalCost { get; set; }
    public string ResourceTitle { get; set; }

    public bool isUsed { get; set; }
}

public class ProjectCostFilterDto
{
    public string Id { get; set; }
    public string ProductTitle { get; set; }
    public string BorTitle { get; set; }
    public DateTime? Date { get; set; }
    public string ResourceType { get; set; }
    public string ResourceTypeId { get; set; }
    public string PmolTitle { get; set; }
    public string ResourceNumber { get; set; }
    public string ResourceTitle { get; set; }
    public string Mou { get; set; }
    public double? ConsumedQuantity { get; set; }
    public double? CostMou { get; set; }

    public double? SpMou { get; set; }
    public double? TotalCost { get; set; }
    public string MouId { get; set; }

    public bool IsPlannedResource { get; set; }
    public string ProductType { get; set; }
}