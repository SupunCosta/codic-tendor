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
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories;

public class OrganizationSettingsRepository : IOrganizationSettingsRepository
{
    public async Task<string> CreateOrganizationCompetencies(OSParameter OSParameter)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options, OSParameter.TenantProvider);
            string OCSequenceId = null;
            OrganizationCompetencies data;


            await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

            data = connection
                .Query<OrganizationCompetencies>("SELECT * FROM OrganizationCompetencies WHERE Id = @Id ",
                    new { OSParameter.OrganizationCompetenciesCreate.Id }).FirstOrDefault();


            if (data == null)
            {
                const string insertQuery =
                    "INSERT INTO OrganizationCompetencies VALUES (@Id, @Name, @SequenceId, @Title, @Description, @ExperienceLevelId, @Occupation, @Qualification, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate,@SkillTypeId)";
                var idGenerator = new IdGenerator();
                OCSequenceId = idGenerator.GenerateId(applicationDbContext, "OC-", "OCSequence");

                var parameters = new
                {
                    OSParameter.OrganizationCompetenciesCreate.Id,
                    OSParameter.OrganizationCompetenciesCreate.Name,
                    SequenceId = OCSequenceId,
                    Title = OCSequenceId + " " + OSParameter.OrganizationCompetenciesCreate.Name,
                    OSParameter.OrganizationCompetenciesCreate.Description,
                    OSParameter.OrganizationCompetenciesCreate.ExperienceLevelId,
                    OSParameter.OrganizationCompetenciesCreate.Occupation,
                    OSParameter.OrganizationCompetenciesCreate.Qualification,
                    CreatedBy = OSParameter.UserId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = OSParameter.UserId,
                    ModifiedDate = DateTime.UtcNow,
                    OSParameter.OrganizationCompetenciesCreate.SkillTypeId
                };
                await connection.ExecuteAsync(insertQuery, parameters);
            }
            else
            {
                OCSequenceId = data.SequenceId;
                var updateQuery =
                    @"UPDATE OrganizationCompetencies SET Name = @Name, Title = @Title, Description = @Description,ExperienceLevelId =  @ExperienceLevelId, Occupation = @Occupation, Qualification = @Qualification, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate, SkillTypeId = @SkillTypeId WHERE Id = @Id";

                var parameters = new
                {
                    data.Id,
                    OSParameter.OrganizationCompetenciesCreate.Name,
                    Title = OCSequenceId + " " + OSParameter.OrganizationCompetenciesCreate.Name,
                    OSParameter.OrganizationCompetenciesCreate.Description,
                    OSParameter.OrganizationCompetenciesCreate.ExperienceLevelId,
                    OSParameter.OrganizationCompetenciesCreate.Occupation,
                    OSParameter.OrganizationCompetenciesCreate.Qualification,
                    ModifiedBy = OSParameter.UserId,
                    ModifiedDate = DateTime.UtcNow,
                    OSParameter.OrganizationCompetenciesCreate.SkillTypeId
                };

                await connection.ExecuteAsync(updateQuery, parameters);
            }

            if (OSParameter.OrganizationCompetenciesCreate.SkillTypeId != null)
            {
                var query =
                    @"MERGE INTO dbo.CompetenciesTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE  SET Title = @Title, ParentId = @ParentId WHEN NOT MATCHED THEN INSERT (Id,CompetenciesId,ParentId,CompetenciesTaxonomyLevelId,Title,ParentCompetenciesId) VALUES (@Id,@CompetenciesId,@ParentId,@CompetenciesTaxonomyLevelId,@Title, @ParentCompetenciesId);";
                var parametertax = new
                {
                    OSParameter.OrganizationCompetenciesCreate.Id,
                    Title = OSParameter.OrganizationCompetenciesCreate.Name,
                    CompetenciesId = OSParameter.OrganizationCompetenciesCreate.Id,
                    ParentId = OSParameter.OrganizationCompetenciesCreate.SkillTypeId,
                    CompetenciesTaxonomyLevelId = "4010e768-3e06-po02-b337-ee367a82addb",
                    ParentCompetenciesId = OSParameter.OrganizationCompetenciesCreate.Id
                };


                await connection.ExecuteAsync(query, parametertax);

                return OCSequenceId;
            }


            return OCSequenceId;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<GetOrganizationCompetencies>> GetOrganizationCompetenceList(
        OSParameter OSParameter)
    {
        const string query = @"SELECT * FROM OrganizationCompetencies";

        IEnumerable<GetOrganizationCompetencies> data;
        var parameters = new { lang = OSParameter.Lang };
        await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);
        data = await connection.QueryAsync<GetOrganizationCompetencies>(query, parameters);

        return data;
    }

    public async Task<GetOrganizationCompetencies> OrganizationCompetenceGetById(OSParameter OSParameter)
    {
        //string connectionString = ConnectionString.MapConnectionString(OSParameter.ContractingUnitSequenceId, OSParameter.ProjectSequenceId, OSParameter.TenantProvider);

        const string query = @"SELECT * FROM OrganizationCompetencies WHERE SequenceId = @Id";

        GetOrganizationCompetencies data;
        OrganizationCompetenciesHistory historyLog;
        var parameters = new { OSParameter.Id };
        await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

        data = connection.Query<GetOrganizationCompetencies>(query, parameters).FirstOrDefault();
        historyLog = connection
            .Query<OrganizationCompetenciesHistory>(
                "SELECT[CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM[dbo].[OrganizationCompetencies] where [SequenceId] = @Id",
                new { OSParameter.Id }).FirstOrDefault();


        const string ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";


        if (historyLog != null)
        {
            var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
            historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                .FirstOrDefault();


            var CreatByParam = new { oid = historyLog.CreatedBy };
            historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();
        }

        if (data != null) data.History = historyLog;

        return data;
    }

    public async Task<OrganizationCompetenciesDownData> GetOrganizationCompetenceDropdown(OSParameter OSParameter)
    {
        const string query =
            @"SELECT * FROM dbo.CompetenciesTaxonomyLevel WHERE LanguageCode = @lang ORDER BY DisplayOrder";
        const string query2 = @"
                              SELECT
                              PbsExperienceLocalizedData.PbsExperienceId AS [key]
                            ,PbsExperienceLocalizedData.Label AS Text
                            FROM dbo.PbsExperienceLocalizedData
                            WHERE PbsExperienceLocalizedData.LanguageCode = @lang
                            order by PbsExperienceLocalizedData.Label
                             ";
        var DropDownData = new OrganizationCompetenciesDownData();
        var parameters = new { lang = OSParameter.Lang };

        await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);
        DropDownData.CompetenciesTaxonomyLevel =
            connection.Query<CompetenciesTaxonomyLevel>(query, parameters);
        DropDownData.Experience = connection.Query<PbsExperienceDto>(query2, parameters);

        return DropDownData;
    }

    public async Task<OrganizationDownData> GetOrganizationDropdown(OSParameter OSParameter)
    {
        const string query2 =
            @"SELECT RoleId AS [Key], Name AS Text FROM dbo.OrganizationTeamRole WHERE LanguageCode = @lang ORDER BY DisplayOrder";
        var DropDownData = new OrganizationDownData();
        var parameters = new { lang = OSParameter.Lang };

        await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

        DropDownData.OrganizationTeamRole = connection.Query<OrganizationTeamRoleDto>(query2, parameters);

        return DropDownData;
    }

    public async Task<string> CreateCompetenceTaxonomy(OSParameter OSParameter)
    {
        if (OSParameter.CompetenciesTaxonomyDto.Id != null)
        {
            const string query =
                @"MERGE INTO dbo.CompetenciesTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE  SET Title = @Title WHEN NOT MATCHED THEN INSERT (Id,CompetenciesId,ParentId,CompetenciesTaxonomyLevelId,Title,ParentCompetenciesId) VALUES (@Id,@CompetenciesId,@ParentId,@CompetenciesTaxonomyLevelId,@Title, @ParentCompetenciesId);";
            var parameters = new
            {
                OSParameter.CompetenciesTaxonomyDto.Id,
                OSParameter.CompetenciesTaxonomyDto.Title,
                OSParameter.CompetenciesTaxonomyDto.CompetenciesId,
                OSParameter.CompetenciesTaxonomyDto.ParentId,
                OSParameter.CompetenciesTaxonomyDto.CompetenciesTaxonomyLevelId,
                ParentCompetenciesId = OSParameter.CompetenciesTaxonomyDto.CompetenciesId
            };

            await using (var connection =
                         new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString))
            {
                await connection.ExecuteAsync(query, parameters);
            }

            return OSParameter.CompetenciesTaxonomyDto.Id;
        }

        throw new Exception("ID not set");
    }

    public async Task<IEnumerable<CompetenciesTaxonomyList>> GetCompetenceTaxonomyList(OSParameter OSParameter)
    {
        const string query =
            @"SELECT CompetenciesTaxonomy.Id,CompetenciesTaxonomy.CompetenciesId,CompetenciesTaxonomy.ParentId,CompetenciesTaxonomy.CompetenciesTaxonomyLevelId,CompetenciesTaxonomy.Title,CompetenciesTaxonomy.ParentCompetenciesId,CompetenciesTaxonomyLevel.DisplayOrder,CompetenciesTaxonomyLevel.IsChildren FROM dbo.CompetenciesTaxonomy LEFT OUTER JOIN dbo.CompetenciesTaxonomyLevel ON CompetenciesTaxonomy.CompetenciesTaxonomyLevelId = CompetenciesTaxonomyLevel.LevelId WHERE(CompetenciesTaxonomyLevel.LanguageCode = @lang OR CompetenciesTaxonomy.CompetenciesTaxonomyLevelId IS NULL)";

        var sb = new StringBuilder(query);

        if (OSParameter.TaxonomyFilter.CompetenciesId != null)
            sb.Append(" AND CompetenciesTaxonomy.CompetenciesId = '" + OSParameter.TaxonomyFilter.CompetenciesId +
                      "'");

        var parameters = new { lang = OSParameter.Lang };
        IEnumerable<CompetenciesTaxonomyList> data;
        await using var connection = OSParameter.TenantProvider.orgSqlConnection();

        data = await connection.QueryAsync<CompetenciesTaxonomyList>(sb.ToString(), parameters);
        foreach (var mList in data)
            if (mList.CompetenciesTaxonomyLevelId == "4010e768-3e06-po02-b337-ee367a82addb")
                if (mList.CompetenciesId != null)
                {
                    mList.Competence = connection
                        .Query<GetOrganizationCompetencies>(
                            "SELECT * FROM OrganizationCompetencies WHERE Id = @Id",
                            new { Id = mList.CompetenciesId }).FirstOrDefault();
                    mList.Competence.ExperienceLevelName = connection.Query<string>(
                            "SELECT PbsExperienceLocalizedData.Label FROM dbo.PbsExperienceLocalizedData WHERE PbsExperienceLocalizedData.LanguageCode = @lang AND  PbsExperienceLocalizedData.PbsExperienceId = @PbsExperienceId",
                            new { PbsExperienceId = mList.Competence.ExperienceLevelId, lang = OSParameter.Lang })
                        .FirstOrDefault();
                }

        return data;
    }

    public async Task<IEnumerable<CompetenciesTaxonomyLevel>> GetCompetenceTaxonomyLevels(OSParameter OSParameter)
    {
        const string query =
            @"SELECT * FROM dbo.CompetenciesTaxonomyLevel WHERE LanguageCode = @lang ORDER BY DisplayOrder";
        IEnumerable<CompetenciesTaxonomyLevel> data;
        var parameters = new { lang = OSParameter.Lang };
        await using var connection = OSParameter.TenantProvider.orgSqlConnection();
        return connection.Query<CompetenciesTaxonomyLevel>(query, parameters);
        ;
    }

    public async Task<string> CreateOrganizationCertification(OSParameter OSParameter)
    {
        string OCESequenceId = null;
        OrganizationCertification data;

        await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

        data = connection
            .Query<OrganizationCertification>("SELECT * FROM OrganizationCertification WHERE Id = @Id ",
                new { OSParameter.OrganizationCertificationCreate.Id }).FirstOrDefault();


        if (data == null)
        {
            const string insertQuery =
                "INSERT INTO OrganizationCertification VALUES (@Id, @Name, @SequenceId, @Title, @Description, @QualificationDate, @QualificationOrganization, @StartDate, @EndDate, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate, @QualificationTypeId)";
            var idGenerator = new IdGenerator();
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext =
                new ApplicationDbContext(options, OSParameter.TenantProvider);
            OCESequenceId = idGenerator.GenerateId(applicationDbContext, "CERT-", "CERTSequence");
            var parameters = new
            {
                OSParameter.OrganizationCertificationCreate.Id,
                OSParameter.OrganizationCertificationCreate.Name,
                SequenceId = OCESequenceId,
                Title = OCESequenceId + " " + OSParameter.OrganizationCertificationCreate.Name,
                OSParameter.OrganizationCertificationCreate.Description,
                OSParameter.OrganizationCertificationCreate.QualificationDate,
                OSParameter.OrganizationCertificationCreate.QualificationOrganization,
                OSParameter.OrganizationCertificationCreate.StartDate,
                OSParameter.OrganizationCertificationCreate.EndDate,
                CreatedBy = OSParameter.UserId,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = OSParameter.UserId,
                ModifiedDate = DateTime.UtcNow,
                OSParameter.OrganizationCertificationCreate.QualificationTypeId
            };

            await connection.ExecuteAsync(insertQuery, parameters);
        }
        else
        {
            OCESequenceId = data.SequenceId;
            var updateQuery =
                @"UPDATE OrganizationCertification SET Name = @Name, Title = @Title, Description = @Description,QualificationDate =  @QualificationDate, QualificationOrganization = @QualificationOrganization, StartDate = @StartDate, EndDate = @EndDate, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate, QualificationTypeId = @QualificationTypeId WHERE Id = @Id";

            var parameters = new
            {
                OSParameter.OrganizationCertificationCreate.Id,
                OSParameter.OrganizationCertificationCreate.Name,
                Title = OCESequenceId + " " + OSParameter.OrganizationCertificationCreate.Name,
                OSParameter.OrganizationCertificationCreate.Description,
                OSParameter.OrganizationCertificationCreate.QualificationDate,
                OSParameter.OrganizationCertificationCreate.QualificationOrganization,
                OSParameter.OrganizationCertificationCreate.StartDate,
                OSParameter.OrganizationCertificationCreate.EndDate,
                ModifiedBy = OSParameter.UserId,
                ModifiedDate = DateTime.UtcNow,
                OSParameter.OrganizationCertificationCreate.QualificationTypeId
            };


            await connection.ExecuteAsync(updateQuery, parameters);
        }

        if (OSParameter.OrganizationCertificationCreate.QualificationTypeId != null)
        {
            var query =
                @"MERGE INTO dbo.CertificationTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE  SET Title = @Title, ParentId = @ParentId WHEN NOT MATCHED THEN INSERT (Id,CetificationId,ParentId,CertificationTaxonomyLevelId,Title, ParentCertificationId) VALUES (@Id,@CetificationId,@ParentId,@CertificationTaxonomyLevelId,@Title, @ParentCertificationId);";
            var parameters = new
            {
                OSParameter.OrganizationCertificationCreate.Id,
                Title = OSParameter.OrganizationCertificationCreate.Name,
                CetificationId = OSParameter.OrganizationCertificationCreate.Id,
                ParentId = OSParameter.OrganizationCertificationCreate.QualificationTypeId,
                CertificationTaxonomyLevelId = "oo10e768-3e06-po02-b337-ee367a82adoo",
                ParentCertificationId = OSParameter.OrganizationCertificationCreate.Id
            };


            await connection.ExecuteAsync(query, parameters);
        }

        return OCESequenceId;
    }

    public async Task<IEnumerable<GetOrganizationCertification>> GetOrganizationCertificationList(
        OSParameter OSParameter)
    {
        const string query = @"SELECT * FROM OrganizationCertification";

        IEnumerable<GetOrganizationCertification> data;
        var parameters = new { lang = OSParameter.Lang };
        await using var connection = OSParameter.TenantProvider.orgSqlConnection();
        return await connection.QueryAsync<GetOrganizationCertification>(query, parameters);
    }

    public async Task<GetOrganizationCertification> OrganizationCertificationGetById(OSParameter OSParameter)
    {
        try
        {
            const string query = @"SELECT * FROM [dbo].[OrganizationCertification] WHERE SequenceId = @Id";

            GetOrganizationCertification data;
            OrganizationCertificationHistory historyLog;
            QualificationOrganization organization = null;
            var parameters = new { OSParameter.Id };
            await using var connection = OSParameter.TenantProvider.orgSqlConnection();

            data = connection.Query<GetOrganizationCertification>(query, parameters).FirstOrDefault();
            historyLog = connection
                .Query<OrganizationCertificationHistory>(
                    "SELECT [CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM [dbo].[OrganizationCertification] where [SequenceId] = @Id",
                    new { OSParameter.Id }).FirstOrDefault();
            if (data != null)
                organization = connection
                    .Query<QualificationOrganization>(
                        "SELECT Id As [Key], Name As Text FROM CabCompany WHERE Id = @Id",
                        new { Id = data.QualificationOrganization }).FirstOrDefault();


            const string ModifiedByUserQuery =
                @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";


            if (historyLog != null)
            {
                var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
                historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                    .FirstOrDefault();


                var CreatByParam = new { oid = historyLog.CreatedBy };
                historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();
            }


            if (data != null)
            {
                data.History = historyLog;
                data.Organization = organization;
            }

            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<OrganizationCertificationDownData> GetOrganizationCertificationDropdown(
        OSParameter OSParameter)
    {
        const string query =
            @"SELECT * FROM dbo.CertificationTaxonomyLevel WHERE LanguageCode = @lang ORDER BY DisplayOrder";
        var DropDownData = new OrganizationCertificationDownData();
        var parameters = new { lang = OSParameter.Lang };

        await using var connection = OSParameter.TenantProvider.orgSqlConnection();
        DropDownData.CertificationTaxonomyLevel =
            connection.Query<CertificationTaxonomyLevel>(query, parameters);

        return DropDownData;
    }

    public async Task<string> CreateCertificationTaxonomy(OSParameter OSParameter)
    {
        if (OSParameter.CertificationTaxonomyDto.Id != null)
        {
            const string query =
                @"MERGE INTO dbo.CertificationTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE  SET Title = @Title WHEN NOT MATCHED THEN INSERT (Id,CetificationId,ParentId,CertificationTaxonomyLevelId,Title,ParentCertificationId ) VALUES (@Id,@CetificationId,@ParentId,@CertificationTaxonomyLevelId,@Title, @ParentCertificationId);";
            var parameters = new
            {
                OSParameter.CertificationTaxonomyDto.Id,
                OSParameter.CertificationTaxonomyDto.Title,
                OSParameter.CertificationTaxonomyDto.CetificationId,
                OSParameter.CertificationTaxonomyDto.ParentId,
                OSParameter.CertificationTaxonomyDto.CertificationTaxonomyLevelId,
                ParentCertificationId = OSParameter.CertificationTaxonomyDto.CetificationId
            };

            await using var connection = OSParameter.TenantProvider.orgSqlConnection();
            await connection.ExecuteAsync(query, parameters);
            return OSParameter.CertificationTaxonomyDto.Id;
        }

        throw new Exception("ID not set");
    }

    public async Task<IEnumerable<CertificationTaxonomyList>> GetCertificationTaxonomyList(OSParameter OSParameter)
    {
        const string query =
            @"SELECT CertificationTaxonomy.Id,CertificationTaxonomy.CetificationId,CertificationTaxonomy.ParentId,CertificationTaxonomy.CertificationTaxonomyLevelId,CertificationTaxonomy.Title ,CertificationTaxonomyLevel.DisplayOrder,CertificationTaxonomyLevel.IsChildren FROM dbo.CertificationTaxonomy LEFT OUTER JOIN dbo.CertificationTaxonomyLevel ON CertificationTaxonomy.CertificationTaxonomyLevelId = CertificationTaxonomyLevel.LevelId WHERE (CertificationTaxonomyLevel.LanguageCode = @lang OR CertificationTaxonomy.CertificationTaxonomyLevelId IS NULL)";

        var sb = new StringBuilder(query);

        if (OSParameter.TaxonomyFilter.CertificationId != null)
            sb.Append(" AND CertificationTaxonomy.CetificationId = '" + OSParameter.TaxonomyFilter.CertificationId +
                      "'");

        var parameters = new { lang = OSParameter.Lang };
        IEnumerable<CertificationTaxonomyList> data;
        await using var connection = OSParameter.TenantProvider.orgSqlConnection();
        data = await connection.QueryAsync<CertificationTaxonomyList>(sb.ToString(), parameters);
        foreach (var mList in data)
            if (mList.CertificationTaxonomyLevelId == "oo10e768-3e06-po02-b337-ee367a82adoo")

                mList.Certification = connection
                    .Query<GetOrganizationCertification>(
                        "SELECT Name, SequenceId, Title, StartDate, EndDate FROM OrganizationCertification WHERE Id = @Id",
                        new { Id = mList.CetificationId }).FirstOrDefault();


        return data;
    }


    public async Task<string> CreateOrganization(OSParameter OSParameter)
    {
        var options = new DbContextOptions<ApplicationDbContext>();
        var applicationDbContext =
            new ApplicationDbContext(options, OSParameter.TenantProvider);
        string ORGSequenceId = null;
        Organization data;

        await using var connection = OSParameter.TenantProvider.orgSqlConnection();

        data = connection.Query<Organization>("SELECT * FROM Organization WHERE Id = @Id ",
            new { OSParameter.OrganizationCreate.Id }).FirstOrDefault();


        if (data == null)
        {
            const string insertQuery =
                "INSERT INTO Organization VALUES (@Id, @Name, @SequenceId, @Title, @CreatedBy, @CreatedDate, @ModifiedBy, @ModifiedDate, @OrganizationTaxonomyId)";
            var idGenerator = new IdGenerator();
            ORGSequenceId = idGenerator.GenerateId(applicationDbContext, "ORG-", "ORGSequence");
            var parameters = new
            {
                OSParameter.OrganizationCreate.Id,
                OSParameter.OrganizationCreate.Name,
                SequenceId = ORGSequenceId,
                Title = ORGSequenceId + " " + OSParameter.OrganizationCreate.Name,
                CreatedBy = OSParameter.UserId,
                CreatedDate = DateTime.UtcNow,
                ModifiedBy = OSParameter.UserId,
                ModifiedDate = DateTime.UtcNow,
                OSParameter.OrganizationCreate.OrganizationTaxonomyId
            };
            await connection.ExecuteAsync(insertQuery, parameters);
        }
        else
        {
            ORGSequenceId = data.SequenceId;
            const string updateQuery =
                @"UPDATE Organization SET Name = @Name, Title = @Title, ModifiedBy = @ModifiedBy, ModifiedDate = @ModifiedDate, OrganizationTaxonomyId = @OrganizationTaxonomyId WHERE Id = @Id";

            var parameters = new
            {
                OSParameter.OrganizationCreate.Id,
                OSParameter.OrganizationCreate.Name,
                Title = ORGSequenceId + " " + OSParameter.OrganizationCreate.Name,
                ModifiedBy = OSParameter.UserId,
                ModifiedDate = DateTime.UtcNow,
                OSParameter.OrganizationCreate.OrganizationTaxonomyId
            };


            await connection.ExecuteAsync(updateQuery, parameters);
        }

        if (OSParameter.OrganizationCreate.OrganizationTaxonomyId != null)
        {
            const string query =
                @"MERGE INTO dbo.OrganizationTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE  SET Title = @Title WHEN NOT MATCHED THEN INSERT (Id,OrganizationId,ParentId,OrganizationTaxonomyLevelId,Title ) VALUES (@Id,@OrganizationId,@ParentId,@OrganizationTaxonomyLevelId,@Title);";
            var parameters = new
            {
                OSParameter.OrganizationCreate.Id,
                Title = OSParameter.OrganizationCreate.Name,
                OrganizationId = OSParameter.OrganizationCreate.Id,
                ParentId = OSParameter.OrganizationCreate.OrganizationTaxonomyId,
                OrganizationTaxonomyLevelId = "yr10e768-3e06-po02-b337-ee367a82adjh"
            };


            await connection.ExecuteAsync(query, parameters);
        }


        return ORGSequenceId;
    }

    public async Task<IEnumerable<GetOrganization>> GetOrganizationList(OSParameter OSParameter)
    {
        const string query = @"SELECT * FROM Organization";

        var parameters = new { lang = OSParameter.Lang };
        await using var connection = OSParameter.TenantProvider.orgSqlConnection();
        return await connection.QueryAsync<GetOrganization>(query, parameters);
        ;
    }

    public async Task<GetOrganization> OrganizationGetById(OSParameter OSParameter)
    {
        const string query = @"SELECT * FROM [dbo].[Organization] WHERE Id = @Id";

        GetOrganization data;
        OrganizationHistory historyLog;

        var parameters = new { OSParameter.Id };
        await using var connection = OSParameter.TenantProvider.orgSqlConnection();

        data = connection.Query<GetOrganization>(query, parameters).FirstOrDefault();
        historyLog = connection
            .Query<OrganizationHistory>(
                "SELECT [CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM [dbo].[Organization] where [Id] = @Id",
                new { OSParameter.Id }).FirstOrDefault();


        const string ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";


        if (historyLog != null)
        {
            var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
            historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                .FirstOrDefault();


            var CreatByParam = new { oid = historyLog.CreatedBy };
            historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();
        }


        if (data != null) data.History = historyLog;

        return data;
    }


    public async Task<string> CreateOrganizationTaxonomy(OSParameter OSParameter)
    {
        if (OSParameter.OrganizationTaxonomyDto.Id != null)
        {
            if (OSParameter.OrganizationTaxonomyDto.OrganizationTaxonomyLevelId ==
                "oo10e768-3e06-po02-b337-ee367a82admn")
            {
                string bu = null;
                await using (var connection =
                             new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString))
                {
                    bu = connection.Query<string>("SELECT BuSequenceId FROM dbo.OrganizationTaxonomy WHERE Id = @Id",
                        new { OSParameter.OrganizationTaxonomyDto.Id }).FirstOrDefault();
                }

                if (bu == null)
                {
                    var options = new DbContextOptions<ApplicationDbContext>();
                    var applicationDbContext = new ApplicationDbContext(options, OSParameter.TenantProvider);

                    var idGenerator = new IdGenerator();
                    OSParameter.OrganizationTaxonomyDto.BuSequenceId =
                        idGenerator.GenerateId(applicationDbContext, "BU-", "BUSequence");
                }

                OSParameter.OrganizationTaxonomyDto.Title = OSParameter.OrganizationTaxonomyDto.BuSequenceId + " " +
                                                            OSParameter.OrganizationTaxonomyDto.BuName + " " + "(" +
                                                            OSParameter.OrganizationTaxonomyDto.PersonName + ")";
            }

            const string query =
                @"MERGE INTO dbo.OrganizationTaxonomy t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id) WHEN MATCHED THEN UPDATE  SET Title = @Title,PersonId = @PersonId, StartDate = @StartDate, EndDate = @EndDate, RoleId = @RoleId, ModifiedDate = @ModifiedDate, BuSequenceId = @BuSequenceId, BuName = @BuName WHEN NOT MATCHED THEN INSERT (Id,OrganizationId,ParentId,OrganizationTaxonomyLevelId,Title, PersonId,StartDate,EndDate,RoleId,ModifiedDate,BuSequenceId,BuName) VALUES (@Id,@OrganizationId,@ParentId,@OrganizationTaxonomyLevelId,@Title,@PersonId,@StartDate,@EndDate,@RoleId,@ModifiedDate,@BuSequenceId,@BuName);";

            var parameters = new
            {
                OSParameter.OrganizationTaxonomyDto.Id,
                OSParameter.OrganizationTaxonomyDto.Title,
                OSParameter.OrganizationTaxonomyDto.OrganizationId,
                OSParameter.OrganizationTaxonomyDto.ParentId,
                OSParameter.OrganizationTaxonomyDto.OrganizationTaxonomyLevelId,
                OSParameter.OrganizationTaxonomyDto.PersonId,
                OSParameter.OrganizationTaxonomyDto.StartDate,
                OSParameter.OrganizationTaxonomyDto.EndDate,
                OSParameter.OrganizationTaxonomyDto.RoleId,
                ModifiedDate = DateTime.UtcNow,
                OSParameter.OrganizationTaxonomyDto.BuSequenceId,
                OSParameter.OrganizationTaxonomyDto.BuName
            };

            var updatequery =
                @"UPDATE [dbo].[HRHeader] SET [OrganizationTaxonomyId] = @OrganizationTaxonomyId WHERE PersonId = @PersonId";

            await using (var connection =
                         new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString))
            {
                await connection.ExecuteAsync(query, parameters);

                if (OSParameter.OrganizationTaxonomyDto.PersonId != null)
                {
                    await connection.ExecuteAsync(updatequery,
                        new
                        {
                            OSParameter.OrganizationTaxonomyDto.PersonId,
                            OrganizationTaxonomyId = OSParameter.OrganizationTaxonomyDto.Id
                        });
                }
            }

            return OSParameter.OrganizationTaxonomyDto.Id;
        }

        throw new Exception("ID not set");
    }

    public async Task<IEnumerable<OrganizationTaxonomyList>> GetOrganizationTaxonomyList(OSParameter OSParameter)
    {
        const string query = @"SELECT
              OrganizationTaxonomy.Id
             ,OrganizationTaxonomy.OrganizationId
             ,OrganizationTaxonomy.ParentId
             ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
             ,OrganizationTaxonomy.PersonId
             ,OrganizationTaxonomy.StartDate
             ,OrganizationTaxonomy.EndDate
             ,CONCAT(OrganizationTaxonomy.Title, ' (', OrganizationTaxonomyLevel.Name, ')') AS Title
             ,OrganizationTaxonomy.RoleId
             ,OrganizationTaxonomyLevel.DisplayOrder
             ,OrganizationTaxonomyLevel.IsChildren
             ,OrganizationTeamRole.RoleId AS RoleId
             ,OrganizationTaxonomy.IsDefaultBu
             ,OrganizationTaxonomy.BuName
             ,OrganizationTaxonomy.BuSequenceId
            FROM dbo.OrganizationTaxonomy
            LEFT OUTER JOIN dbo.OrganizationTaxonomyLevel
              ON OrganizationTaxonomy.OrganizationTaxonomyLevelId = OrganizationTaxonomyLevel.LevelId
            LEFT OUTER JOIN dbo.OrganizationTeamRole
              ON OrganizationTaxonomy.RoleId = OrganizationTeamRole.RoleId
            WHERE (OrganizationTaxonomyLevel.LanguageCode = @lang
            OR OrganizationTaxonomy.OrganizationTaxonomyLevelId IS NULL)
            AND (OrganizationTeamRole.LanguageCode = @lang
            OR OrganizationTaxonomy.RoleId IS NULL)
            AND (OrganizationTaxonomy.TemporaryTeamNameId != '7bcb4e8d-8e8c-487d-team-6b91c89fAcce' OR OrganizationTaxonomy.TemporaryTeamNameId IS NULL)";

        var sb = new StringBuilder(query);


        if (OSParameter.Filter.OrganizationTaxonomyLevelId != null)
            switch (OSParameter.Filter.OrganizationTaxonomyLevelId)
            {
                case "2210e768-3e06-po02-b337-ee367a82adjj":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk')");
                    break;
                case "oo10e768-3e06-po02-b337-ee367a82admn":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj')");
                    break;
                case "1210e768-3e06-po02-b337-ee367a82ad12":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn')");
                    break;
                case "fg10e768-3e06-po02-b337-ee367a82adfg":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn','1210e768-3e06-po02-b337-ee367a82ad12')");
                    break;
                case "we10e768-3e06-po02-b337-ee367a82adwe":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn','1210e768-3e06-po02-b337-ee367a82ad12','fg10e768-3e06-po02-b337-ee367a82adfg')");
                    break;
            }


        var parameters = new { lang = OSParameter.Lang };
        IEnumerable<OrganizationTaxonomyList> data;
        await using var connection =
            new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

        await connection.OpenAsync();

        data = await connection.QueryAsync<OrganizationTaxonomyList>(sb.ToString(), parameters);


        var selectBu = @"with name_tree as
                                        (SELECT
                                          OrganizationTaxonomy.Id
                                         ,OrganizationTaxonomy.Title
                                         ,OrganizationTaxonomy.ParentId
                                         ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
                                        FROM dbo.OrganizationTaxonomy
                                        WHERE OrganizationTaxonomy.Id = @Id
                                          UNION ALL
                                          SELECT c.Id, c.Title,c.ParentId,c.OrganizationTaxonomyLevelId
                                          FROM dbo.OrganizationTaxonomy c
                                          JOIN name_tree p on p.ParentId = c.Id)
                                          select OrganizationTaxonomyLevelId, Title, Id,ParentId
                                          from name_tree
                                          where OrganizationTaxonomyLevelId = 'oo10e768-3e06-po02-b337-ee367a82admn'";
        foreach (var mList in data)
        {
            if (mList.OrganizationTaxonomyLevelId == "yr10e768-3e06-po02-b337-ee367a82adjh")
                mList.Organization = connection
                    .Query<GetOrganization>("SELECT Name, SequenceId, Title  FROM Organization WHERE Id = @Id",
                        new { Id = mList.OrganizationId }).FirstOrDefault();
            else if (mList.OrganizationTaxonomyLevelId == "oo10e768-3e06-po02-b337-ee367a82admn")
                if (mList.PersonId != null)
                    mList.PersonName = connection
                        .Query<string>(
                            "SELECT CabPerson.FullName FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Id = @Id ",
                            new { Id = mList.PersonId }).FirstOrDefault();

            var bu = connection.Query<OrganizationTaxonomy>(selectBu, new { mList.Id }).FirstOrDefault();
            if (bu != null) mList.BuId = bu.Id;
        }

        return data;
    }

    public async Task<IEnumerable<OrganizationTaxonomyListForProjectPlan>> GetOrganizationTaxonomyListForProjectPlan(
        OSParameter OSParameter)
    {
        const string query =
            @"SELECT
        OrganizationTaxonomy.Id
            ,OrganizationTaxonomy.OrganizationId
            ,OrganizationTaxonomy.ParentId
            ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
            ,OrganizationTaxonomy.PersonId AS CabPersonCompanyId
            ,OrganizationTaxonomy.StartDate
            ,OrganizationTaxonomy.EndDate
            ,CONCAT(OrganizationTaxonomy.Title, ' (', OrganizationTaxonomyLevel.Name, ')') AS Title
            ,OrganizationTaxonomy.RoleId
            ,OrganizationTaxonomyLevel.DisplayOrder
            ,OrganizationTaxonomyLevel.IsChildren
            ,OrganizationTeamRole.RoleId AS RoleId
            ,OrganizationTaxonomy.IsDefaultBu
            ,OrganizationTaxonomy.BuName
            ,OrganizationTaxonomy.BuSequenceId
            ,CabPersonCompany.PersonId AS CabPersonId
            FROM dbo.OrganizationTaxonomy
            LEFT OUTER JOIN dbo.OrganizationTaxonomyLevel
            ON OrganizationTaxonomy.OrganizationTaxonomyLevelId = OrganizationTaxonomyLevel.LevelId
        LEFT OUTER JOIN dbo.OrganizationTeamRole
            ON OrganizationTaxonomy.RoleId = OrganizationTeamRole.RoleId
        LEFT OUTER JOIN dbo.CabPersonCompany
            ON OrganizationTaxonomy.PersonId = CabPersonCompany.Id
        WHERE (OrganizationTaxonomyLevel.LanguageCode = @lang
        OR OrganizationTaxonomy.OrganizationTaxonomyLevelId IS NULL)
        AND (OrganizationTeamRole.LanguageCode = @lang
        OR OrganizationTaxonomy.RoleId IS NULL)";
        var sb = new StringBuilder(query);


        if (OSParameter.Filter.OrganizationTaxonomyLevelId != null)
            switch (OSParameter.Filter.OrganizationTaxonomyLevelId)
            {
                case "2210e768-3e06-po02-b337-ee367a82adjj":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk')");
                    break;
                case "oo10e768-3e06-po02-b337-ee367a82admn":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj')");
                    break;
                case "1210e768-3e06-po02-b337-ee367a82ad12":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn')");
                    break;
                case "fg10e768-3e06-po02-b337-ee367a82adfg":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn','1210e768-3e06-po02-b337-ee367a82ad12')");
                    break;
                case "we10e768-3e06-po02-b337-ee367a82adwe":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn','1210e768-3e06-po02-b337-ee367a82ad12','fg10e768-3e06-po02-b337-ee367a82adfg')");
                    break;
            }


        var parameters = new { lang = OSParameter.Lang };
        IEnumerable<OrganizationTaxonomyListForProjectPlan> data;
        await using var connection =
            new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

        await connection.OpenAsync();

        data = await connection.QueryAsync<OrganizationTaxonomyListForProjectPlan>(sb.ToString(), parameters);


        var selectBu = @"with name_tree as
                                        (SELECT
                                          OrganizationTaxonomy.Id
                                         ,OrganizationTaxonomy.Title
                                         ,OrganizationTaxonomy.ParentId
                                         ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
                                        FROM dbo.OrganizationTaxonomy
                                        WHERE OrganizationTaxonomy.Id = @Id
                                          UNION ALL
                                          SELECT c.Id, c.Title,c.ParentId,c.OrganizationTaxonomyLevelId
                                          FROM dbo.OrganizationTaxonomy c
                                          JOIN name_tree p on p.ParentId = c.Id)
                                          select OrganizationTaxonomyLevelId, Title, Id,ParentId
                                          from name_tree
                                          where OrganizationTaxonomyLevelId = 'oo10e768-3e06-po02-b337-ee367a82admn'";
        foreach (var mList in data)
        {
            if (mList.OrganizationTaxonomyLevelId == "yr10e768-3e06-po02-b337-ee367a82adjh")
                mList.Organization = connection
                    .Query<GetOrganization>("SELECT Name, SequenceId, Title  FROM Organization WHERE Id = @Id",
                        new { Id = mList.OrganizationId }).FirstOrDefault();
            else if (mList.OrganizationTaxonomyLevelId == "oo10e768-3e06-po02-b337-ee367a82admn")
                if (mList.CabPersonCompanyId != null)
                {
                    var mCabPersonCompany = connection.Query<CabPersonOrganizationTaxonomy>(
                        "SELECT CabPerson.FullName,CabPersonCompany.PersonId AS Id FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Id = @Id ",
                        new { Id = mList.CabPersonCompanyId }).FirstOrDefault();
                    mList.PersonName = mCabPersonCompany?.FullName;
                }

            var bu = connection.Query<OrganizationTaxonomy>(selectBu, new { mList.Id }).FirstOrDefault();
            if (bu != null) mList.BuId = bu.Id;
        }

        return data;
    }

    public async Task<IEnumerable<OrganizationTaxonomyList>> GetOrganizationTaxonomyListForMyCalender(
        OSParameter OSParameter)
    {
        const string query = @"SELECT
              OrganizationTaxonomy.Id
             ,OrganizationTaxonomy.OrganizationId
             ,OrganizationTaxonomy.ParentId
             ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
             ,OrganizationTaxonomy.PersonId
             ,OrganizationTaxonomy.StartDate
             ,OrganizationTaxonomy.EndDate
             ,CONCAT(OrganizationTaxonomy.Title, ' (', OrganizationTaxonomyLevel.Name, ')') AS Title
             ,OrganizationTaxonomy.RoleId
             ,OrganizationTaxonomyLevel.DisplayOrder
             ,OrganizationTaxonomyLevel.IsChildren
             ,OrganizationTeamRole.RoleId AS RoleId
             ,OrganizationTaxonomy.IsDefaultBu
             ,OrganizationTaxonomy.BuName
             ,OrganizationTaxonomy.BuSequenceId
            FROM dbo.OrganizationTaxonomy
            LEFT OUTER JOIN dbo.OrganizationTaxonomyLevel
              ON OrganizationTaxonomy.OrganizationTaxonomyLevelId = OrganizationTaxonomyLevel.LevelId
            LEFT OUTER JOIN dbo.OrganizationTeamRole
              ON OrganizationTaxonomy.RoleId = OrganizationTeamRole.RoleId
            WHERE (OrganizationTaxonomyLevel.LanguageCode = @lang
            OR OrganizationTaxonomy.OrganizationTaxonomyLevelId IS NULL)
            AND (OrganizationTeamRole.LanguageCode = @lang
            OR OrganizationTaxonomy.RoleId IS NULL)
            AND (OrganizationTaxonomy.TemporaryTeamNameId != '7bcb4e8d-8e8c-487d-team-6b91c89fAcce' OR OrganizationTaxonomy.TemporaryTeamNameId IS NULL)";

        var sb = new StringBuilder(query);


        if (OSParameter.Filter.OrganizationTaxonomyLevelId != null)
            switch (OSParameter.Filter.OrganizationTaxonomyLevelId)
            {
                case "2210e768-3e06-po02-b337-ee367a82adjj":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk')");
                    break;
                case "oo10e768-3e06-po02-b337-ee367a82admn":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj')");
                    break;
                case "1210e768-3e06-po02-b337-ee367a82ad12":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn')");
                    break;
                case "fg10e768-3e06-po02-b337-ee367a82adfg":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn','1210e768-3e06-po02-b337-ee367a82ad12')");
                    break;
                case "we10e768-3e06-po02-b337-ee367a82adwe":
                    sb.Append(
                        " AND OrganizationTaxonomy.OrganizationTaxonomyLevelId NOT IN ('qq282458-0b40-poa3-b0f9-c2e40344c8kk','2210e768-3e06-po02-b337-ee367a82adjj','oo10e768-3e06-po02-b337-ee367a82admn','1210e768-3e06-po02-b337-ee367a82ad12','fg10e768-3e06-po02-b337-ee367a82adfg')");
                    break;
            }


        var parameters = new { lang = OSParameter.Lang };
        IEnumerable<OrganizationTaxonomyList> data;
        await using var connection =
            new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

        await connection.OpenAsync();

        data = await connection.QueryAsync<OrganizationTaxonomyList>(sb.ToString(), parameters);


        var selectBu = @"with name_tree as
                                        (SELECT
                                          OrganizationTaxonomy.Id
                                         ,OrganizationTaxonomy.Title
                                         ,OrganizationTaxonomy.ParentId
                                         ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
                                        FROM dbo.OrganizationTaxonomy
                                        WHERE OrganizationTaxonomy.Id = @Id
                                          UNION ALL
                                          SELECT c.Id, c.Title,c.ParentId,c.OrganizationTaxonomyLevelId
                                          FROM dbo.OrganizationTaxonomy c
                                          JOIN name_tree p on p.ParentId = c.Id)
                                          select OrganizationTaxonomyLevelId, Title, Id,ParentId
                                          from name_tree
                                          where OrganizationTaxonomyLevelId = 'oo10e768-3e06-po02-b337-ee367a82admn'";
        foreach (var mList in data)
        {
            if (mList.OrganizationTaxonomyLevelId == "yr10e768-3e06-po02-b337-ee367a82adjh")
                mList.Organization = connection
                    .Query<GetOrganization>("SELECT Name, SequenceId, Title  FROM Organization WHERE Id = @Id",
                        new { Id = mList.OrganizationId }).FirstOrDefault();
            else if (mList.OrganizationTaxonomyLevelId == "oo10e768-3e06-po02-b337-ee367a82admn")
                if (mList.PersonId != null)
                    mList.PersonName = connection
                        .Query<string>(
                            "SELECT CabPerson.FullName FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Id = @Id ",
                            new { Id = mList.PersonId }).FirstOrDefault();

            var bu = connection.Query<OrganizationTaxonomy>(selectBu, new { mList.Id }).FirstOrDefault();
            if (bu != null) mList.BuId = bu.Id;
        }

        return data;
    }

    public async Task<IEnumerable<OrganizationTaxonomyLevel>> GetOrganizationTaxonomyLevel(OSParameter OSParameter)
    {
        const string query =
            @"SELECT * FROM dbo.OrganizationTaxonomyLevel WHERE LanguageCode = @lang ORDER BY DisplayOrder";

        var parameters = new { lang = OSParameter.Lang };
        await using var connection = OSParameter.TenantProvider.orgSqlConnection();
        return connection.Query<OrganizationTaxonomyLevel>(query, parameters);
        ;
    }

    public async Task<string> CreateCorporateShedule(OSParameter OSParameter)
    {
        string SequenceId = null;

        if (OSParameter.CSDto.Id != null)
        {
            var checkquery = @"SELECT * FROM dbo.CorporateShedule WHERE Id = @Id";

            var checkparm = new { OSParameter.CSDto.Id };
            CorporateShedule data;

            await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

            data = connection.Query<CorporateShedule>(checkquery, checkparm).FirstOrDefault();


            if (data == null)
            {
                var options = new DbContextOptions<ApplicationDbContext>();
                var applicationDbContext = new ApplicationDbContext(options, OSParameter.TenantProvider);
                var idGenerator = new IdGenerator();
                SequenceId = idGenerator.GenerateId(applicationDbContext, "CS-", "CSSequence");

                var insertquery =
                    @"INSERT INTO dbo.CorporateShedule (Id,Name,SequenceId,Title,CreatedDate,ModifiedDate,CreatedBy,IsDefault) VALUES (@Id, @Name, @SequenceId, @Title, @CreatedDate,@ModifiedDate,@CreatedBy,@IsDefault)";

                var parm = new
                {
                    OSParameter.CSDto.Id,
                    SequenceId,
                    OSParameter.CSDto.Name,
                    Title = SequenceId + " " + OSParameter.CSDto.Name,
                    CreatedBy = OSParameter.UserId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    OSParameter.CSDto.IsDefault
                };


                await connection.ExecuteAsync(insertquery, parm);
            }

            else
            {
                SequenceId = OSParameter.CSDto.SequenceId;

                var updatequery =
                    @"UPDATE dbo.CorporateShedule SET Id = @Id,Name = @Name,SequenceId = @SequenceId,Title = @Title,ModifiedDate = @ModifiedDate,ModifiedBy = @ModifiedBy, IsDefault=@IsDefault WHERE Id = @Id";

                var parm2 = new
                {
                    OSParameter.CSDto.Id,
                    SequenceId,
                    OSParameter.CSDto.Name,
                    Title = SequenceId + " " + OSParameter.CSDto.Name,
                    ModifiedBy = OSParameter.UserId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    OSParameter.CSDto.IsDefault
                };


                await connection.ExecuteAsync(updatequery, parm2);
            }

            foreach (var time in OSParameter.CSDto.CorporateSheduleTime)
            {
                var timequery = @"MERGE INTO dbo.CorporateSheduleTime
                                            t1 USING (SELECT 1 id) t2 ON (t1.Id = @Id)
                                            WHEN MATCHED THEN UPDATE SET [Day] = @Day, [StartTime] = @StartTime,[EndTime] = @EndTime,[DisplayOrder] = @DisplayOrder,[CorporateSheduleId] = @CorporateSheduleId
                                            WHEN NOT MATCHED THEN INSERT ([Id],[Day],[StartTime],[EndTime],[DisplayOrder],[CorporateSheduleId])
                                            VALUES (@Id, @Day,@StartTime,@EndTime,@DisplayOrder,@CorporateSheduleId);";

                var timeparm = new
                {
                    time.Id,
                    time.Day,
                    time.StartTime,
                    time.EndTime,
                    time.DisplayOrder,
                    time.CorporateSheduleId
                };


                await connection.ExecuteAsync(timequery, timeparm);
            }
        }

        if (OSParameter.CSDto.IsDefault)
        {
            const string IsDefault = @"UPDATE dbo.CorporateShedule SET IsDefault = 0 WHERE Id NOT IN (@Id)";

            var db = new List<ProjectDefinition>();
            await using (var connection =
                         new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString))
            {
                await connection.ExecuteAsync(IsDefault, new { OSParameter.CSDto.Id });
                db = connection.Query<ProjectDefinition>(@"SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0")
                    .ToList();
            }

            var teamsWithPmol = new List<TeamsWithPmolDto>();
            IEnumerable<AvailableLabour> labour = null;
            IEnumerable<PomlVehicle> tool = null;

            foreach (var pConnectionString in db.Select(project => project.ProjectConnectionString)
                         .Where(pconnectionString => pconnectionString != null))
            {
                const string pmol =
                    @"SELECT PMol.ExecutionDate,PMol.Id,PMol.ExecutionStartTime,PMol.ExecutionEndTime FROM dbo.PMol WHERE ExecutionDate IS NOT NULL AND( PMol.ExecutionStartTime IS NULL OR PMol.ExecutionEndTime IS NULL OR ExecutionDate > @date)";

                IEnumerable<Pmol> Pmols;

                await using (var connection = new SqlConnection(pConnectionString))
                {
                    Pmols = connection.Query<Pmol>(pmol, new { date = DateTime.UtcNow });
                }

                foreach (var a in Pmols)
                {
                    if (a.ExecutionStartTime == null)
                    {
                        const string update =
                            @"UPDATE dbo.PMol SET ExecutionStartTime = @ExecutionStartTime WHERE Id = @Id";

                        var eDate = a.ExecutionDate.Value.DayOfWeek;

                        var stime = OSParameter.CSDto.CorporateSheduleTime.Where(i =>
                            i.Day == eDate.ToString());

                        await using (var connection = new SqlConnection(pConnectionString))
                        {
                            await connection.ExecuteAsync(update,
                                new { a.Id, ExecutionStartTime = stime.Select(i => i.StartTime) });
                        }
                    }

                    if (a.ExecutionEndTime == null)
                    {
                        const string update =
                            @"UPDATE dbo.PMol SET ExecutionEndTime = @ExecutionEndTime WHERE Id = @Id";

                        var eDate = a.ExecutionDate.Value.DayOfWeek;

                        var stime = OSParameter.CSDto.CorporateSheduleTime.Where(i =>
                            i.Day == eDate.ToString());

                        await using (var connection = new SqlConnection(pConnectionString))
                        {
                            await connection.ExecuteAsync(update,
                                new { a.Id, ExecutionEndTime = stime.Select(i => i.EndTime) });
                        }
                    }
                }
            }
        }

        return SequenceId;
    }


    public async Task<IEnumerable<CorporateShedule>> GetCorporateSheduleList(OSParameter OSParameter)
    {
        const string query = @"SELECT * FROM dbo.CorporateShedule";

        var sb = new StringBuilder(query);

        if (OSParameter.CSList.Title != null)
        {
            OSParameter.CSList.Title = OSParameter.CSList.Title.Replace("'", "''");

            var words = OSParameter.CSList.Title.Split(" ");
            foreach (var element in words) sb.Append(" where CorporateShedule.Title LIKE '%" + element + "%'");
        }

        if (OSParameter.CSList.Sorter.Attribute == null) sb.Append(" ORDER BY CorporateShedule.SequenceId desc");

        if (OSParameter.CSList.Sorter.Attribute != null)
            if (OSParameter.CSList.Sorter.Attribute.ToLower().Equals("title"))
                sb.Append(" ORDER BY CorporateShedule.Title " + OSParameter.CSList.Sorter.Order);


        var parm = new { lang = OSParameter.Lang };

        IEnumerable<CorporateShedule> CorporateShedule;

        await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);
        CorporateShedule = await connection.QueryAsync<CorporateShedule>(sb.ToString(), parm);

        return CorporateShedule;
    }


    public async Task<CorporateSheduleDto> CorporateSheduleGetById(OSParameter OSParameter)
    {
        const string query = @"SELECT * FROM dbo.CorporateShedule WHERE SequenceId = @Id";

        var parm = new { OSParameter.Id };

        CorporateSheduleDto CorporateShedule = null;

        await using var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

        CorporateShedule = connection.Query<CorporateSheduleDto>(query, parm).FirstOrDefault();

        var timequery =
            @"SELECT * FROM dbo.CorporateSheduleTime WHERE CorporateSheduleId = @Id ORDER BY DisplayOrder ";

        var parameter = new { CorporateShedule.Id };

        CorporateShedule.CorporateSheduleTime =
            connection.Query<CorporateSheduleTimeDto>(timequery, parameter).ToList();


        CorporateSheduleHistory historyLog;

        var historyquery =
            @"SELECT [CreatedDate],[ModifiedDate],[CreatedBy],[ModifiedBy] FROM [dbo].[CorporateShedule] where SequenceId = @Id";

        var ModifiedByUserQuery =
            @"SELECT CONCAT(ApplicationUser.FirstName,' ',ApplicationUser.LastName) AS [ModifiedBy] FROM ApplicationUser WHERE ApplicationUser.Oid = @oid";


        historyLog = connection.Query<CorporateSheduleHistory>(historyquery, parm).FirstOrDefault();

        if (historyLog != null)
        {
            var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
            historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                .FirstOrDefault();


            var CreatByParam = new { oid = historyLog.CreatedBy };
            historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam).FirstOrDefault();

            historyLog.CreatedDate = historyLog.CreatedDate;
            historyLog.ModifiedDate = historyLog.ModifiedDate;
        }

        CorporateShedule.History = historyLog;

        return CorporateShedule;
    }

    public async Task<string> DeleteOrganizationTaxonomyNode(OSParameter OSParameter)
    {
        await using (var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString))
        {
            await connection.ExecuteAsync("DELETE FROM OrganizationTaxonomy WHERE Id = @Id ",
                new { OSParameter.Id });
        }

        return OSParameter.Id;
    }

    public async Task<IEnumerable<ProjectPersonFilterDto>> Filter(OSParameter OSParameter)
    {
       
        //CREATE VIEW CabFilter AS 
        var sql = @"SELECT
             CabPerson.IsSaved AS IsSaved
            ,CabCompany.Id AS CompanyId
            ,CabPerson.Id AS Id
            ,CabPerson.FullName AS FullName
            ,CabPersonCompany.Id AS Id
            ,CabPersonCompany.JobRole AS JobTitle
            ,CabEmail.EmailAddress AS Email
            ,CabMobilePhone.MobilePhoneNumber AS MobileNumber
            ,CabCompany.Id AS Id
            ,CabCompany.Name AS Name
            ,CabCompany.Name AS organisation

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
                WHERE SequenceId IS NOT NULL AND CabPerson.IsDeleted = 0";


        var sb = new StringBuilder(sql);
        var filter = OSParameter.organizationCabPersonFilter;
        if (filter != null)
        {
            if (filter.FullName != null)
            {
                filter.FullName = filter.FullName.Replace("'", "''");

                var words = filter.FullName.Split(" ");
                foreach (var element in words)
                    sb.Append(" AND CabPerson.FullName LIKE '%" + element + "%'");
                //sb.Append(" AND CorporateProductCatalog.Title  LIKE '%" + cpcParameters.filter.Title + "%'");
            }

            if (filter.MobileNumber != null)
                sb.Append(" AND CabMobilePhone.MobilePhoneNumber LIKE '%" + filter.MobileNumber + "%'");

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
                switch (filter.IsSaved.ToLower())
                {
                    case "true":
                        sb.Append(" AND CabPerson.IsSaved = 1");
                        break;
                    case "false":
                        sb.Append(" AND CabPerson.IsSaved = 0");
                        break;
                }
        }

        //organisation mobileNumber
        if (filter != null && filter.CabPersonSortingModel != null)
        {
            if (filter.CabPersonSortingModel.Attribute != null &&
                filter.CabPersonSortingModel.Order.Equals("asc"))
                sb.Append(" ORDER BY " + filter.CabPersonSortingModel.Attribute + " ASC");

            if (filter.CabPersonSortingModel.Attribute != null &&
                filter.CabPersonSortingModel.Order.Equals("desc"))
                sb.Append(" ORDER BY " + filter.CabPersonSortingModel.Attribute + " DESC");
        }

        var query2 = @"with name_tree as
            (SELECT
              OrganizationTaxonomy.Id
             ,OrganizationTaxonomy.Title
             ,OrganizationTaxonomy.ParentId
			 ,OrganizationTaxonomy.PersonId
             ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
            FROM dbo.OrganizationTaxonomy
            WHERE OrganizationTaxonomy.Id = (select OrganizationTaxonomy.Id FROM OrganizationTaxonomy WHERE (Id = @ComId))
              UNION ALL
              SELECT c.Id, c.Title,c.ParentId, c.PersonId,c.OrganizationTaxonomyLevelId
              FROM dbo.OrganizationTaxonomy c
              JOIN name_tree p on p.Id = c.ParentId)
              select OrganizationTaxonomyLevelId, Title, Id,ParentId, PersonId
              from name_tree
              where OrganizationTaxonomyLevelId = 'we10e768-3e06-po02-b337-ee367a82adwe'";

        // var sb2 = new StringBuilder(query2);
        //
        // var options = new DbContextOptions<ShanukaDbContext>();
        //
        // var connectionString = ConnectionString.MapConnectionString(
        //     OSParameter.ContractingUnitSequenceId, OSParameter.ProjectSequenceId,
        //     OSParameter.TenantProvider);
        //
        // using var context =
        //     new ShanukaDbContext(options, connectionString, OSParameter.TenantProvider);

        await using var connection = OSParameter.TenantProvider.orgSqlConnection();

        var result =
            await connection
                .QueryAsync<ProjectPersonFilterDto, ProjectPersonFilterPersonDto, ProjectPersonFilterPersonCompanyDto,
                    ProjectPersonFilterCompanyDto, ProjectPersonFilterDto>(sb.ToString(),
                    (cabData, personData, personCompany, company) =>
                    {
                        cabData.Person = personData;
                        cabData.PersonCompany = personCompany;
                        cabData.Company = company;
                        return cabData;
                    }, new { SequenceCode = OSParameter.ProjectSequenceId });

        // var parm = new { OSParameter.organizationCabPersonFilter.ComId };
        // var result2 = dbConnection.Query<OrganizationTaxonomyPerson>(query2, parm);
        //
        // 

        // if (comId != null)
        // {
        //
        //     var existingMembers = result2.ToList();
        //
        //     result = result
        //         .Where(x => existingMembers.All(c => c.PersonId != x.PersonCompany.Id)).ToList();
        // }

        return result;
    }

    public async Task<IEnumerable<ProjectPersonFilterDto>> PersonFilterForBu(OSParameter OSParameter)
    {
        var nProjectPersonFilterDto = new List<ProjectPersonFilterDto>();
        var mProjectPersonFilterDto = new List<ProjectPersonFilterPersonDto>();
        var projectSelect =
            @"SELECT * FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId WHERE ProjectClassificationBuisnessUnit = @BuID";

        await using (var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var mProjectDefinition = connection
                .Query<ProjectDefinition>(projectSelect, new { OSParameter.OrganizationCabPersonFilterDto.BuID })
                .ToList();

            foreach (var i in mProjectDefinition)
            {
                var cuId = OSParameter.OrganizationCabPersonFilterDto.ComId;
                var sql = @"SELECT DISTINCT 
                              CabPersonCompany.Id
                             ,CabPersonCompany.PersonId
                             ,CabPersonCompany.CompanyId
                             ,CabPersonCompany.JobRole AS JobTitle
                             ,CabPerson.FullName
                            FROM dbo.HRHeader h
                            LEFT OUTER JOIN dbo.CabPersonCompany
                              ON h.PersonId = CabPersonCompany.Id
                            INNER JOIN dbo.CabPerson
                              ON CabPersonCompany.PersonId = CabPerson.Id
                              WHERE h.PersonId IS NOT NULL";


                var sb = new StringBuilder(sql);
                var filter = OSParameter.OrganizationCabPersonFilterDto;
                if (filter != null)
                    if (filter.FullName != null)
                    {
                        filter.FullName = filter.FullName.Replace("'", "''");

                        var words = filter.FullName.Split(" ");
                        foreach (var element in words)
                            sb.Append(" AND CabPerson.FullName LIKE '%" + element + "%'");
                        //sb.Append(" AND CorporateProductCatalog.Title  LIKE '%" + cpcParameters.filter.Title + "%'");
                    }

                using IDbConnection dbConnection =
                    new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

                var existingMembers = new List<OrganizationTaxonomyPerson>();
                var result = dbConnection
                    .Query<ProjectPersonFilterPersonDto>(sb.ToString(), new { ProjectSequenceCode = i.SequenceCode })
                    .ToList();


                if (cuId != null)
                {
                    result = result.DistinctBy(c => c.PersonId).ToList();
                    mProjectPersonFilterDto.AddRange(result);
                }
            }
        }

        foreach (var i in mProjectPersonFilterDto)
        {
            var data = new ProjectPersonFilterDto();
            var data1 = new ProjectPersonFilterPersonCompanyDto
            {
                Id = i.Id
            };
            data.Person = i;
            data.PersonCompany = data1;
            nProjectPersonFilterDto.Add(data);
        }

        return nProjectPersonFilterDto;
    }

    public async Task<IEnumerable<ProjectPersonFilterDto>> PersonFilterForBuTeam(OSParameter OSParameter)
    {
        var nProjectPersonFilterDto = new HashSet<ProjectPersonFilterDto>();
        var mProjectPersonFilterDto = new List<ProjectPersonFilterPersonDto>();
        var projectSelect =
            @"SELECT * FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId WHERE ProjectClassificationBuisnessUnit = @BuID";

        await using (var connection = new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString))
        {
            var mProjectDefinition = connection
                .Query<ProjectDefinition>(projectSelect, new { OSParameter.OrganizationCabPersonFilterDto.BuID })
                .ToList();

            foreach (var i in mProjectDefinition)
            {
                var cuId = OSParameter.OrganizationCabPersonFilterDto.ComId;
                var sql = @"SELECT DISTINCT 
                              CabPersonCompany.Id
                             ,CabPersonCompany.PersonId
                             ,CabPersonCompany.CompanyId
                             ,CabPersonCompany.JobRole AS JobTitle
                             ,CabPerson.FullName
                            FROM dbo.HRHeader h
                            LEFT OUTER JOIN dbo.CabPersonCompany
                              ON h.PersonId = CabPersonCompany.Id
                            INNER JOIN dbo.CabPerson
                              ON CabPersonCompany.PersonId = CabPerson.Id
                              WHERE h.PersonId IS NOT NULL";


                var sb = new StringBuilder(sql);
                var filter = OSParameter.OrganizationCabPersonFilterDto;
                if (filter != null)
                    if (filter.FullName != null)
                    {
                        filter.FullName = filter.FullName.Replace("'", "''");

                        var words = filter.FullName.Split(" ");
                        foreach (var element in words)
                            sb.Append(" AND CabPerson.FullName LIKE '%" + element + "%'");
                        //sb.Append(" AND CorporateProductCatalog.Title  LIKE '%" + cpcParameters.filter.Title + "%'");
                    }

                var query2 = @"with name_tree as
                    (SELECT
                      OrganizationTaxonomy.Id
                     ,OrganizationTaxonomy.Title
                     ,OrganizationTaxonomy.ParentId
			         ,OrganizationTaxonomy.PersonId
                     ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
                    FROM dbo.OrganizationTaxonomy
                    WHERE OrganizationTaxonomy.Id = @TeamId
                      UNION ALL
                      SELECT c.Id, c.Title,c.ParentId, c.PersonId,c.OrganizationTaxonomyLevelId
                      FROM dbo.OrganizationTaxonomy c
                      JOIN name_tree p on p.Id = c.ParentId)
                      select OrganizationTaxonomyLevelId, Title, Id,ParentId, PersonId
                      from name_tree
                      where OrganizationTaxonomyLevelId = 'we10e768-3e06-po02-b337-ee367a82adwe'";

                using IDbConnection dbConnection =
                    new SqlConnection(OSParameter.TenantProvider.GetTenant().ConnectionString);

                var existingMembers = new List<OrganizationTaxonomyPerson>();
                var result = dbConnection
                    .Query<ProjectPersonFilterPersonDto>(sb.ToString(), new { ProjectSequenceCode = i.SequenceCode })
                    .ToList();
                var parm = new
                    { OSParameter.OrganizationCabPersonFilterDto.TeamId, ProjectSequenceCode = i.SequenceCode };
                existingMembers = dbConnection.Query<OrganizationTaxonomyPerson>(query2, parm).ToList();


                if (cuId != null)
                {
                    result = result
                        .Where(x => existingMembers.All(c => c.PersonId != x.Id)).ToList();
                    result = result.DistinctBy(c => c.PersonId).ToList();
                    mProjectPersonFilterDto.AddRange(result);
                }
            }
        }

        foreach (var i in mProjectPersonFilterDto)
        {
            var data = new ProjectPersonFilterDto();
            var data1 = new ProjectPersonFilterPersonCompanyDto
            {
                Id = i.Id
            };
            data.Person = i;
            data.PersonCompany = data1;
            nProjectPersonFilterDto.Add(data);
        }

        var returnData = nProjectPersonFilterDto.GroupBy(x => x.Person.PersonId).Select(x => x.First()).ToList();
        return returnData;
    }

    public async Task<string> OrganizationTaxonomySetDefaultBu(OSParameter OSParameter)
    {
        await using var connection = OSParameter.TenantProvider.orgSqlConnection();
        var selectCu = @"with name_tree as
            (SELECT
              OrganizationTaxonomy.Id
             ,OrganizationTaxonomy.Title
             ,OrganizationTaxonomy.ParentId
			 ,OrganizationTaxonomy.PersonId
             ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
            FROM dbo.OrganizationTaxonomy
            WHERE OrganizationTaxonomy.Id = @Id
              UNION ALL
              SELECT c.Id, c.Title,c.ParentId, c.PersonId,c.OrganizationTaxonomyLevelId
              FROM dbo.OrganizationTaxonomy c
              JOIN name_tree p on p.ParentId = c.Id)
              select OrganizationTaxonomyLevelId, Title, Id,ParentId, PersonId
              from name_tree
              where OrganizationTaxonomyLevelId = '2210e768-3e06-po02-b337-ee367a82adjj'";

        var cu = connection.Query<OrganizationTaxonomy>(selectCu, new { OSParameter.OrganizationTaxonomyBu.Id })
            .FirstOrDefault();

        if (cu != null)
        {
            var selectDefaultBu = @"with name_tree as
            (SELECT
              OrganizationTaxonomy.Id
             ,OrganizationTaxonomy.Title
             ,OrganizationTaxonomy.ParentId
			       ,OrganizationTaxonomy.PersonId
             ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
             ,OrganizationTaxonomy.IsDefaultBu
            FROM dbo.OrganizationTaxonomy
            WHERE OrganizationTaxonomy.Id = @Id
              UNION ALL
              SELECT c.Id, c.Title,c.ParentId, c.PersonId,c.OrganizationTaxonomyLevelId,c.IsDefaultBu
              FROM dbo.OrganizationTaxonomy c
              JOIN name_tree p on p.Id = c.ParentId)
              select OrganizationTaxonomyLevelId, Title, Id,ParentId, PersonId,IsDefaultBu
              from name_tree
              where OrganizationTaxonomyLevelId = 'oo10e768-3e06-po02-b337-ee367a82admn'
              AND IsDefaultBu = 1";
            var defaultBu = connection.Query<OrganizationTaxonomy>(selectDefaultBu, new { cu.Id }).FirstOrDefault();

            if (defaultBu != null)
            {
                var updateBu = @"UPDATE dbo.OrganizationTaxonomy SET IsDefaultBu = 0 WHERE Id = @Id;";
                connection.ExecuteAsync(updateBu, new { defaultBu.Id });
            }
        }

        connection.ExecuteAsync("UPDATE dbo.OrganizationTaxonomy SET IsDefaultBu = 1 WHERE Id = @Id;",
            new { OSParameter.OrganizationTaxonomyBu.Id });

        return OSParameter.OrganizationTaxonomyBu.Id;
    }
}