using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.INV;
using UPrinceV4.Web.Data.PC;

namespace UPrinceV4.Web.Data.PS;

public class InvResource
{
    public string Id { get; set; }
    [ForeignKey("Invoice")] public string InvoiceId { get; set; }
    public virtual Invoice Invoice { get; set; }
    public string CpcId { get; set; }
    public string CpcResourceNumber { get; set; }
    public string CpcTitle { get; set; }
    public double? ConsumedQuantity { get; set; }
    public double? CostToMou { get; set; }
    public double? TotalCost { get; set; }
    public double? SoldQuantity { get; set; }
    public double? SpToMou { get; set; }
    public string ConsumedQuantityMou { get; set; }
    public string MouId { get; set; }
    public string Status { get; set; }
    [ForeignKey("CpcResourceType")] public string CpcResourceTypeId { get; set; }
    public virtual CpcResourceType CpcResourceType { get; set; }

    [ForeignKey("ProjectCost")] public string ProjectCostId { get; set; }
    public virtual ProjectCost ProjectCost { get; set; }
    public string ProductTitle { get; set; }
}

public class InvResourceDto
{
    public string Id { get; set; }
    public string PsId { get; set; }
    public string CpcId { get; set; }
    public string CpcResourceNumber { get; set; }
    public string CpcTitle { get; set; }
    public double? ConsumedQuantity { get; set; }
    public double? CostToMou { get; set; }
    public double? TotalCost { get; set; }
    public double? SoldQuantity { get; set; }
    public double? SpToMou { get; set; }
    public string ConsumedQuantityMou { get; set; }
    public string MouId { get; set; }
    public string Status { get; set; }
    public string CpcResourceTypeId { get; set; }
    public string ProjectCostId { get; set; }
    public string ProductTitle { get; set; }
}

public class InvResourceCreateDto
{
    public string PsId { get; set; }
    public string GrandTotal { get; set; }
    public IEnumerable<PsResourceDto> Resources { get; set; }
}

public class InvResourceeReadDto
{
    public string Id { get; set; }
    public string PsId { get; set; }
    public string CpcId { get; set; }
    public string CpcResourceNumber { get; set; }
    public string CpcTitle { get; set; }
    public double? ConsumedQuantity { get; set; }
    public double? CostToMou { get; set; }
    public double? TotalCost { get; set; }
    public double? SoldQuantity { get; set; }
    public double? SpToMou { get; set; }
    public string ConsumedQuantityMou { get; set; }
    public string MouId { get; set; }
    public string Mou { get; set; }
    public string Status { get; set; }
    public string CpcResourceTypeId { get; set; }
    public string CpcResourceType { get; set; }
    public string ProjectCostId { get; set; }
    public string ProductTitle { get; set; }
}