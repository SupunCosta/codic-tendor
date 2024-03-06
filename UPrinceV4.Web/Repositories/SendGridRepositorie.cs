using System;
using System.Collections.Generic;
using System.Data;
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
using UPrinceV4.Web.Data.StandardMails;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class SendGridRepositorie : ISendGridRepositorie
{
    public async Task<bool> SendInvitation(SendGridParameter SendGridParameter)
    {
        CabPersonCompany CabPersonCompany;

        var checkOid =
            @"SELECT CabEmail.EmailAddress AS EmailId ,CabPersonCompany.Oid ,CabPerson.FullName AS PersonId FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.PersonId = @PersonId AND CabPerson.IsDeleted = 0";

        var parm = new { PersonId = SendGridParameter.Id };

        using (var connection =
               new SqlConnection(SendGridParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            CabPersonCompany = connection.Query<CabPersonCompany>(checkOid, parm).FirstOrDefault();
        }

        var apikey = SendGridParameter.Configuration.GetValue<string>("SENDGRID_API_KEY");
        var templateId_en = SendGridParameter.Configuration.GetValue<string>("TemplateId_en");
        var templateId_nl = SendGridParameter.Configuration.GetValue<string>("TemplateId_nl");
        var email = SendGridParameter.Configuration.GetValue<string>("Reminder_Email");
        var emailName = SendGridParameter.Configuration.GetValue<string>("Reminder_Email_Name");

        if (CabPersonCompany != null)
        {
            var client =
                new SendGridClient(apikey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(email, emailName));
            msg.AddTo(new EmailAddress(CabPersonCompany.EmailId, CabPersonCompany.PersonId));

            if (SendGridParameter.TemplateId == null)
            {
                // if (SendGridParameter.Lang == "en") msg.SetTemplateId(templateId_en);
                //
                // if (SendGridParameter.Lang == "nl") msg.SetTemplateId(templateId_nl);

                switch (SendGridParameter.ProjLang)
                {
                    case "en":
                    case null:
                        msg.SetTemplateId(templateId_en);
                        break;
                    case "nl":
                        msg.SetTemplateId(templateId_nl);
                        break;
                }
            }
            else
            {
                msg.SetTemplateId(SendGridParameter.TemplateId);
            }

            SendGridParameter.GetContractorByIdForMailDto = await GetProjectData(SendGridParameter, "connetion");

            var standardMail =
                @"SELECT Id ,MailHeader ,Title ,SequenceId ,RequestToWrittenInTender ,MeasuringStateRecieved ,Reminder1 ,Reminder1TimeFrameTender ,Reminder2 ,Reminder2TimeFrameTender ,Reminder3 ,Reminder3TimeFrameTender ,TenderWon ,TenderLost ,OutStandingComments ,CreatedDate ,Createdby ,ModifiedDate ,Modifiedby ,Name FROM dbo.StandardMailHeader where SequenceId = 'SM-0026';";

            StandardMailHeader mStandardMailHeader;

            var cuConnection = ConnectionString.MapConnectionString(
                SendGridParameter.GetContractorByIdForMailDto.Cu,
                null, SendGridParameter.TenantProvider);

            using (var connection = new SqlConnection(cuConnection))
            {
                await connection.OpenAsync();
                mStandardMailHeader = connection.Query<StandardMailHeader>(standardMail).FirstOrDefault();
            }

            var dynamicTemplateData = new ExampleTemplateDataNew
            {
                Date = SendGridParameter.GetContractorByIdForMailDto.EndDate,
                Code = SendGridParameter.GetContractorByIdForMailDto.Name,
                Services = SendGridParameter.GetContractorByIdForMailDto.ContractingUnit,
                Sector = SendGridParameter.GetContractorByIdForMailDto.Sector,
                Architect = SendGridParameter.GetContractorByIdForMailDto.Architect,
                Builder = SendGridParameter.GetContractorByIdForMailDto.Customer,
                AdditinalInfo = SendGridParameter.GetContractorByIdForMailDto.Description,
                EmailContent = SendGridParameter.MailBody,
                EmailContentHeader = SendGridParameter.Subject,
                Link = SendGridParameter.Url,
                LotTitle = SendGridParameter.LotTitle,
                StartDate = SendGridParameter.StartDate,
                EndDate = SendGridParameter.EndDate,
                Subject = SendGridParameter.Subject,
                ButtonText = SendGridParameter.ButtonText,
                StatusImage = SendGridParameter.StatusImage,
                DisplayImage = SendGridParameter.DisplayImage,
                ProjectLanguage = SendGridParameter.ProjLang
            };

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }

        return false;
    }

    public async Task<bool> ReminderSendEmail(SendGridParameter sendGridParameter)
    {
        try
        {
            var tenetConnection = new SqlConnection(sendGridParameter.TenantProvider.GetTenant().ConnectionString);

            var status = true;
            var issend = true;
            var url = sendGridParameter.Configuration.GetValue<string>("DomainUrl") + "/CU/";

            List<ProjectDefinition> mProjectDefinition = null;

            var project =
                @"SELECT ProjectDefinition.SequenceCode,ProjectDefinition.ProjectConnectionString,ProjectDefinition.Name FROM dbo.ProjectDefinition WHERE ProjectDefinition.IsDeleted = 0 ORDER BY SequenceCode";

            using (var connection = new SqlConnection(sendGridParameter.TenantProvider.GetTenant().ConnectionString))
            {
                mProjectDefinition = connection.Query<ProjectDefinition>(project).ToList();
            }

            foreach (var i in mProjectDefinition)
            {
                sendGridParameter.ProjectSequenceId = i.SequenceCode;

                var person =
                    @"SELECT ContractorTeamList.InvitationId,ContractorHeader.ReminderFourDate ,ContractorHeader.ReminderOneDate ,ContractorHeader.ReminderThreeDate ,ContractorHeader.ReminderTwoDate,* FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorTeamList.InvitationId IS NOT NULL;";

                List<ContractorTeamListDto> mContractorTeamList = null;

                using (var connection = new SqlConnection(i.ProjectConnectionString))
                {
                    mContractorTeamList = connection.Query<ContractorTeamListDto>(person).ToList();
                    //mContractorTeamList = (List<ContractorTeamListDto>) mContractorTeamList.Where(r => r.InvitationId != null);
                    if (mContractorTeamList.Any())
                    {
                        sendGridParameter.GetContractorByIdForMailDto =
                            await GetProjectData(sendGridParameter, i.ProjectConnectionString);

                        var standardMail =
                            @"SELECT Id ,MailHeader ,Title ,SequenceId ,RequestToWrittenInTender ,MeasuringStateRecieved ,Reminder1 ,Reminder1TimeFrameTender ,Reminder2 ,Reminder2TimeFrameTender ,Reminder3 ,Reminder3TimeFrameTender ,TenderWon ,TenderLost ,OutStandingComments ,CreatedDate ,Createdby ,ModifiedDate ,Modifiedby ,Name FROM dbo.StandardMailHeader WHERE IsDefault = 1;";

                        var wfId =
                            @"SELECT SequenceId FROM dbo.ConstructorWorkFlow WHERE Lot = @lot AND CabCompanyId = @Company;";

                        var cuConnection = ConnectionString.MapConnectionString(
                            sendGridParameter.GetContractorByIdForMailDto.Cu,
                            null, sendGridParameter.TenantProvider);
                        StandardMailHeader mStandardMailHeader;

                        foreach (var n in mContractorTeamList)
                            if (n.InvitationId != null)
                            {
                                var wfStatus = connection
                                    .Query<string>(
                                        "SELECT STatus FROM dbo.ConstructorWorkFlow WHERE Lot = @lot AND CabCompanyId = @Company",
                                        new { lot = n.LotContractorId, Company = n.CompanyId }).FirstOrDefault();
                                var query =
                                    @"SELECT ContractorHeader.StandardMailId,ContractorHeader.Title,ContractorHeader.StartDate,ContractorHeader.EndDate,ContractorHeader.Name FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE InvitationId = @Id";

                                ContractorHeader smail;

                                smail = connection.Query<ContractorHeader>(query, new { Id = n.InvitationId })
                                    .FirstOrDefault();

                                if (smail != null && smail.StandardMailId != null)
                                    using (var cuconnection = new SqlConnection(cuConnection))
                                    {
                                        await cuconnection.OpenAsync();

                                        mStandardMailHeader = cuconnection
                                            .Query<StandardMailHeader>(
                                                "SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id;",
                                                new { Id = smail.StandardMailId }).FirstOrDefault();

                                        if (cuconnection.State != ConnectionState.Closed) connection.Close();
                                    }
                                else
                                    using (var cuconnection = new SqlConnection(cuConnection))
                                    {
                                        await cuconnection.OpenAsync();

                                        mStandardMailHeader = cuconnection.Query<StandardMailHeader>(standardMail)
                                            .FirstOrDefault();

                                        if (cuconnection.State != ConnectionState.Closed) connection.Close();
                                    }
                                var projLang = tenetConnection.Query<string>(
                                    "SELECT pl.Name FROM ProjectDefinition LEFT OUTER JOIN ProjectLanguage pl ON ProjectDefinition.Language = pl.TypeId WHERE ProjectDefinition.SequenceCode = @Id",
                                    new { Id = sendGridParameter.ProjectSequenceId }).FirstOrDefault();
                                
                                if (n.ReminderOneDate != null && n.Approve == "0")
                                    if (n.ReminderOneDate.Value.Date == DateTime.Now.Date)
                                    {
                                        n.ConstructorWorkFlowSequenceId = connection
                                            .Query<string>(wfId, new { lot = n.LotContractorId, Company = n.CompanyId })
                                            .FirstOrDefault();
                                        sendGridParameter.Id = n.CabPersonId;
                                        sendGridParameter.MailBody = mStandardMailHeader.Reminder1;
                                        sendGridParameter.LotTitle = smail.Name;
                                        sendGridParameter.StartDate = smail.StartDate?.ToString("dd/MM/yyyy");
                                        sendGridParameter.EndDate = smail.EndDate?.ToString("dd/MM/yyyy");
                                        sendGridParameter.Subject = smail.Title + " " + "Reminder for Accept";
                                        sendGridParameter.Url = url +
                                                                sendGridParameter.GetContractorByIdForMailDto.Cu +
                                                                "/project/" +
                                                                sendGridParameter.GetContractorByIdForMailDto
                                                                    .SequenceCode + "/lot-invitation/" +
                                                                n.SequenceId + "/" + n.InvitationId +
                                                                "" + "/" + projLang;
                                        
                                        
                    
                                        sendGridParameter.ButtonText = projLang switch
                                        {
                                            "en" or null =>
                                                "Link to Project information form",
                                            "nl" =>  "klik hiervoor meer info over het project",
                                            _ => sendGridParameter.ButtonText
                                        };

                                        sendGridParameter.ProjLang = projLang;
                                        // if (sendGridParameter.Lang == "en")
                                        // {
                                        //     sendGridParameter.ButtonText = "Link to Project information form";
                                        //
                                        // }
                                        // else
                                        // {
                                        //     sendGridParameter.ButtonText = "klik hiervoor meer info over het project";
                                        //
                                        // }
                                        sendGridParameter.DisplayImage = "none";
                                        sendGridParameter.DisplayBtn = "block";
                                        await SendReminder(sendGridParameter);
                                        await ConstructorWfStatusUpdate(
                                            "94282458-0b40-con3-b0f9-Lot344c8f1", // Reminder to join tender
                                            n.ConstructorWorkFlowSequenceId, i.ProjectConnectionString);
                                    }

                                if (n.ReminderTwoDate != null && n.Approve == "1" && wfStatus is "d60aad0b-2e84-con2-ad25-Lot0d49477" or "94282458-0b40-con3-b0f9-Lot344c8f1")
                                    if (n.ReminderTwoDate.Value.Date == DateTime.Now.Date)
                                    {
                                        n.ConstructorWorkFlowSequenceId = connection
                                            .Query<string>(wfId, new { lot = n.LotContractorId, Company = n.CompanyId })
                                            .FirstOrDefault();
                                        sendGridParameter.Id = n.CabPersonId;
                                        sendGridParameter.MailBody = mStandardMailHeader.Reminder2;
                                        sendGridParameter.LotTitle = smail.Name;
                                        sendGridParameter.StartDate = smail.StartDate?.ToString("dd/MM/yyyy");
                                        sendGridParameter.EndDate = smail.EndDate?.ToString("dd/MM/yyyy");
                                        sendGridParameter.Subject = smail.Title + " " + "Reminder for Download";
                                        sendGridParameter.Url = url +
                                                                sendGridParameter.GetContractorByIdForMailDto.Cu +
                                                                "/project/" +
                                                                sendGridParameter.GetContractorByIdForMailDto
                                                                    .SequenceCode + "/download-documents/" +
                                                                n.ConstructorWorkFlowSequenceId + "/" + n.InvitationId +
                                                                "" + "/" + projLang;
                                        
                                        sendGridParameter.ButtonText = projLang switch
                                        {
                                            "en" or null =>
                                                "Link to Download form",
                                            "nl" =>  "klik hier voor het verkrijgen van de documenten",
                                            _ => sendGridParameter.ButtonText
                                        };
                                        
                                        sendGridParameter.ProjLang = projLang;

                                        // if (sendGridParameter.Lang == "en")
                                        // {
                                        //     sendGridParameter.ButtonText = "Link to Download form";
                                        //
                                        // }
                                        // else
                                        // {
                                        //     sendGridParameter.ButtonText = "klik hier voor het verkrijgen van de documenten";
                                        //
                                        // }
                                        sendGridParameter.DisplayImage = "none";
                                        sendGridParameter.DisplayBtn = "block";
                                        await SendReminder(sendGridParameter);
                                        await ConstructorWfStatusUpdate(
                                            "94282458-0b40-con4-b0f9-Lot344c8f1", // Reminder download the tender
                                            n.ConstructorWorkFlowSequenceId, i.ProjectConnectionString);
                                    }

                                if (n.ReminderThreeDate != null &&  wfStatus is "d60aad0b-2e84-con1-ad25-Lot0d49477" or "7143ff01-d173-con5-8c17-Lotecdb84c")
                                    if (n.ReminderThreeDate.Value.Date == DateTime.Now.Date)
                                    {
                                        n.ConstructorWorkFlowSequenceId = connection
                                            .Query<string>(wfId, new { lot = n.LotContractorId, Company = n.CompanyId })
                                            .FirstOrDefault();
                                        sendGridParameter.Id = n.CabPersonId;
                                        sendGridParameter.MailBody = mStandardMailHeader.Reminder3;
                                        sendGridParameter.LotTitle = smail.Name;
                                        sendGridParameter.StartDate = smail.StartDate?.ToString("dd/MM/yyyy");
                                        sendGridParameter.EndDate = smail.EndDate?.ToString("dd/MM/yyyy");
                                        sendGridParameter.Subject = smail.Title + " " + "Reminder for Subscribe";
                                        sendGridParameter.Url = url +
                                                                sendGridParameter.GetContractorByIdForMailDto.Cu +
                                                                "/project/" +
                                                                sendGridParameter.GetContractorByIdForMailDto
                                                                    .SequenceCode + "/lot-subscribe/" +
                                                                n.ConstructorWorkFlowSequenceId + "/" + n.InvitationId +
                                                                "" + "/" + projLang;
                                        
                                        sendGridParameter.ButtonText = projLang switch
                                        {
                                            "en" or null =>
                                                "Link to subscribe to tender form",
                                            "nl" =>  "klik hier om aan te geven of u wenst een offerte aan te bieden.",
                                            _ => sendGridParameter.ButtonText
                                        };
                                        
                                        sendGridParameter.ProjLang = projLang;

                                        // if (sendGridParameter.Lang == "en")
                                        // {
                                        //     sendGridParameter.ButtonText = "Link to subscribe to tender form";
                                        // }
                                        // else
                                        // {
                                        //     sendGridParameter.ButtonText = "klik hier om aan te geven of u wenst een offerte aan te bieden.";
                                        //
                                        // }
                                        sendGridParameter.DisplayImage = "none";
                                        sendGridParameter.DisplayBtn = "block";
                                        await SendReminder(sendGridParameter);
                                        await ConstructorWfStatusUpdate(
                                            "7143ff01-d173-con5-8c17-Lotecdb84c", // Tender subscription reminder
                                            n.ConstructorWorkFlowSequenceId, i.ProjectConnectionString);
                                    }
                            }
                    }
                }
            }

            return status;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> SendReminder(SendGridParameter SendGridParameter)
    {
        CabPersonCompany CabPersonCompany;

        var checkOid =
            @"SELECT CabEmail.EmailAddress AS EmailId ,CabPersonCompany.Oid ,CabPerson.FullName AS PersonId FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.PersonId = @PersonId";

        var parm = new { PersonId = SendGridParameter.Id };
        using (var connection = new SqlConnection(SendGridParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            CabPersonCompany = connection.Query<CabPersonCompany>(checkOid, parm).FirstOrDefault();
        }

        var apikey = SendGridParameter.Configuration.GetValue<string>("SENDGRID_API_KEY");
        var templateId_en = SendGridParameter.Configuration.GetValue<string>("TemplateId_en");
        var templateId_nl = SendGridParameter.Configuration.GetValue<string>("TemplateId_nl");
        var email = SendGridParameter.Configuration.GetValue<string>("Reminder_Email");
        var emailName = SendGridParameter.Configuration.GetValue<string>("Reminder_Email_Name");

        if (CabPersonCompany != null)
        {
            var client =
                new SendGridClient(apikey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(email, emailName));
            msg.AddTo(new EmailAddress(CabPersonCompany.EmailId, CabPersonCompany.PersonId));
            // if (SendGridParameter.Lang == "en") msg.SetTemplateId(templateId_en);
            //
            // if (SendGridParameter.Lang == "nl") msg.SetTemplateId(templateId_nl);
            
            switch (SendGridParameter.ProjLang)
            {
                case "en":
                case null:
                    msg.SetTemplateId(templateId_en);
                    break;
                case "nl":
                    msg.SetTemplateId(templateId_nl);
                    break;
            }
            var dynamicTemplateData = new ExampleTemplateDataNew
            {
                Date = SendGridParameter.GetContractorByIdForMailDto.EndDate,
                Code = SendGridParameter.GetContractorByIdForMailDto.ProjectName,
                Services = SendGridParameter.GetContractorByIdForMailDto.ContractingUnit,
                Sector = SendGridParameter.GetContractorByIdForMailDto.Sector,
                Architect = SendGridParameter.GetContractorByIdForMailDto.Architect,
                Builder = SendGridParameter.GetContractorByIdForMailDto.Customer,
                AdditinalInfo = SendGridParameter.GetContractorByIdForMailDto.Description,
                EmailContent = SendGridParameter.MailBody,
                EmailContentHeader = SendGridParameter.Subject,
                Link = SendGridParameter.Url,
                LotTitle = SendGridParameter.LotTitle,
                StartDate = SendGridParameter.StartDate,
                EndDate = SendGridParameter.EndDate,
                Subject = SendGridParameter.Subject,
                DisplayBtn = SendGridParameter.DisplayBtn,
                DisplayImage = SendGridParameter.DisplayImage,
                ButtonText = SendGridParameter.ButtonText,
                ProjectLanguage = SendGridParameter.ProjLang
            };

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }

        return false;
    }

    public async Task<GetContractorByIdForMailDto> GetProjectData(SendGridParameter SendGridParameter,
        string Connection)
    {
        var connectionString = ConnectionString.MapConnectionString(
            SendGridParameter.ContractingUnitSequenceId,
            SendGridParameter.ProjectSequenceId, SendGridParameter.TenantProvider);
        var query =
            @"SELECT ProjectDefinition.SequenceCode,ProjectDefinition.Name AS ProjectName ,CabPerson.FullName AS Customer ,ProjectClassificationSector.Name AS Sector ,ProjectTime.TenderEndDate AS EndDate ,ProjectTime.TenderStartDate AS StartDate ,ProjectDefinition.Name,ProjectDefinition.Description ,CabCompany.Name AS ContractingUnit,CabCompany.SequenceCode AS Cu FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.CabPerson ON ProjectDefinition.CustomerId = CabPerson.Id LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId LEFT OUTER JOIN dbo.ProjectClassificationSector ON ProjectClassification.ProjectClassificationSectorId = ProjectClassificationSector.TypeId LEFT OUTER JOIN dbo.ProjectTime ON ProjectTime.ProjectId = ProjectDefinition.Id LEFT OUTER JOIN dbo.CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE ProjectDefinition.SequenceCode = @Id AND (ProjectClassificationSector.LanguageCode = @lang OR ProjectClassification.ProjectClassificationSectorId IS NULL OR ProjectTime.TenderEndDate IS NULL)";

        var architect =
            @"SELECT CabPerson.FullName FROM dbo.ProjectTeam INNER JOIN dbo.ProjectDefinition ON ProjectTeam.ProjectId = ProjectDefinition.Id INNER JOIN dbo.ProjectTeamRole ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id INNER JOIN dbo.CabPerson ON ProjectTeamRole.CabPersonId = CabPerson.Id WHERE ProjectDefinition.SequenceCode = @ID AND ProjectTeamRole.RoleId = 'tec51857-arch-44b4-8d0e-362ba468000c'";

        var lotquery = @"SELECT LotContractorId,Approve FROM dbo.ContractorTeamList WHERE InvitationId = @Id";

        GetContractorByIdForMailDto mGetContractorByIdForMailDto;

        using (var connection = new SqlConnection(SendGridParameter.TenantProvider.GetTenant().ConnectionString))
        {
            mGetContractorByIdForMailDto = connection.Query<GetContractorByIdForMailDto>(query,
                new { Id = SendGridParameter.ProjectSequenceId, lang = SendGridParameter.Lang }).FirstOrDefault();
            var Architect = connection.Query<string>(architect,
                new { Id = SendGridParameter.ProjectSequenceId, lang = SendGridParameter.Lang }).FirstOrDefault();
            if (Architect != null) mGetContractorByIdForMailDto.Architect = Architect;
        }

        return mGetContractorByIdForMailDto;
    }

    public async Task<string> LotStatusUpdate(string StatusId, string Id, string Connection)
    {
        await using var connection = new SqlConnection(Connection);

        var query = @"UPDATE dbo.ContractorHeader SET StatusId =@StatusId WHERE Id = @LotId ;";
        var timetableup =
            @"INSERT INTO dbo.LotStatusChangeTime ( Id ,LotId ,StatusId ,DateTime ) VALUES ( @Id ,@LotId ,@StatusId ,@DateTime );";


        var param = new
        {
            StatusId,
            LotId = Id,
            Id = Guid.NewGuid().ToString(),
            DateTime = DateTime.UtcNow
        };

        var statusExist = connection
            .Query<string>("Select StatusId From LotStatusChangeTime Where LotId = @LotId AND StatusId = @StatusId",
                param).Any();

        if (!statusExist)
        {
            await connection.ExecuteAsync(timetableup, param);
            await connection.ExecuteAsync(
                "UPDATE dbo.ContractorHeader SET StatusChangeDate =@DateTime WHERE Id = @LotId ", param);
        }

        await connection.ExecuteAsync(query, param);

        return StatusId;
    }

    public async Task<string> ConstructorWfStatusUpdate(string StatusId, string Id, string Connection)
    {
        await using var connection = new SqlConnection(Connection);

        var wfId = connection.Query<string>("SELECT Id FROM dbo.ConstructorWorkFlow WHERE SequenceId = @wfId",
            new { wfId = Id });

        var query = @"UPDATE dbo.ConstructorWorkFlow SET Status =@StatusId WHERE Id = @wfId;";
        var timetableup =
            @"INSERT INTO dbo.ConstructorWfStatusChangeTime ( Id ,ConstructorWf ,StatusId ,DateTime ) VALUES ( @Id ,@wfId ,@StatusId ,@DateTime );";

        var param = new
        {
            StatusId,
            wfId,
            Id = Guid.NewGuid().ToString(),
            DateTime = DateTime.UtcNow
        };

        var statusExist = connection
            .Query<string>(
                "Select StatusId From ConstructorWfStatusChangeTime Where ConstructorWf = @wfId AND StatusId = @StatusId",
                param).Any();

        if (!statusExist)
        {
            await connection.ExecuteAsync(timetableup, param);
            await connection.ExecuteAsync(
                "UPDATE dbo.ConstructorWorkFlow SET StatusChangeDate =@DateTime WHERE Id = @wfId ", param);
        }

        await connection.ExecuteAsync(query, param);

        return StatusId;
    }


    private class ExampleTemplateDataNew
    {
        [JsonProperty("date")] public string Date { get; set; }

        [JsonProperty("code")] public string Code { get; set; }

        [JsonProperty("services")] public string Services { get; set; }

        [JsonProperty("sector")] public string Sector { get; set; }

        [JsonProperty("architect")] public string Architect { get; set; }

        [JsonProperty("builder")] public string Builder { get; set; }

        [JsonProperty("additinalInfo")] public string AdditinalInfo { get; set; }

        [JsonProperty("emailContentHeader")] public string EmailContentHeader { get; set; }

        [JsonProperty("emailContent")] public string EmailContent { get; set; }

        [JsonProperty("startDate")] public string StartDate { get; set; }

        [JsonProperty("endDate")] public string EndDate { get; set; }

        [JsonProperty("lotTitle")] public string LotTitle { get; set; }

        [JsonProperty("link")] public string Link { get; set; }

        [JsonProperty("subject")] public string Subject { get; set; }
        [JsonProperty("buttonText")] public string ButtonText { get; set; }
        [JsonProperty("statusImage")] public string StatusImage { get; set; }
        [JsonProperty("displayImage")] public string DisplayImage { get; set; } 
        [JsonProperty("displayBtn")] public string DisplayBtn { get; set; } 
        [JsonProperty("projectLanguage")] public string ProjectLanguage { get; set; } 

    }
}