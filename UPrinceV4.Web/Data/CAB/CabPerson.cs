using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CAB;

public class CabPerson : MetaData
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string FullName { get; set; }
    public string CallName { get; set; }
    [ForeignKey("CabEmail")] public string BusinessEmailId { get; set; }
    public virtual CabEmail BusinessEmail { get; set; }
    [ForeignKey("CabMobilePhone")] public string BusinessPhoneId { get; set; }
    public virtual CabMobilePhone BusinessPhone { get; set; }
    public string Image { get; set; }
    [ForeignKey("CabAddress")] public string AddressId { get; set; }
    public virtual CabAddress Address { get; set; }
    [ForeignKey("CabEmail")] public string EmailId { get; set; }
    public virtual CabEmail Email { get; set; }
    [ForeignKey("CabLandPhone")] public string LandPhoneId { get; set; }
    public virtual CabLandPhone LandPhone { get; set; }
    [ForeignKey("CabMobilePhone")] public string MobilePhoneId { get; set; }
    public virtual CabMobilePhone MobilePhone { get; set; }
    [ForeignKey("CabWhatsApp")] public string WhatsAppId { get; set; }
    public virtual CabWhatsApp WhatsApp { get; set; }
    [ForeignKey("CabSkype")] public string SkypeId { get; set; }
    public virtual CabSkype Skype { get; set; }
    public ICollection<CabCompany> CompanyList { get; set; }
    public ICollection<CabPersonCompany> PersonCompanyList { get; set; }

    [NotMapped] public CabHistoryLogDto History { get; set; }
    [NotMapped] public string NationalityId { get; set; }
}

public class CabDataDto
{
    public string key { get; set; }
    public PersonDto Person { get; set; }
    public string CompanyId { get; set; }
    public CompanyDto Company { get; set; }
    public PersonCompanyDto PersonCompany { get; set; }
    public bool IsSaved { get; set; }
    public CabHistoryLogDto History { get; set; }
}

internal class CabDataDapperDto
{
    public string PersonId { get; set; }
    public string FullName { get; set; }
    public string JobTitle { get; set; }
    public string Organisation { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public bool IsSaved { get; set; }
    public string CompanyId { get; set; }
    public string PersonCompanyId { get; set; }
}

public class PersonDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string FullName { get; set; }
    public string CallName { get; set; }
    public string Image { get; set; }
    public AddressDto Address { get; set; }
    public string BusinessEmail { get; set; }
    public string BusinessPhone { get; set; }
    public string Email { get; set; }
    public string LandPhone { get; set; }
    public string MobilePhone { get; set; }
    public string Whatsapp { get; set; }
    public string Skype { get; set; }
    public string NationalityId { get; set; }
    public NationalityDto Nationality { get; set; }
}

public class PersonDtoDapper
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobilePhone { get; set; }
    public bool IsSaved { get; set; }
}

public class CompanyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string VAT { get; set; }
    public string BankAccount { get; set; }
    public AddressDto Address { get; set; }
    public string Email { get; set; }
    public string LandPhone { get; set; }
    public string MobilePhone { get; set; }
    public string WhatsApp { get; set; }
    public string Skype { get; set; }
    public CabHistoryLogDto History { get; set; }
    public bool IsSaved { get; set; }
    public string AccountingNumber { get; set; }
    public List<string> ContractorTaxonomyId { get; set; }
}

public class GroupByCompanyDto
{
    public GroupByCompanyDto()
    {
        PersonList = new List<PersonDtoDapper>();
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public List<PersonDtoDapper> PersonList { get; set; }
}

public class GroupByCompanyLoadingDto
{
    public string CompanyId { get; set; }
    public string FirstName { get; set; }
    public string FullName { get; set; }
    public string Name { get; set; }
    public string PersonId { get; set; }
    public string Email { get; set; }
    public string MobilePhone { get; set; }
    public string Cid { get; set; }
    public bool IsSaved { get; set; }
}

public class UnassignedCompanyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class PersonCompanyDto
{
    public string Id { get; set; }
    public string JobRole { get; set; }
    public string Email { get; set; }
    public string MobilePhone { get; set; }
    public string LandPhone { get; set; }
    public string Whatsapp { get; set; }
    public string Skype { get; set; }
    public string PersonId { get; set; }
    public string CompanyId { get; set; }
}

public class AddressDto
{
    public string Id { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string MailBox { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string CountryId { get; set; }
}

public class CabIdDataDto
{
    public string PersonId { get; set; }
    public string PersonCompanyId { get; set; }
}

public class CabPersonNameFilterDto
{
    public string Key { get; set; }
    public string Text { get; set; }
}

public class ProjectPersonFilterDto
{
    public bool IsSaved { get; set; }
    public string CompanyId { get; set; }
    public ProjectPersonFilterPersonDto Person { get; set; }
    public ProjectPersonFilterPersonCompanyDto PersonCompany { get; set; }
    public ProjectPersonFilterCompanyDto Company { get; set; }
}

public class ProjectPersonFilterPersonDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string PersonId { get; set; }
}

public class ProjectPersonFilterPersonCompanyDto
{
    public string Id { get; set; }
    public string JobTitle { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
}

public class ProjectPersonFilterCompanyDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}