using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.UserException;

namespace UPrinceV4.Web.Controllers.PBS;


[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class PbsInstructionController : CommonConfigurationController
{
    private readonly IPbsInstructionFamilyRepository _iPbsInstructionFamilyRepository;
    private readonly IPbsInstructionsRepository _iPbsInstructionsRepository;

    public PbsInstructionController(ApplicationDbContext uPrinceCustomerContext,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IPbsInstructionsRepository iPbsInstructionsRepository,
        IPbsInstructionFamilyRepository iPbsInstructionFamilyRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        _iPbsInstructionsRepository = iPbsInstructionsRepository;
        _iPbsInstructionFamilyRepository = iPbsInstructionFamilyRepository;
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("CreateInstructions")]
    [HttpPost("CreateInstructions")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateInstructions(
        [FromBody] PbsInstructionDto pbsInstruction)
    {
        try
        {
            var _pbsInstructionsRepositoryParameter = new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };

            var pbsInstructions = new Instructions
            {
                Id = pbsInstruction.Id,
                Name = pbsInstruction.Name,
                InstructionsDetails = pbsInstruction.InstructionsDetails,
                PbsInstructionFamilyId = pbsInstruction.PbsInstructionFamilyId,
                InstructionType = pbsInstruction.InstructionType
            };

            var instructions = new List<PbsInstructionLink>();
            foreach (var inst in pbsInstruction.PbsInstructionLink)
            {
                var link = new PbsInstructionLink
                {
                    Id = inst.Id,
                    Title = inst.Title,
                    Type = inst.Type,
                    Link = inst.Value,
                    PbsInstructionId = inst.PbsInstructionId
                };
                instructions.Add(link);
            }

            pbsInstructions.PbsInstructionLink = instructions;
            _pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            _pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _pbsInstructionsRepositoryParameter.PbsInstruction = pbsInstructions;

            var id = await _iPbsInstructionsRepository.AddPbsInstruction(_pbsInstructionsRepositoryParameter);


            return Ok(new ApiOkResponse(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadInstructionsList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetInstructionsList()
    {
        try
        {
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter();
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            pbsInstructionsRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.Lang = lang;

            var instructionList =
                await _iPbsInstructionsRepository.GetPbsInstructionist(pbsInstructionsRepositoryParameter);

            if (instructionList == null || !instructionList.Any())
                //string emptyMessage = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableRisk, lang).Message;
                return Ok(new ApiOkResponse(instructionList, "noAvailableRisk"));

            return Ok(new ApiOkResponse(instructionList));
        }
        catch (EmptyListException ex)
        {
            return Ok(new ApiResponse(200, false, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("InstructionsFilter")]
  
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> InstructionsFilter([FromBody] PbsInstructionFilter filter)
    {
        try
        {
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter();
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            pbsInstructionsRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.Lang = lang;
            pbsInstructionsRepositoryParameter.Filter = filter;

            var instructionList =
                await _iPbsInstructionsRepository.InstructionsFilter(pbsInstructionsRepositoryParameter);

            if (instructionList == null || !instructionList.Any())
                //string emptyMessage = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableRisk, lang).Message;
                return Ok(new ApiOkResponse(instructionList, "noAvailableRisk"));

            return Ok(new ApiOkResponse(instructionList));
        }
        catch (EmptyListException ex)
        {
            return Ok(new ApiResponse(200, false, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

  
    [HttpGet("ReadInstructions/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetInstructionsById(string id)
    {
        try
        {
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsInstructionId = id;
            pbsInstructionsRepositoryParameter.Lang = lang;
            var pbsInstruction =
                await _iPbsInstructionsRepository.GetPbsInstructionById(pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadInstructionsByPbsProduct")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetInstructionsByPbsProductId()
    {
        try
        {
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = HttpContext.Request.Query["pbsId"];
            pbsInstructionsRepositoryParameter.InstructionType = HttpContext.Request.Query["instructionType"];
            pbsInstructionsRepositoryParameter.Lang = lang;

            var pbsInstruction =
                await _iPbsInstructionsRepository.GetPbsInstructionsByPbsProductId(
                    pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadPictureOfInstallationInstructionsByPbsProductId/{pbsId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPictureOfInstallationInstructionsByPbsProductId(string pbsId)
    {
        try
        {
            //Picture of the installation
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = pbsId;
            pbsInstructionsRepositoryParameter.InstructionType = "100";
            pbsInstructionsRepositoryParameter.Lang = lang;

            var pbsInstruction =
                await _iPbsInstructionsRepository.GetPictureOfInstallationInstructionsByPbsProductId(
                    pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadPipingInstrumentationDiagramInstructionsByPbsProductId/{pbsId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetPipingInstrumentationDiagramInstructionsByPbsProductId(string pbsId)
    {
        try
        {
            //Picture of the installation
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter();
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            pbsInstructionsRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = pbsId;
            pbsInstructionsRepositoryParameter.InstructionType = "100";
            pbsInstructionsRepositoryParameter.Lang = lang;

            var pbsInstruction =
                await _iPbsInstructionsRepository.GetPipingInstrumentationDiagramByPbsProductId(
                    pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("ReadWeldingProceduresSpecificationsInstructionsPbsProductId/{pbsId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetWeldingProceduresSpecificationsInstructionsPbsProductId(string pbsId)
    {
        try
        {
            //Picture of the installation
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = pbsId;
            pbsInstructionsRepositoryParameter.InstructionType = "100";
            pbsInstructionsRepositoryParameter.Lang = lang;

            var pbsInstruction =
                await _iPbsInstructionsRepository.GetWeldingProceduresSpecificationsPbsProductId(
                    pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("ReadIsoMetricDrawingsInstructionsPbsProductId/{pbsId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetIsoMetricDrawingsInstructionsPbsProductId(string pbsId)
    {
        try
        {
            //Picture of the installation
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = pbsId;
            pbsInstructionsRepositoryParameter.InstructionType = "100";
            pbsInstructionsRepositoryParameter.Lang = lang;

            var pbsInstruction =
                await _iPbsInstructionsRepository.GetIsoMetricDrawingsPbsProductId(
                    pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    [HttpGet("ReadHealthSafetyEnvironmenInstructionsByPbsProductId/{pbsId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetHealthSafetyEnvironmenInstructionsByPbsProductId(string pbsId)
    {
        try
        {
            //Picture of the installation
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());


            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = pbsId;
            pbsInstructionsRepositoryParameter.Lang = lang;

            var pbsInstruction =
                await _iPbsInstructionsRepository.GetHealthSafetyEnvironmenInstructionsByPbsProductId(
                    pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }


    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadInstructionsByPbsProduct")]
    [HttpGet("GetAllInstructionsByPbsProductId")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllInstructionsByPbsProductId([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId = CU;
            pbsInstructionsRepositoryParameter.ProjectSequenceId = Project;
            pbsInstructionsRepositoryParameter.Lang = lang;

            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = HttpContext.Request.Query["pbsId"];
            pbsInstructionsRepositoryParameter.InstructionType = HttpContext.Request.Query["instructionType"];

            var pbsInstruction =
                await _iPbsInstructionsRepository.GetAllPbsInstructionsByPbsProductId(
                    pbsInstructionsRepositoryParameter);

            if (pbsInstruction == null)
                //string emptyMessage = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.NoAvailableRiskForTheId, lang).Message;
                return Ok(new ApiOkResponse(null, "noAvailableRiskForTheId"));

            return Ok(new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetAllPbsInstructionsByPbsProductAllTypes")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllPbsInstructionsByPbsProductAllTypes([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromQuery(Name = "pbsId")] string pbsId,
        [FromQuery(Name = "instructionType")] string instructionType)
    {
        try
        {
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId = CU;
            pbsInstructionsRepositoryParameter.ProjectSequenceId = Project;
            pbsInstructionsRepositoryParameter.Lang = lang;

            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = pbsId;
            pbsInstructionsRepositoryParameter.InstructionType = instructionType;
            return Ok(new ApiOkResponse(await _iPbsInstructionsRepository.GetAllPbsInstructionsByPbsProductAllTypes(
                pbsInstructionsRepositoryParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("GetAllInstructionsByPbsProductIdAll")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllInstructionsByPbsProductIdAll([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId = CU;
            pbsInstructionsRepositoryParameter.ProjectSequenceId = Project;
            pbsInstructionsRepositoryParameter.Lang = lang;

            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = HttpContext.Request.Query["pbsId"];


            var pbsInstruction =
                await _iPbsInstructionsRepository.GetAllPbsInstructionsByPbsProductId(
                    pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("DeleteInstructions")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteInstructions([FromBody] List<string> idList)
    {
        try
        {
            var _pbsInstructionsRepositoryParameter = new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            _pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            _pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _pbsInstructionsRepositoryParameter.IdList = idList;
            await _iPbsInstructionsRepository.DeletePbsInstruction(_pbsInstructionsRepositoryParameter);
            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpDelete("RemoveInstruction")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteInstruction([FromBody] List<string> idList)
    {
        try
        {
            var _pbsInstructionsRepositoryParameter = new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            _pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            _pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _pbsInstructionsRepositoryParameter.IdList = idList;
            await _iPbsInstructionsRepository.DeleteInstruction(_pbsInstructionsRepositoryParameter);
            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadInstructionDropdowns")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetInstructionDropdowns()
    {
        try
        {
            var _pbsInstructionsRepositoryParameter = new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            _pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            _pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _pbsInstructionsRepositoryParameter.Lang = lang;
            var _pbsInstructionFamilyRepositoryParameter = new PbsInstructionFamilyRepositoryParameter();
            _pbsInstructionFamilyRepositoryParameter.TenantProvider = ItenantProvider;
            _pbsInstructionFamilyRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            _pbsInstructionFamilyRepositoryParameter.Lang = lang;
            _pbsInstructionFamilyRepositoryParameter.ContractingUnitSequenceId =
                Request.Headers["CU"].FirstOrDefault();
            _pbsInstructionFamilyRepositoryParameter.ProjectSequenceId =
                Request.Headers["Project"].FirstOrDefault();
            _pbsInstructionsRepositoryParameter.IPbsInstructionFamilyRepository = _iPbsInstructionFamilyRepository;
            _pbsInstructionsRepositoryParameter.PbsInstructionFamilyRepositoryParameter =
                _pbsInstructionFamilyRepositoryParameter;


            return Ok(new ApiOkResponse(
                await _iPbsInstructionsRepository.GetPbsInstructionDropdownData(
                    _pbsInstructionsRepositoryParameter)));
        }
        catch (EmptyListException ex)
        {
            return BadRequest(new ApiResponse(200, false, ex.Message));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("UploadDocument")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadDocument([FromForm] IFormCollection document,
        [FromForm] string id)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var _pbsInstructionsRepositoryParameter = new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            _pbsInstructionsRepositoryParameter.Image = document;
            _pbsInstructionsRepositoryParameter.FolderName = "Instruction_" + id;
            _pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(
                await _iPbsInstructionsRepository.UploadImage(_pbsInstructionsRepositoryParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpPost("CreatePbsInstruction")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> CreatePbsInstruction([FromBody] CreateInstructionDto instruction)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

            var _pbsInstructionsRepositoryParameter = new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            _pbsInstructionsRepositoryParameter.CreateInstruction = instruction;
            _pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            return Ok(new ApiOkResponse(
                await _iPbsInstructionsRepository.CreatePbsInstruction(_pbsInstructionsRepositoryParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("ReadAllInstructionsByPbsProductId/{pbsId}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadAllInstructionsByPbsProductId(string pbsId)
    {
        try
        {
            //Picture of the installation
            var pbsInstructionsRepositoryParameter =
                new PbsInstructionsRepositoryParameter
                {
                    ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault(),
                    ProjectSequenceId = Request.Headers["Project"].FirstOrDefault()
                };
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
            pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            pbsInstructionsRepositoryParameter.PbsProductId = pbsId;
            pbsInstructionsRepositoryParameter.InstructionType = "100";
            pbsInstructionsRepositoryParameter.Lang = lang;

            var pbsInstruction =
                await _iPbsInstructionsRepository.ReadAllInstructionsByPbsProductId(
                    pbsInstructionsRepositoryParameter);

            return Ok(pbsInstruction == null
                ? new ApiOkResponse(null, "noAvailableRiskForTheId")
                : new ApiOkResponse(pbsInstruction));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}