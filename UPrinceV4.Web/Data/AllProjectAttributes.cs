using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data;

[NotMapped]
public class AllProjectAttributes
{
    public string ProjectDefinitionId { get; set; }
    public string Name { get; set; }
    public string SequenceCode { get; set; }
    public string ProjectTypeName { get; set; }
    public string ProjectManagementLevelName { get; set; }
    public string ProjectTemplateName { get; set; }
    public string ProjectToleranceStateName { get; set; }
    public string Title { get; set; }
    public string ProjectManagerName { get; set; }
    public string SiteManagerName { get; set; }
    public string ProjectManagerCabPersonId { get; set; }
    public string ProjectStatusName { get; set; }
    public string CustomerName { get; set; }
    public string SectorName { get; set; }
    public string ContractingUnitId { get; set; }
    public string ProjectAddress { get; set; }
    public string CiawNumber { get; set; }
    public string CustomerCompanyName { get; set; }

}

public class UserProjectList
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceCode { get; set; }
    public string ProjectConnectionString { get; set; }
    public string Title { get; set; }
    public string Cu { get; set; }

}