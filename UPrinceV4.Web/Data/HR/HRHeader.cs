using System;
using System.Collections.Generic;

namespace UPrinceV4.Web.Data.HR;

public class HRHeader
{
    public string Id { get; set; }
    public string PersonName { get; set; }
    public string PersonId { get; set; }
    public string Title { get; set; }
    public string SequenceId { get; set; }
    public string OrganizationTaxonomyId { get; set; }
    public string AzureOid { get; set; }
    public string Role { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public string CpcLabourItemId { get; set; }
    public string FontColor { get; set; }
    public string BgColor { get; set; }
    public bool IsContactManager { get; set; }
    public string WorkingOrganization { get; set; }
    public string Organization { get; set; }
}

public class CreateHRDto
{
    public string Id { get; set; }
    public string PersonName { get; set; }
    public string PersonId { get; set; }
    public string OrganizationTaxonomyId { get; set; }
    public string Role { get; set; }
    public List<WorkSchedule> WorkSchedule { get; set; }
    public string CpcLabourItemId { get; set; }
    public string FontColor { get; set; }
    public string BgColor { get; set; }
    public bool IsContactManager { get; set; }
    public string WorkingOrganization { get; set; }
    public string Organization { get; set; }
}

public class HRHistoryDto
{
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}

public class GetHRByIdDto
{
    public string Id { get; set; }
    public string PersonName { get; set; }
    public string PersonId { get; set; }
    public string Title { get; set; }
    public string SequenceId { get; set; }
    public string OrganizationTaxonomyId { get; set; }
    public string AzureOid { get; set; }
    public string Role { get; set; }
    public HRHistoryDto History { get; set; }
    public string CabPersonId { get; set; }
    public IEnumerable<GetWorkScheduleDto> WorkSchedule { get; set; }
    public string CpcLabourItemId { get; set; }
    public string CpcLabourItemTitle { get; set; }
    public string FontColor { get; set; }
    public string BgColor { get; set; }
    public bool IsContactManager { get; set; }
    public string WorkingOrganization { get; set; }
    public string WorkingOrganizationName { get; set; }
    public string Organization { get; set; }
    public string CompanyName { get; set; }
}

public class PersonById
{
    public string TaxonomyId { get; set; }
    public string AzureOid { get; set; }
}

public class GetHRListDto
{
    public string Id { get; set; }
    public string SequenceId { get; set; }
    public string JobTitle { get; set; }
    public string Organization { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string FullName { get; set; }
}

public class FilterHR
{
    public string JobTitle { get; set; }
    public string Organization { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string FullName { get; set; }
    public bool Active { get; set; } = false;
    public bool NonActive { get; set; } = false;
    public Sorter Sorter { get; set; }
}

public class Sorter
{
    public string Attribute { get; set; }
    public string Order { get; set; }
}

public class HRLabourPmolPr
{
    public string Project { get; set; }
    public List<PmolPr> Pmol { get; set; }
    public List<PmolPr> Pr { get; set; }

}

public class PmolPr
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string SequenceId { get; set; }

}