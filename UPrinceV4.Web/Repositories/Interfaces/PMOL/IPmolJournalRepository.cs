using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.PMOL;

namespace UPrinceV4.Web.Repositories.Interfaces.PMOL;

public interface IPmolJournalRepository
{
    Task<string> CreateJournal(PmolJournalParameter PmolParameter);
    Task<PmolJournalCreateDto> ReadJournal(PmolJournalParameter PmolParameter);
    Task<string> ReadJournalDoneWork(PmolJournalParameter pmolParameter);
    Task<string> ReadJournalEncounteredProblem(PmolJournalParameter pmolParameter);
    Task<string> ReadJournalLessonsLearned(PmolJournalParameter pmolParameter);
    Task<string> ReadJournalReportedThings(PmolJournalParameter pmolParameter);
    Task<string> UploadJournalPictureForMobile(PmolJournalParameter pmolParameter);
}

public class PmolJournalParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string Id { get; set; }
    public PmolJournalCreateDto PmolDto { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public IFormCollection formData { get; set; }
}