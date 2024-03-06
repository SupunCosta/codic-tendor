using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.RFQ;
using UPrinceV4.Web.Repositories.Interfaces.RFQ;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.RFQ;

public class RFQRepository : IRFQRepository
{
    public async Task<string> SendRfqEmail(RFQParameter RFQParameter)
    {
        var cuconnectionString = ConnectionString.MapConnectionString(RFQParameter.ContractingUnitSequenceId, null,
            RFQParameter.TenantProvider);

        await using var dbconnection = new SqlConnection(RFQParameter.TenantProvider.GetTenant().ConnectionString);
        await using var connection = new SqlConnection(cuconnectionString);

        var personQuery =
            @"SELECT CabEmail.EmailAddress AS EmailAddress ,CabPersonCompany.Oid , CabPersonCompany.Id,CabPerson.FullName , CabPersonCompany.JobRole ,CabPersonCompany.CompanyId , CabCompany.Name AS CompanyName FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id LEFT OUTER JOIN CabCompany  ON CabPersonCompany.CompanyId = CabCompany.Id LEFT OUTER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE  CabPerson.IsDeleted = 0";

        var updateQuery =
            @"Update POHeader Set POStatusId = '7143ff01-d173-4a20-8c17-cacdfecdb84c' where SequenceId = @SequenceId"; // in review status
        var rfqList =
            connection.Query<POHeader>(
                "SELECT * FROM POHeader WHERE PORequestType = 'f4d6ba08-3937-44ca-a0a1-7cf33c03e290'");


        var cabPersons = dbconnection.Query<RfqCab>(personQuery);


        var apikey = RFQParameter.Configuration.GetValue<string>("SENDGRID_API_KEY");
        var templateId_en = RFQParameter.Configuration.GetValue<string>("RfqTemplate_en");
        var templateId_nl = RFQParameter.Configuration.GetValue<string>("RfqTemplate_nl");
        var email = RFQParameter.Configuration.GetValue<string>("Reminder_Email");
        var name = RFQParameter.Configuration.GetValue<string>("RFQ_Email_Name");

        var url = RFQParameter.Configuration.GetValue<string>("DomainUrl") + "/CU/";

        var cpcId = RFQParameter.Configuration.GetValue<string>("RFQCpc");


        if (RFQParameter.IdList.Any())
            foreach (var rfq in RFQParameter.IdList)
            {
                var poRfq = rfqList.FirstOrDefault(x => x.SequenceId == rfq);
                // if (poRfq?.POStatusId != "7143ff01-d173-4a20-8c17-cacdfecdb84c" ||
                //     poRfq?.POStatusId != "7bcb4e8d-8e8c-487d-8170-6b91c89fc3da")
                if (poRfq?.POStatusId == "94282458-0b40-40a3-b0f9-c2e40344c8f1")
                {
                    var cabData = cabPersons.FirstOrDefault(x => x.Id == poRfq?.SupplierCabPersonCompanyId);

                    var vendorData = connection
                        .Query<CpcVendor>(
                            "SELECT * FROM CpcVendor WHERE CompanyId = @CompanyId AND CoperateProductCatalogId = @cpcId",
                            new { cabData.CompanyId, cpcId }).FirstOrDefault();

                    var projectTitle = dbconnection
                        .Query<string>("SELECT Title FROM ProjectDefinition WHERE SequenceCode = @sequenceCode ",
                            new { sequenceCode = poRfq?.ProjectSequenceCode }).FirstOrDefault();
                    if (cabData != null)
                    {
                        var client =
                            new SendGridClient(apikey);
                        var msg = new SendGridMessage();
                        msg.SetFrom(new EmailAddress(email, name));

                        msg.AddTo(new EmailAddress(cabData?.EmailAddress, cabData?.FullName));


                        //msg.Subject = "RFQ Accept";
                        if (RFQParameter.Lang == "en") msg.SetTemplateId(templateId_en);

                        if (RFQParameter.Lang == "nl") msg.SetTemplateId(templateId_nl);


                        var dynamicTemplateData = new RfqEmail
                        {
                            Date = poRfq.DeliveryDate?.ToShortDateString(),
                            Code = projectTitle,
                            RfqTitle = rfq,
                            Customer = cabData.FullName,
                            Year = DateTime.UtcNow.Year,
                            EmailContent = "sample RFQ Email",
                            EmailContentHeader = "RFQ Documents",
                            Subject = "RFQ Email - " + poRfq.DeliveryDate?.ToString("dd/MM/yyyy"),
                            Link = url +
                                   RFQParameter.ContractingUnitSequenceId + "/rfq-invitation/" +
                                   rfq,
                            CustomerJobRole = cabData.JobRole,
                            ResourceLeadTime = vendorData?.ResourceLeadTime,
                            UnitPrice = vendorData?.ResourcePrice.ToString()
                        };

                        msg.SetTemplateData(dynamicTemplateData);
                        var response = await client.SendEmailAsync(msg);

                        if (!response.IsSuccessStatusCode)
                            throw new Exception(response.Body.ReadAsStringAsync().Result);


                        await connection.ExecuteAsync(updateQuery, new { SequenceId = rfq });
                    }
                }
            }


        return "ok";
    }

