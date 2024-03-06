using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Newtonsoft.Json;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.Comment;
using UPrinceV4.Web.Data.Contractor;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PdfToExcel;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.StandardMails;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BM;
using UPrinceV4.Web.Util;
using CompanyDto = UPrinceV4.Web.Data.Contractor.CompanyDto;
using PersonCompanyDto = UPrinceV4.Web.Data.Contractor.PersonCompanyDto;

namespace UPrinceV4.Web.Repositories.BM;

public class ContractorRepository : IContractorReopsitory
{
    public async Task<string> CreateHeader(ContractorParameter ContractorParameter,
        IGraphRepository IGraphRepository, ISendGridRepositorie ISendGridRepositorie)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, ContractorParameter.TenantProvider);

        ContractorHeader data;
        string Id;
        string SequenceId;
        var lot = @"SELECT SequenceId FROM dbo.ContractorHeader WHERE Id = @Id";
        await using var connection = new SqlConnection(connectionString);
        data = connection.Query<ContractorHeader>(lot, new { ContractorParameter.BMLotHeaderDto.Id })
            .FirstOrDefault();
        string ContractorTaxonomyId = null;
        if (ContractorParameter.BMLotHeaderDto.ContractorList.ContractorLot != null)
            ContractorTaxonomyId = ContractorParameter.BMLotHeaderDto.ContractorList.ContractorLot.FirstOrDefault();
        if (data != null)
        {
            SequenceId = data.SequenceId;
        }
        else
        {
            var idGenerator = new IdGenerator();
            SequenceId = idGenerator.GenerateId(applicationDbContext, "LT-", "LTSequence");
        }
        var parameters = new
        {
            ContractorParameter.BMLotHeaderDto.Id,
            ContractorParameter.BMLotHeaderDto.Name,
            Title = SequenceId + " " + ContractorParameter.BMLotHeaderDto.Name,
            SequenceId,
            ContractorParameter.BMLotHeaderDto.ProductItemTypeId,
            ModifiedBy = ContractorParameter.UserId,
            ContractorParameter.BMLotHeaderDto.ContractTaxonomyId,
            //ContractorParameter.BMLotHeaderDto.StatusId,
            StatusId = "d60aad0b-2e84-482b-cowf-618d80d49477",
            ContractorParameter.BMLotHeaderDto.Division,
            ContractorParameter.BMLotHeaderDto.TenderBudget,
            ContractorParameter.BMLotHeaderDto.CustomerBudget,
            ModifiedDateTime = DateTime.UtcNow,
            ContractorParameter.BMLotHeaderDto.ContractorId,
            ContractorParameter.ProjectSequenceId,
            Cu = ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.BMLotHeaderDto.ExpectedNumberOfQuotes,
            CreatedBy = ContractorParameter.UserId,
            CreatedDateTime = DateTime.UtcNow,
            StatusChangeDate = DateTime.UtcNow,
            ContractorTaxonomyId,
            MeasuringStatus = 0.0,
            ContractorParameter.BMLotHeaderDto.StartDate,
            ContractorParameter.BMLotHeaderDto.EndDate,
            ContractorParameter.BMLotHeaderDto.StandardMailId,
            ContractorParameter.BMLotHeaderDto.IsInviteSend,
            ContractorParameter.BMLotHeaderDto.IsPublic
        };
        if (data != null)
        {
            var updateSql =
                "UPDATE dbo.ContractorHeader SET Id = @Id ,Name = @Name ,Title = @Title ,SequenceId = @SequenceId ,ProductItemTypeId = @ProductItemTypeId ,ModifiedBy = @ModifiedBy ,Division = @Division ,TenderBudget = @TenderBudget ,CustomerBudget = @CustomerBudget ,ModifiedDateTime = @ModifiedDateTime ,ContractorId = @ContractorId ,ExpectedNumberOfQuotes = @ExpectedNumberOfQuotes ,ProjectSequenceId = @ProjectSequenceId ,ContractorTaxonomyId = @ContractorTaxonomyId  ,ContractTaxonomyId = @ContractTaxonomyId,StartDate = @StartDate,EndDate = @EndDate,StandardMailId = @StandardMailId , IsInviteSend = @IsInviteSend , IsPublic = @IsPublic WHERE Id = @Id";

            await connection.ExecuteAsync(updateSql, parameters);
        }
        else
        {
            var insertSql =
                "INSERT INTO dbo.ContractorHeader (Id, Name, Title, SequenceId, ProductItemTypeId, StatusId, Division, TenderBudget, CustomerBudget, ContractorId, ExpectedNumberOfQuotes, ProjectSequenceId, ContractorTaxonomyId, CreatedBy, CreatedDateTime, StatusChangeDate,ContractTaxonomyId,MeasuringStatus,StartDate,EndDate,StandardMailId,IsPublic) VALUES (@Id, @Name, @Title, @SequenceId, @ProductItemTypeId, @StatusId, @Division, @TenderBudget, @CustomerBudget, @ContractorId, @ExpectedNumberOfQuotes, @ProjectSequenceId, @ContractorTaxonomyId, @CreatedBy, @CreatedDateTime, @StatusChangeDate,@ContractTaxonomyId , @MeasuringStatus,@StartDate,@EndDate,@StandardMailId,@IsPublic);";

            await connection.ExecuteAsync(insertSql, parameters);
            await LotStatusUpdate(ContractorParameter.BMLotHeaderDto.StatusId, ContractorParameter.BMLotHeaderDto.Id,
                connectionString); //In preperation
        }
        if (ContractorParameter.BMLotHeaderDto.Id == null) throw new Exception("ID not set");
        var BMTechDocsDel = @"DELETE FROM dbo.ContractorTechDocs WHERE LotId = @Id";
        var BMLotTenderDocsDel = @"DELETE FROM dbo.ContractorTenderDocs WHERE LotId = @Id";
        var BMLotTechInstructionsDocsDel = @"DELETE FROM dbo.ContractorTechInstructionsDocs WHERE LotId = @Id";
        var BMLotProvisionalAcceptenceDocsDel =
            @"DELETE FROM dbo.ContractorProvisionalAcceptenceDocs WHERE LotId = @Id";
        var BMLotFinalDeliveryDocsDel = @"DELETE FROM dbo.ContractorFinalDeliveryDocs WHERE LotId = @Id";
        var ContractorTeamListDel = @"DELETE FROM dbo.ContractorTeamList WHERE LotContractorId = @Id";
        var BMLotTenderAwardDel = @"DELETE FROM dbo.ContractorTenderAward WHERE LotId = @Id";
        var BMTechDocs =
            @"INSERT INTO dbo.ContractorTechDocs ( Id ,LotId ,TypeId ,Link ,Title ) VALUES ( @Id ,@LotId ,@Type ,@Link ,@Title );";
        var BMLotTechInstructionsDocs =
            @"INSERT INTO dbo.ContractorTechInstructionsDocs ( Id ,LotId ,TypeId ,Link ,Title ) VALUES ( @Id ,@LotId ,@Type ,@Link ,@Title );";
        var BMLotProvisionalAcceptenceDocs =
            @"INSERT INTO dbo.ContractorProvisionalAcceptenceDocs ( Id ,LotId ,TypeId ,Link ,Title ) VALUES ( @Id ,@LotId ,@Type ,@Link ,@Title );";
        var BMLotFinalDeliveryDocs =
            @"INSERT INTO dbo.ContractorFinalDeliveryDocs ( Id ,LotId ,TypeId ,Link ,Title ) VALUES ( @Id ,@LotId ,@Type ,@Link ,@Title );";
        var BMLotTenderDocs =
            @"INSERT INTO dbo.ContractorTenderDocs ( Id ,LotId ,TypeId ,Link ,Title ) VALUES ( @Id ,@LotId ,@Type ,@Link ,@Title );";
        var ContractorTeamList =
            @"MERGE INTO dbo.ContractorTeamList t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE SET Id = @Id ,Name = @Name ,Company = @Company ,RoleId = @RoleId ,InvitationMail = @InvitationMail ,CabPersonId = @CabPersonId ,CabPersonName = @CabPersonName ,LotContractorId = @LotContractorId ,RoleName = @RoleName ,CompanyId = @CompanyId ,IsManual = @IsManual ,InvitationId = @InvitationId ,Approve = @Approve ,IsDownloded = @IsDownloded ,IsSubscribed = @IsSubscribed WHEN NOT MATCHED THEN INSERT (Id, Name, Company, RoleId, InvitationMail, CabPersonId, CabPersonName, LotContractorId, RoleName, CompanyId, IsManual, InvitationId, Approve, IsDownloded, IsSubscribed) VALUES (@Id, @Name, @Company, @RoleId, @InvitationMail, @CabPersonId, @CabPersonName, @LotContractorId, @RoleName, @CompanyId, @IsManual, @InvitationId, @Approve, @IsDownloded, @IsSubscribed);";
        var parmContractorList = new
        {
            Id = Id = ContractorParameter.BMLotHeaderDto.Id, ContractorLot = ContractorParameter.BMLotHeaderDto.Id,
            LotId = ContractorParameter.BMLotHeaderDto.Id
        };
        await connection.ExecuteAsync(BMTechDocsDel, parmContractorList);
        await connection.ExecuteAsync(BMLotTenderDocsDel, parmContractorList);
        await connection.ExecuteAsync(BMLotTechInstructionsDocsDel, parmContractorList);
        await connection.ExecuteAsync(BMLotProvisionalAcceptenceDocsDel, parmContractorList);
        await connection.ExecuteAsync(BMLotFinalDeliveryDocsDel, parmContractorList);
        await connection.ExecuteAsync(ContractorTeamListDel, parmContractorList);
        await connection.ExecuteAsync(BMLotTenderAwardDel, parmContractorList);
        if (ContractorTaxonomyId != null)
        {
            await connection.ExecuteAsync(
                "Delete From ContractorHasTaxonony Where ContractorId = @ContractorId",
                new { ContractorId = ContractorParameter.BMLotHeaderDto.Id });

            var taxSql =
                "INSERT INTO dbo.ContractorHasTaxonony ( Id ,TaconomyId ,ContractorId ) VALUES ( @Id ,@TaconomyId ,@ContractorId )";

            foreach (var taxonomy in ContractorParameter.BMLotHeaderDto.ContractorList.ContractorLot)
            {
                var taxParam = new
                {
                    Id = Guid.NewGuid().ToString(),
                    TaconomyId = taxonomy,
                    ContractorId = ContractorParameter.BMLotHeaderDto.Id
                };

                await connection.ExecuteAsync(taxSql, taxParam);
            }
        }
        else
        {
            await connection.ExecuteAsync(
                "Delete From ContractorHasTaxonony Where ContractorId = @ContractorId",
                new { ContractorId = ContractorParameter.BMLotHeaderDto.Id });
        }
        if (ContractorParameter.BMLotHeaderDto.TechnicalDocumentation.TechnicalDocList != null)
            foreach (var i in ContractorParameter.BMLotHeaderDto.TechnicalDocumentation.TechnicalDocList)
            {
                var parm = new
                {
                    i.Id, LotId = ContractorParameter.BMLotHeaderDto.Id,
                    Type = i.TypeId,
                    i.Link,
                    i.Title
                };
                await connection.ExecuteAsync(BMTechDocs, parm);
            }
        if (ContractorParameter.BMLotHeaderDto.LotDocumentation.TenderDocuments != null)
            foreach (var i in ContractorParameter.BMLotHeaderDto.LotDocumentation.TenderDocuments)
            {
                var parm = new
                {
                    i.Id, LotId = ContractorParameter.BMLotHeaderDto.Id,
                    Type = i.TypeId,
                    i.Link,
                    i.Title
                };
                await connection.ExecuteAsync(BMLotTenderDocs, parm);
            }
        if (ContractorParameter.BMLotHeaderDto.LotDocumentation.FinalDelivery != null)
            foreach (var i in ContractorParameter.BMLotHeaderDto.LotDocumentation.FinalDelivery)
            {
                var parm = new
                {
                    i.Id, LotId = ContractorParameter.BMLotHeaderDto.Id,
                    Type = i.TypeId,
                    i.Link,
                    i.Title
                };
                await connection.ExecuteAsync(BMLotFinalDeliveryDocs, parm);
            }
        if (ContractorParameter.BMLotHeaderDto.LotDocumentation.ProvisionalAcceptance != null)
            foreach (var i in ContractorParameter.BMLotHeaderDto.LotDocumentation.ProvisionalAcceptance)
            {
                var parm = new
                {
                    i.Id, LotId = ContractorParameter.BMLotHeaderDto.Id,
                    Type = i.TypeId,
                    i.Link,
                    i.Title
                };
                await connection.ExecuteAsync(BMLotProvisionalAcceptenceDocs, parm);
            }
        if (ContractorParameter.BMLotHeaderDto.LotDocumentation.TechnicalInstructions != null)
            foreach (var i in ContractorParameter.BMLotHeaderDto.LotDocumentation.TechnicalInstructions)
            {
                var parm = new
                {
                    i.Id, LotId = ContractorParameter.BMLotHeaderDto.Id,
                    Type = i.TypeId,
                    i.Link,
                    i.Title
                };
                await connection.ExecuteAsync(BMLotTechInstructionsDocs, parm);
            }
        IEnumerable<ContractorTeamList> company;
        var ConstructorWorkFlowselect =
            @"SELECT CabCompanyId as CompanyId FROM dbo.ConstructorWorkFlow WHERE Lot = @Id";
        company = connection.Query<ContractorTeamList>(ConstructorWorkFlowselect,
            new { ContractorParameter.BMLotHeaderDto.Id });
        if (ContractorParameter.BMLotHeaderDto.ContractorList.ContractorTeamList != null)
            foreach (var i in ContractorParameter.BMLotHeaderDto.ContractorList.ContractorTeamList)
            {
                if (ContractorParameter.BMLotHeaderDto.IsPublic)
                {
                    i.IsDownloded = true;
                    i.IsSubscribed = true;
                }
                var contractor = new ConstructorLotInfoDto
                {
                    Lot = parameters.Id,
                    Division = parameters.Division,
                    Price = parameters.TenderBudget,
                    Contractor = i.Company,
                    CompanyId = i.CompanyId
                };
                ContractorParameter.ConstructorLotInfoDto = contractor;
                await CreateContractorWorkflow(ContractorParameter);
                var approve = "0";
                if (i.Approve != null) approve = i.Approve;
                var parm1 = new
                {
                    Id = Guid.NewGuid().ToString(), LotId = ContractorParameter.BMLotHeaderDto.Id,
                    LotContractorId = ContractorParameter.BMLotHeaderDto.Id,
                    i.Name,
                    i.Company,
                    i.RoleId,
                    i.RoleName,
                    i.InvitationMail,
                    i.CabPersonId,
                    i.CabPersonName,
                    i.CompanyId,
                    i.IsManual,
                    Approve = approve,
                    i.InvitationId,
                    i.IsDownloded,
                    i.IsSubscribed
                };
                await connection.ExecuteAsync(ContractorTeamList, parm1);
                company = company.Where(a => a.CompanyId != i.CompanyId);
            }
        var ConstructorWorkFlowDelete =
            @"DELETE FROM dbo.ConstructorWorkFlow WHERE ConstructorWorkFlow.Lot = @Lot AND ConstructorWorkFlow.CabCompanyId = @CabCompanyId";
        foreach (var i in company)
            await connection.ExecuteAsync(ConstructorWorkFlowDelete,
                new { Lot = ContractorParameter.BMLotHeaderDto.Id, CabCompanyId = i.CompanyId });
        await LotAddReminderDates(ContractorParameter, parameters.Id);
        return SequenceId;
    }

    public async Task<string> PublishTender(ContractorParameter ContractorParameter, IGraphRepository IGraphRepository)
    {
        foreach (var i in ContractorParameter.ContractorTeam.ContractorTeamList)
            if (i.Approve == "1")
            {
                var GraphParameter = new GraphParameter();

                GraphParameter.Id = i.CabPersonId;
                GraphParameter.TenantProvider = ContractorParameter.TenantProvider;
                GraphParameter.GraphServiceClient = ContractorParameter.GraphServiceClient;
                GraphParameter.ButtonText = "Accept(nl)";
                await IGraphRepository.SendInvitation(GraphParameter);
            }
        return ContractorParameter.ContractorTeam.SequenceId;
    }

    public async Task<string> ApproveInvitation(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
            var cuconnectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                null, ContractorParameter.TenantProvider);
            await using var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
            StandardMailHeader mStandardMailHeader;
            var accept = @"UPDATE dbo.ContractorTeamList SET Approve = @Approve WHERE InvitationId = @InvitationId;";
            var constructorWf =
                @"SELECT ConstructorWorkFlow.* FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id LEFT OUTER JOIN dbo.ContractorTeamList ON ConstructorWorkFlow.Lot = ContractorTeamList.LotContractorId WHERE ConstructorWorkFlow.CabCompanyId = @Id AND ContractorTeamList.InvitationId = @InvitationId";
            var url = ContractorParameter.Configuration.GetValue<string>("DomainUrl") + "/CU/";
            await using var connection = new SqlConnection(connectionString);
            {
                var sMailId =
                    @"SELECT ContractorHeader.StandardMailId,ContractorHeader.Title,ContractorHeader.Name,ContractorHeader.StartDate,ContractorHeader.EndDate FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorTeamList.InvitationId = @Id";
                ContractorHeader mContractorHeader;
                mContractorHeader = connection.Query<ContractorHeader>(sMailId,
                    new
                    {
                        Id = ContractorParameter.AcceptInvitationDto.InvitationId
                    }).FirstOrDefault();
                if (mContractorHeader.StandardMailId != null)
                    await using (var cuconnection = new SqlConnection(cuconnectionString))
                    {
                        mStandardMailHeader = cuconnection
                            .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id",
                                new { Id = mContractorHeader.StandardMailId }).FirstOrDefault();
                    }
                else
                    await using (var cuconnection = new SqlConnection(cuconnectionString))
                    {
                        mStandardMailHeader = cuconnection
                            .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader WHERE IsDefault = 1")
                            .FirstOrDefault();
                    }
                await connection.ExecuteAsync(accept,
                    new
                    {
                        ContractorParameter.AcceptInvitationDto.Approve,
                        ContractorParameter.AcceptInvitationDto.InvitationId
                    });
                var company = connection.Query<ContractorTeamList>(
                    "SELECT CompanyId FROM ContractorTeamList WHERE InvitationId = @InvitationId",
                    new { ContractorParameter.AcceptInvitationDto.InvitationId }).FirstOrDefault();
                var ConstructorWorkFlow = connection.Query<ConstructorWorkFlow>(constructorWf,
                        new { Id = company.CompanyId, ContractorParameter.AcceptInvitationDto.InvitationId })
                    .FirstOrDefault();
                if (ContractorParameter.AcceptInvitationDto.Approve == "1")
                {
                    var personId = connection
                        .Query<string>(
                            "Select CabPersonId From ContractorTeamList WHERE LotContractorId = @lotId AND CompanyId = @companyId",
                            new { lotId = ConstructorWorkFlow.Lot, companyId = ConstructorWorkFlow.CabCompanyId })
                        .FirstOrDefault();
                    
                    var projLang = dbConnection.Query<string>(
                        "SELECT pl.Name FROM ProjectDefinition LEFT OUTER JOIN ProjectLanguage pl ON ProjectDefinition.Language = pl.TypeId WHERE ProjectDefinition.SequenceCode = @Id",
                        new { Id = ContractorParameter.ProjectSequenceId }).FirstOrDefault();
                    var sendGridParameter = new SendGridParameter();
                    sendGridParameter.Id = personId;
                    sendGridParameter.TenantProvider = ContractorParameter.TenantProvider;
                    sendGridParameter.ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId;
                    sendGridParameter.ProjectSequenceId = ContractorParameter.ProjectSequenceId;
                    sendGridParameter.Lot = ConstructorWorkFlow.Lot;
                    sendGridParameter.Lang = ContractorParameter.Lang;
                    sendGridParameter.MailBody = mStandardMailHeader.DownloadTender;
                    sendGridParameter.LotTitle = mContractorHeader.Name;
                    sendGridParameter.StartDate = mContractorHeader.StartDate?.ToString("dd/MM/yyyy");
                    sendGridParameter.EndDate = mContractorHeader.EndDate?.ToString("dd/MM/yyyy");
                    sendGridParameter.Configuration = ContractorParameter.Configuration;
                    sendGridParameter.Url = url +
                                            ContractorParameter.ContractingUnitSequenceId + "/project/" +
                                            ContractorParameter.ProjectSequenceId + "/download-documents/" +
                                            ConstructorWorkFlow.SequenceId + "/" +
                                            ContractorParameter.AcceptInvitationDto.InvitationId +
                                            "" + "/" + projLang;
                    sendGridParameter.ButtonText = "klik hier voor het verkrijgen van de documenten";
                                    sendGridParameter.StatusImage = projLang switch
                                    {
                                        "en" or null =>
                                            "https://bmengineeringuprinceuat.blob.core.windows.net/uprincev4dev/en-download.png",
                                        "nl" =>  "https://bmengineeringuprinceuat.blob.core.windows.net/uprincev4dev/nl-download.png",
                                        _ => sendGridParameter.StatusImage
                                    };
                    sendGridParameter.ButtonText = projLang switch
                    {
                        "en" or null =>
                            "click here to obtain the documents",
                        "nl" => "klik hier voor het verkrijgen van de documenten",
                        _ => sendGridParameter.ButtonText
                    };
                    sendGridParameter.Subject = projLang switch
                    {
                        "en" or null =>
                            mContractorHeader.Title + " " + "Download",
                        "nl" => mContractorHeader.Title + " " + "Downloaden",
                        _ => sendGridParameter.Subject
                    };
                    sendGridParameter.ProjLang = projLang;
                    sendGridParameter.DisplayImage = "block";
                    sendGridParameter.DisplayBtn = "block";
                    var send = await ContractorParameter.SendGridRepositorie.SendInvitation(sendGridParameter);
                    await ConstructorWfStatusUpdate("d60aad0b-2e84-con2-ad25-Lot0d49477", ConstructorWorkFlow.Id,
                        connectionString); // tender joined
                }
                else if (ContractorParameter.AcceptInvitationDto.Approve == "2")
                {
                    await ConstructorWfStatusUpdate("215cf869-7768-43d9-ac4f-b728cb053ffa", ConstructorWorkFlow.Id,
                        connectionString); // tender declined
                }
            }
            return ContractorParameter.AcceptInvitationDto.InvitationId;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }
    public async Task<ContractorDropDownData> GetContractorDropDownData(ContractorParameter ContractorParameter)
    {
        const string query =
            @"select StatusId as [Key], Name as Text  FROM dbo.ContractorStatus where LanguageCode = @lang
                              ORDER BY DisplayOrder;select TypeId as [Key], Name as Text  FROM dbo.ContractorFileType where LanguageCode = @lang order by DisplayOrder ;
                                SELECT TypeId as [Key], Name as Text FROM dbo.ContractorProductItemType WHERE LanguageCode = @lang;";
        var mBMLotDropDownData = new ContractorDropDownData();
        var parameters = new { lang = ContractorParameter.Lang };
        await using var connection =
            new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var muilti = await connection.QueryMultipleAsync(query, parameters);
        mBMLotDropDownData.Status = muilti.Read<ContractorStatusDto>();
        mBMLotDropDownData.FileType = muilti.Read<ContractorFileTypeDto>();
        mBMLotDropDownData.ProductItemType = muilti.Read<ContractorProductItemTypeDto>();
        return mBMLotDropDownData;
    }
    public async Task<IEnumerable<ContractorListDto>> ContractorFilter(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
            var query = @"SELECT
                          ContractorHeader.Id
                         ,ContractorHeader.SequenceId
                         ,ContractorHeader.Title                         
                         ,ContractorHeader.TenderBudget AS Price
                         ,ContractorHeader.StatusId                      
                         ,ContractorHeader.StatusChangeDate AS StatusChangeDate 
                         ,ContractorHeader.IsInviteSend                        
                         ,'ContractId' AS ContractId
                         ,'ContractSequenceId' AS ContractSequenceId  
                         ,'Contract' AS Type
                         ,ContractorStatus.Name AS Status
                        FROM dbo.ContractorHeader
                        LEFT OUTER JOIN dbo.ContractorStatus
                          ON ContractorHeader.StatusId = ContractorStatus.StatusId
                          WHERE ContractorStatus.LanguageCode = @lang ";

            var query2 = @"SELECT
                              ConstructorWorkFlow.Id
                             ,ConstructorWorkFlow.SequenceId
                             ,ConstructorWorkFlow.Contractor AS Title
                             ,ConstructorWorkFlow.Price
                             ,ConstructorWorkFlow.Status AS StatusId
                             ,ConstructorWorkFlow.StatusChangeDate
                             ,ConstructorWorkFlow.Lot AS ContractId
                             ,ConstructorWorkFlow.CabCompanyId AS CompanyId
                             ,ConstructorWorkFlow.IsInviteSend 
                             ,ContractorHeader.SequenceId AS ContractSequenceId
                             ,'Contractor' AS Type
                             ,ConstructorWorkFlowStatus.Name AS Status
                            FROM dbo.ConstructorWorkFlow
                            INNER JOIN dbo.ContractorHeader
                              ON ConstructorWorkFlow.Lot = ContractorHeader.Id
                            LEFT OUTER JOIN dbo.ConstructorWorkFlowStatus
                              ON ConstructorWorkFlow.Status = ConstructorWorkFlowStatus.StatusId
                            WHERE ConstructorWorkFlow.Lot = @Id AND ConstructorWorkFlowStatus.LanguageCode = @lang
                            ORDER BY ConstructorWorkFlow.SequenceId DESC";
            
            var awardwinner =
                @"SELECT IsWinner AS IsZeroState FROM dbo.ContractorTotalValues WHERE LotId = @Id AND IsWinner = 1 ";
            var sb = new StringBuilder(query);
            if (ContractorParameter.Filter.Title != null)
            {
                ContractorParameter.Filter.Title = ContractorParameter.Filter.Title.Replace("'", "''");
                var words = ContractorParameter.Filter.Title.Split(" ");
                foreach (var element in words) sb.Append(" AND ContractorHeader.Title LIKE '%" + element + "%'");
            }
            if (ContractorParameter.Filter.Sorter.Attribute == null) sb.Append(" ORDER BY ContractorHeader.Title DESC");
            if (ContractorParameter.Filter.Sorter.Attribute != null)
                if (ContractorParameter.Filter.Sorter.Attribute.ToLower().Equals("title"))
                    sb.Append("ORDER BY ContractorHeader.Title " + ContractorParameter.Filter.Sorter.Order);
            var parameters = new { lang = ContractorParameter.Lang };
            IEnumerable<ContractorListDto> data = null;
            IEnumerable<ContractorListDto> data1 = null;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                data = (List<ContractorListDto>)await connection.QueryAsync<ContractorListDto>(sb.ToString(),
                    parameters);
                foreach (var i in data)
                {
                    i.IsZeroState = connection.Query<bool>(awardwinner, new { Id = i.SequenceId }).FirstOrDefault();
                    data1 = (List<ContractorListDto>)await connection.QueryAsync<ContractorListDto>(query2,
                        new { i.Id, lang = ContractorParameter.Lang });

                    foreach (var cwPrice in data1)
                    {
                        cwPrice.Price = connection
                            .Query<string>(
                                "SELECT TotalBAFO FROM dbo.ContractorTotalValuesPublished WHERE LotId = @LotId AND CompanyId = @CompanyId",
                                new { LotId = i.SequenceId, cwPrice.CompanyId }).FirstOrDefault() ?? "0";
                        using (var connection2 =
                               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))

                        {
                            cwPrice.Title = connection2
                                .Query<string>("SELECT Name from dbo.CabCompany WHERE Id = @CompanyId",
                                    new { cwPrice.CompanyId }).FirstOrDefault();
                        }
                    }
                    data = (data ?? (IEnumerable<ContractorListDto>)Enumerable.Empty<string>()).Concat(
                        data1 ?? (IEnumerable<ContractorListDto>)Enumerable.Empty<string>());
                }
            }

            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<ContractorListDto>> ContractorFilterGetLots(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
            var query = @"SELECT
                          ContractorHeader.Id
                         ,ContractorHeader.SequenceId
                         ,ContractorHeader.Title                         
                         ,ContractorHeader.TenderBudget AS Price
                         ,ContractorHeader.StatusId                      
                         ,ContractorHeader.StatusChangeDate AS StatusChangeDate                        
                         ,'ContractId' AS ContractId
                         ,'ContractSequenceId' AS ContractSequenceId  
                         ,'Contract' AS Type
                         ,ContractorStatus.Name AS Status
                        FROM dbo.ContractorHeader
                        LEFT OUTER JOIN dbo.ContractorStatus
                          ON ContractorHeader.StatusId = ContractorStatus.StatusId
                          WHERE ContractorStatus.LanguageCode = @lang ";


            var sb = new StringBuilder(query);

            if (ContractorParameter.Filter.Title != null)
            {
                var words = ContractorParameter.Filter.Title.Split(" ");
                foreach (var element in words) sb.Append(" AND ContractorHeader.Title LIKE '%" + element + "%'");
            }

            if (ContractorParameter.Filter.Sorter.Attribute == null) sb.Append(" ORDER BY ContractorHeader.Title DESC");

            if (ContractorParameter.Filter.Sorter.Attribute != null)
                if (ContractorParameter.Filter.Sorter.Attribute.ToLower().Equals("title"))
                    sb.Append("ORDER BY ContractorHeader.Title " + ContractorParameter.Filter.Sorter.Order);

            var parameters = new { lang = ContractorParameter.Lang };
            IEnumerable<ContractorListDto> data = null;
            IEnumerable<ContractorListDto> data1 = null;
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                data = (List<ContractorListDto>)await connection.QueryAsync<ContractorListDto>(sb.ToString(),
                    parameters);
            }

            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<BMLotHeaderGetDto> GetContractorById(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var cuconnectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            null, ContractorParameter.TenantProvider);

        HistoryLogDto historyLog;
        var query = @"SELECT 
                              ContractorHeader.Id
                             ,ContractorHeader.SequenceId
                             ,ContractorHeader.Name
                             ,ContractorHeader.Title
                             ,ContractorHeader.StatusId
                             ,ContractorHeader.Division
                             ,ContractorHeader.TenderBudget
                             ,ContractorHeader.CustomerBudget
                             ,ContractorHeader.ContractorId
                             ,ContractorHeader.ExpectedNumberOfQuotes
                             ,ContractorHeader.ContractTaxonomyId AS ContractTaxonomyId
                             ,ContractorHeader.MeasuringStatus
                             ,ContractorHeader.StandardMailId
                             ,ContractorHeader.StartDate
                             ,ContractorHeader.EndDate
                             ,ContractorHeader.IsInviteSend
                             ,ContractorProductItemType.TypeId AS [Key]
                             ,ContractorProductItemType.Name AS [Text]
                             ,ContractorStatus.StatusId AS [Key]
                             ,ContractorStatus.Name AS [Text]
                            FROM dbo.ContractorHeader
                            LEFT OUTER JOIN dbo.ContractorProductItemType
                              ON ContractorHeader.ProductItemTypeId = ContractorProductItemType.TypeId
                            LEFT OUTER JOIN dbo.ContractorStatus
                              ON ContractorHeader.StatusId = ContractorStatus.StatusId
                            WHERE ContractorStatus.LanguageCode = @lang
                            AND ContractorProductItemType.LanguageCode = @lang
                            AND ContractorHeader.SequenceId = @Id
                            ";

        var docquery =
            @"SELECT COUNT(*) OVER (PARTITION BY 1) AS Count,ContractorFinalDeliveryDocs.Id ,ContractorFinalDeliveryDocs.LotId ,ContractorFinalDeliveryDocs.Link ,ContractorFinalDeliveryDocs.Title ,ContractorFileType.TypeId ,ContractorFileType.Name AS TypeName FROM dbo.ContractorFinalDeliveryDocs LEFT OUTER JOIN dbo.ContractorFileType ON ContractorFinalDeliveryDocs.TypeId = ContractorFileType.TypeId WHERE LotId = @Id AND ContractorFileType.LanguageCode = @lang;
                                SELECT COUNT(*) OVER (PARTITION BY 1) AS Count,ContractorTechInstructionsDocs.Id ,ContractorTechInstructionsDocs.LotId ,ContractorTechInstructionsDocs.Link ,ContractorTechInstructionsDocs.Title ,ContractorFileType.TypeId AS TypeId ,ContractorFileType.Name AS TypeName FROM dbo.ContractorTechInstructionsDocs LEFT OUTER JOIN dbo.ContractorFileType ON ContractorTechInstructionsDocs.TypeId = ContractorFileType.TypeId WHERE LotId = @Id AND ContractorFileType.LanguageCode = @lang;
                                SELECT COUNT(*) OVER (PARTITION BY 1) AS Count,ContractorTenderDocs.Id ,ContractorTenderDocs.LotId ,ContractorTenderDocs.Link ,ContractorTenderDocs.Title ,ContractorFileType.TypeId AS TypeId ,ContractorFileType.Name AS TypeName FROM dbo.ContractorTenderDocs LEFT OUTER JOIN dbo.ContractorFileType ON ContractorTenderDocs.TypeId = ContractorFileType.TypeId WHERE LotId = @Id AND ContractorFileType.LanguageCode = @lang;
                                SELECT COUNT(*) OVER (PARTITION BY 1) AS Count,ContractorProvisionalAcceptenceDocs.Id ,ContractorProvisionalAcceptenceDocs.LotId ,ContractorProvisionalAcceptenceDocs.Link ,ContractorProvisionalAcceptenceDocs.Title ,ContractorFileType.TypeId AS TypeId ,ContractorFileType.Name AS TypeName FROM dbo.ContractorProvisionalAcceptenceDocs LEFT OUTER JOIN dbo.ContractorFileType ON ContractorProvisionalAcceptenceDocs.TypeId = ContractorFileType.TypeId WHERE LotId = @Id AND ContractorFileType.LanguageCode = @lang;
                                SELECT COUNT(*) OVER (PARTITION BY 1) AS Count,ContractorTechDocs.Id ,ContractorTechDocs.LotId ,ContractorTechDocs.Link ,ContractorTechDocs.Title ,ContractorFileType.TypeId AS TypeId ,ContractorFileType.Name AS TypeName FROM dbo.ContractorTechDocs LEFT OUTER JOIN dbo.ContractorFileType ON ContractorTechDocs.TypeId = ContractorFileType.TypeId WHERE LotId = @Id AND ContractorFileType.LanguageCode = @lang;
                                SELECT COUNT(*) OVER (PARTITION BY 1) AS Count,ContractorTeamList.Id ,ContractorTeamList.Name ,ContractorTeamList.Company ,ContractorTeamList.RoleId ,ContractorTeamList.InvitationMail ,ContractorTeamList.CabPersonId ,ContractorTeamList.CabPersonName ,ContractorTeamList.RoleName ,ContractorTeamList.LotContractorId,ContractorTeamList.CompanyId,ContractorTeamList.IsManual,ContractorTeamList.Approve,ContractorTeamList.InvitationId,ContractorTeamList.IsDownloded,ContractorTeamList.IsSubscribed,ContractorTeamList.InvitationDateTime FROM dbo.ContractorTeamList WHERE ContractorTeamList.LotContractorId = @Id;
                                SELECT ModifiedBy ,ModifiedDateTime ,CreatedBy ,CreatedDateTime FROM ContractorHeader WHERE Id = @Id;
                                SELECT Id ,LotId ,ContractorId ,Price ,IsWinner FROM dbo.ContractorTenderAward WHERE LotId = @Id;
                                SELECT (SELECT COUNT(*) FROM dbo.ContractorFinalDeliveryDocs WHERE LotId = @Id) + (SELECT COUNT(*) FROM dbo.ContractorProvisionalAcceptenceDocs WHERE LotId = @Id) + (SELECT COUNT(*) FROM dbo.ContractorTechInstructionsDocs WHERE LotId = @Id) + (SELECT COUNT(*) FROM dbo.ContractorTechDocs WHERE LotId = @Id) + (SELECT COUNT(*) FROM dbo.ContractorTechInstructionsDocs WHERE LotId = @Id ); 
                                ";

        var ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

        var parameters = new { lang = ContractorParameter.Lang, ContractorParameter.Id };

        BMLotHeaderGetDto mBMLotHeaderGetDto = null;
        using (var connection = new SqlConnection(connectionString))
        {
            mBMLotHeaderGetDto = connection
                .Query<BMLotHeaderGetDto, ContractorProductItemTypeDto, ContractorStatusDto, BMLotHeaderGetDto>(
                    query,
                    (lotHeader, lotTypeDto, lotStatusDto) =>
                    {
                        lotHeader.ProductItemType = lotTypeDto;
                        lotHeader.Status = lotStatusDto;

                        return lotHeader;
                    }, parameters,
                    splitOn: "Key, Key").FirstOrDefault();


            if (mBMLotHeaderGetDto != null)
            {
                var mLotDocumentation = new ContractorDocumentationget();
                var mTechnicalDocumentationGet = new TechnicalDocumentationGet();
                var mContractorList = new ContractorList();
                var mBMLotDropDownData = new ContractorDropDownData();
                var parameters1 = new
                {
                    lang = ContractorParameter.Lang,
                    mBMLotHeaderGetDto.Id,
                    LotContractorId = mBMLotHeaderGetDto.ContractorId
                };

                mBMLotHeaderGetDto.ContractorTaxonomyId = connection
                    .Query<string>("SELECT TaconomyId From ContractorHasTaxonony WHERE ContractorId = @Id",
                        new { mBMLotHeaderGetDto.Id }).ToList();

                using (var multi = await connection.QueryMultipleAsync(docquery, parameters1))
                {
                    mLotDocumentation.FinalDelivery = multi.Read<ContractorFinalDeliveryDocsDto>().ToList();
                    mLotDocumentation.TechnicalInstructions =
                        multi.Read<ContractorTechInstructionsDocsDto>().ToList();
                    mLotDocumentation.TenderDocuments = multi.Read<ContractorTenderDocsDto>().ToList();
                    mLotDocumentation.ProvisionalAcceptance =
                        multi.Read<ContractorProvisionalAcceptenceDocsDto>().ToList();
                    mTechnicalDocumentationGet.TechnicalDocList = multi.Read<ContractorTechDocsDto>().ToList();
                    mContractorList.ContractorTeamList = multi.Read<ContractorTeamList>().ToList();
                    historyLog = multi.Read<HistoryLogDto>().FirstOrDefault();
                }

                mBMLotHeaderGetDto.TenderAwarding = await GetContractorsByLotId(ContractorParameter);
                mBMLotHeaderGetDto.TimeTable = connection.Query<GetLotStatusChangeTime>(
                    @"SELECT ContractorStatus.Name AS StatusName ,LotStatusChangeTime.* FROM dbo.LotStatusChangeTime LEFT OUTER JOIN dbo.ContractorStatus ON LotStatusChangeTime.StatusId = ContractorStatus.StatusId WHERE LotId = @LotId AND ContractorStatus.LanguageCode = @lang Order By DateTime Desc",
                    new { LotId = mBMLotHeaderGetDto.Id, lang = ContractorParameter.Lang }).ToList();


                mBMLotHeaderGetDto.MeasuringStatus = connection
                    .Query<double>("Select MeasuringStatus From ContractorHeader Where SequenceId = @SequenceId",
                        new { SequenceId = ContractorParameter.Id }).FirstOrDefault().ToString("0.0");

                var publishTenderDto = new PublishTender
                {
                    DocumentsUploaded = mLotDocumentation.FinalDelivery.Count +
                                        mLotDocumentation.TechnicalInstructions.Count +
                                        mLotDocumentation.TenderDocuments.Count +
                                        mLotDocumentation.ProvisionalAcceptance.Count +
                                        mTechnicalDocumentationGet.TechnicalDocList.Count,
                    ContractorsInList = mContractorList.ContractorTeamList.Count,
                    MeasuringStatus = mBMLotHeaderGetDto.MeasuringStatus
                };
                foreach (var i in mContractorList.ContractorTeamList)
                    using (var connection2 =
                           new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))

                    {
                        i.Company = connection2.Query<string>("SELECT Name from dbo.CabCompany WHERE Id = @CompanyId",
                            new { i.CompanyId }).FirstOrDefault();
                    }

                mBMLotHeaderGetDto.LotDocumentation = mLotDocumentation;
                mBMLotHeaderGetDto.TechnicalDocumentation = mTechnicalDocumentationGet;
                mBMLotHeaderGetDto.ContractorList = mContractorList;
                mBMLotHeaderGetDto.PublishTender = publishTenderDto;
                mBMLotHeaderGetDto.ContractorList.ContractorLot = mBMLotHeaderGetDto.ContractorTaxonomyId;
                using (var dbconnection =
                       new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
                    historyLog.ModifiedBy = dbconnection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                        .FirstOrDefault();

                    var CreatByParam = new { oid = historyLog.CreatedBy };
                    historyLog.CreatedBy = dbconnection.Query<string>(ModifiedByUserQuery, CreatByParam)
                        .FirstOrDefault();
                }

                mBMLotHeaderGetDto.HistoryLog = historyLog;

                StandardMailDto mStandardMailDto;
                var standardMail = @"SELECT Id AS Value,Title AS Label FROM dbo.StandardMailHeader WHERE Id = @Id;";
                using (var cuconnetion = new SqlConnection(cuconnectionString))
                {
                    mStandardMailDto = cuconnetion
                        .Query<StandardMailDto>(standardMail, new { Id = mBMLotHeaderGetDto.StandardMailId })
                        .FirstOrDefault();
                }

                if (mStandardMailDto != null) mBMLotHeaderGetDto.StandardMail = mStandardMailDto;

                if (mBMLotHeaderGetDto.TenderBudget.IsNullOrEmpty())
                {
                    mBMLotHeaderGetDto.TenderBudget = mBMLotHeaderGetDto.TenderAwarding
                        .FirstOrDefault(x => x.isWinner == true)?.TotalBAFO.ToString();
                }
            }
        }

        return mBMLotHeaderGetDto;
    }

    public async Task<string> UpdateContractorWorkflow(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId, ContractorParameter.ProjectSequenceId,
            ContractorParameter.TenantProvider);
        // var options = new DbContextOptions<ApplicationDbContext>();
        // var applicationDbContext =
        //     new ApplicationDbContext(options, ContractorParameter.TenantProvider);

        var query =
            @"UPDATE dbo.ConstructorWorkFlow SET Division = @Division ,Lot = @Lot ,Contractor = @Contractor ,Price = @Price ,ModifiedDateTime = @ModifiedDateTime ,CreatedDateTime = @CreatedDateTime ,ModifiedBy = @ModifiedBy ,CreatedBy = @CreatedBy WHERE Id = @Id;";

        var delContractorSupplierList = @"DELETE FROM dbo.ContractorSupplierList WHERE ContractorId = @Id ;";
        var delContractorAccreditation = @"DELETE FROM dbo.ContractorAccreditation WHERE ContractorId = @Id ;";
        var delConstructorTeam = @"DELETE FROM dbo.ConstructorTeam WHERE ContractorId = @Id;";

        var ContractorSupplierList =
            @"INSERT INTO dbo.ContractorSupplierList ( Id ,ContractorId ,FileName ,CabPersonId ,CabPersonName ) VALUES ( @Id ,@ContractorId ,@FileName ,@CabPersonId ,@CabPersonName );";
        var ContractorAccreditation =
            @"INSERT INTO dbo.ContractorAccreditation ( Id ,ContractorId ,Skill ,experienceName ,CabPersonId ,CabPersonName ,ExperienceId ) VALUES ( @Id ,@ContractorId ,@Skill ,@experienceName ,@CabPersonId ,@CabPersonName ,@ExperienceId );";
        var ContractorTeam =
            @"INSERT INTO dbo.ConstructorTeam ( Id ,CabPearsonId ,ContractorId ,FullName ) VALUES ( @Id ,@CabPearsonId ,@ContractorId ,@FullName );";

        var parm = new
        {
            ContractorParameter.ConstructorLotInfoDto.Id,
            ContractorParameter.ConstructorLotInfoDto.Division,
            ContractorParameter.ConstructorLotInfoDto.Lot,
            ContractorParameter.ConstructorLotInfoDto.Contractor,
            ContractorParameter.ConstructorLotInfoDto.Price,
            ModifiedDateTime = DateTime.UtcNow,
            ModifiedBy = ContractorParameter.UserId,
            CreatedDateTime = DateTime.UtcNow,
            CreatedBy = ContractorParameter.UserId,
            Status = ContractorParameter.ConstructorLotInfoDto.Status.Key,
            CabCompanyId = ContractorParameter.ConstructorLotInfoDto.CompanyId
        };

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(query, parm);
            await connection.ExecuteAsync(delContractorSupplierList, parm);
            await connection.ExecuteAsync(delContractorAccreditation, parm);
            await connection.ExecuteAsync(delConstructorTeam, parm);

            var company = @"SELECT CompanyId FROM dbo.CabPersonCompany WHERE PersonId = @Id";
            CabPersonCompany mCabPersonCompany;

            if (ContractorParameter.ConstructorLotInfoDto.AccreditationAndSupplier.ContractorSupplierList != null)
                foreach (var i in ContractorParameter.ConstructorLotInfoDto.AccreditationAndSupplier
                             .ContractorSupplierList)
                {
                    var parm1 = new
                    {
                        Id = Guid.NewGuid().ToString(), ContractorId = ContractorParameter.ConstructorLotInfoDto.Id,
                        i.FileName, i.CabPersonId, i.CabPersonName
                    };
                    await connection.ExecuteAsync(ContractorSupplierList, parm1);
                }

            if (ContractorParameter.ConstructorLotInfoDto.AccreditationAndSupplier.ContractorAccreditation != null)
                foreach (var i in ContractorParameter.ConstructorLotInfoDto.AccreditationAndSupplier
                             .ContractorAccreditation)
                {
                    var parm1 = new
                    {
                        Id = Guid.NewGuid().ToString(), i.CabPersonId, i.CabPersonName, i.Skill, i.ExperienceId,
                        i.experienceName, ContractorId = ContractorParameter.ConstructorLotInfoDto.Id
                    };
                    await connection.ExecuteAsync(ContractorAccreditation, parm1);
                }

            if (ContractorParameter.ConstructorLotInfoDto.ConstructorTeamList != null)
                foreach (var i in ContractorParameter.ConstructorLotInfoDto.ConstructorTeamList)
                {
                    var isExist = connection.Query<string>(
                        "Select Id From dbo.ConstructorTeam Where CabPearsonId = @CabPearsonId AND  ContractorId = @ContractorId",
                        new
                        {
                            CabPearsonId = i.CabPersonId, ContractorId = ContractorParameter.ConstructorLotInfoDto.Id
                        }).Any();

                    if (!isExist)
                    {
                        var parm1 = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            ContractorId = ContractorParameter.ConstructorLotInfoDto.Id,
                            CabPearsonId = i.CabPersonId, FullName = i.CabPersonName
                        };
                        await connection.ExecuteAsync(ContractorTeam, parm1);
                    }
                }
        }

        return ContractorParameter.ConstructorLotInfoDto.SequenceId;
    }

    public async Task<List<PmolShortcutpaneDataDto>> ShortcutPaneData(ContractorParameter ContractorParameter)
    {
        var data = new List<PmolShortcutpaneDataDto>();

        var ShortcutpaneData1 = new PmolShortcutpaneDataDto();
        ShortcutpaneData1.Id = "1";
        ShortcutpaneData1.Name = "ShortcutpaneData1";
        ShortcutpaneData1.Type = "0";
        ShortcutpaneData1.Value = 1;

        data.Add(ShortcutpaneData1);

        var ShortcutpaneData2 = new PmolShortcutpaneDataDto();
        ShortcutpaneData2.Id = "2";
        ShortcutpaneData2.Name = "ShortcutpaneData2";
        ShortcutpaneData2.Type = "0";
        ShortcutpaneData2.Value = 2;

        data.Add(ShortcutpaneData2);

        var ShortcutpaneData3 = new PmolShortcutpaneDataDto();
        ShortcutpaneData3.Id = "3";
        ShortcutpaneData3.Name = "ShortcutpaneData3";
        ShortcutpaneData3.Type = "0";
        ShortcutpaneData3.Value = 3;

        data.Add(ShortcutpaneData3);

        return data;
    }

    public async Task<ConstructorWorkFlowDto> GetConstructorById(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        HistoryLogDto historyLog;
        var query =
            @"SELECT ConstructorWorkFlow.Id ,ConstructorWorkFlow.SequenceId ,ConstructorWorkFlow.Division ,ConstructorWorkFlow.Lot ,ConstructorWorkFlow.Contractor , ConstructorWorkFlow.IsInviteSend , ConstructorWorkFlow.Price ,ConstructorWorkFlow.ModifiedDateTime ,ConstructorWorkFlow.CreatedDateTime ,ConstructorWorkFlow.ModifiedBy ,ConstructorWorkFlow.CreatedBy ,ConstructorWorkFlow.StatusChangeDate ,ConstructorWorkFlow.CabCompanyId,ConstructorWorkFlow.Title,ContractorHeader.SequenceId AS LotSequenceId  ,ContractorHeader.StatusId As LotStatusId ,ContractorHeader.Title AS ContractTitle,ConstructorWorkFlow.TypeId,ConstructorWorkFlowStatus.StatusId AS [Key] ,ConstructorWorkFlowStatus.Name AS Text FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ConstructorWorkFlowStatus ON ConstructorWorkFlow.Status = ConstructorWorkFlowStatus.StatusId INNER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id WHERE ConstructorWorkFlow.SequenceId = @Id AND ConstructorWorkFlowStatus.LanguageCode = @lang";
        var ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

        var contractList =
            @"SELECT ContractorHeader.Id ,ContractorHeader.Title,ConstructorWorkFlow.Price ,ConstructorWorkFlow.CabCompanyId FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id WHERE ConstructorWorkFlow.Contractor = @name;
                SELECT ModifiedBy ,ModifiedDateTime ,CreatedBy ,CreatedDateTime FROM ConstructorWorkFlow WHERE Id = @Id;
                SELECT ContractorAccreditation.Id ,ContractorAccreditation.ContractorId ,ContractorAccreditation.Skill ,ContractorAccreditation.experienceName ,ContractorAccreditation.CabPersonId ,ContractorAccreditation.CabPersonName ,ContractorAccreditation.ExperienceId ,ConstructorWorkFlow.Contractor AS Company FROM ContractorAccreditation LEFT OUTER JOIN dbo.ConstructorWorkFlow ON ContractorAccreditation.ContractorId = ConstructorWorkFlow.Id WHERE ContractorAccreditation.ContractorId = @Id;
                SELECT ContractorSupplierList.Id ,ContractorSupplierList.ContractorId ,ContractorSupplierList.FileName ,ContractorSupplierList.CabPersonId ,ContractorSupplierList.CabPersonName ,ConstructorWorkFlow.Contractor AS Company FROM dbo.ContractorSupplierList LEFT OUTER JOIN dbo.ConstructorWorkFlow ON ContractorSupplierList.ContractorId = ConstructorWorkFlow.Id WHERE ContractorSupplierList.ContractorId = @Id;
                SELECT Id ,CabPearsonId AS CabPersonId,ContractorId ,FullName AS CabPersonName FROM dbo.ConstructorTeam WHERE ContractorId = @Id;
                SELECT sum(CAST(ConstructorWorkFlow.Price AS FLOAT)) FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id WHERE ConstructorWorkFlow.Contractor = @name;
                SELECT ConstructorWorkFlowStatus.Name AS StatusName ,ConstructorWfStatusChangeTime.* FROM dbo.ConstructorWfStatusChangeTime LEFT OUTER JOIN dbo.ConstructorWorkFlowStatus ON ConstructorWfStatusChangeTime.StatusId = ConstructorWorkFlowStatus.StatusId WHERE ConstructorWf = @Id AND ConstructorWorkFlowStatus.LanguageCode = @lang Order By DateTime Desc";

        var parameters = new { lang = ContractorParameter.Lang, ContractorParameter.Id };

        ConstructorWorkFlowDto mConstructorWorkFlowDto = null;
        using (var connection = new SqlConnection(connectionString))
        {
            mConstructorWorkFlowDto = connection
                .Query<ConstructorWorkFlowDto, ConstructorWorkFlowStatusDto, ConstructorWorkFlowDto>(
                    query,
                    (contractHeader, contractStatusDto) =>
                    {
                        contractHeader.Status = contractStatusDto;
                        return contractHeader;
                    }, parameters,
                    splitOn: "Key, Key").FirstOrDefault();

            if (mConstructorWorkFlowDto != null)
            {
                List<ContractList> mContractList;
                List<ContractorAccreditation> mContractorAccreditation;
                List<ContractorSupplierList> mContractorSupplierList;

                var mContractorAccreditationAndSupplierDto =
                    new ContractorAccreditationAndSupplierDto();

                var parameters1 = new
                {
                    lang = ContractorParameter.Lang,
                    mConstructorWorkFlowDto.Id,
                    name = mConstructorWorkFlowDto.Contractor
                };


                using (var multi = await connection.QueryMultipleAsync(contractList, parameters1))
                {
                    mContractList = multi.Read<ContractList>().ToList();
                    historyLog = multi.Read<HistoryLogDto>().FirstOrDefault();
                    mContractorAccreditationAndSupplierDto.ContractorAccreditation =
                        multi.Read<ContractorAccreditationDto>().ToList();
                    mContractorAccreditationAndSupplierDto.ContractorSupplierList =
                        multi.Read<ContractorSupplierListDto>().ToList();
                    mConstructorWorkFlowDto.ConstructorTeamList = multi.Read<ConstructorTeamList>().ToList();
                    var TotalPrice = multi.Read<float>().FirstOrDefault();
                    mConstructorWorkFlowDto.TimeTable = multi.Read<GetConstructorWfStatusChangeTime>().ToList();
                }

                mConstructorWorkFlowDto.Price = connection
                    .Query<string>(
                        "SELECT TotalBAFO FROM dbo.ContractorTotalValuesPublished WHERE LotId = (Select SequenceId From ContractorHeader Where Id = @Id) AND CompanyId = @CompanyId",
                        new { Id = mConstructorWorkFlowDto.Lot, CompanyId = mConstructorWorkFlowDto.CabCompanyId })
                    .FirstOrDefault() ?? "0";

                mConstructorWorkFlowDto.Version = connection
                    .Query<double>("Select Version From ConstructorWorkFlow Where SequenceId = @SequenceId",
                        new { SequenceId = ContractorParameter.Id }).FirstOrDefault().ToString("0.0");

                mConstructorWorkFlowDto.CabPersonId = connection
                    .Query<string>(
                        "SELECT CabPersonId FROM ContractorTeamList WHERE CompanyId = @CompanyId AND LotContractorId = @LotContractorId",
                        new
                        {
                            CompanyId = mConstructorWorkFlowDto.CabCompanyId,
                            LotContractorId = mConstructorWorkFlowDto.Lot
                        }).FirstOrDefault();
                foreach (var conList in mContractList)
                    conList.Price = connection
                        .Query<string>(
                            "SELECT TotalBAFO FROM dbo.ContractorTotalValuesPublished WHERE LotId = (Select SequenceId From ContractorHeader Where Id = @Id) AND CompanyId = @CompanyId",
                            new { conList.Id, CompanyId = conList.CabCompanyId }).FirstOrDefault() ?? "0";
                var TPrice = mContractList.Sum(x => x.Price.ToDouble());
                mConstructorWorkFlowDto.TotalPrice = TPrice.ToString("0.00");

                var company = @"SELECT Name FROM dbo.CabCompany WHERE Id = @Id";
                var companybyperson =
                    @"SELECT CabCompany.Name FROM dbo.CabPersonCompany INNER JOIN dbo.CabCompany ON CabPersonCompany.CompanyId = CabCompany.Id WHERE CabPersonCompany.PersonId = @Id";
                CabCompany mCabCompany;
                mConstructorWorkFlowDto.ContractList = mContractList;

                using (var dbconnection =
                       new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
                    historyLog.ModifiedBy = dbconnection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                        .FirstOrDefault();

                    var CreatByParam = new { oid = historyLog.CreatedBy };
                    historyLog.CreatedBy = dbconnection.Query<string>(ModifiedByUserQuery, CreatByParam)
                        .FirstOrDefault();
                    mCabCompany = dbconnection
                        .Query<CabCompany>(company, new { Id = mConstructorWorkFlowDto.CabCompanyId })
                        .FirstOrDefault();
                    Parallel.ForEach(mContractorAccreditationAndSupplierDto.ContractorAccreditation, i =>
                    {
                        i.Company = dbconnection.Query<string>(companybyperson, new { Id = i.CabPersonId })
                            .FirstOrDefault();
                    });
                }

                mConstructorWorkFlowDto.HistoryLog = historyLog;
                mConstructorWorkFlowDto.AccreditationAndSupplier = mContractorAccreditationAndSupplierDto;
                mConstructorWorkFlowDto.Contractor = mCabCompany.Name;
                mConstructorWorkFlowDto.LotDocLink = connection
                    .Query<string>(
                        "SELECT Link FROM ContractorLotUploadedDocs WHERE LotId = @Id AND CompanyId = @CompanyId",
                        new { Id = mConstructorWorkFlowDto.LotSequenceId, CompanyId = mConstructorWorkFlowDto.CabCompanyId })
                    .FirstOrDefault();
            }
        }

        return mConstructorWorkFlowDto;
    }

    public async Task<List<ContractorTeamList>> GetConstructorByTaxonomy(ContractorParameter ContractorParameter)
    {
        var query = @"SELECT
                      CabCompany.Name AS Company
                     ,CabPersonCompany.JobRole AS RoleName
                     ,CabPerson.FullName AS CabPersonName
                     ,CabPersonCompany.PersonId AS CabPersonId
                     ,CabPersonCompany.CompanyId
                    FROM dbo.CabContractorTaxonomycs
                    INNER JOIN dbo.CabPersonCompany
                      ON CabContractorTaxonomycs.CompanyId = CabPersonCompany.CompanyId
                    INNER JOIN dbo.CabPerson
                      ON CabPersonCompany.PersonId = CabPerson.Id
                    INNER JOIN dbo.CabCompany
                      ON CabPersonCompany.CompanyId = CabCompany.Id
                    WHERE CabContractorTaxonomycs.TaxonomyId IN @TaxonomyIds
                     ORDER BY RoleName DESC
                    ";

        var parm = new { TaxonomyIds = ContractorParameter.IdList };
        List<ContractorTeamList> data;
        var data1 = new List<ContractorTeamList>();
        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            data = (List<ContractorTeamList>)await connection.QueryAsync<ContractorTeamList>(query, parm);
           
        }

        var result = data.GroupBy(r => r.Company);
        foreach (var i in result)
        {
            var first = i.FirstOrDefault();
            data1.Add(first);
        }

        return data1;
    }

    public async Task<string> ExcelUpload(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            if (ContractorParameter.UploadExcelDto.ContractId != null)
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync("Delete From CBCExcelLotData Where ContractId = @ContractId ",
                        new { ContractorParameter.UploadExcelDto.ContractId });
                }

            var query =
                @"INSERT INTO dbo.CBCExcelLotData ( Id ,Title ,ParentId ,ArticleNo ,Quantity ,ContractId ) VALUES ( @Id ,@Title ,@ParentId ,@ArticleNo ,@Quantity ,@ContractId );";
            Parallel.ForEach(ContractorParameter.UploadExcelDto.ExcelData, async dto =>
            {
                var parm = new
                {
                    Id = Guid.NewGuid().ToString(),
                    dto.Title,
                    dto.ParentId,
                    ArticleNo = dto.Id,
                    dto.Quantity,
                    ContractorParameter.UploadExcelDto.ContractId
                };

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(query, parm);
                }
            });
            return "Ok";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> ExcelUploadTest(ContractorParameter ContractorParameter)
    {
        try
        {
            string lotName = null;
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


            var values = dbConnection.Query<FieldValues>(
                "Select ProjectDefinition.Title As ProjectTitle, CabCompany.Name As CompanyName From ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id   Where ProjectDefinition.SequenceCode = @SequenceCode",
                new { SequenceCode = ContractorParameter.ProjectSequenceId }).FirstOrDefault();

            if (ContractorParameter.UploadExcelDto.ContractId != null)
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync("Delete From CBCExcelLotData Where ContractId = @ContractId ",
                        new { ContractorParameter.UploadExcelDto.ContractId });

                    lotName = connection.Query<string>("SELECT Title FROM ContractorHeader WHERE Id = @Id",
                        new { Id = ContractorParameter.UploadExcelDto.ContractId }).FirstOrDefault();
                }

            var query =
                @"INSERT INTO dbo.CBCExcelLotData ( Id ,Title ,ParentId ,ArticleNo ,Quantity ,ContractId ,Unit, UnitPrice,TotalPrice,MeasurementCode , Mou , IsExclude ,Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5, RealArticleNo) VALUES ( @Id ,@Title ,@ParentId ,@ArticleNo ,@Quantity ,@ContractId ,@Unit, @UnitPrice,@TotalPrice , @MeasurementCode , @Mou , @IsExclude ,@Key1, @Value1, @Key2, @Value2, @Key3, @Value3 ,@Key4, @Value4, @Key5, @Value5, @RealArticleNo);";
            // Parallel.ForEach(ContractorParameter.UploadExcelDto.ExcelData, async dto =>
            // {
            foreach (var dto in ContractorParameter.UploadExcelDto.ExcelData)
            {
                var artNo = dto.Id;

                if (dto.Key1.IsNullOrEmpty())
                {
                    dto.Key1 = null;
                }
                if (dto.Value1.IsNullOrEmpty())
                {
                    dto.Value1 = null;
                }
                if (dto.Key2.IsNullOrEmpty())
                {
                    dto.Key2 = null;
                }
                if (dto.Value2.IsNullOrEmpty())
                {
                    dto.Value2 = null;
                }
                if (dto.Key3.IsNullOrEmpty())
                {
                    dto.Key3 = null;
                }
                if (dto.Value3.IsNullOrEmpty())
                {
                    dto.Value3 = null;
                }
                if (dto.Key4.IsNullOrEmpty())
                {
                    dto.Key4 = null;
                }
                if (dto.Value4.IsNullOrEmpty())
                {
                    dto.Value4 = null;
                }if (dto.Key5.IsNullOrEmpty())
                {
                    dto.Key5 = null;
                }
                if (dto.Value5.IsNullOrEmpty())
                {
                    dto.Value5 = null;
                }
                
                if (dto.Key1 != null && dto.Value1 != null)
                {
                    artNo = artNo + " / " +  dto.Value1.Trim();
                }
                if (dto.Key2 != null && dto.Value2 != null)
                {
                    artNo = artNo + " / " +  dto.Value2.Trim();
                }
                if (dto.Key3 != null && dto.Value3 != null)
                {
                    artNo = artNo + " / " +  dto.Value3.Trim();
                }
                if (dto.Key4 != null && dto.Value4 != null)
                {
                    artNo = artNo + " / " +  dto.Value4.Trim();
                }
                if (dto.Key5 != null && dto.Value5 != null)
                {
                    artNo = artNo + " / " +  dto.Value5.Trim();
                }

                var upperVal = ContractorParameter.Configuration.GetValue<string>("UpperVal");
                var lowerVal = ContractorParameter.Configuration.GetValue<string>("LowerVal");
                var isExclude = false;

                var parts = dto.Id.Split('.');
                var lastPart = parts[^1];

                if (int.TryParse(lastPart, out var lastPartInt))
                    if (lastPartInt >= int.Parse(lowerVal) && lastPartInt <= int.Parse(upperVal))
                        isExclude = true;

                dto.IsExclude = isExclude;

                var parm = new
                {
                    Id = Guid.NewGuid().ToString(),
                    dto.Title,
                    dto.ParentId,
                    ArticleNo = artNo,
                    dto.Quantity,
                    ContractorParameter.UploadExcelDto.ContractId,
                    dto.Unit,
                    dto.UnitPrice,
                    dto.TotalPrice,
                    dto.MeasurementCode,
                    dto.Mou,
                    IsExclude = isExclude,
                    dto.Key1,
                    dto.Value1,
                    dto.Key2,
                    dto.Value2,
                    dto.Key3,
                    dto.Value3,
                    dto.Key4,
                    dto.Value4,
                    dto.Key5,
                    dto.Value5,
                    RealArticleNo = dto.Id
                };

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(query, parm);
                }
            }
            //     });

            var total = ContractorParameter.UploadExcelDto.ExcelData.Where(v => v.IsExclude == false)
                .Sum(x => x.TotalPrice)?.ToString("0.00");

            ContractorParameter.Id = ContractorParameter.UploadExcelDto.ContractId;
            var uploadItem = SharePointUpload(ContractorParameter, total, "0", "1");

            return await uploadItem;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    //ContractorPsUpload
    public async Task<string> ContractorPsUpload(ContractorParameter ContractorParameter)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options, ContractorParameter.TenantProvider);
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


            if (ContractorParameter.ContractorPsUploadDto.ContractorId == null)
            {
                var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
                    new { Oid = ContractorParameter.UserId }).FirstOrDefault();
                ContractorParameter.ContractorPsUploadDto.ContractorId = companyId;
            }
            
            var isExist = connection.Query<string>(
                "Select Id From ContractorPs Where LotId = @LotId AND PsOrderNumber = @PsOrderNumber",
                new
                {
                    LotId = ContractorParameter.ContractorPsUploadDto.ContractId,
                    ContractorParameter.ContractorPsUploadDto.PsOrderNumber
                }).Any();

            
            var query =
                @"INSERT INTO dbo.ContractorPs ( Id , ArticleNumber, Title, MeasurementCode, QuantityQuotation, UnitPrice, QuantityConsumed, Total, LotId, CompanyId ,PsSequenceId , PsOrderNumber , IsApproved) VALUES ( @Id , @ArticleNumber, @Title, @MeasurementCode, @QuantityQuotation, @UnitPrice, @QuantityConsumed, @Total, @LotId, @CompanyId ,@PsSequenceId , @PsOrderNumber , @IsApproved);";
            // Parallel.ForEach(ContractorParameter.UploadExcelDto.ExcelData, async dto =>
            // {

            if (!isExist)
            {
                var idGenerator = new IdGenerator();
                var sequenceId = idGenerator.GenerateId(applicationDbContext, "LTPS-", "LotPsSequenceCode");

                ContractorParameter.ContractorPsUploadDto.PsOrderNumber ??= "1";

                foreach (var dto in ContractorParameter.ContractorPsUploadDto.ExcelData)
                {
                     var artNo = dto.ArticleNumber;
                
                if (dto.Key1.IsNullOrEmpty())
                {
                    dto.Key1 = null;
                }
                if (dto.Value1.IsNullOrEmpty())
                {
                    dto.Value1 = null;
                }
                if (dto.Key2.IsNullOrEmpty())
                {
                    dto.Key2 = null;
                }
                if (dto.Value2.IsNullOrEmpty())
                {
                    dto.Value2 = null;
                }
                if (dto.Key3.IsNullOrEmpty())
                {
                    dto.Key3 = null;
                }
                if (dto.Value3.IsNullOrEmpty())
                {
                    dto.Value3 = null;
                }
                if (dto.Key4.IsNullOrEmpty())
                {
                    dto.Key4 = null;
                }
                if (dto.Value4.IsNullOrEmpty())
                {
                    dto.Value4 = null;
                }if (dto.Key5.IsNullOrEmpty())
                {
                    dto.Key5 = null;
                }
                if (dto.Value5.IsNullOrEmpty())
                {
                    dto.Value5 = null;
                }
                
                if (dto.Key1 != null && dto.Value1 != null)
                {
                    artNo = artNo + " / " +  dto.Value1.Trim();
                }
                if (dto.Key2 != null && dto.Value2 != null)
                {
                    artNo = artNo + " / " +  dto.Value2.Trim();
                }
                if (dto.Key3 != null && dto.Value3 != null)
                {
                    artNo = artNo + " / " +  dto.Value3.Trim();
                }
                if (dto.Key4 != null && dto.Value4 != null)
                {
                    artNo = artNo + " / " +  dto.Value4.Trim();
                }
                if (dto.Key5 != null && dto.Value5 != null)
                {
                    artNo = artNo + " / " +  dto.Value5.Trim();
                }
                
                var parm = new
                {
                    Id = Guid.NewGuid().ToString(),
                    ArticleNumber = artNo,
                    dto.Title,
                    dto.MeasurementCode,
                    dto.QuantityQuotation,
                    dto.UnitPrice,
                    dto.QuantityConsumed,
                    dto.Total,
                    LotId = ContractorParameter.ContractorPsUploadDto.ContractId,
                    CompanyId = ContractorParameter.ContractorPsUploadDto.ContractorId,
                    PsSequenceId = sequenceId,
                    ContractorParameter.ContractorPsUploadDto.PsOrderNumber,
                    IsApproved = false
                };


                await connection.ExecuteAsync(query, parm);
                }
            }
            else
            {
                await connection.ExecuteAsync(
                    "Delete From ContractorPs Where LotId = @LotId AND PsOrderNumber = @PsOrderNumber",
                    new
                    {
                        LotId = ContractorParameter.ContractorPsUploadDto.ContractId,
                        ContractorParameter.ContractorPsUploadDto.PsOrderNumber
                    });

                var idGenerator = new IdGenerator();
                var sequenceId = idGenerator.GenerateId(applicationDbContext, "LTPS-", "LotPsSequenceCode");

                
                foreach (var dto in ContractorParameter.ContractorPsUploadDto.ExcelData)
                {
                    
                    var artNo = dto.ArticleNumber;
                
                if (dto.Key1.IsNullOrEmpty())
                {
                    dto.Key1 = null;
                }
                if (dto.Value1.IsNullOrEmpty())
                {
                    dto.Value1 = null;
                }
                if (dto.Key2.IsNullOrEmpty())
                {
                    dto.Key2 = null;
                }
                if (dto.Value2.IsNullOrEmpty())
                {
                    dto.Value2 = null;
                }
                if (dto.Key3.IsNullOrEmpty())
                {
                    dto.Key3 = null;
                }
                if (dto.Value3.IsNullOrEmpty())
                {
                    dto.Value3 = null;
                }
                if (dto.Key4.IsNullOrEmpty())
                {
                    dto.Key4 = null;
                }
                if (dto.Value4.IsNullOrEmpty())
                {
                    dto.Value4 = null;
                }if (dto.Key5.IsNullOrEmpty())
                {
                    dto.Key5 = null;
                }
                if (dto.Value5.IsNullOrEmpty())
                {
                    dto.Value5 = null;
                }
                
                if (dto.Key1 != null && dto.Value1 != null)
                {
                    artNo = artNo + " / " +  dto.Value1.Trim();
                }
                if (dto.Key2 != null && dto.Value2 != null)
                {
                    artNo = artNo + " / " +  dto.Value2.Trim();
                }
                if (dto.Key3 != null && dto.Value3 != null)
                {
                    artNo = artNo + " / " +  dto.Value3.Trim();
                }
                if (dto.Key4 != null && dto.Value4 != null)
                {
                    artNo = artNo + " / " +  dto.Value4.Trim();
                }
                if (dto.Key5 != null && dto.Value5 != null)
                {
                    artNo = artNo + " / " +  dto.Value5.Trim();
                }
                
                    var parm = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        ArticleNumber = artNo,
                        dto.Title,
                        dto.MeasurementCode,
                        dto.QuantityQuotation,
                        dto.UnitPrice,
                        dto.QuantityConsumed,
                        dto.Total,
                        LotId = ContractorParameter.ContractorPsUploadDto.ContractId,
                        CompanyId = ContractorParameter.ContractorPsUploadDto.ContractorId,
                        PsSequenceId = sequenceId,
                        ContractorParameter.ContractorPsUploadDto.PsOrderNumber,
                        IsApproved = false
                    };


                    await connection.ExecuteAsync(query, parm);
                }
            }
            //     });

            // var uploadedItem = await ContractorParameter.GraphServiceClient
            //     .Drive
            //     .Root
            //     //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
            //     .ItemWithPath(ContractorParameter.ContractorPsUploadDto.ContractId + "/" + ContractorParameter.File.FileName)
            //
            //     .Content
            //     .Request()
            //     .PutAsync<DriveItem>(ContractorParameter.File.OpenReadStream());
            //
            // return uploadedItem.WebUrl;

            return ContractorParameter.ContractorPsUploadDto.ContractId;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<ConstructorTeamList>> GetConstructorTeam(ContractorParameter ContractorParameter)
    {
        // var connectionString = ConnectionString.MapConnectionString(
        //     ContractorParameter.ContractingUnitSequenceId,
        //     ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var filter = ContractorParameter.PersonFilter;
        var query = @"SELECT
                              CabPerson.FullName AS CabPersonName
                             ,CabPerson.Id AS CabPersonId
                             ,CabCompany.Name AS CompanyName
                             ,CabPersonCompany.CompanyId
                             ,CabPersonCompany.JobRole AS JobTitle
                            FROM dbo.CabPersonCompany
                            INNER JOIN dbo.CabPerson
                              ON CabPersonCompany.PersonId = CabPerson.Id
                            INNER JOIN dbo.CabCompany
                              ON CabPersonCompany.CompanyId = CabCompany.Id
                            WHERE CabPersonCompany.CompanyId = @CompanyId
                            AND CabPerson.FullName LIKE '%" + filter.FullName + "%'";

        //string companyselect = @"SELECT CabCompanyId as CompanyId FROM dbo.ConstructorWorkFlow WHERE Lot = @Id";
        var parm = new { filter.CompanyId, Id = filter.LotId };
        IEnumerable<ConstructorTeamList> data;
        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            data = await connection.QueryAsync<ConstructorTeamList>(query, parm);
            
        }
        var mPersonCompanyDto = new PersonCompanyDto();
        var mCompanyDto = new CompanyDto();

        foreach (var i in data)
        {
            mPersonCompanyDto.Id = i.CabPersonId;
            mPersonCompanyDto.JobTitle = i.JobTitle;
            mCompanyDto.Id = i.CompanyId;
            mCompanyDto.Name = i.CompanyName;
            i.PersonCompany = mPersonCompanyDto;
            i.Company = mCompanyDto;
        }

        return data;
    }

    public async Task<IEnumerable<ConstructorTeamList>> LotPersonFilter(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var filter = ContractorParameter.PersonFilter;
        var query = @"SELECT
                              CabPerson.FullName AS CabPersonName
                             ,CabPerson.Id AS CabPersonId
                             ,CabCompany.Name AS CompanyName
                             ,CabPersonCompany.CompanyId
                             ,CabPersonCompany.JobRole AS JobTitle
                            FROM dbo.CabPersonCompany
                            INNER JOIN dbo.CabPerson
                              ON CabPersonCompany.PersonId = CabPerson.Id
                            INNER JOIN dbo.CabCompany
                              ON CabPersonCompany.CompanyId = CabCompany.Id
                            WHERE CabPerson.FullName LIKE '%" + filter.FullName +
                    "%'  AND CabPersonCompany.CompanyId NOT IN (@Company)";

        var companyselect = @"SELECT CabCompanyId as CompanyId FROM dbo.ConstructorWorkFlow WHERE Lot = @Id";
        var parm = new { filter.CompanyId, Id = filter.LotId, Company = ContractorParameter.Configuration.GetValue<string>("CompanyId")};
        IEnumerable<ConstructorTeamList> data;
        IEnumerable<ConstructorTeamList> company;
        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            data = await connection.QueryAsync<ConstructorTeamList>(query, parm);
        }

        using (var connection =
               new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            company = await connection.QueryAsync<ConstructorTeamList>(companyselect, parm);
            
        }

        foreach (var i in company) data = data.Where(a => a.CompanyId != i.CompanyId);


        foreach (var i in data)
        {
            var mPersonCompanyDto = new PersonCompanyDto();
            var mCompanyDto = new CompanyDto();
            mPersonCompanyDto.Id = i.CabPersonId;
            mPersonCompanyDto.JobTitle = i.JobTitle;
            mCompanyDto.Id = i.CompanyId;
            mCompanyDto.Name = i.CompanyName;
            i.PersonCompany = mPersonCompanyDto;
            i.Company = mCompanyDto;
        }

        return data;
    }

    public async Task<IEnumerable<ExcelLotDataDto>> GetCbcExcelLotData(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


        await using var connection = new SqlConnection(connectionString);

        var query =
            @"SELECT ArticleNo AS Id ,Title ,ParentId ,ArticleNo ,Quantity ,ContractId FROM dbo.CBCExcelLotData where ContractId = @ContractId;
                            SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC";
        // string query2 = @"SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LEN(ContractorPdfData.Unit) < 3 AND ISNUMERIC(ContractorPdfData.Unit) = 0 AND LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC ";
        //string query2 = @"SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC ";


        IEnumerable<ExcelLotDataDto> data;
        IEnumerable<Contractors> pdf;

        using (var multi = await connection.QueryMultipleAsync(query,
                   new
                   {
                       ContractId = ContractorParameter.Id
                   }))
        {
            data = multi.Read<ExcelLotDataDto>();
            pdf = multi.Read<Contractors>();
        }

        foreach (var lot in data)
        {
            var contractors = pdf.Where(c => c.ArticleNo == lot.ArticleNo || c.Title == lot.Title);
            lot.Contractors = contractors.DistinctBy(v => v.CompanyId);
        }


        return data.OrderBy(c => c.ArticleNo);
    }

    public async Task<List<ExcelLotDataDtoTest>> GetCbcExcelLotDataTest(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        await using var connection = new SqlConnection(connectionString);
        var query =
            @"SELECT ArticleNo AS Id ,Title , Id As UId ,ParentId ,ArticleNo ,Quantity ,ContractId,Unit,UnitPrice,TotalPrice ,MeasurementCode , Mou ,Key1, Value1, Key2, Value2, Key3, Value3,Key4, Value4, Key5, Value5, IsExclude, RealArticleNo  FROM dbo.CBCExcelLotData where ContractId = @ContractId ORDER BY ArticleNo ASC;
                            SELECT PublishedContractorsPdfData.Id ,PublishedContractorsPdfData.ArticleNo ,PublishedContractorsPdfData.CompanyId ,PublishedContractorsPdfData.Title ,PublishedContractorsPdfData.Unit ,PublishedContractorsPdfData.VH ,PublishedContractorsPdfData.Quantity ,PublishedContractorsPdfData.UnitPrice ,PublishedContractorsPdfData.TotalPrice,PublishedContractorsPdfData.RealArticleNo,Key1, Value1, Key2, Value2, Key3, Value3,Key4, Value4, Key5, Value5 FROM dbo.PublishedContractorsPdfData WHERE LotId =@ContractId ORDER BY PublishedContractorsPdfData.ArticleNo ASC";
        
        List<ExcelLotDataDtoTest> data;
        IEnumerable<ExcelLotDataDtoTest> pdf;
        var maxData = new List<ExcelLotDataDtoTest>();
        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();
        if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
        {
            using (var multi = await connection.QueryMultipleAsync(query,
                       new
                       {
                           ContractId = ContractorParameter.Id
                       }))
            {
                data = multi.Read<ExcelLotDataDtoTest>().ToList();
                pdf = multi.Read<ExcelLotDataDtoTest>();
            }

            var articleList = dbConnection.Query<string>("SELECT ProductId FROM dbo.PbsProduct").ToList();

            using var connection2 = new SqlConnection(connectionString);
            foreach (var lot in data)
            {
               
                var contractors = pdf.Where(c => c.ArticleNo == lot.ArticleNo);
                lot.Contractors = contractors.DistinctBy(v => v.CompanyId);
                var subTotal = connection2.Query<ExcelLotDataDtoTest>(@"WITH ret AS (SELECT * FROM CBCExcelLotData WHERE Id = @Id AND ContractId = @ContractId UNION ALL SELECT t.* FROM CBCExcelLotData t INNER JOIN ret r ON t.ParentId = r.RealArticleNo WHERE t.ContractId = @ContractId AND t.IsExclude = 0 ) SELECT * FROM ret",
                    new { Id = lot.UId, ContractId = ContractorParameter.Id });
                lot.SubTotal = subTotal?.Sum(x => x.TotalPrice.ToFloat());
                lot.IsParent = subTotal?.Count() > 1;
                var subList = subTotal.Select(x => x.ArticleNo).ToList();
                Parallel.ForEach(lot.Contractors, con =>
                {
                    var gg = pdf.Where(x =>
                        subList.Contains(x.ArticleNo) && x.CompanyId == con.CompanyId);

                    con.SubTotal = gg.Sum(x => x.TotalPrice.ToFloat());
                    con.IsParent = lot.IsParent;
                   
                });

                if (lot.Contractors.FirstOrDefault() != null)
                    if (lot.IsParent && !lot.RealArticleNo.Contains('.'))
                    {
                        var item = lot.Contractors?.MaxBy(x => x.SubTotal);
                        maxData.Add(item);
                    }
            }
            

            maxData = maxData.OrderByDescending(x => x.SubTotal).Take(4).ToList();
            var hh = maxData.Select(x => x.ArticleNo).ToList();

            var cbcData = data.Where(p => hh.Contains(p.ArticleNo)).ToList();
            cbcData.ForEach(c => c.IsMostExpensive = true);

            var errorsCbc = data.Where(p => articleList.All(p2 => p2 != p.RealArticleNo && p.IsExclude == false))
                .ToList();
            errorsCbc.ForEach(c => c.isError = true);
            var errors = pdf.Where(p => data.All(p2 => p2.ArticleNo != p.ArticleNo.Trim()))
                .ToList();
            errors.ForEach(c => c.isError = true);
            data.AddRange(errors);
        }
        else
        {
            data = await GetCbcExcelLotDataFilterContractor(ContractorParameter);
        }

        return data.OrderBy(c => c.ArticleNo).ToList();
    }

    public async Task<List<ExcelLotDataDtoTest>> GetCbcExcelLotDataTestForZeroState(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


        await using var connection = new SqlConnection(connectionString);

        var query =
            @"SELECT ArticleNo AS Id ,Title ,ParentId , Id As UId ,ArticleNo ,Quantity ,ContractId,Unit,UnitPrice,TotalPrice,MeasurementCode , Mou ,Key1, Value1, Key2, Value2, Key3, Value3,Key4, Value4, Key5, Value5, IsExclude, RealArticleNo FROM dbo.CBCExcelLotData where ContractId = @ContractId;
                            SELECT PublishedContractorsPdfData.Id ,PublishedContractorsPdfData.ArticleNo ,PublishedContractorsPdfData.CompanyId ,PublishedContractorsPdfData.Title ,PublishedContractorsPdfData.Unit ,PublishedContractorsPdfData.VH ,PublishedContractorsPdfData.Quantity ,PublishedContractorsPdfData.UnitPrice ,PublishedContractorsPdfData.TotalPrice ,PublishedContractorsPdfData.RealArticleNo,Key1, Value1, Key2, Value2, Key3, Value3,Key4, Value4, Key5, Value5 FROM dbo.PublishedContractorsPdfData WHERE LotId =@ContractId ORDER BY PublishedContractorsPdfData.ArticleNo DESC";
        // string query2 = @"SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LEN(ContractorPdfData.Unit) < 3 AND ISNUMERIC(ContractorPdfData.Unit) = 0 AND LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC ";
        //string query2 = @"SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC ";

        var isZero = @"SELECT CompanyId FROM dbo.ContractorTotalValuesPublished WHERE IsWinner = 1 AND LotId = @ContractId";

        var company = connection.Query<string>(isZero, new { ContractId = ContractorParameter.Id }).FirstOrDefault();

        List<ExcelLotDataDtoTest> data = null;
        IEnumerable<ExcelLotDataDtoTest> pdf;

        var maxData = new List<ExcelLotDataDtoTest>();


        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        if (company != null)
        {
            if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
            {
                using (var multi = await connection.QueryMultipleAsync(query,
                           new
                           {
                               ContractId = ContractorParameter.Id
                           }))
                {
                    data = multi.Read<ExcelLotDataDtoTest>().ToList();
                    pdf = multi.Read<ExcelLotDataDtoTest>();
                }

                pdf = pdf.Where(c => c.CompanyId == company);

                var articleList =
                    dbConnection.Query<string>("SELECT ProductId FROM dbo.PbsProduct").ToList();


                // Parallel.ForEach(data, lot => 
                // { 
                using var connection2 = new SqlConnection(connectionString);
                foreach (var lot in data)
                {
                   
                    var contractors = pdf.Where(c => c.ArticleNo == lot.ArticleNo);
                    lot.Contractors = contractors.DistinctBy(v => v.CompanyId);

                    var subTotal = connection2.Query<ExcelLotDataDtoTest>(@"WITH ret
                                                                            AS
                                                                            (SELECT
                                                                            *
                                                                            FROM CBCExcelLotData
                                                                            WHERE Id = @Id
                                                                            AND ContractId = @ContractId
                                                                            UNION ALL
                                                                            SELECT
                                                                            t.*
                                                                            FROM CBCExcelLotData t
                                                                            INNER JOIN ret r
                                                                            ON t.ParentId = r.RealArticleNo
                                                                            WHERE t.ContractId = @ContractId AND t.IsExclude = 0 )
                                                                            SELECT
                                                                            *
                                                                            FROM ret",
                        new { Id = lot.UId, ContractId = ContractorParameter.Id });

                    lot.SubTotal = subTotal?.Sum(x => x.TotalPrice.ToFloat());
                    lot.IsParent = subTotal?.Count() > 1;
                    var subList = subTotal.Select(x => x.ArticleNo).ToList();

                    Parallel.ForEach(lot.Contractors, con =>
                    {
                        using var connection3 = new SqlConnection(connectionString);
                        // foreach (var con in lot.Contractors)
                        // {
                        // var gg = pdf.Where(x => subTotal.All(v => v.ArticleNo == x.ArticleNo))
                        //     .Where(b => b.CompanyId == con.CompanyId);
                        // var gg = connection3.Query<ExcelLotDataDtoTest>(
                        //     "Select * from PublishedContractorsPdfData Where ArticleNo In @ArticleNos AND LotId =@ContractId AND CompanyId = @CompanyId",
                        //     new
                        //     {
                        //         ArticleNos = subList, ContractId = ContractorParameter.Id,
                        //         CompanyId = con.CompanyId
                        //     });

                        var gg = pdf.Where(x =>
                            subList.Contains(x.ArticleNo) && x.CompanyId == con.CompanyId);

                        con.SubTotal = gg.Sum(x => x.TotalPrice.ToFloat());
                        con.IsParent = lot.IsParent;
                        //}
                    });

                    if (lot.Contractors.FirstOrDefault() != null)
                        if (lot.IsParent && !lot.RealArticleNo.Contains('.'))
                        {
                            var item = lot.Contractors?.MaxBy(x => x.SubTotal);
                            maxData.Add(item);
                        }
                }
                // });

                maxData = maxData.OrderByDescending(x => x.SubTotal).Take(4).ToList();
                var hh = maxData.Select(x => x.ArticleNo).ToList();

                var cbcData = data.Where(p => hh.Contains(p.ArticleNo)).ToList();
                cbcData.ForEach(c => c.IsMostExpensive = true);

                var errorsCbc = data.Where(p => articleList.All(p2 => p2 != p.RealArticleNo && p.IsExclude == false))
                    .ToList();
                errorsCbc.ForEach(c => c.isError = true);

                //data.AddRange(errorsCbc);

                var errors = pdf.Where(p => data.All(p2 => p2.ArticleNo != p.ArticleNo.Trim()))
                    .ToList();

                errors.ForEach(c => c.isError = true);

                data.AddRange(errors);

                //return data.OrderBy(c => c.ArticleNo).ToList();
            }
            else
            {
                data = await GetCbcExcelLotDataFilterContractor(ContractorParameter);
            }

            return data.OrderBy(c => c.ArticleNo).ToList();
        }

        return data;
    }

    public async Task<List<ExcelLotDataDtoTest>> GetCbcExcelLotDataFilterContractor(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


        await using var connection = new SqlConnection(connectionString);

        var query =
            @"SELECT ArticleNo AS Id ,Title ,ParentId , Id As UId ,ArticleNo ,Quantity ,ContractId,Unit,UnitPrice,TotalPrice , MeasurementCode , Mou ,Key1, Value1, Key2, Value2, Key3, Value3,Key4, Value4, Key5, Value5, IsExclude, RealArticleNo FROM dbo.CBCExcelLotdataPublished where ContractId = @ContractId;
                            SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice ,ContractorPdfData.RealArticleNo,Key1, Value1, Key2, Value2, Key3, Value3,Key4, Value4, Key5, Value5 FROM dbo.ContractorPdfData WHERE LotId =@ContractId AND CompanyId = @CompanyId  ORDER BY ContractorPdfData.ArticleNo DESC";
        // string query2 = @"SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LEN(ContractorPdfData.Unit) < 3 AND ISNUMERIC(ContractorPdfData.Unit) = 0 AND LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC ";
        //string query2 = @"SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC ";


        List<ExcelLotDataDtoTest> data = null;
        IEnumerable<ExcelLotDataDtoTest> pdf;

        var maxData = new List<ExcelLotDataDtoTest>();


        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        if (companyId != null)
        {
            using (var multi = await connection.QueryMultipleAsync(query,
                       new
                       {
                           ContractId = ContractorParameter.Id,
                           CompanyId = companyId
                       }))
            {
                data = multi.Read<ExcelLotDataDtoTest>().ToList();
                pdf = multi.Read<ExcelLotDataDtoTest>();
            }

            var articleList = dbConnection.Query<string>("SELECT ProductId FROM dbo.PbsProduct").ToList();

            // Parallel.ForEach(data, lot =>
            // {
            using var connection2 = new SqlConnection(connectionString);
            foreach (var lot in data)
            {
                var contractors = pdf.Where(c => c.ArticleNo == lot.ArticleNo);
                lot.Contractors = contractors.DistinctBy(v => v.CompanyId);

                var subTotal = connection2.Query<ExcelLotDataDtoTest>(@"WITH ret
                                                                            AS
                                                                            (SELECT
                                                                            *
                                                                            FROM CBCExcelLotdataPublished
                                                                            WHERE Id = @Id
                                                                            AND ContractId = @ContractId
                                                                            UNION ALL
                                                                            SELECT
                                                                            t.*
                                                                            FROM CBCExcelLotdataPublished t
                                                                            INNER JOIN ret r
                                                                            ON t.ParentId = r.RealArticleNo
                                                                            WHERE t.ContractId = @ContractId AND t.IsExclude = 0)
                                                                            SELECT
                                                                            *
                                                                            FROM ret",
                    new { Id = lot.UId, ContractId = ContractorParameter.Id }).ToList();

                lot.SubTotal = subTotal?.Sum(x => x.TotalPrice.ToFloat());
                lot.IsParent = subTotal?.Count() > 1;
                var subList = subTotal.Select(x => x.ArticleNo).ToList();

                Parallel.ForEach(lot.Contractors, con =>
                {
                    using var connection3 = new SqlConnection(connectionString);
                    // foreach (var con in lot.Contractors)
                    // {
                    // var jj = pdf.Where(x => subTotal.All(v => v.ArticleNo == x.ArticleNo || v.Title == x.Title));
                    // var gg = pdf.Where(x => subTotal.All(v => v.ArticleNo == x.ArticleNo))
                    //     .Where(b => b.CompanyId == con.CompanyId);
                    // var gg = connection3.Query<ExcelLotDataDtoTest>(
                    //     "Select * from ContractorPdfData Where ArticleNo In @ArticleNos AND LotId =@ContractId AND CompanyId = @CompanyId",
                    //     new
                    //     {
                    //         ArticleNos = subList, ContractId = ContractorParameter.Id,
                    //         CompanyId = con.CompanyId
                    //     });

                    var gg = pdf.Where(x =>
                        subList.Contains(x.ArticleNo) && x.CompanyId == con.CompanyId);

                    con.SubTotal = gg.Sum(x => x.TotalPrice.ToFloat());
                    con.IsParent = lot.IsParent;
                    //}
                });

                if (lot.Contractors.FirstOrDefault() != null)
                    if (lot.IsParent && !lot.RealArticleNo.Contains('.'))
                    {
                        var item = lot.Contractors?.MaxBy(x => x.SubTotal);
                        maxData.Add(item);
                    }
            }
            // });

            // });

            maxData = maxData.OrderByDescending(x => x.SubTotal).Take(4).ToList();
            var hh = maxData.Select(x => x.ArticleNo).ToList();

            var cbcData = data.Where(p => hh.Contains(p.ArticleNo)).ToList();
            cbcData.ForEach(c => c.IsMostExpensive = true);

            var errorsCbc = data.Where(p => articleList.All(p2 => p2 != p.RealArticleNo && p.IsExclude == false))
                .ToList();
            errorsCbc.ForEach(c => c.isError = true);

            //data.AddRange(errorsCbc);

            var errors = pdf.Where(p => data.All(p2 => p2.ArticleNo != p.ArticleNo.Trim()))
                .ToList();

            errors.ForEach(c => c.isError = true);

            data.AddRange(errors);
        }

        return data.OrderBy(c => c.ArticleNo).ToList();
    }


    public async Task<List<ContractorsCompanyList>> GetContractorsByLotId(ContractorParameter ContractorParameter)
    {
        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var connection = new SqlConnection(connectionString);

        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();
        List<ContractorsCompanyList> data = null;

        if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
        {
            var pdfToExcelParameter = new PdfToExcelParameter
            {
                ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId,
                ProjectSequenceId = ContractorParameter.ProjectSequenceId,
                TenantProvider = ContractorParameter.TenantProvider,
                Id = ContractorParameter.Id,
                UserId = ContractorParameter.UserId,
                Configuration = ContractorParameter.Configuration
            };

            var errorList =
                await ContractorParameter._pdfToExcelRepository
                    .ContractorPdfErrorLogGetByLotId(pdfToExcelParameter);


            data = connection.Query<ContractorsCompanyList>(
                "SELECT DISTINCT CompanyId FROM PublishedContractorsPdfData WHERE LotId =@LotId",
                new { LotId = ContractorParameter.Id }).ToList();

            if (data.Any())
                foreach (var company in data)
                {
                    company.CompanyName = dbConnection.Query<string>("SELECT name FROM CabCompany WHERE Id = @Id",
                        new { Id = company.CompanyId }).FirstOrDefault();

                    company.Errors = errorList.Exists(c => c.CompanyId == company.CompanyId);

                    var contractorData = connection
                        .Query<ContractorTotalValuesDto>(
                            "Select * From ContractorTotalValuesPublished Where LotId = @LotId AND CompanyId = @CompanyId",
                            new { company.CompanyId, LotId = ContractorParameter.Id }).FirstOrDefault();
                    var tc = connection
                        .Query<string>(
                            "SELECT SUM(TotalPrice) AS TotalPriceSum   FROM PublishedContractorsPdfData  WHERE LotId = @LotId AND CompanyId = @CompanyId",
                            new { LotId = ContractorParameter.Id, company.CompanyId })
                        .FirstOrDefault();

                    if (tc != null)
                        company.TotalCost = tc.ToFloat();
                    else
                        company.TotalCost = 0;

                    if (contractorData != null)
                    {
                        company.TotalBAFO = contractorData.TotalBAFO;
                        company.isWinner = contractorData.IsWinner;
                    }
                    else
                    {
                        company.TotalBAFO = company.TotalCost;
                    }
                }
        }
        else
        {
            data = await GetContractorsByLotIdFilterContractor(ContractorParameter);
        }

        return data;
    }

    public async Task<IEnumerable<ContractorsCompanyList>> GetContractorsByLotIdForZeroState(
        ContractorParameter ContractorParameter)
    {
        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var connection = new SqlConnection(connectionString);

        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();
        List<ContractorsCompanyList> data = null;

        if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
        {
            var pdfToExcelParameter = new PdfToExcelParameter
            {
                ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId,
                ProjectSequenceId = ContractorParameter.ProjectSequenceId,
                TenantProvider = ContractorParameter.TenantProvider,
                Id = ContractorParameter.Id,
                UserId = ContractorParameter.UserId,
                Configuration = ContractorParameter.Configuration
            };

            var errorList =
                await ContractorParameter._pdfToExcelRepository
                    .ContractorPdfErrorLogGetByLotId(pdfToExcelParameter);


            data = connection.Query<ContractorsCompanyList>(
                "SELECT DISTINCT PublishedContractorsPdfData.CompanyId FROM dbo.PublishedContractorsPdfData LEFT OUTER JOIN dbo.ContractorTotalValuesPublished ON PublishedContractorsPdfData.LotId = ContractorTotalValuesPublished.LotId WHERE PublishedContractorsPdfData.LotId = @LotId AND ContractorTotalValuesPublished.IsWinner = 1",
                new { LotId = ContractorParameter.Id }).ToList();

            if (data.Any())
                foreach (var company in data)
                {
                    company.CompanyName = dbConnection.Query<string>("SELECT name FROM CabCompany WHERE Id = @Id",
                        new { Id = company.CompanyId }).FirstOrDefault();

                    company.Errors = errorList.Exists(c => c.CompanyId == company.CompanyId);

                    company.FileType = connection
                        .Query<string>(
                            @"SELECT FileType FROM ContractorUploadedFiles WHERE LotId = @LotId AND CompanyId = @CompanyId",
                            new { company.CompanyId, LotId = ContractorParameter.Id }).FirstOrDefault();

                    var contractorData = connection
                        .Query<ContractorTotalValuesDto>(
                            "Select * From ContractorTotalValuesPublished Where LotId = @LotId AND CompanyId = @CompanyId",
                            new { company.CompanyId, LotId = ContractorParameter.Id }).FirstOrDefault();
                    var tc = connection
                        .Query<string>(
                            "SELECT SUM(TotalPrice) AS TotalPriceSum   FROM PublishedContractorsPdfData  WHERE LotId = @LotId AND CompanyId = @CompanyId",
                            new { LotId = ContractorParameter.Id, company.CompanyId })
                        .FirstOrDefault();

                    if (tc != null)
                        company.TotalCost = tc.ToFloat();
                    else
                        company.TotalCost = 0;
                    if (contractorData != null)
                    {
                        company.TotalBAFO = contractorData.TotalBAFO;
                        company.isWinner = contractorData.IsWinner;
                    }
                    else
                    {
                        company.TotalBAFO = company.TotalCost;
                    }
                }
        }
        else
        {
            data = await GetContractorsByLotIdFilterContractor(ContractorParameter);
        }

        var data1 = data.Where(a => a.isWinner);
        return data1;
    }

    public async Task<List<ContractorsCompanyList>> GetContractorsByLotIdFilterContractor(
        ContractorParameter ContractorParameter)
    {
        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var connection = new SqlConnection(connectionString);

        var pdfToExcelParameter = new PdfToExcelParameter
        {
            ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId,
            ProjectSequenceId = ContractorParameter.ProjectSequenceId,
            TenantProvider = ContractorParameter.TenantProvider,
            Id = ContractorParameter.Id,
            UserId = ContractorParameter.UserId,
            Configuration = ContractorParameter.Configuration
        };
        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();
        List<ContractorsCompanyList> data = null;

        if (companyId != null)
        {
            var errorList =
                await ContractorParameter._pdfToExcelRepository
                    .ContractorPdfErrorLogGetByLotIdFilterContractor(pdfToExcelParameter);


            data = connection.Query<ContractorsCompanyList>(
                "SELECT DISTINCT CompanyId FROM ContractorPdfData WHERE LotId =@LotId AND CompanyId = @CompanyId",
                new { LotId = ContractorParameter.Id, CompanyId = companyId }).ToList();

            if (data.Any())
                foreach (var company in data)
                {
                    company.CompanyName = dbConnection.Query<string>("SELECT name FROM CabCompany WHERE Id = @Id",
                        new { Id = company.CompanyId }).FirstOrDefault();

                    company.Errors = errorList.Exists(c => c.CompanyId == company.CompanyId);

                    company.FileType = connection
                        .Query<string>(
                            @"SELECT FileType FROM ContractorUploadedFiles WHERE LotId = @LotId AND CompanyId = @CompanyId",
                            new { company.CompanyId, LotId = ContractorParameter.Id }).FirstOrDefault();

                    var contractorData = connection
                        .Query<ContractorTotalValuesDto>(
                            "Select * From ContractorTotalValues Where LotId = @LotId AND CompanyId = @CompanyId",
                            new { company.CompanyId, LotId = ContractorParameter.Id }).FirstOrDefault();

                    var tc = connection
                        .Query<string>(
                            "SELECT SUM(TotalPrice) AS TotalPriceSum   FROM ContractorPdfData  WHERE LotId = @LotId AND CompanyId = @CompanyId",
                            new { LotId = ContractorParameter.Id, company.CompanyId })
                        .FirstOrDefault();

                    if (tc != null)
                        company.TotalCost = tc.ToFloat();
                    else
                        company.TotalCost = 0;

                    if (contractorData != null)
                    {
                        company.TotalBAFO = contractorData.TotalBAFO;
                        company.isWinner = contractorData.IsWinner;
                    }
                    else
                    {
                        company.TotalBAFO = company.TotalCost;
                    }
                }
        }

        return data;
    }

    public async Task<string> ConstructorWorkFlowDelete(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var query =
                @"DELETE FROM dbo.ConstructorWorkFlow WHERE Lot = @Lot AND CabCompanyId = @CabCompanyId;";


            foreach (var i in ContractorParameter.ConstructorWorkFlowDelete.CabCompanyId)
            {
                var parm = new
                {
                    ContractorParameter.ConstructorWorkFlowDelete.Lot,
                    CabCompanyId = i
                };

                await using (var connection = new SqlConnection(connectionString))
                {
                    await connection.ExecuteAsync(query, parm);
                }
            }

            return ContractorParameter.ConstructorWorkFlowDelete.Lot;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<string>> GetContractorsErrorListByLotId(ContractorParameter ContractorParameter)
    {
        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var connection = new SqlConnection(connectionString);


        List<string> data;
        List<string> results;


        data = dbConnection.Query<string>(
            "SELECT ProductId FROM PbsProduct").ToList();


        results = connection.Query<string>(
            "SELECT ArticleNo FROM CBCExcelLotData WHERE ContractId = @Id AND ArticleNo NOT IN @ArticleNos",
            new { ContractorParameter.Id, ArticleNos = data }).ToList();


        return results;
    }

    public async Task<string> CrateCommentCard(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        CabPersonCompany CabPersonCompany;
        var company = @"SELECT CompanyId FROM dbo.CabPersonCompany WHERE Oid = @Id";
        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            CabPersonCompany = connection.Query<CabPersonCompany>(company, new { Id = ContractorParameter.UserId })
                .FirstOrDefault();
        }

        string ContractorId = null;
        if (ContractorParameter.CommentCard.CompanyId == null)
            ContractorId = CabPersonCompany.CompanyId;
        else
            ContractorId = ContractorParameter.CommentCard.CompanyId;

        CommentCard CommentCard;
        CommentCardContractor mCommentCardContractor;
        var checkArticleNo =
            @"SELECT Id,ArticleNo FROM dbo.CommentCard WHERE ArticleNo = @ArticleNo AND  CommentCard.ColumnName = @ColumnName AND LotId = @LotId";
        var checkContractor =
            @"SELECT CommentCardContractor.Id ,CommentCardContractor.CommentCardId ,CommentCardContractor.ContractorId FROM dbo.CommentCardContractor INNER JOIN dbo.CommentCard ON CommentCardContractor.CommentCardId = CommentCard.Id WHERE CommentCardContractor.ContractorId = @Id AND CommentCard.ArticleNo = @ArticleNo AND CommentCard.ColumnName = @ColumnName AND CommentCardContractor.Accept = '0' AND CommentCard.LotId = @LotId";
        var insertCommentCard =
            @"INSERT INTO dbo.CommentCard ( Id ,Title ,ArticleNo ,CardTitle ,LotId,ColumnName,Date ) VALUES ( @Id ,@Title ,@ArticleNo ,@CardTitle ,@LotId,@ColumnName,@Date );";

        var insertContractor =
            @"INSERT INTO dbo.CommentCardContractor ( Id ,CommentCardId ,ContractorId ,Accept,Field ,Priority ,Severity ,Status,Reporter,CreaterId,ChangeType ) VALUES ( @Id ,@CommentCardId ,@ContractorId ,@Accept,@Field ,@Priority ,@Severity ,@Status,@Reporter,@CreaterId,@ChangeType );";

        string Id = null;
        await using (var connection = new SqlConnection(connectionString))
        {
            CommentCard = connection.Query<CommentCard>(checkArticleNo,
                new
                {
                    ContractorParameter.CommentCard.ArticleNo, ContractorParameter.CommentCard.ColumnName,
                    ContractorParameter.CommentCard.LotId
                }).FirstOrDefault();
            mCommentCardContractor = connection.Query<CommentCardContractor>(checkContractor,
                new
                {
                    Id = ContractorId, ContractorParameter.CommentCard.ArticleNo,
                    ContractorParameter.CommentCard.ColumnName, ContractorParameter.CommentCard.LotId
                }).FirstOrDefault();
            if (CommentCard == null)
            {
                Id = Guid.NewGuid().ToString();
                var parm = new
                {
                    Id,
                    ContractorParameter.CommentCard.Title,
                    ContractorParameter.CommentCard.ArticleNo,
                    ContractorParameter.CommentCard.LotId,
                    CardTitle = ContractorParameter.CommentCard.ArticleNo + " " +
                                ContractorParameter.CommentCard.Title + " " +
                                ContractorParameter.CommentCard.ColumnName,
                    ContractorParameter.CommentCard.ColumnName,
                    Date = DateTime.UtcNow
                };
                await connection.ExecuteAsync(insertCommentCard, parm);
            }
            else
            {
                Id = CommentCard.Id;
            }

            if (mCommentCardContractor == null)
            {
                var parm1 = new
                {
                    Id = Guid.NewGuid().ToString(),
                    CommentCardId = Id,
                    ContractorId,
                    Field = "7bcb4e8d-Field-487d-cowf-6b91c89fAcce",
                    Priority = "7bcb4e8d-Fiel1-Very-cowf-6b91c89fAcce",
                    Severity = "7bcb4e8d-Field-Seve-cowf-6b91c89fAcce",
                    Status = "7bcb4e8d-8e8c-487d-cowf-6b91c89fAcce",
                    Reporter = ContractorParameter.UserId,
                    CreaterId = CabPersonCompany.CompanyId,
                    Accept = "0",
                    ChangeType = "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv"
                };

                await connection.ExecuteAsync(insertContractor, parm1);
            }
        }

        return Id;
    }

    public async Task<string> AddComment(ContractorParameter ContractorParameter,
        ISendGridRepositorie ISendGridRepositorie)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var cuconnectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            null, ContractorParameter.TenantProvider);

        var insert =
            @"MERGE INTO dbo.ContractorComment t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE SET PersonId = @PersonId ,MESSAGE = @Message ,CommentCardContractorsId = @CommentCardContractorsId,TimeStamp = @TimeStamp WHEN NOT MATCHED THEN INSERT (Id ,PersonId ,Message ,CommentCardContractorsId,TimeStamp) VALUES (@Id ,@PersonId ,@Message ,@CommentCardContractorsId,@TimeStamp);";
        var user =
            @"SELECT CommentCardContractor.Assigner as Id, ContractorHeader.SequenceId, ContractorHeader.StandardMailId, ContractorHeader.StartDate, ContractorHeader.EndDate,ContractorHeader.Name FROM dbo.CommentCardContractor LEFT OUTER JOIN dbo.CommentCard ON CommentCardContractor.CommentCardId = CommentCard.Id LEFT OUTER JOIN dbo.ContractorHeader ON CommentCard.LotId = ContractorHeader.SequenceId LEFT OUTER JOIN dbo.ContractorTeamList ON ContractorHeader.Id = ContractorTeamList.LotContractorId WHERE CommentCardContractor.Id = @Id;";
        ContractorHeader mContractorHeader;
        var parm = new
        {
            Id = Guid.NewGuid().ToString(),
            PersonId = ContractorParameter.UserId,
            ContractorParameter.ContractorComment.Message,
            ContractorParameter.ContractorComment.CommentCardContractorsId,
            TimeStamp = GetTimestamp(DateTime.Now)
        };
        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(insert, parm);
            mContractorHeader = connection
                .Query<ContractorHeader>(user,
                    new { Id = ContractorParameter.ContractorComment.CommentCardContractorsId }).FirstOrDefault();
        }
        
        

        StandardMailHeader mStandardMailHeader;
        if (mContractorHeader?.StandardMailId != null)
            await using (var cuconnection = new SqlConnection(cuconnectionString))
            {
                mStandardMailHeader = cuconnection
                    .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id",
                        new { Id = mContractorHeader.StandardMailId }).FirstOrDefault();
            }

        else
            await using (var cuconnection = new SqlConnection(cuconnectionString))
            {
                mStandardMailHeader = cuconnection
                    .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader where IsDefault = 1")
                    .FirstOrDefault();
            }

        if (mContractorHeader != null)
        {
            if (mContractorHeader.Id == "all")
            {
                await using (var connection = new SqlConnection(connectionString))
                {
                    var contractPersons = connection.Query<string>(
                        "SELECT CabPersonId FROM ContractorTeamList  LEFT OUTER JOIN ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE  ContractorHeader.SequenceId = @lotId",
                        new { lotId = mContractorHeader.SequenceId }).ToList();

                    foreach (var person in contractPersons)
                    {
                        
                        bool issend;
                        var sendGridParameter = new SendGridParameter();
                        sendGridParameter.Id = person;
                        sendGridParameter.TenantProvider = ContractorParameter.TenantProvider;
                        sendGridParameter.ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId;
                        sendGridParameter.ProjectSequenceId = ContractorParameter.ProjectSequenceId;
                        sendGridParameter.Lot = mContractorHeader.SequenceId;
                        sendGridParameter.Lang = ContractorParameter.Lang;
                        sendGridParameter.MailBody = ContractorParameter.ContractorComment.Message;
                        sendGridParameter.LotTitle = mContractorHeader.Name;
                        sendGridParameter.StartDate = mContractorHeader.StartDate?.ToString("dd/MM/yyyy");
                        sendGridParameter.EndDate = mContractorHeader.EndDate?.ToString("dd/MM/yyyy");
                        sendGridParameter.Subject = mContractorHeader.Title + " " + "Out Standing Comments";
                        sendGridParameter.Configuration = ContractorParameter.Configuration;
                        
                        var projLang = connection.Query<string>(
                            "SELECT pl.Name FROM ProjectDefinition LEFT OUTER JOIN ProjectLanguage pl ON ProjectDefinition.Language = pl.TypeId WHERE ProjectDefinition.SequenceCode = @Id",
                            new { Id = ContractorParameter.ProjectSequenceId }).FirstOrDefault();

                        sendGridParameter.TemplateId = projLang switch
                        {
                            "en" or null =>
                                ContractorParameter.Configuration.GetValue<string>("TemplateId_en1"),
                            "nl" =>  ContractorParameter.Configuration.GetValue<string>("TemplateId_nl1"),
                            _ => sendGridParameter.TemplateId
                        };
                        // if (ContractorParameter.Lang == "en")
                        //     sendGridParameter.TemplateId =
                        //         ContractorParameter.Configuration.GetValue<string>("TemplateId_en1");
                        // if (ContractorParameter.Lang == "nl")
                        //     sendGridParameter.TemplateId =
                        //         ContractorParameter.Configuration.GetValue<string>("TemplateId_nl1");
                        issend = await ContractorParameter.SendGridRepositorie.SendInvitation(sendGridParameter);
                    }
                }
            }
            else
            {
                await using var connection = new SqlConnection(connectionString);

                bool issend;
                var sendGridParameter = new SendGridParameter();
                sendGridParameter.Id = mContractorHeader.Id;
                sendGridParameter.TenantProvider = ContractorParameter.TenantProvider;
                sendGridParameter.ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId;
                sendGridParameter.ProjectSequenceId = ContractorParameter.ProjectSequenceId;
                sendGridParameter.Lot = mContractorHeader.SequenceId;
                sendGridParameter.Lang = ContractorParameter.Lang;
                sendGridParameter.MailBody = ContractorParameter.ContractorComment.Message;
                sendGridParameter.LotTitle = mContractorHeader.Name;
                sendGridParameter.StartDate = mContractorHeader.StartDate?.ToString("dd/MM/yyyy");
                sendGridParameter.EndDate = mContractorHeader.EndDate?.ToString("dd/MM/yyyy");
                sendGridParameter.Subject = mContractorHeader.Title + " " + "Out Standing Comments";
                sendGridParameter.Configuration = ContractorParameter.Configuration;
                
                var projLang = connection.Query<string>(
                    "SELECT pl.Name FROM ProjectDefinition LEFT OUTER JOIN ProjectLanguage pl ON ProjectDefinition.Language = pl.TypeId WHERE ProjectDefinition.SequenceCode = @Id",
                    new { Id = ContractorParameter.ProjectSequenceId }).FirstOrDefault();

                sendGridParameter.TemplateId = projLang switch
                {
                    "en" or null =>
                        ContractorParameter.Configuration.GetValue<string>("TemplateId_en1"),
                    "nl" =>  ContractorParameter.Configuration.GetValue<string>("TemplateId_nl1"),
                    _ => sendGridParameter.TemplateId
                };
                // if (ContractorParameter.Lang == "en")
                //     sendGridParameter.TemplateId = ContractorParameter.Configuration.GetValue<string>("TemplateId_en1");
                // if (ContractorParameter.Lang == "nl")
                //     sendGridParameter.TemplateId = ContractorParameter.Configuration.GetValue<string>("TemplateId_nl1");
                issend = await ContractorParameter.SendGridRepositorie.SendInvitation(sendGridParameter);
            }
        }

        return ContractorParameter.ContractorComment.Id;
    }

    public async Task<IEnumerable<CommentCardDto>> GetComment(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        CabPersonCompany mCabPersonCompany;
        var companyId = @"SELECT * FROM dbo.CabPersonCompany WHERE Oid = @Id";
        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            mCabPersonCompany = connection.Query<CabPersonCompany>(companyId, new { Id = ContractorParameter.UserId })
                .FirstOrDefault();
            
        }

        var queryCommentCard = @"SELECT * FROM dbo.CommentCard WHERE CommentCard.LotId = @LotId ";
        StringBuilder sb;
        sb = new StringBuilder(queryCommentCard);

        if (ContractorParameter.CommentFilter.Sorter.Attribute == null)
            sb.Append(" ORDER BY CommentCard.ArticleNo asc");

        if (ContractorParameter.CommentFilter.ArticalNo != null)
            if (ContractorParameter.CommentFilter.Sorter.Attribute != null)
            {
                if (ContractorParameter.CommentFilter.Sorter.Attribute.ToLower().Equals("articleno"))
                {
                    var words = ContractorParameter.CommentFilter.ArticalNo.Split(" ");
                    foreach (var element in words) sb.Append("AND CommentCard.ArticleNo LIKE '%" + element + "%'");

                    sb.Append(
                        "ORDER BY CommentCard.ArticleNo " + ContractorParameter.CommentFilter.Sorter.Order);
                }

                if (ContractorParameter.CommentFilter.Sorter.Attribute.ToLower().Equals("title"))
                {
                    var words = ContractorParameter.CommentFilter.ArticalNo.Split(" ");
                    foreach (var element in words) sb.Append("AND CommentCard.CardTitle LIKE '%" + element + "%'");

                    sb.Append(
                        "ORDER BY CommentCard.CardTitle " + ContractorParameter.CommentFilter.Sorter.Order);
                }
            }

        var queryContractors = @"SELECT
                                          CommentCardContractor.Id
                                         ,CommentCardContractor.CommentCardId
                                         ,CommentCardContractor.ContractorId
                                         ,CommentCardContractor.CreaterId
                                         ,CommentCardContractor.Assigner AS AssignerId
                                         ,CommentCardContractor.Reporter
                                         ,CommentLogPriority.PriorityId AS [Key]
                                         ,CommentLogPriority.Name AS Text
                                         ,CommentLogField.FieldId AS [Key]
                                         ,CommentLogField.Name AS Text
                                         ,CommentLogSeverity.SeverityId AS [Key]
                                         ,CommentLogSeverity.Name AS Text
                                         ,CommentLogStatus.StatusId AS [Key]
                                         ,CommentLogStatus.Name AS Text
                                         ,CommentChangeType.TypeId AS [Key]
                                         ,CommentChangeType.Name AS Text
                                        FROM dbo.CommentCardContractor
                                        LEFT OUTER JOIN dbo.CommentLogField
                                          ON CommentCardContractor.Field = CommentLogField.FieldId
                                        LEFT OUTER JOIN dbo.CommentLogPriority
                                          ON CommentCardContractor.Priority = CommentLogPriority.PriorityId
                                        LEFT OUTER JOIN dbo.CommentLogSeverity
                                          ON CommentCardContractor.Severity = CommentLogSeverity.SeverityId
                                        LEFT OUTER JOIN dbo.CommentLogStatus
                                          ON CommentCardContractor.Status = CommentLogStatus.StatusId
                                        LEFT OUTER JOIN dbo.CommentChangeType
                                          ON CommentCardContractor.ChangeType = CommentChangeType.TypeId
                                          WHERE CommentLogField.LanguageCode = @lang
                                          AND CommentLogPriority.LanguageCode = @lang
                                          AND CommentLogSeverity.LanguageCode = @lang
                                          AND CommentLogStatus.LanguageCode = @lang
                                          AND CommentChangeType.LanguageCode = @lang
                                           AND CommentCardContractor.Accept NOT IN ('1','2')";

        var queryComments = @"SELECT * FROM dbo.ContractorComment";

        var company = @"SELECT CabCompany.Name FROM dbo.CabCompany WHERE Id = @Id";
        var person =
            @"SELECT CabPerson.FullName,CabPerson.Image FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE Oid = @Id";
        var cabperson = @"SELECT CabPerson.FullName FROM dbo.CabPerson WHERE Id = @Id";

        IEnumerable<CommentCardDto> data = null;
        IEnumerable<ContractorsCommentCardDto> data1 = null;
        IEnumerable<CommentsDto> data2 = null;

        var param = new
        {
            lang = ContractorParameter.Lang, ContractorParameter.CommentFilter.LotId,
            ContractorId = mCabPersonCompany.CompanyId
        };

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            data = await connection.QueryAsync<CommentCardDto>(sb.ToString(), param);
            //data1 = await connection.QueryAsync<ContractorsCommentCardDto>(queryContractors);

            data1 = connection
                .Query<ContractorsCommentCardDto, CommentLogPriorityDto, CommentLogFieldDto, CommentLogSeverityDto,
                    CommentLogStatusDto, CommentChangeTypeDto, ContractorsCommentCardDto>(
                    queryContractors,
                    (contractorsCommentCardDto, commentLogPriorityDto, commentLogFieldDto, commentLogSeverityDto,
                        commentLogStatusDto, commentChangeTypeDto) =>
                    {
                        contractorsCommentCardDto.Priority = commentLogPriorityDto;
                        contractorsCommentCardDto.Field = commentLogFieldDto;
                        contractorsCommentCardDto.Severity = commentLogSeverityDto;
                        contractorsCommentCardDto.Status = commentLogStatusDto;
                        contractorsCommentCardDto.ChangeType = commentChangeTypeDto;
                        return contractorsCommentCardDto;
                    }, param,
                    splitOn: "Key, Key,Key,Key,Key");


            data2 = await connection.QueryAsync<CommentsDto>(queryComments);

        }

        if (mCabPersonCompany.CompanyId !=ContractorParameter.Configuration.GetValue<string>("CompanyId"))
            data1 = data1.Where(a =>
                a.ContractorId == mCabPersonCompany.CompanyId || a.CreaterId == mCabPersonCompany.CompanyId ||
                a.ContractorId == ContractorParameter.Configuration.GetValue<string>("CompanyId"));

        data1 = data1.OrderBy(a => a.ContractorName);

        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            foreach (var i in data)
            {
                var ContractorsCommentCardDto = new List<ContractorsCommentCardDto>();
                ContractorsCommentCardDto = data1.Where(a => a.CommentCardId == i.Id).ToList();

                i.Contractors = ContractorsCommentCardDto;
                foreach (var r in i.Contractors)
                {
                    CabCompany CabCompany;
                    CabPerson cabPerson;
                    CabCompany = connection.Query<CabCompany>(company, new { Id = r.ContractorId })
                        .FirstOrDefault();
                    if (r.AssignerId != null)
                    {
                        if (r.AssignerId != "all")
                        {
                            cabPerson = connection.Query<CabPerson>(cabperson, new { Id = r.AssignerId })
                                .FirstOrDefault();
                            r.Assigner = cabPerson.FullName;
                        }
                        else
                        {
                            r.Assigner = "all";

                        }
                    }

                    if (r.Reporter != null)
                    {
                        cabPerson = connection.Query<CabPerson>(person, new { Id = r.Reporter })
                            .FirstOrDefault();
                        r.Reporter = cabPerson.FullName;
                    }

                    if (CabCompany != null) r.ContractorName = CabCompany.Name;

                    if (r.CreaterId != null)
                    {
                        CabCompany = connection.Query<CabCompany>(company, new { Id = r.CreaterId })
                            .FirstOrDefault();
                        r.Creater = CabCompany.Name;
                    }

                    var CommentsDto = new List<CommentsDto>();
                    CommentsDto = data2.Where(a => a.CommentCardContractorsId == r.Id).ToList();
                    r.Comments = CommentsDto;
                    foreach (var a in r.Comments)
                    {
                        CabPerson CabPerson;
                        CabPerson = connection.Query<CabPerson>(person, new { Id = a.PersonId })
                            .FirstOrDefault();
                        a.Name = CabPerson.FullName;
                    }
                }
            }
        }

        data = data.Where(a => a.Contractors.Count != 0);
        return data;
    }

    public async Task<string> ContractorLotExcelUpload(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            using var connection = new SqlConnection(connectionString);
            using var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);

            if (ContractorParameter.ContractorLotExcelData.ContractorId == null)
                ContractorParameter.ContractorLotExcelData.ContractorId = dbConnection.Query<string>(
                    "Select CompanyId From CabPersonCompany Where Oid = @Oid",
                    new { Oid = ContractorParameter.UserId }).FirstOrDefault();

            if (ContractorParameter.ContractorLotExcelData.ContractId != null &&
                ContractorParameter.ContractorLotExcelData.ContractorId != null)
            {
                await connection.ExecuteAsync(
                    " Delete From dbo.ContractorPdfData Where LotId =@LotId AND CompanyId = @CompanyId",
                    new
                    {
                        CompanyId = ContractorParameter.ContractorLotExcelData.ContractorId,
                        LotId = ContractorParameter.ContractorLotExcelData.ContractId
                    });
                await connection.ExecuteAsync(
                    " Delete From dbo.ContractorUploadedFiles Where LotId =@LotId AND CompanyId = @CompanyId",
                    new
                    {
                        CompanyId = ContractorParameter.ContractorLotExcelData.ContractorId,
                        LotId = ContractorParameter.ContractorLotExcelData.ContractId
                    });
                
                await connection.ExecuteAsync(
                    " Delete From dbo.ContractorLotUploadedDocs Where LotId =@LotId AND CompanyId = @CompanyId",
                    new
                    {
                        CompanyId = ContractorParameter.ContractorLotExcelData.ContractorId,
                        LotId = ContractorParameter.ContractorLotExcelData.ContractId
                    });
            }

            var lotRowNumber = 0;
            ;

            foreach (var excelData in ContractorParameter.ContractorLotExcelData.ExcelData)
            {
                // var artNo = excelData.ArticleNo;

                if (excelData.Key1.IsNullOrEmpty())
                {
                    excelData.Key1 = null;
                }
                if (excelData.Value1.IsNullOrEmpty())
                {
                    excelData.Value1 = null;
                }
                if (excelData.Key2.IsNullOrEmpty())
                {
                    excelData.Key2 = null;
                }
                if (excelData.Value2.IsNullOrEmpty())
                {
                    excelData.Value2 = null;
                }
                if (excelData.Key3.IsNullOrEmpty())
                {
                    excelData.Key3 = null;
                }
                if (excelData.Value3.IsNullOrEmpty())
                {
                    excelData.Value3 = null;
                }
                if (excelData.Key4.IsNullOrEmpty())
                {
                    excelData.Key4 = null;
                }
                if (excelData.Value4.IsNullOrEmpty())
                {
                    excelData.Value4 = null;
                }if (excelData.Key5.IsNullOrEmpty())
                {
                    excelData.Key5 = null;
                }
                if (excelData.Value5.IsNullOrEmpty())
                {
                    excelData.Value5 = null;
                }
                
                var sql =
                    "INSERT INTO dbo.ContractorPdfData ( Id ,ArticleNo ,CompanyId ,Title ,Unit ,VH ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ,PageRowColumn ,PageRow ,LotRowNumber,Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5 , MeasurementCode, RealArticleNo) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@VH ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate, @PageRowColumn, @PageRow ,@LotRowNumber, @Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5 , @MeasurementCode , @RealArticleNo)";

                // if (excelData.Key1 != null && excelData.Value1 != null)
                // {
                //     artNo = artNo + " / " + excelData.Value1.Trim();
                // }
                // if (excelData.Key2 != null && excelData.Value2 != null)
                // {
                //     artNo = artNo + " / "  + excelData.Value2.Trim();
                // }
                // if (excelData.Key3 != null && excelData.Value3 != null)
                // {
                //     artNo = artNo + " / "  + excelData.Value3.Trim();
                // }
                // if (excelData.Key4 != null && excelData.Value4 != null)
                // {
                //     artNo = artNo + " / " + excelData.Value4.Trim();
                // }
                // if (excelData.Key5 != null && excelData.Value5 != null)
                // {
                //     artNo = artNo + " / " + excelData.Value5.Trim();
                // }
                
                const string pattern = @"^([^/]+)";

                var match = Regex.Match(excelData.ArticleNo, pattern);
                
                var realArtNo = match.Groups[1].Value.Trim();


                var param = new
                {
                    Id = Guid.NewGuid().ToString(),
                    ArticleNo = excelData.ArticleNo,
                    CompanyId = ContractorParameter.ContractorLotExcelData.ContractorId,
                    excelData.Title,
                    excelData.Unit,
                    excelData.VH,
                    excelData.Quantity,
                    UnitPrice = excelData.UnitPrice?.ToFloat(),
                    TotalPrice = excelData.TotalPrice?.ToFloat(),
                    LotId = ContractorParameter.ContractorLotExcelData.ContractId,
                    CreatedDate = DateTime.UtcNow,
                    excelData.PageRowColumn,
                    excelData.PageRow,
                    LotRowNumber = lotRowNumber.ToString().PadLeft(4, '0'),
                    excelData.Key1,
                    excelData.Value1,
                    excelData.Key2,
                    excelData.Value2,
                    excelData.Key3,
                    excelData.Value3,
                    excelData.Key4,
                    excelData.Value4,
                    excelData.Key5,
                    excelData.Value5,
                    MeasurementCode = excelData.MeasurementCode,
                    RealArticleNo = realArtNo
                };

                await connection.ExecuteAsync(sql, param);

                lotRowNumber++;
            }
            // string companyName =    dbConnection.Query<string>("SELECT name FROM CabCompany WHERE Id = @Id",
            //     new {Id = ContractorParameter.ContractorLotExcelData.ContractorId}).FirstOrDefault();
            // var uploadedItem = await ContractorParameter.GraphServiceClient
            //     .Drive
            //     .Root
            //     //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
            //     .ItemWithPath(ContractorParameter.ContractorLotExcelData.ContractId + "/" + companyName + "/" + ContractorParameter.File.FileName)
            //
            //     .Content
            //     .Request()
            //     .PutAsync<DriveItem>(ContractorParameter.File.OpenReadStream());

            var client = new FileClient();
            var url = client.PersistLotDocUpload(ContractorParameter.File.FileName, ContractorParameter.TenantProvider
                , ContractorParameter.File, ContractorParameter.ContractorLotExcelData.ContractId,"contractorLotFiles");
            
            var filesSql =
                "INSERT INTO dbo.ContractorUploadedFiles ( Id ,CompanyId ,LotId ,FileType ) VALUES ( @Id ,@CompanyId ,@LotId ,@FileType )";

            var filesParam = new
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = ContractorParameter.ContractorLotExcelData.ContractorId,
                LotId = ContractorParameter.ContractorLotExcelData.ContractId,
                FileType = "excel"
            };

            await connection.ExecuteAsync(filesSql, filesParam);
            
            var insertSql =
                "INSERT INTO dbo.ContractorLotUploadedDocs ( Id ,CompanyId ,LotId ,Link,Title,CreatedDate ) VALUES ( @Id ,@CompanyId ,@LotId ,@Link ,@Title ,@CreatedDate)";

            var insertParam = new
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = ContractorParameter.ContractorLotExcelData.ContractorId,
                LotId = ContractorParameter.ContractorLotExcelData.ContractId,
                Link = url,
                Title = ContractorParameter.File.FileName,
                CreatedDate = DateTime.UtcNow
            };

            await connection.ExecuteAsync(insertSql, insertParam);


            return "Ok";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }


    public async Task<string> LotPublish(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);

        using var connection = new SqlConnection(connectionString);

        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        var measuringState = connection
            .Query<double>("Select MeasuringStatus From ContractorHeader Where SequenceId = @Id",
                new { ContractorParameter.Id }).FirstOrDefault();

        var lotId = connection.Query<string>("Select Id From ContractorHeader Where SequenceId = @Id",
            new { ContractorParameter.Id }).FirstOrDefault();

        if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
        {
            await connection.ExecuteAsync(
                "Update ContractorHeader Set MeasuringStatus = @MeasuringStatus Where SequenceId = @Id",
                new { ContractorParameter.Id, MeasuringStatus = measuringState + 1.0 });

            await connection.ExecuteAsync("Delete From CBCExcelLotdataPublished Where ContractId = @LotId",
                new { LotId = ContractorParameter.Id });

            var excelLotdata = connection.Query<CBCExcelLotData>(
                @"Select * From CBCExcelLotData Where ContractId = @LotId",
                new { LotId = ContractorParameter.Id });

            foreach (var excelData in excelLotdata)
            {
                var sql =
                    "INSERT INTO dbo.CBCExcelLotdataPublished ( Id ,Title ,ParentId ,ArticleNo ,Quantity ,ContractId ,Unit ,UnitPrice ,TotalPrice ,MeasurementCode , Mou ,Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5 ,IsExclude ,RealArticleNo ) VALUES ( @Id ,@Title ,@ParentId ,@ArticleNo ,@Quantity ,@ContractId ,@Unit ,@UnitPrice ,@TotalPrice ,@MeasurementCode , @Mou , @Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5 ,@IsExclude ,@RealArticleNo);";
                // Parallel.ForEach(pdfdata, async data =>
                // {
                var param = new
                {
                    excelData.Id,
                    excelData.Title,
                    excelData.ParentId,
                    excelData.ArticleNo,
                    excelData.Quantity,
                    excelData.ContractId,
                    excelData.Unit,
                    excelData.UnitPrice,
                    excelData.TotalPrice,
                    excelData.MeasurementCode,
                    excelData.Mou,
                    excelData.Key1,
                    excelData.Value1,
                    excelData.Key2,
                    excelData.Value2,
                    excelData.Key3,
                    excelData.Value3,
                    excelData.Key4,
                    excelData.Value4,
                    excelData.Key5,
                    excelData.Value5,
                    IsExclude = excelData.IsExclude,
                    RealArticleNo = excelData.RealArticleNo
                };
                await using (var connection2 = new SqlConnection(connectionString))
                {
                    await connection2.ExecuteAsync(sql, param);
                }
            }

            await LotStatusUpdate("94282458-0b40-40a3-cowf-c2e40344c8f1", lotId, connectionString); //in tendering
        }
        else
        {
            var version = connection
                .Query<ConstructorWorkFlow>(
                    "Select * From ConstructorWorkFlow Where Lot = @Id AND CabCompanyId = @CabCompanyId ",
                    new { Id = lotId, CabCompanyId = companyId }).FirstOrDefault();
            if (measuringState.ToString().First() == version.Version.ToString().First())
                await connection.ExecuteAsync(
                    "Update ConstructorWorkFlow Set Version = @Version Where Lot = @Id AND CabCompanyId = @CabCompanyId",
                    new { Id = lotId, Version = version.Version + 0.1, CabCompanyId = companyId });
            else
                await connection.ExecuteAsync(
                    "Update ConstructorWorkFlow Set Version = @Version Where Lot = @Id AND CabCompanyId = @CabCompanyId",
                    new { Id = lotId, Version = measuringState + 0.1, CabCompanyId = companyId });

            if (version.MeasuringStateReceived == false)
                await SendMeasuringStateReceivedMail(version, ContractorParameter);

            var cwList = connection.Query<ConstructorWorkFlow>(
                "Select * From ConstructorWorkFlow Where Lot = @Id ",
                new { Id = lotId });

            var publishedCount = cwList.Where(x => x.Version > 0.0).Count();

            if (publishedCount >= 2)
                await LotStatusUpdate("7143ff01-d173-4a20-cowf-cacdfecdb84c", // in price comparison
                    lotId, connectionString);

            await connection.ExecuteAsync(
                "Delete From PublishedContractorsPdfData Where LotId = @LotId AND CompanyId = @CompanyId",
                new { LotId = ContractorParameter.Id, CompanyId = companyId });

            await connection.ExecuteAsync(
                "Delete From ContractorTotalValuesPublished Where LotId = @LotId AND CompanyId = @CompanyId",
                new { LotId = ContractorParameter.Id, CompanyId = companyId });

            var pdfdata = connection.Query<ContractorPdfData>(
                @"Select * From ContractorPdfData Where LotId = @LotId AND CompanyId = @CompanyId",
                new { LotId = ContractorParameter.Id, CompanyId = companyId });

            var contractorValues = connection.Query<ContractorTotalValuesDto>(
                @"Select * From ContractorTotalValues Where LotId = @LotId AND CompanyId = @CompanyId",
                new { LotId = ContractorParameter.Id, CompanyId = companyId });

            foreach (var data in pdfdata)
            {
                var sql =
                    "INSERT INTO dbo.PublishedContractorsPdfData ( Id ,ArticleNo ,CompanyId ,Title ,Unit ,VH ,Quantity ,UnitPrice ,TotalPrice,LotId,CreatedDate ,PageRowColumn ,PageRow ,LotRowNumber, Key1, Value1, Key2, Value2, Key3, Value3, Key4, Value4, Key5, Value5 ,MeasurementCode,RealArticleNo ) VALUES ( @Id ,@ArticleNo ,@CompanyId ,@Title ,@Unit ,@VH ,@Quantity ,@UnitPrice ,@TotalPrice, @LotId, @CreatedDate, @PageRowColumn, @PageRow ,@LotRowNumber , @Key1, @Value1, @Key2, @Value2, @Key3, @Value3, @Key4, @Value4, @Key5, @Value5 , @MeasurementCode ,@RealArticleNo)";
                // Parallel.ForEach(pdfdata, async data =>
                // {
                var param = new
                {
                    data.Id,
                    data.ArticleNo,
                    data.CompanyId,
                    data.Title,
                    data.Unit,
                    data.VH,
                    data.Quantity,
                    data.UnitPrice,
                    data.TotalPrice,
                    data.LotId,
                    CreatedDate = DateTime.UtcNow,
                    data.PageRowColumn,
                    data.PageRow,
                    data.LotRowNumber,
                    data.Key1,
                    data.Value1,
                    data.Key2,
                    data.Value2,
                    data.Key3,
                    data.Value3,
                    data.Key4,
                    data.Value4,
                    data.Key5,
                    data.Value5,
                    MeasurementCode = data.MeasurementCode,
                    RealArticleNo = data.RealArticleNo
                };
                await using (var connection2 = new SqlConnection(connectionString))
                {
                    await connection2.ExecuteAsync(sql, param);
                }
            }

            

            foreach (var values in contractorValues)
            {
                var sql3 =
                    "INSERT INTO dbo.ContractorTotalValuesPublished ( Id ,LotId ,CompanyId ,TotalBAFO ,TotalCost, IsWinner ) VALUES ( @Id ,@LotId ,@CompanyId ,@TotalBAFO ,@TotalCost , @IsWinner)";

                var param3 = new
                {
                    Id = Guid.NewGuid().ToString(),
                    values.LotId,
                    values.CompanyId,
                    values.TotalBAFO,
                    values.TotalCost,
                    values.IsWinner
                };

                await connection.ExecuteAsync(sql3, param3);
            }
        }

        return ContractorParameter.Id;
    }

    public async Task<string> SendInvitation(ContractorParameter ContractorParameter,
        ISendGridRepositorie ISendGridRepositorie)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
            var cuconnectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                null, ContractorParameter.TenantProvider);

            await using var dbConnection =
                new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


            StandardMailHeader mStandardMailHeader;
            var update =
                @"UPDATE dbo.ContractorTeamList SET InvitationMail = @InvitationMail ,InvitationId = @InvitationId,InvitationDateTime = @InvitationDateTime WHERE LotContractorId = @Id AND CompanyId = @CompanyId;";


            var constructorWf =
                @"SELECT ConstructorWorkFlow.* FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id LEFT OUTER JOIN dbo.ContractorTeamList ON ConstructorWorkFlow.Lot = ContractorTeamList.LotContractorId WHERE ConstructorWorkFlow.CabCompanyId = @Id AND ContractorTeamList.InvitationId = @InvitationId";

            var url = ContractorParameter.Configuration.GetValue<string>("DomainUrl") + "/CU/";

            await using (var connection = new SqlConnection(connectionString))
            {
                var sMailId =
                    @"SELECT ContractorHeader.StandardMailId,ContractorHeader.Name,ContractorHeader.Title,ContractorHeader.StartDate,ContractorHeader.EndDate FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorHeader.Id = @Id";

                ContractorHeader mContractorHeader;
                mContractorHeader = connection.Query<ContractorHeader>(sMailId,
                    new
                    {
                        Id = ContractorParameter.ContractorTeam.LotId
                    }).FirstOrDefault();
                if (mContractorHeader?.StandardMailId != null)
                    await using (var cuconnection = new SqlConnection(cuconnectionString))
                    {
                        mStandardMailHeader = cuconnection
                            .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id",
                                new { Id = mContractorHeader.StandardMailId }).FirstOrDefault();
                    }

                else
                    await using (var cuconnection = new SqlConnection(cuconnectionString))
                    {
                        mStandardMailHeader = cuconnection
                            .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader where IsDefault = 1")
                            .FirstOrDefault();
                    }

                var projLang = dbConnection.Query<string>(
                    "SELECT pl.Name FROM ProjectDefinition LEFT OUTER JOIN ProjectLanguage pl ON ProjectDefinition.Language = pl.TypeId WHERE ProjectDefinition.SequenceCode = @Id",
                    new { Id = ContractorParameter.ProjectSequenceId }).FirstOrDefault();
                
                ConstructorWorkFlow ConstructorWorkFlow = null;
                foreach (var i in ContractorParameter.ContractorTeam.ContractorTeamList)
                    if (i.InvitationMail == false)
                    {
                        bool issend;
                        var sendGridParameter = new SendGridParameter();
                        sendGridParameter.Id = i.CabPersonId;
                        sendGridParameter.TenantProvider = ContractorParameter.TenantProvider;
                        sendGridParameter.ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId;
                        sendGridParameter.ProjectSequenceId = ContractorParameter.ProjectSequenceId;
                        sendGridParameter.Lot = ContractorParameter.ContractorTeam.SequenceId;
                        sendGridParameter.Lang = ContractorParameter.Lang;
                        sendGridParameter.MailBody = mStandardMailHeader.AcceptTender;
                        sendGridParameter.LotTitle = mContractorHeader.Name;
                        sendGridParameter.StartDate = mContractorHeader.StartDate?.ToString("dd/MM/yyyy");
                        sendGridParameter.EndDate = mContractorHeader.EndDate?.ToString("dd/MM/yyyy");
                        //sendGridParameter.Subject = mContractorHeader.Title + " " + "Accept Invitation";
                        sendGridParameter.Configuration = ContractorParameter.Configuration;
                        //sendGridParameter.ButtonText = "klik hiervoor meer info over het project";
                        var invitationId = Guid.NewGuid().ToString();
                        // sendGridParameter.Url = "https://bmengineering.uprince.com/CU/" +
                        //                         ContractorParameter.ContractingUnitSequenceId + "/project/" +
                        //                         ContractorParameter.ProjectSequenceId + "/lot-invitation/" +
                        //                         ContractorParameter.ContractorTeam.SequenceId + "/" + invitationId +
                        //                         "";
                        sendGridParameter.Url = url +
                                                ContractorParameter.ContractingUnitSequenceId + "/project/" +
                                                ContractorParameter.ProjectSequenceId + "/lot-invitation/" +
                                                ContractorParameter.ContractorTeam.SequenceId + "/" + invitationId +
                                                "" + "/" + projLang;
                        
                        sendGridParameter.ProjLang = projLang;
                        sendGridParameter.StatusImage = projLang switch
                        {
                            "en" or null =>
                                "https://bmengineeringuprinceuat.blob.core.windows.net/uprincev4dev/en-accept.png",
                            "nl" => "https://bmengineeringuprinceuat.blob.core.windows.net/uprincev4dev/nl-accept.png",
                            _ => sendGridParameter.StatusImage
                        };
                        
                        sendGridParameter.ButtonText = projLang switch
                        {
                            "en" or null =>
                                "click here for more information about the project",
                            "nl" => "klik hiervoor meer info over het project",
                            _ => sendGridParameter.ButtonText
                        };
                        sendGridParameter.Subject = projLang switch
                        {
                            "en" or null =>
                                mContractorHeader.Title + " " + "Accept Invitation",
                            "nl" => mContractorHeader.Title + " " + "Uitnodiging Accepteren",
                            _ => sendGridParameter.Subject
                        };

                        sendGridParameter.DisplayImage = "block";
                        sendGridParameter.DisplayBtn = "block";

                        issend = await ISendGridRepositorie.SendInvitation(sendGridParameter);

                        var parm = new
                        {
                            InvitationId = invitationId,
                            InvitationMail = issend,
                            Id = i.LotContractorId,
                            i.CompanyId,
                            InvitationDateTime = DateTime.UtcNow
                        };


                        await connection.ExecuteAsync(update, parm);
                        ConstructorWorkFlow = connection.Query<ConstructorWorkFlow>(constructorWf,
                            new { Id = i.CompanyId, InvitationId = invitationId }).FirstOrDefault();

                        await connection.ExecuteAsync(
                            "Update ContractorHeader Set IsInviteSend = @IsInviteSend Where Id = @Id",
                            new { Id = i.LotContractorId, IsInviteSend = true });
                        await connection.ExecuteAsync(
                            "Update ConstructorWorkFlow Set IsInviteSend = @IsInviteSend Where Id = @Id",
                            new { ConstructorWorkFlow.Id, IsInviteSend = issend });

                        if (issend)
                            await ConstructorWfStatusUpdate("bvxbdkjg4e8d-fhhd-487d-8170-6b91c89fdvnfd",
                                ConstructorWorkFlow.Id, connectionString);
                    }
            }

            return ContractorParameter.ContractorTeam.SequenceId;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }


    public async Task<string> AcceptComment(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var update = "UPDATE dbo.CommentCardContractor SET Accept = @Accept WHERE Id = @Id";

            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(update,
                    new { ContractorParameter.AcceptComment.Id, ContractorParameter.AcceptComment.Accept });
            }

            return "Ok";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CommentLogDropDownData> GetCommentLogDropDownData(ContractorParameter ContractorParameter)
    {
        const string query =
            @"SELECT PriorityId AS [Key], Name as Text FROM dbo.CommentLogPriority Where LanguageCode = @lang ORDER BY DisplayOrder;
                                    SELECT SeverityId AS [Key], Name as Text FROM dbo.CommentLogSeverity Where LanguageCode = @lang ORDER BY DisplayOrder;
                                    SELECT StatusId AS [Key], Name as Text FROM dbo.CommentLogStatus Where LanguageCode = @lang ORDER BY DisplayOrder;
                                    SELECT FieldId AS [Key], Name as Text FROM dbo.CommentLogField Where LanguageCode = @lang ORDER BY DisplayOrder;
                                    SELECT TypeId AS [Key], Name as Text FROM dbo.CommentChangeType Where LanguageCode = @lang ORDER BY DisplayOrder;";

        var mCommentLogDropDownData = new CommentLogDropDownData();
        var parameters = new { lang = ContractorParameter.Lang };
        await using var connection =
            new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var muilti = await connection.QueryMultipleAsync(query, parameters);
        mCommentLogDropDownData.Priority = muilti.Read<CommentLogPriorityDto>();
        mCommentLogDropDownData.Severity = muilti.Read<CommentLogSeverityDto>();
        mCommentLogDropDownData.Status = muilti.Read<CommentLogStatusDto>();
        mCommentLogDropDownData.Field = muilti.Read<CommentLogFieldDto>();
        mCommentLogDropDownData.ChangeType = muilti.Read<CommentChangeTypeDto>();

        return mCommentLogDropDownData;
    }

    public async Task<string> UpdateCommentLogDropDown(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var update =
                "UPDATE dbo.CommentCardContractor SET Field = @Field ,Priority = @Priority ,Severity = @Severity ,Status = @Status,Assigner = @Assigner,ChangeType = @ChangeType WHERE Id = @Id;";

            var param = new
            {
                ContractorParameter.CommentCardContractorDto.Id,
                ContractorParameter.CommentCardContractorDto.Priority,
                ContractorParameter.CommentCardContractorDto.Severity,
                ContractorParameter.CommentCardContractorDto.Status,
                ContractorParameter.CommentCardContractorDto.Field,
                ContractorParameter.CommentCardContractorDto.Assigner,
                ContractorParameter.CommentCardContractorDto.ChangeType
            };
            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(update, param);
            }


            return ContractorParameter.CommentCardContractorDto.Id;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<GetProjectByUser>> GetProjectsByUser(ContractorParameter ContractorParameter)
    {
        try
        {
            var company =
                @"SELECT CabCompany.Name ,CabCompany.Id FROM dbo.CabPersonCompany INNER JOIN dbo.CabCompany ON CabPersonCompany.CompanyId = CabCompany.Id WHERE CabPersonCompany.Oid = @Oid";
            CabCompany mCabCompany;

            var query1 =
                @"SELECT DISTINCT ProjectDefinition.Title ,ProjectDefinition.SequenceCode ,ProjectDefinition.Name ,ProjectDefinition.ContractingUnitId,ProjectDefinition.Id AS [Key] FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.ProjectUserRole ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId LEFT OUTER JOIN dbo.UserRole ON ProjectUserRole.UsrRoleId = UserRole.Id LEFT OUTER JOIN dbo.ApplicationUser ON UserRole.ApplicationUserOid = ApplicationUser.OId INNER JOIN dbo.CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE CabCompany.SequenceCode = @cuId AND ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.Title IS NOT NULL";
            var query2 =
                @"SELECT DISTINCT ProjectDefinition.Title ,ProjectDefinition.SequenceCode ,ProjectDefinition.Name ,ProjectDefinition.ContractingUnitId,ProjectDefinition.Id AS [Key] FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.ProjectUserRole ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId LEFT OUTER JOIN dbo.UserRole ON ProjectUserRole.UsrRoleId = UserRole.Id LEFT OUTER JOIN dbo.ApplicationUser ON UserRole.ApplicationUserOid = ApplicationUser.OId INNER JOIN dbo.CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE ApplicationUser.OId = @oid AND CabCompany.SequenceCode = @cuId AND ProjectDefinition.IsDeleted = 0 ";

            var lot1 =
                @"SELECT ContractorHeader.Id ,ContractorHeader.SequenceId ,ContractorHeader.Title FROM dbo.ContractorHeader";
            var lot2 =
                @"SELECT ContractorHeader.Id ,ContractorHeader.SequenceId ,ContractorHeader.Title ,ConstructorWorkFlow.Price FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id LEFT OUTER JOIN dbo.ContractorTeamList ON ConstructorWorkFlow.Lot = ContractorTeamList.LotContractorId WHERE ConstructorWorkFlow.CabCompanyId = @Id AND ContractorTeamList.CompanyId = @Id AND ContractorTeamList.IsSubscribed = 1";

            var param = new
            {
                lang = ContractorParameter.Lang, oid = ContractorParameter.UserId,
                cuId = ContractorParameter.ContractingUnitSequenceId
            };
            IEnumerable<GetProjectByUser> result = null;
            var result1 = new List<GetProjectByUser>();
            await using var connection =
                new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
            mCabCompany = connection.Query<CabCompany>(company, new { Oid = ContractorParameter.UserId })
                .FirstOrDefault();
            if (mCabCompany.Id == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
            {
                var sb = new StringBuilder(query2);
                if (ContractorParameter.FilterByUser.Title != null)
                {
                    var words = ContractorParameter.FilterByUser.Title.Split(" ");
                    foreach (var element in words)
                        sb.Append(" AND ProjectDefinition.Title LIKE '%" + element + "%'");
                }

                sb.Append(" ORDER BY ProjectDefinition.SequenceCode ASC");
                result = connection.Query<GetProjectByUser>(sb.ToString(), param).ToList();

                var mContractList = new List<ContractList>();
                foreach (var i in result)
                {
                    var connectionString = ConnectionString.MapConnectionString(
                        ContractorParameter.ContractingUnitSequenceId,
                        i.SequenceCode, ContractorParameter.TenantProvider);

                    using (var connectiondb =
                           new SqlConnection(connectionString))
                    {
                        mContractList = connectiondb.Query<ContractList>(lot1).ToList();
                    }

                    if (mContractList.Any()) result1.Add(i);
                }
            }
            else
            {
                var sb = new StringBuilder(query1);
                if (ContractorParameter.FilterByUser.Title != null)
                {
                    var words = ContractorParameter.FilterByUser.Title.Split(" ");
                    foreach (var element in words)
                        sb.Append(" AND ProjectDefinition.Title LIKE '%" + element + "%'");
                }

                sb.Append(" ORDER BY ProjectDefinition.SequenceCode ASC");
                result = connection.Query<GetProjectByUser>(sb.ToString(), param).ToList();
                var mContractList = new List<ContractList>();

                foreach (var i in result)
                {
                    var connectionString = ConnectionString.MapConnectionString(
                        ContractorParameter.ContractingUnitSequenceId,
                        i.SequenceCode, ContractorParameter.TenantProvider);

                    using (var connectiondb =
                           new SqlConnection(connectionString))
                    {
                        mContractList = connectiondb.Query<ContractList>(lot2, new { mCabCompany.Id }).ToList();
                    }

                    if (mContractList.Any()) result1.Add(i);
                }
            }

            return result1;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<IEnumerable<ContractList>> GetLotByUser(ContractorParameter ContractorParameter)
    {
        var company =
            @"SELECT CabCompany.Name ,CabCompany.Id FROM dbo.CabPersonCompany INNER JOIN dbo.CabCompany ON CabPersonCompany.CompanyId = CabCompany.Id WHERE CabPersonCompany.Oid = @Oid";
       
        CabCompany mCabCompany;
        
        using (var connection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            mCabCompany = connection.Query<CabCompany>(company, new { Oid = ContractorParameter.UserId })
                .FirstOrDefault();
        }

        var query1 =
            @"SELECT ContractorHeader.Id ,ContractorHeader.SequenceId ,ContractorHeader.Title FROM dbo.ContractorHeader ";

        var query2 =
            @"SELECT ContractorHeader.Id ,ContractorHeader.SequenceId ,ContractorHeader.Title ,ConstructorWorkFlow.Price FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id LEFT OUTER JOIN dbo.ContractorTeamList ON ConstructorWorkFlow.Lot = ContractorTeamList.LotContractorId WHERE ConstructorWorkFlow.CabCompanyId = @Id AND ContractorTeamList.CompanyId = @Id AND ContractorTeamList.IsSubscribed = 1";

        var awardwinner =
            @"SELECT IsWinner AS IsZeroState FROM dbo.ContractorTotalValuesPublished WHERE LotId = @Id AND IsWinner = 1 ";

        var parm = new { mCabCompany.Id };

        var mContractList = new List<ContractList>();

        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        using (var connection =
               new SqlConnection(connectionString))
        {
            if (mCabCompany.Id == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
            {
                var sb = new StringBuilder(query1);
                if (ContractorParameter.FilterByUser.Title != null)
                {
                    var words = ContractorParameter.FilterByUser.Title.Split(" ");
                    foreach (var element in words) sb.Append(" where ContractorHeader.Title LIKE '%" + element + "%'");
                }

                sb.Append(" ORDER BY ContractorHeader.SequenceId");
                mContractList = connection.Query<ContractList>(sb.ToString(), parm).ToList();
            }
            else
            {
                var sb = new StringBuilder(query2);
                if (ContractorParameter.FilterByUser.Title != null)
                {
                    var words = ContractorParameter.FilterByUser.Title.Split(" ");
                    foreach (var element in words) sb.Append(" AND ContractorHeader.Title LIKE '%" + element + "%'");
                }

                sb.Append(" ORDER BY ContractorHeader.SequenceId");
                mContractList = connection.Query<ContractList>(sb.ToString(), parm).ToList();
            }

            foreach (var i in mContractList)
            {
                i.IsZeroState = connection.Query<bool>(awardwinner, new { Id = i.SequenceId }).FirstOrDefault();

                if (mCabCompany.Id != ContractorParameter.Configuration.GetValue<string>("CompanyId"))
                    i.IsPsUpload = connection
                        .Query<ContractorPs>(
                            @"Select * From ContractorPs Where LotId = @LotId AND CompanyId = @CompanyId",
                            new { LotId = i.SequenceId, CompanyId = mCabCompany.Id }).Any();
                else
                    i.IsPsUpload = connection
                        .Query<ContractorPs>(
                            @"Select * From ContractorPs Where LotId = @LotId",
                            new { LotId = i.SequenceId }).Any();
            }
        }

        return mContractList;
    }

    public async Task<IEnumerable<GetContractingUnitByUser>> GetContractingUnitByUser(
        ContractorParameter ContractorParameter)
    {
        var company = @"SELECT CabCompany.Name ,CabCompany.Id FROM dbo.CabPersonCompany WHERE CabCompany.Id = @Id";
        
        var query1 =
            @"select Id AS [Key], Name, SequenceCode from CabCompany where IsDeleted = 0 AND IsContractingUnit = 1 ";
        var query2 = @"select Id AS [Key], Name, SequenceCode from CabCompany WHERE CabCompany.Id = @Id ";

        var mGetContractingUnitByUser = new List<GetContractingUnitByUser>();
        var resurlt = new List<GetContractingUnitByUser>();

        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var sb = new StringBuilder(query1);
            if (ContractorParameter.FilterByUser.Title != null)
            {
                var words = ContractorParameter.FilterByUser.Title.Split(" ");
                foreach (var element in words) sb.Append(" AND CabCompany.Name LIKE '%" + element + "%'");
            }

            mGetContractingUnitByUser = connection.Query<GetContractingUnitByUser>(query1).ToList();
            foreach (var r in mGetContractingUnitByUser)
            {
                ContractorParameter.ContractingUnitSequenceId = r.SequenceCode;
                var mGetProjectByUser = await GetProjectsByUser(ContractorParameter);
                var groupresources = mGetProjectByUser.GroupBy(r => r.ContractingUnitId);

                foreach (var i in groupresources)
                {
                    GetContractingUnitByUser GetContractingUnitByUser = connection.Query<GetContractingUnitByUser>(query2, new { Id = i.Key })
                        .FirstOrDefault();
                    resurlt.Add(GetContractingUnitByUser);
                }
            }
        }

        return resurlt;
    }

    public async Task<GetLotTotalPrices> GetLotTotalPriceById(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


        GetLotTotalPrices data = null;
        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
        {
            data = tenetConnection.Query<GetLotTotalPrices>(
                @"SELECT Description AS ExtraInfo, CabPerson.FullName AS Customer FROM ProjectDefinition LEFT OUTER JOIN CabPerson ON ProjectDefinition.CustomerId = CabPerson.Id WHERE SequenceCode = @SequenceCode",
                new { SequenceCode = ContractorParameter.ProjectSequenceId }).FirstOrDefault();
            var ff = dbConnection
                .Query<string>("SELECT SUM(TotalPrice) FROM CBCExcelLotdata where ContractId = @LotId",
                    new { LotId = ContractorParameter.Id })
                .FirstOrDefault();

            if (ff != null)
                data.TotalCost = ff.ToFloat();
            else
                data.TotalCost = 0;
        }
        else
        {
            data = tenetConnection.Query<GetLotTotalPrices>(
                @"SELECT Description AS ExtraInfo, CabPerson.FullName AS Customer FROM ProjectDefinition LEFT OUTER JOIN CabPerson ON ProjectDefinition.CustomerId = CabPerson.Id WHERE SequenceCode = @SequenceCode",
                new { SequenceCode = ContractorParameter.ProjectSequenceId }).FirstOrDefault();
            var ff = dbConnection
                .Query<string>(
                    "SELECT SUM(TotalPrice) As TotalCost  FROM CBCExcelLotdataPublished where ContractId = @LotId",
                    new { LotId = ContractorParameter.Id })
                .FirstOrDefault();
            if (ff != null)
                data.TotalCost = ff.ToFloat();
            else
                data.TotalCost = 0;
        }

        return data;
    }

    public async Task<string> AddTenderAwardWinner(ContractorParameter ContractorParameter,
        ISendGridRepositorie ISendGridRepositorie)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var cuconnectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            null, ContractorParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);

        if (ContractorParameter.AwardWinner != null)
        {
            var query =
                "Update ContractorTotalValues Set IsWinner = @IsWinner Where LotId = @LotId AND CompanyId = @CompanyId";
            var query2 =
                @"Update ContractorTotalValuesPublished Set IsWinner = @IsWinner Where LotId = @LotId AND CompanyId = @CompanyId";

            var param = new
            {
                ContractorParameter.AwardWinner.IsWinner,
                ContractorParameter.AwardWinner.LotId,
                ContractorParameter.AwardWinner.CompanyId
            };
            await dbConnection.ExecuteAsync(query, param);
            await dbConnection.ExecuteAsync(query2, param);
            var LotId = @"SELECT Id FROM dbo.ContractorHeader WHERE SequenceId = @LotId";


            var Id = dbConnection.Query<string>(LotId, new { ContractorParameter.AwardWinner.LotId }).FirstOrDefault();
            await LotStatusUpdate("8e8c4e8d-7bcb-487d-cowf-6b91c89fc3da", Id, connectionString);
            
            var download = @"UPDATE dbo.ContractorTeamList SET IsDownloded = 1 WHERE LotContractorId = @LotContractorId";

            await dbConnection.ExecuteAsync(download,
                new { LotContractorId = Id });

            var contractorsComapanies =
                dbConnection.Query<ConstructorWorkFlow>(
                    "Select * From ConstructorWorkFlow Where Lot = @Lot", new { Lot = Id });

            var totalValues = dbConnection
                .Query<ContractorTotalValuesPublished>(
                    "Select * From ContractorTotalValuesPublished Where LotId = @LotId AND CompanyId = @CompanyId ",
                    param).FirstOrDefault();


            await dbConnection.ExecuteAsync(
                "Update ContractorHeader Set TenderBudget = @TenderBudget WHERE SequenceId = @LotId ",
                new { LotId = ContractorParameter.AwardWinner.LotId, TenderBudget = totalValues?.TotalBAFO });


            foreach (var item in contractorsComapanies)
                if (item.CabCompanyId == ContractorParameter.AwardWinner.CompanyId)
                    await ConstructorWfStatusUpdate("nnnnad0b-2e84-con1-ad25-Lot0d49477", item.Id, connectionString);
                else
                    await ConstructorWfStatusUpdate("xxxxad0b-2e84-con1-ad25-Lot0d49477", item.Id, connectionString);
            StandardMailHeader mStandardMailHeader;
            var sMailId =
                @"SELECT ContractorHeader.StandardMailId,ContractorHeader.Title,ContractorHeader.StartDate,ContractorHeader.EndDate,ContractorHeader.Name FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorHeader.Id = @Id";

            ContractorHeader mContractorHeader;
            mContractorHeader = dbConnection.Query<ContractorHeader>(sMailId, new { Id }).FirstOrDefault();
            if (mContractorHeader.StandardMailId != null)
                await using (var cuconnection = new SqlConnection(cuconnectionString))
                {
                    mStandardMailHeader = cuconnection
                        .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id",
                            new { Id = mContractorHeader.StandardMailId }).FirstOrDefault();
                }

            else
                await using (var cuconnection = new SqlConnection(cuconnectionString))
                {
                    mStandardMailHeader = cuconnection
                        .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader").FirstOrDefault();
                }

            var queryTeam = "SELECT * FROM dbo.ContractorTeamList WHERE LotContractorId = @LotId";

            IEnumerable<ContractorTeamList> mContractorTeamList;

            
            mContractorTeamList = dbConnection.Query<ContractorTeamList>(queryTeam, new { LotId = Id });
            foreach (var i in mContractorTeamList)
            {
                var sendGridParameter = new SendGridParameter();
                sendGridParameter.Id = i.CabPersonId;
                sendGridParameter.TenantProvider = ContractorParameter.TenantProvider;
                sendGridParameter.ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId;
                sendGridParameter.ProjectSequenceId = ContractorParameter.ProjectSequenceId;
                sendGridParameter.Lot = ContractorParameter.AwardWinner.LotId;
                sendGridParameter.Lang = ContractorParameter.Lang;
                sendGridParameter.LotTitle = mContractorHeader.Name;
                sendGridParameter.StartDate = mContractorHeader.StartDate?.ToString("dd/MM/yyyy");
                sendGridParameter.EndDate = mContractorHeader.EndDate?.ToString("dd/MM/yyyy");
                sendGridParameter.Configuration = ContractorParameter.Configuration;
                if (i.CompanyId == ContractorParameter.AwardWinner.CompanyId)
                {
                    sendGridParameter.MailBody = mStandardMailHeader.TenderWon;
                    sendGridParameter.Subject = mContractorHeader.Title + " " + "Contract Award Decision-Awarded";
                }
                else
                {
                    sendGridParameter.MailBody = mStandardMailHeader.TenderLost;
                    sendGridParameter.Subject = mContractorHeader.Title + " " + "Contract Award Decision-Not Awarded";
                }

                sendGridParameter.DisplayBtn = "none";
                var projLang = tenetConnection.Query<string>(
                    "SELECT pl.Name FROM ProjectDefinition LEFT OUTER JOIN ProjectLanguage pl ON ProjectDefinition.Language = pl.TypeId WHERE ProjectDefinition.SequenceCode = @Id",
                    new { Id = ContractorParameter.ProjectSequenceId }).FirstOrDefault();

                sendGridParameter.TemplateId = projLang switch
                {
                    "en" or null =>
                        ContractorParameter.Configuration.GetValue<string>("TemplateId_en1"),
                    "nl" =>  ContractorParameter.Configuration.GetValue<string>("TemplateId_nl1"),
                    _ => sendGridParameter.TemplateId
                };

                sendGridParameter.ProjLang = projLang;
                // if (ContractorParameter.Lang == "en")
                //     sendGridParameter.TemplateId = ContractorParameter.Configuration.GetValue<string>("TemplateId_en1");
                // if (ContractorParameter.Lang == "nl")
                //     sendGridParameter.TemplateId = ContractorParameter.Configuration.GetValue<string>("TemplateId_nl1");
                
                var send = await ContractorParameter.SendGridRepositorie.SendInvitation(sendGridParameter);
            }
        }

        return "ok";
    }

    public async Task<string> ContractorLotUploadDocuments(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var tenetConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


            var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
                new { Oid = ContractorParameter.UserId }).FirstOrDefault();
            var companyName = tenetConnection.Query<string>("SELECT Name FROM CabCompany WHERE Id = @Id",
                new { Id = companyId }).FirstOrDefault();

            // var uploadedItem = await ContractorParameter.GraphServiceClient
            //     .Drive
            //     .Root
            //     //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
            //     .ItemWithPath(ContractorParameter.ContractorLotUploadedDocs.LotId + "/" + companyName + "/" +
            //                   ContractorParameter.ContractorLotUploadedDocs.Type + "/" +
            //                   ContractorParameter.File.FileName)
            //     .Content
            //     .Request()
            //     .PutAsync<DriveItem>(ContractorParameter.File.OpenReadStream());


            var type = "view";

            var password = "ThisIsMyPrivatePassword";

            var scope = "anonymous";

            // var result = await ContractorParameter.GraphServiceClient
            //     .Drive
            //     .Root
            //     //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
            //     .ItemWithPath(ContractorParameter.ContractorLotUploadedDocs.LotId + "/" + companyName + "/" +
            //                   ContractorParameter.ContractorLotUploadedDocs.Type)
            //     .CreateLink(type, scope)
            //     .Request()
            //     .PostAsync();


            // var query =
            //     @"INSERT INTO dbo.ContractorLotUploadedDocs ( Id ,LotId ,CompanyId ,Type ,Link ,Title ,CreatedDate , FileType ) VALUES ( @Id ,@LotId ,@CompanyId ,@Type ,@Link ,@Title ,@CreatedDate , @FileType );";
            //
            // var parm = new
            // {
            //     Id = Guid.NewGuid().ToString(),
            //     ContractorParameter.ContractorLotUploadedDocs.LotId,
            //     CompanyId = companyId,
            //     ContractorParameter.ContractorLotUploadedDocs.Type, //progress , technical
            //     Link = uploadedItem.WebUrl,
            //     Title = ContractorParameter.File.FileName,
            //     CreatedDate = DateTime.UtcNow,
            //     ContractorParameter.ContractorLotUploadedDocs.FileType
            // };
            //
            // using (var connection = new SqlConnection(connectionString))
            // {
            //     await connection.ExecuteAsync(query, parm);
            // }


            return "uploadedItem.WebUrl";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<GetContractorLotUploadedDocs>> GetContractorLotUploadDocuments(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var tenetConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var data = new List<GetContractorLotUploadedDocs>();

        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        await using var connection = new SqlConnection(connectionString);

        if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
        {
            var result = connection.Query<ContractorLotUploadedDocs>(
                @"Select * From ContractorLotUploadedDocs WHERE LotId = @LotId",
                new { LotId = ContractorParameter.Id });

            var companyGroup = result.GroupBy(x => x.CompanyId).ToList();

            foreach (var company in companyGroup)
            {
                var companyList = new GetContractorLotUploadedDocs();
                companyList.Company = tenetConnection.Query<string>("SELECT Name FROM CabCompany WHERE Id = @Id",
                    new { Id = company.Key }).FirstOrDefault();
                var typeGroup = company.GroupBy(v => v.Type);
                foreach (var type in typeGroup)
                    if (type.Key == "progress")
                        companyList.ProgressDocuments = type;
                    else if (type.Key == "technical") companyList.TechnicalDocuments = type;

                data.Add(companyList);
            }
        }
        else
        {
            var result = connection.Query<ContractorLotUploadedDocs>(
                @"Select * From ContractorLotUploadedDocs WHERE LotId = @LotId AND CompanyId = @CompanyId",
                new { LotId = ContractorParameter.Id, CompanyId = companyId });
            var companyList = new GetContractorLotUploadedDocs();
            companyList.Company = tenetConnection.Query<string>("SELECT Name FROM CabCompany WHERE Id = @Id",
                new { Id = companyId }).FirstOrDefault();

            var typeGroup = result.GroupBy(x => x.Type).ToList();
            foreach (var type in typeGroup)
                if (type.Key == "progress")
                    companyList.ProgressDocuments = type;
                else if (type.Key == "technical") companyList.TechnicalDocuments = type;

            data.Add(companyList);
        }

        return data;
    }

    public async Task<GetUserInformationDto> GetUserInformation(ContractorParameter ContractorParameter)
    {
        var query =
            "SELECT CabPerson.FullName AS UserName ,CabPerson.Id AS UserId ,CabCompany.Name AS Organisation ,CabCompany.Id AS OrganisationId FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id INNER JOIN dbo.CabCompany ON CabPersonCompany.CompanyId = CabCompany.Id WHERE CabPersonCompany.Oid = @Oid";

        var selectBu =
            @"SELECT OrganizationTaxonomy.Id FROM dbo.OrganizationTaxonomy LEFT OUTER JOIN dbo.CabPersonCompany ON OrganizationTaxonomy.PersonId = CabPersonCompany.Id LEFT OUTER JOIN dbo.OrganizationTaxonomy c ON OrganizationTaxonomy.ParentId = c.Id WHERE Oid = @UserId AND OrganizationTaxonomy.OrganizationTaxonomyLevelId = 'oo10e768-3e06-po02-b337-ee367a82admn'";

        var projectSelect = @"SELECT ProjectDefinition.Title ,ProjectDefinition.ProjectConnectionString ,ProjectDefinition.SequenceCode ,CabCompany.SequenceCode AS ContractingUnitId, CabCompany.Id,ProjectClassification.ProjectClassificationBuisnessUnit AS BuId FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId WHERE ProjectClassification.ProjectClassificationBuisnessUnit IN @BuIds  ORDER BY ProjectDefinition.SequenceCode";
        
        GetUserInformationDto GetUserInformationDto;

        using (var connection =ContractorParameter.TenantProvider.orgSqlConnection())
        {
            GetUserInformationDto = connection.Query<GetUserInformationDto>(query,
                new { Oid = ContractorParameter.UserId }).FirstOrDefault();

            var bu = connection.Query<OrganizationTaxonomy>(selectBu, new { ContractorParameter.UserId }).ToList();
            var projects = connection.Query<ProjectDefinition>(projectSelect, new { BuIds = bu.Select((s=>s.Id)) });
            foreach (var i in bu)
            {

                var project = projects.FirstOrDefault(s => s.BuId == i.Id);

                if (project != null)
                {
                    if (GetUserInformationDto != null)
                    {
                        var mContractingUnit = new ContractingUnit();
                        mContractingUnit.Id = project.Id;
                        mContractingUnit.SequnceCode = project.ContractingUnitId;
                        GetUserInformationDto.ContractingUnit = mContractingUnit;
                    }

                    break;
                }
            }
        }

        return GetUserInformationDto;
    }

    public async Task<GetUserTypeDto> GetLoggedUserType(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var tenetConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var data = new GetUserTypeDto();

        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        await using var connection = new SqlConnection(connectionString);

        if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
            data.IsContractor = false;
        else
            data.IsContractor = true;

        return data;
    }

    public async Task<GetContractorByIdForMailDto> GetContractorByIdForMail(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var approve = @"SELECT Approve FROM dbo.ContractorTeamList WHERE InvitationId = @Id";

        ContractorTeamList contractorTeamList;
        using (var connection =
               new SqlConnection(connectionString))
        {
            contractorTeamList = connection.Query<ContractorTeamList>(approve, new { ContractorParameter.Id })
                .FirstOrDefault();
        }


        var query =
            @"SELECT ProjectDefinition.SequenceCode, ProjectDefinition.Name AS ProjectName ,CabPerson.FullName AS Customer ,ProjectClassificationSector.Name AS Sector ,ProjectTime.TenderEndDate AS EndDate ,ProjectDefinition.Description ,CabCompany.Name AS ContractingUnit FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.CabPerson ON ProjectDefinition.CustomerId = CabPerson.Id LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId LEFT OUTER JOIN dbo.ProjectClassificationSector ON ProjectClassification.ProjectClassificationSectorId = ProjectClassificationSector.TypeId LEFT OUTER JOIN dbo.ProjectTime ON ProjectTime.ProjectId = ProjectDefinition.Id LEFT OUTER JOIN dbo.CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE ProjectDefinition.SequenceCode = @Id AND (ProjectClassificationSector.LanguageCode = @lang OR ProjectClassification.ProjectClassificationSectorId IS NULL OR ProjectTime.TenderEndDate IS NULL)";

        var architect =
            @"SELECT CabPerson.FullName FROM dbo.ProjectTeam INNER JOIN dbo.ProjectDefinition ON ProjectTeam.ProjectId = ProjectDefinition.Id INNER JOIN dbo.ProjectTeamRole ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id INNER JOIN dbo.CabPerson ON ProjectTeamRole.CabPersonId = CabPerson.Id WHERE ProjectDefinition.SequenceCode = @ID AND ProjectTeamRole.RoleId = 'tec51857-arch-44b4-8d0e-362ba468000c'";

        var lotquery =
            @"SELECT ContractorTeamList.LotContractorId ,ContractorTeamList.Approve ,ContractorTeamList.IsDownloded ,ContractorTeamList.IsSubscribed ,ContractorHeader.Title AS LotTitle,ContractorHeader.Name ,ContractorHeader.StartDate,ContractorHeader.EndDate,ContractorHeader.SequenceId As LotId FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorTeamList.InvitationId = @Id";
        GetContractorByIdForMailDto mGetContractorByIdForMailDto;

        using (var connection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            mGetContractorByIdForMailDto = connection.Query<GetContractorByIdForMailDto>(query,
                new { Id = ContractorParameter.ProjectSequenceId, lang = ContractorParameter.Lang }).FirstOrDefault();
            var Architect = connection.Query<string>(architect,
                new { Id = ContractorParameter.ProjectSequenceId, lang = ContractorParameter.Lang }).FirstOrDefault();
            if (Architect != null) mGetContractorByIdForMailDto.Architect = Architect;
        }

        using (var connection =
               new SqlConnection(connectionString))
        {
            var lot = connection.Query<ContractorTeamListDto>(lotquery, new { ContractorParameter.Id })
                .FirstOrDefault();
            if (lot != null)
                if (mGetContractorByIdForMailDto != null)
                {
                    mGetContractorByIdForMailDto.Approve = lot.Approve;
                    mGetContractorByIdForMailDto.Id = lot.LotContractorId;
                    mGetContractorByIdForMailDto.LotStartDate = lot.StartDate;
                    mGetContractorByIdForMailDto.LotEndDate = lot.EndDate;
                    mGetContractorByIdForMailDto.LotTitle = lot.Name;
                    mGetContractorByIdForMailDto.IsDownloded = lot.IsDownloded;
                    mGetContractorByIdForMailDto.IsSubscribed = lot.IsSubscribed;
                    mGetContractorByIdForMailDto.LotId = lot.LotId;
                    mGetContractorByIdForMailDto.TenderDocs = connection
                        .Query<DownloadLotLinks>(
                            "SELECT Link, Title FROM dbo.ContractorTenderDocs WHERE LotId = @LotId",
                            new { LotId = lot.LotContractorId }).ToList();
                    mGetContractorByIdForMailDto.TechnicalDocs = connection
                        .Query<DownloadLotLinks>(
                            "SELECT Link, Title FROM dbo.ContractorTechInstructionsDocs WHERE LotId = @LotId",
                            new { LotId = lot.LotContractorId }).ToList();
                }
        }

        return mGetContractorByIdForMailDto;
    }

    public async Task<GetContractorByIdForMailDto> GetContractorByIdForSubscribMail(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var approve = @"SELECT Approve FROM dbo.ContractorTeamList WHERE InvitationId = @Id";

        ContractorTeamList contractorTeamList;
        using (var connection =
               new SqlConnection(connectionString))
        {
            contractorTeamList = connection.Query<ContractorTeamList>(approve, new { ContractorParameter.Id })
                .FirstOrDefault();
        }


        var query =
            @"SELECT ProjectDefinition.SequenceCode, ProjectDefinition.Name AS ProjectName ,CabPerson.FullName AS Customer ,ProjectClassificationSector.Name AS Sector ,ProjectTime.TenderEndDate AS EndDate ,ProjectDefinition.Description ,CabCompany.Name AS ContractingUnit FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.CabPerson ON ProjectDefinition.CustomerId = CabPerson.Id LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId LEFT OUTER JOIN dbo.ProjectClassificationSector ON ProjectClassification.ProjectClassificationSectorId = ProjectClassificationSector.TypeId LEFT OUTER JOIN dbo.ProjectTime ON ProjectTime.ProjectId = ProjectDefinition.Id LEFT OUTER JOIN dbo.CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE ProjectDefinition.SequenceCode = @Id AND (ProjectClassificationSector.LanguageCode = @lang OR ProjectClassification.ProjectClassificationSectorId IS NULL OR ProjectTime.TenderEndDate IS NULL)";

        var architect =
            @"SELECT CabPerson.FullName FROM dbo.ProjectTeam INNER JOIN dbo.ProjectDefinition ON ProjectTeam.ProjectId = ProjectDefinition.Id INNER JOIN dbo.ProjectTeamRole ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id INNER JOIN dbo.CabPerson ON ProjectTeamRole.CabPersonId = CabPerson.Id WHERE ProjectDefinition.SequenceCode = @ID AND ProjectTeamRole.RoleId = 'tec51857-arch-44b4-8d0e-362ba468000c'";

        var lotquery =
            @"SELECT ContractorTeamList.LotContractorId ,ContractorTeamList.Approve ,ContractorTeamList.IsDownloded ,ContractorTeamList.IsSubscribed,ContractorTeamList.IsNotSubscribe,ContractorHeader.Title AS LotTitle,ContractorHeader.Name ,ContractorHeader.StartDate,ContractorHeader.EndDate,ContractorTeamList.IsSubscribed FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorTeamList.InvitationId = @Id";
        GetContractorByIdForMailDto mGetContractorByIdForMailDto;

        using (var connection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            mGetContractorByIdForMailDto = connection.Query<GetContractorByIdForMailDto>(query,
                new { Id = ContractorParameter.ProjectSequenceId, lang = ContractorParameter.Lang }).FirstOrDefault();
            var Architect = connection.Query<string>(architect,
                new { Id = ContractorParameter.ProjectSequenceId, lang = ContractorParameter.Lang }).FirstOrDefault();
            if (Architect != null) mGetContractorByIdForMailDto.Architect = Architect;
        }

        using (var connection =
               new SqlConnection(connectionString))
        {
            var lot = connection.Query<ContractorTeamListDto>(lotquery, new { ContractorParameter.Id })
                .FirstOrDefault();
            if (lot != null)
            {
                mGetContractorByIdForMailDto.Approve = lot.Approve;
                mGetContractorByIdForMailDto.Id = lot.LotContractorId;
                mGetContractorByIdForMailDto.LotStartDate = lot.StartDate;
                mGetContractorByIdForMailDto.LotEndDate = lot.EndDate;
                mGetContractorByIdForMailDto.LotTitle = lot.Name;
                mGetContractorByIdForMailDto.IsDownloded = lot.IsDownloded;
                mGetContractorByIdForMailDto.IsSubscribed = lot.IsSubscribed;
                mGetContractorByIdForMailDto.IsNotSubscribe = lot.IsNotSubscribe;
                mGetContractorByIdForMailDto.TenderDocs = connection
                    .Query<DownloadLotLinks>("SELECT Link, Title FROM dbo.ContractorTenderDocs WHERE LotId = @LotId",
                        new { LotId = lot.LotContractorId }).ToList();
            }
        }

        return mGetContractorByIdForMailDto;
    }

    public async Task<List<DownloadLotLinks>> DownloadLotDocuments(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var cuconnectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                null, ContractorParameter.TenantProvider);
            
            await using var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


            StandardMailHeader mStandardMailHeader;
            List<DownloadLotLinks> link = null;


            var constructorWf =
                @"SELECT ConstructorWorkFlow.* FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id LEFT OUTER JOIN dbo.ContractorTeamList ON ConstructorWorkFlow.Lot = ContractorTeamList.LotContractorId WHERE ConstructorWorkFlow.CabCompanyId = @Id AND ContractorTeamList.InvitationId = @InvitationId";

            var url = ContractorParameter.Configuration.GetValue<string>("DomainUrl") + "/CU/";

            await using var connection = new SqlConnection(connectionString);
            {
                var sMailId =
                    @"SELECT ContractorHeader.StandardMailId,ContractorHeader.Title,ContractorHeader.Name,ContractorHeader.StartDate,ContractorHeader.EndDate FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorHeader.Id = @Id";

                ContractorHeader mContractorHeader;
                mContractorHeader = connection.Query<ContractorHeader>(sMailId,
                    new
                    {
                        Id = ContractorParameter.DownloadLotDocsDto.LotId
                    }).FirstOrDefault();
                if (mContractorHeader.StandardMailId != null)
                    await using (var cuconnection = new SqlConnection(cuconnectionString))
                    {
                        mStandardMailHeader = cuconnection
                            .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id",
                                new { Id = mContractorHeader.StandardMailId }).FirstOrDefault();
                    }

                else
                    await using (var cuconnection = new SqlConnection(cuconnectionString))
                    {
                        mStandardMailHeader = cuconnection
                            .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader").FirstOrDefault();
                    }

                // await connection.ExecuteAsync(download,
                //     new
                //     {
                //         ContractorParameter.DownloadLotDocsDto.InvitationId
                //     });


                var company = connection.Query<ContractorTeamList>(
                    "SELECT CompanyId FROM ContractorTeamList WHERE InvitationId = @InvitationId",
                    new { ContractorParameter.DownloadLotDocsDto.InvitationId }).FirstOrDefault();


                var ConstructorWorkFlow = connection.Query<ConstructorWorkFlow>(constructorWf,
                        new { Id = company.CompanyId, ContractorParameter.DownloadLotDocsDto.InvitationId })
                    .FirstOrDefault();


                var personId = connection
                    .Query<string>(
                        "Select CabPersonId From ContractorTeamList WHERE LotContractorId = @lotId AND CompanyId = @companyId",
                        new { lotId = ConstructorWorkFlow.Lot, companyId = ConstructorWorkFlow.CabCompanyId })
                    .FirstOrDefault();
                
                var projLang = dbConnection.Query<string>(
                    "SELECT pl.Name FROM ProjectDefinition LEFT OUTER JOIN ProjectLanguage pl ON ProjectDefinition.Language = pl.TypeId WHERE ProjectDefinition.SequenceCode = @Id",
                    new { Id = ContractorParameter.ProjectSequenceId }).FirstOrDefault();

                var send = false;
                if (ConstructorWorkFlow.Status != "d60aad0b-2e84-con1-ad25-Lot0d49477")
                {
                    
                    var sendGridParameter = new SendGridParameter();
                    sendGridParameter.Id = personId;
                    sendGridParameter.TenantProvider = ContractorParameter.TenantProvider;
                    sendGridParameter.ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId;
                    sendGridParameter.ProjectSequenceId = ContractorParameter.ProjectSequenceId;
                    sendGridParameter.Lot = ConstructorWorkFlow.Lot;
                    sendGridParameter.Lang = ContractorParameter.Lang;
                    sendGridParameter.MailBody = mStandardMailHeader.SubscribeTender;
                    sendGridParameter.LotTitle = mContractorHeader.Name;
                    sendGridParameter.StartDate = mContractorHeader.StartDate?.ToString("dd/MM/yyyy");
                    sendGridParameter.EndDate = mContractorHeader.EndDate?.ToString("dd/MM/yyyy");
                    //sendGridParameter.Subject = mContractorHeader.Title + " " + "Subscribe";
                    sendGridParameter.Configuration = ContractorParameter.Configuration;
                    sendGridParameter.Url = url +
                                            ContractorParameter.ContractingUnitSequenceId + "/project/" +
                                            ContractorParameter.ProjectSequenceId + "/lot-subscribe/" +
                                            ConstructorWorkFlow.SequenceId + "/" +
                                            ContractorParameter.DownloadLotDocsDto.InvitationId +
                                            "" + "/" + projLang;
                    //sendGridParameter.ButtonText = "klik hier om aan te geven of u wenst een offerte aan te bieden.";



                    sendGridParameter.StatusImage = projLang switch
                    {
                        "en" or null =>
                            "https://bmengineeringuprinceuat.blob.core.windows.net/uprincev4dev/en-subscribe.png",
                        "nl" => "https://bmengineeringuprinceuat.blob.core.windows.net/uprincev4dev/nl-subscribe.png",
                        _ => sendGridParameter.StatusImage
                    };

                    // if (ContractorParameter.Lang == "en")
                    // {
                    //     sendGridParameter.StatusImage = "https://bmengineeringuprinceuat.blob.core.windows.net/uprincev4dev/en-subscribe.png";
                    //
                    // }
                    //
                    // else
                    // {
                    //     sendGridParameter.StatusImage = "https://bmengineeringuprinceuat.blob.core.windows.net/uprincev4dev/nl-subscribe.png";
                    //
                    // }

                    sendGridParameter.ButtonText = projLang switch
                    {
                        "en" or null =>
                            "Click here to subscribe.",
                        "nl" => "klik hier om aan te geven of u wenst een offerte aan te bieden.",
                        _ => sendGridParameter.ButtonText
                    };
                    sendGridParameter.Subject = projLang switch
                    {
                        "en" or null =>
                            mContractorHeader.Title + " " + "Subscribe",
                        "nl" => mContractorHeader.Title + " " + "Inschrijven",
                        _ => sendGridParameter.Subject
                    };

                    sendGridParameter.ProjLang = projLang;
                    sendGridParameter.DisplayImage = "block";
                    sendGridParameter.DisplayBtn = "block";
                     send = await ContractorParameter.SendGridRepositorie.SendInvitation(sendGridParameter);

                }
                
                link = connection
                    .Query<DownloadLotLinks>(
                        "SELECT Link, Title FROM dbo.ContractorTenderDocs WHERE LotId = @LotId",
                        new { LotId = ConstructorWorkFlow.Lot }).ToList();

                await ConstructorWfStatusUpdate("d60aad0b-2e84-con1-ad25-Lot0d49477", ConstructorWorkFlow.Id,
                    connectionString);
            }
            return link;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> SubscribeLot(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
            
            var constructorWf =
                @"SELECT ConstructorWorkFlow.* FROM dbo.ConstructorWorkFlow LEFT OUTER JOIN dbo.ContractorHeader ON ConstructorWorkFlow.Lot = ContractorHeader.Id LEFT OUTER JOIN dbo.ContractorTeamList ON ConstructorWorkFlow.Lot = ContractorTeamList.LotContractorId WHERE ConstructorWorkFlow.CabCompanyId = @Id AND ContractorTeamList.InvitationId = @InvitationId";


            if (ContractorParameter.DownloadLotDocsDto.IsNotSubscribe)
            {
                var update = @"UPDATE dbo.ContractorTeamList SET IsNotSubscribe = 1 WHERE InvitationId = @InvitationId;";
                await using var connection = new SqlConnection(connectionString);
                {
                    await connection.ExecuteAsync(update,
                        new
                        {
                            ContractorParameter.DownloadLotDocsDto.InvitationId
                        });
                    
                    var company = connection.Query<ContractorTeamList>(
                        "SELECT CompanyId FROM ContractorTeamList WHERE InvitationId = @InvitationId",
                        new { ContractorParameter.DownloadLotDocsDto.InvitationId }).FirstOrDefault();
                    
                    var ConstructorWorkFlow = connection.Query<ConstructorWorkFlow>(constructorWf,
                            new { Id = company.CompanyId, ContractorParameter.DownloadLotDocsDto.InvitationId })
                        .FirstOrDefault();
                    
                    var query = @"UPDATE dbo.ConstructorWorkFlow SET Status =@StatusId WHERE Id = @wfId ;";
                    var timetableup =
                        @"INSERT INTO dbo.ConstructorWfStatusChangeTime ( Id ,ConstructorWf ,StatusId ,DateTime,SubscriptionComment) VALUES ( @Id ,@wfId ,@StatusId ,@DateTime,@Comment );";

                    var param = new
                    {
                        StatusId = "xxxxad0b-2e84-nsub-ad25-Lot0d49477",
                        wfId = ConstructorWorkFlow.Id,
                        Id = Guid.NewGuid().ToString(),
                        DateTime = DateTime.UtcNow,
                        ContractorParameter.DownloadLotDocsDto.Comment
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
                }

            }
            else
            {
                var download =
                    @"UPDATE dbo.ContractorTeamList SET IsSubscribed = 1 WHERE InvitationId = @InvitationId;";
                
                var url = ContractorParameter.Configuration.GetValue<string>("DomainUrl") + "/CU/";

                await using var connection = new SqlConnection(connectionString);
                {
                    await connection.ExecuteAsync(download,
                        new
                        {
                            ContractorParameter.DownloadLotDocsDto.InvitationId
                        });

                    var company = connection.Query<ContractorTeamList>(
                        "SELECT CompanyId FROM ContractorTeamList WHERE InvitationId = @InvitationId",
                        new { ContractorParameter.DownloadLotDocsDto.InvitationId }).FirstOrDefault();


                    var ConstructorWorkFlow = connection.Query<ConstructorWorkFlow>(constructorWf,
                            new { Id = company.CompanyId, ContractorParameter.DownloadLotDocsDto.InvitationId })
                        .FirstOrDefault();


                    var personId = connection
                        .Query<string>(
                            "Select CabPersonId From ContractorTeamList WHERE LotContractorId = @lotId AND CompanyId = @companyId",
                            new { lotId = ConstructorWorkFlow.Lot, companyId = ConstructorWorkFlow.CabCompanyId })
                        .FirstOrDefault();

                    await ConstructorWfStatusUpdate("7143ff01-d173-con6-8c17-Lotecdb84c", ConstructorWorkFlow.Id,connectionString);
                }
                
            }
            return ContractorParameter.DownloadLotDocsDto.InvitationId;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<CBCExcelLotDataParent>> GetLotCbcTree(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        List<CBCExcelLotDataParent> data = null;

        data = connection.Query<CBCExcelLotDataParent>("Select * From CBCExcelLotData Where ContractId = @ContractId",
            new { ContractId = ContractorParameter.Id }).ToList();

        foreach (var item in data)
        {
            var subTotal = connection.Query<float>(@"WITH ret
                                                                            AS
                                                                            (SELECT
                                                                            *
                                                                            FROM CBCExcelLotData
                                                                            WHERE Id = @Id
                                                                            AND ContractId = @ContractId
                                                                            UNION ALL
                                                                            SELECT
                                                                            t.*
                                                                            FROM CBCExcelLotData t
                                                                            INNER JOIN ret r
                                                                            ON t.ParentId = r.ArticleNo
                                                                            WHERE t.ContractId = @ContractId)
                                                                            SELECT
                                                                            SUM(TotalPrice) AS SubTotal
                                                                            FROM ret",
                new { item.Id, ContractId = ContractorParameter.Id }).FirstOrDefault();

            item.TotalPrice = subTotal;
        }

        // data = data
        //     .Where(c => c.ParentId == "0")
        //     .Select(c => new CBCExcelLotDataParent()
        //     {
        //         Id = c.Id,
        //         ArticleNo = c.ArticleNo,
        //         ParentId = c.ParentId,
        //         Title = c.Title,
        //         Children = GetChildren(data, c.ArticleNo)
        //         
        //
        //     })
        //     .ToList();
        //
        // return HieararchyWalk(data);

        return data.OrderBy(c => c.ArticleNo).ToList();
    }

    public async Task<IEnumerable<ContractorsPsList>> GetPsSequenceIdByLotIdForZeroState(
        ContractorParameter ContractorParameter)
    {
        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var connection = new SqlConnection(connectionString);

        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();
        List<ContractorsPsList> data = null;


        data = connection.Query<ContractorsPsList>(
            "SELECT ContractorPs.*, ContractorTotalValues.IsWinner FROM dbo.ContractorPs LEFT OUTER JOIN dbo.ContractorTotalValues ON ContractorPs.LotId = ContractorTotalValues.LotId WHERE ContractorPs.LotId = @LotId AND ContractorTotalValues.IsWinner = 1",
            new { LotId = ContractorParameter.Id }).ToList();

        data = data.DistinctBy(x => x.PsSequenceId).ToList();

        var errors = await ContractorPsErrorLogForZeroState(ContractorParameter);

        foreach (var item in data)
        {
            item.IsError = errors.Any(x => x.PsSequenceId == item.PsSequenceId);
            item.IsPublished = connection
                .Query<string>(
                    @"Select Id From ContractorPsPublished Where LotId = @LotId And PsSequenceId = @PsSequenceId",
                    new { LotId = ContractorParameter.Id, item.PsSequenceId }).Any();

            var total = connection
                .Query<string>(
                    @"SELECT SUM(CAST(Total AS FLOAT)) Total FROM ContractorPs WHERE PsSequenceId = @PsSequenceId AND LotId = @LotId AND Total IS NOT NULL  ",
                    new { item.PsSequenceId, LotId = ContractorParameter.Id }).FirstOrDefault();

            if (total != null)
                item.Total = total.ToFloat();
            else
                item.Total = 0;
        }

        data.ForEach(x => x.Order = x.PsOrderNumber.ToInt());

        return data.OrderBy(c => c.Order);
    }

    public async Task<List<ExcelLotDataDtoTest>> GetContractorPsDataForZeroState(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


        await using var connection = new SqlConnection(connectionString);

        string subQuery = null;

        string subTotlalQuery = null;


        var query =
            @"SELECT ArticleNo AS Id ,Title ,ParentId , Id As UId ,ArticleNo ,Quantity ,ContractId,Unit,UnitPrice,TotalPrice,MeasurementCode , Mou ,Key1, Value1, Key2, Value2, Key3, Value3,Key4, Value4, Key5, Value5,IsExclude,RealArticleNo   FROM dbo.CBCExcelLotData where ContractId = @ContractId;
                            SELECT PublishedContractorsPdfData.Id ,PublishedContractorsPdfData.ArticleNo ,PublishedContractorsPdfData.CompanyId ,PublishedContractorsPdfData.Title ,PublishedContractorsPdfData.Unit ,PublishedContractorsPdfData.VH ,PublishedContractorsPdfData.Quantity ,PublishedContractorsPdfData.UnitPrice ,PublishedContractorsPdfData.TotalPrice ,PublishedContractorsPdfData.RealArticleNo FROM dbo.PublishedContractorsPdfData WHERE LotId =@ContractId ORDER BY PublishedContractorsPdfData.ArticleNo DESC;
                            SELECT * FROM ContractorPsPublished Where LotId = @ContractId ;
                            SELECT ContractorPsPublished.*, ContractorPsPublished.ArticleNumber As ArticleNo FROM ContractorPsPublished Where LotId = @ContractId";

        var query2 =
            @"SELECT ArticleNo AS Id ,Title ,ParentId , Id As UId ,ArticleNo ,Quantity ,ContractId,Unit,UnitPrice,TotalPrice,MeasurementCode , Mou ,Key1, Value1, Key2, Value2, Key3, Value3,Key4, Value4, Key5, Value5 ,IsExclude,RealArticleNo  FROM dbo.CBCExcelLotdataPublished where ContractId = @ContractId;
                            SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice , ContractorPdfData.RealArticleNo FROM dbo.ContractorPdfData WHERE LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC;
                            SELECT * FROM ContractorPs Where LotId = @ContractId;
                            SELECT ContractorPs.*, ContractorPs.ArticleNumber As ArticleNo FROM ContractorPs Where LotId = @ContractId";
        // string query2 = @"SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LEN(ContractorPdfData.Unit) < 3 AND ISNUMERIC(ContractorPdfData.Unit) = 0 AND LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC ";
        //string query2 = @"SELECT ContractorPdfData.Id ,ContractorPdfData.ArticleNo ,ContractorPdfData.CompanyId ,ContractorPdfData.Title ,ContractorPdfData.Unit ,ContractorPdfData.VH ,ContractorPdfData.Quantity ,ContractorPdfData.UnitPrice ,ContractorPdfData.TotalPrice FROM dbo.ContractorPdfData WHERE LotId =@ContractId ORDER BY ContractorPdfData.ArticleNo DESC ";

        var isZero = @"SELECT CompanyId FROM dbo.ContractorTotalValues WHERE IsWinner = 1 AND LotId = @ContractId";

        var company = connection.Query<string>(isZero, new { ContractId = ContractorParameter.Id }).FirstOrDefault();

        List<ExcelLotDataDtoTest> data = null;
        IEnumerable<ExcelLotDataDtoTest> pdf;
        IEnumerable<ContractorPs> psData;
        IEnumerable<ExcelLotDataDtoTest> PsDataNew;


        var companyId = dbConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        if (company != null)
        {
            if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
            {
                using (var multi = await connection.QueryMultipleAsync(query,
                           new
                           {
                               ContractId = ContractorParameter.Id
                           }))
                {
                    data = multi.Read<ExcelLotDataDtoTest>().ToList();
                    pdf = multi.Read<ExcelLotDataDtoTest>();
                    psData = multi.Read<ContractorPs>();
                    PsDataNew = multi.Read<ExcelLotDataDtoTest>();
                }

                subQuery =
                    "Select * from PublishedContractorsPdfData Where ArticleNo In @ArticleNos AND LotId =@ContractId AND CompanyId = @CompanyId";
                subTotlalQuery = @"WITH ret
                                                                            AS
                                                                            (SELECT
                                                                            *
                                                                            FROM CBCExcelLotData
                                                                            WHERE Id = @Id
                                                                            AND ContractId = @ContractId
                                                                            UNION ALL
                                                                            SELECT
                                                                            t.*
                                                                            FROM CBCExcelLotData t
                                                                            INNER JOIN ret r
                                                                            ON t.ParentId = r.ArticleNo
                                                                            WHERE t.ContractId = @ContractId AND t.IsExclude = 0)
                                                                            SELECT
                                                                            *
                                                                            FROM ret ";
            }
            else
            {
                using (var multi = await connection.QueryMultipleAsync(query2,
                           new
                           {
                               ContractId = ContractorParameter.Id
                           }))
                {
                    data = multi.Read<ExcelLotDataDtoTest>().ToList();
                    pdf = multi.Read<ExcelLotDataDtoTest>();
                    psData = multi.Read<ContractorPs>();
                    PsDataNew = multi.Read<ExcelLotDataDtoTest>();
                }

                subQuery =
                    "Select * from ContractorPdfData Where ArticleNo In @ArticleNos AND LotId =@ContractId AND CompanyId = @CompanyId";
                subTotlalQuery = @"WITH ret
                                                                            AS
                                                                            (SELECT
                                                                            *
                                                                            FROM CBCExcelLotdataPublished
                                                                            WHERE Id = @Id
                                                                            AND ContractId = @ContractId
                                                                            UNION ALL
                                                                            SELECT
                                                                            t.*
                                                                            FROM CBCExcelLotdataPublished t
                                                                            INNER JOIN ret r
                                                                            ON t.ParentId = r.ArticleNo
                                                                            WHERE t.ContractId = @ContractId AND t.IsExclude = 0)
                                                                            SELECT
                                                                            *
                                                                            FROM ret ";
            }

            pdf = pdf.Where(c => c.CompanyId == company);

            var articleList =
                dbConnection.Query<string>("SELECT ProductId FROM dbo.PbsProduct").ToList();

            foreach (var lot in data)
            {
                var contractors = pdf.Where(c => c.ArticleNo == lot.ArticleNo);
                lot.Contractors = contractors.DistinctBy(v => v.CompanyId);
                lot.Ps = psData.Where(x => x.ArticleNumber == lot.ArticleNo);

                var subTotal = connection.Query<ExcelLotDataDtoTest>(subTotlalQuery,
                    new { Id = lot.UId, ContractId = ContractorParameter.Id });

                lot.SubTotal = subTotal?.Sum(x => x.TotalPrice.ToFloat());
                lot.IsParent = subTotal?.Count() > 1;
                var subList = subTotal.Select(x => x.ArticleNo).ToList();

                foreach (var con in lot.Contractors)
                {
                    // var gg = pdf.Where(x => subTotal.All(v => v.ArticleNo == x.ArticleNo))
                    //     .Where(b => b.CompanyId == con.CompanyId);
                    var gg = connection.Query<ExcelLotDataDtoTest>(
                        subQuery,
                        new { ArticleNos = subList, ContractId = ContractorParameter.Id, con.CompanyId });
                    con.SubTotal = gg.Sum(x => x.TotalPrice.ToFloat());
                    con.IsParent = lot.IsParent;
                }
            }

            var errorsCbc = data.Where(p => articleList.All(p2 => p2 != p.RealArticleNo))
                .ToList();
            errorsCbc.ForEach(c => c.isError = true);

            //data.AddRange(errorsCbc);

            var errors = pdf.Where(p => data.All(p2 => p2.ArticleNo != p.ArticleNo.Trim()))
                .ToList();

            errors.ForEach(c => c.isError = true);

            data.AddRange(errors);

            var errorsps = PsDataNew.Where(p => data.All(p2 => p2.ArticleNo != p.ArticleNo.Trim()))
                .ToList();

            errorsps.ForEach(c => c.isError = true);

            data.AddRange(errorsps);

            //return data.OrderBy(c => c.ArticleNo).ToList();


            return data.OrderBy(c => c.ArticleNo).ToList();
        }

        return data;
    }

    public async Task<List<ContractorPdfErrorLog>> ContractorPsErrorLogForZeroState(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


        var data = new List<ContractorPdfErrorLog>();

        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        IEnumerable<ContractorPs> psData = null;
        List<ContractorPdfErrorLog> excelData = null;

        if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
        {
            psData = dbConnection.Query<ContractorPs>("SELECT * FROM ContractorPsPublished Where LotId = @ContractId",
                new { ContractId = ContractorParameter.Id });

            excelData = dbConnection
                .Query<ContractorPdfErrorLog>(
                    @"SELECT Title ,ParentId ,ArticleNo ,Quantity ,ContractId ,UnitPrice, TotalPrice,IsExclude FROM dbo.CBCExcelLotData where ContractId = @ContractId",
                    new { ContractId = ContractorParameter.Id }).ToList();
        }
        else
        {
            psData = dbConnection.Query<ContractorPs>("SELECT * FROM ContractorPs Where LotId = @ContractId",
                new { ContractId = ContractorParameter.Id });

            excelData = dbConnection
                .Query<ContractorPdfErrorLog>(
                    @"SELECT Title ,ParentId ,ArticleNo ,Quantity ,ContractId ,UnitPrice, TotalPrice,IsExclude FROM dbo.CBCExcelLotdataPublished where ContractId = @ContractId",
                    new { ContractId = ContractorParameter.Id }).ToList();
        }


        psData = psData.Where(o => excelData.All(g => g.ParentId != o.ArticleNumber && g.IsExclude == false)).ToList();

        //var fff = pdfData.Where(o => excelData.All(g => g.ArticleNo != o.ArticleNo));


        foreach (var item in psData)
        {
            if (!excelData.Exists(c => c.ArticleNo == item.ArticleNumber.Trim()))
            {
                var listdata = new ContractorPdfErrorLog
                {
                    ArticleNo = item.ArticleNumber,
                    Title = item.Title,
                    Error = "articleNotInLot",
                    CreatedDate = DateTime.UtcNow,
                    ColumnName = "ArticleNo",
                    CompanyId = item.CompanyId,
                    LotId = item.LotId,
                    PsSequenceId = item.PsSequenceId
                };

                data.Add(listdata);
            }

            if (item.Total.IsNullOrEmpty())
            {
                var listdata = new ContractorPdfErrorLog
                {
                    ArticleNo = item.ArticleNumber,
                    Title = item.Title,
                    Error = "totalPriceErrorEmpty",
                    CreatedDate = DateTime.UtcNow,
                    ColumnName = "TotalPrice",
                    CompanyId = item.CompanyId,
                    LotId = item.LotId,
                    PsSequenceId = item.PsSequenceId
                };

                data.Add(listdata);
            }
            else if (!Regex.IsMatch(item.Total, "^[1-9]") && !Regex.IsMatch(item.Total, "0."))
            {
                var listdata = new ContractorPdfErrorLog
                {
                    ArticleNo = item.ArticleNumber,
                    Title = item.Title,
                    Error = "totalPriceIs0",
                    CreatedDate = DateTime.UtcNow,
                    ColumnName = "TotalPrice",
                    CompanyId = item.CompanyId,
                    LotId = item.LotId,
                    PsSequenceId = item.PsSequenceId
                };

                data.Add(listdata);
            }

            if (!item.QuantityConsumed.IsNullOrEmpty() && !item.UnitPrice.IsNullOrEmpty() &&
                !item.Total.IsNullOrEmpty())
                if (Regex.IsMatch(item.Total, "^[1-9]") &&
                    Regex.IsMatch(item.UnitPrice, "^[1-9]") &&
                    !Regex.IsMatch(item.QuantityConsumed, "[a-z,A-Z./:!@#$%&*()+]"))
                {
                    // var nn = item.QuantityConsumed.ToFloat().ToString("0.00");
                    // var tt = item.UnitPrice.ToFloat().ToString("0.00");
                    var qq = item.Total.ToFloat().ToString("0.00");
                    var calculatedPricerounded =
                        (item.QuantityConsumed.ToFloat() * item.UnitPrice.ToFloat()).ToString("0.00");
                    if (calculatedPricerounded.ToFloat() != qq.ToFloat())
                    {
                        var listdata = new ContractorPdfErrorLog
                        {
                            ArticleNo = item.ArticleNumber,
                            Title = item.Title,
                            Error = "totalPriceErrorWrong",
                            CreatedDate = DateTime.UtcNow,
                            ColumnName = "TotalPrice",
                            CompanyId = item.CompanyId,
                            LotId = item.LotId,
                            TotalPrice = item.Total.ToFloat(),
                            TotalPricerounded = qq.ToFloat(),
                            calculatedPricerounded = calculatedPricerounded.ToFloat(),
                            UnitPrice = item.UnitPrice.ToFloat(),
                            Quantity = item.QuantityConsumed,
                            PsSequenceId = item.PsSequenceId
                        };

                        data.Add(listdata);
                    }
                }

            if (item.QuantityConsumed.IsNullOrEmpty())
            {
                var listdata = new ContractorPdfErrorLog
                {
                    ArticleNo = item.ArticleNumber,
                    Title = item.Title,
                    Error = "quantityErrorEmpty",
                    CreatedDate = DateTime.UtcNow,
                    ColumnName = "QuantityConsumed",
                    CompanyId = item.CompanyId,
                    LotId = item.LotId,
                    PsSequenceId = item.PsSequenceId
                };

                data.Add(listdata);
            }
            else if (Regex.IsMatch(item.QuantityConsumed, "[a-z,A-Z/:!@#$%&*()+]"))
            {
                var listdata = new ContractorPdfErrorLog
                {
                    ArticleNo = item.ArticleNumber,
                    Title = item.Title,
                    Error = "quantityErrorNotValid",
                    CreatedDate = DateTime.UtcNow,
                    ColumnName = "QuantityConsumed",
                    CompanyId = item.CompanyId,
                    LotId = item.LotId,
                    PsSequenceId = item.PsSequenceId
                };

                data.Add(listdata);
            }

            if (item.UnitPrice.IsNullOrEmpty())
            {
                var listdata = new ContractorPdfErrorLog
                {
                    ArticleNo = item.ArticleNumber,
                    Title = item.Title,
                    Error = "unitPriceErrorEmpty",
                    CreatedDate = DateTime.UtcNow,
                    ColumnName = "UnitPrice",
                    CompanyId = item.CompanyId,
                    LotId = item.LotId,
                    PsSequenceId = item.PsSequenceId
                };

                data.Add(listdata);
            }
            else if (!Regex.IsMatch(item.UnitPrice, "^[1-9]") && !Regex.IsMatch(item.UnitPrice, "0."))
            {
                var listdata = new ContractorPdfErrorLog
                {
                    ArticleNo = item.ArticleNumber,
                    Title = item.Title,
                    Error = "UnitPriceIs0",
                    CreatedDate = DateTime.UtcNow,
                    ColumnName = "UnitPrice",
                    CompanyId = item.CompanyId,
                    LotId = item.LotId,
                    PsSequenceId = item.PsSequenceId
                };

                data.Add(listdata);
            }
        }


        return data.OrderBy(c => c.ArticleNo).ToList();
    }

    public async Task<string> SaveContractorPsData(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        var connection = new SqlConnection(connectionString);
        var tenetConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);

        var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
            new { Oid = ContractorParameter.UserId }).FirstOrDefault();

        if (ContractorParameter.PsData.ContractId != null)
        {
            if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
                await connection.ExecuteAsync("Delete From CBCExcelLotData Where ContractId = @ContractId",
                    new { ContractorParameter.PsData.ContractId });
            else
                await connection.ExecuteAsync("Delete From ContractorPs Where LotId = @ContractId",
                    new { ContractorParameter.PsData.ContractId });

            foreach (var excelData in ContractorParameter.PsData.ExcelData)
                if (companyId == ContractorParameter.Configuration.GetValue<string>("CompanyId"))
                {
                    var sql =
                        "INSERT INTO dbo.CBCExcelLotData ( Id ,Title ,ParentId ,ArticleNo ,Quantity ,ContractId ,Unit ,UnitPrice ,TotalPrice ,MeasurementCode , Mou ,Key1, Value1, Key2, Value2, Key3, Value3 ,IsExclude ,RealArticleNo ) VALUES ( @Id ,@Title ,@ParentId ,@ArticleNo ,@Quantity ,@ContractId ,@Unit ,@UnitPrice ,@TotalPrice ,@MeasurementCode , @Mou , @Key1, @Value1, @Key2, @Value2, @Key3, @Value3 ,@IsExclude ,@RealArticleNo);";
                    // Parallel.ForEach(pdfdata, async data =>
                    // {
                    var param = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        excelData.Title,
                        excelData.ParentId,
                        excelData.ArticleNo,
                        excelData.Quantity,
                        ContractorParameter.PsData.ContractId,
                        excelData.Unit,
                        excelData.UnitPrice,
                        excelData.TotalPrice,
                        excelData.MeasurementCode,
                        excelData.Mou,
                        excelData.Key1,
                        excelData.Value1,
                        excelData.Key2,
                        excelData.Value2,
                        excelData.Key3,
                        excelData.Value3,
                        IsExclude = excelData.IsExclude,
                        RealArticleNo = excelData.RealArticleNo
                    };

                    await connection.ExecuteAsync(sql, param);
                }
                else
                {
                    foreach (var dto in excelData.Ps)
                    {
                        var query =
                            @"INSERT INTO dbo.ContractorPs ( Id , ArticleNumber, Title, MeasurementCode, QuantityQuotation, UnitPrice, QuantityConsumed, Total, LotId, CompanyId ,PsSequenceId ,PsOrderNumber ,IsApproved ,ApprovedDate) VALUES ( @Id , @ArticleNumber, @Title, @MeasurementCode, @QuantityQuotation, @UnitPrice, @QuantityConsumed, @Total, @LotId, @CompanyId ,@PsSequenceId , @PsOrderNumber , @IsApproved , @ApprovedDate)";

                        var parm = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            ArticleNumber = excelData.ArticleNo,
                            excelData.Title,
                            dto.MeasurementCode,
                            dto.QuantityQuotation,
                            dto.UnitPrice,
                            dto.QuantityConsumed,
                            dto.Total,
                            LotId = ContractorParameter.PsData.ContractId,
                            dto.CompanyId,
                            dto.PsSequenceId,
                            dto.PsOrderNumber,
                            dto.IsApproved,
                            dto.ApprovedDate
                        };

                        await connection.ExecuteAsync(query, parm);
                    }
                }
        }


        return ContractorParameter.PsData.ContractId;
    }

    public async Task<string> CreateCommentCardForPs(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        CabPersonCompany CabPersonCompany;
        var company = @"SELECT CompanyId FROM dbo.CabPersonCompany WHERE Oid = @Id";
        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            CabPersonCompany = connection.Query<CabPersonCompany>(company, new { Id = ContractorParameter.UserId })
                .FirstOrDefault();
        }

        string ContractorId = null;
        if (ContractorParameter.CommentCardPs.CompanyId == null)
            ContractorId = CabPersonCompany.CompanyId;
        else
            ContractorId = ContractorParameter.CommentCardPs.CompanyId;

        CommentCardPs CommentCard;
        CommentCardContractorPs mCommentCardContractor;
        var checkArticleNo =
            @"SELECT Id,ArticleNo FROM dbo.CommentCardPs WHERE ArticleNo = @ArticleNo AND  CommentCardPs.ColumnName = @ColumnName AND LotId = @LotId";
        var checkContractor =
            @"SELECT CommentCardContractorPs.Id ,CommentCardContractorPs.CommentCardId ,CommentCardContractorPs.ContractorId,CommentCardContractorPs.PsSequenceId FROM dbo.CommentCardContractorPs INNER JOIN dbo.CommentCardPs ON CommentCardContractorPs.CommentCardId = CommentCardPs.Id WHERE  CommentCardPs.ArticleNo = @ArticleNo AND CommentCardPs.ColumnName = @ColumnName AND CommentCardContractorPs.Accept = '0' AND CommentCardPs.LotId = @LotId AND CommentCardContractorPs.PsSequenceId = @PsSequenceId";
        var insertCommentCard =
            @"INSERT INTO dbo.CommentCardPs ( Id ,Title ,ArticleNo ,CardTitle ,LotId ,ColumnName ,Message ,CompanyId ,Date ) VALUES ( @Id ,@Title ,@ArticleNo ,@CardTitle ,@LotId ,@ColumnName ,@Message ,@CompanyId ,@Date )";

        var insertContractor =
            @"INSERT INTO dbo.CommentCardContractorPs ( Id ,CommentCardId ,ContractorId ,Accept,Field ,Priority ,Severity ,Status,Reporter,CreaterId,ChangeType ,PsSequenceId ) VALUES ( @Id ,@CommentCardId ,@ContractorId ,@Accept,@Field ,@Priority ,@Severity ,@Status,@Reporter,@CreaterId,@ChangeType , @PsSequenceId );";

        string Id = null;
        await using (var connection = new SqlConnection(connectionString))
        {
            CommentCard = connection.Query<CommentCardPs>(checkArticleNo,
                new
                {
                    ContractorParameter.CommentCardPs.ArticleNo, ContractorParameter.CommentCardPs.ColumnName,
                    ContractorParameter.CommentCardPs.LotId
                }).FirstOrDefault();
            mCommentCardContractor = connection.Query<CommentCardContractorPs>(checkContractor,
                new
                {
                    ContractorParameter.CommentCardPs.PsSequenceId, ContractorParameter.CommentCardPs.ArticleNo,
                    ContractorParameter.CommentCardPs.ColumnName, ContractorParameter.CommentCardPs.LotId
                }).FirstOrDefault();
            if (CommentCard == null)
            {
                Id = Guid.NewGuid().ToString();
                var parm = new CommentCardPs
                {
                    Id = Id,
                    Title = ContractorParameter.CommentCardPs.Title,
                    ArticleNo = ContractorParameter.CommentCardPs.ArticleNo,
                    LotId = ContractorParameter.CommentCardPs.LotId,
                    CardTitle = ContractorParameter.CommentCardPs.ArticleNo + " " +
                                ContractorParameter.CommentCardPs.Title + " " +
                                ContractorParameter.CommentCardPs.ColumnName,
                    ColumnName = ContractorParameter.CommentCardPs.ColumnName,
                    Date = DateTime.UtcNow,
                    CompanyId = ContractorId,
                    Message = ContractorParameter.CommentCardPs.Message
                };
                await connection.ExecuteAsync(insertCommentCard, parm);
            }
            else
            {
                Id = CommentCard.Id;
            }

            if (mCommentCardContractor == null)
            {
                var parm1 = new
                {
                    Id = Guid.NewGuid().ToString(),
                    CommentCardId = Id,
                    ContractorId,
                    Field = "7bcb4e8d-Field-487d-cowf-6b91c89fAcce",
                    Priority = "7bcb4e8d-Fiel1-Very-cowf-6b91c89fAcce",
                    Severity = "7bcb4e8d-Field-Seve-cowf-6b91c89fAcce",
                    Status = "7bcb4e8d-8e8c-487d-cowf-6b91c89fAcce",
                    Reporter = ContractorParameter.UserId,
                    CreaterId = CabPersonCompany.CompanyId,
                    Accept = "0",
                    ChangeType = "vvvdkjg4e8d-fhhd-487d-8170-6b91c89fdddvvv",
                    ContractorParameter.CommentCardPs.PsSequenceId
                };

                await connection.ExecuteAsync(insertContractor, parm1);
            }
        }

        return Id;
    }

    public async Task<IEnumerable<CommentCardDto>> GetPsComment(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        CabPersonCompany mCabPersonCompany;
        var companyId = @"SELECT * FROM dbo.CabPersonCompany WHERE Oid = @Id";
        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            mCabPersonCompany = connection.Query<CabPersonCompany>(companyId, new { Id = ContractorParameter.UserId })
                .FirstOrDefault();
            
        }

        var queryCommentCard = @"SELECT * FROM dbo.CommentCardPs WHERE CommentCardPs.LotId = @LotId ";
        StringBuilder sb;
        sb = new StringBuilder(queryCommentCard);

        if (ContractorParameter.CommentFilter.Sorter.Attribute == null)
            sb.Append(" ORDER BY CommentCardPs.ArticleNo asc");

        if (ContractorParameter.CommentFilter.ArticalNo != null)
            if (ContractorParameter.CommentFilter.Sorter.Attribute != null)
            {
                if (ContractorParameter.CommentFilter.Sorter.Attribute.ToLower().Equals("articleno"))
                {
                    var words = ContractorParameter.CommentFilter.ArticalNo.Split(" ");
                    foreach (var element in words) sb.Append("AND CommentCardPs.ArticleNo LIKE '%" + element + "%'");

                    sb.Append(
                        "ORDER BY CommentCardPs.ArticleNo " + ContractorParameter.CommentFilter.Sorter.Order);
                }

                if (ContractorParameter.CommentFilter.Sorter.Attribute.ToLower().Equals("title"))
                {
                    var words = ContractorParameter.CommentFilter.ArticalNo.Split(" ");
                    foreach (var element in words) sb.Append("AND CommentCardPs.CardTitle LIKE '%" + element + "%'");

                    sb.Append(
                        "ORDER BY CommentCardPs.CardTitle " + ContractorParameter.CommentFilter.Sorter.Order);
                }
            }

        var queryContractors = @"SELECT
                                          CommentCardContractorPs.Id
                                         ,CommentCardContractorPs.CommentCardId
                                         ,CommentCardContractorPs.ContractorId
                                         ,CommentCardContractorPs.CreaterId
                                         ,CommentCardContractorPs.Assigner AS AssignerId
                                         ,CommentCardContractorPs.Reporter
                                         ,CommentCardContractorPs.PsSequenceId
                                         ,CommentLogPriority.PriorityId AS [Key]
                                         ,CommentLogPriority.Name AS Text
                                         ,CommentLogField.FieldId AS [Key]
                                         ,CommentLogField.Name AS Text
                                         ,CommentLogSeverity.SeverityId AS [Key]
                                         ,CommentLogSeverity.Name AS Text
                                         ,CommentLogStatus.StatusId AS [Key]
                                         ,CommentLogStatus.Name AS Text
                                         ,CommentChangeType.TypeId AS [Key]
                                         ,CommentChangeType.Name AS Text
                                        FROM dbo.CommentCardContractorPs
                                        LEFT OUTER JOIN dbo.CommentLogField
                                          ON CommentCardContractorPs.Field = CommentLogField.FieldId
                                        LEFT OUTER JOIN dbo.CommentLogPriority
                                          ON CommentCardContractorPs.Priority = CommentLogPriority.PriorityId
                                        LEFT OUTER JOIN dbo.CommentLogSeverity
                                          ON CommentCardContractorPs.Severity = CommentLogSeverity.SeverityId
                                        LEFT OUTER JOIN dbo.CommentLogStatus
                                          ON CommentCardContractorPs.Status = CommentLogStatus.StatusId
                                        LEFT OUTER JOIN dbo.CommentChangeType
                                          ON CommentCardContractorPs.ChangeType = CommentChangeType.TypeId
                                          WHERE CommentLogField.LanguageCode = @lang
                                          AND CommentLogPriority.LanguageCode = @lang
                                          AND CommentLogSeverity.LanguageCode = @lang
                                          AND CommentLogStatus.LanguageCode = @lang
                                          AND CommentChangeType.LanguageCode = @lang
                                           AND CommentCardContractorPs.Accept NOT IN ('1','2')";

        var queryComments = @"SELECT * FROM dbo.ContractorComment";

        var company = @"SELECT CabCompany.Name FROM dbo.CabCompany WHERE Id = @Id";
        var person =
            @"SELECT CabPerson.FullName,CabPerson.Image FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE Oid = @Id";
        var cabperson = @"SELECT CabPerson.FullName FROM dbo.CabPerson WHERE Id = @Id";

        IEnumerable<CommentCardDto> data = null;
        IEnumerable<ContractorsCommentCardDto> data1 = null;
        IEnumerable<CommentsDto> data2 = null;

        var param = new
        {
            lang = ContractorParameter.Lang, ContractorParameter.CommentFilter.LotId,
            ContractorId = mCabPersonCompany.CompanyId
        };

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            data = await connection.QueryAsync<CommentCardDto>(sb.ToString(), param);
            //data1 = await connection.QueryAsync<ContractorsCommentCardDto>(queryContractors);

            data1 = connection
                .Query<ContractorsCommentCardDto, CommentLogPriorityDto, CommentLogFieldDto, CommentLogSeverityDto,
                    CommentLogStatusDto, CommentChangeTypeDto, ContractorsCommentCardDto>(
                    queryContractors,
                    (contractorsCommentCardDto, commentLogPriorityDto, commentLogFieldDto, commentLogSeverityDto,
                        commentLogStatusDto, commentChangeTypeDto) =>
                    {
                        contractorsCommentCardDto.Priority = commentLogPriorityDto;
                        contractorsCommentCardDto.Field = commentLogFieldDto;
                        contractorsCommentCardDto.Severity = commentLogSeverityDto;
                        contractorsCommentCardDto.Status = commentLogStatusDto;
                        contractorsCommentCardDto.ChangeType = commentChangeTypeDto;
                        return contractorsCommentCardDto;
                    }, param,
                    splitOn: "Key, Key,Key,Key,Key");


            data2 = await connection.QueryAsync<CommentsDto>(queryComments);

            
        }

        // if (mCabPersonCompany.CompanyId != "b7533cc9-5098-4409-a2ab-dde75e47f435")
        // {
        //     data1 = data1.Where(a => a.ContractorId == mCabPersonCompany.CompanyId || a.CreaterId == mCabPersonCompany.CompanyId || a.ContractorId == "b7533cc9-5098-4409-a2ab-dde75e47f435");
        //
        // }
        //
        // data1 = data1.OrderBy(a => a.ContractorName);

        using (var connection =
               new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString))
        {
            foreach (var i in data)
            {
                var ContractorsCommentCardDto = new List<ContractorsCommentCardDto>();
                ContractorsCommentCardDto = data1.Where(a => a.CommentCardId == i.Id).ToList();

                i.Ps = ContractorsCommentCardDto.OrderByDescending(x => x.PsSequenceId).ToList();
                foreach (var r in i.Ps)
                {
                    CabCompany CabCompany;
                    CabPerson cabPerson;
                    CabCompany = connection.Query<CabCompany>(company, new { Id = r.ContractorId })
                        .FirstOrDefault();
                    if (r.AssignerId != null)
                    {
                        cabPerson = connection.Query<CabPerson>(cabperson, new { Id = r.AssignerId })
                            .FirstOrDefault();
                        r.Assigner = cabPerson.FullName;
                    }

                    if (r.Reporter != null)
                    {
                        cabPerson = connection.Query<CabPerson>(person, new { Id = r.Reporter })
                            .FirstOrDefault();
                        r.Reporter = cabPerson.FullName;
                    }

                    if (CabCompany != null) r.ContractorName = CabCompany.Name;

                    if (r.CreaterId != null)
                    {
                        CabCompany = connection.Query<CabCompany>(company, new { Id = r.CreaterId })
                            .FirstOrDefault();
                        r.Creater = CabCompany.Name;
                    }

                    var CommentsDto = new List<CommentsDto>();
                    CommentsDto = data2.Where(a => a.CommentCardContractorsId == r.Id).ToList();
                    r.Comments = CommentsDto;
                    foreach (var a in r.Comments)
                    {
                        CabPerson CabPerson;
                        CabPerson = connection.Query<CabPerson>(person, new { Id = a.PersonId })
                            .FirstOrDefault();
                        a.Name = CabPerson.FullName;
                    }

                    using (var dbConnection = new SqlConnection(connectionString))
                    {
                        r.PsOrderNumber = dbConnection
                            .Query<string>(
                                "Select PsOrderNumber From ContractorPs Where PsSequenceId = @PsSequenceId",
                                new { r.PsSequenceId })
                            .FirstOrDefault();
                    }
                }
            }
        }

        data = data.Where(a => a.Ps.Count != 0);
        return data;
    }

    public async Task<string> AddCommentForPs(ContractorParameter ContractorParameter,
        ISendGridRepositorie ISendGridRepositorie)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var cuconnectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            null, ContractorParameter.TenantProvider);

        var insert =
            @"MERGE INTO dbo.ContractorComment t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE SET PersonId = @PersonId ,MESSAGE = @Message ,CommentCardContractorsId = @CommentCardContractorsId,TimeStamp = @TimeStamp WHEN NOT MATCHED THEN INSERT (Id ,PersonId ,Message ,CommentCardContractorsId,TimeStamp) VALUES (@Id ,@PersonId ,@Message ,@CommentCardContractorsId,@TimeStamp);";
        var user =
            @"SELECT CommentCardContractorPs.Assigner AS Id ,ContractorHeader.SequenceId ,ContractorHeader.StandardMailId ,ContractorHeader.StartDate ,ContractorHeader.EndDate FROM dbo.CommentCardContractorPs LEFT OUTER JOIN dbo.ContractorPs ON ContractorPs.PsSequenceId = CommentCardContractorPs.PsSequenceId LEFT OUTER JOIN dbo.ContractorHeader ON ContractorHeader.SequenceId = ContractorPs.LotId WHERE CommentCardContractorPs.Id = @Id;";
        ContractorHeader mContractorHeader;
        var parm = new
        {
            Id = Guid.NewGuid().ToString(),
            PersonId = ContractorParameter.UserId,
            ContractorParameter.ContractorComment.Message,
            ContractorParameter.ContractorComment.CommentCardContractorsId,
            TimeStamp = GetTimestamp(DateTime.Now)
        };
        await using (var connection = new SqlConnection(connectionString))
        {
            await connection.ExecuteAsync(insert, parm);
            mContractorHeader = connection
                .Query<ContractorHeader>(user,
                    new { Id = ContractorParameter.ContractorComment.CommentCardContractorsId }).FirstOrDefault();
        }

        StandardMailHeader mStandardMailHeader;
        if (mContractorHeader?.StandardMailId != null)
            await using (var cuconnection = new SqlConnection(cuconnectionString))
            {
                mStandardMailHeader = cuconnection
                    .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id",
                        new { Id = mContractorHeader.StandardMailId }).FirstOrDefault();
            }

        else
            await using (var cuconnection = new SqlConnection(cuconnectionString))
            {
                mStandardMailHeader = cuconnection
                    .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader where IsDefault = 1")
                    .FirstOrDefault();
            }

        bool issend;
        var sendGridParameter = new SendGridParameter();
        sendGridParameter.Id = mContractorHeader.Id;
        sendGridParameter.TenantProvider = ContractorParameter.TenantProvider;
        sendGridParameter.ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId;
        sendGridParameter.ProjectSequenceId = ContractorParameter.ProjectSequenceId;
        sendGridParameter.Lot = mContractorHeader.SequenceId;
        sendGridParameter.Lang = ContractorParameter.Lang;
        sendGridParameter.MailBody = mStandardMailHeader.OutStandingComments;
        sendGridParameter.LotTitle = mContractorHeader.Title;
        sendGridParameter.StartDate = mContractorHeader.StartDate.ToString();
        sendGridParameter.EndDate = mContractorHeader.EndDate.ToString();
        sendGridParameter.Subject = mContractorHeader.Title + " " + "Out Standing Comments";
        sendGridParameter.Configuration = ContractorParameter.Configuration;
        if (ContractorParameter.Lang == "en")
            sendGridParameter.TemplateId = ContractorParameter.Configuration.GetValue<string>("TemplateId_en1");
        if (ContractorParameter.Lang == "nl")
            sendGridParameter.TemplateId = ContractorParameter.Configuration.GetValue<string>("TemplateId_nl1");
        issend = await ContractorParameter.SendGridRepositorie.SendInvitation(sendGridParameter);

        return ContractorParameter.ContractorComment.Id;
    }

    public async Task<string> UpdateCommentLogDropDownForPs(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var update =
                "UPDATE dbo.CommentCardContractorPs SET Field = @Field ,Priority = @Priority ,Severity = @Severity ,Status = @Status,Assigner = @Assigner,ChangeType = @ChangeType WHERE Id = @Id;";

            var param = new
            {
                ContractorParameter.CommentCardContractorDto.Id,
                ContractorParameter.CommentCardContractorDto.Priority,
                ContractorParameter.CommentCardContractorDto.Severity,
                ContractorParameter.CommentCardContractorDto.Status,
                ContractorParameter.CommentCardContractorDto.Field,
                ContractorParameter.CommentCardContractorDto.Assigner,
                ContractorParameter.CommentCardContractorDto.ChangeType
            };
            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(update, param);
            }


            return ContractorParameter.CommentCardContractorDto.Id;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> ApproveCommentForPs(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var update = "UPDATE dbo.CommentCardContractorPs SET Accept = @Accept WHERE Id = @Id";

            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(update,
                    new { ContractorParameter.AcceptComment.Id, ContractorParameter.AcceptComment.Accept });
            }

            return "Ok";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<GetContractorPsOrderNumber>> ContractorPsOrderNumberDropDown(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        IEnumerable<GetContractorPsOrderNumber> results = null;

        results = connection
            .Query<GetContractorPsOrderNumber>(
                @"SELECT Name + 0 AS [Text], Name AS [Key] FROM ContractorPsOrderNumber ORDER BY Text ASC");

        foreach (var item in results)
        {
            item.IsExist = connection
                .Query<string>(
                    @"Select Id From ContractorPs Where LotId = @LotId And PsOrderNumber = @PsOrderNumber",
                    new { LotId = ContractorParameter.Id, PsOrderNumber = item.Key }).Any();

            item.IsPublished = connection
                .Query<string>(
                    @"Select Id From ContractorPsPublished Where LotId = @LotId And PsOrderNumber = @PsOrderNumber",
                    new { LotId = ContractorParameter.Id, PsOrderNumber = item.Key }).Any();
        }

        return results;
    }

    public async Task<string> PublishContractorPs(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var errorList = await ContractorPsErrorLogForZeroState(ContractorParameter);
        var errorPs = errorList.DistinctBy(x => x.PsSequenceId).Select(v => v.PsSequenceId).ToList();

        var param = new
        {
            LotId = ContractorParameter.Id, errorPs
        };
        await connection.ExecuteAsync(
            "Delete From ContractorPsPublished Where LotId = @LotId AND PsSequenceId NOT IN @errorPs", param);

        var psData =
            connection.Query<ContractorPs>(
                "Select * From ContractorPs Where LotId = @LotId AND PsSequenceId NOT IN @errorPs",
                param).ToList();

        var insertQuery = @"INSERT INTO dbo.ContractorPsPublished
                                    (
                                      Id
                                     ,ArticleNumber
                                     ,Title
                                     ,MeasurementCode
                                     ,QuantityQuotation
                                     ,UnitPrice
                                     ,QuantityConsumed
                                     ,Total
                                     ,LotId
                                     ,CompanyId
                                     ,PsSequenceId
                                     ,PsOrderNumber
                                     ,IsApproved
                                     ,ApprovedDate
                                    )
                                    VALUES
                                    (
                                     @Id
                                     ,@ArticleNumber
                                     ,@Title
                                     ,@MeasurementCode
                                     ,@QuantityQuotation
                                     ,@UnitPrice
                                     ,@QuantityConsumed
                                     ,@Total
                                     ,@LotId
                                     ,@CompanyId
                                     ,@PsSequenceId
                                     ,@PsOrderNumber
                                     ,@IsApproved
                                     ,@ApprovedDate
                                    )";
        foreach (var item in psData) await connection.ExecuteAsync(insertQuery, item);


        return ContractorParameter.Id;
    }

    public async Task<string> ApproveContractorPs(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var updateSql1 =
            "UPDATE ContractorPs SET IsApproved  = @IsApproved , ApprovedDate = @ApprovedDate WHERE  LotId = @LotId AND PsSequenceId = @PsSequenceId";

        var updateSql2 =
            "UPDATE ContractorPsPublished SET IsApproved  = @IsApproved , ApprovedDate = @ApprovedDate WHERE  LotId = @LotId AND PsSequenceId = @PsSequenceId";

        var param = new
        {
            IsApproved = true,
            LotId = ContractorParameter.lotId,
            PsSequenceId = ContractorParameter.psSequenceId,
            ApprovedDate = DateTime.UtcNow
        };

        await connection.ExecuteAsync(updateSql1, param);
        await connection.ExecuteAsync(updateSql2, param);


        return ContractorParameter.psSequenceId;
    }

    public async Task<string> ContractorPsMinusPlusWork(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var isExist = connection.Query<ContractorPsMinusPlusWork>(
            "Select * from ContractorPsMinusPlusWork Where LotId = @LotId AND PsOrderNumber = @PsOrderNumber",
            new
            {
                ContractorParameter.MinusPlusWorkDto.PsOrderNumber, ContractorParameter.MinusPlusWorkDto.LotId
            }).FirstOrDefault();
        string version = null;
        version = isExist != null ? (isExist.Version.ToInt() + 1).ToString() : "1";
        var client = new FileClient();
        
        var url = client.PersistPhotoInNewFolder(ContractorParameter.File.FileName, ContractorParameter.TenantProvider
            , ContractorParameter.File, "PsMinusPlusWork/" + ContractorParameter.MinusPlusWorkDto.LotId);
        

        var insertQuery = @"INSERT INTO dbo.ContractorPsMinusPlusWork
                                    (
                                      Id
                                     ,LotId
                                     ,PsOrderNumber
                                     ,Status
                                     ,TotalPrice
                                     ,Url
                                     ,Version
                                     ,IsApproved
                                    )
                                    VALUES
                                    (
                                    @Id
                                     ,@LotId
                                     ,@PsOrderNumber
                                     ,@Status
                                     ,@TotalPrice
                                     ,@Url
                                     ,@Version
                                     ,0
                                    )";


        if (isExist != null)
        {
            var updateQuery =
                @"Update ContractorPsMinusPlusWork Set Url = @Url , Version = @Version , TotalPrice = @TotalPrice Where Id = @Id";
            var param = new ContractorPsMinusPlusWork
            {
                Id = isExist.Id,
                TotalPrice = ContractorParameter.MinusPlusWorkDto.TotalPrice,
                Url =  url,
                Version = (isExist.Version.ToInt() + 1).ToString()
            };

            await connection.ExecuteAsync(updateQuery, param);
        }
        else
        {
            var param = new ContractorPsMinusPlusWork
            {
                Id = Guid.NewGuid().ToString(),
                LotId = ContractorParameter.MinusPlusWorkDto.LotId,
                PsOrderNumber = ContractorParameter.MinusPlusWorkDto.PsOrderNumber,
                Status = ContractorParameter.MinusPlusWorkDto.Status,
                TotalPrice = ContractorParameter.MinusPlusWorkDto.TotalPrice,
                Url =  url,
                Version = "1"
            };

            await connection.ExecuteAsync(insertQuery, param);
        }
        
        var urlSharepoint = SharePointUpload(ContractorParameter, ContractorParameter.MinusPlusWorkDto.TotalPrice,
            ContractorParameter.MinusPlusWorkDto.PsOrderNumber.ToString(), version);

        var nn = await urlSharepoint;
        return  url;
    }

    public async Task<List<GetContractorPsMinusPlusWork>> GetContractorPsMinusPlusWork(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var data = connection
            .Query<GetContractorPsMinusPlusWork>(
                "Select * from ContractorPsMinusPlusWork Where LotId = @LotId Order By PsOrderNumber ASC",
                new { LotId = ContractorParameter.Id }).ToList();


        return data;
    }

    public async Task<List<GetContractorPsMinusPlusWork>> UpdateContractorPsMinusPlusWorkTotalPrice(
        ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var updateQuery =
            @"Update ContractorPsMinusPlusWork Set  TotalPrice = @TotalPrice Where Id = @Id";

        foreach (var item in ContractorParameter.GetContractorPsMinusPlusWork)
            await connection.ExecuteAsync(updateQuery, new { item.TotalPrice, item.Id });


        return ContractorParameter.GetContractorPsMinusPlusWork;
    }

    public async Task<string> ApproveContractorPsMinusPlusWork(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        await connection
            .ExecuteAsync("Update ContractorPsMinusPlusWork Set  IsApproved = 1, Date = @date Where Id = @Id",
                new { ContractorParameter.Id, date = DateTime.UtcNow });


        return ContractorParameter.Id;
    }

    public async Task<string> ZipDownload(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var tenetConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);


            var companyId = tenetConnection.Query<string>("Select CompanyId From CabPersonCompany Where Oid = @Oid",
                new { Oid = ContractorParameter.UserId }).FirstOrDefault();
            var companyName = tenetConnection.Query<string>("SELECT Name FROM CabCompany WHERE Id = @Id",
                new { Id = companyId }).FirstOrDefault();

