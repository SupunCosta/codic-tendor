using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.CPC;

namespace UPrinceV4.Web.Controllers.CPC;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class CpcNicknameController : CommonConfigurationController
{
    private readonly ICpcNicknameRepository _ICpcNicknameRepository;

    public CpcNicknameController(ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor,
        ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse
        , ICpcNicknameRepository iCpcNicknameRepository,
        ITenantProvider iTenantProvider)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _ICpcNicknameRepository = iCpcNicknameRepository;
    }

    [HttpPut("Update")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateNickname(
        [FromBody] CpcResourceNicknameCreateDto cpcDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _CpcNicknameParameters = new CpcNicknameParameters
            {
                CpcNicknameDto = cpcDto,
                Context = UPrinceCustomerContext
            };

            return Ok(new ApiOkResponse(await _ICpcNicknameRepository.UpdateNickname(_CpcNicknameParameters)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}