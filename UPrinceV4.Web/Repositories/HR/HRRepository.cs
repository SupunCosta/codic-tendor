using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.HR;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.HR;

public class HRRepository : IHRRepository
{
    public async Task<string> CreateHR(HRParameter HRParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext = new ApplicationDbContext(options, HRParameter.TenantProvider);

        var checkquery = @"SELECT * FROM dbo.HRHeader WHERE Id = @Id";

        var checkparm = new { HRParameter.CreateHR.Id };
        HRHeader data;
        string oid = null;
        string HRSequenceId = null;

        await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = connection.Query<HRHeader>(checkquery, checkparm).FirstOrDefault();
            oid = connection.Query<string>("SELECT Oid FROM CabPersonCompany WHERE Id = @PersonId",
                new { HRParameter.CreateHR.PersonId }).FirstOrDefault();
            if (HRParameter.CreateHR.IsContactManager)
                await connection.ExecuteAsync(
                    @"Update HRHeader Set IsContactManager = 0 Where HRHeader.Id IN (SELECT                             
                HRHeader.Id                            
                    FROM dbo.HRHeader
                    LEFT OUTER JOIN dbo.CabPersonCompany
                    ON HRHeader.PersonId = CabPersonCompany.Id
                WHERE CabPersonCompany.CompanyId = (SELECT                             
                CabPersonCompany.CompanyId 
                    FROM dbo.HRHeader
                    LEFT OUTER JOIN dbo.CabPersonCompany
                    ON HRHeader.PersonId = CabPersonCompany.Id                                                     
                WHERE HRHeader.PersonId = @personId))", new { personId = HRParameter.CreateHR.PersonId });
        }


        if (data == null)
        {
            var insertquery =
                @"INSERT INTO [dbo].[HRHeader] VALUES (@Id,@PersonName,@PersonId,@Title,@SequenceId,@OrganizationTaxonomyId,@AzureOid,@Role,@CreatedDate,@ModifiedDate,@CreatedBy,@ModifiedBy,@CpcLabourItemId,@BgColor,@FontColor,@IsContactManager,@WorkingOrganization,@Organization)";
            var idGenerator = new IdGenerator();
            HRSequenceId = idGenerator.GenerateId(applicationDbContext, "HR-", "HRSequence");
            var parm = new
            {
                HRParameter.CreateHR.Id,
                HRParameter.CreateHR.PersonName,
                HRParameter.CreateHR.PersonId,
                Title = HRSequenceId + " " + HRParameter.CreateHR.PersonName,
                SequenceId = HRSequenceId,
                HRParameter.CreateHR.OrganizationTaxonomyId,
                AzureOid = oid,
                HRParameter.CreateHR.Role,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreatedBy = HRParameter.UserId,
                ModifiedBy = HRParameter.UserId,
                HRParameter.CreateHR.CpcLabourItemId,
                HRParameter.CreateHR.BgColor,
                HRParameter.CreateHR.FontColor,
                HRParameter.CreateHR.IsContactManager,
                HRParameter.CreateHR.WorkingOrganization,
                HRParameter.CreateHR.Organization
            };

            await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
            {
                await connection.ExecuteAsync(insertquery, parm);
            }
        }

