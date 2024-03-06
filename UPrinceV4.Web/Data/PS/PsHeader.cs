using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.PS;

public class PsHeader
{
    public string Id { get; set; }
    public string ProgressStatementId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string ProjectTypeId { get; set; }
    public string ProjectStatusId { get; set; }

    public DateTime? Date { get; set; }

    //[ForeignKey("ProjectCost")]
    //public string ProjectCostId { get; set; }
    //public virtual ProjectCost ProjectCost { get; set; }
    public DateTime? WorkPeriodFrom { get; set; }
    public DateTime? WorkPeriodTo { get; set; }
    public string PurchaseOrderNumber { get; set; }
    public double TotalAmount { get; set; }
    public string ProjectSequenceCode { get; set; }

    public string GeneralLedgerId { get; set; }
    public string InvoiceComment { get; set; }
}

public class PsHeaderCreateDto
{
    public string Id { get; set; }
    public string ProgressStatementId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string ProjectTypeId { get; set; }
    public string ProjectStatusId { get; set; }
    public string ProjectCostId { get; set; }
    public DateTime? WorkPeriodFrom { get; set; }
    public DateTime? WorkPeriodTo { get; set; }
    public string PurchaseOrderNumber { get; set; }
    public string GeneralLedgerId { get; set; }
    public string InvoiceComment { get; set; }
}

public class PsHeaderDropdowndto
{
    public string ProjectSequenceCode { get; set; }
}

public class PsHeaderReadDto
{
    public string Id { get; set; }
    public string ProgressStatementId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string ProjectTypeId { get; set; }
    public string ProjectType { get; set; }
    public string ProjectStatusId { get; set; }
    public string ProjectStatus { get; set; }
    public string ProjectCostId { get; set; }
    public double TotalAmount { get; set; }
    public DateTime? WorkPeriodFrom { get; set; }
    public DateTime? WorkPeriodTo { get; set; }
    public bool IsProjectFinished { get; set; }
    public DateTime? Date { get; set; }
    public string PurchaseOrderNumber { get; set; }
    public ProjectDefinitionHistoryLogDto historyLog { get; set; }
    public IEnumerable<PsResourceReadDto> Resources { get; set; }
    public string ProjectSequenceCode { get; set; }
    public string GeneralLedgerId { get; set; }
    public string GeneralLedgerValue { get; set; }

    public string AccountingNumber { get; set; }
    public string InvoiceComment { get; set; }
    public string ProjectScopeStatusId { get; set; }
    public string ProjectScope { get; set; }
}

public class PsFilterReadDto
{
    public string Id { get; set; }
    public string ProgressStatementId { get; set; }
    public string Status { get; set; }
    public string Title { get; set; }
    public DateTime? Date { get; set; }
    public double? TotalAmount { get; set; }
    public string Project { get; set; }
    public string ProjectSequenceCode { get; set; }

    public string ProjectScope { get; set; }
}

public class PsProjectDto
{
    public string ProjectScopeStatusId { get; set; }
    public string AccountingNumber { get; set; }

    public string ProjectScope { get; set; }
}