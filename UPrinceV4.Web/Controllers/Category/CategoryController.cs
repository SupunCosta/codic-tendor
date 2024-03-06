using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.Category;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.Category;

namespace UPrinceV4.Web.Controllers.Category;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoryController : CommonConfigurationController
{
    private readonly CategoryParameter _categoryParameter;
    private readonly ApplicationDbContext _DbContext;
    private readonly ICategoryRepository _iCategoryRepository;
    private readonly ILogger<CategoryController> _logger;
    private readonly ITenantProvider _TenantProvider;

    public CategoryController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApplicationDbContext dbContext,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        CategoryParameter CategoryParameter, ILogger<CategoryController> logger, ITenantProvider iTenantProvider
        , ICategoryRepository iCategoryRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        _iCategoryRepository = iCategoryRepository;
        _categoryParameter = new CategoryParameter();
        _TenantProvider = tenantProvider;
        _DbContext = dbContext;
        _logger = logger;
    }

    [HttpPost("CreateCategory")]
    public async Task<ActionResult> CreateCategory(
        [FromBody] CategoryLevelCreateDto CategoryDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
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

            _categoryParameter.ContractingUnitSequenceId = CU;
            _categoryParameter.ProjectSequenceId = Project;

            _categoryParameter.Lang = lang;
            _categoryParameter.CategoryDto = CategoryDto;
            _categoryParameter.ContextAccessor = ContextAccessor;
            _categoryParameter.TenantProvider = ItenantProvider;
            _categoryParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims
                .First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                .Value;
            var s = await _iCategoryRepository.CreateCategory(_categoryParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("GetCategory")]
    public async Task<ActionResult> GetCategory(
        [FromBody] CategoryLevelCreateDto CategoryDto, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
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

            _categoryParameter.ContractingUnitSequenceId = CU;
            _categoryParameter.ProjectSequenceId = Project;

            _categoryParameter.Lang = lang;
            _categoryParameter.CategoryDto = CategoryDto;
            _categoryParameter.ContextAccessor = ContextAccessor;
            _categoryParameter.TenantProvider = ItenantProvider;
            _categoryParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims
                .First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                .Value;
            var s = await _iCategoryRepository.GetCategory(_categoryParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreatePost")]
    public async Task<ActionResult> CreatePost([FromBody] PostDto PostDto,
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

            _categoryParameter.ContractingUnitSequenceId = CU;
            _categoryParameter.ProjectSequenceId = Project;

            _categoryParameter.Lang = lang;
            _categoryParameter.PostDto = PostDto;
            _categoryParameter.ContextAccessor = ContextAccessor;
            _categoryParameter.TenantProvider = ItenantProvider;
            _categoryParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims
                .First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                .Value;
            var s = await _iCategoryRepository.CreatePost(_categoryParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetPostById/{PostId}")]
    public async Task<ActionResult> GetPostById(string PostId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
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

            _categoryParameter.ContractingUnitSequenceId = CU;
            _categoryParameter.ProjectSequenceId = Project;

            _categoryParameter.Lang = lang;
            _categoryParameter.Id = PostId;
            _categoryParameter.ContextAccessor = ContextAccessor;
            _categoryParameter.TenantProvider = ItenantProvider;
            _categoryParameter.UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims
                .First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                .Value;
            var s = await _iCategoryRepository.GetPostById(_categoryParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}