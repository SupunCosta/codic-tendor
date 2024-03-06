using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

public class ProjectFinanceHistory
{
    public string Id { get; set; }
    public double? TotalBudget { get; set; }
    public double? BudgetLabour { get; set; }
    public double? BudgetMaterial { get; set; }

    public int CurrencyId { get; set; }
    [NotMapped] public virtual Currency Currency { get; set; }

    public string ProjectId { get; set; }
    [NotMapped] public virtual ProjectDefinition ProjectDefinition { get; set; }
    public string ChangeByUserId { get; set; }
    [NotMapped] public virtual ApplicationUser ChangeByUser { get; set; }
    public string Action { get; set; }
    public int RevisionNumber { get; set; }
    [Key] public DateTime SysStartTime { get; set; }
    public DateTime SysEndTime { get; set; }
}