        else
        {
            var updatequery =
                @"UPDATE [dbo].[HRHeader] SET [PersonName] = @PersonName, [PersonId] = @PersonId, [Title] = @Title, [AzureOid] = @AzureOid, [OrganizationTaxonomyId] = @OrganizationTaxonomyId, [Role] = @Role, [ModifiedDate] = @ModifiedDate, [ModifiedBy] = @ModifiedBy, [CpcLabourItemId] = @CpcLabourItemId,[BgColor] = @BgColor,[FontColor] = @FontColor, IsContactManager = @IsContactManager, [WorkingOrganization] = @WorkingOrganization , [Organization] = @Organization WHERE Id = @Id";
            HRSequenceId = data.SequenceId;
            var parm = new
            {
                HRParameter.CreateHR.Id,
                HRParameter.CreateHR.PersonName,
                HRParameter.CreateHR.PersonId,
                Title = HRSequenceId + " " + HRParameter.CreateHR.PersonName,
                HRParameter.CreateHR.OrganizationTaxonomyId,
                AzureOid = oid,
                HRParameter.CreateHR.Role,
                ModifiedDate = DateTime.UtcNow,
                ModifiedBy = HRParameter.UserId,
                HRParameter.CreateHR.CpcLabourItemId,
                HRParameter.CreateHR.BgColor,
                HRParameter.CreateHR.FontColor,
                HRParameter.CreateHR.IsContactManager,
                HRParameter.CreateHR.WorkingOrganization,
                HRParameter.CreateHR.Organization
            };

            await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
            {
                await connection.ExecuteAsync(updatequery, parm);
            }
        }

        foreach (var time in HRParameter.CreateHR.WorkSchedule)
        {
            var timequery = @"MERGE INTO dbo.WorkSchedule
                                            t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id)
                                            WHEN MATCHED THEN UPDATE SET [Day] = @Day, [StartTime] = @StartTime,[EndTime] = @EndTime,[DisplayOrder] = @DisplayOrder,[HRHeaderId] = @HRHeaderId
                                            WHEN NOT MATCHED THEN INSERT ([Id],[Day],[StartTime],[EndTime],[DisplayOrder],[HRHeaderId])
                                            VALUES (@Id, @Day,@StartTime,@EndTime,@DisplayOrder,@HRHeaderId);";

            var timeparm = new
            {
                time.Id,
                time.Day,
                time.StartTime,
                time.EndTime,
                time.DisplayOrder,
                HRHeaderId = HRParameter.CreateHR.Id
            };

