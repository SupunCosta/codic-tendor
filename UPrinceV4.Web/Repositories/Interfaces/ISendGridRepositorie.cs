using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.Contractor;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ISendGridRepositorie
{
    Task<bool> SendInvitation(SendGridParameter SendGridParameter);
    Task<bool> ReminderSendEmail(SendGridParameter SendGridParameter);
}

public class SendGridParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }
    public ITenantProvider TenantProvider { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Url { get; set; }
    public string UserName { get; set; }
    public string Lot { get; set; }
    public string Email { get; set; }
    public string TemplateId { get; set; }
    public string Content { get; set; }
    public string Server { get; set; }
    public GetContractorByIdForMailDto GetContractorByIdForMailDto { get; set; }
    public string MailBody { get; set; }
    public IConfiguration Configuration { get; set; }
    public string LotTitle { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Subject { get; set; }
    public string ButtonText { get; set; }
    public string StatusImage { get; set; }
    public string DisplayImage { get; set; } = "none";
    public string DisplayBtn { get; set; } = "none";
    public string ProjLang { get; set; }

}