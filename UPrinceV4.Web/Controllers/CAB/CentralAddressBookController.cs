using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Contract;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.UserException;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Controllers.CAB;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class CentralAddressBookController : CommonConfigurationController
{
    private readonly ICabHistoryLogRepository _iCabHistoryLogRepository;
    private readonly ICompanyRepository _iCompanyRepository;
    private readonly IPersonCompanyRepository _iPersonCompanyRepository;
    private readonly IPersonRepository _iPersonRepository;
    private readonly IUniqueContactDetailsRepository _iUniqueContactDetailsRepository;
    private readonly ILogger<CentralAddressBookController> _logger;
    private readonly ITenantProvider _tenantProvider;


    public CentralAddressBookController(ApplicationDbContext uPrinceCustomerContext,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse,
        IPersonCompanyRepository iPersonCompanyRepository,
        IPersonRepository iPersonRepository, ICompanyRepository iCompanyRepository,
        ICabHistoryLogRepository iCabHistoryLogRepository, ITenantProvider tenantProvider,
        IUniqueContactDetailsRepository iUniqueContactDetailsRepository,
        ILogger<CentralAddressBookController> logger) :
        base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            tenantProvider)
    {
        _iPersonCompanyRepository = iPersonCompanyRepository;
        _iPersonRepository = iPersonRepository;
        _iCompanyRepository = iCompanyRepository;
        _iCabHistoryLogRepository = iCabHistoryLogRepository;
        _iUniqueContactDetailsRepository = iUniqueContactDetailsRepository;
        _tenantProvider = ItenantProvider;
        _logger = logger;
    }

    [HttpGet("ReadCabOrganizationListGroupByCompany")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCabOrganizationListGroupByCompany()
    {
        try
        {

            var _companyRepositoryParameter = new CompanyRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                TenantProvider = _tenantProvider
            };
            var companyList = await _iCompanyRepository.GetCompanyListGroupByCompany(_companyRepositoryParameter);
            if (companyList == null || !companyList.Any())
            {
                ApiOkResponse.Result = companyList;
                ApiOkResponse.Message = "noAvailableOrganizations";
                return Ok(ApiOkResponse);
            }

            var mApiOkResponse = new ApiOkResponse(companyList)
            {
                Status = true,
                Result = companyList,
                Message = "ok"
            };
            return Ok(mApiOkResponse);
        }
        catch (EmptyListException ex)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("ReadCabOrganizationList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCabOrganizationList()
    {
        try
        {
            var _companyRepositoryParameter = new CompanyRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                TenantProvider = _tenantProvider
            };

            var companyList = await _iCompanyRepository.GetCompanyList(_companyRepositoryParameter);
            if (companyList == null || !companyList.Any())
            {
                ApiOkResponse.Result = companyList;
                ApiOkResponse.Message = "noAvailableOrganizations";
                return Ok(ApiOkResponse);
            }

            var mApiOkResponse = new ApiOkResponse(companyList)
            {
                Status = true,
                Result = companyList,
                Message = "ok"
            };
            return Ok(mApiOkResponse);
        }
        catch (EmptyListException ex)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("ReadCabOrganizationList/{name}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCabOrganizationListByName(string name)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        try
        {
            var _companyRepositoryParameter = new CompanyRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                CompanyName = name,
                TenantProvider = ItenantProvider
            };

            var companyList = await _iCompanyRepository.GetCompanyListByName(_companyRepositoryParameter);
            if (companyList == null || !companyList.Any())
            {
                ApiOkResponse.Result = companyList;
                ApiOkResponse.Message = "noAvailableOrganizationsForTheName";
                return Ok(ApiOkResponse);
            }

            var mApiOkResponse = new ApiOkResponse(companyList)
            {
                Status = true,
                Result = companyList,
                Message = "ok"
            };

            return Ok(mApiOkResponse);
        }
        catch (EmptyListException)
        {
            var message = ApiErrorMessages
                .GetErrorMessage(ItenantProvider, ErrorMessageKey.NoOrganisationInYourSearchCriteria, lang)
                .Message;
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = message;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadCabPersonList/{name}")]
    [HttpGet("ReadCabPersonList/{name}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCabPersonListByName(string name)
    {
        var lang = langCode(Request.Headers["lang"].FirstOrDefault());
        if (lang.Contains("nl")) lang = "nl";

        if (lang.Contains("en")) lang = "en";

        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                Name = name,
                TenantProvider = ItenantProvider
            };

            var personList = await _iPersonRepository.GetPersonListByName(_personRepositoryParameter);
            if (personList == null || !personList.Any())
                return Ok(new ApiOkResponse(null, "noAvailablePersonForTheName"));

            var mApiOkResponse = new ApiOkResponse(personList);
            return Ok(mApiOkResponse);
        }
        catch (EmptyListException)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "noPeopleInYourSearchCriteria";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("ReadCabPersonList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCabPersonList()
    {
        try
        {
            
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                CompanyRepository = _iCompanyRepository,
                TenantProvider = ItenantProvider
            };

            var personList = await _iPersonRepository.GetPersonList(_personRepositoryParameter);
            if (personList == null || !personList.Any())
            {
                ApiOkResponse.Result = personList;
                ApiOkResponse.Message = "noAvailablePeople";
                return Ok(ApiOkResponse);
            }

            var mApiOkResponse = new ApiOkResponse(personList)
            {
                Status = true,
                Result = personList,
                Message = "ok"
            };

            return Ok(mApiOkResponse);
        }
        catch (EmptyListException ex)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadCabOrganization/{id}")]
    [HttpGet("ReadCabOrganization/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCabOrganizationById(string id)
    {
        try
        {
            var _companyRepositoryParameter = new CompanyRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                CompanyId = id,
                TenantProvider = ItenantProvider
            };

            var company = await _iCompanyRepository.GetCompanyById(_companyRepositoryParameter);
            if (company == null)
            {
                ApiOkResponse.Result = null;
                ApiOkResponse.Message = "noAvailableOrganizationForTheId";
                return Ok(ApiOkResponse);
            }

            return Ok(new ApiOkResponse(company));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("ReadCabPerson/{id}")]
    [HttpGet("ReadCabPerson/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetCabPersonById(string id)
    {
        try
        {
           
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                PersonId = id,
                CompanyRepository = _iCompanyRepository,
                TenantProvider = ItenantProvider
            };

            var person = await _iPersonRepository.GetPersonById(_personRepositoryParameter);
            if (person == null)
            {
                ApiOkResponse.Result = null;
                ApiOkResponse.Message = "emptyMessage";
                return Ok(ApiOkResponse);
            }


            return Ok(new ApiOkResponse(person));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("ReadMobileNumber/{id}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ReadMobileNumberById(string id)
    {
        try
        {
           
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                PersonId = id,
                CompanyRepository = _iCompanyRepository,
                TenantProvider = ItenantProvider
            };

            var person = await _iPersonRepository.GetMobileNumberId(_personRepositoryParameter);
            if (person == null)
            {
                ApiOkResponse.Result = null;
                ApiOkResponse.Message = "noAvailablePersonForTheId";
                return Ok(ApiOkResponse);
            }

            var mApiOkResponse = new ApiOkResponse(person)
            {
                Status = true,
                Result = person,
                Message = "ok"
            };
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("PersonFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PersonFilter(
        [FromBody] CabPersonFilterModel cabPersonFilter)
    {
        try
        {
            var personParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                CabPersonFilter = cabPersonFilter,
                CompanyRepository = _iCompanyRepository,
                TenantProvider = ItenantProvider
            };
            // _personRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
            // _personRepositoryParameter.CabPersonFilter = cabPersonFilter;
            // _personRepositoryParameter.CompanyRepository = _iCompanyRepository;
            // _personRepositoryParameter.TenantProvider = ItenantProvider;
            // _companyRepositoryParameter.TenantProvider = ItenantProvider;
            var personList = await _iPersonRepository.Filter(personParameter);

            var mApiOkResponse = new ApiOkResponse(personList)
            {
                Status = true,
                Result = personList,
                Message = "ok"
            };

            if (!personList.Any())
            {
                var mApiResponse = new ApiOkResponse(null, "noavailableperson")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (EmptyListException)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "noPeopleInYourSearchCriteria";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("ProjectPersonFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ProjectPersonFilter(
        [FromBody] CabPersonFilterModel cabPersonFilter)
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                CabPersonFilter = cabPersonFilter,
                CompanyRepository = _iCompanyRepository,
                TenantProvider = ItenantProvider
            };

            var personList = await _iPersonRepository.ProjectPersonFilter(_personRepositoryParameter);

            var mApiOkResponse = new ApiOkResponse(personList)
            {
                Status = true,
                Result = personList,
                Message = "ok"
            };

            if (!personList.Any())
            {
                var mApiResponse = new ApiOkResponse(personList, "noavailableperson2")
                {
                    Status = false
                };
                return Ok(mApiResponse);
            }

            return Ok(mApiOkResponse);
        }
        catch (EmptyListException)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "noPeopleInYourSearchCriteria";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("IsUsedUniqueContactDetailsFilter")]
    [HttpPost("IsUsedUniqueContactDetailsFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> IsUsedUniqueContactDetailsFilter(
        [FromBody] CabUniqueContactDetailsFilterModel CabUniqueContactDetailsModel)
    {
        try
        {
            var _uniqueContactDetailsRepositoryParameter = new UniqueContactDetailsRepositoryParameter
                {
                    ApplicationDbContext = UPrinceCustomerContext,
                    TenantProvider = _tenantProvider,
                    CabUniqueContactDetailsFilterModel = CabUniqueContactDetailsModel
                };

            var state = await _iUniqueContactDetailsRepository.IsUsedUniqueContact(
                _uniqueContactDetailsRepositoryParameter);

            var mApiOkResponse = new ApiOkResponse(state)
            {
                Status = true,
                Result = state,
                Message = "ok"
            };
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("UsedUniqueContactDetailListFilter")]
    [HttpPost("UsedUniqueContactDetailListFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UsedUniqueContactDetailListFilter(
        [FromBody] CabUniqueContactDetailsFilterModel CabUniqueContactDetailsModel)
    {
        try
        {
            
            var _uniqueContactDetailsRepositoryParameter = new UniqueContactDetailsRepositoryParameter
                {
                    ApplicationDbContext = UPrinceCustomerContext,
                    TenantProvider = _tenantProvider,
                    CabUniqueContactDetailsFilterModel = CabUniqueContactDetailsModel
                };


            var contactList =
                await _iUniqueContactDetailsRepository.GetCabUsedUniqueContactDataList(
                    _uniqueContactDetailsRepositoryParameter);

            var mApiOkResponse = new ApiOkResponse(contactList)
            {
                Status = true,
                Result = contactList,
                Message = "ok"
            };
            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("UploadPersonImage")]
    [HttpPost("UploadPersonImage")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> UploadPersonImage([FromForm] IFormCollection image)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApiBadRequestResponse.ModelState = ModelState;
                return BadRequest(ApiBadRequestResponse);
            }

          
            
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                Image = image,
                TenantProvider = ItenantProvider
            };

            var url = await _iPersonRepository.UploadImage(_personRepositoryParameter);
            ApiOkResponse.Result = url;
            ApiOkResponse.Message = "ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("CreateCabEntry")]
    [HttpPost("CreateCabEntry")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateCabEntry([FromBody] CabDataDto cabDataDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser
                .FirstOrDefault(u => u.OId == objectIdentifier);

            var cabDtoCompanyId = cabDataDto.CompanyId;

            var cabDtoPerson = cabDataDto.Person;
            var person = new CabPerson
            {
                IsSaved = cabDataDto.IsSaved,
                Image = cabDtoPerson.Image,
                Email = new CabEmail { EmailAddress = cabDtoPerson.Email },
                FirstName = cabDtoPerson.FirstName,
                LandPhone = new CabLandPhone { LandPhoneNumber = cabDtoPerson.LandPhone },
                MobilePhone = new CabMobilePhone { MobilePhoneNumber = cabDtoPerson.MobilePhone },
                WhatsApp = new CabWhatsApp { WhatsAppNumber = cabDtoPerson.Whatsapp },
                Skype = new CabSkype { SkypeNumber = cabDtoPerson.Skype },
                Id = cabDtoPerson.Id,
                Surname = cabDtoPerson.Surname,
                FullName = cabDtoPerson.FullName,
                CallName = cabDtoPerson.CallName,
                BusinessEmail = new CabEmail { EmailAddress = cabDtoPerson.BusinessEmail },
                BusinessPhone = new CabMobilePhone { MobilePhoneNumber = cabDtoPerson.BusinessPhone }
            };


            if (cabDtoPerson.Address != null)
            {
                var address = new CabAddress
                {
                    Street = cabDtoPerson.Address.Street,
                    StreetNumber = cabDtoPerson.Address.StreetNumber,
                    City = cabDtoPerson.Address.City,
                    Region = cabDtoPerson.Address.Region,
                    CountryId = cabDtoPerson.Address.CountryId,
                    MailBox = cabDtoPerson.Address.MailBox,
                    PostalCode = cabDtoPerson.Address.PostalCode
                };
                person.Address = address;
            }

            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                PersonId = cabDataDto.Person.Id,
                ApplicationDbContext = UPrinceCustomerContext,
                Person = person
            };

            var _cabHistoryLogRepositoryParameter = new CabHistoryLogRepositoryParameter
            {
                Person = cabDtoPerson,
                ApplicationDbContext = UPrinceCustomerContext
            };

            _personRepositoryParameter.TenantProvider = ItenantProvider;
            _cabHistoryLogRepositoryParameter.ChangedUser = user;
            _personRepositoryParameter.CabHistoryLogRepository = _iCabHistoryLogRepository;
            _personRepositoryParameter.CabHistoryLogRepositoryParameter = _cabHistoryLogRepositoryParameter;
            _personRepositoryParameter.NationalityId = cabDataDto.Person.NationalityId;

            var personId = await _iPersonRepository.AddPerson(_personRepositoryParameter);
            person.Id = personId;


            string personCompanyId = null;
            var cabDtoPersonCompany = cabDataDto.PersonCompany;
            if (cabDtoPersonCompany != null || cabDtoCompanyId != null)
            {
                CabPersonCompany personCompany = null;
                if (cabDtoPersonCompany != null)
                    personCompany = new CabPersonCompany
                    {
                        Id = cabDataDto.PersonCompany.Id,
                        IsSaved = cabDataDto.IsSaved,
                        CompanyId = cabDtoCompanyId,
                        PersonId = person.Id,
                        Email = new CabEmail { EmailAddress = cabDtoPersonCompany.Email },
                        JobRole = cabDtoPersonCompany.JobRole,
                        LandPhone = new CabLandPhone { LandPhoneNumber = cabDtoPersonCompany.LandPhone },
                        MobilePhone = new CabMobilePhone { MobilePhoneNumber = cabDtoPersonCompany.MobilePhone },
                        WhatsApp = new CabWhatsApp { WhatsAppNumber = cabDtoPersonCompany.Whatsapp },
                        Skype = new CabSkype { SkypeNumber = cabDtoPersonCompany.Skype }
                    };
                else
                    personCompany = new CabPersonCompany
                    {
                        CompanyId = cabDtoCompanyId,
                        PersonId = person.Id
                    };

                var _personCompanyRepositoryParameter = new PersonCompanyRepositoryParameter
                {
                    PersonCompany = personCompany,
                    ApplicationDbContext = UPrinceCustomerContext,
                    Logger = _logger
                };


                //adding history
                _cabHistoryLogRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
                _cabHistoryLogRepositoryParameter.CabDataDto = cabDataDto;
                _cabHistoryLogRepositoryParameter.ChangedUser = user;
                _personCompanyRepositoryParameter.TenantProvider = ItenantProvider;
                _personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter =
                    _cabHistoryLogRepositoryParameter;
                _personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter.TenantProvider = ItenantProvider;
                _personCompanyRepositoryParameter.CabHistoryLogRepository = _iCabHistoryLogRepository;
                _personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter.Company =
                    new CompanyDto { Id = cabDataDto.CompanyId };

                try
                {
                    personCompanyId =
                        await _iPersonCompanyRepository.AddPersonCompany(_personCompanyRepositoryParameter);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            var CabIdDataDto = new CabIdDataDto();
            CabIdDataDto.PersonId = person.Id;
            CabIdDataDto.PersonCompanyId = personCompanyId;

            ApiOkResponse.Message = "ok";
            ApiOkResponse.Result = CabIdDataDto;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return BadRequest(ApiResponse);
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("CreateCabOrganization")]
    [HttpPost("CreateCabOrganization")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateCabOrganization([FromBody] CompanyDto companyDto)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser
                .FirstOrDefault(u => u.OId == objectIdentifier);

            var company = new CabCompany
            {
                Id = companyDto.Id,
                Name = companyDto.Name,
                IsSaved = companyDto.IsSaved,
                CabVat = new CabVat
                {
                    Vat = companyDto.VAT
                },
                CabBankAccount = new CabBankAccount
                {
                    BankAccount = companyDto.BankAccount
                },
                Email = new CabEmail { EmailAddress = companyDto.Email },
                LandPhone = new CabLandPhone { LandPhoneNumber = companyDto.LandPhone },
                MobilePhone = new CabMobilePhone { MobilePhoneNumber = companyDto.MobilePhone },
                WhatsApp = new CabWhatsApp { WhatsAppNumber = companyDto.WhatsApp },
                Skype = new CabSkype { SkypeNumber = companyDto.Skype },
                AccountingNumber = companyDto.AccountingNumber,
                ContractorTaxonomyId = companyDto.ContractorTaxonomyId
            };
            if (companyDto.Address != null)
            {
                var address = new CabAddress
                {
                    Street = companyDto.Address.Street,
                    StreetNumber = companyDto.Address.StreetNumber,
                    City = companyDto.Address.City,
                    Region = companyDto.Address.Region,
                    CountryId = companyDto.Address.CountryId,
                    MailBox = companyDto.Address.MailBox,
                    PostalCode = companyDto.Address.PostalCode
                };
                company.Address = address;
            }

            var _companyRepositoryParameter = new CompanyRepositoryParameter
            {
                CompanyId = companyDto.Id,
                ApplicationDbContext = UPrinceCustomerContext,
                Company = company
            };

            //adding history
            var _cabHistoryLogRepositoryParameter = new CabHistoryLogRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                Company = companyDto,
                ChangedUser = user
            };

            _companyRepositoryParameter.CabHistoryLogRepositoryParameter = _cabHistoryLogRepositoryParameter;
            _companyRepositoryParameter.CabHistoryLogRepository = _iCabHistoryLogRepository;
            _companyRepositoryParameter.TenantProvider = _tenantProvider;

            var companyId = await _iCompanyRepository.AddCompany(_companyRepositoryParameter);
            company.Id = companyId;
            ApiOkResponse.Message = "ok";
            ApiOkResponse.Result = company.Id;
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            ApiResponse.StatusCode = 400;
            ApiResponse.Status = false;
            if (ex.InnerException != null) ApiResponse.Message = ex.InnerException.Message;
            ;
            return BadRequest(ApiResponse);
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpDelete("DeleteCabEntry")]
    [HttpDelete("DeleteCabEntry")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCabEntry([FromBody] List<string> idList)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                IdListForDelete = idList
            };

            var companyList = await _iPersonRepository.DeletePerson(_personRepositoryParameter);
            ApiOkResponse.Message = "ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpDelete("DeleteOrganization")]
    [HttpDelete("DeleteOrganization")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteOrganization([FromBody] List<string> idList)
    {
        try
        {
            var _companyRepositoryParameter = new CompanyRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                IdListForDelete = idList
            };

            await _iCompanyRepository.DeleteCompany(_companyRepositoryParameter);
            ApiOkResponse.Message = "ok";
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpGet("IsExistCabEntry")]
    [HttpGet("IsExistCabEntry")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> IsExistCabEntry()
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                TenantProvider = ItenantProvider
            };

            var state = await _iPersonRepository.IsExistCabEntry(_personRepositoryParameter);
            string message = null;
            message = state ? "ok" : "noAvailablePeople";

            var mApiOkResponse = new ApiOkResponse(state)
            {
                Status = true,
                Result = state,
                Message = message
            };

            return Ok(mApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetUnassignedCompanyList")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetUnassignedCompanyList()
    {
        try
        {
            var _companyRepositoryParameter = new CompanyRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                TenantProvider = _tenantProvider,
                CompanyName = HttpContext.Request.Query["name"]
            };

            var companyList = await _iCompanyRepository.GetUnassignedCompanyList(_companyRepositoryParameter);
            if (companyList == null || !companyList.Any())
            {
                ApiOkResponse.Message = "noAvailableOrganizations";
                return Ok(ApiOkResponse);
            }

            ApiOkResponse.Result = companyList;
            ApiOkResponse.Message = "ok";
            return Ok(ApiOkResponse);
        }
        catch (EmptyListException ex)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = ex.Message;
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    //[Microsoft.AspNetCore.Mvc.HttpPost("PersonFilterByName")]
    [HttpPost("PersonFilterByName")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PersonFilterByName(
        [FromBody] PersonFilterByName cabPersonFilterbyName)
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                Name = cabPersonFilterbyName.FullName,
                TenantProvider = ItenantProvider
            };

            var personList = await _iPersonRepository.PersonFilterByName(_personRepositoryParameter);
            var result = personList;
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiOkResponse.Result = result;
            ApiOkResponse.Message = "ok";
            return Ok(ApiOkResponse);
        }
        catch (EmptyListException)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "noPeopleInYourSearchCriteria";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("ForemanFilterByName")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ForemanFilterByName(
        [FromBody] PersonFilterByName cabPersonFilterbyName)
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                Name = cabPersonFilterbyName.FullName,
                TenantProvider = ItenantProvider,
                ProjectSqCode = Request.Headers["Project"].FirstOrDefault()
            };

            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;

            return Ok(new ApiOkResponse(await _iPersonRepository.ForemanFilterByName(_personRepositoryParameter)));
        }
        catch (EmptyListException)
        {
            ApiResponse.StatusCode = 200;
            ApiResponse.Status = false;
            ApiResponse.Message = "NoPeopleInYourSearchCriteria";
            return Ok(ApiResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("ReadBusinessEmail/{personId}")]
    public async Task<ActionResult> ReadBusinessEmail(string personId)
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                PersonId = personId,
                CompanyRepository = _iCompanyRepository,
                TenantProvider = ItenantProvider
            };

            var email = await _iPersonRepository.GetEmailByPersonId(_personRepositoryParameter);
            return Ok(new ApiOkResponse(email));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreateCabCompetencies")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateCabCompetencies(
        [FromBody] CabCompetencies cabCompetenciesDto)
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                CabCompetenciesDto = cabCompetenciesDto,
                TenantProvider = ItenantProvider
            };

            var result = await _iPersonRepository.CreateCabCompetencies(_personRepositoryParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetCabCompetencies/{personId}")]
    public async Task<ActionResult> GetCabCompetencies(string personId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                PersonId = personId,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPersonRepository.GetCabCompetencies(_personRepositoryParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteCabCompetencies")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCabCompetencies([FromBody] List<string> idList)
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                TenantProvider = ItenantProvider,
                IdListForDelete = idList
            };

            await _iPersonRepository.DeleteCabCompetencies(_personRepositoryParameter);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreateCabCertification")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateCabCertification(
        [FromBody] CabCertification cabCertificationDto)
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                CabCertificationDto = cabCertificationDto,
                TenantProvider = ItenantProvider
            };

            var result = await _iPersonRepository.CreateCabCertification(_personRepositoryParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetCabCertification/{personId}")]
    public async Task<ActionResult> GetCabCertification(string personId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                PersonId = personId,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPersonRepository.GetCabCertification(_personRepositoryParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("GetCabCertificationCiaw")]
    public async Task<ActionResult> GetCabCertificationCiaw([FromBody] GetCabCertificationDto GetCabCertificationDto)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                GetCabCertificationDto = GetCabCertificationDto,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            return Ok(new ApiOkResponse(await _iPersonRepository.GetCabCertificationCiaw(_personRepositoryParameter)));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpDelete("DeleteCabCertification")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCabCertification([FromBody] List<string> idList)
    {
        try
        {
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                TenantProvider = ItenantProvider,
                IdListForDelete = idList
            };

            await _iPersonRepository.DeleteCabCertification(_personRepositoryParameter);
            return Ok(ApiOkResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("GetContractorTaxonomyList")]
    public async Task<ActionResult> GetContractorTaxonomyList(string personId)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                Lang = lang,
                TenantProvider = ItenantProvider
            };

            var result = await _iPersonRepository.GetContractorTaxonomyList(_personRepositoryParameter);
            return Ok(new ApiOkResponse(result, "Ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpPost("CreateContractorTaxonomy")]
    public async Task<ActionResult> CreateContractorTaxonomy(
        [FromBody] ContractTaxonomyDto contractTaxonomyDto,
        [FromHeader(Name = "CU")] string cu, [FromHeader(Name = "Project")] string project,
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
            var _personRepositoryParameter = new PersonRepositoryParameter
            {
                ApplicationDbContext = UPrinceCustomerContext,
                Lang = lang,
                ContractTaxonomyDto = contractTaxonomyDto,
                TenantProvider = ItenantProvider
            };

            var s = await _iPersonRepository.CreateContractorTaxonomy(_personRepositoryParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}