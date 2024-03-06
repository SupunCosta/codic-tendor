using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Contract;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.CAB;

public class PersonRepository : IPersonRepository
{
    public async Task<IEnumerable<CabDataDto>> GetPersonList(PersonRepositoryParameter personRepositoryParameter)
    {
        //string sql = "SELECT * FROM CabEntyExport";
        //CREATE VIEW CabEntyExport AS 
        var sql =
            "SELECT   CabPerson.Id AS PersonId  ,CabPerson.FullName AS FullName  ,CabPersonCompany.JobRole AS JobTitle" +
            "  ,CabPerson.IsSaved AS IsSaved  ,CabEmail.EmailAddress AS Email  ,CabMobilePhone.MobilePhoneNumber AS Mobile   ,CabCompany.Name AS Organisation " +
            "  ,CabCompany.Id AS CompanyId    ,CabPersonCompany.Id AS PersonCompanyId  FROM dbo.CabPerson LEFT OUTER JOIN dbo.CabPersonCompany" +
            "   ON CabPerson.Id = CabPersonCompany.PersonId LEFT OUTER JOIN dbo.CabCompany   ON CabCompany.Id = CabPersonCompany.CompanyId" +
            " LEFT OUTER JOIN dbo.CabEmail   ON CabPersonCompany.EmailId = CabEmail.Id   LEFT OUTER JOIN dbo.CabMobilePhone" +
            "   ON CabPersonCompany.MobilePhoneId = CabMobilePhone.Id WHERE CabPerson.IsDeleted = 0";
        using IDbConnection dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
        var result = dbConnection.Query<CabDataDapperDto>(sql).ToList();
        

        var cabDataDtos = new List<CabDataDto>();
        foreach (var dto in result)
        {
            var cabDataDto = new CabDataDto
            {
                IsSaved = dto.IsSaved,
                CompanyId = dto.CompanyId
            };

            var personDto = new PersonDto
            {
                Id = dto.PersonId,
                FullName = dto.FullName
            };

            cabDataDto.Person = personDto;

            var personCompanyDto = new PersonCompanyDto
            {
                Id = dto.PersonCompanyId,
                JobRole = dto.JobTitle,
                Email = dto.Email,
                MobilePhone = dto.MobileNumber
            };

            cabDataDto.PersonCompany = personCompanyDto;

            var companyDto = new CompanyDto
            {
                Name = dto.Organisation,
                Id = dto.CompanyId
            };

            cabDataDto.Company = companyDto;
            cabDataDtos.Add(cabDataDto);
        }

        return cabDataDtos;
    }

    public async Task<IEnumerable<CabDataDto>> GetPersonListByName(
        PersonRepositoryParameter personRepositoryParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);
        var personList = applicationDbContext.CabPerson.Where(p =>
                p.FullName.ToLower().Contains(personRepositoryParameter.Name.ToLower()) &&
                p.IsDeleted == false && p.IsSaved == true)
            //.Include(p => p.LandPhone).Include(p => p.Email)
            //.Include(p => p.MobilePhone).Include(p => p.Skype).
            //Include(p => p.WhatsApp)
            //.Include(p => p.BusinessPhone)
            //.Include(p => p.BusinessEmail)
            //.Include(p => p.Address).Include(p => p.Address.Country)
            .Include(p => p.PersonCompanyList).ThenInclude(l => l.Company)
            .Include(p => p.PersonCompanyList).ThenInclude(l => l.Email)
            //.Include(p => p.PersonCompanyList).ThenInclude(l => l.MobilePhone)
            //.Include(p => p.PersonCompanyList).ThenInclude(l => l.LandPhone)
            //.Include(p => p.PersonCompanyList).ThenInclude(l => l.WhatsApp)
            //.Include(p => p.PersonCompanyList).ThenInclude(l => l.Skype)
            .ToList();

