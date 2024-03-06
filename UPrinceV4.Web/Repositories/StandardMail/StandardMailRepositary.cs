using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.StandardMails;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.StandardMail;

public class StandardMailRepositary : IStandardMailRepositary
{
    public async Task<IEnumerable<StandardMailHeaderDto>> StandardMailFilter(
        StandardMailParameters StandardMailParameters)
    {
        IEnumerable<StandardMailHeaderDto> results;
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                StandardMailParameters.ContractingUnitSequenceId,
                null, StandardMailParameters.TenantProvider);
            await using var connection = new SqlConnection(connectionString);

            var sql = "Select * From StandardMailHeader WHERE SequenceId IS NOT NULL ";

            var sb = new StringBuilder(sql);


            if (StandardMailParameters.Filter.Title != null)
            {
                StandardMailParameters.Filter.Title = StandardMailParameters.Filter.Title.Replace("'", "''");
                sb.Append("AND Title LIKE '%" + StandardMailParameters.Filter.Title + "%' ");

            }
                
            if (StandardMailParameters.Filter.Sorter.Attribute != null)
            {
                if (StandardMailParameters.Filter.Sorter.Attribute.ToLower().Equals("title"))
                    sb.Append("ORDER BY Title " + StandardMailParameters.Filter.Sorter.Order);
            }
            else
            {
                sb.Append("ORDER BY Title Desc");
            }

