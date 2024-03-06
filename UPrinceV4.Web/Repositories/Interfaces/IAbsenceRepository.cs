using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.VisualPlan;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IAbsenceRepository
{
    Task<string> Create(AbsenceParameter AbsenceParameter);

    Task<IEnumerable<AbsenceHeaderGetDto>> GetAbsenceListByPersonId(AbsenceParameter AbsenceParameter);


    Task<string> DeleteAbsence(AbsenceParameter AbsenceParameter);

    Task<AbsenceHeaderGetDto> AbsenceGetById(AbsenceParameter AbsenceParameter);
    Task<IEnumerable<AbsenceLeaveTypeDto>> GetAbsenceLeaveType(AbsenceParameter AbsenceParameter);
}

public class AbsenceParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public AbsenceHeaderCreateDto AbsenceDto { get; set; }


    public List<string> IdList { get; set; }
}