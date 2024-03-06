using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers.BMLot;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BMLotController : CommonConfigurationController
{
    private readonly AbsenceParameter _absenceParameter;
    private readonly IAbsenceRepository _iAbsenceRepository;
    private readonly ITenantProvider _tenantProvider;


    public BMLotController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApplicationDbContext dbContext,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, PmolParameter PmolParameter,
        ILogger<BMLotController> logger, ITenantProvider iTenantProvider
        , IAbsenceRepository iAbsenceRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iAbsenceRepository = iAbsenceRepository;
        _absenceParameter = new AbsenceParameter();
        _tenantProvider = tenantProvider;
    }
}