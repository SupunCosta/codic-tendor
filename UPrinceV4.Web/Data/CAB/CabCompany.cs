using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Graph.Models;

namespace UPrinceV4.Web.Data.CAB;

public class CabCompany : MetaData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SequenceCode { get; set; }
    [ForeignKey("CabVat")] public string VatId { get; set; }
    public virtual CabVat CabVat { get; set; }
    [ForeignKey("CabBankAccount")] public string BankAccountId { get; set; }
    public virtual CabBankAccount CabBankAccount { get; set; }
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
    public string ContractingUnitDbNameId { get; set; }
    [NotMapped] public ICollection<CabPerson> PersonList { get; set; }
    [NotMapped] public CabHistoryLogDto History { get; set; }
    public bool IsContractingUnit { get; set; }
    public string AccountingNumber { get; set; }
    [NotMapped] public List<string> ContractorTaxonomyId { get; set; }
}

public class CabCompanyDto
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string SequenceCode { get; set; }
}

public class ContractingUnitDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
}

public class UserLoginDetailsDto
{
    public ApplicationUser User { get; set; }
    public List<ContractingUnitDto> ContractingUnits { get; set; }
    public IEnumerable<AllProjectAttributes> Projects { get; set; }
   public List<DirectoryObject>  groups { get; set; }

    public string profileImage { get; set; }
}