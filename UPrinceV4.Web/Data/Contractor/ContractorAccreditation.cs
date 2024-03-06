namespace UPrinceV4.Web.Data.Contractor;

public class ContractorAccreditation
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string CabPersonName { get; set; }
    public string Skill { get; set; }
    public string ExperienceId { get; set; }
    public string experienceName { get; set; }
    public string ContractorId { get; set; }
}

public class ContractorAccreditationDto
{
    public string Id { get; set; }
    public string CabPersonId { get; set; }
    public string CabPersonName { get; set; }
    public string Skill { get; set; }
    public string ExperienceId { get; set; }
    public string experienceName { get; set; }
    public string ContractorId { get; set; }
    public string Company { get; set; }
}