            results = await connection.QueryAsync<StandardMailHeaderDto>(sb.ToString());
        }
        catch (Exception e)
        {
            throw e;
        }

        return results;
    }

    public async Task<string> StandardMailCreate(StandardMailParameters StandardMailParameters)
    {
        var options1 = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options1, StandardMailParameters.TenantProvider);

        string seqId = null;

        var connectionString = ConnectionString.MapConnectionString(
            StandardMailParameters.ContractingUnitSequenceId,
            StandardMailParameters.ProjectSequenceId, StandardMailParameters.TenantProvider);
        await using var connection = new SqlConnection(connectionString);
        if (StandardMailParameters.StandardMailDto.Id != null)
        {
            var data = connection.QueryFirstOrDefault<StandardMailHeaderDto>(
                "Select * from StandardMailHeader where id = @id",
                new { id = StandardMailParameters.StandardMailDto.Id });

            if (data == null)
            {
                var idGenerator = new IdGenerator();
                seqId = idGenerator.GenerateId(applicationDbContext, "SM-", "StandardMailSequenceCode");
                var insertQuery =
                    "INSERT INTO dbo.StandardMailHeader ( Id ,MailHeader ,Title ,Name ,SequenceId ,RequestToWrittenInTender ,MeasuringStateRecieved ,Reminder1 ,Reminder1TimeFrameTender ,Reminder2 ,Reminder2TimeFrameTender ,Reminder3 ,Reminder3TimeFrameTender , Reminder4 ,Reminder4TimeFrameTender ,TenderWon ,TenderLost ,OutStandingComments ,CreatedDate ,Createdby , IsDefault ,AcceptTender , SubscribeTender , DownloadTender ) VALUES ( @Id ,@MailHeader ,@Title ,@Name ,@SequenceId ,@RequestToWrittenInTender ,@MeasuringStateRecieved ,@Reminder1 ,@Reminder1TimeFrameTender ,@Reminder2 ,@Reminder2TimeFrameTender ,@Reminder3 ,@Reminder3TimeFrameTender , @Reminder4 , @Reminder4TimeFrameTender ,@TenderWon ,@TenderLost ,@OutStandingComments ,@CreatedDate ,@Createdby ,@IsDefault , @AcceptTender , @SubscribeTender , @DownloadTender  )";
                var param = new
                {
                    StandardMailParameters.StandardMailDto.Id,
                    StandardMailParameters.StandardMailDto.MailHeader,
                    StandardMailParameters.StandardMailDto.Name,
                    Title = seqId + " - " + StandardMailParameters.StandardMailDto.Name,
                    SequenceId = seqId,
                    StandardMailParameters.StandardMailDto.RequestToWrittenInTender,
                    StandardMailParameters.StandardMailDto.MeasuringStateRecieved,
                    StandardMailParameters.StandardMailDto.Reminder1,
                    StandardMailParameters.StandardMailDto.Reminder1TimeFrameTender,
                    StandardMailParameters.StandardMailDto.Reminder2,
                    StandardMailParameters.StandardMailDto.Reminder2TimeFrameTender,
                    StandardMailParameters.StandardMailDto.Reminder3,
                    StandardMailParameters.StandardMailDto.Reminder3TimeFrameTender,
                    StandardMailParameters.StandardMailDto.TenderWon,
                    StandardMailParameters.StandardMailDto.TenderLost,
                    StandardMailParameters.StandardMailDto.OutStandingComments,
                    CreatedDate = DateTime.UtcNow,
                    Createdby = StandardMailParameters.UserId,
                    StandardMailParameters.StandardMailDto.IsDefault,
                    StandardMailParameters.StandardMailDto.Reminder4,
                    StandardMailParameters.StandardMailDto.Reminder4TimeFrameTender,
                    AcceptTender  = StandardMailParameters.StandardMailDto.AcceptTender,
                    SubscribeTender = StandardMailParameters.StandardMailDto.SubscribeTender, 
                    DownloadTender = StandardMailParameters.StandardMailDto.DownloadTender
                };

                if (StandardMailParameters.StandardMailDto.IsDefault)
                    await connection.ExecuteAsync(
                        "UPDATE dbo.StandardMailHeader SET IsDefault = @IsDefault Where Id IS NOT NULL",
                        new { IsDefault = false });

                await connection.ExecuteAsync(insertQuery, param);
            }
            else
            {
                var updateQuery =
                    "UPDATE dbo.StandardMailHeader SET MailHeader = @MailHeader ,Title = @Title , Name = @Name ,RequestToWrittenInTender = @RequestToWrittenInTender ,MeasuringStateRecieved = @MeasuringStateRecieved ,Reminder1 = @Reminder1 ,Reminder1TimeFrameTender = @Reminder1TimeFrameTender ,Reminder2 = @Reminder2 ,Reminder2TimeFrameTender = @Reminder2TimeFrameTender ,Reminder3 = @Reminder3 ,Reminder3TimeFrameTender = @Reminder3TimeFrameTender , Reminder4 = @Reminder4 ,Reminder4TimeFrameTender = @Reminder4TimeFrameTender ,TenderWon = @TenderWon ,TenderLost = @TenderLost ,OutStandingComments = @OutStandingComments ,ModifiedDate = @ModifiedDate ,Modifiedby =@Modifiedby , IsDefault = @IsDefault , AcceptTender = @AcceptTender , SubscribeTender = @SubscribeTender, DownloadTender = @DownloadTender WHERE Id = @Id ";

                seqId = data.SequenceId;
                var param = new
                {
                    StandardMailParameters.StandardMailDto.Id,
                    StandardMailParameters.StandardMailDto.MailHeader,
                    StandardMailParameters.StandardMailDto.Name,
                    Title = data.SequenceId + " - " + StandardMailParameters.StandardMailDto.Name,
                    StandardMailParameters.StandardMailDto.RequestToWrittenInTender,
                    StandardMailParameters.StandardMailDto.MeasuringStateRecieved,
                    StandardMailParameters.StandardMailDto.Reminder1,
                    StandardMailParameters.StandardMailDto.Reminder1TimeFrameTender,
                    StandardMailParameters.StandardMailDto.Reminder2,
                    StandardMailParameters.StandardMailDto.Reminder2TimeFrameTender,
                    StandardMailParameters.StandardMailDto.Reminder3,
                    StandardMailParameters.StandardMailDto.Reminder3TimeFrameTender,
                    StandardMailParameters.StandardMailDto.TenderWon,
                    StandardMailParameters.StandardMailDto.TenderLost,
                    StandardMailParameters.StandardMailDto.OutStandingComments,
                    ModifiedDate = DateTime.UtcNow,
                    Modifiedby = StandardMailParameters.UserId,
                    StandardMailParameters.StandardMailDto.IsDefault,
                    StandardMailParameters.StandardMailDto.Reminder4,
                    StandardMailParameters.StandardMailDto.Reminder4TimeFrameTender,
                    AcceptTender  = StandardMailParameters.StandardMailDto.AcceptTender,
                    SubscribeTender = StandardMailParameters.StandardMailDto.SubscribeTender, 
                    DownloadTender = StandardMailParameters.StandardMailDto.DownloadTender
                };

                if (StandardMailParameters.StandardMailDto.IsDefault)
                    await connection.ExecuteAsync(
                        "UPDATE dbo.StandardMailHeader SET IsDefault = @IsDefault Where Id IS NOT NULL",
                        new { IsDefault = false });
                await connection.ExecuteAsync(updateQuery, param);
            }
        }

        return seqId;
    }

    public async Task<StandardMailHeaderDto> StandardMailGetById(StandardMailParameters StandardMailParameters)
    {
        StandardMailHeaderDto results;
        try
        {
            var connectionString = ConnectionString.MapConnectionString(
                StandardMailParameters.ContractingUnitSequenceId,
                StandardMailParameters.ProjectSequenceId, StandardMailParameters.TenantProvider);
            await using var connection = new SqlConnection(connectionString);

            results = connection.Query<StandardMailHeaderDto>(
                "Select * From StandardMailHeader Where SequenceId = @Id ",
                new { StandardMailParameters.Id }).FirstOrDefault();

            var history = connection
                .Query<History>(
                    "SELECT [CreatedDate],[Createdby],[Modifiedby],[ModifiedDate] FROM[dbo].[StandardMailHeader] where [SequenceId] = @Id",
                    new { StandardMailParameters.Id }).FirstOrDefault();

            var ModifiedByUserQuery =
                @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [Modifiedby] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

            await using (var connection2 =
                         new SqlConnection(StandardMailParameters.TenantProvider.GetTenant().ConnectionString))
            {
                var ModifiedByParameter = new { oid = history.Modifiedby };
                history.Modifiedby = connection2.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                    .FirstOrDefault();

                var CreatByParam = new { oid = history.Createdby };
                history.Createdby = connection2.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();
            }

            results.History = history;
        }
        catch (Exception e)
        {
            throw e;
        }

        return results;
    }
}