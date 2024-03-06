using System.Collections.Generic;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Repositories.Interfaces.PBS;

public interface IPbsExperienceRepository
{
    IEnumerable<PbsExperienceDto> GetExperienceList(
        PbsExperienceRepositoryParameter pbsExperienceRepositoryParameter);
}

public class PbsExperienceRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
}