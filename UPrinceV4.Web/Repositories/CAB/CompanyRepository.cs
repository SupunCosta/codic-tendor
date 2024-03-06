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
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.CAB;

public class CompanyRepository : ICompanyRepository
{
    public async Task<IEnumerable<CompanyDto>> GetCompanyList(CompanyRepositoryParameter companyRepositoryParameter)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options, companyRepositoryParameter.TenantProvider);
            var companyList = applicationDbContext.CabCompany.Where(c => c.IsDeleted == false && c.IsSaved == true)
                .Include(b => b.CabBankAccount)
                .Include(v => v.CabVat)
                .Include(v => v.Email)
                .Include(v => v.LandPhone)
                .Include(v => v.MobilePhone)
                .Include(v => v.WhatsApp)
                .Include(v => v.Skype)
                .Include(c => c.Address).ThenInclude(c => c.Country).ToList();
            var dtoList = await GetCompanyDtoList(companyList);
            return dtoList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<UnassignedCompanyDto>> GetUnassignedCompanyList(
        CompanyRepositoryParameter companyRepositoryParameter)
    {
        try
        {
            var sql = "SELECT   CabCompany.Id  ,CabCompany.Name FROM dbo.CabCompany " +
                      " LEFT OUTER JOIN dbo.CabPersonCompany   ON CabCompany.Id = CabPersonCompany.CompanyId " +
                      " WHERE CabPersonCompany.CompanyId IS NULL";
            var sb = new StringBuilder(sql);
            if (!string.IsNullOrEmpty(companyRepositoryParameter.CompanyName))
            {
                companyRepositoryParameter.CompanyName = companyRepositoryParameter.CompanyName.Replace("'", "''");
                sb.Append(" AND CabCompany.Name LIKE '%" + companyRepositoryParameter.CompanyName + "%'");

            }
                
            using (IDbConnection dbConnection =
                   new SqlConnection(companyRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
            {
                var result = dbConnection.Query<UnassignedCompanyDto>(sb.ToString()).ToList();
                

                return result;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<CompanyDto> GetCompanyById(CompanyRepositoryParameter companyRepositoryParameter)
    {
        try
        {
            using IDbConnection dbConnection =
                new SqlConnection(companyRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options, companyRepositoryParameter.TenantProvider);

            var company = applicationDbContext.CabCompany.Where(p =>
                    p.Id == companyRepositoryParameter.CompanyId && p.IsDeleted == false && p.IsSaved == true)
                .Include(b => b.CabBankAccount)
                .Include(v => v.CabVat)
                .Include(v => v.Email)
                .Include(v => v.LandPhone)
                .Include(v => v.MobilePhone)
                .Include(v => v.WhatsApp)
                .Include(v => v.Skype)
                .Include(c => c.Address)
                .ThenInclude(c => c.Country).ToList().FirstOrDefault();
            CompanyDto companyDto = null;
            if (company != null)
            {
                var
                    optionsHistory = new DbContextOptions<ApplicationDbContext>();
                var applicationDbContextHistory =
                    new ApplicationDbContext(optionsHistory, companyRepositoryParameter.TenantProvider);
                var historyDetails = applicationDbContextHistory.CabHistoryLog
                    .Where(p => p.CompanyId == company.Id)
                    .OrderByDescending(p => p.ChangedTime)
                    .Include(h => h.ChangedByUser);
                var lastState = historyDetails.FirstOrDefault();
                var firstState = historyDetails.LastOrDefault();
                companyDto = new CompanyDto();
                companyDto.Id = company.Id;
                companyDto.Name = company.Name;
                companyDto.AccountingNumber = company.AccountingNumber;
                companyDto.ContractorTaxonomyId = dbConnection
                    .Query<string>("Select TaxonomyId from CabContractorTaxonomycs Where CompanyId = @CompanyId",
                        new { companyRepositoryParameter.CompanyId }).ToList();

                if (company.MobilePhone != null)
                    companyDto.MobilePhone = company.MobilePhone.MobilePhoneNumber;
                if (company.Skype != null)
                    companyDto.Skype = company.Skype.SkypeNumber;
                if (company.LandPhone != null)
                    companyDto.LandPhone = company.LandPhone.LandPhoneNumber;
                if (company.Email != null)
                    companyDto.Email = company.Email.EmailAddress;
                if (company.CabVat != null)
                    companyDto.VAT = company.CabVat.Vat;
                if (company.CabBankAccount != null)
                    companyDto.BankAccount = company.CabBankAccount.BankAccount;
                if (company.WhatsApp != null)
                    companyDto.WhatsApp = company.WhatsApp.WhatsAppNumber;
                companyDto.IsSaved = companyDto.IsSaved;
                if (company.Address != null)
                {
                    var addressDto = new AddressDto
                    {
                        Id = company.Address.Id,
                        Street = company.Address.Street,
                        MailBox = company.Address.MailBox,
                        PostalCode = company.Address.PostalCode,
                        City = company.Address.City,
                        Region = company.Address.Region,
                        StreetNumber = company.Address.StreetNumber,
                        CountryId = company.Address.CountryId
                    };
                    companyDto.Address = addressDto;
                }

                companyDto.IsSaved = companyDto.IsSaved;

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
                    companyDto.History = cabHistoryLogDto;
                    if (historyDetails.Count() <= 1) return companyDto;
                    cabHistoryLogDto.UpdatedByUser =
                        lastState.ChangedByUser?.FirstName + " " + lastState.ChangedByUser?.LastName;
                    cabHistoryLogDto.UpdatedDateTime = lastState.ChangedTime;
                    cabHistoryLogDto.RevisionNumber = lastState.RevisionNumber;
                    companyDto.History = cabHistoryLogDto;
                }
            }

            return companyDto;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> AddCompany(CompanyRepositoryParameter companyRepositoryParameter)
    {
        try
        {
            using IDbConnection dbConnection =
                new SqlConnection(companyRepositoryParameter.TenantProvider.GetTenant().ConnectionString);

            var dbContext = companyRepositoryParameter.ApplicationDbContext;
            var company = companyRepositoryParameter.Company;

            if (company.Id == null)
            {
                company.Id = Guid.NewGuid().ToString();
                var idGenerator = new IdGenerator();
                company.SequenceCode = idGenerator.GenerateId(dbContext, "COM-", "CompanySequenceCode");
                company.AccountingNumber = company.AccountingNumber;
                if (company.Address != null)
                {
                    var address = new CabAddress
                    {
                        Id = Guid.NewGuid().ToString(),
                        City = company.Address.City,
                        CountryId = company.Address.CountryId,
                        Region = company.Address.Region,
                        Street = company.Address.Street,
                        StreetNumber = company.Address.StreetNumber,
                        MailBox = company.Address.MailBox,
                        PostalCode = company.Address.PostalCode
                    };
                    company.AddressId = address.Id;
                    company.Address = address;
                    companyRepositoryParameter.ApplicationDbContext.CabAddress.Add(address);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }
                else
                {
                    company.Address = null;
                }


                if (company.CabBankAccount.BankAccount != null)
                {
                    var bankAccount = new CabBankAccount
                    {
                        Id = Guid.NewGuid().ToString(),
                        BankAccount = company.CabBankAccount.BankAccount
                    };
                    company.BankAccountId = bankAccount.Id;
                    company.CabBankAccount = bankAccount;
                    companyRepositoryParameter.ApplicationDbContext.CabBankAccount.Add(bankAccount);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }
                else
                {
                    company.CabBankAccount = null;
                }


                if (company.CabVat.Vat != null)
                {
                    var vat = new CabVat
                    {
                        Id = Guid.NewGuid().ToString(),
                        Vat = company.CabVat.Vat
                    };
                    company.VatId = vat.Id;
                    company.CabVat = vat;
                    companyRepositoryParameter.ApplicationDbContext.CabVat.Add(vat);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }
                else
                {
                    company.CabVat = null;
                }

                if (company.Email.EmailAddress != null)
                {
                    var existingEmail =
                        dbContext.CabEmail.FirstOrDefault(e =>
                            e.EmailAddress == company.Email.EmailAddress && e.IsDeleted == true);
                    if (existingEmail != null)
                    {
                        company.EmailId = existingEmail.Id;
                        company.Email = existingEmail;
                        existingEmail.IsDeleted = false;
                        dbContext.CabEmail.Update(existingEmail);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        var email = new CabEmail
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmailAddress = company.Email.EmailAddress
                        };
                        company.EmailId = email.Id;
                        company.Email = email;
                        dbContext.CabEmail.Add(email);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    company.Email = null;
                }


                if (company.LandPhone.LandPhoneNumber != null)
                {
                    var existingLangPhone =
                        dbContext.CabLandPhone.FirstOrDefault(
                            l => l.LandPhoneNumber == company.LandPhone.LandPhoneNumber && l.IsDeleted == true);
                    if (existingLangPhone != null)
                    {
                        company.LandPhoneId = existingLangPhone.Id;
                        company.LandPhone = existingLangPhone;
                        existingLangPhone.IsDeleted = false;
                        dbContext.CabLandPhone.Update(existingLangPhone);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        var landPhone = new CabLandPhone
                        {
                            Id = Guid.NewGuid().ToString(),
                            LandPhoneNumber = company.LandPhone.LandPhoneNumber
                        };
                        company.LandPhoneId = landPhone.Id;
                        company.LandPhone = landPhone;
                        dbContext.CabLandPhone.Add(landPhone);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    company.LandPhone = null;
                }


                if (company.MobilePhone.MobilePhoneNumber != null)
                {
                    var existingMobilePhone = dbContext.CabMobilePhone.FirstOrDefault(m =>
                        m.MobilePhoneNumber == company.MobilePhone.MobilePhoneNumber &&
                        m.IsDeleted == true);
                    if (existingMobilePhone != null)
                    {
                        company.MobilePhoneId = existingMobilePhone.Id;
                        company.MobilePhone = existingMobilePhone;
                        existingMobilePhone.IsDeleted = false;
                        dbContext.CabMobilePhone.Update(existingMobilePhone);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        var mobilePhone = new CabMobilePhone
                        {
                            Id = Guid.NewGuid().ToString(),
                            MobilePhoneNumber = company.MobilePhone.MobilePhoneNumber
                        };
                        company.MobilePhoneId = mobilePhone.Id;
                        company.MobilePhone = mobilePhone;
                        dbContext.CabMobilePhone.Add(mobilePhone);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    company.MobilePhone = null;
                }


                if (company.WhatsApp.WhatsAppNumber != null)
                {
                    var existingWhatsApp = dbContext.CabWhatsApp.FirstOrDefault(w =>
                        w.WhatsAppNumber == company.WhatsApp.WhatsAppNumber &&
                        w.IsDeleted == true);
                    if (existingWhatsApp != null)
                    {
                        company.WhatsAppId = existingWhatsApp.Id;
                        company.WhatsApp = existingWhatsApp;
                        existingWhatsApp.IsDeleted = false;
                        dbContext.CabWhatsApp.Update(existingWhatsApp);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        var whatsapp = new CabWhatsApp
                        {
                            Id = Guid.NewGuid().ToString(),
                            WhatsAppNumber = company.WhatsApp.WhatsAppNumber
                        };
                        company.WhatsAppId = whatsapp.Id;
                        company.WhatsApp = whatsapp;
                        dbContext.CabWhatsApp.Add(whatsapp);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    company.WhatsApp = null;
                }


                if (company.Skype.SkypeNumber != null)
                {
                    var existingSkype = dbContext.CabSkype.FirstOrDefault(s =>
                        s.SkypeNumber == company.Skype.SkypeNumber && s.IsDeleted == true);
                    if (existingSkype != null)
                    {
                        company.SkypeId = existingSkype.Id;
                        company.Skype = existingSkype;
                        existingSkype.IsDeleted = false;
                        dbContext.CabSkype.Update(existingSkype);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        var skype = new CabSkype
                        {
                            Id = Guid.NewGuid().ToString(),
                            SkypeNumber = company.Skype.SkypeNumber
                        };
                        company.SkypeId = skype.Id;
                        company.Skype = skype;
                        dbContext.CabSkype.Add(skype);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    company.Skype = null;
                }


                companyRepositoryParameter.ApplicationDbContext.CabCompany.Add(company);
                companyRepositoryParameter.ApplicationDbContext.SaveChanges();

                companyRepositoryParameter.CabHistoryLogRepositoryParameter.TenantProvider =
                    companyRepositoryParameter.TenantProvider;
                var history = companyRepositoryParameter.CabHistoryLogRepositoryParameter;
                history.Action = HistoryState.ADDED.ToString();
                history.Company.Id = company.Id;
                var historyRepository =
                    companyRepositoryParameter.CabHistoryLogRepository.AddCabHistoryLog(history);

                if (company.ContractorTaxonomyId?.FirstOrDefault() != null)
                {
                    var insertQuery =
                        "INSERT INTO dbo.CabContractorTaxonomycs ( Id ,TaxonomyId ,CompanyId ) VALUES ( @Id ,@TaxonomyId ,@CompanyId )";

                    foreach (var item in company.ContractorTaxonomyId)
                    {
                        var InsertParam = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TaxonomyId = item,
                            CompanyId = company.Id
                        };

                        await dbConnection.ExecuteAsync(insertQuery, InsertParam);
                    }
                }

                return company.Id;
            }

            var updatedCompany = dbContext.CabCompany.FirstOrDefault(c =>
                c.Id == company.Id);
            if (updatedCompany != null)
            {
                updatedCompany.AccountingNumber = company.AccountingNumber;
                updatedCompany.Name = company.Name;
                updatedCompany.IsSaved = company.IsSaved;
                var updatedAddress = dbContext.CabAddress.FirstOrDefault(a => a.Id == updatedCompany.AddressId);
                if (updatedAddress == null)
                {
                    var address = new CabAddress
                    {
                        Id = Guid.NewGuid().ToString()
                    };
                    if (company.Address != null)
                    {
                        address.City = company.Address.City;
                        address.CountryId = company.Address.CountryId;
                        address.Region = company.Address.Region;
                        address.Street = company.Address.Street;
                        address.StreetNumber = company.Address.StreetNumber;
                        address.MailBox = company.Address.MailBox;
                        address.PostalCode = company.Address.PostalCode;
                    }

                    updatedCompany.AddressId = address.Id;
                    updatedCompany.Address = address;
                    companyRepositoryParameter.ApplicationDbContext.CabAddress.Add(address);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }
                else
                {
                    updatedAddress.City = company.Address.City;
                    updatedAddress.CountryId = company.Address.CountryId;
                    updatedAddress.Region = company.Address.Region;
                    updatedAddress.Street = company.Address.Street;
                    updatedAddress.StreetNumber = company.Address.StreetNumber;
                    updatedAddress.MailBox = company.Address.MailBox;
                    updatedAddress.PostalCode = company.Address.PostalCode;
                    updatedCompany.AddressId = updatedAddress.Id;
                    updatedCompany.Address = updatedAddress;
                    companyRepositoryParameter.ApplicationDbContext.CabAddress.Update(updatedAddress);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }

                if (company.Email.EmailAddress != null)
                {
                    var updatedEmail = dbContext.CabEmail.FirstOrDefault(a => a.Id == updatedCompany.EmailId);
                    if (updatedEmail == null)
                    {
                        var email = new CabEmail
                        {
                            Id = Guid.NewGuid().ToString(),
                            EmailAddress = company.Email.EmailAddress
                        };

                        updatedCompany.EmailId = email.Id;
                        updatedCompany.Email = email;
                        dbContext.CabEmail.Add(email);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        updatedEmail.EmailAddress = company.Email.EmailAddress;
                        dbContext.CabEmail.Update(updatedEmail);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    //if (updatedCompany.Email != null)
                    //{
                    var email = dbContext.CabEmail.FirstOrDefault(e => e.Id == updatedCompany.EmailId);
                    if (email != null)
                    {
                        email.IsDeleted = true;
                        dbContext.CabEmail.Update(email);
                        dbContext.SaveChanges();
                        updatedCompany.Email = null;
                        updatedCompany.EmailId = null;
                        //}
                    }
                }

                if (company.LandPhone.LandPhoneNumber != null)
                {
                    var updatedLandPhone =
                        dbContext.CabLandPhone.FirstOrDefault(a => a.Id == updatedCompany.LandPhoneId);
                    if (updatedLandPhone == null)
                    {
                        var landPhone = new CabLandPhone
                        {
                            Id = Guid.NewGuid().ToString(),
                            LandPhoneNumber = company.LandPhone.LandPhoneNumber
                        };

                        updatedCompany.LandPhoneId = landPhone.Id;
                        updatedCompany.LandPhone = landPhone;
                        dbContext.CabLandPhone.Add(landPhone);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        updatedLandPhone.LandPhoneNumber = company.LandPhone.LandPhoneNumber;
                        dbContext.CabLandPhone.Update(updatedLandPhone);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    //if (updatedCompany.LandPhone != null)
                    //{
                    var landPhone =
                        dbContext.CabLandPhone.FirstOrDefault(e => e.Id == updatedCompany.LandPhoneId);
                    if (landPhone != null)
                    {
                        landPhone.IsDeleted = true;
                        dbContext.CabLandPhone.Update(landPhone);
                        dbContext.SaveChanges();
                        updatedCompany.LandPhone = null;
                        updatedCompany.LandPhoneId = null;
                    }
                    //}
                }

                if (company.MobilePhone.MobilePhoneNumber != null)
                {
                    var updatedMobilePhone =
                        dbContext.CabMobilePhone.FirstOrDefault(a => a.Id == updatedCompany.MobilePhoneId);
                    if (updatedMobilePhone == null)
                    {
                        var mobilePhone = new CabMobilePhone
                        {
                            Id = Guid.NewGuid().ToString(),
                            MobilePhoneNumber = company.MobilePhone.MobilePhoneNumber
                        };

                        updatedCompany.MobilePhoneId = mobilePhone.Id;
                        updatedCompany.MobilePhone = mobilePhone;
                        dbContext.CabMobilePhone.Add(mobilePhone);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        updatedMobilePhone.MobilePhoneNumber = company.MobilePhone.MobilePhoneNumber;
                        dbContext.CabMobilePhone.Update(updatedMobilePhone);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    //if (updatedCompany.MobilePhone != null)
                    //{
                    var mobilePhone =
                        dbContext.CabMobilePhone.FirstOrDefault(e => e.Id == updatedCompany.MobilePhoneId);
                    if (mobilePhone != null)
                    {
                        mobilePhone.IsDeleted = true;
                        dbContext.CabMobilePhone.Update(mobilePhone);
                        dbContext.SaveChanges();
                        updatedCompany.MobilePhone = null;
                        updatedCompany.MobilePhoneId = null;
                    }
                    //}
                }

                if (company.WhatsApp.WhatsAppNumber != null)
                {
                    var updatedWhatsApp =
                        dbContext.CabWhatsApp.FirstOrDefault(a => a.Id == updatedCompany.WhatsAppId);
                    if (updatedWhatsApp == null)
                    {
                        var whatsApp = new CabWhatsApp
                        {
                            Id = Guid.NewGuid().ToString(),
                            WhatsAppNumber = company.WhatsApp.WhatsAppNumber
                        };

                        updatedCompany.WhatsAppId = whatsApp.Id;
                        updatedCompany.WhatsApp = whatsApp;
                        dbContext.CabWhatsApp.Add(whatsApp);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        updatedWhatsApp.WhatsAppNumber = company.WhatsApp.WhatsAppNumber;
                        dbContext.CabWhatsApp.Update(updatedWhatsApp);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    var whatsApp = dbContext.CabWhatsApp.FirstOrDefault(e => e.Id == updatedCompany.WhatsAppId);
                    if (whatsApp != null)
                    {
                        whatsApp.IsDeleted = true;
                        dbContext.CabWhatsApp.Update(whatsApp);
                        dbContext.SaveChanges();
                        updatedCompany.WhatsApp = null;
                        updatedCompany.WhatsAppId = null;
                    }
                }

                if (company.Skype.SkypeNumber != null)
                {
                    var updatedSkype = dbContext.CabSkype.FirstOrDefault(a => a.Id == updatedCompany.SkypeId);
                    if (updatedSkype == null)
                    {
                        var skype = new CabSkype
                        {
                            Id = Guid.NewGuid().ToString(),
                            SkypeNumber = company.Skype.SkypeNumber
                        };

                        updatedCompany.SkypeId = skype.Id;
                        updatedCompany.Skype = skype;
                        dbContext.CabSkype.Add(skype);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        updatedSkype.SkypeNumber = company.Skype.SkypeNumber;
                        dbContext.CabSkype.Update(updatedSkype);
                        dbContext.SaveChanges();
                    }
                }
                else
                {
                    var skype = dbContext.CabSkype.FirstOrDefault(e => e.Id == updatedCompany.SkypeId);
                    if (skype != null)
                    {
                        skype.IsDeleted = true;
                        dbContext.CabSkype.Update(skype);
                        dbContext.SaveChanges();
                        updatedCompany.SkypeId = null;
                        updatedCompany.Skype = null;
                    }
                }

                var updatedBankAccount =
                    dbContext.CabBankAccount.FirstOrDefault(b => b.Id == updatedCompany.BankAccountId);
                if (updatedBankAccount == null && company.CabBankAccount.BankAccount != null)
                {
                    var bankAccount = new CabBankAccount
                    {
                        Id = Guid.NewGuid().ToString(),
                        BankAccount = company.CabBankAccount.BankAccount
                    };
                    updatedCompany.BankAccountId = bankAccount.Id;
                    updatedCompany.CabBankAccount = bankAccount;
                    companyRepositoryParameter.ApplicationDbContext.CabBankAccount.Add(bankAccount);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }
                else if (company.CabBankAccount.BankAccount != null)
                {
                    updatedBankAccount.BankAccount = company.CabBankAccount.BankAccount;
                    updatedCompany.BankAccountId = updatedBankAccount.Id;
                    updatedCompany.CabBankAccount = updatedBankAccount;
                    companyRepositoryParameter.ApplicationDbContext.CabBankAccount.Update(updatedBankAccount);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }

                var updatedCabVat = dbContext.CabVat.FirstOrDefault(v => v.Id == updatedCompany.VatId);
                if (updatedCabVat == null && company.CabVat.Vat != null)
                {
                    var vat = new CabVat
                    {
                        Id = Guid.NewGuid().ToString(),
                        Vat = company.CabVat.Vat
                    };
                    updatedCompany.VatId = vat.Id;
                    updatedCompany.CabVat = vat;
                    companyRepositoryParameter.ApplicationDbContext.CabVat.Add(vat);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }
                else if (company.CabVat.Vat != null)
                {
                    updatedCabVat.Vat = company.CabVat.Vat;
                    updatedCompany.VatId = updatedCabVat.Id;
                    updatedCompany.CabVat = updatedCabVat;
                    companyRepositoryParameter.ApplicationDbContext.CabVat.Update(updatedCabVat);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                }

                companyRepositoryParameter.ApplicationDbContext.CabCompany.Update(updatedCompany);
                companyRepositoryParameter.ApplicationDbContext.SaveChanges();

                // Add history data
                companyRepositoryParameter.CabHistoryLogRepositoryParameter.TenantProvider =
                    companyRepositoryParameter.TenantProvider;
                var history = companyRepositoryParameter.CabHistoryLogRepositoryParameter;
                history.Action = HistoryState.UPDATED.ToString();
                history.Company.Id = updatedCompany.Id;
                var historyRepository =
                    companyRepositoryParameter.CabHistoryLogRepository.AddCabHistoryLog(history);

                if (company.ContractorTaxonomyId?.FirstOrDefault() != null)
                {
                    await dbConnection.ExecuteAsync(
                        "Delete from dbo.CabContractorTaxonomycs Where CompanyId = @CompanyId",
                        new { CompanyId = company.Id });
                    var insertQuery =
                        "INSERT INTO dbo.CabContractorTaxonomycs ( Id ,TaxonomyId ,CompanyId ) VALUES ( @Id ,@TaxonomyId ,@CompanyId )";

                    foreach (var item in company.ContractorTaxonomyId)
                    {
                        var InsertParam = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            TaxonomyId = item,
                            CompanyId = company.Id
                        };

                        await dbConnection.ExecuteAsync(insertQuery, InsertParam);
                    }
                }
                else
                {
                    await dbConnection.ExecuteAsync(
                        "Delete from dbo.CabContractorTaxonomycs Where CompanyId = @CompanyId",
                        new { CompanyId = company.Id });
                }

                return updatedCompany.Id;
            }

            return "Cab Company Id is invalid";

            //}
            //else
            //{
            //    var existingCompany = GetCompanyById(companyRepositoryParameter);
            //    if (existingCompany != null)
            //    {
            //        return existingCompany.Id;
            //    }
            //    else
            //    {
            //        throw new Exception("Cant Find Company");
            //    }
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> DeleteCompany(CompanyRepositoryParameter companyRepositoryParameter)
    {
        try
        {
            var isUpdated = false;
            foreach (var companyId in companyRepositoryParameter.IdListForDelete)
            {
                var company =
                    companyRepositoryParameter.ApplicationDbContext.CabCompany.FirstOrDefault(
                        p => p.Id == companyId);
                if (company != null)
                {
                    company.IsDeleted = true;
                    companyRepositoryParameter.ApplicationDbContext.CabCompany.Update(company);
                    companyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    isUpdated = true;
                }
            }

            return isUpdated;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<CompanyDto>> GetCompanyListByName(
        CompanyRepositoryParameter companyRepositoryParameter)
    {
        try
        {
            IEnumerable<CabCompany> companyList = null;
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options, companyRepositoryParameter.TenantProvider);
            if (companyRepositoryParameter.CompanyName == null)
                companyList = applicationDbContext.CabCompany
                    .Where(p => p.IsDeleted == false && p.IsSaved == true).Include(p => p.Address)
                    .Include(p => p.CabBankAccount)
                    .Include(p => p.CabVat).Take(50)
                    .Include(v => v.Email)
                    .Include(v => v.LandPhone)
                    .Include(v => v.MobilePhone)
                    .Include(v => v.WhatsApp)
                    .Include(v => v.Skype)
                    .ToList();
            else
                companyList = applicationDbContext.CabCompany
                    .Where(p => p.Name.ToLower().Contains(companyRepositoryParameter.CompanyName) &&
                                p.IsDeleted == false && p.IsSaved == true).Include(p => p.Address)
                    .Include(p => p.CabBankAccount)
                    .Include(p => p.CabVat).Include(v => v.Email)
                    .Include(v => v.LandPhone)
                    .Include(v => v.MobilePhone)
                    .Include(v => v.WhatsApp)
                    .Include(v => v.Skype)
                    .ToList();

            var dtoList = await GetCompanyDtoList(companyList);
            return dtoList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<GroupByCompanyDto>> GetCompanyListGroupByCompany(
        CompanyRepositoryParameter companyRepositoryParameter)
    {
        try
        {
            var sql =
                "SELECT   CabPersonCompany.CompanyId AS CompanyId  ,CabPersonCompany.PersonId AS PersonId  ,CabPerson.Id AS Pid  ,CabCompany.Id AS Cid  ,CabCompany.Name AS Name  ,CabEmail.EmailAddress AS Email  ,CabMobilePhone.MobilePhoneNumber AS MobilePhone  ,CabPerson.IsSaved AS IsSaved  ,CabPerson.FullName AS FullName FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson   ON CabPersonCompany.PersonId = CabPerson.Id RIGHT OUTER JOIN dbo.CabCompany   ON CabPersonCompany.CompanyId = CabCompany.Id LEFT OUTER JOIN dbo.CabEmail   ON CabPersonCompany.EmailId = CabEmail.Id LEFT OUTER JOIN dbo.CabMobilePhone   ON CabPersonCompany.MobilePhoneId = CabMobilePhone.Id WHERE (CabPersonCompany.EmailId IS NULL OR CabPersonCompany.EmailId = CabEmail.Id) AND (CabPersonCompany.MobilePhoneId IS NULL OR CabPersonCompany.MobilePhoneId = CabMobilePhone.Id) ORDER BY LOWER(CabCompany.Name)";
            using (IDbConnection dbConnection =
                   new SqlConnection(companyRepositoryParameter.TenantProvider.GetTenant().ConnectionString))
            {
                var result = dbConnection.Query<GroupByCompanyLoadingDto>(sql).ToList();

                var dtoDataDictionary = new Dictionary<string, GroupByCompanyDto>();
                foreach (var dto in result)
                    if (dto.Cid != null)
                    {
                        if (dtoDataDictionary.Any(p => p.Key.Equals(dto.Cid)))
                        {
                            var existingDto = dtoDataDictionary[dto.Cid];
                            var personDto = new PersonDtoDapper
                            {
                                Id = dto.PersonId,
                                FullName = dto.FullName,
                                Email = dto.Email,
                                MobilePhone = dto.MobilePhone,
                                IsSaved = dto.IsSaved
                            };
                            existingDto.PersonList.Add(personDto);
                        }
                        else
                        {
                            var groupByCompanyDto = new GroupByCompanyDto
                            {
                                Id = dto.Cid,
                                Name = dto.Name
                            };

                            var personDto = new PersonDtoDapper
                            {
                                FullName = dto.FullName,
                                Email = dto.Email,
                                MobilePhone = dto.MobilePhone
                            };

                            groupByCompanyDto.PersonList.Add(personDto);
                            dtoDataDictionary.Add(dto.Cid, groupByCompanyDto);
                        }
                    }

                

                var list = dtoDataDictionary.Values.ToList();
                var srtList = list.OrderBy(d => d.Name);
                return srtList;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private async Task<IEnumerable<CompanyDto>> GetCompanyDtoList(IEnumerable<CabCompany> companyList)
    {
        var companyDtoList = new List<CompanyDto>();
        foreach (var company in companyList)
        {
            var companyDto = new CompanyDto();
            companyDto.Id = company.Id;
            companyDto.Name = company.Name;
            if (company.MobilePhone != null)
                companyDto.MobilePhone = company.MobilePhone.MobilePhoneNumber;
            if (company.Skype != null)
                companyDto.Skype = company.Skype.SkypeNumber;
            if (company.LandPhone != null)
                companyDto.LandPhone = company.Name;
            if (company.Email != null)
                companyDto.Email = company.Email.EmailAddress;
            if (company.CabVat != null)
                companyDto.VAT = company.CabVat.Vat;
            if (company.CabBankAccount != null)
                companyDto.BankAccount = company.CabBankAccount.BankAccount;
            if (company.WhatsApp != null)
                companyDto.WhatsApp = company.WhatsApp.WhatsAppNumber;
            companyDto.IsSaved = companyDto.IsSaved;
            if (company.Address != null)
            {
                var addressDto = new AddressDto
                {
                    Id = company.Address.CountryId,
                    Street = company.Address.Street,
                    MailBox = company.Address.MailBox,
                    PostalCode = company.Address.PostalCode,
                    City = company.Address.City,
                    Region = company.Address.Region,
                    StreetNumber = company.Address.StreetNumber,
                    CountryId = company.Address.CountryId
                };
                companyDto.Address = addressDto;
            }

            companyDtoList.Add(companyDto);
        }

        return companyDtoList;
    }
}