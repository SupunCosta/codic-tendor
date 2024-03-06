using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;

namespace UPrinceV4.Web.Controllers.PMOL;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PmolResourceController : CommonConfigurationController
{
    private readonly ICoporateProductCatalogRepository _ICoporateProductCatalogRepository;
    private readonly IPmolResourceRepository _iPmolResourceRepository;
    private readonly IVPRepository _iVPRepository;


    public PmolResourceController( IPmolResourceRepository iPmolResourceRepository,
        ICoporateProductCatalogRepository iCoporateProductCatalogRepository,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IVPRepository iVPRepository,IConfiguration iConfiguration
    )
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iPmolResourceRepository = iPmolResourceRepository;
        _ICoporateProductCatalogRepository = iCoporateProductCatalogRepository;
        _iVPRepository = iVPRepository;
        _iConfiguration = iConfiguration;

    }

    private IConfiguration _iConfiguration { get; }


    [HttpPost("CreateConsumable")]
    public async Task<ActionResult> CreateResource([FromBody] PmolResourceCreateDto dto)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("CreateConsumable" + JsonToStringConverter.getStringFromJson(dto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.CreateConsumable(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateLabour")]
    public async Task<ActionResult> CreateLabour([FromBody] PmolResourceCreateDto dto)
    {
        try
        {
            // _logger.LogTrace("Started");
            // _logger.LogError("CreateLabour" + JsonToStringConverter.getStringFromJson(dto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository,
                VpRepository = _iVPRepository,
                Configuration = _iConfiguration
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.CreateLabour(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateLabourForMobileApp")]
    public async Task<ActionResult> CreateLabourForMobileApp(
        [FromBody] PmolResourceCreateMobileDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateMobileDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(
                new ApiOkResponse(await _iPmolResourceRepository.CreateLabourForMobileApp(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateLabourTeamForMobileApp")]
    public async Task<ActionResult> CreateLabourTeamForMobileApp(
        [FromBody] PmolTeamRoleCreateDto pmolTeamCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                pmolTeamCreateDto = pmolTeamCreateDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(
                await _iPmolResourceRepository.CreateLabourTeamForMobileApp(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateMaterial")]
    public async Task<ActionResult> CreateMaterial([FromBody] PmolResourceCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.CreateMaterial(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateTools")]
    public async Task<ActionResult> CreateTools([FromBody] PmolResourceCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.CreateTools(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadConsumable/{Id}")]
    public async Task<ActionResult> ReadConsumable(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadConsumable(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadLabour/{Id}")]
    public async Task<ActionResult> ReadLabour(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadLabour(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadMaterial/{Id}")]
    public async Task<ActionResult> ReadMaterial(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadMaterial(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadTools/{Id}")]
    public async Task<ActionResult> ReadTools(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadTools(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadPlannedResources/{Id}")]
    public async Task<ActionResult> ReadPlannedResources(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            var mPmolResourceReadAllDto = new PmolResourceReadAllDto();
            mPmolResourceReadAllDto.Labour = await _iPmolResourceRepository.ReadLabour(_pmolParameter);
            mPmolResourceReadAllDto.Consumable = await _iPmolResourceRepository.ReadConsumable(_pmolParameter);
            mPmolResourceReadAllDto.Material = await _iPmolResourceRepository.ReadMaterial(_pmolParameter);
            mPmolResourceReadAllDto.Tools = await _iPmolResourceRepository.ReadTools(_pmolParameter);

            return Ok(new ApiOkResponse(mPmolResourceReadAllDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("ReadExtraTools/{Id}")]
    public async Task<ActionResult> ReadExtraTools(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadExtraTools(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadExtraLabour/{Id}")]
    public async Task<ActionResult> ReadExtraLabour(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadExtraLabour(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadExtraMaterial/{Id}")]
    public async Task<ActionResult> ReadExtraMaterial(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadExtraMaterial(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadExtraConsumable/{Id}")]
    public async Task<ActionResult> ReadExtraConsumable(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadExtraConsumable(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadExtraResources/{Id}")]
    public async Task<ActionResult> ReadExtraResources(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            var mPmolResourceReadAllDto = new PmolResourceReadAllDto();
            mPmolResourceReadAllDto.Labour = await _iPmolResourceRepository.ReadExtraLabour(_pmolParameter);
            mPmolResourceReadAllDto.Consumable = await _iPmolResourceRepository.ReadExtraConsumable(_pmolParameter);
            mPmolResourceReadAllDto.Material = await _iPmolResourceRepository.ReadExtraMaterial(_pmolParameter);
            mPmolResourceReadAllDto.Tools = await _iPmolResourceRepository.ReadExtraTools(_pmolParameter);

            return Ok(new ApiOkResponse(mPmolResourceReadAllDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpDelete("DeletePmolConsumable")]
    public async Task<IActionResult> DeletePmolConsumable([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                IdList = idList,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.DeleteConsumable(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePmolLabour")]
    public async Task<IActionResult> DeletePmolLabour([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                IdList = idList,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.DeleteLabour(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePmolMaterial")]
    public async Task<IActionResult> DeletePmolMaterial([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                IdList = idList,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.DeleteMaterial(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeletePmolTools")]
    public async Task<IActionResult> DeletePmolTools([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                IdList = idList,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.DeleteTools(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePmolTeamRole")]
    public async Task<ActionResult> CreatePmolTeamRole(
        [FromBody] PmolTeamRoleCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                pmolTeamCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.CreateTeamRole(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadPmolPlannedTeam/{LabourId}")]
    public async Task<ActionResult> ReadPmolPlannedTeam(string LabourId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = LabourId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadPlannedTeamMember(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadPmolExtraTeam/{LabourId}")]
    public async Task<ActionResult> ReadPmolExtraTeam(string LabourId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = LabourId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.ReadExtraTeamMember(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteTeamMember")]
    public async Task<IActionResult> DeleteTeamMember([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                IdList = idList,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.DeleteTeamMember(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateMaterialForMobile")]
    public async Task<ActionResult> CreateMaterialForMobile(
        [FromBody] PmolResourceCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(
                await _iPmolResourceRepository.CreateMaterialForMobile(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateLabourForMobile")]
    public async Task<ActionResult> CreateLabourForMobile(
        [FromBody] PmolResourceCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.CreateLabourForMobile(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateConsumableForMobile")]
    public async Task<ActionResult> CreateConsumableForMobile(
        [FromBody] PmolResourceCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(
                await _iPmolResourceRepository.CreateConsumableForMobile(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreateToolsForMobile")]
    public async Task<ActionResult> CreateToolsForMobile(
        [FromBody] PmolResourceCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ResourceCreateDto = dto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(await _iPmolResourceRepository.CreateToolsForMobile(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadPmolExtraTeamForMobile/{PmolId}")]
    public async Task<ActionResult> ReadPmolExtraTeamForMobile(string PmolId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = PmolId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(
                new ApiOkResponse(await _iPmolResourceRepository.ReadExtraTeamMemberForMobile(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadPmolPLannedTeamForMobile/{PmolId}")]
    public async Task<ActionResult> ReadPmolPLannedTeamForMobile(string PmolId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = PmolId,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(
                await _iPmolResourceRepository.ReadPlannedTeamMemberForMobile(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UpdateLabourTeamForMobileApp")]
    public async Task<ActionResult> UpdateLabourTeamForMobileApp(
        [FromBody] PmolTeamRoleCreateDto pmolTeamCreateDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                pmolTeamCreateDto = pmolTeamCreateDto,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ICoporateProductCatalogRepository = _ICoporateProductCatalogRepository
            };

            return Ok(new ApiOkResponse(
                await _iPmolResourceRepository.UpdateLabourTeamForMobileApp(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadWebPlannedResources/{Id}")]
    public async Task<ActionResult> ReadWebPlannedResources(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                isWeb = true,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            var mPmolResourceReadAllDto = new PmolResourceReadAllDto();
            mPmolResourceReadAllDto.Labour = await _iPmolResourceRepository.ReadLabour(_pmolParameter);
            mPmolResourceReadAllDto.Consumable = await _iPmolResourceRepository.ReadConsumable(_pmolParameter);
            mPmolResourceReadAllDto.Material = await _iPmolResourceRepository.ReadMaterial(_pmolParameter);
            mPmolResourceReadAllDto.Tools = await _iPmolResourceRepository.ReadTools(_pmolParameter);

            return Ok(new ApiOkResponse(mPmolResourceReadAllDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadWebExtraResources/{Id}")]
    public async Task<ActionResult> ReadWebExtraResources(string Id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = Id,
                isWeb = true,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            var mPmolResourceReadAllDto = new PmolResourceReadAllDto();
            mPmolResourceReadAllDto.Labour = await _iPmolResourceRepository.ReadExtraLabour(_pmolParameter);
            mPmolResourceReadAllDto.Consumable = await _iPmolResourceRepository.ReadExtraConsumable(_pmolParameter);
            mPmolResourceReadAllDto.Material = await _iPmolResourceRepository.ReadExtraMaterial(_pmolParameter);
            mPmolResourceReadAllDto.Tools = await _iPmolResourceRepository.ReadExtraTools(_pmolParameter);

            return Ok(new ApiOkResponse(mPmolResourceReadAllDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}