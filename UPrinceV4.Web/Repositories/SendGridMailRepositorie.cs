using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class SendGridMailRepositorie : ISendGridMailRepositorie
{
    public SendGridMailRepositorie(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public async Task<bool> SendInvitation(SendGridMailParameter SendGridMailParameter)
    {
        CabPersonCompany CabPersonCompany;

        var checkOid =
            @"SELECT CabEmail.EmailAddress AS EmailId ,CabPersonCompany.Oid ,CabPerson.FullName AS PersonId FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.PersonId = @PersonId";

        var parm = new { PersonId = SendGridMailParameter.Id };
        using (var connection =
               new SqlConnection(SendGridMailParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            CabPersonCompany = connection.Query<CabPersonCompany>(checkOid, parm).FirstOrDefault();
        }

        if (CabPersonCompany != null)
        {
            var client =
                new SendGridClient("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");
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("shanukagayashan@gmail.com", "Example User"));
            msg.AddTo(new EmailAddress(CabPersonCompany.EmailId, "Example User"));
            msg.SetTemplateId("d-ddd343eb5e1b4b9e90a50cecf929050c");

            var dynamicTemplateData = new ExampleTemplateData
            {
                UserName = CabPersonCompany.PersonId,
                Link = SendGridMailParameter.Url,
                Lot = SendGridMailParameter.Lot,
                Project = SendGridMailParameter.ProjectSequenceId
            };

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }

        return false;
    }

    public async Task<bool> ReminderSendEmail(SendGridMailParameter SendGridMailParameter)
    {
        try
        {
            var mainconnectionString =
                "Server=tcp:bmengineering.database.windows.net,1433;Initial Catalog=bmengineering;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            var status = true;
            var issend = true;
            List<ProjectDefinition> mProjectDefinition = null;
            var project =
                @"SELECT ProjectDefinition.SequenceCode FROM dbo.ProjectDefinition WHERE ProjectDefinition.IsDeleted = 0 ORDER BY SequenceCode";
            using (var connection = new SqlConnection(mainconnectionString))
            {
                mProjectDefinition = connection.Query<ProjectDefinition>(project).ToList();
            }

            // foreach (var i in mProjectDefinition)
            // {
            //     var connectionString = "Server=tcp:bmengineering.database.windows.net,1433;Initial Catalog=" +
            //                            i.SequenceCode +
            //                            ";Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //
            //     string person = @"SELECT CabPersonId FROM dbo.ContractorTeamList WHERE Approve = '0'";
            //     
            //     List<ContractorTeamList> mContractorTeamList = null;
            //     using (SqlConnection connection = new SqlConnection(connectionString))
            //     {
            //         mContractorTeamList = connection.Query<ContractorTeamList>(person).ToList();
            //     }
            //
            //     foreach (var n in mContractorTeamList)
            //     {
            //         SendGridMailParameter.Id = n.CabPersonId;
            //         issend = await SendInvitation2(SendGridMailParameter,mainconnectionString);
            //         if (issend == false)
            //         {
            //             status = false;
            //         }
            //     }
            //     
            //     
            // }

            var connectionString = "Server=tcp:bmengineering.database.windows.net,1433;Initial Catalog=" +
                                   "P0088" +
                                   ";Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            var person = @"SELECT CabPersonId FROM dbo.ContractorTeamList";

            List<ContractorTeamList> mContractorTeamList = null;
            using (var connection = new SqlConnection(connectionString))
            {
                mContractorTeamList = connection.Query<ContractorTeamList>(person).ToList();
            }

            foreach (var n in mContractorTeamList)
            {
                SendGridMailParameter.Id = n.CabPersonId;
                issend = await SendInvitation2(SendGridMailParameter, mainconnectionString);
                if (issend == false) status = false;
            }

            return status;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> SendInvitation2(SendGridMailParameter SendGridMailParameter, string Connection)
    {
        CabPersonCompany CabPersonCompany;

        var checkOid =
            @"SELECT CabEmail.EmailAddress AS EmailId ,CabPersonCompany.Oid ,CabPerson.FullName AS PersonId FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.PersonId = @PersonId";

        var parm = new { PersonId = SendGridMailParameter.Id };
        using (var connection =
               new SqlConnection(Connection))
        {
            await connection.OpenAsync();
            CabPersonCompany = connection.Query<CabPersonCompany>(checkOid, parm).FirstOrDefault();
        }

        if (CabPersonCompany != null)
        {
            var client =
                new SendGridClient("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");
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("shanukagayashan@gmail.com", "Example User"));
            msg.AddTo(new EmailAddress(CabPersonCompany.EmailId, "Example User"));
            msg.SetTemplateId("d-ddd343eb5e1b4b9e90a50cecf929050c");

            var dynamicTemplateData = new ExampleTemplateData
            {
                UserName = CabPersonCompany.PersonId,
                Link = SendGridMailParameter.Url,
                Lot = SendGridMailParameter.Lot,
                Project = SendGridMailParameter.ProjectSequenceId
            };

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }

        return false;
    }

    public async Task<bool> MailService(SendGridMailParameter SendGridMailParameter)
    {
        CabPersonCompany CabPersonCompany;

        var checkOid =
            @"SELECT CabEmail.EmailAddress AS EmailId ,CabPersonCompany.Oid ,CabPerson.FullName AS PersonId FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.PersonId = @PersonId";

        var parm = new { PersonId = SendGridMailParameter.Id };
        using (var connection =
               new SqlConnection(SendGridMailParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            CabPersonCompany = connection.Query<CabPersonCompany>(checkOid, parm).FirstOrDefault();
        }

        if (CabPersonCompany != null)
        {
            var client =
                new SendGridClient("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");
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("shanukagayashan@gmail.com", "Example User"));
            msg.AddTo(new EmailAddress(CabPersonCompany.EmailId, "Example User"));
            msg.SetTemplateId(SendGridMailParameter.TemplateId);

            var dynamicTemplateData = new ExampleTemplateData
            {
                UserName = CabPersonCompany.PersonId,
                Link = SendGridMailParameter.Url,
                Lot = SendGridMailParameter.Lot,
                Project = SendGridMailParameter.ProjectSequenceId
            };

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }

        return false;
    }

    private class ExampleTemplateData
    {
        [JsonProperty("userName")] public string UserName { get; set; }

        [JsonProperty("link")] public string Link { get; set; }

        [JsonProperty("lot")] public string Lot { get; set; }

        [JsonProperty("project")] public string Project { get; set; }
    }
}