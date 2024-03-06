using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.VisualPlan;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IResourceMatrixRepository
{
    Task<ResourceMatrix> GetResourceMatrixFromPmol(ResourceMatrixParameter resourceMatrixParameter);
    Task<ResourceMatrix> GetResourceMatrixFromPmolMonth(ResourceMatrixParameter resourceMatrixParameter);
    Task<ResourceMatrix> GetResourceMatrixFromPmolQuarter(ResourceMatrixParameter resourceMatrixParameter);
    Task<ResourceMatrix> GetResourceMatrixFromPbs(ResourceMatrixParameter resourceMatrixParameter);
    Task<ResourceMatrix> GetResourceMatrixFromPbsMonth(ResourceMatrixParameter resourceMatrixParameter);
    Task<ResourceMatrix> GetResourceMatrixFromPbsQuarter(ResourceMatrixParameter resourceMatrixParameter);
    Task<LabourMatrix> GetLabourMatrixByDate(ResourceMatrixParameter resourceMatrixParameter);
    Task<LabourMatrix> GetLabourMatrixByWeek(ResourceMatrixParameter resourceMatrixParameter);
    Task<LabourMatrix> GetLabourMatrixByMonth(ResourceMatrixParameter resourceMatrixParameter);
    Task<OrganizationMatrix> GetOrganizationMatrixDay(ResourceMatrixParameter resourceMatrixParameter);
    Task<OrganizationMatrix> GetOrganizationMatrixWeek(ResourceMatrixParameter resourceMatrixParameter);
    Task<OrganizationMatrix> GetOrganizationMatrixMonth(ResourceMatrixParameter resourceMatrixParameter);
}

public class ResourceMatrixParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public GetResourceMatrixDto getResourceMatrixDto { get; set; }
    public PbsDate PbsDate { get; set; }
    public bool IsMyEnv { get; set; }
    public bool IsCu { get; set; }

}