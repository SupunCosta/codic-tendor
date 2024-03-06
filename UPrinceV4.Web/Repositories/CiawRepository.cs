using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using ServiceStack;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CIAW;
using UPrinceV4.Web.Data.RFQ;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UPrinceV4.Web.Repositories;

public class CiawRepository : ICiawRepository
{
    public async Task<CiawCreateReturnDto> Create(CiawParameter CiawParameter)
    {
        var ciaw = new List<CiawHeader>();

        var selectProject = @"SELECT
                                  ProjectDefinition.Title
                                 ,ProjectDefinition.ProjectConnectionString
                                 ,ProjectDefinition.SequenceCode
                                 ,CabCompany.SequenceCode AS ContractingUnitId
                                 ,ProjectCiawSite.IsCiawEnabled
                                FROM dbo.ProjectDefinition
                                LEFT OUTER JOIN dbo.CabCompany
                                  ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                                LEFT OUTER JOIN dbo.ProjectClassification
                                  ON ProjectDefinition.Id = ProjectClassification.ProjectId
                                LEFT OUTER JOIN dbo.ProjectCiawSite
                                  ON ProjectDefinition.Id = ProjectCiawSite.ProjectId
                                WHERE ProjectClassification.ProjectClassificationBuisnessUnit = @BuId
                                ORDER BY ProjectDefinition.SequenceCode";

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var project = connection.Query<ProjectDefinition>(selectProject, new { CiawParameter.CiawCreateDto.BuId });

        var insert =
            @"INSERT INTO dbo.CiawHeader ( Id ,CabPersonId ,CiawStatus ,Date ,CreatedBy ,ModifiedBy ,CreatedDate ,ModifiedDate ,Project ,Reference ,PmolTeamRoleId,ContractingUnit,PmolId,CIAWReferenceId ) VALUES ( NEWID() ,@CabPersonId ,@CiawStatus ,@Date ,@CreatedBy ,@ModifiedBy ,@CreatedDate ,@ModifiedDate ,@Project ,@Reference ,@PmolTeamRoleId,@ContractingUnit,@Id,@CIAWReferenceId );";

        foreach (var i in project)
            if (i.IsCiawEnabled)
            {
                var cuconnectionString =
                    ConnectionString.MapConnectionString(i.ContractingUnitId, null, CiawParameter.TenantProvider);

                await using var cuconnection = new SqlConnection(cuconnectionString);

                await using var dbconnection = new SqlConnection(i.ProjectConnectionString);

                var pmol = dbconnection.Query<string>("SELECT Id FROM dbo.PMol WHERE ExecutionDate = @Date",
                    new { CiawParameter.CiawCreateDto.Date }).ToList();

                foreach (var n in pmol)
                {
                    var selectLabour =
                        @"INSERT INTO dbo.CiawHeader (Id, CabPersonId, CiawStatus, Date, CreatedBy, CreatedDate, Project, Reference, PmolTeamRoleId, ContractingUnit, PmolId, CIAWReferenceId)
                                          SELECT
                                            NEWID()
                                           ,PmolTeamRole.CabPersonId
                                           ,@CiawStatus
                                           ,PMol.ExecutionDate
                                           ,@CreatedBy
                                           ,@CreatedDate
                                           ,@Project
                                           ,@Reference
                                           ,PmolTeamRole.Id AS PmolTeamRoleId
                                           ,@ContractingUnit
                                           ,@PmolId
                                           ,ABS(CAST(CAST(NEWID() AS VARBINARY) AS INT))
                                          FROM dbo.PMolPlannedWorkLabour
                                          INNER JOIN dbo.PMol
                                            ON PMolPlannedWorkLabour.PmolId = PMol.Id
                                          INNER JOIN dbo.PmolTeamRole
                                            ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id
                                          WHERE PMol.Id = @PmolId
                                          AND PMolPlannedWorkLabour.IsDeleted = 0
                                          AND PmolTeamRole.IsDeleted = 0
                                          AND PmolTeamRole.CabPersonId NOT IN (SELECT
                                              CiawHeader.CabPersonId
                                            FROM CiawHeader
                                            WHERE CiawHeader.Date = @Date
                                            AND CiawHeader.CiawStatus NOT IN ('7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce')
                                            AND CiawHeader.Project = @Project)";

                    var param = new
                    {
                        //Id = Guid.NewGuid().ToString("N").Substring(0, 12),
                        CreatedBy = CiawParameter.UserId,
                        CiawStatus = "4010e768-3e06-4702-ciaws-ee367a82addb",
                        CiawParameter.CiawCreateDto.Date,
                        CreatedDate = DateTime.UtcNow,
                        Project = i.SequenceCode,
                        Reference = Guid.NewGuid(),
                        ContractingUnit = i.ContractingUnitId,
                        PmolId = n,
                        CIAWReferenceId = Guid.NewGuid().ToString("N").Substring(0, 12)
                    };

                    await dbconnection.ExecuteAsync(selectLabour, param);

                    var ciawSelect =
                        dbconnection.Query<CiawHeader>("SELECT * FROM dbo.CiawHeader where Reference = @Reference",
                            param);

                    foreach (var r in ciawSelect) await cuconnection.ExecuteAsync(insert, r);
                    ciaw.AddRange(ciawSelect);
                }
            }

        var cu = ciaw.GroupBy(e => e.ContractingUnit);

        var ciawCiawApproved = 0;
        foreach (var i in cu)
        {
            var cuconnectionString = ConnectionString.MapConnectionString(i.Key, null, CiawParameter.TenantProvider);
            await using var cuconnection = new SqlConnection(cuconnectionString);

            var CiawApproved = connection
                .Query<CiawHeader>(
                    "SELECT * FROM dbo.CiawHeader WHERE CiawStatus = '7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce' AND Date = @Date",new{Date = CiawParameter.CiawCreateDto.Date}).ToList();

            ciawCiawApproved = CiawApproved.Count() + ciawCiawApproved;
        }

        var ciawreturn = new CiawCreateReturnDto();

        ciawreturn.Date = CiawParameter.CiawCreateDto.Date;
        ciawreturn.CiawApproved = ciawCiawApproved.ToString();
        ciawreturn.CiawRequested = ciaw.Count.ToString();
        ciawreturn.RequestedBy = connection.Query<RequestedBy>(
            "SELECT CabPerson.Id AS RequestedPerson,CabPerson.FullName AS RequestedPersonName FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE Oid = @UserId",
            new { CiawParameter.UserId }).FirstOrDefault();

        return ciawreturn;
    }

