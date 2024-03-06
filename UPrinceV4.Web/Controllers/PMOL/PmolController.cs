using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Repositories.Interfaces.PS;

namespace UPrinceV4.Web.Controllers.PMOL;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PmolController : CommonConfigurationController
{
    private readonly IBorRepository _borRepository;
    private readonly CompetenciesRepositoryParameter _competenciesRepositoryParameter;


    private readonly IBorRepository _iBorRepository;

    private readonly ICompetenciesRepository _iCompetenciesRepository;
    private readonly IPbsInstructionFamilyRepository _iPbsInstructionFamilyRepository;
    private readonly IPbsInstructionsRepository _iPbsInstructionsRepository;

    private readonly IPbsQualityRepository _IPbsQualityRepository;
    private readonly IPbsRepository _IPbsRepository;

    private readonly IPbsRiskRepository _IPbsRiskRepository;
    private readonly IPersonRepository _ipersonRepository;

    private readonly IPmolJournalRepository _IPmolJournalRepository;
    private readonly IPmolRepository _iPmolRepository;
    private readonly IPmolResourceRepository _iPmolResourceRepository;
    private readonly IPsRepository _iPsRepository;
    private readonly IRiskRepository _iRiskRepository;


    private readonly IRiskStatusRepository _iRiskStatusRepository;
    private readonly IRiskTypeRepository _iRiskTypeRepository;

    private readonly IShiftRepository _iShiftRepository;
    private readonly PbsInstructionFamilyRepositoryParameter _pbsInstructionFamilyRepositoryParameter;
    private readonly PbsInstructionsRepositoryParameter _pbsInstructionsRepositoryParameter;

    private readonly ITenantProvider _TenantProvider;
    private readonly IVPRepository _iVpRepository;


    public PmolController(ITenantProvider tenantProvider, IPmolRepository iPmolRepository,
        IBorRepository borRepository, ICompetenciesRepository iCompetenciesRepository, IPsRepository iPsRepository,
        CompetenciesRepositoryParameter competenciesRepositoryParameter,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        IPmolResourceRepository iPmolResourceRepository,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        ITenantProvider iTenantProvider
        , IPbsInstructionsRepository iPbsInstructionsRepository,
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter,
        IPbsInstructionFamilyRepository iPbsInstructionFamilyRepository,
        PbsInstructionFamilyRepositoryParameter pbsInstructionFamilyRepositoryParameter,
        IBorRepository iBorRepository, IPbsRepository iPbsRepository, IRiskStatusRepository iRiskStatusRepository,
        IRiskTypeRepository iRiskTypeRepository,
        IRiskRepository iRiskRepository, IPersonRepository ipersonRepository,
        IPbsQualityRepository iPbsQualityRepository,
        IPbsRiskRepository iPbsRiskRepository, IPmolJournalRepository IPmolJournalRepository,
        IConfiguration iConfiguration, IShiftRepository iShiftRepository, IVPRepository iVpRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iPmolRepository = iPmolRepository;
        _TenantProvider = tenantProvider;

        _borRepository = borRepository;
        _iPmolResourceRepository = iPmolResourceRepository;

        _iPbsInstructionsRepository = iPbsInstructionsRepository;
        _pbsInstructionsRepositoryParameter = pbsInstructionsRepositoryParameter;
        _iPbsInstructionFamilyRepository = iPbsInstructionFamilyRepository;
        _pbsInstructionFamilyRepositoryParameter = pbsInstructionFamilyRepositoryParameter;
        _iBorRepository = iBorRepository;
        _IPbsRepository = iPbsRepository;
        _iRiskStatusRepository = iRiskStatusRepository;
        _iRiskTypeRepository = iRiskTypeRepository;
        _iRiskRepository = iRiskRepository;
        _ipersonRepository = ipersonRepository;
        _IPbsQualityRepository = iPbsQualityRepository;
        _IPbsRiskRepository = iPbsRiskRepository;
        _iCompetenciesRepository = iCompetenciesRepository;
        _competenciesRepositoryParameter = competenciesRepositoryParameter;
        _IPmolJournalRepository = IPmolJournalRepository;
        _iPsRepository = iPsRepository;
        _iConfiguration = iConfiguration;
        _iShiftRepository = iShiftRepository;
        _iVpRepository = iVpRepository;
    }

    private IConfiguration _iConfiguration { get; }

