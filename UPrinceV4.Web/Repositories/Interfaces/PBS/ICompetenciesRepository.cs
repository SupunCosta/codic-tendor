using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;

namespace UPrinceV4.Web.Repositories.Interfaces.PBS;

public interface ICompetenciesRepository
{
    Task<IEnumerable<PbsSkillExperience>> GetCompetencies(
        CompetenciesRepositoryParameter competenciesRepositoryParameter);

    Task<PbsSkillExperience> GetCompetenceById(CompetenciesRepositoryParameter competenciesRepositoryParameter);
    Task<string> AddCompetence(CompetenciesRepositoryParameter competenciesRepositoryParameter);
    Task<bool> DeleteCompetencies(CompetenciesRepositoryParameter competenciesRepositoryParameter);

    Task<CompetenciesDropdown> GetCompetenciesDropdownData(
        CompetenciesRepositoryParameter competenciesRepositoryParameter);

    Task<IEnumerable<PbsSkillExperienceDto>> GetCompetenceByPbsId(
        CompetenciesRepositoryParameter competenciesRepositoryParameter);
}

public class CompetenciesDropdown
{
    public IEnumerable<PbsExperienceDto> PbsExperience { get; set; }
    public IEnumerable<PbsSkillDto> PbsSkills { get; set; }
}

public class CompetenciesRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public IPbsSkillRepository IPbsSkillRepository { get; set; }
    public PbsSkillRepositoryParameter PbsSkillRepositoryParameter { get; set; }
    public IPbsExperienceRepository IPbsExperienceRepository { get; set; }
    public PbsExperienceRepositoryParameter PbsExperienceRepositoryParameter { get; set; }
    public PbsSkillExperience PbsSkillExperience { get; set; }
    public List<string> IdList { get; set; }
    public string Id { get; set; }
    public string PbsId { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
}