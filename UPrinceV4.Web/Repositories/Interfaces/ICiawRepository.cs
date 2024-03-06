using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CIAW;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ICiawRepository
{
    Task<CiawCreateReturnDto> Create(CiawParameter CiawParameter);
    Task<IEnumerable<CiawHeaderFilterDto>> FilterCiaw(CiawParameter CiawParameter);
    Task<CiawDropDownData> CiawDropDownData(CiawParameter CiawParameter);
    Task<CiawDropDownData> CiawCancelDropDownData(CiawParameter CiawParameter);

    Task<CiawGetByIdDto> CiawGetById(CiawParameter CiawParameter);
    Task<string> ProjectCiawSiteCreate(CiawParameter CiawParameter);
    Task<List<object>> CiawSendRequest(CiawParameter CiawParameter);

    Task<List<object>> CiawSendSingleRequest(CiawParameter CiawParameter, List<CiawRequestData> ciawList);
    Task<object> CiawCancelPresences(CiawParameter CiawParameter);
    Task<ProjectCiawSite> ProjectCiawSiteGet(CiawParameter CiawParameter);
    Task<IEnumerable<NationalityDto>> FilterNationality(CiawParameter CiawParameter);
    Task<string> SendCiawWarningEmail(CiawParameter CiawParameter);
}

public class CiawParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public CiawHeader CiawHeader { get; set; }
    public DateTime Date { get; set; }
    public CiawCreateDto CiawCreateDto { get; set; }
    public CiawFilter CiawFilter { get; set; }
    public ProjectCiawSite ProjectCiawSite { get; set; }
    public NationalityFilter NationalityFilter { get; set; }
    public CiawSendRequest CiawSendRequest { get; set; }
    public CiawCancleRequest CiawCancleRequest { get; set; }
    public IConfiguration Configuration { get; set; }
}