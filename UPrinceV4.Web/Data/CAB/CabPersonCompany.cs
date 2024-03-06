using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.CAB;

public class CabPersonCompany : MetaData
{
    public string Id { get; set; }
    [ForeignKey("CabPerson")] public string PersonId { get; set; }
    public virtual CabPerson Person { get; set; }
    [ForeignKey("CabCompany")] public string CompanyId { get; set; }
    public virtual CabCompany Company { get; set; }
    public string JobRole { get; set; }

    [ForeignKey("CabEmail")] public string EmailId { get; set; }
    public virtual CabEmail Email { get; set; }
    [ForeignKey("CabMobilePhone")] public string MobilePhoneId { get; set; }
    public virtual CabMobilePhone MobilePhone { get; set; }
    [ForeignKey("CabLandPhone")] public string LandPhoneId { get; set; }
    public virtual CabLandPhone LandPhone { get; set; }
    [ForeignKey("CabWhatsApp")] public string WhatsAppId { get; set; }
    public virtual CabWhatsApp WhatsApp { get; set; }
    [ForeignKey("CabSkype")] public string SkypeId { get; set; }
    public virtual CabSkype Skype { get; set; }
    public string Oid { get; set; }
}