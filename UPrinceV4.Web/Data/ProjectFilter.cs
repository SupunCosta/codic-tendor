namespace UPrinceV4.Web.Data;

public class ProjectFilter
{
    //public string Title => "%" + Title + "%";
    public string Title { get; set; }
    public string ProjectTypeId { get; set; }
    public string ManagementLevelId { get; set; }
    public string ToleranceStateId { get; set; }
    public string ToleranceState { get; set; }
    public string TemplateId { get; set; }
    public bool History { get; set; }
    public string ProjectManagerId { get; set; }
    public string ProjectStatus { get; set; }
    public string CustomerId { get; set; }
    public string CustomerCompanyId { get; set; }
    public string ProjectClassificationSectorId { get; set; }
    public string ContractingUnit { get; set; }
    public string SiteManagerId { get; set; }

    public Sorter Sorter { get; set; }
    public string BuId { get; set; }
    public string ciawNumber { get; set; }
    public string projectAddress { get; set; }

        
}

public class Sorter
{
    public string Attribute { get; set; }
    public string Order { get; set; }
}

public class ProjectFilterProjectPlanDto
{
    public string BuId { get; set; }
    public string Title { get; set; }
}