            await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
            {
                await connection.ExecuteAsync(timequery, timeparm);
            }
        }

        return HRSequenceId;
    }

    public async Task<GetHRByIdDto> GetHRById(HRParameter HRParameter)
    {
        // var options = new DbContextOptions<ApplicationDbContext>();
        // var applicationDbContext = new ApplicationDbContext(options, HRParameter.TenantProvider);

        var cuconnectionString = ConnectionString.MapConnectionString(HRParameter.ContractingUnitSequenceId,
            null, HRParameter.TenantProvider);

        GetHRByIdDto data;
        HRHistoryDto history;
        IEnumerable<GetWorkScheduleDto> workSchedule;

        var sql =
            @"SELECT
                          CabPerson.FullName AS PersonName
                         ,CabPerson.Id AS CabPersonId
                         ,CabCompany.Name AS WorkingOrganizationName
                         ,cc.Name AS CompanyName
                         ,HRHeader.*
                        FROM dbo.HRHeader
                        LEFT OUTER JOIN dbo.CabPersonCompany
                          ON HRHeader.PersonId = CabPersonCompany.Id
                        LEFT OUTER JOIN dbo.CabPerson
                          ON CabPersonCompany.PersonId = CabPerson.Id
                        LEFT OUTER JOIN dbo.CabCompany
                          ON HRHeader.WorkingOrganization = CabCompany.Id
                         LEFT OUTER JOIN dbo.CabCompany cc
                          ON CabPersonCompany.CompanyId = cc.Id

                        WHERE HRHeader.SequenceId = @SequenceId";
        await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = connection.Query<GetHRByIdDto>(sql, new { SequenceId = HRParameter.Id }).FirstOrDefault();
            history = connection
                .Query<HRHistoryDto>(
                    "SELECT[CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM[dbo].[HRHeader] where [SequenceId] = @Id",
                    new { HRParameter.Id }).FirstOrDefault();
            workSchedule = connection.Query<GetWorkScheduleDto>(
                "SELECT * FROM WorkSchedule WHERE HRHeaderId = @HRHeaderId ORDER BY DisplayOrder ASC",
                new { HRHeaderId = data.Id });
        }

        await using (var connection = new SqlConnection(cuconnectionString))
        {
            data.CpcLabourItemTitle = connection
                .Query<string>("SELECT Title FROM dbo.CorporateProductCatalog WHERE Id = @CPCId",
                    new { CPCId = data.CpcLabourItemId }).FirstOrDefault();
        }

        var ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";

        await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var ModifiedByParameter = new { oid = history.ModifiedBy };
            history.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                .FirstOrDefault();

            var CreatByParam = new { oid = history.CreatedBy };
            history.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();
        }

        data.History = history;
        data.WorkSchedule = workSchedule;

        return data;
    }

    public async Task<IEnumerable<GetHRListDto>> FilterHRList(HRParameter HRParameter)
    {
        IEnumerable<GetHRListDto> data = null;


        var sql = @"SELECT DISTINCT
                              HRHeader.SequenceId
                             ,CabPersonCompany.JobRole AS JobTitle
                             ,CabMobilePhone.MobilePhoneNumber AS Mobile
                             ,HRHeader.Id
                             ,CabPerson.FullName
                             ,CabCompany.Name AS Organization
                             ,CabEmail.EmailAddress AS Email
                            FROM dbo.HRHeader
                            LEFT OUTER JOIN dbo.CabPersonCompany
                              ON HRHeader.PersonId = CabPersonCompany.Id
                            LEFT OUTER JOIN dbo.CabPerson
                              ON CabPersonCompany.PersonId = CabPerson.Id
                            LEFT OUTER JOIN dbo.CabMobilePhone
                              ON CabPersonCompany.MobilePhoneId = CabMobilePhone.Id
                            LEFT OUTER JOIN dbo.CabCompany
                              ON CabPersonCompany.CompanyId = CabCompany.Id
                            LEFT OUTER JOIN dbo.CabEmail
                              ON CabPersonCompany.EmailId = CabEmail.Id
                            LEFT OUTER JOIN dbo.HRContractorList hl
                            ON HRHeader.Id = hl.HRId
                            WHERE SequenceId IS NOT NULL ";


        var sb = new StringBuilder(sql);


        if (HRParameter.Filter.FullName != null)
        {
            HRParameter.Filter.FullName = HRParameter.Filter.FullName.Replace("'", "''");
            sb.Append(" AND CabPerson.FullName like '%" + HRParameter.Filter.FullName + "%' ");

        }

        if (HRParameter.Filter.Email != null)
        {
            HRParameter.Filter.Email = HRParameter.Filter.Email.Replace("'", "''");
            sb.Append(" AND CabEmail.EmailAddress like '%" + HRParameter.Filter.Email + "%' ");

        }

        if (HRParameter.Filter.Mobile != null)
        {
            HRParameter.Filter.Mobile = HRParameter.Filter.Mobile.Replace("'", "''");
            sb.Append(" AND CabMobilePhone.MobilePhoneNumber like '%" + HRParameter.Filter.Mobile + "%' ");

        }

        if (HRParameter.Filter.JobTitle != null)
        {
            HRParameter.Filter.JobTitle = HRParameter.Filter.JobTitle.Replace("'", "''");
            sb.Append(" AND CabPersonCompany.JobRole like '%" + HRParameter.Filter.JobTitle + "%' ");

        }

        if (HRParameter.Filter.Organization != null)
        {
            HRParameter.Filter.Organization = HRParameter.Filter.Organization.Replace("'", "''");
            sb.Append(" AND CabCompany.Name like '%" + HRParameter.Filter.Organization + "%' ");

        }

        if (HRParameter.Filter.Active)
        {
            sb.Append(" AND ('"+ DateTime.UtcNow + "' BETWEEN hl.StartDate AND hl.EndDate) OR (hl.EndDate IS NULL AND hl.StartDate <= '"+ DateTime.UtcNow +"') ");
        }

        if (HRParameter.Filter.NonActive)
        {
            sb.Append(" AND ('"+ DateTime.UtcNow + "' NOT BETWEEN hl.StartDate AND hl.EndDate OR hl.StartDate IS NULL) ");
        }

        if (HRParameter.Filter.Sorter.Attribute == null) sb.Append("ORDER BY CabPerson.FullName asc ");

        if (HRParameter.Filter.Sorter.Attribute != null)
            switch (HRParameter.Filter.Sorter.Attribute.ToLower())
            {
                case "fullname":
                    sb.Append("ORDER BY CabPerson.FullName " + HRParameter.Filter.Sorter.Order);
                    break;
                case "email":
                    sb.Append("ORDER BY CabEmail.EmailAddress " + HRParameter.Filter.Sorter.Order);
                    break;
                case "mobile":
                    sb.Append("ORDER BY CabMobilePhone.MobilePhoneNumber " + HRParameter.Filter.Sorter.Order);
                    break;
                case "jobtitle":
                    sb.Append("ORDER BY CabPersonCompany.JobRole " + HRParameter.Filter.Sorter.Order);
                    break;
                case "organization":
                    sb.Append("ORDER BY CabCompany.Name " + HRParameter.Filter.Sorter.Order);
                    break;
            }

        await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.OpenAsync();
            data = await connection.QueryAsync<GetHRListDto>(sb.ToString(), new { SequenceId = HRParameter.Id });
        }
        return data;
    }

    public async Task<PersonById> GetTaxonomyIdByPersonId(HRParameter HRParameter)
    {
        PersonById data = null;
        if (HRParameter.Id != null)
        {
            var query = @"SELECT
  CabPersonCompany.Oid AS AzureOid
 ,OrganizationTaxonomy.Id AS TaxonomyId
FROM dbo.OrganizationTaxonomy
INNER JOIN dbo.CabPersonCompany
  ON OrganizationTaxonomy.PersonId = CabPersonCompany.Id
  WHERE ParentId IN (SELECT id FROM OrganizationTaxonomy ) AND OrganizationTaxonomy.PersonId = @PersonId;";


            await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
            {
                data = connection.Query<PersonById>(query, new { PersonId = HRParameter.Id }).FirstOrDefault();
            }
        }

        return data;
    }

    public async Task<IEnumerable<GetHRRoles>> GetHRRoles(HRParameter HRParameter)
    {
        IEnumerable<GetHRRoles> data;
        var sql = @"SELECT RoleId As [Key], Name As Text FROM HRRoles WHERE LanguageCode = @lang";
        await using (var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString))
        {
            data = connection.Query<GetHRRoles>(sql, new { lang = HRParameter.Lang });
        }

        return data;
    }

    public async Task<IEnumerable<PmolData>> GetLabourHistory(HRParameter HRParameter)
    {
        var pmoldata = new List<PmolData>();

        var projectManager =
            HRParameter.IPmolRepository.ProjectPm(HRParameter.TenantProvider.GetTenant().ConnectionString);

        foreach (var manager in projectManager)
        {
            try
            {

            
                IEnumerable<TeamsWithPmolDto> teams;

                var vpParameter = new VPParameter
                {
                    Lang = HRParameter.Lang,
                    TenantProvider = HRParameter.TenantProvider,
                    ContractingUnitSequenceId = HRParameter.ContractingUnitSequenceId,
                    ProjectSequenceId = manager.SequenceCode,
                    IsLabourHistory = true
                };

                teams = await HRParameter.IVPRepository.Teams(vpParameter);

                var data = teams.Where(p => p.Team.Any(t => t.Id == HRParameter.Id));

                foreach (var mm in data)
                {
                    foreach (var pmol in mm.Pmol)
                    {
                        pmol.ProjectManager = manager.FullName;
                        pmol.ProjectSequenceCode = manager.SequenceCode;
                        var sDate = Convert.ToDateTime(pmol.ExecutionStartTime);
                        var eDate = Convert.ToDateTime(pmol.ExecutionEndTime);
                        pmol.ExecutionStartTime = sDate.ToString("HH:MM");
                        pmol.ExecutionEndTime = eDate.ToString("HH:MM");
                    }

                    pmoldata.AddRange(mm.Pmol.ToList());
                    pmoldata = pmoldata.OrderByDescending(x => x.ExecutionDate).ToList();
                }
            
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                throw;
            }
        }

        return pmoldata;
    }

    public async Task<IEnumerable<CabDataDto>> HRPersonFilter(HRParameter HRParameter)
    {
        //CREATE VIEW CabFilter AS 
        var sql = "SELECT CabPerson.Id AS PersonId ,CabPerson.FullName AS FullName" +
                  " ,CabPersonCompany.JobRole AS JobTitle ,CabPerson.IsSaved AS IsSaved" +
                  " ,CabEmail.EmailAddress AS Email ,CabMobilePhone.MobilePhoneNumber AS MobileNumber" +
                  " ,CabCompany.Name AS Organisation ,CabCompany.Id AS CompanyId " +
                  " ,CabPersonCompany.Id AS PersonCompanyId FROM dbo.CabPerson" +
                  " LEFT OUTER JOIN dbo.CabPersonCompany ON CabPerson.Id = CabPersonCompany.PersonId" +
                  " LEFT OUTER JOIN dbo.CabCompany ON CabCompany.Id = CabPersonCompany.CompanyId" +
                  " LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id" +
                  " LEFT OUTER JOIN dbo.CabMobilePhone ON CabPersonCompany.MobilePhoneId = CabMobilePhone.Id" +
                  " WHERE CabPerson.IsDeleted = 0 AND CabPersonCompany.Id NOT IN (SELECT PersonId FROM HRHeader WHERE PersonId IS NOT NULL)";
        var sb = new StringBuilder(sql);
        var filter = HRParameter.CabPersonFilter;
        if (filter != null)
        {
            if (filter.FullName != null)
            {
                filter.FullName = filter.FullName.Replace("'", "''");
                sb.Append(" AND CabPerson.FullName LIKE '%" + filter.FullName + "%'");

            }

            if (filter.MobileNumber != null)
            {
                filter.MobileNumber = filter.MobileNumber.Replace("'", "''");
                sb.Append(" AND CabMobilePhone.MobilePhoneNumber LIKE '%" + filter.MobileNumber + "%'");

            }

            if (filter.Organisation != null)
            {
                filter.Organisation = filter.Organisation.Replace("'", "''");
                sb.Append(" AND CabCompany.Name LIKE '%" + filter.Organisation + "%'");

            }

            if (filter.JobTitle != null)
            {
                filter.JobTitle = filter.JobTitle.Replace("'", "''");
                sb.Append(" AND CabPersonCompany.JobRole LIKE '%" + filter.JobTitle + "%'");
 
            }

            if (filter.Email != null)
            {
                filter.Email = filter.Email.Replace("'", "''");
                sb.Append(" AND CabEmail.EmailAddress LIKE '%" + filter.Email + "%'");

            }
                
            if (filter.IsSaved != null)
            {
                if (filter.IsSaved.ToLower() == "true")
                    sb.Append(" AND CabPerson.IsSaved = 1");
                else if (filter.IsSaved.ToLower() == "false") sb.Append(" AND CabPerson.IsSaved = 0");
            }
        }

        //organisation mobileNumber
        if (filter.CabPersonSortingModel != null)
        {
            if (filter.CabPersonSortingModel.Attribute != null &&
                filter.CabPersonSortingModel.Order.Equals("asc"))
                sb.Append(" ORDER BY " + filter.CabPersonSortingModel.Attribute + " ASC");

            if (filter.CabPersonSortingModel.Attribute != null &&
                filter.CabPersonSortingModel.Order.Equals("desc"))
                sb.Append(" ORDER BY " + filter.CabPersonSortingModel.Attribute + " DESC");
        }

        using IDbConnection dbConnection =
            new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString);
        var result = await dbConnection.QueryAsync<CabDataDapperDto>(sb.ToString());

        var cabDataDtos = new List<CabDataDto>();
        foreach (var dto in result.ToList())
        {
            var cabDataDto = new CabDataDto
            {
                IsSaved = dto.IsSaved,
                CompanyId = dto.CompanyId
            };

            var personDto = new PersonDto
            {
                Id = dto.PersonId,
                FullName = dto.FullName
            };

            cabDataDto.Person = personDto;

            var personCompanyDto = new PersonCompanyDto
            {
                Id = dto.PersonCompanyId,
                JobRole = dto.JobTitle,
                Email = dto.Email,
                MobilePhone = dto.MobileNumber
            };

            cabDataDto.PersonCompany = personCompanyDto;

            var companyDto = new CompanyDto
            {
                Name = dto.Organisation,
                Id = dto.CompanyId
            };

            cabDataDto.Company = companyDto;
            cabDataDtos.Add(cabDataDto);
        }

        

        return cabDataDtos;
    }

    public async Task<string> RemoveHr(HRParameter HRParameters)
    {
        IEnumerable<HRHeader> hrHeader;
        IEnumerable<VpHR> taxonomyList;
        hrHeader = null;

        var taxonomySql = @"DELETE
                            FROM dbo.OrganizationTaxonomy
                            WHERE PersonId IN (SELECT dbo.HRHeader.PersonId FROM dbo.HRHeader WHERE Id = @id)";

        var sql = @"DELETE FROM dbo.HRHeader WHERE Id = @id";


        await using (var dbConnection = new SqlConnection(HRParameters.TenantProvider.GetTenant().ConnectionString))
        {
            taxonomyList = dbConnection.Query<VpHR>(taxonomySql, new { id = HRParameters.Id }).ToList();
            hrHeader = dbConnection.Query<HRHeader>(sql, new { id = HRParameters.Id });
        }

        return HRParameters.Id;
    }

    public async Task<List<HRLabourPmolPr>> GetLabourPmolPr(HRParameter HRParameter)
    {
        await using var dbConnection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString);
        var cuconnectionString = ConnectionString.MapConnectionString(HRParameter.ContractingUnitSequenceId, null,
            HRParameter.TenantProvider);
        
        await using var cuConnection = new SqlConnection(cuconnectionString);

        var data = new List<HRLabourPmolPr>();
        
        var cabPersonId = dbConnection
            .Query<string>("SELECT PersonId FROM CabPersonCompany WHERE Id = @Id", new { Id = HRParameter.Id })
            .FirstOrDefault();
        var projects = dbConnection.Query<ProjectDefinition>(@"SELECT * FROM dbo.ProjectDefinition WHERE ProjectDefinition.IsDeleted = 0").ToList();

        var poData = cuConnection.Query<POHeader>("Select * from POHeader").ToList();
        foreach (var item in projects)
        {
            await using var connection = new SqlConnection(item.ProjectConnectionString);

            
            var list = new HRLabourPmolPr()
            {
                Project = item.Title
            };

            var param1 = new
            {
                PersonId = HRParameter.Id,
                ProjectSequenceCode = item.SequenceCode,
                Date = DateTime.UtcNow.Date
            };
            var pr = dbConnection
                .Query<PmolPr>(
                    "SELECT PoId AS SequenceId,Id FROM VpHR WHERE PersonId = @PersonId AND ProjectSequenceCode = @ProjectSequenceCode AND EndDate > @Date",
                    param1).ToList();
            
            pr.ForEach(x => x.Title = poData.FirstOrDefault(c => c.SequenceId == x.SequenceId)?.Title);
            list.Pr = pr;

            list.Pmol = connection.Query<PmolPr>(
                    @"SELECT pmol.Id,PMol.ProjectMoleculeId AS SequenceId,PMol.Title  FROM PMol
                                                LEFT OUTER JOIN PMolPlannedWorkLabour ppwl ON PMol.Id = ppwl.PmolId LEFT OUTER JOIN PmolTeamRole ptr ON ppwl.Id = ptr.PmolLabourId 
                                                WHERE ptr.CabPersonId = @PersonId AND ppwl.IsDeleted = 0 AND ptr.IsDeleted = 0 AND pmol.ExecutionDate > @date",new{PersonId = cabPersonId ,date = DateTime.UtcNow.Date})
                .ToList();

            if (list.Pr.Any() || list.Pmol.Any())
            {
                data.Add(list);

            }

        }

        return data;
    }
    
    public async Task<HRContractorList> CreateHrContractorList(HRParameter HRParameter)
    {
        // var cuConnectionString = ConnectionString.MapConnectionString(HRParameter.ContractingUnitSequenceId, null,
        //     HRParameter.TenantProvider);
        await using var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString);

        //await using var cuConnection = new SqlConnection(cuConnectionString);

        var insertQuery =
            @"INSERT INTO dbo.HRContractorList ( Id ,HRId ,StartDate ,EndDate ,ContractTypeId ,Url ) VALUES ( @Id ,@HRId ,@StartDate ,@EndDate ,@ContractTypeId ,@Url )";
        
        var updteQuery =
            @"UPDATE  dbo.HRContractorList SET StartDate = @StartDate , EndDate = @EndDate , ContractTypeId = @ContractTypeId , Url = @Url Where Id = @Id";
        
       
            if (HRParameter.HRContractorListDto.Id == null)
            {
                HRParameter.HRContractorListDto.Id = Guid.NewGuid().ToString();
                await connection.ExecuteAsync(insertQuery, HRParameter.HRContractorListDto);
            }
            else
            {
                await connection.ExecuteAsync(updteQuery, HRParameter.HRContractorListDto);
 
            }
        
        
        return HRParameter.HRContractorListDto;
    }
    
    public async Task<List<HRContractorList>> GetHrContractorList(HRParameter HRParameter)
    {
        // var cuConnectionString = ConnectionString.MapConnectionString(HRParameter.ContractingUnitSequenceId, null,
        //     HRParameter.TenantProvider);
        await using var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString);

        // await using var cuConnection = new SqlConnection(cuConnectionString);

        var data = connection.Query<HRContractorList>("Select * From HRContractorList Where HRId = @HRId",
            new { HRId = HRParameter.Id }).ToList();
        
        
        return data;
    }
    
    public async Task<List<string>> DeleteHrContractorList(HRParameter HRParameter)
    {
        // var cuConnectionString = ConnectionString.MapConnectionString(HRParameter.ContractingUnitSequenceId, null,
        //     HRParameter.TenantProvider);
        await using var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString);

        //await using var cuConnection = new SqlConnection(cuConnectionString);

        var data = connection.Query<HRContractorList>("Delete  From HRContractorList Where Id IN @Ids",
            new { Ids = HRParameter.IdList });
        
        
        return HRParameter.IdList;
    }
    
    public async Task<List<GetHRContractTypes>> GetHrContractTypes(HRParameter HRParameter)
    {
        // var cuConnectionString = ConnectionString.MapConnectionString(HRParameter.ContractingUnitSequenceId, null,
        //     HRParameter.TenantProvider);
        
        await using var connection = new SqlConnection(HRParameter.TenantProvider.GetTenant().ConnectionString);

        
        //await using var cuConnection = new SqlConnection(cuConnectionString);

        var data = connection.Query<GetHRContractTypes>(
            "Select TypeId As [Key], Name As Text From HRContractTypes Where LanguageCode = @lang",
            new { lang = HRParameter.Lang }).ToList();
        
        return data;
    }

}