    public async Task<IEnumerable<CiawHeaderFilterDto>> FilterCiaw(CiawParameter CiawParameter)
    {
        var cuconnectionString = ConnectionString.MapConnectionString(CiawParameter.ContractingUnitSequenceId, null,
            CiawParameter.TenantProvider);
        await using var cuconnection = new SqlConnection(cuconnectionString);

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var query = @"SELECT
                          CiawStatus.Name AS CiawStatus
                         ,CiawHeader.Id
                         ,CiawHeader.CabPersonId
                         ,CiawHeader.Date
                         ,CiawHeader.CreatedBy
                         ,CiawHeader.ModifiedBy
                         ,CiawHeader.CreatedDate
                         ,CiawHeader.ModifiedDate
                         ,CiawHeader.Project
                         ,CiawHeader.Reference
                         ,CiawHeader.CIAWReferenceId
                         ,CiawHeader.PmolTeamRoleId
                         ,CiawHeader.PresenceRegistrationId as PresenceRegistrationId
                        FROM dbo.CiawHeader
                        INNER JOIN dbo.CiawStatus
                          ON CiawHeader.CiawStatus = CiawStatus.StatusId
                        WHERE CiawStatus.LanguageCode = @lang ";

        var sb = new StringBuilder(query);

        if (CiawParameter.CiawFilter.Date != null)
            sb.Append("AND CiawHeader.Date = '" + CiawParameter.CiawFilter.Date + "' ");

        if (CiawParameter.CiawFilter.CiawStatus != null)
            sb.Append("AND CiawStatus.StatusId = '" + CiawParameter.CiawFilter.CiawStatus + "' ");

        if (CiawParameter.CiawFilter.Sorter.Attribute != null)
        {
            if (CiawParameter.CiawFilter.Sorter.Attribute.ToLower() == "date")
                sb.Append("ORDER BY Date " + CiawParameter.CiawFilter.Sorter.Order);
            if (CiawParameter.CiawFilter.Sorter.Attribute.ToLower() == "project")
                sb.Append("ORDER BY Project " + CiawParameter.CiawFilter.Sorter.Order);
            if (CiawParameter.CiawFilter.Sorter.Attribute.ToLower() == "ciawstatus")
                sb.Append("ORDER BY CiawStatus " + CiawParameter.CiawFilter.Sorter.Order);
        }

        var parameters = new { lang = CiawParameter.Lang, Cu = CiawParameter.ContractingUnitSequenceId };
        IEnumerable<CiawHeaderFilterDto> data;

        data = await cuconnection.QueryAsync<CiawHeaderFilterDto>(sb.ToString(), parameters);

        var ciawErrorLIst = cuconnection.Query<CiawError>("SELECT * FROM dbo.CiawError ORDER BY RequestDateTime DESC")
            .ToList();

        var certificate =
            @"SELECT
              CabCertification.EndDate
             ,CabCertification.PersonId
             ,CabCertification.CertificationTaxonomyId
            FROM dbo.CabCertification";

        var cabQuery = @"SELECT
                          CabPerson.FullName AS CabPersonName
                         ,CabAddress.CountryId AS NationalityId
                         ,CabPerson.Id AS CabPersonId
                         ,CabVat.Vat AS VatId
                        FROM dbo.CabPerson
                        LEFT OUTER JOIN dbo.CabNationality
                          ON CabPerson.Id = CabNationality.CabPersonId
                        LEFT OUTER JOIN dbo.CabPersonCompany
                          ON CabPerson.Id = CabPersonCompany.PersonId
                        LEFT OUTER JOIN dbo.CabCompany
                          ON CabPersonCompany.CompanyId = CabCompany.Id
                        LEFT OUTER JOIN dbo.CabAddress
                          ON CabCompany.AddressId = CabAddress.Id
                        LEFT OUTER JOIN dbo.CabVat
                          ON CabCompany.VATId = CabVat.Id
                            WHERE CabPerson.Id In @CabPersonId;
                            SELECT
                          ProjectDefinition.Title AS ProjectTitle
                         ,ProjectCiawSite.CiawSiteCode AS CiawSiteCode
                         ,ProjectDefinition.SequenceCode AS ProjectSequenceCode
                        ,ProjectDefinition.VATId as VatId
                        FROM dbo.ProjectDefinition
                        FULL OUTER JOIN dbo.ProjectCiawSite
                          ON ProjectDefinition.Id = ProjectCiawSite.ProjectId
                        WHERE ProjectDefinition.SequenceCode IN @Project";

        var cabdataListM = connection.QueryMultiple(cabQuery,
            new { Project = data.Select(d => d.Project), CabPersonId = data.Select(d => d.CabPersonId) });
        var cabdataList = cabdataListM.Read<CiawHeaderFilterDto>();
        var CiawSiteCodeList = cabdataListM.Read<CiawSiteCodeList>();
        // var CiawSiteCodeList = connection.Query<CiawSiteCodeList>(
        //     "SELECT ProjectCiawSite.CiawSiteCode, ProjectDefinition.SequenceCode As ProjectSequenceCode FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.ProjectCiawSite ON ProjectCiawSite.ProjectId = ProjectDefinition.Id WHERE ProjectDefinition.SequenceCode IN @Project",
        //     new { Project = data.Select(d => d.Project) });

        var dates = connection.Query<CiawcertificateDate>(certificate,
            new
            {
                CabPersonId = data.Select(d => d.CabPersonId),
                Id = new[] { "87c045e4-cfc5-4a9e-be7c-c4755880a7d7", "24aa5219-b510-4aac-8024-a4d40c8a9060" }
            });
        
        var mcabdataList = cabdataList.Where(e => !e.VatId.IsNullOrEmpty()).ToList();

        var featchStatus = cuconnection.Query<CiawFeatchStatus>("SELECT Id,Status FROM dbo.CiawFeatchStatus WHERE Id = '1'").FirstOrDefault();


        List<SearchPresenceRegistrationItem> searchPresenceRegistrationItemList;
        
        if (!featchStatus.Status)
        {
            //cuconnection.Execute("DELETE FROM dbo.CiawRemark");
            
            searchPresenceRegistrationItemList = await SearchPresence(CiawParameter, mcabdataList);
            
            foreach (var i in data)
            {
                var searchPresenceRegistrationItems =
                    searchPresenceRegistrationItemList.FirstOrDefault(s =>
                        s.presenceRegistrationId == i.PresenceRegistrationId.ToInt());
                if (searchPresenceRegistrationItems != null)
                {
                    i.SearchPresenceRegistrationItem = searchPresenceRegistrationItems;

                    var lastValidationRemarkList = searchPresenceRegistrationItems?.lastValidation?.remarkList;

                    if (lastValidationRemarkList != null)
                    {
                        foreach (var a in lastValidationRemarkList?.remark)
                        {
                            const string q = @"MERGE
                                    INTO dbo.CiawRemark t1 USING (SELECT
                                        1 CiawId) t2
                                    ON (t1.CiawId = @CiawId)
                                    WHEN MATCHED
                                      THEN UPDATE
                                        SET Error = @Error
                                    WHEN NOT MATCHED
                                      THEN INSERT ( Id ,CiawId ,Error ) VALUES ( @Id ,@CiawId ,@Error );";

                            await cuconnection.ExecuteAsync(q, new {Id = Guid.NewGuid(),CiawId = i.Id,Error = a });
                        }
                    }

                }
                
                var cabdataP = cabdataList.FirstOrDefault(ca => ca.CabPersonId == i.CabPersonId);


                var cabdata = CiawSiteCodeList.FirstOrDefault(ca => ca.ProjectSequenceCode == i.Project);


                //var CiawSiteCode = CiawSiteCodeList.Where(ca => ca.ProjectSequenceCode ==i.Project ).Select(s=>s.CiawSiteCode).FirstOrDefault();


                if (cabdataP != null)
                {
                    i.NationalityId = cabdataP.NationalityId;
                    i.CabPersonName = cabdataP.CabPersonName;
                }

                if (cabdata != null) i.ProjectTitle = cabdata.ProjectTitle;
                if (cabdata?.CiawSiteCode != null) i.CiawSiteCode = cabdata.CiawSiteCode;


                if (i.CiawStatus != "Completed")
                {
                    if (i.NationalityId == null) i.IsCiawEligible = "2";
                    if (i.CiawSiteCode == null) i.IsCiawEligible = "2";
                    if ((cabdataP?.VatId).IsNullOrEmpty()) i.IsCiawEligible = "2";
                    if (i.NationalityId == "420f5bbd-0891-44ae-9527-75341234ec49")
                    {
                        var date = dates
                            .Where(ca =>
                                ca.PersonId == i.CabPersonId &&
                                ca.CertificationTaxonomyId == "87c045e4-cfc5-4a9e-be7c-c4755880a7d7").Select(s => s.EndDate)
                            .FirstOrDefault();
                        // var date = connection.Query<DateTime>(certificate,
                        //         new { i.CabPersonId, Id = "87c045e4-cfc5-4a9e-be7c-c4755880a7d7" })
                        //     .FirstOrDefault();

                        if (date < i.Date) i.IsCiawEligible = "2";

                        var mCertificate = dates.FirstOrDefault(ca => ca.PersonId == i.CabPersonId &&
                                                                      ca.CertificationTaxonomyId ==
                                                                      "87c045e4-cfc5-4a9e-be7c-c4755880a7d7");
                        if (mCertificate == null) i.IsCiawEligible = "2";
                    }
                    else
                    {
                        var date = dates
                            .Where(ca =>
                                ca.PersonId == i.CabPersonId &&
                                ca.CertificationTaxonomyId == "24aa5219-b510-4aac-8024-a4d40c8a9060").Select(s => s.EndDate)
                            .FirstOrDefault();

                        // var date = connection.Query<DateTime>(certificate,
                        //         new { i.CabPersonId, Id = "24aa5219-b510-4aac-8024-a4d40c8a9060" })
                        //     .FirstOrDefault();

                        if (date < i.Date) i.IsCiawEligible = "2";

                        var mCertificate = dates.FirstOrDefault(ca => ca.PersonId == i.CabPersonId &&
                                                                      ca.CertificationTaxonomyId ==
                                                                      "24aa5219-b510-4aac-8024-a4d40c8a9060");
                        if (mCertificate == null) i.IsCiawEligible = "2";
                    }

                    var error = ciawErrorLIst.FirstOrDefault(e => e.CiawId == i.CIAWReferenceId);

                    if (error != null) i.IsCiawEligible = "3";
                }

                i.IsCiawEligibleOrder = i.IsCiawEligible switch
                {
                    "2" => 3,
                    "3" => 2,
                    "1" => 1,
                    _ => i.IsCiawEligibleOrder
                };
            
            }

            cuconnection.Execute("UPDATE dbo.CiawFeatchStatus SET Status = 1 WHERE Id = 1");
        }

        else
        {
            foreach (var i in data)
            {
                var searchPresenceRegistrationItems = new SearchPresenceRegistrationItem(
                   
                    );
                searchPresenceRegistrationItems.lastValidation = new LastValidationResponse();
                searchPresenceRegistrationItems.lastValidation.remarkList = new RemarkList();
                var remarkListRemark = cuconnection.QueryAsync<string>("SELECT Error FROM dbo.CiawRemark WHERE CiawId = @Id", new {Id = i.Id}).Result.ToList();
                if (remarkListRemark != null)
                {
                    searchPresenceRegistrationItems.lastValidation.remarkList.remark = remarkListRemark;
                }
                if (searchPresenceRegistrationItems != null)
                {
                    i.SearchPresenceRegistrationItem = searchPresenceRegistrationItems;

                    //var lastValidationRemarkList = searchPresenceRegistrationItems?.lastValidation?.remarkList;
                    //
                    // if (lastValidationRemarkList != null)
                    // {
                    //     foreach (var a in lastValidationRemarkList?.remark)
                    //     {
                    //         var q = @"INSERT INTO dbo.CiawRemark ( Id ,CiawId ,Error ) VALUES ( @Id ,@CiawId ,@Error );";
                    //
                    //         cuconnection.Execute(q, new {Id = Guid.NewGuid(),CiawId = i.Id,Error = a });
                    //     }
                    // }

                }
                
                var cabdataP = cabdataList.FirstOrDefault(ca => ca.CabPersonId == i.CabPersonId);


                var cabdata = CiawSiteCodeList.FirstOrDefault(ca => ca.ProjectSequenceCode == i.Project);


                //var CiawSiteCode = CiawSiteCodeList.Where(ca => ca.ProjectSequenceCode ==i.Project ).Select(s=>s.CiawSiteCode).FirstOrDefault();


                if (cabdataP != null)
                {
                    i.NationalityId = cabdataP.NationalityId;
                    i.CabPersonName = cabdataP.CabPersonName;
                }

                if (cabdata != null) i.ProjectTitle = cabdata.ProjectTitle;
                if (cabdata?.CiawSiteCode != null) i.CiawSiteCode = cabdata.CiawSiteCode;


                if (i.CiawStatus != "Completed")
                {
                    if (i.NationalityId == null) i.IsCiawEligible = "2";
                    if (i.CiawSiteCode == null) i.IsCiawEligible = "2";
                    if ((cabdataP?.VatId).IsNullOrEmpty()) i.IsCiawEligible = "2";
                    if (i.NationalityId == "420f5bbd-0891-44ae-9527-75341234ec49")
                    {
                        var date = dates
                            .Where(ca =>
                                ca.PersonId == i.CabPersonId &&
                                ca.CertificationTaxonomyId == "87c045e4-cfc5-4a9e-be7c-c4755880a7d7").Select(s => s.EndDate)
                            .FirstOrDefault();
                        // var date = connection.Query<DateTime>(certificate,
                        //         new { i.CabPersonId, Id = "87c045e4-cfc5-4a9e-be7c-c4755880a7d7" })
                        //     .FirstOrDefault();

                        if (date < i.Date) i.IsCiawEligible = "2";

                        var mCertificate = dates.FirstOrDefault(ca => ca.PersonId == i.CabPersonId &&
                                                                      ca.CertificationTaxonomyId ==
                                                                      "87c045e4-cfc5-4a9e-be7c-c4755880a7d7");
                        if (mCertificate == null) i.IsCiawEligible = "2";
                    }
                    else
                    {
                        var date = dates
                            .Where(ca =>
                                ca.PersonId == i.CabPersonId &&
                                ca.CertificationTaxonomyId == "24aa5219-b510-4aac-8024-a4d40c8a9060").Select(s => s.EndDate)
                            .FirstOrDefault();

                        // var date = connection.Query<DateTime>(certificate,
                        //         new { i.CabPersonId, Id = "24aa5219-b510-4aac-8024-a4d40c8a9060" })
                        //     .FirstOrDefault();

                        if (date < i.Date) i.IsCiawEligible = "2";

                        var mCertificate = dates.FirstOrDefault(ca => ca.PersonId == i.CabPersonId &&
                                                                      ca.CertificationTaxonomyId ==
                                                                      "24aa5219-b510-4aac-8024-a4d40c8a9060");
                        if (mCertificate == null) i.IsCiawEligible = "2";
                    }

                    var error = ciawErrorLIst.FirstOrDefault(e => e.CiawId == i.CIAWReferenceId);

                    if (error != null) i.IsCiawEligible = "3";
                }

                i.IsCiawEligibleOrder = i.IsCiawEligible switch
                {
                    "2" => 3,
                    "3" => 2,
                    "1" => 1,
                    _ => i.IsCiawEligibleOrder
                };
            
            }

        }

        
        if (CiawParameter.CiawFilter.CabPerson != null)
            data = data.Where(e => e.CabPersonName.ToLower().Contains(CiawParameter.CiawFilter.CabPerson.ToLower()))
                .ToList();

        if (CiawParameter.CiawFilter.Project != null)
            data = data.Where(e => e.ProjectTitle.ToLower().Contains(CiawParameter.CiawFilter.Project.ToLower()))
                .ToList();

        if (CiawParameter.CiawFilter.IsCiawEligible != null)
            if (CiawParameter.CiawFilter.IsCiawEligible != "0")
                data = data.Where(e =>
                        e.IsCiawEligible.ToLower().Contains(CiawParameter.CiawFilter.IsCiawEligible.ToLower()))
                    .ToList();

        if (CiawParameter.CiawFilter.Sorter.Attribute == null)
        {
            data = data.OrderBy(e => e.CabPersonName);
        }
        else
        {
            if (CiawParameter.CiawFilter.Sorter.Attribute == "cabPersonName")
                data = CiawParameter.CiawFilter.Sorter.Order == "desc"
                    ? data.OrderByDescending(e => e.CabPersonName)
                    : data.OrderBy(e => e.CabPersonName);

            if (CiawParameter.CiawFilter.Sorter.Attribute == "title")
                data = CiawParameter.CiawFilter.Sorter.Order == "desc"
                    ? data.OrderByDescending(e => e.ProjectTitle)
                    : data.OrderBy(e => e.ProjectTitle);

            if (CiawParameter.CiawFilter.Sorter.Attribute == "isCiawEligible")
                data = CiawParameter.CiawFilter.Sorter.Order == "desc"
                    ? data.OrderByDescending(e => e.IsCiawEligibleOrder)
                    : data.OrderBy(e => e.IsCiawEligibleOrder);
        }

        return data;
    }

