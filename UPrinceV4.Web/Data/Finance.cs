using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectFinance
{
    public string Id { get; set; }
    public double? TotalBudget { get; set; }
    public double? BudgetLabour { get; set; }
    public double? BudgetMaterial { get; set; }

    [ForeignKey("Currency")] public int? CurrencyId { get; set; }
    public virtual Currency Currency { get; set; }

    [ForeignKey("ProjectDefinition")] public string ProjectId { get; set; }
    public virtual ProjectDefinition ProjectDefinition { get; set; }
    public string Invoiced { get; set; }
    public string Paid { get; set; }
    public string TenderBudget { get; set; }
    public string CustomerBudget { get; set; }
    public string DifferenceTenderAndCustomer { get; set; }
    public string CustomerBudgetSpent { get; set; }
    public string ToBeInvoiced { get; set; }
    public string MinWork { get; set; }
    public string ExtraWork { get; set; }
    public string MinAndExtraWork { get; set; }
    public string ExpectedTotalProjectCost { get; set; }
    public string DifferenceEstimatedCostAndTenderBudget { get; set; }
}

public class ProjectFinanceCreateDto
{
    public double? TotalBudget { get; set; }
    public double? BudgetLabour { get; set; }
    public double? BudgetMaterial { get; set; }
    public int? CurrencyId { get; set; }
    public string ProjectId { get; set; }
    public string Invoiced { get; set; }
    public string Paid { get; set; }
    public string TenderBudget { get; set; }
    public string CustomerBudget { get; set; }
    public string DifferenceTenderAndCustomer { get; set; }
    public string CustomerBudgetSpent { get; set; }
    public string ToBeInvoiced { get; set; }
    public string MinWork { get; set; }
    public string ExtraWork { get; set; }
    public string MinAndExtraWork { get; set; }
    public string ExpectedTotalProjectCost { get; set; }
    public string DifferenceEstimatedCostAndTenderBudget { get; set; }
}

public class ProjectFinanceUpdateDto
{
    public string Id { get; set; }
    public double? TotalBudget { get; set; }
    public double? BudgetLabour { get; set; }
    public double? BudgetMaterial { get; set; }
    public int? CurrencyId { get; set; }
    public string ProjectId { get; set; }
    public string Invoiced { get; set; }
    public string Paid { get; set; }
    public string TenderBudget { get; set; }
    public string CustomerBudget { get; set; }
    public string DifferenceTenderAndCustomer { get; set; }
    public string CustomerBudgetSpent { get; set; }
    public string ToBeInvoiced { get; set; }
    public string MinWork { get; set; }
    public string ExtraWork { get; set; }
    public string MinAndExtraWork { get; set; }
    public string ExpectedTotalProjectCost { get; set; }
    public string DifferenceEstimatedCostAndTenderBudget { get; set; }
}