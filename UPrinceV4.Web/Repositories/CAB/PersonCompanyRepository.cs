using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.CAB;

public class PersonCompanyRepository : IPersonCompanyRepository
{
    public async Task<IEnumerable<CabPersonCompany>> GetPersonCompanyList(
        PersonCompanyRepositoryParameter personCompanyRepositoryParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<CabPerson> GetPersonCompanyById(
        PersonCompanyRepositoryParameter personCompanyRepositoryParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddPersonCompany(PersonCompanyRepositoryParameter personCompanyRepositoryParameter)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var dbContext =
                new ApplicationDbContext(options, personCompanyRepositoryParameter.TenantProvider);

            var personCompany = personCompanyRepositoryParameter.PersonCompany;
            if (personCompany.Id == null)
            {
                personCompany.Id = Guid.NewGuid().ToString();
                if (personCompany.Email != null)
                {
                    if (personCompany.Email.EmailAddress != null)
                    {
                        var existingEmail =
                            dbContext.CabEmail.FirstOrDefault(e =>
                                e.EmailAddress == personCompany.Email.EmailAddress && e.IsDeleted == true);
                        if (existingEmail != null)
                        {
                            personCompany.EmailId = existingEmail.Id;
                            personCompany.Email = existingEmail;
                            existingEmail.IsDeleted = false;
                            dbContext.CabEmail.Update(existingEmail);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var email = new CabEmail
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmailAddress = personCompany.Email.EmailAddress
                            };
                            personCompany.EmailId = email.Id;
                            personCompany.Email = email;
                            personCompanyRepositoryParameter.ApplicationDbContext.CabEmail.Add(email);
                            personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        personCompany.Email = null;
                    }
                }

                if (personCompany.LandPhone != null)
                {
                    if (personCompany.LandPhone.LandPhoneNumber != null)
                    {
                        var existingLangPhone =
                            dbContext.CabLandPhone.FirstOrDefault(
                                l => l.LandPhoneNumber == personCompany.LandPhone.LandPhoneNumber &&
                                     l.IsDeleted == true);
                        if (existingLangPhone != null)
                        {
                            personCompany.LandPhoneId = existingLangPhone.Id;
                            personCompany.LandPhone = existingLangPhone;
                            existingLangPhone.IsDeleted = false;
                            dbContext.CabLandPhone.Update(existingLangPhone);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var landPhone = new CabLandPhone
                            {
                                Id = Guid.NewGuid().ToString(),
                                LandPhoneNumber = personCompany.LandPhone.LandPhoneNumber
                            };
                            personCompany.LandPhoneId = landPhone.Id;
                            personCompany.LandPhone = landPhone;
                            personCompanyRepositoryParameter.ApplicationDbContext.CabLandPhone.Add(landPhone);
                            personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        personCompany.LandPhone = null;
                    }
                }

                if (personCompany.MobilePhone != null)
                {
                    if (personCompany.MobilePhone.MobilePhoneNumber != null)
                    {
                        var existingMobilePhone = dbContext.CabMobilePhone.FirstOrDefault(m =>
                            m.MobilePhoneNumber == personCompany.MobilePhone.MobilePhoneNumber &&
                            m.IsDeleted == true);
                        if (existingMobilePhone != null)
                        {
                            personCompany.MobilePhoneId = existingMobilePhone.Id;
                            personCompany.MobilePhone = existingMobilePhone;
                            existingMobilePhone.IsDeleted = false;
                            dbContext.CabMobilePhone.Update(existingMobilePhone);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var mobilePhone = new CabMobilePhone
                            {
                                Id = Guid.NewGuid().ToString(),
                                MobilePhoneNumber = personCompany.MobilePhone.MobilePhoneNumber
                            };
                            personCompany.MobilePhoneId = mobilePhone.Id;
                            personCompany.MobilePhone = mobilePhone;
                            personCompanyRepositoryParameter.ApplicationDbContext.CabMobilePhone.Add(mobilePhone);
                            personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        personCompany.MobilePhone = null;
                    }
                }

                if (personCompany.WhatsApp != null)
                {
                    if (personCompany.WhatsApp.WhatsAppNumber != null)
                    {
                        var existingWhatsApp = dbContext.CabWhatsApp.FirstOrDefault(w =>
                            w.WhatsAppNumber == personCompany.WhatsApp.WhatsAppNumber &&
                            w.IsDeleted == true);
                        if (existingWhatsApp != null)
                        {
                            personCompany.WhatsAppId = existingWhatsApp.Id;
                            personCompany.WhatsApp = existingWhatsApp;
                            existingWhatsApp.IsDeleted = false;
                            dbContext.CabWhatsApp.Update(existingWhatsApp);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var whatsapp = new CabWhatsApp
                            {
                                Id = Guid.NewGuid().ToString(),
                                WhatsAppNumber = personCompany.WhatsApp.WhatsAppNumber
                            };
                            personCompany.WhatsAppId = whatsapp.Id;
                            personCompany.WhatsApp = whatsapp;
                            personCompanyRepositoryParameter.ApplicationDbContext.CabWhatsApp.Add(whatsapp);
                            personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        personCompany.WhatsApp = null;
                    }
                }

                if (personCompany.Skype != null)
                {
                    if (personCompany.Skype.SkypeNumber != null)
                    {
                        var existingSkype = dbContext.CabSkype.FirstOrDefault(s =>
                            s.SkypeNumber == personCompany.Skype.SkypeNumber && s.IsDeleted == true);
                        if (existingSkype != null)
                        {
                            personCompany.SkypeId = existingSkype.Id;
                            personCompany.Skype = existingSkype;
                            existingSkype.IsDeleted = false;
                            dbContext.CabSkype.Update(existingSkype);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var skype = new CabSkype
                            {
                                Id = Guid.NewGuid().ToString(),
                                SkypeNumber = personCompany.Skype.SkypeNumber
                            };
                            personCompany.SkypeId = skype.Id;
                            personCompany.Skype = skype;
                            personCompanyRepositoryParameter.ApplicationDbContext.CabSkype.Add(skype);
                            personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        personCompany.Skype = null;
                    }
                }

                personCompanyRepositoryParameter.ApplicationDbContext.CabPersonCompany.Add(
                    personCompanyRepositoryParameter.PersonCompany);
                personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();

                // Add History data
                var history = personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter;
                history.TenantProvider = personCompanyRepositoryParameter.TenantProvider;
                history.Action = HistoryState.ADDED.ToString();
                history.Company.Id = personCompany.CompanyId;
                history.Person.Id = personCompany.PersonId;
                var historyRepository =
                    personCompanyRepositoryParameter.CabHistoryLogRepository.AddCabHistoryLog(history);

                var jsonPerson = JsonConvert.SerializeObject(personCompany, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                personCompanyRepositoryParameter.Logger.LogError("CreateCabEntry personCompany created " +
                                                                 jsonPerson);

                return personCompany.Id;
            }

            var updatedPersonCompany =
                dbContext.CabPersonCompany.FirstOrDefault(pc => pc.Id == personCompany.Id);
            if (updatedPersonCompany != null)
            {
                updatedPersonCompany.CompanyId = personCompany.CompanyId;
                updatedPersonCompany.PersonId = personCompany.PersonId;
                updatedPersonCompany.JobRole = personCompany.JobRole;

                if (personCompany.Email.EmailAddress != null)
                {
                    var updatedEmail =
                        dbContext.CabEmail.FirstOrDefault(a => a.Id == updatedPersonCompany.EmailId);
                    if (updatedEmail == null)
                    {
                        var deletedEmail = dbContext.CabEmail.FirstOrDefault(a =>
                            a.EmailAddress == personCompany.Email.EmailAddress
                            && a.IsDeleted == true);
                        if (deletedEmail != null)
                        {
                            deletedEmail.IsDeleted = false;
                            updatedPersonCompany.EmailId = deletedEmail.Id;
                            updatedPersonCompany.Email = deletedEmail;
                            personCompanyRepositoryParameter.ApplicationDbContext.CabEmail.Update(deletedEmail);
                            personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                        }
                        else
                        {
                            var email = new CabEmail
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmailAddress = personCompany.Email.EmailAddress
                            };

                            updatedPersonCompany.EmailId = email.Id;
                            updatedPersonCompany.Email = email;
                            personCompanyRepositoryParameter.ApplicationDbContext.CabEmail.Add(email);
                            personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        updatedEmail.EmailAddress = personCompany.Email.EmailAddress;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabEmail.Update(updatedEmail);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                }
                else
                {
                    //if (updatedPersonCompany.Email != null)
                    //{
                    var email = dbContext.CabEmail.FirstOrDefault(e => e.Id == updatedPersonCompany.EmailId);
                    if (email != null)
                    {
                        email.IsDeleted = true;
                        dbContext.CabEmail.Update(email);
                        dbContext.SaveChanges();
                        updatedPersonCompany.Email = null;
                        updatedPersonCompany.EmailId = null;
                    }
                    //}
                }

                if (personCompany.LandPhone.LandPhoneNumber != null)
                {
                    var updatedLandPhone =
                        dbContext.CabLandPhone.FirstOrDefault(a => a.Id == updatedPersonCompany.LandPhoneId);
                    if (updatedLandPhone == null)
                    {
                        var landPhone = new CabLandPhone
                        {
                            Id = Guid.NewGuid().ToString(),
                            LandPhoneNumber = personCompany.LandPhone.LandPhoneNumber
                        };

                        updatedPersonCompany.LandPhoneId = landPhone.Id;
                        updatedPersonCompany.LandPhone = landPhone;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabLandPhone.Add(landPhone);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                    else
                    {
                        updatedLandPhone.LandPhoneNumber = personCompany.LandPhone.LandPhoneNumber;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabLandPhone.Update(
                            updatedLandPhone);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                }
                else
                {
                    //if (updatedPersonCompany.LandPhone != null)
                    //{
                    var landPhone =
                        dbContext.CabLandPhone.FirstOrDefault(e => e.Id == updatedPersonCompany.LandPhoneId);
                    if (landPhone != null)
                    {
                        landPhone.IsDeleted = true;
                        dbContext.CabLandPhone.Update(landPhone);
                        dbContext.SaveChanges();
                        updatedPersonCompany.LandPhone = null;
                        updatedPersonCompany.LandPhoneId = null;
                    }
                    //}
                }

                if (personCompany.MobilePhone.MobilePhoneNumber != null)
                {
                    var updatedMobilePhone =
                        dbContext.CabMobilePhone.FirstOrDefault(a =>
                            a.Id == updatedPersonCompany.MobilePhoneId);
                    if (updatedMobilePhone == null)
                    {
                        var mobilePhone = new CabMobilePhone
                        {
                            Id = Guid.NewGuid().ToString(),
                            MobilePhoneNumber = personCompany.MobilePhone.MobilePhoneNumber
                        };

                        updatedPersonCompany.MobilePhoneId = mobilePhone.Id;
                        updatedPersonCompany.MobilePhone = mobilePhone;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabMobilePhone.Add(mobilePhone);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                    else
                    {
                        updatedMobilePhone.MobilePhoneNumber = personCompany.MobilePhone.MobilePhoneNumber;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabMobilePhone.Update(
                            updatedMobilePhone);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                }
                else
                {
                    //if (updatedPersonCompany.MobilePhone != null)
                    //{
                    var mobilePhone =
                        dbContext.CabMobilePhone.FirstOrDefault(e =>
                            e.Id == updatedPersonCompany.MobilePhoneId);
                    if (mobilePhone != null)
                    {
                        mobilePhone.IsDeleted = true;
                        dbContext.CabMobilePhone.Update(mobilePhone);
                        dbContext.SaveChanges();
                        updatedPersonCompany.MobilePhone = null;
                        updatedPersonCompany.MobilePhoneId = null;
                    }
                    //}
                }

                if (personCompany.WhatsApp.WhatsAppNumber != null)
                {
                    var updatedWhatsApp =
                        dbContext.CabWhatsApp.FirstOrDefault(a => a.Id == updatedPersonCompany.WhatsAppId);
                    if (updatedWhatsApp == null)
                    {
                        var whatsApp = new CabWhatsApp
                        {
                            Id = Guid.NewGuid().ToString(),
                            WhatsAppNumber = personCompany.WhatsApp.WhatsAppNumber
                        };

                        updatedPersonCompany.WhatsAppId = whatsApp.Id;
                        updatedPersonCompany.WhatsApp = whatsApp;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabWhatsApp.Add(whatsApp);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                    else
                    {
                        updatedWhatsApp.WhatsAppNumber = personCompany.WhatsApp.WhatsAppNumber;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabWhatsApp.Update(
                            updatedWhatsApp);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                }
                else
                {
                    var whatsApp =
                        dbContext.CabWhatsApp.FirstOrDefault(e => e.Id == updatedPersonCompany.WhatsAppId);
                    if (whatsApp != null)
                    {
                        whatsApp.IsDeleted = true;
                        dbContext.CabWhatsApp.Update(whatsApp);
                        dbContext.SaveChanges();
                        updatedPersonCompany.WhatsApp = null;
                        updatedPersonCompany.WhatsAppId = null;
                    }
                }

                if (personCompany.Skype.SkypeNumber != null)
                {
                    var updatedSkype =
                        dbContext.CabSkype.FirstOrDefault(a => a.Id == updatedPersonCompany.SkypeId);
                    if (updatedSkype == null)
                    {
                        var skype = new CabSkype
                        {
                            Id = Guid.NewGuid().ToString(),
                            SkypeNumber = personCompany.Skype.SkypeNumber
                        };

                        updatedPersonCompany.SkypeId = skype.Id;
                        updatedPersonCompany.Skype = skype;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabSkype.Add(skype);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                    else
                    {
                        updatedSkype.SkypeNumber = personCompany.Skype.SkypeNumber;
                        personCompanyRepositoryParameter.ApplicationDbContext.CabSkype.Update(updatedSkype);
                        personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();
                    }
                }
                else
                {
                    var skype = dbContext.CabSkype.FirstOrDefault(e => e.Id == updatedPersonCompany.SkypeId);
                    if (skype != null)
                    {
                        skype.IsDeleted = true;
                        dbContext.CabSkype.Update(skype);
                        dbContext.SaveChanges();
                        updatedPersonCompany.SkypeId = null;
                        updatedPersonCompany.Skype = null;
                    }
                }

                personCompanyRepositoryParameter.ApplicationDbContext.CabPersonCompany.Update(
                    updatedPersonCompany);
                personCompanyRepositoryParameter.ApplicationDbContext.SaveChanges();

                // Add History data
                var history = personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter;
                history.Action = HistoryState.UPDATED.ToString();
                history.Company.Id = personCompanyRepositoryParameter.PersonCompany.CompanyId;
                history.Person.Id = personCompanyRepositoryParameter.PersonCompany.PersonId;
                var historyRepository =
                    personCompanyRepositoryParameter.CabHistoryLogRepository.AddCabHistoryLog(history);

                var jsonPerson = JsonConvert.SerializeObject(updatedPersonCompany, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                personCompanyRepositoryParameter.Logger.LogError("CreateCabEntry personCompany updated " +
                                                                 jsonPerson);

                return updatedPersonCompany.Id;
            }

            return "Cab PersonCompany Id is invalid";
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }

    public async Task<bool> DeletePersonCompany(PersonCompanyRepositoryParameter personCompanyRepositoryParameter)
    {
        throw new NotImplementedException();
    }
}