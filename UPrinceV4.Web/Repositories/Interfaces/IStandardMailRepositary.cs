using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.StandardMails;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IStandardMailRepositary
{
    Task<IEnumerable<StandardMailHeaderDto>> StandardMailFilter(StandardMailParameters StandardMailParameters);
    Task<string> StandardMailCreate(StandardMailParameters StandardMailParameters);
    Task<StandardMailHeaderDto> StandardMailGetById(StandardMailParameters StandardMailParameters);
}

public class StandardMailParameters
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public StandardMailHeaderDto StandardMailDto { get; set; }
    public string UserId { get; set; }
    public StandardMailFilter Filter { get; set; }
}