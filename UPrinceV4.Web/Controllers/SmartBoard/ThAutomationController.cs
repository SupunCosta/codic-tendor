using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.ThAutomation;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.ThAutomation;

namespace UPrinceV4.Web.Controllers.SmartBoard;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class SmartBoardController : CommonConfigurationController
{
    private readonly IPbsRepository _iPbsRepository;
    private readonly IShiftRepository _iShiftRepository;
    private readonly IThAutomationRepository _iThAutomationRepository;
    private readonly ITenantProvider _TenantProvider;
    //private readonly ThAutomationParameter _thAutomationParameter;
    private readonly IStockRepository _iStockRepository;


    public SmartBoardController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider
        , IConfiguration iConfiguration, IThAutomationRepository iThAutomationRepository,
        IShiftRepository iShiftRepository, IPbsRepository iPbsRepository,IStockRepository iStockRepository)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)

    {
        //_thAutomationParameter = new ThAutomationParameter();
        _iThAutomationRepository = iThAutomationRepository;
        _TenantProvider = tenantProvider;
        _iConfiguration = iConfiguration;
        _iShiftRepository = iShiftRepository;
        _iPbsRepository = iPbsRepository;
        _iStockRepository = iStockRepository;

    }

    private IConfiguration _iConfiguration { get; }

    [Authorize]
    [HttpPost("GetProductsWithTrucks")]
    public async Task<ActionResult> GetProductsWithTrucks([FromBody] ThProductWithTrucksDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ThProductWithTrucksDto = dto
            };
            //return Ok(new ApiOkResponse(await _iThAutomationRepository.GetProductsWithTrucks(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.GetProductsWithTrucksForMyEnv(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [Authorize]
    [HttpPost("GetTruckAssignData")]
    public async Task<ActionResult> GetTruckAssignData([FromBody] TruckAssignDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                TruckAssignDto = dto
            };
            // return Ok(new ApiOkResponse(await _iThAutomationRepository.GetTruckAssignData(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.GetTruckAssignDataForMyEnv(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [Authorize]
    [HttpPost("GetTruckWithProduct")]
    public async Task<ActionResult> GetTruckWithProduct([FromBody] ThTruckWithProductDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ThTruckWithProductDto = dto
            };
            // return Ok(new ApiOkResponse(await _iThAutomationRepository.GetTruckWithProduct(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.GetTruckWithProductForMyEnv(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [Authorize]
    [HttpPost("AddTrucksToProduct")]
    public async Task<ActionResult> AddTrucksToProduct([FromBody] GetThProductWithTrucksDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                GetThProductWithTrucksDto = dto,
                _iShiftRepository = _iShiftRepository,
                Configuration = _iConfiguration,
                iPbsRepository = _iPbsRepository,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            //return Ok(new ApiOkResponse(await _iThAutomationRepository.AddTrucksToProduct(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.AddTrucksToProductMyEnv(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [Authorize]
    [HttpPost("RemoveTruckFromDay")]
    public async Task<ActionResult> RemoveTruckFromDay([FromBody] RemoveTruckFromDayDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                RemoveTruckFromDayDto = dto,
                _iShiftRepository = _iShiftRepository,
                Configuration = _iConfiguration
            };

            // return Ok(new ApiOkResponse(await _iThAutomationRepository.RemoveTruckFromDay(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.RemoveTruckFromDayForMyEnv(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [Authorize]
    [HttpPost("CalculateDistance")]
    public async Task<ActionResult> CalculateDistance([FromBody] CaculateDistance dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                CaculateDistance = dto,
                _iShiftRepository = _iShiftRepository,
                Configuration = _iConfiguration
            };

            return Ok(new ApiOkResponse(await _iThAutomationRepository.CalculateDistance(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [Authorize]
    [HttpPost("RemoveTHProduct")]
    public async Task<ActionResult> RemoveTHProduct([FromBody] RemoveThProductDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                RemoveThProductDto = dto,
                _iShiftRepository = _iShiftRepository,
                Configuration = _iConfiguration
            };

            return Ok(new ApiOkResponse(await _iThAutomationRepository.RemoveTHProduct(thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [AllowAnonymous]
    [HttpPost("DeliveryNoteUpload")]
    public async Task<ActionResult<string>> DeliveryNoteUpload([FromForm] IFormCollection zip,
        [FromForm] string ProgressStatement, [FromForm] string Customer,
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
            var thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                Zip = zip,
                TenantProvider = ItenantProvider,
                ContextAccessor = ContextAccessor
            };


            var th = new WeTransfer
            {
                ProgressStatement = ProgressStatement.Replace(" ",""),
                Customer = Customer.Replace(" ","")
            };

            thAutomationParameter.WeTransfer = th;

            if (zip.Files.FirstOrDefault() != null)
            {
                return Ok(new ApiOkResponse(await _iThAutomationRepository.ThFileUpload(thAutomationParameter)));

            }
            else
            {
                throw new Exception("Please upload file");
            }
            //            var client = new FileClient();
            //            var url = client.PersistThZipUpload(zip.Files.FirstOrDefault()?.FileName, _TenantProvider
            //                , zip.Files.FirstOrDefault(), ZipProject, ZipProduct);

            //            var response = new ApiOkResponse(url);
            //            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [HttpGet("DownloadMultipleFiles/{lotId}")]
    public async Task<ActionResult<string>> DownloadMultipleFiles(string lotId,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var tenant = ItenantProvider.GetTenant();
            //var connectionString = ConnectionString.MapConnectionString(CU, Project, ItenantProvider);

            var blobContainerClient = new BlobContainerClient(_TenantProvider.GetTenant().StorageConnectionString,
                _TenantProvider.GetTenant().AzureContainer);
            var blobs = blobContainerClient.GetBlobs(prefix: lotId);

            var vv = blobs.Count();
            var zipFileName = "LotDocuments.zip";

            foreach (var blobItem in blobs)
            {
                // Download the blob to a temporary file
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                var tempFilePath = Path.GetTempFileName();
                using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Write, FileShare.Write))
                {
                    blobClient.DownloadTo(fileStream);
                }

                // Add the temporary file to the zip archive
                using (var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var archive = ZipFile.Open(zipFileName, ZipArchiveMode.Update))
                    {
                        var entryName = Path.GetFileName(blobItem.Name);
                        archive.CreateEntryFromFile(fileStream.Name, entryName);
                    }
                }

                // Delete the temporary file
                System.IO.File.Delete(tempFilePath);
            }

            string zipPath = null;
            // Upload the zip file to Azure Blob Storage
            var zipBlobClient = blobContainerClient.GetBlobClient(zipFileName);
            using (var fileStream = new FileStream(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                zipBlobClient.Upload(fileStream, true);
                zipPath = fileStream.Name;

            }

            System.IO.File.Delete(zipPath);

            // Delete the zip file
            // File.Delete(zipFileName);

            return zipBlobClient.Uri.AbsoluteUri;
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }

    [Authorize]
    [HttpPost("UpdateThProduct")]
    public async Task<ActionResult> UpdateThProduct([FromBody] GetThTrucksSchedule dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                UpdateThProduct = dto,
                _iShiftRepository = _iShiftRepository,
                Configuration = _iConfiguration
            };

            return Ok(new ApiOkResponse(await _iThAutomationRepository.UpdateThProduct(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    [AllowAnonymous]
    [HttpPost("CreateVehicleResourceFamily")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateVehicleResourceFamily([FromBody] CpcResourceFamilyLocalizedData CpcResourceFamilyLocalizedData,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = "COM-0001",
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                cpcResourceFamily = CpcResourceFamilyLocalizedData
            };
            var result = await _iThAutomationRepository.CreateVehicleResourceFamily(_thAutomationParameter);
            var mApiOkResponse = new ApiOkResponse(result);
            
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [Authorize]
    [HttpPost("AddPumpsToProduct")]
    public async Task<ActionResult> AddPumpsToProduct([FromBody] GetThProductWithTrucksDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                GetThProductWithTrucksDto = dto,
                _iShiftRepository = _iShiftRepository,
                Configuration = _iConfiguration,
                iPbsRepository = _iPbsRepository,
                UserId = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                    claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value
            };

            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.AddPumpsToProductMyEnv(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [Authorize]
    [HttpPost("GetPumpWithProduct")]
    public async Task<ActionResult> GetPumpWithProduct([FromBody] ThTruckWithProductDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ThTruckWithProductDto = dto
            };
            // return Ok(new ApiOkResponse(await _iThAutomationRepository.GetTruckWithProduct(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.GetPumpsWithProductForMyEnv(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [Authorize]
    [HttpPost("GetThResourceFamilies")]
    public async Task<ActionResult> GetThResourceFamilies([FromBody] ThResourceFamiliesDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ThResourceFamiliesDto = dto
            };
            // return Ok(new ApiOkResponse(await _iThAutomationRepository.GetTruckWithProduct(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.GetThResourceFamilies(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [HttpPost("AddTruckAvailableTimes")]
    public async Task<ActionResult> AddTruckAvailableTimes([FromBody] ThTruckAvailabilityDto dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ThTruckAvailabilityDto = dto
            };
            // return Ok(new ApiOkResponse(await _iThAutomationRepository.GetTruckWithProduct(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.AddTruckAvailableTimes(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    
    [HttpPost("DeleteThStockAvailableTime")]
    public async Task<ActionResult> DeleteThStockAvailableTime([FromBody] ThStockDelete dto,
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var _thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider,
                ThStockDelete = dto,
                iStockRepository = _iStockRepository
            };

            // return Ok(new ApiOkResponse(await _iThAutomationRepository.GetTruckWithProduct(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.DeleteThStockAvailableTime(_thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
    
    [Authorize]
    [HttpGet("GetColourCodes")]
    public async Task<ActionResult> GetColourCodes(
        [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);
            var thAutomationParameter = new ThAutomationParameter
            {
                ContractingUnitSequenceId = CU,
                ProjectSequenceId = Project,
                Lang = lang,
                ContextAccessor = ContextAccessor,
                TenantProvider = ItenantProvider
            };

            // return Ok(new ApiOkResponse(await _iThAutomationRepository.GetTruckWithProduct(_thAutomationParameter)));
            return Ok(new ApiOkResponse(
                await _iThAutomationRepository.GetColourCodes(thAutomationParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));        }
    }
}