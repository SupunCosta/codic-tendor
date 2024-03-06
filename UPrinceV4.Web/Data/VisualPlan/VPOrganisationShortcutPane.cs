using System.Collections.Generic;
using UPrinceV4.Web.Data.VisualPlan;

namespace UPrinceV4.Web.Data.VisualPlaane;

public class VPOrganisationShortcutPane
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public string OrganisationId { get; set; }
}

public class VPShortCutPaneCommon
{
    public IEnumerable<OrganizationTaxonomyLevel> Organisation { get; set; }
}

public class VPOrganisationShortcutPaneDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string OrganisationId { get; set; }
    public string LanguageCode { get; set; }
    public int DisplayOrder { get; set; }
    public string Label { get; set; }
}