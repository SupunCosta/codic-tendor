using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories.VP;

public class AbsenceRepository : IAbsenceRepository
{
    public async Task<AbsenceHeaderGetDto> AbsenceGetById(AbsenceParameter AbsenceParameter)
    {
        const string query = @"SELECT AbsenceHeader.Id
                                      ,AbsenceHeader.Person
                                      ,AbsenceHeader.LeaveType
                                      ,AbsenceHeader.StartDate
                                      ,AbsenceHeader.EndDate
                                      ,AbsenceHeader.StartTime
                                      ,AbsenceHeader.EndTime
                                      ,AbsenceHeader.AllDay
                                      ,CabPerson.FullName As PersonName
	                                  FROM dbo.AbsenceHeader
LEFT OUTER JOIN CabPersonCompany
ON AbsenceHeader.Person = CabPersonCompany.Id
INNER JOIN CabPerson
ON CabPersonCompany.PersonId = CabPerson.Id
	                                  WHERE AbsenceHeader.Id = @Id";

        var parm = new { AbsenceParameter.Id, AbsenceParameter.Lang };

        AbsenceHeaderGetDto absenceHeaderGetDto = null;

        await using var connection = new SqlConnection(AbsenceParameter.TenantProvider.GetTenant().ConnectionString);
        absenceHeaderGetDto = connection.Query<AbsenceHeaderGetDto>(query, parm).FirstOrDefault();

        return absenceHeaderGetDto;
    }

    public async Task<string> Create(AbsenceParameter AbsenceParameter)
    {
        var checkquery = @"SELECT * FROM dbo.AbsenceHeader WHERE Id = @Id";

        var checkparm = new { AbsenceParameter.AbsenceDto.Id };
        AbsenceHeader data;

        await using (var connection = new SqlConnection(AbsenceParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = connection.Query<AbsenceHeader>(checkquery, checkparm).FirstOrDefault();
        }

        AbsenceParameter.AbsenceDto.EndDate ??= AbsenceParameter.AbsenceDto.StartDate;

        if (data == null)
        {
            var insertquery =
                @"INSERT INTO [dbo].[AbsenceHeader] ([Id],[Person],[LeaveType],[StartDate],[EndDate],[StartTime],[EndTime],[AllDay]) VALUES (@Id,@Person,@LeaveType,@StartDate,@EndDate,@StartTime,@EndTime,@AllDay)";

            var parm = new
            {
                AbsenceParameter.AbsenceDto.Id,
                AbsenceParameter.AbsenceDto.Person,
                AbsenceParameter.AbsenceDto.LeaveType,
                AbsenceParameter.AbsenceDto.StartDate,
                AbsenceParameter.AbsenceDto.EndDate,
                AbsenceParameter.AbsenceDto.StartTime,
                AbsenceParameter.AbsenceDto.EndTime,
                AbsenceParameter.AbsenceDto.AllDay
            };

            await using var connection =
                new SqlConnection(AbsenceParameter.TenantProvider.GetTenant().ConnectionString);
            await connection.ExecuteAsync(insertquery, parm);
        }

        else
        {
            var updatequery =
                @"UPDATE [dbo].[AbsenceHeader] SET [Id] = @Id,[Person] = @Person,[LeaveType] = @LeaveType,[StartDate] = @StartDate,[EndDate] = @EndDate,[StartTime] = @StartTime,[EndTime] = @EndTime,[AllDay] = @AllDay WHERE Id = @Id";

            var parm = new
            {
                AbsenceParameter.AbsenceDto.Id,
                AbsenceParameter.AbsenceDto.Person,
                AbsenceParameter.AbsenceDto.LeaveType,
                AbsenceParameter.AbsenceDto.StartDate,
                AbsenceParameter.AbsenceDto.EndDate,
                AbsenceParameter.AbsenceDto.StartTime,
                AbsenceParameter.AbsenceDto.EndTime,
                AbsenceParameter.AbsenceDto.AllDay
            };

            await using var connection =
                new SqlConnection(AbsenceParameter.TenantProvider.GetTenant().ConnectionString);
            await connection.ExecuteAsync(updatequery, parm);
        }

        return AbsenceParameter.AbsenceDto.Id;
    }


    public async Task<IEnumerable<AbsenceLeaveTypeDto>> GetAbsenceLeaveType(AbsenceParameter AbsenceParameter)
    {
        IEnumerable<AbsenceLeaveTypeDto> data = null;

        const string query =
            @"SELECT Name As [Text], TypeId As [Key] FROM  dbo.AbsenceLeaveType WHERE LanguageCode = @lang;";

        await using var connection = new SqlConnection(AbsenceParameter.TenantProvider.GetTenant().ConnectionString);
        data = connection.Query<AbsenceLeaveTypeDto>(query, new { lang = AbsenceParameter.Lang });
        return data;
    }


    public async Task<IEnumerable<AbsenceHeaderGetDto>> GetAbsenceListByPersonId(AbsenceParameter AbsenceParameter)
    {
        IEnumerable<AbsenceHeaderGetDto> data = null;
        if (AbsenceParameter.Id != null)
        {
            const string query = @"SELECT
                                      AbsenceHeader.Id
                                     ,AbsenceHeader.Person
                                     ,AbsenceHeader.StartDate
                                     ,AbsenceHeader.EndDate
                                     ,AbsenceHeader.StartTime
                                     ,AbsenceHeader.EndTime
                                     ,AbsenceHeader.AllDay
                                     ,AbsenceLeaveType.TypeId AS [Key]
                                     ,AbsenceLeaveType.Name AS Text
                                     ,CabPerson.FullName As PersonName
                                    FROM dbo.AbsenceHeader
                                    LEFT OUTER JOIN dbo.AbsenceLeaveType
                                      ON AbsenceHeader.LeaveType = AbsenceLeaveType.TypeId
                                    LEFT OUTER JOIN CabPersonCompany
                                    ON AbsenceHeader.Person = CabPersonCompany.Id
                                    INNER JOIN CabPerson
                                    ON CabPersonCompany.PersonId = CabPerson.Id
                                    WHERE (AbsenceLeaveType.LanguageCode = @lang OR AbsenceHeader.LeaveType IS NULL)
                                    AND AbsenceHeader.Person = @PersonId;";


            await using var connection =
                new SqlConnection(AbsenceParameter.TenantProvider.GetTenant().ConnectionString);
            connection.Open();

            data = connection.Query<AbsenceHeaderGetDto, AbsenceLeaveTypeDto, AbsenceHeaderGetDto>(
                    query,
                    (invoice, invoiceDetail) =>
                    {
                        invoice.LeaveType = invoiceDetail;
                        return invoice;
                    }, new { PersonId = AbsenceParameter.Id, lang = AbsenceParameter.Lang },
                    splitOn: "Key")
                .Distinct()
                .ToList();
        }

        return data;
    }


    public async Task<string> DeleteAbsence(AbsenceParameter AbsenceParameter)
    {
        const string deleteQuery = @"DELETE FROM AbsenceHeader WHERE Id =@Id";
        if (AbsenceParameter.IdList.FirstOrDefault() != null)
            foreach (var id in AbsenceParameter.IdList)
            {
                await using var dbConnection =
                    new SqlConnection(AbsenceParameter.TenantProvider.GetTenant().ConnectionString);
                await dbConnection.ExecuteAsync(deleteQuery, new { Id = id });
            }

        return null;
    }
}