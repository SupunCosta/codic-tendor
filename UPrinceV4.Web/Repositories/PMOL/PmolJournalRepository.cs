using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PMOL;

public class PmolJournalRepository : IPmolJournalRepository
{
    public async Task<string> CreateJournal(PmolJournalParameter PmolParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            PmolParameter.ProjectSequenceId, PmolParameter.TenantProvider);
        var journal = new PmolJournal();
        await using (var context = new ShanukaDbContext(options, connectionString, PmolParameter.TenantProvider))
        {
            string id = null;
            var exixtingJournal =
                context.PMolJournal.FirstOrDefault(j => j.PmolId.Equals(PmolParameter.PmolDto.PmolId));
            if (exixtingJournal == null)
            {
                journal.DoneWork = PmolParameter.PmolDto.DoneWork;
                journal.EncounteredProblem = PmolParameter.PmolDto.EncounteredProblem;
                journal.LessonsLearned = PmolParameter.PmolDto.LessonsLearned;
                journal.PmolId = PmolParameter.PmolDto.PmolId;
                journal.ReportedThings = PmolParameter.PmolDto.ReportedThings;
                journal.Id = Guid.NewGuid().ToString();
                context.PMolJournal.Add(journal);
                context.SaveChanges();

                id = journal.Id;
            }
            else
            {
                if (PmolParameter.PmolDto.DoneWork != null)
                    exixtingJournal.DoneWork = PmolParameter.PmolDto.DoneWork;

                if (PmolParameter.PmolDto.EncounteredProblem != null)
                    exixtingJournal.EncounteredProblem = PmolParameter.PmolDto.EncounteredProblem;

                if (PmolParameter.PmolDto.LessonsLearned != null)
                    exixtingJournal.LessonsLearned = PmolParameter.PmolDto.LessonsLearned;

                if (PmolParameter.PmolDto.ReportedThings != null)
                    exixtingJournal.ReportedThings = PmolParameter.PmolDto.ReportedThings;

                context.PMolJournal.Update(exixtingJournal);
                context.SaveChanges();
                id = exixtingJournal.Id;
            }

            if (PmolParameter.PmolDto.PictureList != null)
                foreach (var dto in PmolParameter.PmolDto.PictureList)
                {
                    var picture = new PmolJournalPicture();
                    picture.Link = dto.Link;
                    picture.PmolJournalId = id;
                    picture.Type = dto.Type;

                    if (dto.Id == null)
                    {
                        picture.Id = Guid.NewGuid().ToString();
                        context.PMolJournalPicture.Add(picture);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        picture.Id = dto.Id;
                        context.PMolJournalPicture.Update(picture);
                        await context.SaveChangesAsync();
                    }
                }
        }

        return journal.Id;
    }

    public async Task<PmolJournalCreateDto> ReadJournal(PmolJournalParameter PmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(PmolParameter.ContractingUnitSequenceId,
            PmolParameter.ProjectSequenceId, PmolParameter.TenantProvider);
        var sql = "select * from PmolJournal where PmolId = @Id";

        var param = new { PmolParameter.Id };
        PmolJournalCreateDto journal;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            journal = await dbConnection.QueryFirstOrDefaultAsync<PmolJournalCreateDto>(sql, param);
            
        }

        if (journal != null)
        {
            var sqlPicture = "select * from PMolJournalPicture where PmolJournalId = @Id";

            var paramPicture = new { journal.Id };
            using (var dbConnection = new SqlConnection(connectionString))
            {
                journal.PictureList =
                    await dbConnection.QueryAsync<PmolJournalPictureCreateDto>(sqlPicture, paramPicture);
                
            }
        }

        return journal;
    }

    public async Task<string> ReadJournalDoneWork(PmolJournalParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var sql = "select DoneWork AS DoneWork from PmolJournal where PmolId = @Id";

        var param = new { pmolParameter.Id };
        using var dbConnection = new SqlConnection(connectionString);
        var doneWork = dbConnection.Query<string>(sql, param).FirstOrDefault();
        

        return doneWork;
    }

    public async Task<string> ReadJournalEncounteredProblem(PmolJournalParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var sql = "select EncounteredProblem AS EncounteredProblem from PmolJournal where PmolId = @Id";

        var param = new { pmolParameter.Id };
        string encounteredProblem;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            encounteredProblem = await dbConnection.QueryFirstOrDefaultAsync<string>(sql, param);
            
        }

        return encounteredProblem;
    }

    public async Task<string> ReadJournalLessonsLearned(PmolJournalParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var sql = "select LessonsLearned AS LessonsLearned from PmolJournal where PmolId = @Id";

        var param = new { pmolParameter.Id };
        string lessonsLearned;
        using var dbConnection = new SqlConnection(connectionString);
        lessonsLearned = await dbConnection.QueryFirstOrDefaultAsync<string>(sql, param);
        

        return lessonsLearned;
    }

    public async Task<string> ReadJournalReportedThings(PmolJournalParameter pmolParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
        var sql = "select * from PmolJournal where PmolId = @Id";

        var param = new { pmolParameter.Id };
        string reportedThings;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            reportedThings = await dbConnection.QueryFirstOrDefaultAsync<string>(sql, param);
            
        }

        return reportedThings;
    }

    public async Task<string> UploadJournalPictureForMobile(PmolJournalParameter pmolParameter)
    {
        //var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId,
            pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);

        await using var connection = new SqlConnection(connectionString);

        //var context = new ShanukaDbContext(options, connectionString, pmolParameter.TenantProvider);

        string pmolId = pmolParameter.formData["pmolId"];
        string type = pmolParameter.formData["type"];
        //var Journal = context.PMolJournal.FirstOrDefault(e => e.PmolId == pmolId);

        var Journal = connection
            .Query<PmolJournal>("Select * from PMolJournal Where PmolId = @PmolId", new { PmolId = pmolId })
            .FirstOrDefault();
        string id = null;
        if (Journal == null)
        {
            var insertQuery = "INSERT INTO dbo.PMolJournal ( Id , PmolId ) VALUES ( @Id , @PmolId ) ";
            var pmolJournal = new PmolJournal
            {
                PmolId = pmolId,
                Id = Guid.NewGuid().ToString()
            };
            // context.PMolJournal.Add(pmolJournal);
            // await context.SaveChangesAsync();
            await connection.ExecuteAsync(insertQuery, pmolJournal);

            id = pmolJournal.Id;
        }
        else
        {
            id = Journal.Id;
        }

        var client = new FileClient();

        foreach (var file in pmolParameter.formData.Files)
        {
            var url = client.PersistPhotoInNewFolder(file?.FileName, pmolParameter.TenantProvider, file,
                "Journal Documents");

            var picQuery =
                "INSERT INTO dbo.PMolJournalPicture ( Id ,Link ,Type ,PmolJournalId ) VALUES ( @Id ,@Link ,@Type ,@PmolJournalId )";
            var pmolJournalPicture = new PmolJournalPicture
            {
                Link = url,
                Type = type,
                PmolJournalId = id,
                Id = Guid.NewGuid().ToString()
            };

            // context.PMolJournalPicture.Add(pmolJournalPicture);
            // await context.SaveChangesAsync();

            await connection.ExecuteAsync(picQuery, pmolJournalPicture);
        }

        return pmolId;
        // return id;
    }
}