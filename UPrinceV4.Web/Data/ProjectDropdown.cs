using System.Collections.Generic;
using UPrinceV4.Web.Data.GL;
using UPrinceV4.Web.Data.ProjectClassification;

namespace UPrinceV4.Web.Data;

public class ProjectDropdown
{
    public IEnumerable<ProjectManagementLevelDto> ManagementLevels { get; set; }
    public IEnumerable<Currency> Currencies { get; set; }
    public IEnumerable<ProjectToleranceStateDto> ToleranceStates { get; set; }
    public IEnumerable<ProjectState> States { get; set; }
    public IEnumerable<ProjectTypeDto> Types { get; set; }
    public IEnumerable<ProjectTemplateDto> Templates { get; set; }
    public IEnumerable<GeneralLedgerNumber> GenaralLederNumber { get; set; }
    public IEnumerable<ProjectScopeStatus> ProjectScopeStatus { get; set; }
    public IEnumerable<ProjectFinancialStatus> ProjectFinancialStatus { get; set; }

    public IEnumerable<RoleDto> Role { get; set; }
    public IEnumerable<ProjectClassificationTypeDto> ProjectClassificationBuisnessUnit { get; set; }
    public IEnumerable<ProjectClassificationTypeDto> ProjectClassificationSize { get; set; }

    public IEnumerable<ProjectClassificationTypeDto> ProjectClassificationConstructionType { get; set; }

    public IEnumerable<ProjectClassificationTypeDto> ProjectClassificationSector { get; set; }
    public IEnumerable<ProjectLanguageDto> ProjectLanguage { get; set; }
}