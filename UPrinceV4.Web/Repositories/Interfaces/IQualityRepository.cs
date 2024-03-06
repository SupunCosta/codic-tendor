using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;

namespace UPrinceV4.Web.Repositories;

public interface IQualityRepository
{
    Task<IEnumerable<Quality>> GetQualityList(QualityRepositoryParameter qualityRepositoryParameter);
    Task<Quality> GetQualityById(QualityRepositoryParameter qualityRepositoryParameter);
    Task<Quality> AddQuality(QualityRepositoryParameter qualityRepositoryParameter);
    bool DeleteQuality(QualityRepositoryParameter qualityRepositoryParameter);
    Task<IEnumerable<Quality>> Filter(QualityRepositoryParameter qualityRepositoryParameter);
}

public class QualityRepositoryParameter
{
    public ApplicationDbContext ApplicationDbContext { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public List<string> IdList { get; set; } // should pass sequence code list as id
    public Quality Quality { get; set; }
    public string QualityId { get; set; } // should pass sequence code as id
    public string Lang { get; set; }
    public QualityFilterModel QualityFilterModel { get; set; }
    public ApplicationUser ChangedUser { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
}