    public async Task<CiawDropDownData> CiawDropDownData(CiawParameter CiawParameter)
    {
        const string query =
            @"select StatusId as [Key], Name as Text  FROM dbo.CiawStatus where LanguageCode = @lang
                              ORDER BY DisplayOrder;";

        var mCiawDropDownData = new CiawDropDownData();

        var parameters = new { lang = CiawParameter.Lang };

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var muilti = await connection.QueryMultipleAsync(query, parameters);

        mCiawDropDownData.Status = muilti.Read<CiawStatusDto>();

        return mCiawDropDownData;
    }

    public async Task<CiawDropDownData> CiawCancelDropDownData(CiawParameter CiawParameter)
    {
        const string query =
            @"select StatusId as [Key], Name as Text  FROM dbo.CiawCancelStatus where LanguageCode = @lang
                              ORDER BY DisplayOrder;";

        var mCiawDropDownData = new CiawDropDownData();

        var parameters = new { lang = CiawParameter.Lang };

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var muilti = await connection.QueryMultipleAsync(query, parameters);

        mCiawDropDownData.Status = muilti.Read<CiawStatusDto>();

        return mCiawDropDownData;
    }

    public async Task<CiawGetByIdDto> CiawGetById(CiawParameter CiawParameter)
    {
        var cuconnectionString = ConnectionString.MapConnectionString(CiawParameter.ContractingUnitSequenceId, null,
            CiawParameter.TenantProvider);

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var query = @"SELECT
                          CiawHeader.Id
                         ,CiawHeader.CabPersonId
                         ,CiawHeader.Date
                         ,CiawHeader.CreatedBy
                         ,CiawHeader.ModifiedBy
                         ,CiawHeader.CreatedDate
                         ,CiawHeader.ModifiedDate
                         ,CiawHeader.Project
                         ,CiawHeader.Reference
                         ,CiawHeader.PmolTeamRoleId
                         ,CiawHeader.CIAWReferenceId
                         ,CiawHeader.IsMailSend
                         ,CiawHeader.PresenceRegistrationId AS CiawCode
                         ,CiawStatus.StatusId AS [Key]
                         ,CiawStatus.Name AS Text
                        FROM dbo.CiawHeader
                        LEFT OUTER JOIN dbo.CiawStatus
                          ON CiawHeader.CiawStatus = CiawStatus.StatusId
                        WHERE CiawStatus.LanguageCode = @lang
                        AND CiawHeader.Id = @Id";

        var parameters = new { lang = CiawParameter.Lang, CiawParameter.Id };

        HistoryLogDto historyLog = null;

        CiawGetByIdDto mCiawGetByIdDto = null;

        using (var cuconnection = new SqlConnection(cuconnectionString))
        {
            mCiawGetByIdDto = cuconnection
                .Query<CiawGetByIdDto, CiawStatusDto, CiawGetByIdDto>(
                    query,
                    (ciawheader, ciawStatus) =>
                    {
                        ciawheader.Status = ciawStatus;
                        return ciawheader;
                    }, parameters,
                    splitOn: "Key").FirstOrDefault();

            var cabQuery =
                @"SELECT CabPerson.FullName AS CabPersonName ,ProjectDefinition.Title AS ProjectTitle ,ProjectDefinition.ProjectManagerId AS ProjectManager ,'Approved' AS CiawRegistrationStatus ,Nationality.Name AS Nationality ,Nationality.NationalityId FROM dbo.ProjectDefinition ,dbo.CabPerson LEFT OUTER JOIN dbo.CabNationality ON CabPerson.Id = CabNationality.CabPersonId LEFT OUTER JOIN dbo.Nationality ON CabNationality.NationalityId = Nationality.NationalityId WHERE CabPerson.Id = @CabPersonId AND ProjectDefinition.SequenceCode = @Project";

            var cabdata = connection
                .Query<CiawGetByIdDto>(cabQuery, new { mCiawGetByIdDto.CabPersonId, mCiawGetByIdDto.Project })
                .FirstOrDefault();

            var orgQuery = @"SELECT
                              CabVat.Vat AS COMPANY_ID
                             ,Country.Id AS CabCountry
                             ,Country.CountryName AS CabCountryName
                             ,CabCompany.Name AS CabCompany
                            FROM dbo.CabPersonCompany
                            LEFT OUTER JOIN dbo.CabCompany
                              ON CabPersonCompany.CompanyId = CabCompany.Id
                            LEFT OUTER JOIN dbo.CabVat
                              ON CabCompany.VatId = CabVat.Id
                            LEFT OUTER JOIN dbo.CabAddress
                              ON CabCompany.AddressId = CabAddress.Id
                            LEFT OUTER JOIN dbo.Country
                              ON CabAddress.CountryId = Country.Id
                            WHERE CabPersonCompany.PersonId = @CabPersonId;
                            SELECT
                              CabCertification.CertificationTitle AS INSS
                             ,CabCertification.CertificationTaxonomyId
                            FROM dbo.CabCertification
                            WHERE CabCertification.CertificationTitle IS NOT NULL
                            AND CabCertification.CertificationTaxonomyId IN ('24aa5219-b510-4aac-8024-a4d40c8a9060', '87c045e4-cfc5-4a9e-be7c-c4755880a7d7')
                            AND CabCertification.PersonId = @CabPersonId;
                            SELECT
                              ProjectCiawSite.CiawSiteCode AS WORKPLACE_ID
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectCiawSite
                              ON ProjectDefinition.Id = ProjectCiawSite.ProjectId
                            WHERE ProjectCiawSite.CiawSiteCode IS NOT NULL
                            AND ProjectDefinition.SequenceCode = @Project;";

            var orgData = connection
                .Query<CiawRequestData>(orgQuery, new { mCiawGetByIdDto.CabPersonId, mCiawGetByIdDto.Project })
                .FirstOrDefault();

            var cabdataListM =
                connection.QueryMultiple(orgQuery, new { mCiawGetByIdDto.CabPersonId, mCiawGetByIdDto.Project });
            var mCompanyDto = cabdataListM.Read<CompanyDataDto>().FirstOrDefault();
            var mCertificationDto = cabdataListM.Read<CertificationDto>().FirstOrDefault();
            var mWorkPlaceIdDto = cabdataListM.Read<WorkPlaceIdDto>().FirstOrDefault();

            var error = cuconnection
                .Query<string>(
                    "SELECT errorDescription FROM dbo.CiawError WHERE CiawId = @CiawId ORDER BY RequestDateTime desc",
                    new { CiawId = mCiawGetByIdDto.CIAWReferenceId }).FirstOrDefault();

            var ciawJsonString = cuconnection.Query<CiawResponseJson>(
                "SELECT * FROM dbo.CiawResponseJson WHERE CiawReferenceId = @CiawReferenceId",
                new { CiawReferenceId = mCiawGetByIdDto.CIAWReferenceId }).FirstOrDefault();


            string successJson;
            string errorJson;

            if (ciawJsonString != null)
            {
                if (ciawJsonString.SuccessJson != null)
                    mCiawGetByIdDto.JsonString = ciawJsonString.SuccessJson;
                else
                    mCiawGetByIdDto.JsonString = ciawJsonString.ErrorJson;
            }


            var ciawCertificate = connection.Query<CabCertification>(
                "SELECT * FROM dbo.CabCertification WHERE PersonId = @PersonId",
                new { PersonId = mCiawGetByIdDto.CabPersonId }).ToList();

            if (cabdata != null)
            {
                mCiawGetByIdDto.NationalityId = cabdata.NationalityId;
                mCiawGetByIdDto.CabPersonName = cabdata.CabPersonName;
                mCiawGetByIdDto.ProjectTitle = cabdata.ProjectTitle;
                mCiawGetByIdDto.ProjectManager = cabdata.ProjectManager;
                mCiawGetByIdDto.Nationality = cabdata.Nationality;
            }

            if (cabdataListM != null)
            {
                mCiawGetByIdDto.ProjectCiawCode = mWorkPlaceIdDto?.WORKPLACE_ID.Trim().ToUpper();
                mCiawGetByIdDto.Organisation = mCompanyDto?.CabCompany;
                mCiawGetByIdDto.OrgCountryCode = mCompanyDto?.CabCountry;
                mCiawGetByIdDto.OrgCountryName = mCompanyDto?.CabCountryName;
                if (!string.IsNullOrEmpty(mCompanyDto?.COMPANY_ID))
                    mCiawGetByIdDto.OrgCiawCode = new string(mCompanyDto?.COMPANY_ID.ToCharArray()
                        .Where(char.IsDigit)
                        .ToArray()).ToLong();

                if (!string.IsNullOrEmpty(mCertificationDto?.INSS))
                    mCiawGetByIdDto.CertificationId = new string(mCertificationDto?.INSS.ToCharArray()
                        .Where(c => !char.IsWhiteSpace(c))
                        .ToArray()).ToUpper();
            }

            if (mCiawGetByIdDto.Status.Key != "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce" &&
                mCiawGetByIdDto.Status.Key != "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce")
                if (error != null)
                    mCiawGetByIdDto.ErrorWarning = error;

            var historyQuery =
                @"SELECT [CreatedDate],[CreatedBy],[ModifiedBy],[ModifiedDate] FROM [dbo].[CiawHeader] where [Id] =@Id ";

            historyLog = cuconnection.Query<HistoryLogDto>(historyQuery, new { CiawParameter.Id }).FirstOrDefault();

            var ModifiedByUserQuery =
                @"SELECT CabPerson.FullName FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE Oid = @oid";

            var ModifiedByParameter = new { oid = historyLog.ModifiedBy };
            historyLog.ModifiedBy = connection.Query<string>(ModifiedByUserQuery, ModifiedByParameter)
                .FirstOrDefault();

            var CreatByParam = new { oid = historyLog.CreatedBy };
            historyLog.CreatedBy = connection.Query<string>(ModifiedByUserQuery, CreatByParam)
                .FirstOrDefault();

            mCiawGetByIdDto.HistoryLog = historyLog;

            mCiawGetByIdDto.ProjectManager = connection
                .Query<string>("SELECT FullName FROM dbo.CabPerson WHERE Id = @Id",
                    new { Id = mCiawGetByIdDto.ProjectManager }).FirstOrDefault();

            var mCabCertification = connection
                .Query<CabCertification>("SELECT * FROM CabCertification WHERE PersonId = @CabPersonId",
                    new { mCiawGetByIdDto.CabPersonId }).ToList();

            if (mCabCertification.Any())
            {
                if (mCiawGetByIdDto.OrgCountryCode == "420f5bbd-0891-44ae-9527-75341234ec49")
                {
                    mCiawGetByIdDto.CabCertification = mCabCertification.FirstOrDefault(e =>
                        e.CertificationTaxonomyId == "87c045e4-cfc5-4a9e-be7c-c4755880a7d7");
                    if (mCiawGetByIdDto.CabCertification != null)
                        mCiawGetByIdDto.CertificationId = new string(mCiawGetByIdDto.CabCertification.CertificationTitle
                            .ToCharArray()
                            .Where(c => !char.IsWhiteSpace(c))
                            .ToArray()).ToUpper();
                }
                else
                {
                    mCiawGetByIdDto.CabCertification = mCabCertification.FirstOrDefault(e =>
                        e.CertificationTaxonomyId == "24aa5219-b510-4aac-8024-a4d40c8a9060");
                    if (mCiawGetByIdDto.CabCertification != null)
                        mCiawGetByIdDto.CertificationId = new string(mCiawGetByIdDto.CabCertification.CertificationTitle
                            .ToCharArray()
                            .Where(c => !char.IsWhiteSpace(c))
                            .ToArray()).ToUpper();
                }
            }

            var isCiawEligible = "1";

            if (mCiawGetByIdDto.Status.Key != "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce")
            {
                var CiawSiteCode = connection.Query<string>(
                    "SELECT ProjectCiawSite.CiawSiteCode FROM dbo.ProjectDefinition LEFT OUTER JOIN dbo.ProjectCiawSite ON ProjectCiawSite.ProjectId = ProjectDefinition.Id WHERE ProjectDefinition.SequenceCode = @Project",
                    new { mCiawGetByIdDto.Project }).FirstOrDefault();
                if (CiawSiteCode == null)
                {
                    isCiawEligible = "2";

                    mCiawGetByIdDto.StatusError = "CiawSiteCode Is null";
                }

                if (mCiawGetByIdDto.OrgCountryCode == "420f5bbd-0891-44ae-9527-75341234ec49")
                {
                    var date = mCabCertification
                        .Where(e => e.CertificationTaxonomyId == "87c045e4-cfc5-4a9e-be7c-c4755880a7d7")
                        .Select(e => e.EndDate).FirstOrDefault();

                    if ((date != null && date < mCiawGetByIdDto.Date) || date == null)
                    {
                        isCiawEligible = "2";
                        mCiawGetByIdDto.StatusError = "SSNI Certificate Not valid";
                    }

                    if (date == null) mCiawGetByIdDto.StatusError = "No certification for this person";
                }
                else
                {
                    var date = mCabCertification
                        .Where(e => e.CertificationTaxonomyId == "24aa5219-b510-4aac-8024-a4d40c8a9060")
                        .Select(e => e.EndDate).FirstOrDefault();

                    if ((date != null && date < mCiawGetByIdDto.Date) || date == null)
                    {
                        isCiawEligible = "2";

                        mCiawGetByIdDto.StatusError = "Limosa Certificate Not Available";
                    }

                    if (date == null) mCiawGetByIdDto.StatusError = "No certification for this person";
                }

                if ((mCompanyDto?.COMPANY_ID).IsNullOrEmpty())
                {
                    isCiawEligible = "2";
                    mCiawGetByIdDto.StatusError = "company VatID is Null";
                }

                if (error != null)
                {
                    isCiawEligible = "3";
                    mCiawGetByIdDto.StatusError = "errors occured when sending ciaw";
                }
            }

            if (mCiawGetByIdDto.Status.Key == "4010e768-3e06-4702-ciaws-ee367a82addb")
            {
                if (isCiawEligible == "1" || isCiawEligible == "2")
                    mCiawGetByIdDto.CiawRegistrationStatus = "Not Send";
                else if (isCiawEligible == "3") mCiawGetByIdDto.CiawRegistrationStatus = "Check in fail";
            }

            if (mCiawGetByIdDto.Status.Key == "7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce" && isCiawEligible == "1")
                mCiawGetByIdDto.CiawRegistrationStatus = "Check in pass";
            if (mCiawGetByIdDto.Status.Key == "7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce" && isCiawEligible == "1")
                mCiawGetByIdDto.CiawRegistrationStatus = "Canceled";
            
            
            var lastValidation = new LastValidation();
            lastValidation.remarkList = new RemarkList();
            lastValidation.remarkList.remark  = cuconnection.Query<string>("SELECT Error FROM dbo.CiawRemark WHERE CiawId = @Id", new {Id = mCiawGetByIdDto.Id}).ToList();
            mCiawGetByIdDto.remarks = lastValidation;
            



        }

        // if (mCiawGetByIdDto.CiawCode != null)
        // {
        //     var client = new HttpClient();
        //     client.BaseAddress = new Uri(CiawParameter.Configuration.GetValue<string>("Ciaw_Url"));
        //     //client.BaseAddress = new Uri("https://localhost:44329/");
        //     var response =
        //         await client.GetAsync("api/PresenceRegistration/GetPresenceRegistration?presenceRegistrationId=" +
        //                               mCiawGetByIdDto.CiawCode);
        //     var jsonString = await response.Content.ReadAsStringAsync();
        //
        //     var dataInstance = JObject.Parse(jsonString)["getPresenceRegistrationResponse"];
        //     var dataInstance2 = JObject.Parse(dataInstance.ToString())["getPresenceRegistrationResponseType"];
        //     var dataInstance3 = JObject.Parse(dataInstance2.ToString())["getPresenceRegistrationType"];
        //
        //     var serializeObject = JsonConvert.SerializeObject(dataInstance3);
        //     var presenceRegistrationTypeResponce =
        //         JsonConvert.DeserializeObject<PresenceRegistrationTypeResponce>(serializeObject);
        //     mCiawGetByIdDto.remarks = presenceRegistrationTypeResponce?.presenceRegistrationType?.lastValidation;
        // }
        

        return mCiawGetByIdDto;
    }