    public async Task<string> AcceptRfqEmail(RFQParameter RFQParameter)
    {
        var cuconnectionString =
            ConnectionString.MapConnectionString(RFQParameter.RfqAccept.Cu, null, RFQParameter.TenantProvider);

        await using var dbconnection = new SqlConnection(RFQParameter.TenantProvider.GetTenant().ConnectionString);
        await using var connection = new SqlConnection(cuconnectionString);

        var personQuery =
            @"SELECT CabEmail.EmailAddress AS EmailAddress ,CabPersonCompany.Oid , CabPersonCompany.Id,CabPerson.FullName , CabCompany.Name AS CompanyName FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id LEFT OUTER JOIN CabCompany  ON CabPersonCompany.CompanyId = CabCompany.Id LEFT OUTER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE  CabPerson.IsDeleted = 0 AND CabPersonCompany.Id = @Id";

        var updateQuery =
            @"Update POHeader Set POStatusId = '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da' where SequenceId = @SequenceId"; // approved status


        var apikey = RFQParameter.Configuration.GetValue<string>("SENDGRID_API_KEY");
        var templateId_en = RFQParameter.Configuration.GetValue<string>("RfqAcceptTemplate_en");
        var templateId_nl = RFQParameter.Configuration.GetValue<string>("RfqAcceptTemplate_nl");
        var email = RFQParameter.Configuration.GetValue<string>("Reminder_Email");
        var name = RFQParameter.Configuration.GetValue<string>("RFQ_Email_Name");


        var url = RFQParameter.Configuration.GetValue<string>("DomainUrl") + "/CU/";

        if (RFQParameter.RfqAccept.SequenceId != null)
        {
            var pdf = RFQParameter.File.OpenReadStream();
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                await pdf?.CopyToAsync(memoryStream)!;
                bytes = memoryStream.ToArray();
            }

            var pdfBase64 = Convert.ToBase64String(bytes);

            var poRfq = connection.Query<POHeader>("SELECT * FROM POHeader WHERE SequenceId = @SequenceId",
                new { RFQParameter.RfqAccept.SequenceId }).FirstOrDefault();

            var cabData = dbconnection.Query<RfqCab>(personQuery, new { Id = poRfq?.SupplierCabPersonCompanyId })
                .FirstOrDefault();

            var projectTitle = dbconnection
                .Query<string>("SELECT Title FROM ProjectDefinition WHERE SequenceCode = @sequenceCode ",
                    new { sequenceCode = poRfq?.ProjectSequenceCode }).FirstOrDefault();

            if (cabData != null)
            {
                var client =
                    new SendGridClient(apikey);
                var msg = new SendGridMessage();
                msg.SetFrom(new EmailAddress(email, name));

                msg.AddTo(new EmailAddress(cabData?.EmailAddress, cabData?.FullName));
                msg.AddAttachment(RFQParameter.File.FileName, pdfBase64);

                // var company = dbconnection
                //     .Query<string>(
                //         "SELECT CabEmail.EmailAddress FROM CabCompany LEFT OUTER JOIN CabEmail  ON CabCompany.EmailId = CabEmail.Id WHERE SequenceCode = @SequenceCode",
                //         new { SequenceCode = RFQParameter.RfqAccept.Cu }).FirstOrDefault();

                //var company = dbconnection
                //    .Query<string>(
                //        @"SELECT                             
                //            CabEmail.EmailAddress                        
                //                FROM dbo.HRHeader
                //                LEFT OUTER JOIN dbo.CabPersonCompany
                //                ON HRHeader.PersonId = CabPersonCompany.Id
                //            LEFT OUTER JOIN dbo.CabEmail 
                //                ON CabPersonCompany.EmailId = CabEmail.Id
                //            WHERE CabPersonCompany.CompanyId = @CompanyId AND HRHeader.IsContactManager = 1",
                //        new { CompanyId = RFQParameter.Configuration.GetValue<String>("CompanyId") }).FirstOrDefault();


                var company = RFQParameter.Configuration.GetValue<string>("OrganizationEmail");
                var companyName = RFQParameter.Configuration.GetValue<string>("OrganizationName");

                if (company != null) msg.AddCc(new EmailAddress(company, companyName));


                msg.Subject = "RFQ Email - " + poRfq.DeliveryDate;
                if (RFQParameter.Lang == "en") msg.SetTemplateId(templateId_en);

                if (RFQParameter.Lang == "nl") msg.SetTemplateId(templateId_nl);


                var dynamicTemplateData = new RfqEmail
                {
                    Date = DateTime.UtcNow.ToShortDateString(),
                    Code = projectTitle,
                    RfqTitle = RFQParameter.RfqAccept.SequenceId,
                    Customer = cabData.FullName,
                    Year = DateTime.UtcNow.Year,
                    EmailContent = "sample RFQ accept Email",
                    EmailContentHeader = "RFQ Documents",
                    //Subject = "Rfq Accepted",
                    Subject = "RFQ Email - " + poRfq.DeliveryDate?.ToString("dd/MM/yyyy"),
                    Link = url +
                           RFQParameter.ContractingUnitSequenceId + "/rfq-invitation/" +
                           RFQParameter.RfqAccept.SequenceId
                };

                msg.SetTemplateData(dynamicTemplateData);
                var response = await client.SendEmailAsync(msg);

                if (!response.IsSuccessStatusCode) throw new Exception(response.Body.ReadAsStringAsync().Result);

                var insertQuery = @"INSERT INTO dbo.RfqSignatures
                                                    (
                                                      Id
                                                     ,RfdId
                                                     ,FullName
                                                     ,Signature
                                                     ,Date
                                                    )
                                                    VALUES
                                                    (
                                                      @Id
                                                     ,@RfdId
                                                     ,@FullName
                                                     ,@Signature
                                                     ,@Date
                                                    )";

                var param = new
                {
                    Id = Guid.NewGuid().ToString(),
                    RfdId = poRfq.Id,
                    RFQParameter.RfqAccept.FullName,
                    RFQParameter.RfqAccept.Signature,
                    Date = DateTime.UtcNow
                };


                await connection.ExecuteAsync(updateQuery, new { RFQParameter.RfqAccept.SequenceId });
                await connection.ExecuteAsync(insertQuery, param);
            }
        }


        return RFQParameter.RfqAccept.SequenceId;
    }
}