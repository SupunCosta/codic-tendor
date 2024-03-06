using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Controllers.SendGrid;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class SendGridController : CommonConfigurationController
{
    private readonly ISendGridRepositorie _iSendGridRepositorie;
    private readonly SendGridParameter _sendGridParameter;
    private readonly ITenantProvider _TenantProvider;
    private IConfiguration Configuration;


    public SendGridController(ITenantProvider tenantProvider,
        ApplicationDbContext uPrinceCustomerContext, IHttpContextAccessor contextAccessor, ApiResponse apiResponse,
        ApiBadRequestResponse apiBadRequestResponse, ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        IConfiguration configuration, SendGridParameter sendGridParameter, ISendGridRepositorie iSendGridRepositorie,
        IConfiguration iConfiguration)
        : base(uPrinceCustomerContext, contextAccessor, apiResponse, apiBadRequestResponse, apiOkResponse,
            iTenantProvider)
    {
        Configuration = configuration;
        _TenantProvider = iTenantProvider;
        _sendGridParameter = sendGridParameter;
        _iSendGridRepositorie = iSendGridRepositorie;
        _iConfiguration = iConfiguration;
    }

    private IConfiguration _iConfiguration { get; }

    // [AllowAnonymous]
    // [HttpGet("SendEmail")]
    // public async Task<ActionResult> SendEmail()
    // {
    //     try
    //     {
    //         var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
    //         var client = new SendGridClient("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");
    //         var from = new EmailAddress("npkarunarathne@gmail.com", "Example User");
    //         var subject = "Sending with SendGrid is Fun";
    //         var to = new EmailAddress("shanuka@mickiesoft.com", "Example User");
    //         var plainTextContent = "and easy to do anywhere, even with C#";
    //         string htmlbody = "<html>\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n<script type=\"application/adaptivecard+json\">\n{\n    \"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\",\n    \"type\": \"AdaptiveCard\",\n    \"version\": \"1.0\",\n    \"originator\": \"\",\n    \"body\": [\n        {\n            \"size\": \"large\",\n            \"text\": \"Hello Actionable message\",\n            \"wrap\": true,\n            \"type\": \"TextBlock\"\n        }\n    ],\n    \"actions\": [\n        {\n            \"type\": \"Action.InvokeAddInCommand\",\n            \"title\": \"Open Actionable Messages Debugger\",\n            \"addInId\": \"3d1408f6-afb3-4baf-aacd-55cd867bb0fa\",\n            \"desktopCommandId\": \"amDebuggerOpenPaneButton\"\n        }\n    ]\n}\n</script>\n</head>\nIf the card doesn&#39;t appear, <a target=\"_blank\" href=\"https://store.office.com/app.aspx?assetid=WA104381686&productgroup=Outlook&mktvid=PN102957641&mktcmpid=sendmailsample\"/>install Actionable Messages Debugger Outlook add-in</a> to debug the issue.\n<body>\n</body>\n</html>";
    //         var htmlContent = htmlbody;
    //         var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
    //         var response = await client.SendEmailAsync(msg);
    //
    //         return Ok(new ApiOkResponse(response));
    //     }
    //     catch (Exception e)
    //     {
    //         //Console.WriteLine(e);
    //         throw;
    //     }
    // }

    [AllowAnonymous]
    [HttpGet("SendEmail")]
    public async Task<ActionResult> SendEmail()
    {
        try
        {
            var message = new SendGridMessage();

            message.Personalizations = new List<Personalization>
            {
                new()
                {
                    Tos = new List<EmailAddress>
                    {
                        new()
                        {
                            Email = "emiel@mickiesoft.com",
                            Name = "Emiel"
                        },
                        new()
                        {
                            Email = "npkarunarathne@gmail.com",
                            Name = "Nimesh"
                        }
                    }
                }
            };

            message.From = new EmailAddress
            {
                Email = "nimesh@mickiesoft.com",
                Name = "Example Order Confirmation"
            };

            message.ReplyTo = new EmailAddress
            {
                Email = "customer_service@example.com",
                Name = "Example Customer Service Team"
            };

            message.Subject = "Testing";

            message.Contents = new List<Content>
            {
                new()
                {
                    Type = "text/html",
                    Value = "<strong>and easy to do anywhere, even with C#</strong>"
                }
            };


            var apiKey =
                Environment.GetEnvironmentVariable(
                    "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");
            var client = new SendGridClient("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");
            var response = await client.SendEmailAsync(message).ConfigureAwait(false);
            return Ok(new ApiOkResponse(response));
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    [AllowAnonymous]
    [HttpGet("ReminderSendEmail")]
    public async Task<ActionResult> ReminderSendEmail()
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _sendGridParameter.TenantProvider = _TenantProvider;
            _sendGridParameter.Lang = lang;
            _sendGridParameter.Configuration = _iConfiguration;
            var response = await _iSendGridRepositorie.ReminderSendEmail(_sendGridParameter);
            return Ok(new ApiOkResponse(response));
        }
        catch (Exception e)
        {
            return BadRequest(new ApiResponse(400, false, e.Message));
            throw;
        }
    }
}