    public async Task<List<object>> CiawSendRequest(CiawParameter CiawParameter)
    {
        var cuconnectionString = ConnectionString.MapConnectionString(CiawParameter.ContractingUnitSequenceId, null,
            CiawParameter.TenantProvider);

        await using var cuconnection = new SqlConnection(cuconnectionString);

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var ciawList = new List<CiawHeader>();

        if (CiawParameter.CiawSendRequest != null)
        {
            var ciawListSql = @"SELECT * FROM dbo.CiawHeader WHERE CiawHeader.Id IN @Ids";

            ciawList = cuconnection.Query<CiawHeader>(ciawListSql, new { Ids = CiawParameter.CiawSendRequest.CiawId })
                .ToList();
        }
        else
        {
            var ciawListSql =
                @"SELECT * FROM dbo.CiawHeader  WHERE CiawHeader.Date = @Date AND CiawHeader.CiawStatus = '4010e768-3e06-4702-ciaws-ee367a82addb'";

            ciawList = cuconnection.Query<CiawHeader>(ciawListSql, new { Date = DateTime.UtcNow.Date.AddDays(1) })
                .ToList();
        }


        var ciawListdata = new List<CiawRequestData>();

        var responseSet = new List<object>();

        var registerRequestList = new RegisterRequestList();

        var registerRequest = new List<RegisterRequest>();

        foreach (var ciaw in ciawList)
        {
            var cabQuery = @"SELECT
                              ProjectCiawSite.CiawSiteCode AS WORKPLACE_ID
                             ,CabCertification.CertificationTitle AS INSS
                             ,CabVat.Vat AS COMPANY_ID
                             ,CabCertification.CertificationTaxonomyId
                             ,CabAddress.CountryId AS CabCountry
                            FROM dbo.CabCertification
                                ,dbo.ProjectDefinition
                                 LEFT OUTER JOIN dbo.ProjectCiawSite
                                   ON ProjectDefinition.Id = ProjectCiawSite.ProjectId
                                ,dbo.CabPersonCompany
                                 LEFT OUTER JOIN dbo.CabCompany
                                   ON CabPersonCompany.CompanyId = CabCompany.Id
                                 LEFT OUTER JOIN dbo.CabVat
                                   ON CabCompany.VatId = CabVat.Id
                                 LEFT OUTER JOIN dbo.CabAddress
								   ON CabCompany.AddressId = CabAddress.Id
                            WHERE CabCertification.CertificationTitle IS NOT NULL
                            AND ProjectCiawSite.CiawSiteCode IS NOT NULL
                            AND CabCertification.CertificationTaxonomyId IN ('24aa5219-b510-4aac-8024-a4d40c8a9060', '87c045e4-cfc5-4a9e-be7c-c4755880a7d7')
                            AND CabCertification.PersonId = @CabPersonId
                            AND ProjectDefinition.SequenceCode = @Project
                            AND CabPersonCompany.PersonId = @CabPersonId";

            var ciawData = connection.Query<CiawRequestData>(cabQuery, new { ciaw.Project, ciaw.CabPersonId })
                .FirstOrDefault();

            if (ciawData != null)
            {
                ciawData.CIAWReferenceId = ciaw.CIAWReferenceId.ToString();
                ciawData.Date = DateTime.Parse(ciaw.Date.ToString());
                ciawListdata.Add(ciawData);

                if (ciawData.CabCountry == "420f5bbd-0891-44ae-9527-75341234ec49")
                {
                    var item = new Item
                    {
                        INSS = new string(ciawData.INSS.ToCharArray()
                            .Where(c => !char.IsWhiteSpace(c))
                            .ToArray()).ToUpper()
                    };
                    if (!string.IsNullOrEmpty(ciawData.COMPANY_ID))
                        item.COMPANY_ID = new string(ciawData.COMPANY_ID.ToCharArray()
                            .Where(char.IsDigit)
                            .ToArray()).ToLong();
                    var registerRequest1 = new RegisterRequest
                    {
                        WORKPLACE_ID = ciawData.WORKPLACE_ID.Trim().ToUpper(),
                        Item = item,
                        CiawId = ciawData.CIAWReferenceId.Trim(),
                        RegistrationDate = ciawData.Date
                    };

                    registerRequest.Add(registerRequest1);
                }

                else
                {
                    var item = new Item
                    {
                        LimosaId = new string(ciawData.INSS.ToCharArray()
                            .Where(c => !char.IsWhiteSpace(c))
                            .ToArray()).ToUpper() //limosaCertificateTitle
                    };
                    var registerRequest1 = new RegisterRequest
                    {
                        Item = item,
                        CiawId = ciawData.CIAWReferenceId.Trim(),
                        WORKPLACE_ID = ciawData.WORKPLACE_ID.Trim().ToUpper(),
                        RegistrationDate = DateTime.Parse(ciawData.Date.ToString())
                    };

                    registerRequest.Add(registerRequest1);
                }
            }
        }

        registerRequestList.AuthKey = "cf139b23c491bb39d5ba2ade880700961c24be33";
        registerRequestList.RegisterRequest = registerRequest;
        var payload = JsonSerializer.Serialize(registerRequestList);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var client = new HttpClient();
        client.BaseAddress = new Uri(CiawParameter.Configuration.GetValue<string>("Ciaw_Url"));
        //client.BaseAddress = new Uri("https://localhost:44329/");

        var response = await client.PostAsync("api/PresenceRegistration/RegisterPresences", content);
        var jsonString = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var myDeserializedClass = JsonConvert.DeserializeObject<List<ResponceRoot>>(jsonString);

            foreach (var i in myDeserializedClass)
            {
                var ciawResponseJson = JsonSerializer.Serialize(i);
                var ciawResponse = new StringContent(ciawResponseJson, Encoding.UTF8, "application/json");
                if (i.item.presenceRegistrationId != 0)
                {
                    await cuconnection.ExecuteAsync(
                        "UPDATE dbo.CiawHeader SET PresenceRegistrationId = @PresenceRegistrationId WHERE CIAWReferenceId = @Id",
                        new
                        {
                            PresenceRegistrationId = i.item.presenceRegistrationId,
                            Id = i.item.item.clientPresenceRegistrationReference
                        });
                    await cuconnection.ExecuteAsync(
                        "UPDATE dbo.CiawHeader  SET CiawStatus = '7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce' WHERE CIAWReferenceId = @Id",
                        new { Id = i.item.item.clientPresenceRegistrationReference });

                    var responseData = cuconnection.Query<CiawResponseJson>(
                        "SELECT * FROM dbo.CiawResponseJson WHERE CiawResponseJson.CiawReferenceId = @Id",
                        new { Id = i.item.item.clientPresenceRegistrationReference }).FirstOrDefault();

                    if (responseData != null)
                        await cuconnection.ExecuteAsync(
                            "UPDATE dbo.CiawResponseJson  SET SuccessJson = @SuccessJson, ErrorJson = null WHERE CIAWReferenceId = @Id",
                            new
                            {
                                Id = i.item.item.clientPresenceRegistrationReference, SuccessJson = ciawResponseJson
                            });
                    else
                        await cuconnection.ExecuteAsync(
                            "INSERT INTO dbo.CiawResponseJson (Id ,CiawReferenceId ,SuccessJson) VALUES (@Id ,@CiawReferenceId ,@SuccessJson)",
                            new
                            {
                                CiawReferenceId = i.item.item.clientPresenceRegistrationReference,
                                Id = Guid.NewGuid().ToString(), SuccessJson = ciawResponseJson
                            });
                }

                if (i.item.errorList != null)
                {
                    var sql =
                        @"INSERT INTO dbo.CiawError ( Id ,CiawId ,errorCode ,errorDescription,RequestDateTime ) VALUES ( @Id ,@CiawId ,@errorCode ,@errorDescription,@RequestDateTime );";

                    foreach (var n in i.item.errorList)
                    {
                        var param = new
                        {
                            Id = Guid.NewGuid(),
                            CiawId = i.item.presenceRegistrationSubmitted.clientPresenceRegistrationReference,
                            n.errorDescription,
                            n.errorCode,
                            RequestDateTime = DateTime.UtcNow
                        };

                        await cuconnection.ExecuteAsync(sql, param);

                        var responseData = cuconnection.Query<CiawResponseJson>(
                                "SELECT * FROM dbo.CiawResponseJson WHERE CiawResponseJson.CiawReferenceId = @Id",
                                new { Id = i.item.presenceRegistrationSubmitted.clientPresenceRegistrationReference })
                            .FirstOrDefault();

                        if (responseData != null)
                            await cuconnection.ExecuteAsync(
                                "UPDATE dbo.CiawResponseJson  SET ErrorJson = @ErrorJson, SuccessJson = null WHERE CIAWReferenceId = @Id",
                                new
                                {
                                    Id = i.item.presenceRegistrationSubmitted.clientPresenceRegistrationReference,
                                    ErrorJson = ciawResponseJson
                                });
                        else
                            await cuconnection.ExecuteAsync(
                                "INSERT INTO dbo.CiawResponseJson (Id ,CiawReferenceId ,ErrorJson) VALUES (@Id ,@CiawReferenceId ,@ErrorJson)",
                                new
                                {
                                    CiawReferenceId = i.item.presenceRegistrationSubmitted
                                        .clientPresenceRegistrationReference,
                                    Id = Guid.NewGuid().ToString(), ErrorJson = ciawResponseJson
                                });
                    }
                }
            }

            responseSet.Add(jsonString);
        }
        else
        {
            // var json = JsonConvert.DeserializeObject<object>(jsonString);
            responseSet = await CiawSendSingleRequest(CiawParameter, ciawListdata);
        }