        var dtoList = await CreateDtoList(personList, personRepositoryParameter);
        return dtoList;
    }

    public async Task<CabDataDto> GetPersonById(PersonRepositoryParameter personRepositoryParameter)
    {
        using IDbConnection dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);
        var person = applicationDbContext.CabPerson
            .Where(p => p.Id == personRepositoryParameter.PersonId && p.IsDeleted == false)
            .Include(p => p.LandPhone).Include(p => p.Email)
            .Include(p => p.MobilePhone).Include(p => p.Skype).Include(p => p.WhatsApp)
            .Include(p => p.Address).Include(p => p.Address.Country)
            .Include(p => p.BusinessPhone)
            .Include(p => p.BusinessEmail)
            .Include(p => p.CompanyList)
            .Include(p => p.PersonCompanyList).ThenInclude(l => l.Email)
            .Include(p => p.PersonCompanyList).ThenInclude(l => l.MobilePhone)
            .Include(p => p.PersonCompanyList).ThenInclude(l => l.LandPhone)
            .Include(p => p.PersonCompanyList).ThenInclude(l => l.WhatsApp)
            .Include(p => p.PersonCompanyList).ThenInclude(l => l.Skype)
            .ToList().FirstOrDefault();


        var cabDataDto = new CabDataDto();
        if (person != null)
        {
            cabDataDto.IsSaved = person.IsSaved;

            // if (personRepositoryParameter.TenantProvider.GetTenant().ConnectionString.Contains("bmengineering"))
            // {
            //     person = applicationDbContext.CabPerson
            // .Where(p => p.Id == personRepositoryParameter.PersonId && p.IsDeleted == false).Include(p => p.ContractorTaxonomyId).ToList().FirstOrDefault();
            // }

            var personDto = new PersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                Surname = person.Surname,
                FullName = person.FullName,
                CallName = person.CallName,
                Image = person.Image
            };

            if (person.BusinessEmail != null)
                personDto.BusinessEmail = person.BusinessEmail.EmailAddress;
            if (person.BusinessPhone != null)
                personDto.BusinessPhone = person.BusinessPhone.MobilePhoneNumber;
            if (person.Email != null)
                personDto.Email = person.Email.EmailAddress;
            if (person.LandPhone != null)
                personDto.LandPhone = person.LandPhone.LandPhoneNumber;
            if (person.MobilePhone != null)
                personDto.MobilePhone = person.MobilePhone.MobilePhoneNumber;
            if (person.WhatsApp != null)
                personDto.Whatsapp = person.WhatsApp.WhatsAppNumber;
            if (person.Skype != null)
                personDto.Skype = person.Skype.SkypeNumber;
            if (person.Address != null)
            {
                var addressDto = new AddressDto
                {
                    Id = person.Address.Id,
                    MailBox = person.Address.MailBox,
                    PostalCode = person.Address.PostalCode,
                    Street = person.Address.Street,
                    StreetNumber = person.Address.StreetNumber,
                    City = person.Address.City,
                    Region = person.Address.Region,
                    CountryId = person.Address.CountryId
                };
                personDto.Address = addressDto;
            }

            var sql =
                @"SELECT Nationality.NationalityId AS [Key] ,Nationality.Name AS [Text] FROM dbo.CabNationality LEFT OUTER JOIN dbo.Nationality ON CabNationality.NationalityId = Nationality.NationalityId WHERE CabPersonId = @CabPersonId";

            personDto.Nationality =
                dbConnection.Query<NationalityDto>(sql, new { CabPersonId = person.Id }).FirstOrDefault();
            cabDataDto.Person = personDto;

            if (person.PersonCompanyList != null && person.PersonCompanyList.Any())
            {
                var personCompany = person.PersonCompanyList.First();
                var personCompanyDto = new PersonCompanyDto
                {
                    Id = personCompany.Id,
                    JobRole = personCompany.JobRole
                };

                if (personCompany.Email != null)
                    personCompanyDto.Email = personCompany.Email.EmailAddress;
                if (personCompany.MobilePhone != null)
                    personCompanyDto.MobilePhone = personCompany.MobilePhone.MobilePhoneNumber;
                if (personCompany.LandPhone != null)
                    personCompanyDto.LandPhone = personCompany.LandPhone.LandPhoneNumber;
                if (personCompany.WhatsApp != null)
                    personCompanyDto.Whatsapp = personCompany.WhatsApp.WhatsAppNumber;
                if (personCompany.Skype != null)
                    personCompanyDto.Skype = personCompany.Skype.SkypeNumber;

                personCompanyDto.PersonId = personCompany.PersonId;
                personCompanyDto.CompanyId = personCompany.CompanyId;

                cabDataDto.PersonCompany = personCompanyDto;
            }

            var companyRepositoryParameter = new CompanyRepositoryParameter();
            var optionsx = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContextX =
                new ApplicationDbContext(optionsx, personRepositoryParameter.TenantProvider);
            companyRepositoryParameter.ApplicationDbContext = applicationDbContextX;
            if (cabDataDto.PersonCompany != null)
            {
                companyRepositoryParameter.CompanyId = cabDataDto.PersonCompany.CompanyId;
                companyRepositoryParameter.TenantProvider = personRepositoryParameter.TenantProvider;
                //companyRepositoryParameter.CompanyId = 
                cabDataDto.Company = await
                    personRepositoryParameter.CompanyRepository.GetCompanyById(companyRepositoryParameter);
                if (cabDataDto.Company != null) cabDataDto.CompanyId = cabDataDto.Company.Id;
            }

            var optionsy = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContextY =
                new ApplicationDbContext(optionsy, personRepositoryParameter.TenantProvider);
            var historyDetails = applicationDbContextY.CabHistoryLog
                .Where(p => p.PersonId == person.Id).OrderByDescending(p => p.ChangedTime)
                .Include(h => h.ChangedByUser);
            var lastState = historyDetails.FirstOrDefault();
            var firstState = historyDetails.LastOrDefault();
            if (firstState != null && lastState != null)
            {
                var cabHistoryLogDto = new CabHistoryLogDto
                {
                    CreatedByUser =
                        firstState.ChangedByUser?.FirstName + " " + firstState.ChangedByUser?.LastName,
                    CreatedDateTime = firstState.ChangedTime,
                    RevisionNumber = firstState.RevisionNumber,
                    UpdatedDateTime = null
                };
                cabDataDto.History = cabHistoryLogDto;
                if (historyDetails.Count() <= 1) return cabDataDto;
                cabHistoryLogDto.UpdatedByUser =
                    lastState.ChangedByUser?.FirstName + " " + lastState.ChangedByUser?.LastName;
                cabHistoryLogDto.UpdatedDateTime = lastState.ChangedTime;
                cabHistoryLogDto.RevisionNumber = lastState.RevisionNumber;
                cabDataDto.History = cabHistoryLogDto;
            }
        }

        return cabDataDto;
    }

    public async Task<string> AddPerson(PersonRepositoryParameter personRepositoryParameter)
    {
        try
        {

        
        using IDbConnection dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

        var options = new DbContextOptions<ApplicationDbContext>();
        var dbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);

        var person = personRepositoryParameter.Person;
        if (person.Id == null)
        {
            person.Id = Guid.NewGuid().ToString();
            if (person.Address != null)
            {
                var address = new CabAddress
                {
                    Id = Guid.NewGuid().ToString(),
                    City = person.Address.City,
                    CountryId = person.Address.CountryId,
                    Region = person.Address.Region,
                    Street = person.Address.Street,
                    StreetNumber = person.Address.StreetNumber,
                    MailBox = person.Address.MailBox,
                    PostalCode = person.Address.PostalCode
                };
                person.AddressId = address.Id;
                person.Address = address;
                var options1 = new DbContextOptions<ApplicationDbContext>();
                var dbContext1 =
                    new ApplicationDbContext(options1, personRepositoryParameter.TenantProvider);
                dbContext.CabAddress.Add(address);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                person.Address = null;
            }


            if (person.Email.EmailAddress != null)
            {
                var existingEmail =
                    dbContext.CabEmail.FirstOrDefault(e =>
                        e.EmailAddress == person.Email.EmailAddress && e.IsDeleted == true);
                if (existingEmail != null)
                {
                    person.EmailId = existingEmail.Id;
                    person.Email = existingEmail;
                    existingEmail.IsDeleted = false;
                    var options2 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext2 =
                        new ApplicationDbContext(options2, personRepositoryParameter.TenantProvider);
                    dbContext2.CabEmail.Update(existingEmail);
                    await dbContext2.SaveChangesAsync();
                }
                else
                {
                    var email = new CabEmail
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmailAddress = person.Email.EmailAddress
                    };
                    person.EmailId = email.Id;
                    person.Email = email;
                    var options3 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext3 =
                        new ApplicationDbContext(options3, personRepositoryParameter.TenantProvider);
                    dbContext3.CabEmail.Add(email);
                    await dbContext3.SaveChangesAsync();
                }
            }
            else
            {
                person.Email = null;
            }


            if (person.LandPhone.LandPhoneNumber != null)
            {
                var existingLangPhone =
                    dbContext.CabLandPhone.FirstOrDefault(
                        l => l.LandPhoneNumber == person.LandPhone.LandPhoneNumber && l.IsDeleted == true);
                if (existingLangPhone != null)
                {
                    person.LandPhoneId = existingLangPhone.Id;
                    person.LandPhone = existingLangPhone;
                    existingLangPhone.IsDeleted = false;
                    var options4 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext4 =
                        new ApplicationDbContext(options4, personRepositoryParameter.TenantProvider);
                    dbContext4.CabLandPhone.Update(existingLangPhone);
                    await dbContext4.SaveChangesAsync();
                }
                else
                {
                    var landPhone = new CabLandPhone
                    {
                        Id = Guid.NewGuid().ToString(),
                        LandPhoneNumber = person.LandPhone.LandPhoneNumber
                    };
                    person.LandPhoneId = landPhone.Id;
                    person.LandPhone = landPhone;
                    var options5 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext5 =
                        new ApplicationDbContext(options5, personRepositoryParameter.TenantProvider);
                    dbContext5.CabLandPhone.Add(landPhone);
                    await dbContext5.SaveChangesAsync();
                }
            }
            else
            {
                person.LandPhone = null;
            }


            if (person.MobilePhone.MobilePhoneNumber != null)
            {
                var existingMobilePhone = dbContext.CabMobilePhone.FirstOrDefault(m =>
                    m.MobilePhoneNumber == person.MobilePhone.MobilePhoneNumber &&
                    m.IsDeleted == true);
                if (existingMobilePhone != null)
                {
                    person.MobilePhoneId = existingMobilePhone.Id;
                    person.MobilePhone = existingMobilePhone;
                    existingMobilePhone.IsDeleted = false;
                    var options6 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext6 =
                        new ApplicationDbContext(options6, personRepositoryParameter.TenantProvider);
                    dbContext6.CabMobilePhone.Update(existingMobilePhone);
                    await dbContext6.SaveChangesAsync();
                }
                else
                {
                    var mobilePhone = new CabMobilePhone
                    {
                        Id = Guid.NewGuid().ToString(),
                        MobilePhoneNumber = person.MobilePhone.MobilePhoneNumber
                    };
                    person.MobilePhoneId = mobilePhone.Id;
                    person.MobilePhone = mobilePhone;
                    var options7 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext7 =
                        new ApplicationDbContext(options7, personRepositoryParameter.TenantProvider);
                    dbContext7.CabMobilePhone.Add(mobilePhone);
                    await dbContext7.SaveChangesAsync();
                }
            }
            else
            {
                person.MobilePhone = null;
            }


            if (person.WhatsApp.WhatsAppNumber != null)
            {
                var existingWhatsApp = dbContext.CabWhatsApp.FirstOrDefault(w =>
                    w.WhatsAppNumber == person.WhatsApp.WhatsAppNumber &&
                    w.IsDeleted == true);
                if (existingWhatsApp != null)
                {
                    person.WhatsAppId = existingWhatsApp.Id;
                    person.WhatsApp = existingWhatsApp;
                    existingWhatsApp.IsDeleted = false;
                    var options8 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext8 =
                        new ApplicationDbContext(options8, personRepositoryParameter.TenantProvider);
                    dbContext8.CabWhatsApp.Update(existingWhatsApp);
                    await dbContext8.SaveChangesAsync();
                }
                else
                {
                    var whatsapp = new CabWhatsApp
                    {
                        Id = Guid.NewGuid().ToString(),
                        WhatsAppNumber = person.WhatsApp.WhatsAppNumber
                    };
                    person.WhatsAppId = whatsapp.Id;
                    person.WhatsApp = whatsapp;
                    var options9 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext9 =
                        new ApplicationDbContext(options9, personRepositoryParameter.TenantProvider);
                    dbContext9.CabWhatsApp.Add(whatsapp);
                    await dbContext9.SaveChangesAsync();
                }
            }
            else
            {
                person.WhatsApp = null;
            }


            if (person.Skype.SkypeNumber != null)
            {
                var existingSkype = dbContext.CabSkype.FirstOrDefault(s =>
                    s.SkypeNumber == person.Skype.SkypeNumber && s.IsDeleted == true);
                if (existingSkype != null)
                {
                    person.SkypeId = existingSkype.Id;
                    person.Skype = existingSkype;
                    existingSkype.IsDeleted = false;
                    var options11 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext11 =
                        new ApplicationDbContext(options11, personRepositoryParameter.TenantProvider);
                    dbContext11.CabSkype.Update(existingSkype);
                    await dbContext11.SaveChangesAsync();
                }
                else
                {
                    var skype = new CabSkype
                    {
                        Id = Guid.NewGuid().ToString(),
                        SkypeNumber = person.Skype.SkypeNumber
                    };
                    person.SkypeId = skype.Id;
                    person.Skype = skype;
                    var options22 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext22 =
                        new ApplicationDbContext(options22, personRepositoryParameter.TenantProvider);
                    dbContext22.CabSkype.Add(skype);
                    await dbContext22.SaveChangesAsync();
                }
            }
            else
            {
                person.Skype = null;
            }


            if (person.BusinessPhone.MobilePhoneNumber != null)
            {
                var existingBusinessPhone = dbContext.CabMobilePhone.FirstOrDefault(s =>
                    s.MobilePhoneNumber == person.BusinessPhone.MobilePhoneNumber && s.IsDeleted == true);
                if (existingBusinessPhone != null)
                {
                    person.BusinessPhoneId = existingBusinessPhone.Id;
                    person.BusinessPhone = existingBusinessPhone;
                    existingBusinessPhone.IsDeleted = false;
                    var options33 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext33 =
                        new ApplicationDbContext(options33, personRepositoryParameter.TenantProvider);
                    dbContext33.CabMobilePhone.Update(existingBusinessPhone);
                    await dbContext33.SaveChangesAsync();
                }
                else
                {
                    var businessPhone = new CabMobilePhone
                    {
                        Id = Guid.NewGuid().ToString(),
                        MobilePhoneNumber = person.BusinessPhone.MobilePhoneNumber
                    };
                    person.BusinessPhoneId = businessPhone.Id;
                    person.BusinessPhone = businessPhone;
                    var options44 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext44 =
                        new ApplicationDbContext(options44, personRepositoryParameter.TenantProvider);
                    dbContext44.CabMobilePhone.Add(businessPhone);
                    await dbContext44.SaveChangesAsync();
                }
            }
            else
            {
                person.BusinessPhone = null;
            }

            if (person.BusinessEmail.EmailAddress != null)
            {
                var existingBusinessEmail = dbContext.CabEmail.FirstOrDefault(s =>
                    s.EmailAddress == person.BusinessEmail.EmailAddress && s.IsDeleted == true);
                if (existingBusinessEmail != null)
                {
                    person.BusinessEmailId = existingBusinessEmail.Id;
                    person.BusinessEmail = existingBusinessEmail;
                    existingBusinessEmail.IsDeleted = false;
                    var options55 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext55 =
                        new ApplicationDbContext(options55, personRepositoryParameter.TenantProvider);
                    dbContext55.CabEmail.Update(existingBusinessEmail);
                    await dbContext55.SaveChangesAsync();
                }
                else
                {
                    var businessEmail = new CabEmail
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmailAddress = person.BusinessEmail.EmailAddress
                    };
                    person.BusinessEmailId = businessEmail.Id;
                    person.BusinessEmail = businessEmail;
                    var options66 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext66 =
                        new ApplicationDbContext(options66, personRepositoryParameter.TenantProvider);
                    dbContext66.CabEmail.Add(businessEmail);
                    await dbContext66.SaveChangesAsync();
                }
            }
            else
            {
                person.BusinessEmail = null;
            }

            var options77 = new DbContextOptions<ApplicationDbContext>();
            var dbContext77 =
                new ApplicationDbContext(options77, personRepositoryParameter.TenantProvider);

            // person.AddressId = null;
            var query =
                "INSERT INTO dbo.CabPerson ( Id ,IsDeleted ,IsSaved ,FirstName ,Surname ,FullName ,CallName ,BusinessEmailId ,BusinessPhoneId ,Image ,AddressId ,EmailId ,LandPhoneId ,MobilePhoneId ,WhatsAppId ,SkypeId  )VALUES ( @Id ,@IsDeleted ,@IsSaved ,@FirstName ,@Surname ,@FullName ,@CallName ,@BusinessEmailId ,@BusinessPhoneId ,@Image ,@AddressId ,@EmailId ,@LandPhoneId ,@MobilePhoneId ,@WhatsAppId ,@SkypeId )";

            var param = new
            {
                person.Id,
                person.IsDeleted,
                person.IsSaved,
                person.FirstName,
                person.Surname,
                person.FullName,
                person.CallName,
                person.BusinessEmailId,
                person.BusinessPhoneId,
                person.Image,
                person.AddressId,
                person.EmailId,
                person.LandPhoneId,
                person.MobilePhoneId,
                person.WhatsAppId,
                person.SkypeId
            };

            await dbConnection.ExecuteAsync(query, param);

            // dbContext77.CabPerson.Add(person);
            //  dbContext77.SaveChanges();

            var history = personRepositoryParameter.CabHistoryLogRepositoryParameter;
            history.Action = HistoryState.ADDED.ToString();
            history.Person.Id = person.Id;
            history.TenantProvider = personRepositoryParameter.TenantProvider;
            var historyRepository = personRepositoryParameter.CabHistoryLogRepository.AddCabHistoryLog(history);

            await AddNationlity(personRepositoryParameter, false);

            return personRepositoryParameter.Person.Id;
        }

        var updatedPerson = dbContext.CabPerson.FirstOrDefault(c =>
            c.Id == person.Id);

        if (updatedPerson != null)
        {
            updatedPerson.FirstName = person.FirstName;
            updatedPerson.Surname = person.Surname;
            updatedPerson.FullName = person.FullName;
            updatedPerson.CallName = person.CallName;
            updatedPerson.Image = person.Image;
            updatedPerson.IsSaved = person.IsSaved;


            if (person.Address != null)
            {
                var updatedAddress =
                    dbContext.CabAddress.FirstOrDefault(a => a.Id == updatedPerson.AddressId);
                if (updatedAddress == null)
                {
                    var address = new CabAddress
                    {
                        Id = Guid.NewGuid().ToString(),
                        City = person.Address.City,
                        CountryId = person.Address.CountryId,
                        Region = person.Address.Region,
                        Street = person.Address.Street,
                        StreetNumber = person.Address.StreetNumber,
                        MailBox = person.Address.MailBox,
                        PostalCode = person.Address.PostalCode
                    };
                    updatedPerson.AddressId = address.Id;
                    updatedPerson.Address = address;
                    var options88 =
                        new DbContextOptions<ApplicationDbContext>();
                    var dbContext88 =
                        new ApplicationDbContext(options88, personRepositoryParameter.TenantProvider);
                }
                else
                {
                    updatedAddress.City = person.Address.City;
                    updatedAddress.CountryId = person.Address.CountryId;
                    updatedAddress.Region = person.Address.Region;
                    updatedAddress.Street = person.Address.Street;
                    updatedAddress.StreetNumber = person.Address.StreetNumber;
                    updatedAddress.MailBox = person.Address.MailBox;
                    updatedAddress.PostalCode = person.Address.PostalCode;
                    updatedPerson.AddressId = updatedAddress.Id;
                    updatedPerson.Address = updatedAddress;
                    var options99 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext99 =
                        new ApplicationDbContext(options99, personRepositoryParameter.TenantProvider);
                    applicationDbContext99.Update(updatedAddress);
                    await applicationDbContext99.SaveChangesAsync();
                }
            }

            if (person.Email.EmailAddress != null)
            {
                var updatedEmail = dbContext.CabEmail.FirstOrDefault(a => a.Id == updatedPerson.EmailId);
                if (updatedEmail == null)
                {
                    var email = new CabEmail
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmailAddress = person.Email.EmailAddress
                    };

                    updatedPerson.EmailId = email.Id;
                    updatedPerson.Email = email;
                    var options111 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext111 =
                        new ApplicationDbContext(options111, personRepositoryParameter.TenantProvider);
                    dbContext.CabEmail.Add(email);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    updatedEmail.EmailAddress = person.Email.EmailAddress;
                    var options222 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext222 =
                        new ApplicationDbContext(options222, personRepositoryParameter.TenantProvider);
                    dbContext.CabEmail.Update(updatedEmail);
                    await dbContext.SaveChangesAsync();
                }
            }
            else
            {
                var email = dbContext.CabEmail.FirstOrDefault(e => e.Id == updatedPerson.EmailId);
                if (email != null)
                {
                    email.IsDeleted = true;
                    var options333 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext333 =
                        new ApplicationDbContext(options333, personRepositoryParameter.TenantProvider);
                    dbContext.CabEmail.Update(email);
                    await dbContext.SaveChangesAsync();
                    updatedPerson.Email = null;
                    updatedPerson.EmailId = null;
                }
                // }
            }

            if (person.LandPhone.LandPhoneNumber != null)
            {
                var updatedLandPhone =
                    dbContext.CabLandPhone.FirstOrDefault(a => a.Id == updatedPerson.LandPhoneId);
                if (updatedLandPhone == null)
                {
                    var landPhone = new CabLandPhone
                    {
                        Id = Guid.NewGuid().ToString(),
                        LandPhoneNumber = person.LandPhone.LandPhoneNumber
                    };

                    updatedPerson.LandPhoneId = landPhone.Id;
                    updatedPerson.LandPhone = landPhone;
                    var options444 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext444 =
                        new ApplicationDbContext(options444, personRepositoryParameter.TenantProvider);
                    dbContext.CabLandPhone.Add(landPhone);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    updatedLandPhone.LandPhoneNumber = person.LandPhone.LandPhoneNumber;
                    var options441 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext441 =
                        new ApplicationDbContext(options441, personRepositoryParameter.TenantProvider);
                    dbContext.CabLandPhone.Update(updatedLandPhone);
                    await dbContext.SaveChangesAsync();
                }
            }
            else
            {
                var landPhone =
                    dbContext.CabLandPhone.FirstOrDefault(e => e.Id == updatedPerson.LandPhoneId);
                if (landPhone != null)
                {
                    landPhone.IsDeleted = true;
                    var options442 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext442 =
                        new ApplicationDbContext(options442, personRepositoryParameter.TenantProvider);
                    dbContext.CabLandPhone.Update(landPhone);
                    await dbContext.SaveChangesAsync();
                    updatedPerson.LandPhone = null;
                    updatedPerson.LandPhoneId = null;
                }
            }

            if (person.MobilePhone.MobilePhoneNumber != null)
            {
                var updatedMobilePhone =
                    dbContext.CabMobilePhone.FirstOrDefault(a => a.Id == updatedPerson.MobilePhoneId);
                if (updatedMobilePhone == null)
                {
                    var mobilePhone = new CabMobilePhone
                    {
                        Id = Guid.NewGuid().ToString(),
                        MobilePhoneNumber = person.MobilePhone.MobilePhoneNumber
                    };

                    updatedPerson.MobilePhoneId = mobilePhone.Id;
                    updatedPerson.MobilePhone = mobilePhone;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabMobilePhone.Add(mobilePhone);
                    await applicationDbContext443.SaveChangesAsync();
                }
                else
                {
                    updatedMobilePhone.MobilePhoneNumber = person.MobilePhone.MobilePhoneNumber;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabMobilePhone.Update(updatedMobilePhone);
                    await applicationDbContext443.SaveChangesAsync();
                }
            }
            else
            {
                var mobilePhone =
                    dbContext.CabMobilePhone.FirstOrDefault(e => e.Id == updatedPerson.MobilePhoneId);
                if (mobilePhone != null)
                {
                    mobilePhone.IsDeleted = true;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabMobilePhone.Update(mobilePhone);
                    await applicationDbContext443.SaveChangesAsync();
                    updatedPerson.MobilePhone = null;
                    updatedPerson.MobilePhoneId = null;
                }
            }

            if (person.WhatsApp.WhatsAppNumber != null)
            {
                var updatedWhatsApp =
                    dbContext.CabWhatsApp.FirstOrDefault(a => a.Id == updatedPerson.WhatsAppId);
                if (updatedWhatsApp == null)
                {
                    var whatsApp = new CabWhatsApp
                    {
                        Id = Guid.NewGuid().ToString(),
                        WhatsAppNumber = person.WhatsApp.WhatsAppNumber
                    };

                    updatedPerson.WhatsAppId = whatsApp.Id;
                    updatedPerson.WhatsApp = whatsApp;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabWhatsApp.Add(whatsApp);
                    await applicationDbContext443.SaveChangesAsync();
                }
                else
                {
                    updatedWhatsApp.WhatsAppNumber = person.WhatsApp.WhatsAppNumber;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabWhatsApp.Update(updatedWhatsApp);
                    await applicationDbContext443.SaveChangesAsync();
                }
            }
            else
            {
                var whatsApp = dbContext.CabWhatsApp.FirstOrDefault(e => e.Id == updatedPerson.WhatsAppId);
                if (whatsApp != null)
                {
                    whatsApp.IsDeleted = true;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabWhatsApp.Update(whatsApp);
                    await applicationDbContext443.SaveChangesAsync();
                    updatedPerson.WhatsApp = null;
                    updatedPerson.WhatsAppId = null;
                }
            }

            if (person.Skype.SkypeNumber != null)
            {
                var updatedSkype = dbContext.CabSkype.FirstOrDefault(a => a.Id == updatedPerson.SkypeId);
                if (updatedSkype == null)
                {
                    var skype = new CabSkype
                    {
                        Id = Guid.NewGuid().ToString(),
                        SkypeNumber = person.Skype.SkypeNumber
                    };

                    updatedPerson.SkypeId = skype.Id;
                    updatedPerson.Skype = skype;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabSkype.Add(skype);
                    await applicationDbContext443.SaveChangesAsync();
                }
                else
                {
                    updatedSkype.SkypeNumber = person.Skype.SkypeNumber;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabSkype.Update(updatedSkype);
                    await applicationDbContext443.SaveChangesAsync();
                }
            }
            else
            {
                var skype = dbContext.CabSkype.FirstOrDefault(e => e.Id == updatedPerson.SkypeId);
                if (skype != null)
                {
                    skype.IsDeleted = true;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabSkype.Update(skype);
                    await applicationDbContext443.SaveChangesAsync();
                    updatedPerson.SkypeId = null;
                    updatedPerson.Skype = null;
                }
            }

            if (person.BusinessEmail.EmailAddress != null)
            {
                var updatedBusinessEmail =
                    dbContext.CabEmail.FirstOrDefault(a => a.Id == updatedPerson.BusinessEmailId);
                if (updatedBusinessEmail == null)
                {
                    var bMail = new CabEmail
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmailAddress = person.BusinessEmail.EmailAddress
                    };

                    updatedPerson.BusinessEmailId = bMail.Id;
                    updatedPerson.BusinessEmail = bMail;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabEmail.Add(bMail);
                    await applicationDbContext443.SaveChangesAsync();
                }
                else
                {
                    updatedBusinessEmail.EmailAddress = person.BusinessEmail.EmailAddress;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabEmail.Update(updatedBusinessEmail);
                    await applicationDbContext443.SaveChangesAsync();
                }
            }
            else
            {
                var businessEmail =
                    dbContext.CabEmail.FirstOrDefault(e => e.Id == updatedPerson.BusinessEmailId);
                if (businessEmail != null)
                {
                    businessEmail.IsDeleted = true;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabEmail.Update(businessEmail);
                    await applicationDbContext443.SaveChangesAsync();
                    updatedPerson.BusinessEmailId = null;
                    updatedPerson.BusinessEmail = null;
                }
            }

            if (person.BusinessPhone.MobilePhoneNumber != null)
            {
                var updatedBusinessPhone =
                    dbContext.CabMobilePhone.FirstOrDefault(a => a.Id == updatedPerson.BusinessPhoneId);
                if (updatedBusinessPhone == null)
                {
                    var bPhone = new CabMobilePhone
                    {
                        Id = Guid.NewGuid().ToString(),
                        MobilePhoneNumber = person.BusinessEmail.EmailAddress
                    };

                    updatedPerson.BusinessPhoneId = bPhone.Id;
                    updatedPerson.BusinessPhone = bPhone;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.Add(bPhone);
                    await applicationDbContext443.SaveChangesAsync();
                }
                else
                {
                    updatedBusinessPhone.MobilePhoneNumber = person.BusinessPhone.MobilePhoneNumber;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabMobilePhone.Update(updatedBusinessPhone);
                    await applicationDbContext443.SaveChangesAsync();
                }
            }
            else
            {
                var businessPhone =
                    dbContext.CabMobilePhone.FirstOrDefault(e => e.Id == updatedPerson.BusinessPhoneId);
                if (businessPhone != null)
                {
                    businessPhone.IsDeleted = true;
                    var options443 =
                        new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext443 =
                        new ApplicationDbContext(options443, personRepositoryParameter.TenantProvider);
                    applicationDbContext443.CabMobilePhone.Update(businessPhone);
                    await applicationDbContext443.SaveChangesAsync();
                    updatedPerson.BusinessPhone = null;
                    updatedPerson.BusinessPhoneId = null;
                }
            }

            var options4431 =
                new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext4431 =
                new ApplicationDbContext(options4431, personRepositoryParameter.TenantProvider);
            dbContext.CabPerson.Update(updatedPerson);
            await dbContext.SaveChangesAsync();

            await AddNationlity(personRepositoryParameter, true);

            return updatedPerson.Id;
        }

        return "Cab Person Id is invalid";
        
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    public async Task<bool> DeletePerson(PersonRepositoryParameter personRepositoryParameter)
    {
        var isUpdated = false;
        foreach (var person in personRepositoryParameter.IdListForDelete
                     .Select(id =>
                         personRepositoryParameter.ApplicationDbContext.CabPerson.FirstOrDefault(p => p.Id == id))
                     .Where(person => person != null))
        {
            person.IsDeleted = true;
            personRepositoryParameter.ApplicationDbContext.CabPerson.Update(person);
            await personRepositoryParameter.ApplicationDbContext.SaveChangesAsync();
            isUpdated = true;
        }

        return isUpdated;
    }

    public async Task<string> UploadImage(PersonRepositoryParameter personRepositoryParameter)
    {
        var client = new FileClient();
        var url = client.PersistPhoto(personRepositoryParameter.Image.Files.FirstOrDefault()?.FileName,
            personRepositoryParameter.TenantProvider, personRepositoryParameter.Image.Files.FirstOrDefault());
        return url;
    }

    public async Task<IEnumerable<ProjectPersonFilterDto>> Filter(PersonRepositoryParameter personRepositoryParameter)
    {
        //CREATE VIEW CabFilter AS 
        var sql = @"SELECT
        CabPerson.IsSaved AS IsSaved
            ,CabCompany.Id AS CompanyId
            ,CabPerson.Id AS Id
            ,CabPerson.FullName AS FullName
            ,CabPersonCompany.Id AS Id
            ,CabPersonCompany.JobRole AS JobTitle
            ,CabEmail.EmailAddress AS Email
            ,CabMobilePhone.MobilePhoneNumber AS MobileNumber
            ,CabCompany.Id AS Id
            ,CabCompany.Name AS Name
            ,CabCompany.Name AS organisation


            FROM dbo.CabPerson
            LEFT OUTER JOIN dbo.CabPersonCompany
            ON CabPerson.Id = CabPersonCompany.PersonId
        LEFT OUTER JOIN dbo.CabCompany
            ON CabCompany.Id = CabPersonCompany.CompanyId
        LEFT OUTER JOIN dbo.CabEmail
            ON CabPersonCompany.EmailId = CabEmail.Id
        LEFT OUTER JOIN dbo.CabMobilePhone
            ON CabPersonCompany.MobilePhoneId = CabMobilePhone.Id
        WHERE CabPerson.IsDeleted = 0 ";
        var sb = new StringBuilder(sql);
        var filter = personRepositoryParameter.CabPersonFilter;
        if (filter != null)
        {
            //if (filter.FullName != null) sb.Append(" AND CabPerson.FullName LIKE '%" + filter.FullName + "%'");

            if (filter.FullName != null)
            {
                filter.FullName = filter.FullName.Replace("'", "''");

                var words = filter.FullName.Split(" ");
                foreach (var element in words)
                    sb.Append(" AND CabPerson.FullName LIKE '%" + element + "%'");
                //sb.Append(" AND CorporateProductCatalog.Title  LIKE '%" + cpcParameters.filter.Title + "%'");
            }

            if (filter.MobileNumber != null)
            {
                filter.MobileNumber = filter.MobileNumber.Replace("'", "''");
                sb.Append(" AND CabMobilePhone.MobilePhoneNumber LIKE '%" + filter.MobileNumber + "%'");
            }


            if (filter.Organisation != null)
            {
                filter.Organisation = filter.Organisation.Replace("'", "''");
                sb.Append(" AND CabCompany.Name LIKE '%" + filter.Organisation + "%'");
            }

            if (filter.JobTitle != null)
            {
                filter.JobTitle = filter.JobTitle.Replace("'", "''");

                sb.Append(" AND CabPersonCompany.JobRole LIKE '%" + filter.JobTitle + "%'");

            }

            if (filter.Email != null)
            {
                filter.Email = filter.Email.Replace("'", "''");

                sb.Append(" AND CabEmail.EmailAddress LIKE '%" + filter.Email + "%'");
            }

            if (filter.IsSaved != null)
                switch (filter.IsSaved.ToLower())
                {
                    case "true":
                        sb.Append(" AND CabPerson.IsSaved = 1");
                        break;
                    case "false":
                        sb.Append(" AND CabPerson.IsSaved = 0");
                        break;
                }
        }

        //organisation mobileNumber
        if (filter != null && filter.CabPersonSortingModel != null)
        {
            if (filter.CabPersonSortingModel.Attribute != null &&
                filter.CabPersonSortingModel.Order.Equals("asc"))
                sb.Append(" ORDER BY " + filter.CabPersonSortingModel.Attribute + " ASC");

            if (filter.CabPersonSortingModel.Attribute != null &&
                filter.CabPersonSortingModel.Order.Equals("desc"))
                sb.Append(" ORDER BY " + filter.CabPersonSortingModel.Attribute + " DESC");
            
            if (filter.CabPersonSortingModel.Attribute == null)
                sb.Append(" ORDER BY CabPerson.FullName ASC");
        }
        

        using IDbConnection dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

        var result =
            await dbConnection
                .QueryAsync<ProjectPersonFilterDto, ProjectPersonFilterPersonDto, ProjectPersonFilterPersonCompanyDto,
                    ProjectPersonFilterCompanyDto, ProjectPersonFilterDto>(sb.ToString(),
                    (cabData, personData, personCompany, company) =>
                    {
                        cabData.Person = personData;
                        cabData.PersonCompany = personCompany;
                        cabData.Company = company;
                        return cabData;
                    }, new { SequenceCode = personRepositoryParameter.ProjectSqCode });

        

        return result;
    }

    public async Task<IEnumerable<ProjectPersonFilterDto>> ProjectPersonFilter(
        PersonRepositoryParameter personRepositoryParameter)
    {
        var sql = @"SELECT
  CabPerson.IsSaved AS IsSaved
 ,CabCompany.Id AS CompanyId
 ,CabPerson.Id AS Id
 ,CabPerson.FullName AS FullName
 ,CabPersonCompany.Id AS Id
 ,CabPersonCompany.JobRole AS JobTitle
 ,CabEmail.EmailAddress AS Email
 ,CabMobilePhone.MobilePhoneNumber AS MobileNumber
,CabCompany.Id AS Id
 ,CabCompany.Name AS Name

FROM dbo.CabPerson
LEFT OUTER JOIN dbo.CabPersonCompany
  ON CabPerson.Id = CabPersonCompany.PersonId
LEFT OUTER JOIN dbo.CabCompany
  ON CabCompany.Id = CabPersonCompany.CompanyId
LEFT OUTER JOIN dbo.CabEmail
  ON CabPersonCompany.EmailId = CabEmail.Id
LEFT OUTER JOIN dbo.CabMobilePhone
  ON CabPersonCompany.MobilePhoneId = CabMobilePhone.Id
WHERE CabPerson.IsDeleted = 0 " +
                  " AND CabPerson.Id NOT IN (SELECT ProjectManagerId FROM ProjectDefinition WHERE SequenceCode = @SequenceCode) AND CabPerson.Id NOT IN (SELECT CustomerId FROM ProjectDefinition WHERE SequenceCode = @SequenceCode)";
        var sb = new StringBuilder(sql);
        var filter = personRepositoryParameter.CabPersonFilter;
        if (filter != null)
        {
            //if (filter.FullName != null) sb.Append(" AND CabPerson.FullName LIKE '%" + filter.FullName + "%'");

            if (filter.FullName != null)
            {
                filter.FullName = filter.FullName.Replace("'", "''");

                var words = filter.FullName.Split(" ");
                foreach (var element in words)
                    sb.Append(" AND CabPerson.FullName LIKE '%" + element + "%'");
            }

            if (filter.MobileNumber != null)
                sb.Append(" AND CabMobilePhone.MobilePhoneNumber LIKE '%" + filter.MobileNumber + "%'");

            if (filter.Organisation != null)
            {
                filter.Organisation = filter.Organisation.Replace("'", "''");

                sb.Append(" AND CabCompany.Name LIKE '%" + filter.Organisation + "%'");
            }

            if (filter.JobTitle != null)
            {
                filter.JobTitle = filter.JobTitle.Replace("'", "''");
                sb.Append(" AND CabPersonCompany.JobRole LIKE '%" + filter.JobTitle + "%'");

            }

            if (filter.Email != null)
            {
                filter.Email = filter.Email.Replace("'", "''");

                sb.Append(" AND CabEmail.EmailAddress LIKE '%" + filter.Email + "%'");
            }

            if (filter.IsSaved != null)
                switch (filter.IsSaved.ToLower())
                {
                    case "true":
                        sb.Append(" AND CabPerson.IsSaved = 1");
                        break;
                    case "false":
                        sb.Append(" AND CabPerson.IsSaved = 0");
                        break;
                }
        }

        //organisation mobileNumber
        if (filter != null && filter.CabPersonSortingModel != null)
        {
            if (filter.CabPersonSortingModel.Attribute != null &&
                filter.CabPersonSortingModel.Order.Equals("asc"))
                sb.Append(" ORDER BY " + filter.CabPersonSortingModel.Attribute + " ASC");

            if (filter.CabPersonSortingModel.Attribute != null &&
                filter.CabPersonSortingModel.Order.Equals("desc"))
                sb.Append(" ORDER BY " + filter.CabPersonSortingModel.Attribute + " DESC");
        }

        using IDbConnection dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
        var result =
            await dbConnection
                .QueryAsync<ProjectPersonFilterDto, ProjectPersonFilterPersonDto, ProjectPersonFilterPersonCompanyDto,
                    ProjectPersonFilterCompanyDto, ProjectPersonFilterDto>(sb.ToString(),
                    (cabData, personData, personCompany, company) =>
                    {
                        cabData.Person = personData;
                        cabData.PersonCompany = personCompany;
                        cabData.Company = company;
                        return cabData;
                    }, new { SequenceCode = personRepositoryParameter.ProjectSqCode });


        

        return result;
    }

    public async Task<bool> IsExistCabEntry(PersonRepositoryParameter personRepositoryParameter)
    {
        using IDbConnection dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
        var count = dbConnection.ExecuteScalar<int>("SELECT COUNT(*) FROM CabPerson");
        

        return count > 0;
    }

    public async Task<IEnumerable<CabPersonNameFilterDto>> PersonFilterByName(
        PersonRepositoryParameter personRepositoryParameter)
    {
        var sql = @"SELECT CabPerson.Id AS [Key], CabPerson.FullName AS Text
                               FROM CabPerson
                               where CabPerson.FullName like '%" + personRepositoryParameter.Name +
                  "%' AND CabPerson.IsDeleted = 0";
        //var parameters = new { name = personRepositoryParameter.Name };
        await using var dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
        await dbConnection.OpenAsync();
        var result = await dbConnection.QueryAsync<CabPersonNameFilterDto>(sql);
        

        return result;
    }

    public async Task<string> GetMobileNumberId(PersonRepositoryParameter personRepositoryParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);
        var person = applicationDbContext.CabPersonCompany.Where(p =>
                p.PersonId == personRepositoryParameter.PersonId && p.IsDeleted == false)
            .Include(p => p.MobilePhone)
            //.Include(p => p.BusinessPhone)
            .ToList().FirstOrDefault();
        return person is { MobilePhone: { } } ? person.MobilePhone.MobilePhoneNumber : null;
    }

    public async Task<string> GetEmailByPersonId(PersonRepositoryParameter personRepositoryParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);
        var person = applicationDbContext.CabPersonCompany
            .Where(p => p.PersonId == personRepositoryParameter.PersonId)
            .Include(p => p.Email).FirstOrDefault();

        return person is { Email: { } } ? person.Email.EmailAddress : null;
    }

    public async Task<IEnumerable<CabPersonNameFilterDto>> ForemanFilterByName(
        PersonRepositoryParameter personRepositoryParameter)
    {
        var sql =
            "SELECT DISTINCT CabPerson.Id AS [Key] ,CabPerson.FullName AS Text FROM dbo.ProjectTeamRole " +
            "LEFT OUTER JOIN dbo.CabPerson ON ProjectTeamRole.CabPersonId = CabPerson.Id " +
            "LEFT OUTER JOIN dbo.ProjectTeam ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id " +
            "LEFT OUTER JOIN dbo.ProjectDefinition ON ProjectTeam.ProjectId = ProjectDefinition.Id " +
            "WHERE ProjectDefinition.SequenceCode = '" + personRepositoryParameter.ProjectSqCode +
            "' AND CabPerson.FullName LIKE '%" + personRepositoryParameter.Name + "%'";
        await using var dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
        await dbConnection.OpenAsync();
        var result = await dbConnection.QueryAsync<CabPersonNameFilterDto>(sql);
        

        return result;
    }

    public async Task<string> CreateCabCompetencies(PersonRepositoryParameter personRepositoryParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);

        const string sql = @"SELECT * FROM CabCompetencies WHERE Id = @Id";

        CabCompetencies data;
        await using (var dbConnection =
                     new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = dbConnection
                .Query<CabCompetencies>(sql, new { personRepositoryParameter.CabCompetenciesDto.Id })
                .FirstOrDefault();
        }

        if (data == null)
        {
            var insertSql =
                @"INSERT INTO CabCompetencies VALUES (@Id, @PersonId, @Date, @CompetenciesTaxonomyId, @ExperienceLevelId)";
            var param = new
            {
                personRepositoryParameter.CabCompetenciesDto.Id,
                personRepositoryParameter.CabCompetenciesDto.PersonId,
                personRepositoryParameter.CabCompetenciesDto.Date,
                personRepositoryParameter.CabCompetenciesDto.CompetenciesTaxonomyId,
                personRepositoryParameter.CabCompetenciesDto.ExperienceLevelId
            };
            await using var dbConnection =
                new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
            await dbConnection.ExecuteAsync(insertSql, param);
        }
        else
        {
            const string updateSql =
                @"UPDATE CabCompetencies SET Date = @Date, CompetenciesTaxonomyId = @CompetenciesTaxonomyId, ExperienceLevelId = @ExperienceLevelId WHERE Id = @Id";

            var param = new
            {
                data.Id,
                personRepositoryParameter.CabCompetenciesDto.Date,
                personRepositoryParameter.CabCompetenciesDto.CompetenciesTaxonomyId,
                personRepositoryParameter.CabCompetenciesDto.ExperienceLevelId
            };

            await using var dbConnection =
                new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
            await dbConnection.ExecuteAsync(updateSql, param);
        }

        return personRepositoryParameter.CabCompetenciesDto.Id;
    }

    public async Task<IEnumerable<GetCabCompetencies>> GetCabCompetencies(
        PersonRepositoryParameter personRepositoryParameter)
    {
        const string sql = @"SELECT * FROM CabCompetencies WHERE PersonId = @PersonId";

        IEnumerable<GetCabCompetencies> data;
        CompetenciesTaxonomy taxonomyTitle;
        await using (var dbConnection =
                     new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = dbConnection.Query<GetCabCompetencies>(sql,
                new { personRepositoryParameter.PersonId });
        }

        foreach (var item in data)
        {
            const string sql2 = @"SELECT Title FROM CompetenciesTaxonomy WHERE Id = @Id";
            await using var dbConnection =
                new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
            item.Title = dbConnection.Query<string>(sql2, new { Id = item.CompetenciesTaxonomyId })
                .FirstOrDefault();
            //taxonomyTitle = dbConnection.Query<CompetenciesTaxonomy>(parentTaxonomy, new {Id = item.CompetenciesTaxonomyId }).LastOrDefault();
            item.ExperienceLevelName = dbConnection.Query<string>(
                    "SELECT PbsExperienceLocalizedData.Label FROM dbo.PbsExperienceLocalizedData WHERE PbsExperienceLocalizedData.LanguageCode = @lang AND  PbsExperienceLocalizedData.PbsExperienceId = @PbsExperienceId",
                    new { PbsExperienceId = item.ExperienceLevelId, lang = personRepositoryParameter.Lang })
                .FirstOrDefault();
        }


        return data;
    }

    public async Task<string> DeleteCabCompetencies(PersonRepositoryParameter personRepositoryParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);
        const string deleteQuery = @"DELETE FROM CabCompetencies WHERE Id =@Id";
        if (personRepositoryParameter.IdListForDelete.FirstOrDefault() != null)
            foreach (var id in personRepositoryParameter.IdListForDelete)
            {
                await using var dbConnection =
                    new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
                await dbConnection.ExecuteAsync(deleteQuery, new { Id = id });
            }

        return null;
    }

    public async Task<string> CreateCabCertification(PersonRepositoryParameter personRepositoryParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);

        const string sql = @"SELECT * FROM CabCertification WHERE Id = @Id";

        CabCertification data;
        await using (var dbConnection =
                     new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = dbConnection
                .Query<CabCertification>(sql, new { personRepositoryParameter.CabCertificationDto.Id })
                .FirstOrDefault();
        }

        if (data == null)
        {
            const string insertSql =
                @"INSERT INTO CabCertification VALUES (@Id, @PersonId, @StartDate, @EndDate, @CertificationTaxonomyId,@CertificationTitle,@CertificationUrl)";
            var param = new
            {
                personRepositoryParameter.CabCertificationDto.Id,
                personRepositoryParameter.CabCertificationDto.PersonId,
                personRepositoryParameter.CabCertificationDto.StartDate,
                personRepositoryParameter.CabCertificationDto.CertificationTaxonomyId,
                personRepositoryParameter.CabCertificationDto.EndDate,
                personRepositoryParameter.CabCertificationDto.CertificationTitle,
                personRepositoryParameter.CabCertificationDto.CertificationUrl
            };
            await using var dbConnection =
                new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
            await dbConnection.ExecuteAsync(insertSql, param);
        }
        else
        {
            const string updateSql =
                @"UPDATE CabCertification SET StartDate = @StartDate, CertificationTaxonomyId = @CertificationTaxonomyId, EndDate = @EndDate,CertificationTitle = @CertificationTitle,CertificationUrl = @CertificationUrl WHERE Id = @Id";

            var param = new
            {
                data.Id,
                personRepositoryParameter.CabCertificationDto.StartDate,
                personRepositoryParameter.CabCertificationDto.CertificationTaxonomyId,
                personRepositoryParameter.CabCertificationDto.CertificationTitle,
                personRepositoryParameter.CabCertificationDto.EndDate,
                personRepositoryParameter.CabCertificationDto.CertificationUrl
            };

            await using var dbConnection =
                new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
            await dbConnection.ExecuteAsync(updateSql, param);
        }

        return personRepositoryParameter.CabCertificationDto.Id;
    }

    public async Task<IEnumerable<GetCabCertification>> GetCabCertification(
        PersonRepositoryParameter personRepositoryParameter)
    {
        const string sql = @"SELECT * FROM CabCertification WHERE PersonId = @PersonId";

        IEnumerable<GetCabCertification> data;
        CertificationTaxonomy taxonomy;
        await using (var dbConnection =
                     new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = dbConnection.Query<GetCabCertification>(sql,
                new { personRepositoryParameter.PersonId });
        }

        foreach (var item in data)
        {
            const string sql2 = @"SELECT Title FROM CertificationTaxonomy WHERE Id = @Id";
            await using var dbConnection =
                new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
            item.Title = dbConnection.Query<string>(sql2, new { Id = item.CertificationTaxonomyId })
                .FirstOrDefault();

            if (DateTime.Today < item.EndDate) item.Validity = false;
        }

        return data;
    }

    public async Task<IEnumerable<GetCabCertification>> GetCabCertificationCiaw(
        PersonRepositoryParameter personRepositoryParameter)
    {
        const string sql = @"SELECT * FROM CabCertification WHERE PersonId = @PersonId";

        IEnumerable<GetCabCertification> data;
        CertificationTaxonomy taxonomy;
        await using (var dbConnection =
                     new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = dbConnection.Query<GetCabCertification>(sql,
                new { personRepositoryParameter.GetCabCertificationDto.PersonId });
        }

        foreach (var item in data)
        {
            const string sql2 = @"SELECT Title FROM CertificationTaxonomy WHERE Id = @Id";
            await using var dbConnection =
                new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
            item.Title = dbConnection.Query<string>(sql2, new { Id = item.CertificationTaxonomyId })
                .FirstOrDefault();

            if (personRepositoryParameter.GetCabCertificationDto.Date > item.EndDate) item.Validity = false;
        }

        return data;
    }

    public async Task<string> DeleteCabCertification(PersonRepositoryParameter personRepositoryParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, personRepositoryParameter.TenantProvider);
        const string deleteQuery = @"DELETE FROM CabCertification WHERE Id =@Id";
        if (personRepositoryParameter.IdListForDelete.FirstOrDefault() != null)
            foreach (var id in personRepositoryParameter.IdListForDelete)
            {
                await using var dbConnection =
                    new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);
                await dbConnection.ExecuteAsync(deleteQuery, new { Id = id });
            }

        return null;
    }

    public async Task<IEnumerable<ContractTaxonomyDto>> GetContractorTaxonomyList(
        PersonRepositoryParameter personRepositoryParameter)
    {
        const string sql = @"SELECT * FROM ContractTaxonomy";

        IEnumerable<ContractTaxonomyDto> data;

        await using (var dbConnection =
                     new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = dbConnection.Query<ContractTaxonomyDto>(sql);
        }

        return data;
    }

    public async Task<string> CreateContractorTaxonomy(PersonRepositoryParameter PersonRepositoryParameter)
    {
        const string query =
            @"MERGE INTO dbo.ContractTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE  SET Title = @Title WHEN NOT MATCHED THEN INSERT (Id,ContractId,ParentId,ContractTaxonomyLevelId,Title ) VALUES (@Id,@ContractId,@ParentId,@ContractTaxonomyLevelId,@Title);";
        var parameters = new
        {
            PersonRepositoryParameter.ContractTaxonomyDto.Id,
            PersonRepositoryParameter.ContractTaxonomyDto.Title,
            PersonRepositoryParameter.ContractTaxonomyDto.ContractId,
            PersonRepositoryParameter.ContractTaxonomyDto.ParentId,
            PersonRepositoryParameter.ContractTaxonomyDto.ContractTaxonomyLevelId
        };

        await using (var connection =
                     new SqlConnection(PersonRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.ExecuteAsync(query, parameters);
        }


        return PersonRepositoryParameter.ContractTaxonomyDto.Id;
    }

    public async Task<string> AddNationlity(PersonRepositoryParameter personRepositoryParameter, bool update)
    {
        using IDbConnection dbConnection =
            new SqlConnection(personRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

        var param = new
        {
            Id = Guid.NewGuid(),
            CabPersonId = personRepositoryParameter.Person.Id,
            personRepositoryParameter.NationalityId
        };
        if (update == false)
        {
            var sql =
                @"INSERT INTO dbo.CabNationality ( Id ,CabPersonId ,NationalityId ) VALUES ( @Id ,@CabPersonId ,@NationalityId );";

            await dbConnection.ExecuteAsync(sql, param);
        }
        else
        {
            var sql =
                @"UPDATE dbo.CabNationality SET NationalityId = @NationalityId WHERE CabPersonId = @CabPersonId ;";

            await dbConnection.ExecuteAsync(sql, param);
        }

        return personRepositoryParameter.Person.Id;
    }

    private async Task<IEnumerable<CabDataDto>> CreateDtoList(List<CabPerson> personList,
        PersonRepositoryParameter personRepositoryParameter)
    {
        var cabDataDtoList = new List<CabDataDto>();
        foreach (var person in personList)
        {
            var cabDataDto = new CabDataDto();
            cabDataDto.IsSaved = person.IsSaved;

            var personDto = new PersonDto
            {
                Id = person.Id,

                FullName = person.FullName,
                Image = person.Image
            };

            cabDataDto.Person = personDto;

            if (person.PersonCompanyList != null && person.PersonCompanyList.Any())
            {
                var personCompany = person.PersonCompanyList.First();
                var personCompanyDto = new PersonCompanyDto
                {
                    Id = personCompany.Id,
                    JobRole = personCompany.JobRole
                };

                if (personCompany.Email != null)
                    personCompanyDto.Email = personCompany.Email.EmailAddress;
                if (personCompany.MobilePhone != null)
                    personCompanyDto.MobilePhone = personCompany.MobilePhone.MobilePhoneNumber;


                personCompanyDto.PersonId = personCompany.PersonId;
                personCompanyDto.CompanyId = personCompany.CompanyId;

                cabDataDto.PersonCompany = personCompanyDto;
                var companyDto = new CompanyDto();
                if (personCompany.Company != null) companyDto.Name = personCompany.Company.Name;

                cabDataDto.Company = companyDto;
            }

            cabDataDtoList.Add(cabDataDto);
        }

        return cabDataDtoList;
    }
}