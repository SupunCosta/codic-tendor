using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.Mecops;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IMecopRepository
{
    Task<string> GetMecopData(MecopParameter mecopParameter);
    Task<List<MecopDto>> GetMecopForExel(MecopParameter mecopParameter);
    Task<List<MecopDto>> GetMecopForExelNew(MecopParameter mecopParameter);
    Task<List<MecopMetaData>> GetMecopMetaDataForExel(MecopParameter mecopParameter);
    Task<List<string>> MecopStatusUpdate(MecopParameter mecopParameter);

}

public class MecopParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public MecopStatusUpdateDto MecopStatusUpdateDto { get; set; }
}