        await cuconnection.ExecuteAsync("UPDATE dbo.CiawFeatchStatus SET Status = 0 WHERE Id = 1");
        return responseSet;
    }

    public async Task<List<object>> CiawSendSingleRequest(CiawParameter CiawParameter,
        List<CiawRequestData> ciawList)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(CiawParameter.ContractingUnitSequenceId, null,
            CiawParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var responseSet = new List<object>();

        foreach (var ciaw in ciawList)
        {
            var registerRequestList = new RegisterRequestList();
            var registerRequest = new List<RegisterRequest>();

            if (ciaw.CabCountry == "420f5bbd-0891-44ae-9527-75341234ec49")
            {
                var item = new Item
                {
                    INSS = new string(ciaw.INSS.ToCharArray()
                        .Where(c => !char.IsWhiteSpace(c))
                        .ToArray()),
                    COMPANY_ID = new string(ciaw.COMPANY_ID.ToCharArray()
                        .Where(char.IsDigit)
                        .ToArray()).ToLong()
                };
                var registerRequest1 = new RegisterRequest
                {
                    WORKPLACE_ID = ciaw.WORKPLACE_ID.Trim(),
                    Item = item,
                    CiawId = ciaw.CIAWReferenceId.Trim(),
                    RegistrationDate = DateTime.Parse(ciaw.Date.ToString())
                };

                // registerRequest = registerRequest1;
                registerRequest.Add(registerRequest1);
            }

            else
            {
                var item = new Item
                {
                    LimosaId = new string(ciaw.INSS.ToCharArray()
                        .Where(c => !char.IsWhiteSpace(c))
                        .ToArray()) //limosaCertificateTitle
                };
                var registerRequest1 = new RegisterRequest
                {
                    Item = item,
                    CiawId = ciaw.CIAWReferenceId.Trim(),
                    WORKPLACE_ID = ciaw.WORKPLACE_ID.Trim(),
                    RegistrationDate = DateTime.Parse(ciaw.Date.ToString())
                };
                registerRequest.Add(registerRequest1);
                //registerRequest = registerRequest1;
            }

            registerRequestList.AuthKey = "cf139b23c491bb39d5ba2ade880700961c24be33";
            registerRequestList.RegisterRequest = registerRequest;
            var payload = JsonSerializer.Serialize(registerRequestList);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.BaseAddress = new Uri(CiawParameter.Configuration.GetValue<string>("Ciaw_Url"));
            //client.BaseAddress = new Uri("https://uprincev5ciawproduction.azurewebsites.net/");
            var response = await client.PostAsync("api/PresenceRegistration/RegisterPresences", content);
            var jsonString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var myDeserializedClass = JsonConvert.DeserializeObject<List<ResponceRoot>>(jsonString);

                foreach (var r in myDeserializedClass)
                {
                    var ciawResponse = JsonSerializer.Serialize(r);
                    if (r.item.presenceRegistrationId != 0)
                    {
                        await cuConnection.ExecuteAsync(
                            "UPDATE dbo.CiawHeader SET PresenceRegistrationId = @PresenceRegistrationId WHERE CIAWReferenceId = @Id",
                            new
                            {
                                PresenceRegistrationId = r.item.presenceRegistrationId,
                                Id = r.item.item.clientPresenceRegistrationReference
                            });
                        await cuConnection.ExecuteAsync(
                            "UPDATE dbo.CiawHeader  SET CiawStatus = '7bcb4e8d-8e8c-487d-ciaws-6b91c89fAcce' WHERE CIAWReferenceId = @Id;",
                            new { Id = r.item.item.clientPresenceRegistrationReference });

                        var responseData = cuConnection.Query<CiawResponseJson>(
                            "SELECT * FROM dbo.CiawResponseJson WHERE CiawResponseJson.CiawReferenceId = @Id",
                            new { Id = r.item.item.clientPresenceRegistrationReference }).FirstOrDefault();

                        if (responseData != null)
                            await cuConnection.ExecuteAsync(
                                "UPDATE dbo.CiawResponseJson  SET SuccessJson = @SuccessJson, ErrorJson = null WHERE CIAWReferenceId = @Id;",
                                new
                                {
                                    Id = r.item.item.clientPresenceRegistrationReference, SuccessJson = ciawResponse
                                });
                        else
                            await cuConnection.ExecuteAsync(
                                "INSERT INTO dbo.CiawResponseJson (Id ,CiawReferenceId ,SuccessJson) VALUES (@Id ,@CiawReferenceId ,@SuccessJson);",
                                new
                                {
                                    CiawReferenceId = r.item.item.clientPresenceRegistrationReference,
                                    Id = Guid.NewGuid().ToString(), SuccessJson = ciawResponse
                                });
                    }

                    if (r.item.errorList != null)
                    {
                        var sql =
                            @"INSERT INTO dbo.CiawError ( Id ,CiawId ,errorCode ,errorDescription,RequestDateTime ) VALUES ( @Id ,@CiawId ,@errorCode ,@errorDescription,@RequestDateTime );";

                        foreach (var n in r.item.errorList)
                        {
                            var param = new
                            {
                                Id = Guid.NewGuid(),
                                CiawId = r.item.presenceRegistrationSubmitted.clientPresenceRegistrationReference,
                                n.errorDescription,
                                n.errorCode,
                                RequestDateTime = DateTime.UtcNow
                            };

                            await connection.ExecuteAsync(sql, param);
                        }

                        var responseData = cuConnection.Query<CiawResponseJson>(
                                "SELECT * FROM dbo.CiawResponseJson WHERE CiawResponseJson.CiawReferenceId = @Id",
                                new { Id = r.item.presenceRegistrationSubmitted.clientPresenceRegistrationReference })
                            .FirstOrDefault();

                        if (responseData != null)
                            await cuConnection.ExecuteAsync(
                                "UPDATE dbo.CiawResponseJson  SET ErrorJson = @ErrorJson, SuccessJson = null WHERE CIAWReferenceId = @Id",
                                new
                                {
                                    Id = r.item.presenceRegistrationSubmitted.clientPresenceRegistrationReference,
                                    ErrorJson = ciawResponse
                                });
                        else
                            await cuConnection.ExecuteAsync(
                                "INSERT INTO dbo.CiawResponseJson (Id ,CiawReferenceId ,ErrorJson) VALUES (@Id ,@CiawReferenceId ,@ErrorJson)",
                                new
                                {
                                    CiawReferenceId = r.item.presenceRegistrationSubmitted
                                        .clientPresenceRegistrationReference,
                                    Id = Guid.NewGuid().ToString(), ErrorJson = ciawResponse
                                });
                    }
                }
            }

            else
            {
                var myDeserializedClass = JsonConvert.DeserializeObject<ErrorRoot>(jsonString);

                var sql =
                    @"INSERT INTO dbo.CiawError ( Id ,CiawId ,errorCode ,errorDescription,RequestDateTime ) VALUES ( @Id ,@CiawId ,@errorCode ,@errorDescription,@RequestDateTime );";

                var exceptionMessage = myDeserializedClass.exceptionMessage!;
                if (exceptionMessage == null) exceptionMessage = myDeserializedClass.message!;

                var param = new
                {
                    Id = Guid.NewGuid(),
                    CiawId = ciaw.CIAWReferenceId,
                    errorDescription = exceptionMessage,
                    errorCode = "500",
                    RequestDateTime = DateTime.UtcNow
                };

                await cuConnection.ExecuteAsync(sql, param);

                var responseData = cuConnection.Query<CiawResponseJson>(
                    "SELECT * FROM dbo.CiawResponseJson WHERE CiawResponseJson.CiawReferenceId = @Id",
                    new { Id = ciaw.CIAWReferenceId }).FirstOrDefault();

                if (responseData != null)
                    await cuConnection.ExecuteAsync(
                        "UPDATE dbo.CiawResponseJson  SET ErrorJson = @ErrorJson, SuccessJson = null WHERE CIAWReferenceId = @Id;",
                        new { Id = ciaw.CIAWReferenceId, ErrorJson = jsonString });
                else
                    await cuConnection.ExecuteAsync(
                        "INSERT INTO dbo.CiawResponseJson (Id ,CiawReferenceId ,ErrorJson) VALUES (@Id ,@CiawReferenceId , @ErrorJson);",
                        new
                        {
                            CiawReferenceId = ciaw.CIAWReferenceId, Id = Guid.NewGuid().ToString(),
                            ErrorJson = jsonString
                        });
            }

            responseSet.Add(jsonString);
        }

        return responseSet;
    }

    public async Task<object> CiawCancelPresences(CiawParameter CiawParameter)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(CiawParameter.ContractingUnitSequenceId, null,
            CiawParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var ciawListSql =
            @"SELECT * FROM dbo.CiawHeader WHERE PresenceRegistrationId IS NOT NULL AND CiawHeader.Id = @Ids";

        var ciawList = cuConnection.Query<CiawHeader>(ciawListSql, new { Ids = CiawParameter.CiawCancleRequest.CiawId })
            .ToList();

        var cancelList = new List<CancelPresences>();

        if (ciawList.Any())
            foreach (var i in ciawList)
            {
                var cancelItem = new CancelPresences
                {
                    PresenceRegistrationId = i.PresenceRegistrationId.ToInt(),
                    CancellationReason = CiawParameter.CiawCancleRequest.LeaveReasonId
                };

                await cuConnection.ExecuteAsync(
                    "UPDATE dbo.CiawHeader  SET CiawStatus = '7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce' WHERE Id = @Id;",
                    new { i.Id });

                var projectConnectionString =
                    ConnectionString.MapConnectionString(CiawParameter.ContractingUnitSequenceId, i.Project,
                        CiawParameter.TenantProvider);
                await using var projectConnection = new SqlConnection(projectConnectionString);
                await projectConnection.ExecuteAsync(
                    "UPDATE dbo.CiawHeader  SET CiawStatus = '7bcb4e8d-cancl-487d-ciaws-6b91c89fAcce' WHERE CIAWReferenceId = @Id;",
                    new { Id = i.CIAWReferenceId });
                cancelList.Add(cancelItem);
            }

        var payload = JsonSerializer.Serialize(cancelList);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var client = new HttpClient();
        client.BaseAddress = new Uri(CiawParameter.Configuration.GetValue<string>("Ciaw_Url"));
        var response = await client.PostAsync("api/PresenceRegistration/CancelPresences", content);
        var jsonString = await response.Content.ReadAsStringAsync();
        var json = JsonConvert.DeserializeObject<object>(jsonString);

        return json;
    }

    public async Task<string> ProjectCiawSiteCreate(CiawParameter CiawParameter)
    {
        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var Id = connection
            .Query<string>("SELECT Id FROM dbo.ProjectCiawSite WHERE ProjectId = @ProjectId",
                new { CiawParameter.ProjectCiawSite.ProjectId })
            .FirstOrDefault();

        if (Id == null)
        {
            var sql =
                @"INSERT INTO dbo.ProjectCiawSite ( Id ,CiawSiteCode ,CiawSeverEntry ,ProjectId,IsCiawEnabled ) VALUES ( @Id ,@CiawSiteCode ,@CiawSeverEntry ,@ProjectId,@IsCiawEnabled );";

            var param = new
            {
                CiawParameter.ProjectCiawSite.Id,
                CiawParameter.ProjectCiawSite.CiawSiteCode,
                CiawParameter.ProjectCiawSite.CiawSeverEntry,
                CiawParameter.ProjectCiawSite.ProjectId,
                CiawParameter.ProjectCiawSite.IsCiawEnabled
            };

            await connection.ExecuteAsync(sql, param);

            return CiawParameter.ProjectCiawSite.Id;
        }
        else
        {
            var sql =
                @"UPDATE dbo.ProjectCiawSite SET CiawSiteCode = @CiawSiteCode ,CiawSeverEntry = @CiawSeverEntry ,ProjectId = @ProjectId,IsCiawEnabled = @IsCiawEnabled WHERE ProjectId = @ProjectId;";

            var param = new
            {
                CiawParameter.ProjectCiawSite.CiawSiteCode,
                CiawParameter.ProjectCiawSite.CiawSeverEntry,
                CiawParameter.ProjectCiawSite.ProjectId,
                CiawParameter.ProjectCiawSite.IsCiawEnabled
            };

            await connection.ExecuteAsync(sql, param);

            return CiawParameter.ProjectCiawSite.Id;
        }
    }

    public async Task<ProjectCiawSite> ProjectCiawSiteGet(CiawParameter CiawParameter)
    {
        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var mProjectCiawSite = connection
            .Query<ProjectCiawSite>("SELECT * FROM dbo.ProjectCiawSite WHERE ProjectId = @ProjectId",
                new { ProjectId = CiawParameter.Id }).FirstOrDefault();

        return mProjectCiawSite;
    }

    public async Task<IEnumerable<NationalityDto>> FilterNationality(CiawParameter CiawParameter)
    {
        var sql = @"SELECT NationalityId AS [Key], Name AS [Text] FROM dbo.Nationality WHERE Name LIKE '%" +
                  CiawParameter.NationalityFilter.Name + "%'";

        await using var connection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);

        var mNationality = connection.Query<NationalityDto>(sql).ToList();

        return mNationality;
    }

    public async Task<string> SendCiawWarningEmail(CiawParameter CiawParameter)
    {
        var cuConnectionString = ConnectionString.MapConnectionString(CiawParameter.ContractingUnitSequenceId, null,
            CiawParameter.TenantProvider);

        await using var dbConnection = new SqlConnection(CiawParameter.TenantProvider.GetTenant().ConnectionString);
        await using var connection = new SqlConnection(cuConnectionString);

        var mCiaw = connection
            .Query<CiawHeader>("SELECT * FROM dbo.CiawHeader WHERE Id = @Id",
                new { CiawParameter.Id }).FirstOrDefault();


        if (mCiaw != null)
        {
            var customerQuery = @"SELECT CabPerson.FullName AS CustomerName
                                    ,CabEmail.EmailAddress AS CustomerEmail
                                FROM dbo.CabEmail
                                INNER JOIN dbo.CabPersonCompany
                                    ON CabEmail.Id = CabPersonCompany.EmailId
                                INNER JOIN dbo.ProjectDefinition
                                    ON CabPersonCompany.PersonId = ProjectDefinition.CustomerId
                                INNER JOIN dbo.CabPerson
                                    ON CabPersonCompany.PersonId = CabPerson.Id
                                WHERE ProjectDefinition.SequenceCode = @Project";

            var personQuery =
                @"SELECT CabEmail.EmailAddress AS EmailAddress ,CabPersonCompany.Oid , CabPersonCompany.Id,CabPerson.FullName , CabPersonCompany.JobRole ,CabPersonCompany.CompanyId , CabCompany.Name AS CompanyName,CabVat.Vat FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id LEFT OUTER JOIN CabCompany  ON CabPersonCompany.CompanyId = CabCompany.Id LEFT OUTER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id LEFT OUTER JOIN dbo.CabVat ON CabCompany.VatId = CabVat.Id WHERE  CabPerson.IsDeleted = 0 AND CabPerson.Id = @Id";

            var personQuery2 =
                @"SELECT CabEmail.EmailAddress AS EmailAddress ,CabPersonCompany.Oid , CabPersonCompany.Id,CabPerson.FullName , CabPersonCompany.JobRole ,CabPersonCompany.CompanyId , CabCompany.Name AS CompanyName,CabVat.Vat FROM dbo.CabPersonCompany LEFT OUTER JOIN dbo.CabEmail ON CabPersonCompany.EmailId = CabEmail.Id LEFT OUTER JOIN CabCompany  ON CabPersonCompany.CompanyId = CabCompany.Id LEFT OUTER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id LEFT OUTER JOIN dbo.CabVat ON CabCompany.VatId = CabVat.Id WHERE  CabPerson.IsDeleted = 0 AND CabPersonCompany.Id = @Id";

            RfqCab cabPerson;

            var contactManager = dbConnection.Query<string>(
                @"SELECT                             
                                                     CabPersonCompany.Id                        
                                                    FROM dbo.HRHeader
                                                    LEFT OUTER JOIN dbo.CabPersonCompany
                                                      ON HRHeader.PersonId = CabPersonCompany.Id
                                                     WHERE CabPersonCompany.CompanyId = (SELECT HRHeader.WorkingOrganization FROM dbo.HRHeader LEFT OUTER JOIN dbo.CabPersonCompany ON HRHeader.PersonId = CabPersonCompany.Id WHERE CabPersonCompany.PersonId = @PersonId AND HRHeader.WorkingOrganization IS NOT NULL) AND HRHeader.IsContactManager = 1",
                new { PersonId = mCiaw.CabPersonId }).FirstOrDefault();

            cabPerson = contactManager != null
                ? dbConnection.Query<RfqCab>(personQuery2, new { Id = contactManager }).FirstOrDefault()
                : dbConnection.Query<RfqCab>(personQuery, new { Id = mCiaw.CabPersonId }).FirstOrDefault();

            var customer = dbConnection.Query<RfqCab>(customerQuery, new { mCiaw.Project }).FirstOrDefault();

            var employee = dbConnection.Query<RfqCab>(personQuery, new { Id = mCiaw.CabPersonId }).FirstOrDefault();

            var orgQuery = @"SELECT
                                  ProjectCiawSite.CiawSiteCode AS WORKPLACE_ID
                                 ,CabCertification.CertificationTitle AS INSS
                                 ,CabVat.Vat AS COMPANY_ID
                                 ,CabCertification.CertificationTaxonomyId
                                 ,Country.CountryName AS CabCountry
                                 ,CabCompany.Name AS CabCompany
                                 ,CabAddress.Street
                                 ,CabAddress.StreetNumber
                                 ,CabAddress.PostalCode
                                 ,CabAddress.City
                                FROM dbo.CabCertification
                                    ,dbo.ProjectDefinition
                                     LEFT OUTER JOIN dbo.ProjectCiawSite
                                       ON ProjectDefinition.Id = ProjectCiawSite.ProjectId
                                    ,dbo.CabPersonCompany
                                     LEFT OUTER JOIN dbo.CabCompany
                                       ON CabPersonCompany.CompanyId = CabCompany.Id
                                     LEFT OUTER JOIN dbo.CabVat
                                       ON CabCompany.VatId = CabVat.Id
                                     LEFT OUTER JOIN dbo.CabAddress
                                       ON CabCompany.AddressId = CabAddress.Id
                                LEFT OUTER JOIN dbo.Country 
                                       ON CabAddress.CountryId = Country.Id
                                WHERE CabCertification.CertificationTitle IS NOT NULL
                                AND ProjectCiawSite.CiawSiteCode IS NOT NULL
                                AND CabCertification.CertificationTaxonomyId IN ('24aa5219-b510-4aac-8024-a4d40c8a9060', '87c045e4-cfc5-4a9e-be7c-c4755880a7d7')
                                AND CabCertification.PersonId = @CabPersonId
                                AND ProjectDefinition.SequenceCode = @Project
                                AND CabPersonCompany.PersonId = @CabPersonId";

            var orgData = dbConnection
                .Query<CiawRequestData>(orgQuery, new { mCiaw.CabPersonId, mCiaw.Project })
                .FirstOrDefault();

            var error = connection
                .Query<string>(
                    "SELECT errorDescription FROM dbo.CiawError WHERE CiawId = @CiawId ORDER BY RequestDateTime desc",
                    new { CiawId = mCiaw.CIAWReferenceId }).FirstOrDefault();

            var mCabCertification = dbConnection
                .Query<CabCertification>("SELECT * FROM CabCertification WHERE PersonId = @CabPersonId",
                    new { mCiaw.CabPersonId }).ToList();


            var apikey = CiawParameter.Configuration.GetValue<string>("SENDGRID_API_KEY_2");
            var templateId_en = CiawParameter.Configuration.GetValue<string>("CiawTemplate_en");
            var templateId_nl = CiawParameter.Configuration.GetValue<string>("CiawTemplate_nl");
            var email = CiawParameter.Configuration.GetValue<string>("Ciaw_Email");
            var name = CiawParameter.Configuration.GetValue<string>("Ciaw_Email_Name");

            var url = CiawParameter.Configuration.GetValue<string>("DomainUrl") + "/CU/";


            var client =
                new SendGridClient(apikey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(email, name));

            //msg.AddTo(new EmailAddress(cabPerson?.EmailAddress, cabPerson?.FullName));

            msg.AddTo(new EmailAddress(customer?.CustomerEmail, customer?.CustomerName));


            //msg.Subject = "RFQ Accept";
            if (CiawParameter.Lang == "en") msg.SetTemplateId(templateId_en);

            if (CiawParameter.Lang == "nl") msg.SetTemplateId(templateId_nl);

            foreach (var i in mCabCertification)
                if (!i.CertificationUrl.IsNullOrEmpty())
                {
                    byte[] bytes, fileBytes;
                    using (var webClient = new WebClient())
                    {
                        bytes = webClient.DownloadData(i.CertificationUrl);
                        var ms = new MemoryStream(bytes);

                        fileBytes = ms.ToArray();
                    }

                    var pdfBase64 = Convert.ToBase64String(bytes);

                    var fileName = Path.GetFileName(i.CertificationUrl);

                    msg.AddAttachment(fileName, pdfBase64);
                }

            string subject = null;
            if (CiawParameter.Lang == "en")
                subject = "WERF" + " - " + mCiaw.PresenceRegistrationId + " - aangifte van werken ontbreekt";

            if (CiawParameter.Lang == "nl")
                subject = "WERF" + " - " + mCiaw.PresenceRegistrationId + " - aangifte van werken ontbreekt";

            var dynamicTemplateData = new CiawEmail2
            {
                Subject = subject,
                Reason = error,
                FirstName = employee.CompanyName,
                StreetNumber = orgData?.Street + " - " + orgData?.StreetNumber,
                PostalCode = orgData?.PostalCode + " - " + orgData?.City,
                Country = orgData?.CabCountry,
                VatCode = orgData?.COMPANY_ID,
                WorkingContractor = cabPerson.CompanyName + " - " + cabPerson.Vat,
                AdminAssistant = "Alisson Mare"
            };

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
                throw new Exception(response.Body.ReadAsStringAsync().Result);

            await connection.ExecuteAsync("Update CiawHeader Set IsMailSend = 1 Where Id = @Id",
                new { mCiaw.Id });
        }

        return CiawParameter.Id;
    }

    public async Task<List<SearchPresenceRegistrationItem>> SearchPresence(CiawParameter CiawParameter,
        IEnumerable<CiawHeaderFilterDto> cabdataList)
    {
        var searchPresenceRegistrationItemList = new List<SearchPresenceRegistrationItem>();

        var cabListGroupBy = cabdataList.GroupBy(e => e.VatId).ToList();

        foreach (var i in cabListGroupBy)
        {
            string eMsg;
            try
            {
                var mSearchPresenceRegistrationCriteria = new SearchPresenceRegistrationCriteria
                {
                    CompanyID = new string(i.Key.ToCharArray()
                        .Where(char.IsDigit)
                        .ToArray()).ToLong()
                };
                var mCiawSearchDto = new CiawSearchDto
                {
                    searchPresenceRegistrationCriteria = mSearchPresenceRegistrationCriteria
                };

                var payload = JsonSerializer.Serialize(mCiawSearchDto);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(CiawParameter.Configuration.GetValue<string>("Ciaw_Url"));
                //client.BaseAddress = new Uri("https://localhost:44329/");

                var response = await client.PostAsync("api/PresenceRegistration/SearchPresences", content);
                var jsonString = await response.Content.ReadAsStringAsync();

                var dataInstance = JObject.Parse(jsonString)["searchPresencesResponse"];
                var dataInstance2 = JObject.Parse(dataInstance.ToString())["searchPresencesResponseType"];
                var dataInstance3 = JObject.Parse(dataInstance2.ToString())["item"];
                IList<JToken> results = dataInstance3["presenceRegistration"].Children().ToList();

                var searchPresenceRegistrationItem =
                    JsonConvert.DeserializeObject<List<SearchPresenceRegistrationItem>>(
                        JsonConvert.SerializeObject(results));

                searchPresenceRegistrationItemList.AddRange(searchPresenceRegistrationItem);

            }
            catch (Exception e)
            {
                eMsg = e.ToString();
            }
            
        }

        return searchPresenceRegistrationItemList;
    }
}