    [HttpGet("ReadPmolShortcutPaneData")]
    public async Task<ActionResult> ReadPmolShortcutPaneData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            return Ok(new ApiOkResponse(await _iPmolRepository.GetShortcutpaneData(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadPmolData")]
    public async Task<ActionResult> ReadPmolData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            var pmolDataDto = new PmolDataDto();
            pmolDataDto.PmolDropdown = await _iPmolRepository.GetDropdownData(_pmolParameter);

            _pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            _pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _pbsInstructionsRepositoryParameter.Lang = lang;

            _pbsInstructionFamilyRepositoryParameter.TenantProvider = ItenantProvider;
            _pbsInstructionFamilyRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _pbsInstructionFamilyRepositoryParameter.Lang = lang;
            _pbsInstructionsRepositoryParameter.IPbsInstructionFamilyRepository = _iPbsInstructionFamilyRepository;
            _pbsInstructionsRepositoryParameter.PbsInstructionFamilyRepositoryParameter =
                _pbsInstructionFamilyRepositoryParameter;

            pmolDataDto.PbsInstructionsDropdown =
                await _iPbsInstructionsRepository.GetPbsInstructionDropdownData(_pbsInstructionsRepositoryParameter);
            var _borParameter = new BorParameter();
            _borParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _borParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _borParameter.Lang = lang;
            _borParameter.TenantProvider = ItenantProvider;
            pmolDataDto.BorDropdown = await _iBorRepository.GetBorDropdownData(_borParameter);
            var _PbsParameters = new PbsParameters();
            _PbsParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _PbsParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _PbsParameters.Lang = lang;
            _PbsParameters.TenantProvider = ItenantProvider;

            pmolDataDto.PbsTreeStructureDto = await _IPbsRepository.GetTreeStructureData(_PbsParameters);


            var query = @"select RoleId AS Id, RoleName from Role where LanguageCode=@lang ";

            var parameters = new { lang };

            using (var connection = new SqlConnection(ItenantProvider.GetTenant().ConnectionString))
            {
                pmolDataDto.Roles = connection.Query<Roles>(query, parameters);
            }

            var _riskRepositoryParameter = new RiskRepositoryParameter
            {
                TenantProvider = ItenantProvider,
                ApplicationDbContext = UPrinceCustomerContext,
                Lang = lang
            };
            var _riskStatusRepositoryParameter = new RiskStatusRepositoryParameter
            {
                TenantProvider = ItenantProvider,
                ApplicationDbContext = UPrinceCustomerContext,
                Lang = lang
            };
            _riskRepositoryParameter.RiskStatusRepositoryParameter = _riskStatusRepositoryParameter;
            var _riskTypeRepositoryParameter = new RiskTypeRepositoryParameter
            {
                TenantProvider = ItenantProvider,
                ApplicationDbContext = UPrinceCustomerContext,
                Lang = lang
            };
            _riskRepositoryParameter.RiskTypeRepositoryParameter = _riskTypeRepositoryParameter;
            var _personRepositoryParameter = new PersonRepositoryParameter();
            _personRepositoryParameter.TenantProvider = ItenantProvider;
            _personRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _riskRepositoryParameter.PersonRepositoryParameter = _personRepositoryParameter;

            _riskRepositoryParameter.IPersonRepository = _ipersonRepository;
            _riskRepositoryParameter.IRiskStatusRepository = _iRiskStatusRepository;
            _riskRepositoryParameter.IRiskTypeRepository = _iRiskTypeRepository;

            pmolDataDto.RiskDropdown = await _iRiskRepository.GetRiskDropdownData(_riskRepositoryParameter);


            return Ok(new ApiOkResponse(pmolDataDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("FilterPmol")]
    public async Task<ActionResult> GetPmolList([FromBody] PmolFilter filter)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.filter = filter;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            var result = await _iPmolRepository.GetPmolList(_pmolParameter);


            if (!result.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noPmolAvailable")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            var mApiOkResponse = new ApiOkResponse(result);

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetPmolDropdownData")]
    public async Task<ActionResult> GetPmolDropdownData()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.TenantProvider = ItenantProvider;
            var dropDownData = await _iPmolRepository.GetDropdownData(_pmolParameter);

            return Ok(new ApiOkResponse(dropDownData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadPmolHeaderById/{id}")]
    public async Task<ActionResult> GetPmolHeaderById(string id, CancellationToken cancellationToken)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                borRepository = _borRepository,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            return Ok(new ApiOkResponse(await _iPmolRepository.GetPmolById(_pmolParameter)));
        }

        catch (OperationCanceledException e)
        {
            return BadRequest(new ApiResponse(400, false, e.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreateHeader")]
    public async Task<ActionResult> CreatePmolHeader([FromBody] PmolCreateDto pmolDto)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("CreateHeader" + JsonToStringConverter.getStringFromJson(pmolDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.PmolDto = pmolDto;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
            _pmolParameter.Configuration = _iConfiguration;
            _pmolParameter.IVpRepository = _iVpRepository;
            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var pmol = await _iPmolRepository.CreateHeader(_pmolParameter, false);
            return Ok(new ApiOkResponse(pmol));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpPost("StopHandShake")]
    public async Task<ActionResult> StopHandShake([FromBody] PmolStopHandshakeCreateDto pmolStopHandshakeCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.pmolStopHandshakeCreateDto = pmolStopHandshakeCreateDto;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var id = await _iPmolRepository.CreatePmolStopHandshake(_pmolParameter);

            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpPost("AddStopHandShakeDocuments")]
    public async Task<ActionResult> AddStopHandShakeDocuments(
        [FromBody] PmolStopHandshakeCreateDocumentsDto pmolStopHandshakeCreateDocumentsDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.pmolStopHandshakeCreateDocumentsDto = pmolStopHandshakeCreateDocumentsDto;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var id = await _iPmolRepository.AddPmolStopHandshakeDocuments(_pmolParameter);

            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadStopHandShakesByPmolId")]
    public async Task<ActionResult> ReadStopHandShakesByPmolId([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string project,
        [FromHeader(Name = "lang")] string langX, string id)
    {
        try
        {
            var lang = langCode(langX);

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = CU;
            _pmolParameter.ProjectSequenceId = project;
            _pmolParameter.Lang = lang;

            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.Id = id;
            return Ok(new ApiOkResponse(await _iPmolRepository.GetStopHandShakesByPmolId(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("StartHandShake")]
    public async Task<ActionResult> StartHandShake([FromBody] PmolStartHandshakeCreateDto pmolStartHandshakeCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.pmolStartHandshakeCreateDto = pmolStartHandshakeCreateDto;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iPmolRepository.CreatePmolStartHandshake(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreateLocation")]
    public async Task<ActionResult> CreateLocation([FromBody] MapLocation Location)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Location = Location;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iPmolRepository.CreateLocation(_pmolParameter, false)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UploadStopHandShakeFile")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadStopHandShakeFile([FromForm] IFormCollection image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(image.Files.FirstOrDefault()?.FileName, _TenantProvider,
                image.Files.FirstOrDefault(), "StopHandShakeFiles");

            return Ok(new ApiOkResponse(url));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpPost("UploadExtraworkFile")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadExtraworkFile([FromForm] IFormCollection image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(image.Files.FirstOrDefault()?.FileName, _TenantProvider,
                image.Files.FirstOrDefault(), "UploadExtraworkFiles");
            return Ok(new ApiOkResponse(url));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UploadAudioFile")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadAudioFile([FromForm] IFormCollection audio)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var client = new FileClient();
            var url = client.PersistPhotoInNewFolder(audio.Files.FirstOrDefault()?.FileName, _TenantProvider,
                audio.Files.FirstOrDefault(), "UploadExtraworkFiles");
            // var message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;

            return Ok(new ApiOkResponse(url));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadLocationByPmolId/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadLocationByPmolId(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                Id = id,
                TenantProvider = ItenantProvider
            };

            var projectDefinition = await _iPmolRepository.GetLocationByPmolId(_pmolParameter);
            return Ok(projectDefinition == null
                ? new ApiResponse(400, false, "noLocationForTheID")
                : new ApiOkResponse(projectDefinition));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("ExtraWorkCreate")]
    public async Task<ActionResult> ExtraWorkCreate([FromBody] PmolExtraWorkCreateDto pmolExtraWorkDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.PmolExtraWorkCreateDto = pmolExtraWorkDto;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iPmolRepository.CreatePmolExtraWork(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteExtraWork")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteExtraWork([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.IdList = idList;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var state = await _iPmolRepository.DeletePmolExtraWork(_pmolParameter);

            return Ok(new ApiOkResponse(state));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteExtraWorkFiles")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteExtraWorkFiles([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.IdList = idList;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var state = await _iPmolRepository.DeletePmolExtraWorkFiles(_pmolParameter);

            return Ok(new ApiOkResponse(state));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadExtraWorkByPmolId/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadExtraWorkByPmolId(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var projectDefinition = await _iPmolRepository.GetExtraWorkByPmolId(_pmolParameter);
            return Ok(new ApiOkResponse(projectDefinition));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("ReadAudioByPmolId/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadAudioByPmolId(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var projectDefinition = await _iPmolRepository.ReadAudioByPmolId(_pmolParameter);

            return Ok(projectDefinition == null
                ? new ApiResponse(400, false, "noLocationForTheId")
                : new ApiOkResponse(projectDefinition));
            return null;
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadPmolByUserId")]
    public async Task<ActionResult> ReadPmolByUserId()
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            var shortcutPaneData = await _iPmolRepository.GetPmolByUserId(_pmolParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("PmolClone/{id}")]
    public async Task<ActionResult> ClonePmol(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();
            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.borRepository = _borRepository;
            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
            var pmol = await _iPmolRepository.ClonePmol(_pmolParameter);
            var mApiOkResponse = new ApiOkResponse(pmol);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("ClonePmolDayPlanning")]
    public async Task<ActionResult> ClonePmolDayPlanning([FromBody] DayPlanPmolClone DayPlanPmolClone)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = DayPlanPmolClone.PmolId;
            _pmolParameter.ContractingUnitSequenceId = DayPlanPmolClone.ContractingUinit;
            _pmolParameter.ProjectSequenceId = DayPlanPmolClone.ProjectSequenceCode;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.borRepository = _borRepository;
            _pmolParameter.Configuration = _iConfiguration;
            _pmolParameter.IShiftRepository = _iShiftRepository;
            _pmolParameter.DayPlanPmolClone = DayPlanPmolClone;
            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
            _pmolParameter.IVpRepository = _iVpRepository;
            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var pmol = await _iPmolRepository.ClonePmolDayPlanning(_pmolParameter);
            var mApiOkResponse = new ApiOkResponse(pmol);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPut("UpdateStartTime/{id}")]
    public async Task<ActionResult> UpdateStartTime(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var pmol = await _iPmolRepository.UpdateStartTime(_pmolParameter);
            return Ok(new ApiOkResponse(pmol));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPut("UpdateEndTime/{id}")]
    public async Task<ActionResult> UpdateEndTime(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var pmol = await _iPmolRepository.UpdateEndTime(_pmolParameter);
            return Ok(new ApiOkResponse(pmol));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPut("UpdateFinishedStatus/{id}")]
    public async Task<ActionResult> UpdateFinishedStatus(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var pmol = await _iPmolRepository.UpdateFinishedStatus(_pmolParameter);
            return Ok(new ApiOkResponse(pmol));
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return BadRequest(ApiResponse);
        }
    }

    [HttpPost("UploadImageForMobile")]
    public async Task<ActionResult<string>> UploadImageForMobile([FromForm] IFormCollection image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = Request.Headers["lang"].FirstOrDefault();
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.formData = image;
            var id = await _iPmolRepository.UploadImageForMobile(_pmolParameter);
            return Ok(new ApiOkResponse(id, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UploadAudioForMobile")]
    public async Task<ActionResult<string>> UploadAudioForMobile([FromForm] IFormCollection audio)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.formData = audio;
            var id = await _iPmolRepository.UploadAudioForMobile(_pmolParameter);
            return Ok(new ApiOkResponse(id, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPut("ApprovePmol/{pmolId}")]
    public async Task<ActionResult> ApprovePmol(string pmolId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var parameter = new PmolParameter();
            parameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            parameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            parameter.Lang = lang;
            parameter.Id = pmolId;
            parameter.ContextAccessor = ContextAccessor;
            parameter.TenantProvider = ItenantProvider;
            parameter.ApplicationDbContext = UPrinceCustomerContext;
            var result = await _iPmolRepository.ApprovePmol(parameter);


            // if (result == "False")
            // {
            //     var mApiResponse = new ApiOkResponse(null, "oneOrMoreTeamMembersHaveNoEndTime")
            //     {
            //         Status = false
            //     };
            //     return Ok(mApiResponse);
            // }

            var mApiOkResponse = new ApiOkResponse(result);

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UpdateCommentMobile")]
    public async Task<ActionResult> UpdateCommentMobile([FromBody] PmolCreateCommentDto PmolCreateCommentDto,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("CreateHeader" + JsonToStringConverter.getStringFromJson(pmolDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(langX);

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = CU;
            _pmolParameter.ProjectSequenceId = Project;
            _pmolParameter.Lang = lang;
            _pmolParameter.PmolCreateCommentDto = PmolCreateCommentDto;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            return Ok(new ApiOkResponse(await _iPmolRepository.UpdateCommentMobile(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadPmolIdNew/{id}")]
    public async Task<ActionResult> ReadPmolIdNew(string id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = CU;
            _pmolParameter.ProjectSequenceId = Project;
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.borRepository = _borRepository;

            var mPmolDto = new PmolDto
            {
                Header = await _iPmolRepository.GetPmolById(_pmolParameter)
            };

            var pbsInstructionsRepositoryParameter = new PbsInstructionsRepositoryParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                TenantProvider = ItenantProvider,
                ApplicationDbContext = UPrinceCustomerContext,
                PbsProductId = mPmolDto.Header.ProductId
            };

            mPmolDto.Instruction =
                await _iPbsInstructionsRepository.GetAllPbsInstructionsByPbsProductAllTypes(
                    pbsInstructionsRepositoryParameter);

            var mPmolResourceParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Id = mPmolDto.Header.Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            var mxPmolResourceParameter = new PmolResourceParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                isWeb = true,
                Id = mPmolDto.Header.Id,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            var mPmolResourceReadAllDto = new PmolResourceReadAllDto();
            mPmolResourceReadAllDto.Labour = await _iPmolResourceRepository.ReadLabour(mxPmolResourceParameter);
            mPmolResourceReadAllDto.Consumable = await _iPmolResourceRepository.ReadConsumable(mxPmolResourceParameter);
            mPmolResourceReadAllDto.Material = await _iPmolResourceRepository.ReadMaterial(mxPmolResourceParameter);
            mPmolResourceReadAllDto.Tools = await _iPmolResourceRepository.ReadTools(mxPmolResourceParameter);

            mPmolDto.PlannedResource = mPmolResourceReadAllDto;

            var mPmolResourceReadAllDtoExtra = new PmolResourceReadAllDto();
            mPmolResourceReadAllDtoExtra.Labour =
                await _iPmolResourceRepository.ReadExtraLabour(mxPmolResourceParameter);
            mPmolResourceReadAllDtoExtra.Consumable =
                await _iPmolResourceRepository.ReadExtraConsumable(mxPmolResourceParameter);
            mPmolResourceReadAllDtoExtra.Material =
                await _iPmolResourceRepository.ReadExtraMaterial(mxPmolResourceParameter);
            mPmolResourceReadAllDtoExtra.Tools = await _iPmolResourceRepository.ReadExtraTools(mxPmolResourceParameter);

            mPmolDto.ExtraResource = mPmolResourceReadAllDtoExtra;

            var _PbsQualityParameters = new PbsQualityParameters();
            _PbsQualityParameters.ContractingUnitSequenceId = CU;
            _PbsQualityParameters.ProjectSequenceId = Project;
            _PbsQualityParameters.TenantProvider = ItenantProvider;
            _PbsQualityParameters.PbsProductId = mPmolDto.Header.ProductId;
            _PbsQualityParameters.Lang = lang;

            mPmolDto.Quality = await _IPbsQualityRepository.GetAllPbsQualityByPbsProductId(_PbsQualityParameters);

            var _PbsRiskParameters = new PbsRiskParameters();
            _PbsRiskParameters.ContractingUnitSequenceId = CU;
            _PbsRiskParameters.ProjectSequenceId = Project;

            _PbsRiskParameters.TenantProvider = ItenantProvider;
            _PbsRiskParameters.PbsProductId = mPmolDto.Header.ProductId;
            _PbsRiskParameters.Lang = lang;

            mPmolDto.Risk = await _IPbsRiskRepository.GetAllPbsRiskByPbsProductId(_PbsRiskParameters);


            _competenciesRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _competenciesRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;

            _competenciesRepositoryParameter.TenantProvider = ItenantProvider;
            _competenciesRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _competenciesRepositoryParameter.Lang = lang;
            _competenciesRepositoryParameter.PbsId = mPmolDto.Header.ProductId;

            mPmolDto.Competencies =
                await _iCompetenciesRepository.GetCompetenceByPbsId(_competenciesRepositoryParameter);

            mPmolDto.MapLocation = await _iPmolRepository.GetLocationByPmolId(_pmolParameter);

            var _PmolJournalParameter = new PmolJournalParameter();
            _PmolJournalParameter.ContractingUnitSequenceId = CU;
            _PmolJournalParameter.ProjectSequenceId = Project;
            _PmolJournalParameter.Lang = lang;
            _PmolJournalParameter.Id = mPmolDto.Header.Id;
            _PmolJournalParameter.ContextAccessor = ContextAccessor;
            _PmolJournalParameter.TenantProvider = ItenantProvider;

            mPmolDto.Journal = await _IPmolJournalRepository.ReadJournal(_PmolJournalParameter);
            _pmolParameter.Id = mPmolDto.Header.Id;
            mPmolDto.MapLocation = await _iPmolRepository.GetLocationByPmolId(_pmolParameter);

            mPmolDto.ExtraWork = await _iPmolRepository.GetExtraWorkByPmolId(_pmolParameter);
            mPmolDto.StopHandshake = await _iPmolRepository.GetStopHandShakesByPmolId(_pmolParameter);
            mPmolDto.PmolService = await _iPmolRepository.ReadPmolServiceByPmolId(_pmolParameter);

            return Ok(new ApiOkResponse(mPmolDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadPmolId/{id}")]
    public async Task<ActionResult> ReadPmolId(string id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = CU;
            _pmolParameter.ProjectSequenceId = Project;
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.borRepository = _borRepository;
            var mPmolDto = await _iPmolRepository.GetPmolByIdNew(_pmolParameter);

            var pbsRiskParameters = new PbsRiskParameters
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                TenantProvider = ItenantProvider,
                PbsProductId = mPmolDto.Header.ProductId,
                Lang = lang
            };

            mPmolDto.Risk = await _IPbsRiskRepository.GetAllPbsRiskByPbsProductId(pbsRiskParameters);


            return Ok(new ApiOkResponse(mPmolDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadPmolId/{id}/{langx}")]
    public async Task<ActionResult> ReadPmolIdForPDF(string id, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project, string langX)
    {
        try
        {
            var lang = langX;

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = CU;
            _pmolParameter.ProjectSequenceId = Project;
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.borRepository = _borRepository;
            var mPmolDto = new PmolDto();
            mPmolDto.Header = await _iPmolRepository.GetPmolById(_pmolParameter);

            var pbsInstructionsRepositoryParameter = new PbsInstructionsRepositoryParameter();
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId = CU;
            pbsInstructionsRepositoryParameter.ProjectSequenceId = Project;
            pbsInstructionsRepositoryParameter.Lang = lang;
            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = mPmolDto.Header.ProductId;

            mPmolDto.Instruction =
                await _iPbsInstructionsRepository.GetAllPbsInstructionsByPbsProductAllTypes(
                    pbsInstructionsRepositoryParameter);

            var mPmolResourceParameter = new PmolResourceParameter();
            mPmolResourceParameter.ContractingUnitSequenceId = CU;
            mPmolResourceParameter.ProjectSequenceId = Project;
            mPmolResourceParameter.Lang = lang;
            mPmolResourceParameter.Id = mPmolDto.Header.Id;
            mPmolResourceParameter.ContextAccessor = ContextAccessor;
            mPmolResourceParameter.TenantProvider = ItenantProvider;

            var mxPmolResourceParameter = new PmolResourceParameter();
            mxPmolResourceParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            mxPmolResourceParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            mxPmolResourceParameter.Lang = lang;
            mxPmolResourceParameter.isWeb = true;
            mxPmolResourceParameter.Id = mPmolDto.Header.Id;
            mxPmolResourceParameter.ContextAccessor = ContextAccessor;
            mxPmolResourceParameter.TenantProvider = ItenantProvider;

            var mPmolResourceReadAllDto = new PmolResourceReadAllDto();
            mPmolResourceReadAllDto.Labour = await _iPmolResourceRepository.ReadLabour(mxPmolResourceParameter);
            foreach (var labouritem in mPmolResourceReadAllDto.Labour)
            {
                var _xpmolParameter = new PmolResourceParameter();
                _xpmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
                _xpmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
                _xpmolParameter.Lang = lang;
                _xpmolParameter.Id = labouritem.Id;
                _xpmolParameter.ContextAccessor = ContextAccessor;
                _xpmolParameter.TenantProvider = ItenantProvider;

                var labourteam = await _iPmolResourceRepository.ReadPlannedTeamMember(_xpmolParameter);

                labouritem.Team = labourteam;
            }

            mPmolResourceReadAllDto.Consumable = await _iPmolResourceRepository.ReadConsumable(mxPmolResourceParameter);
            mPmolResourceReadAllDto.Material = await _iPmolResourceRepository.ReadMaterial(mxPmolResourceParameter);
            mPmolResourceReadAllDto.Tools = await _iPmolResourceRepository.ReadTools(mxPmolResourceParameter);

            mPmolDto.PlannedResource = mPmolResourceReadAllDto;

            var mPmolResourceReadAllDtoExtra = new PmolResourceReadAllDto();
            mPmolResourceReadAllDtoExtra.Labour =
                await _iPmolResourceRepository.ReadExtraLabour(mxPmolResourceParameter);
            foreach (var labouritem in mPmolResourceReadAllDtoExtra.Labour)
            {
                var _xpmolParameter = new PmolResourceParameter();
                _xpmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
                _xpmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
                _xpmolParameter.Lang = lang;
                _xpmolParameter.Id = labouritem.Id;
                _xpmolParameter.ContextAccessor = ContextAccessor;
                _xpmolParameter.TenantProvider = ItenantProvider;

                var labourteam = await _iPmolResourceRepository.ReadExtraTeamMember(_xpmolParameter);

                labouritem.Team = labourteam;
            }

            mPmolResourceReadAllDtoExtra.Consumable =
                await _iPmolResourceRepository.ReadExtraConsumable(mxPmolResourceParameter);
            mPmolResourceReadAllDtoExtra.Material =
                await _iPmolResourceRepository.ReadExtraMaterial(mxPmolResourceParameter);
            mPmolResourceReadAllDtoExtra.Tools = await _iPmolResourceRepository.ReadExtraTools(mxPmolResourceParameter);

            mPmolDto.ExtraResource = mPmolResourceReadAllDtoExtra;

            var _PbsQualityParameters = new PbsQualityParameters();
            _PbsQualityParameters.ContractingUnitSequenceId = CU;
            _PbsQualityParameters.ProjectSequenceId = Project;
            _PbsQualityParameters.TenantProvider = ItenantProvider;
            _PbsQualityParameters.PbsProductId = mPmolDto.Header.ProductId;
            _PbsQualityParameters.Lang = lang;

            mPmolDto.Quality = await _IPbsQualityRepository.GetAllPbsQualityByPbsProductId(_PbsQualityParameters);

            var _PbsRiskParameters = new PbsRiskParameters();
            _PbsRiskParameters.ContractingUnitSequenceId = CU;
            _PbsRiskParameters.ProjectSequenceId = Project;

            _PbsRiskParameters.TenantProvider = ItenantProvider;
            _PbsRiskParameters.PbsProductId = mPmolDto.Header.ProductId;
            _PbsRiskParameters.Lang = lang;

            mPmolDto.Risk = await _IPbsRiskRepository.GetAllPbsRiskByPbsProductId(_PbsRiskParameters);


            _competenciesRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _competenciesRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;

            _competenciesRepositoryParameter.TenantProvider = ItenantProvider;
            _competenciesRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _competenciesRepositoryParameter.Lang = lang;
            _competenciesRepositoryParameter.PbsId = mPmolDto.Header.ProductId;

            mPmolDto.Competencies =
                await _iCompetenciesRepository.GetCompetenceByPbsId(_competenciesRepositoryParameter);

            mPmolDto.MapLocation = await _iPmolRepository.GetLocationByPmolId(_pmolParameter);

            var _PmolJournalParameter = new PmolJournalParameter();
            _PmolJournalParameter.ContractingUnitSequenceId = CU;
            _PmolJournalParameter.ProjectSequenceId = Project;
            _PmolJournalParameter.Lang = lang;
            _PmolJournalParameter.Id = mPmolDto.Header.Id;
            _PmolJournalParameter.ContextAccessor = ContextAccessor;
            _PmolJournalParameter.TenantProvider = ItenantProvider;

            mPmolDto.Journal = await _IPmolJournalRepository.ReadJournal(_PmolJournalParameter);
            _pmolParameter.Id = mPmolDto.Header.Id;
            mPmolDto.MapLocation = await _iPmolRepository.GetLocationByPmolId(_pmolParameter);

            mPmolDto.ExtraWork = await _iPmolRepository.GetExtraWorkByPmolId(_pmolParameter);
            mPmolDto.StopHandshake = await _iPmolRepository.GetStopHandShakesByPmolId(_pmolParameter);
            mPmolDto.PmolService = await _iPmolRepository.ReadPmolServiceByPmolId(_pmolParameter);

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.TenantProvider = ItenantProvider;
            mPmolDto.Customer = await _iPsRepository.GetCustomer(_pmolParameter);

            return Ok(new ApiOkResponse(mPmolDto));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreatePmolService")]
    public async Task<ActionResult> CreatePmolService([FromBody] PmolServiceCreateDto pmolDto)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("CreateHeader" + JsonToStringConverter.getStringFromJson(pmolDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.PmolServiceCreate = pmolDto;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var id = await _iPmolRepository.CreatePmolService(_pmolParameter);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadPmolServiceByPmolId/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadPmolServiceByPmolId(string id)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            var data = await _iPmolRepository.ReadPmolServiceByPmolId(_pmolParameter);

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadPmolByUserIdForMobile")]
    public async Task<ActionResult> ReadPmolByUserIdMobile()
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                UserId = objectIdentifier
            };
            

            return Ok(new ApiOkResponse(await _iPmolRepository.GetPmolByUserIdMobile(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
    
    [HttpGet("ReadPmolByUserIdForMobileChanged")]
    public async Task<ActionResult> ReadPmolByUserIdForMobileChanged()
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter
            {
                ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                ProjectSequenceId = Request.Headers["Project"].FirstOrDefault(),
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = _TenantProvider,
                UserId = objectIdentifier
            };
            

            return Ok(new ApiOkResponse(await _iPmolRepository.GetPmolByUserIdMobileChanged(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ForemanCheckForMobile/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ForemanCheckForMobile(string id)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _pmolParameterN = new PmolParameter();
            _pmolParameterN.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameterN.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameterN.Lang = lang;
            _pmolParameterN.ContextAccessor = ContextAccessor;
            _pmolParameterN.TenantProvider = _TenantProvider;
            _pmolParameterN.UserId = objectIdentifier;
            _pmolParameterN.Id = id;
            var foremanCheck = await _iPmolRepository.ForemanCheckMobile(_pmolParameterN);

            var mApiOkResponse = new ApiOkResponse(foremanCheck);

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("UpdatePmolLabourEndTime")]
    public async Task<ActionResult> UpdatePmolLabourEndTime([FromBody] PmolLabourEndTime PmolLabourEndTime)
    {
        try
        {
            //_logger.LogTrace("Started");
            //_logger.LogError("CreateHeader" + JsonToStringConverter.getStringFromJson(pmolDto));
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.PmolLabourEndTime = PmolLabourEndTime;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var id = await _iPmolRepository.UpdatePmolLabourEndTime(_pmolParameter);
            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetWeatherForecast")]
    public async Task<ActionResult> GetWeatherForecast(double lat, double lon, DateTime date, string time)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.Lang = lang;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.borRepository = _borRepository;
            _pmolParameter.Configuration = _iConfiguration;
            _pmolParameter.IShiftRepository = _iShiftRepository;
            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var pmol = await _iPmolRepository.ClonePmolDayPlanning(_pmolParameter);
            var pmol = await _iShiftRepository.GetWeatherForecast(lat, lon, date, time, ItenantProvider,
                _iConfiguration);
            var mApiOkResponse = new ApiOkResponse(pmol);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetPMolPlannedWorkLabour/{Id}")]
    public async Task<ActionResult> GetPMolPlannedWorkLabour(string Id)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.Id = Id;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            var shortcutPaneData = await _iPmolRepository.GetPMolPlannedWorkLabour(_pmolParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("GetPmolByPerson")]
    public async Task<ActionResult> GetPmolByPerson([FromBody] GetPmolByPersonDto GetPmolByPersonDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.GetPmolByPersonDto = GetPmolByPersonDto;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            var shortcutPaneData = await _iPmolRepository.GetPmolByPerson(_pmolParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("GetTrAppData")]
    public async Task<ActionResult> GetTrAppData([FromBody] GetPmolByPersonDto GetPmolByPersonDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.GetPmolByPersonDto = GetPmolByPersonDto;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            return Ok(new ApiOkResponse(await _iPmolRepository.GetTrAppData(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreatePersonCommentCard")]
    public async Task<ActionResult> CreatePersonCommentCard(
        [FromBody] PmolPersonCommentCardDto PmolPersonCommentCardDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.PmolPersonCommentCardDto = PmolPersonCommentCardDto;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            var shortcutPaneData = await _iPmolRepository.CreatePersonCommentCard(_pmolParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("AddPersonComment")]
    public async Task<ActionResult> AddPersonComment([FromBody] PmolPersonCommentDto PmolPersonComment)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.PmolPersonComment = PmolPersonComment;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            var shortcutPaneData = await _iPmolRepository.AddPersonComment(_pmolParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("GetPersonCommentCard")]
    public async Task<ActionResult> GetPersonCommentCard([FromBody] PmolPersonCommentCardDto PmolPersonCommentCardDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.PmolPersonCommentCardDto = PmolPersonCommentCardDto;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            var shortcutPaneData = await _iPmolRepository.GetPersonCommentCard(_pmolParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("PmolStatusUpdate")]
    public async Task<ActionResult> PmolStatusUpdate([FromBody] PmolStatusUpdateDto PmolStatusUpdateDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.PmolStatusUpdateDto = PmolStatusUpdateDto;
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            _pmolParameter.ContractingUnitSequenceId = PmolStatusUpdateDto.SequenceCode;
            _pmolParameter.ProjectSequenceId = PmolStatusUpdateDto.ProjectId;
            _pmolParameter.Id = PmolStatusUpdateDto.ProjectMoleculeId;
            var shortcutPaneData = await _iPmolRepository.PmolStatusUpdate(_pmolParameter, PmolStatusUpdateDto.StatusId,
                PmolStatusUpdateDto.ProjectMoleculeId);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("GetPmolCommentCard")]
    public async Task<ActionResult> GetPmolCommentCard([FromBody] PmolPersonCommentCardDto PmolPersonCommentCardDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.PmolPersonCommentCardDto = PmolPersonCommentCardDto;
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.UserId = objectIdentifier;
            var shortcutPaneData = await _iPmolRepository.GetPmolCommentCard(_pmolParameter);

            return Ok(new ApiOkResponse(shortcutPaneData));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("GetBuPersonWithCompetence")]
    public async Task<ActionResult> GetBuPersonWithCompetence([FromBody] PmolPerson PmolPerson)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.Configuration = _iConfiguration;
            _pmolParameter.UserId = objectIdentifier;
            _pmolParameter.PmolPerson = PmolPerson;
            var data = await _iPmolRepository.GetBuPersonWithCompetence(_pmolParameter);

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("PmolDataForPrint")]
    public async Task<ActionResult> PmolDataForPrint([FromBody] PmolDataForPrintDto PmolDataForPrintDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.Configuration = _iConfiguration;
            _pmolParameter.UserId = objectIdentifier;
            _pmolParameter.PmolDataForPrintDto = PmolDataForPrintDto;
            var data = await _iPmolRepository.PmolDataForPrint(_pmolParameter);

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("ClonePmolMultipleDays")]
    public async Task<ActionResult> ClonePmolMultipleDays([FromBody] MultiplePmolClone MultiplePmolClone)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.Lang = lang;
            _pmolParameter.Id = MultiplePmolClone.PmolId;
            _pmolParameter.ContractingUnitSequenceId = MultiplePmolClone.ContractingUinit;
            _pmolParameter.ProjectSequenceId = MultiplePmolClone.ProjectSequenceCode;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = ItenantProvider;
            _pmolParameter.borRepository = _borRepository;
            _pmolParameter.Configuration = _iConfiguration;
            _pmolParameter.IShiftRepository = _iShiftRepository;
            _pmolParameter.MultiplePmolClone = MultiplePmolClone;
            _pmolParameter.IPmolResourceRepository = _iPmolResourceRepository;
            _pmolParameter.IVpRepository = _iVpRepository;
            _pmolParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var pmol = await _iPmolRepository.ClonePmolMultipleDays(_pmolParameter);
            var mApiOkResponse = new ApiOkResponse(pmol);
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("AddPmolCbcResource")]
    public async Task<ActionResult> AddPmolCbcResource([FromBody] PmolCbcResources dto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _TenantProvider;
            _pmolParameter.Configuration = _iConfiguration;
            _pmolParameter.UserId = objectIdentifier;
            _pmolParameter.PmolCbcResources = dto;
            var data = await _iPmolRepository.AddPmolCbcResource(_pmolParameter);

            return Ok(new ApiOkResponse(data));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeletePmolCbcResource")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeletePmolCbcResource([FromBody] List<string> idList)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.IdList = idList;
            _pmolParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iPmolRepository.DeletePmolCbcResource(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetPmolCbcResourcesById/{Id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPmolCbcResourcesById(string id)
    {
        try
        {
            var lang = Request.Headers["lang"].FirstOrDefault();
            var _pmolParameter = new PmolParameter();

            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            _pmolParameter.Lang = lang;
            _pmolParameter.Id = id;
            _pmolParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(await _iPmolRepository.GetPmolCbcResourcesById(_pmolParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}