//             var uploadedItem = await ContractorParameter.GraphServiceClient
//                 .Drive
//                 .Root
// //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
//                 .ItemWithPath("nn")
//                 .Content
//                 .Request()
//                 .GetAsync();
//.PutAsync<DriveItem>(ContractorParameter.File.OpenReadStream());


            var type = "view";

            var password = "ThisIsMyPrivatePassword";

            var scope = "anonymous";

//             var result = await ContractorParameter.GraphServiceClient
//                 .Drive
//                 .Root
// //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
//                 .ItemWithPath(ContractorParameter.ContractorLotUploadedDocs.LotId + "/" + companyName + "/" +
//                               ContractorParameter.ContractorLotUploadedDocs.Type)
//                 .CreateLink(type, scope)
//                 .Request()
//                 .PostAsync();


            return "uploadedItem.WebUrl";
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> LotAddReminderDates(ContractorParameter ContractorParameter, string Id)
    {
        try
        {
            if (ContractorParameter.BMLotHeaderDto.StartDate != null &&
                ContractorParameter.BMLotHeaderDto.EndDate != null)
            {
                var connectionString = ConnectionString.MapConnectionString(
                    ContractorParameter.ContractingUnitSequenceId,
                    ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
                var cuconnectionString = ConnectionString.MapConnectionString(
                    ContractorParameter.ContractingUnitSequenceId,
                    null, ContractorParameter.TenantProvider);
                StandardMailHeader standardMailHeader;
                if (ContractorParameter.BMLotHeaderDto.StandardMailId != null)
                {
                    var standardMailquery = @"SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id;";

                    using (var connection = new SqlConnection(cuconnectionString))
                    {
                        standardMailHeader = connection.Query<StandardMailHeader>(standardMailquery,
                            new { Id = ContractorParameter.BMLotHeaderDto.StandardMailId }).FirstOrDefault();
                    }
                }
                else
                {
                    var standardMailquery = @"SELECT * FROM dbo.StandardMailHeader;";

                    using (var connection = new SqlConnection(cuconnectionString))
                    {
                        standardMailHeader = connection.Query<StandardMailHeader>(standardMailquery).FirstOrDefault();
                    }
                }

                if (standardMailHeader != null)
                {
                    var r1 = 0;
                    var r2 = 0;
                    var r3 = 0;

                    if (ContractorParameter.BMLotHeaderDto.StartDate != null &&
                        ContractorParameter.BMLotHeaderDto.EndDate != null)
                    {
                        var datedifferent = (ContractorParameter.BMLotHeaderDto.EndDate.Value.Date -
                                             ContractorParameter.BMLotHeaderDto.StartDate.Value.Date).Days;

                        if (standardMailHeader.Reminder1TimeFrameTender != null)
                            r1 = datedifferent * Convert.ToInt32(
                                standardMailHeader.Reminder1TimeFrameTender.Substring(0,
                                    standardMailHeader.Reminder1TimeFrameTender.IndexOf('.') > 0
                                        ? standardMailHeader.Reminder1TimeFrameTender.IndexOf('.')
                                        : standardMailHeader.Reminder1TimeFrameTender.Length)) / 100;

                        if (standardMailHeader.Reminder2TimeFrameTender != null)
                            r2 = datedifferent * Convert.ToInt32(
                                standardMailHeader.Reminder2TimeFrameTender.Substring(0,
                                    standardMailHeader.Reminder2TimeFrameTender.IndexOf('.') > 0
                                        ? standardMailHeader.Reminder2TimeFrameTender.IndexOf('.')
                                        : standardMailHeader.Reminder2TimeFrameTender.Length)) / 100;

                        if (standardMailHeader.Reminder3TimeFrameTender != null)
                            r3 = datedifferent * Convert.ToInt32(
                                standardMailHeader.Reminder3TimeFrameTender.Substring(0,
                                    standardMailHeader.Reminder3TimeFrameTender.IndexOf('.') > 0
                                        ? standardMailHeader.Reminder3TimeFrameTender.IndexOf('.')
                                        : standardMailHeader.Reminder3TimeFrameTender.Length)) / 100;

                        var r4 = datedifferent * 80 / 100;

                        var update =
                            @"UPDATE dbo.ContractorHeader SET ReminderFourDate = @ReminderFourDate ,ReminderOneDate = @ReminderOneDate ,ReminderThreeDate = @ReminderThreeDate ,ReminderTwoDate = @ReminderTwoDate WHERE Id = @Id ;";

                        var param = new
                        {
                            Id,
                            ReminderOneDate = ContractorParameter.BMLotHeaderDto.StartDate.Value.AddDays(r1),
                            ReminderTwoDate = ContractorParameter.BMLotHeaderDto.StartDate.Value.AddDays(r2),
                            ReminderThreeDate = ContractorParameter.BMLotHeaderDto.StartDate.Value.AddDays(r3),
                            ReminderFourDate = ContractorParameter.BMLotHeaderDto.StartDate.Value.AddDays(r4)
                        };
                        using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.ExecuteAsync(update, param);
                        }
                    }
                }

                return "ok";
            }

            {
                return null;
            }
        }


        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }

    public async Task<string> CreateContractorWorkflow(ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options, ContractorParameter.TenantProvider);
            var check =
                @"SELECT [Id] FROM [dbo].[ConstructorWorkFlow] WHERE Lot = @Lot and Contractor = @Contractor";

            ConstructorWorkFlow data;

            var query =
                @"INSERT INTO [dbo].[ConstructorWorkFlow] ([Id] ,[SequenceId],[Division] ,[Lot] ,[Contractor] ,[Price] ,[CreatedDateTime],[CreatedBy],[StatusChangeDate],Status,CabCompanyId,Title,TypeId) VALUES (@Id ,@SequenceId,@Division ,@Lot ,@Contractor ,@Price,@CreatedDateTime,@CreatedBy,@StatusChangeDate,@Status,@CabCompanyId,@Title,@TypeId)";
            string SequenceId = null;

            var parm = new
            {
                ContractorParameter.ConstructorLotInfoDto.Lot,
                ContractorParameter.ConstructorLotInfoDto.Contractor
            };

            using (var connection = new SqlConnection(connectionString))
            {
                data = connection.Query<ConstructorWorkFlow>(check, parm).FirstOrDefault();

                if (data == null)
                {
                    var idGenerator = new IdGenerator();
                    SequenceId = idGenerator.GenerateId(applicationDbContext, "CW-", "CWSequence");

                    var parm1 = new
                    {
                        Id = Guid.NewGuid().ToString(),
                        SequenceId,
                        ContractorParameter.ConstructorLotInfoDto.Division,
                        ContractorParameter.ConstructorLotInfoDto.Lot,
                        ContractorParameter.ConstructorLotInfoDto.Contractor,
                        ContractorParameter.ConstructorLotInfoDto.Price,
                        CreatedDateTime = DateTime.UtcNow,
                        CreatedBy = ContractorParameter.UserId,
                        Status = "4010e768-3e06-added-b337-Lota82addb", //added to contract
                        StatusChangeDate = DateTime.UtcNow,
                        CabCompanyId = ContractorParameter.ConstructorLotInfoDto.CompanyId,
                        Title = SequenceId + " " + ContractorParameter.ConstructorLotInfoDto.Contractor,
                        TypeId = "2210e768-msms-Item-Lot2-ee367a82ad22"
                    };

                    await connection.ExecuteAsync(query, parm1);

                    await ConstructorWfStatusUpdate("4010e768-3e06-added-b337-Lota82addb", parm1.Id, connectionString);
                }
            }

            return parm.Lot;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> SharePointUpload(ContractorParameter ContractorParameter, string total, string psNumber,
        string version)
    {
        string lotName = null;
        var connectionString = ConnectionString.MapConnectionString(
            ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);

        var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);

        var values = dbConnection.Query<FieldValues>(
            "Select ProjectDefinition.Title As ProjectTitle, CabCompany.Name As CompanyName From ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id   Where ProjectDefinition.SequenceCode = @SequenceCode",
            new { SequenceCode = ContractorParameter.ProjectSequenceId }).FirstOrDefault();

        using (var connection = new SqlConnection(connectionString))
        {
            lotName = connection.Query<string>("SELECT Title FROM ContractorHeader WHERE SequenceId = @Id",
                new { ContractorParameter.Id }).FirstOrDefault();
        }
        
        // var scopes = new[] { "https://graph.microsoft.com/.default" };
        // var tenantId = "3d438826-fdde-4b8b-89d1-1b9b4feeaa20";
        // var clientId = "f9ec3629-065f-4065-9dee-f42c22ae74e5";
        // var clientSecret = "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd";
        // var clientSecretCredential = new ClientSecretCredential(
        //     tenantId, clientId, clientSecret);
        // var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
        // var driveResponse1 = await graphClient.Sites["uprince.sharepoint.com"].Drive.GetAsync();
        //
        // var token =  UploadDocumentGetToken();
        //
        // string url = null;
        // using (HttpClient client = new HttpClient())
        // {
        //     client.DefaultRequestHeaders.Authorization =
        //         new AuthenticationHeaderValue("Bearer", token.Result.AccessToken);
        //     
        //     var jsonBody = $"{{\"name\": \"{ContractorParameter.Id + "/" + ContractorParameter.File.FileName}\", \"folder\": {{}}, \"@microsoft.graph.conflictBehavior\": \"rename\"}}";
        //
        //     var folderCreateContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        //
        //     var createFolderResponse = await client.PostAsync($"https://graph.microsoft.com/v1.0/drives/{driveResponse1?.Id}/root:/General:/children", folderCreateContent);
        //
        //     if (createFolderResponse.IsSuccessStatusCode)
        //     {
        //         var requestUrl = $"https://graph.microsoft.com/v1.0/drives/{driveResponse1?.Id}/root:/General/{ContractorParameter.Id + "/" + ContractorParameter.File.FileName}/{ContractorParameter.File.FileName}:/content";
        //
        //         await using (var stream = ContractorParameter.File.OpenReadStream())
        //         {
        //             var content = new StreamContent(stream);
        //             content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //
        //             var fileUploadResponse = await client.PutAsync(requestUrl, content);
        //
        //             if (fileUploadResponse.IsSuccessStatusCode)
        //             {
        //                 var fileUploadContent = await fileUploadResponse.Content.ReadAsStringAsync();
        //                 var dto = JsonConvert.DeserializeObject<FileUploadResponse>(fileUploadContent);
        //                 url = dto.WebUrl;
        //             }
        //         }
        //     }
        // }
        
       // var rootResponse = await graphClient.Drives[ driveResponse1?.Id].Root.GetAsync();

        // var uploadedItemx = await ContractorParameter.GraphServiceClient
        //     .Drives[ driveResponse1?.Id].Root
        //     //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
        //     .ItemWithPath(ContractorParameter.Id + "/" + ContractorParameter.File.FileName)
        //     .Content
        //     .PutAsync(ContractorParameter.File.OpenReadStream());
        //
         // if (ContractorParameter.ProjectSequenceId == null) ContractorParameter.ProjectSequenceId = "project";
         // if (values.ProjectTitle == null) values.ProjectTitle = "projectTitle";
         // if (values.CompanyName == null) values.CompanyName = "bm";
         // if (ContractorParameter.ContractingUnitSequenceId == null) ContractorParameter.ContractingUnitSequenceId = "cu";
         // if (lotName == null) lotName = "lot";
         // if (total == null) total = "0";
         // if (psNumber == null) psNumber = "0";
         // if (version == null) version = "0";
         //
         // var fieldValueSet = new FieldValueSet
         // {
         //     AdditionalData = new Dictionary<string, object>
         //     {
         //         { "Title", ContractorParameter.File.FileName },
         //         { "Project", ContractorParameter.ProjectSequenceId },
         //         { "Project_x0020_Title", values.ProjectTitle },
         //         { "CuTitle", ContractorParameter.ContractingUnitSequenceId },
         //         { "Company", values.CompanyName },
         //         { "Lot_x0020_Title", lotName },
         //         { "_DCDateCreated", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) },
         //         { "Amount", total },
         //         { "_Version", version },
         //         { "Contracting_x0020_Unit_x0020_Title", ContractorParameter.ContractingUnitSequenceId },
         //         { "Progress_x0020_Statement_x0020_Title", psNumber }
         //     }
         // };


        //  var uploadedItem = await ContractorParameter.GraphServiceClient.Drive.Root
        //      .ItemWithPath(ContractorParameter.Id + "/" + ContractorParameter.File.FileName).ListItem.Fields
        //      .Request()
        //      .UpdateAsync(fieldValueSet);
        //
        // var uploadedItem2 =    ContractorParameter.GraphServiceClient.Drive.Root.ItemWithPath(ContractorParameter.Id + "/"+ContractorParameter.File.FileName).ListItem.Fields
        //     .Request()
        //     .UpdateAsync(fieldValueSet);
        
        var drives = await ContractorParameter.GraphServiceClient.Drives.GetAsync();
        var driveId = drives.Value.FirstOrDefault(x => x.Name == "Documents").Id;
        var uploadedItemx = await ContractorParameter.GraphServiceClient
            .Drives[driveId]
            .Root
            //.ItemWithPath("lot/"+ContractorParameter.File.FileName)
            .ItemWithPath(ContractorParameter.Id + "/"+ContractorParameter.File.FileName)
            .Content
            .PutAsync(ContractorParameter.File.OpenReadStream());


         return uploadedItemx.WebUrl;

    }

    public static string GetTimestamp(DateTime value)
    {
        var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        var secondsSinceEpoch = (int)t.TotalSeconds;
        return secondsSinceEpoch.ToString();
    }

    public async Task<string> SendMeasuringStateReceivedMail(ConstructorWorkFlow constructorWorkFlow,
        ContractorParameter ContractorParameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
            var cuconnectionString = ConnectionString.MapConnectionString(
                ContractorParameter.ContractingUnitSequenceId,
                null, ContractorParameter.TenantProvider);

            StandardMailHeader mStandardMailHeader;

            var person =
                @"SELECT * FROM dbo.ContractorTeamList WHERE LotContractorId = @LotContractorId AND CompanyId = @CompanyId";

            await using (var connection = new SqlConnection(connectionString))
            {
                var sMailId =
                    @"SELECT ContractorHeader.StandardMailId,ContractorHeader.Title,ContractorHeader.SequenceId,ContractorHeader.StartDate,ContractorHeader.EndDate,ContractorHeader.Name FROM dbo.ContractorTeamList LEFT OUTER JOIN dbo.ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorHeader.Id = @Id";

                ContractorHeader mContractorHeader;
                mContractorHeader = connection.Query<ContractorHeader>(sMailId,
                    new
                    {
                        Id = constructorWorkFlow.Lot
                    }).FirstOrDefault();
                if (mContractorHeader?.StandardMailId != null)
                    await using (var cuconnection = new SqlConnection(cuconnectionString))
                    {
                        mStandardMailHeader = cuconnection
                            .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader WHERE Id = @Id",
                                new { Id = mContractorHeader.StandardMailId }).FirstOrDefault();
                    }

                else
                    await using (var cuconnection = new SqlConnection(cuconnectionString))
                    {
                        mStandardMailHeader = cuconnection
                            .Query<StandardMailHeader>("SELECT * FROM dbo.StandardMailHeader where IsDefault = 1")
                            .FirstOrDefault();
                    }

                ContractorTeamList mContractorTeamList;
                mContractorTeamList = connection.Query<ContractorTeamList>(person,
                    new
                    {
                        LotContractorId = constructorWorkFlow.Lot, CompanyId = constructorWorkFlow.CabCompanyId
                    }).FirstOrDefault();

                bool issend;
                var sendGridParameter = new SendGridParameter();
                sendGridParameter.Id = mContractorTeamList.CabPersonId;
                sendGridParameter.TenantProvider = ContractorParameter.TenantProvider;
                sendGridParameter.ContractingUnitSequenceId = ContractorParameter.ContractingUnitSequenceId;
                sendGridParameter.ProjectSequenceId = ContractorParameter.ProjectSequenceId;
                sendGridParameter.Lot = mContractorHeader.SequenceId;
                sendGridParameter.Lang = ContractorParameter.Lang;
                sendGridParameter.MailBody = mStandardMailHeader?.MeasuringStateRecieved;
                sendGridParameter.LotTitle = mContractorHeader.Name;
                sendGridParameter.StartDate = mContractorHeader.StartDate?.ToString("dd/MM/yyyy");
                sendGridParameter.EndDate = mContractorHeader.EndDate?.ToString("dd/MM/yyyy");
                sendGridParameter.Subject = mContractorHeader.Title + " " + "Measuring State Recieved";
                sendGridParameter.Configuration = ContractorParameter.Configuration;
                
                var projLang = connection.Query<string>(
                    "SELECT pl.Name FROM ProjectDefinition LEFT OUTER JOIN ProjectLanguage pl ON ProjectDefinition.Language = pl.TypeId WHERE ProjectDefinition.SequenceCode = @Id",
                    new { Id = ContractorParameter.ProjectSequenceId }).FirstOrDefault();

                sendGridParameter.TemplateId = projLang switch
                {
                    "en" or null =>
                        "d-042f989489614e5ea392df549f29d620",
                    "nl" =>  "d-b33b8f4a62e443ebb8b1d884b77e89ad",
                    _ => sendGridParameter.TemplateId
                };
                // if (ContractorParameter.Lang == "en")
                //     sendGridParameter.TemplateId = "d-042f989489614e5ea392df549f29d620";
                // if (ContractorParameter.Lang == "nl")
                //     sendGridParameter.TemplateId = "d-b33b8f4a62e443ebb8b1d884b77e89ad";
                issend = await ContractorParameter.SendGridRepositorie.SendInvitation(sendGridParameter);
                if (issend)
                    await connection.ExecuteAsync(
                        "UPDATE dbo.ConstructorWorkFlow SET MeasuringStateReceived = 1 WHERE Id = @Id;",
                        new { constructorWorkFlow.Id });
            }

            return constructorWorkFlow.Id;
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
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

        var query = @"UPDATE dbo.ConstructorWorkFlow SET Status =@StatusId WHERE Id = @wfId ;";
        var timetableup =
            @"INSERT INTO dbo.ConstructorWfStatusChangeTime ( Id ,ConstructorWf ,StatusId ,DateTime ) VALUES ( @Id ,@wfId ,@StatusId ,@DateTime );";

        var param = new
        {
            StatusId,
            wfId = Id,
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
    
    public async Task<List<ContractorTeamList>> GetContractorListByLotId(ContractorParameter contractorParameter)
    {
        
            var connectionString = ConnectionString.MapConnectionString(
                contractorParameter.ContractingUnitSequenceId,
                contractorParameter.ProjectSequenceId, contractorParameter.TenantProvider);
            
            await using var connection = new SqlConnection(connectionString);

            var query =
                @"SELECT ContractorTeamList.Id ,ContractorTeamList.Name ,ContractorTeamList.Company ,ContractorTeamList.RoleId ,ContractorTeamList.InvitationMail ,ContractorTeamList.CabPersonId ,ContractorTeamList.CabPersonName ,ContractorTeamList.RoleName ,ContractorTeamList.LotContractorId,ContractorTeamList.CompanyId,ContractorTeamList.IsManual,ContractorTeamList.Approve,ContractorTeamList.InvitationId,ContractorTeamList.IsDownloded,ContractorTeamList.IsSubscribed,ContractorTeamList.InvitationDateTime FROM dbo.ContractorTeamList LEFT OUTER JOIN ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorHeader.SequenceId = @Id";

            var data = connection.Query<ContractorTeamList>(query, new { Id = contractorParameter.Id }).ToList();

            var pdfData = connection.Query<ContractorPdfData>(
                @"Select * From ContractorPdfData Where LotId = @LotId",
                new { LotId = contractorParameter.Id}).ToList(); 
            
            foreach (var item in data)
            {
                item.IsUploaded = pdfData.Exists(x => x.CompanyId == item.CompanyId);
                item.FileType = connection
                    .Query<string>(
                        @"SELECT FileType FROM ContractorUploadedFiles WHERE LotId = @LotId AND CompanyId = @CompanyId",
                        new { CompanyId = item.CompanyId, LotId = contractorParameter.Id }).FirstOrDefault();
            }

            return data;
        
    }
    
    public async Task<List<GetTotalPriceErrorsDto>> GetContractorTotalPriceErrorsByLotId(ContractorParameter contractorParameter)
    {
        
        var connectionString = ConnectionString.MapConnectionString(
            contractorParameter.ContractingUnitSequenceId,
            contractorParameter.ProjectSequenceId, contractorParameter.TenantProvider);
            
        await using var connection = new SqlConnection(connectionString);

        var query =
            @"SELECT ContractorTeamList.Id ,ContractorTeamList.Name ,ContractorTeamList.Company ,ContractorTeamList.RoleId ,ContractorTeamList.InvitationMail ,ContractorTeamList.CabPersonId ,ContractorTeamList.CabPersonName ,ContractorTeamList.RoleName ,ContractorTeamList.LotContractorId,ContractorTeamList.CompanyId,ContractorTeamList.IsManual,ContractorTeamList.Approve,ContractorTeamList.InvitationId,ContractorTeamList.IsDownloded,ContractorTeamList.IsSubscribed,ContractorTeamList.InvitationDateTime FROM dbo.ContractorTeamList LEFT OUTER JOIN ContractorHeader ON ContractorTeamList.LotContractorId = ContractorHeader.Id WHERE ContractorHeader.SequenceId = @Id";

        var data = connection.Query<GetTotalPriceErrorsDto>(query, new { Id = contractorParameter.Id }).ToList();

        var pdfData = connection.Query<ContractorPdfData>(
            @"Select * From ContractorPdfData Where LotId = @LotId",
            new { LotId = contractorParameter.Id}).ToList();

        data = new List<GetTotalPriceErrorsDto>();

        var item = new GetTotalPriceErrorsDto()
        {
            Errors = new List<TotalPriceErrors>() { new TotalPriceErrors() }
        };
        
        data.Add(item);
        
        return data;
        
    }

    public async Task<List<IsUnresolvedCommentDto>> IsUnresolvedComment(ContractorParameter ContractorParameter)
    {
        await using var dbConnection = new SqlConnection(ContractorParameter.TenantProvider.GetTenant().ConnectionString);

        var company = dbConnection.Query<string>(@"SELECT CompanyId FROM dbo.CabPersonCompany WHERE Oid = @UserId", new {ContractorParameter.UserId }).FirstOrDefault();
        
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        var result = connection
            .Query<IsUnresolvedCommentDto>(@"SELECT ArticleNo FROM dbo.CBCExcelLotData WHERE ContractId = @Id",
                new { ContractorParameter.Id }).ToList();

        string dataSql = @"SELECT DISTINCT
                          CBCExcelLotData.ArticleNo
                         ,CommentCardContractor.ContractorId
                         ,CommentCardContractor.Accept
                        FROM dbo.CBCExcelLotData
                        LEFT OUTER JOIN dbo.CommentCard
                          ON CBCExcelLotData.ArticleNo = CommentCard.ArticleNo
                        LEFT OUTER JOIN dbo.CommentCardContractor
                          ON CommentCard.Id = CommentCardContractor.CommentCardId
                          WHERE CommentCardContractor.Accept IN ('0','1','2') AND CBCExcelLotData.ContractId = @Id AND CommentCard.LotId = @Id";

        var data = connection.Query<IsUnresolvedCommentData>(dataSql, new { ContractorParameter.Id }).ToList();

        foreach (var i in result)
        {
            var isUnresolved = data.Where(e => e.ArticleNo == i.ArticleNo).ToList();

            foreach (var n in isUnresolved)
            {
                if (n.ContractorId == company || company == "e0386eac-c9a0-4f93-8baf-d24948bedda9")
                {
                    if (n.Accept == "0")
                    {
                        i.IsUnresolvedComment = "1";
                        
                        break;
                    }

                    if (n.Accept is "1" or "2")
                    {
                        i.IsUnresolvedComment = "2";
                    }
                }
            }
        }
        
        return result;
    }

    public async Task<List<ContractorStatusDto>> GetAwardedLotInProject(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        const string sql = "GetAwardedLotInProject";
          // var sql =  "SELECT ch.Id AS [Key],LotId AS Text FROM  ContractorTotalValuesPublished LEFT OUTER JOIN ContractorHeader ch ON ContractorTotalValuesPublished.LotId = ch.SequenceId WHERE IsWinner = 1";
        
        var result =await connection.QueryAsync<ContractorStatusDto>(sql,commandType:CommandType.StoredProcedure);

        return result.ToList();

    }
    
    public async Task<List<AwardedLotDataResult>> GetAwardedContractorLotData(ContractorParameter ContractorParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(ContractorParameter.ContractingUnitSequenceId,
            ContractorParameter.ProjectSequenceId, ContractorParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);

        var sql =
            @"WITH ret
            AS (SELECT pcpd.id,pcpd.ArticleNo,pcpd.CompanyId,Concat(pcpd.ArticleNo,' - ',pcpd.Title) AS Title,pcpd.Unit,pcpd.LotId,pcpd.Quantity FROM  ContractorTotalValuesPublished LEFT OUTER JOIN PublishedContractorsPdfData pcpd ON ContractorTotalValuesPublished.LotId = pcpd.LotId AND ContractorTotalValuesPublished.CompanyId = pcpd.CompanyId WHERE ContractorTotalValuesPublished.IsWinner =1  AND ContractorTotalValuesPublished.LotId = @LotId)
            SELECT * FROM ret ";

        var sb = new StringBuilder(sql);

        if (ContractorParameter.AwardedLotDataDto.Title != null)
        {
            ContractorParameter.AwardedLotDataDto.Title =
                ContractorParameter.AwardedLotDataDto.Title.Replace("'", "''");

            sb.Append(" WHERE Title LIKE '%" + ContractorParameter.AwardedLotDataDto.Title + "%'");

        }

        sb.Append(" Order By ArticleNo ASC");
        
        var result = connection.Query<AwardedLotDataResult>(sb.ToString(),new{LotId = ContractorParameter.AwardedLotDataDto.LotId}).ToList();

        return result;

    }
    
    private async Task<TokenRequest> UploadDocumentGetToken()
    {
        var _httpClient = new HttpClient();
        var requestURL = "https://login.microsoftonline.com/3d438826-fdde-4b8b-89d1-1b9b4feeaa20/oauth2/v2.0/token?";
        var content = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "client_id", "f9ec3629-065f-4065-9dee-f42c22ae74e5" },
            { "client_secret", "zDo8Q~qJEhJmLFbg-jhF8IDsbDispsdJ8J_bMbPd" },
            { "grant_type", "client_credentials" },
            { "scope", "https://graph.microsoft.com/.default" },
        });

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(requestURL))
        {
            Content = content
        };

        var token = new TokenRequest();
        using (var response = await _httpClient.SendAsync(httpRequestMessage))
        {
            var responseStream = await response.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<TokenRequest>(responseStream);
        }
        return token;
    }
    
    public class TokenRequest
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
    
    public class FileUploadResponse
    {
        public string WebUrl { get; set; }
        public string Id { get; set; }
    }

}