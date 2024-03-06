using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ISendGridMailRepositorie
{
    Task<bool> SendInvitation(SendGridMailParameter SendGridMailParameter);
    Task<bool> ReminderSendEmail(SendGridMailParameter SendGridMailParameter);
}

public class SendGridMailParameter
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
}