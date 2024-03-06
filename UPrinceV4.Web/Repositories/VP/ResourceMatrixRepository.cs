using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.HR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.VP;

public class ResourceMatrixRepository : IResourceMatrixRepository
{
    private static int GetIso8601WeekOfYear(DateTime time)
    {
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    private static DateTime FirstDateOfWeekIso8601(int year, int weekOfYear)
    {
        DateTime jan1 = new DateTime(year, 1, 1);
        int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
        DateTime firstThursday = jan1.AddDays(daysOffset);
        var cal = CultureInfo.CurrentCulture.Calendar;
        int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        var weekNum = weekOfYear;
        if (firstWeek == 1)
        {
            weekNum -= 1;
        }

        var result = firstThursday.AddDays(weekNum * 7);

        return result.AddDays(-3);
    }  
    
    public async Task<ResourceMatrix> GetResourceMatrixFromPmol(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);

        var mResourceMatrixPmolData = new List<ResourceMatrixPmolData>();

        string pmolData = @"SELECT
                              PMol.ExecutionDate
                             ,CpcResourceFamilyLocalizedData.Label
                             ,PMolPlannedWorkLabour.RequiredQuantity
                             ,CorporateProductCatalog.Title
                             ,CorporateProductCatalog.Id as CpcId
                            FROM dbo.PMolPlannedWorkLabour
                            INNER JOIN dbo.PMol
                              ON PMolPlannedWorkLabour.PmolId = PMol.Id
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON CorporateProductCatalog.Id = PMolPlannedWorkLabour.CoperateProductCatalogId
                            RIGHT OUTER JOIN dbo.CpcResourceFamilyLocalizedData
                              ON CpcResourceFamilyLocalizedData.CpcResourceFamilyId = CorporateProductCatalog.ResourceFamilyId
                              WHERE PMol.ExecutionDate IS NOT NULL 
                              AND CpcResourceFamilyLocalizedData.Label IS NOT NULL 
                              AND CpcResourceFamilyLocalizedData.LanguageCode = @lang 
                              AND PMolPlannedWorkLabour.RequiredQuantity IS NOT NULL 
                              AND PMol.IsDeleted = 0
                              AND PMolPlannedWorkLabour.IsDeleted = 0
                              ORDER BY CpcResourceFamilyLocalizedData.Label";
        
        if (resourceMatrixParameter.IsMyEnv)
        {
            var selectProject =
                @"SELECT ProjectDefinition.Id,ProjectDefinition.Title, ProjectDefinition.ProjectConnectionString,ProjectDefinition.SequenceCode,CabCompany.SequenceCode AS ContractingUnitId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE  ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.ProjectManagerId = (SELECT PersonId FROM CabPersonCompany LEFT OUTER JOIN CabPerson cp ON CabPersonCompany.PersonId = cp.Id WHERE Oid = @UserId AND cp.IsDeleted = 0 AND CabPersonCompany.IsDeleted = 0) ";

            var db = connection.Query<ProjectDefinition>(selectProject, new {resourceMatrixParameter.UserId}).ToList();

            foreach (var project in db)
            {
                await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                var data = pConnection
                    .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();

                mResourceMatrixPmolData.AddRange(data);
            }
        }else if (resourceMatrixParameter.IsCu)
        {
            if (resourceMatrixParameter.getResourceMatrixDto.BusinessUnit.Any())
            {
                var selectPmProject = @"SELECT
                                      ProjectDefinition.Id
                                     ,ProjectDefinition.Title
                                     ,ProjectDefinition.SequenceCode
                                     ,ProjectDefinition.ProjectConnectionString
                                     ,ProjectDefinition.ProjectManagerId
                                     ,ProjectDefinition.ProjectStatus
                                     ,CabCompany.SequenceCode AS ContractingUnitId
                                     ,ProjectDefinition.ProjectScopeStatusId
                                    FROM dbo.ProjectDefinition
                                    LEFT OUTER JOIN CabCompany
                                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                                    LEFT OUTER JOIN dbo.ProjectClassification
                                      ON ProjectDefinition.Id = ProjectClassification.ProjectId
                                    WHERE ProjectDefinition.IsDeleted = 0
                                    AND ProjectClassification.ProjectClassificationBuisnessUnit IN (SELECT Id FROM dbo.OrganizationTaxonomy WHERE BuSequenceId IN @BuId)
                                    AND ProjectDefinition.ProjectScopeStatusId != '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'";

                var sb1 = new StringBuilder(selectPmProject);
                
                var db = connection
                    .Query<ProjectDefinition>(
                        sb1.ToString(),
                        new
                        {
                            BuId = resourceMatrixParameter.getResourceMatrixDto.BusinessUnit
                        }).ToList();

                foreach (var project in db)
                {
                    await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                    var data = pConnection
                        .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();

                    mResourceMatrixPmolData.AddRange(data);
                }            
            }
        }

        else
        {
            var connectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
                resourceMatrixParameter.ProjectSequenceId, resourceMatrixParameter.TenantProvider);
            
            await using var pConnection = new SqlConnection(connectionString);

            mResourceMatrixPmolData = pConnection
                .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();
        }

        const string preferredCpcSql = @"SELECT
                                  HRHeader.Id
                                 ,HRHeader.CpcLabourItemId AS CpcId
                                 ,AbsenceHeader.StartDate 
                                 ,AbsenceHeader.EndDate
                                 ,DATEDIFF(DAY,AbsenceHeader.StartDate ,AbsenceHeader.EndDate) + 1 AS totalDays
                                FROM dbo.HRHeader
                                LEFT OUTER JOIN dbo.AbsenceHeader
                                  ON HRHeader.PersonId = AbsenceHeader.Person
                                  WHERE CpcLabourItemId IN @CpcId";

        var weeks = new List<string>();
        var resource = new Dictionary<string, CpcDataDto>();
        
        if (mResourceMatrixPmolData.Any())
        {
            var startDate = mResourceMatrixPmolData.Select(e => e.ExecutionDate).Min();
            var endDate = mResourceMatrixPmolData.Select(e => e.ExecutionDate).Max().AddDays(30);
            
            var sDates = new List<DateTime>();
        
            for (var i = startDate; i <= endDate;i = i.AddDays(7))
            {
                var week = GetIso8601WeekOfYear(i);
                
                var weekStart = FirstDateOfWeekIso8601(i.Year, week);
                
                var weekId = "W" + week.ToString()+" ("+weekStart.ToString("dd/MM")+"-"+weekStart.AddDays(6).ToString("dd/MM")+")";
                
                weeks.Add(weekId);
                
                sDates.Add(weekStart);
            }
            
            var mResourceMatrixPmolDataGroup = mResourceMatrixPmolData.GroupBy(e => e.Label).ToList();

            foreach (var i in mResourceMatrixPmolDataGroup)
            {
                var allCpcId = i.Select(e => e.CpcId).ToList();
                var cpcId = allCpcId.Distinct();
                var preferredCpc = connection.Query<PreferredCpcDto>(preferredCpcSql,new {cpcId}).ToList();
                var preferredCpcId = preferredCpc.Select(e => e.CpcId).ToList();

                if (preferredCpc.Any())
                {
                    var cpcGroup = i.GroupBy(e => e.Title).ToList();
                
                    var childData = new Dictionary<string, List<ChildData>>();
                    
                    foreach (var r in cpcGroup)
                    {
                        var preferredCpcSelect = preferredCpc.Where(e => e.CpcId == r.First().CpcId).ToList();

                        if (preferredCpcSelect.Any())
                        {
                            var preferredCpcGroup = preferredCpcSelect.GroupBy(e => e.Id).ToList();
                            var planed = new List<ChildData>();
                        
                            foreach (var sDate in sDates)
                            {
                                var eDate = sDate.AddDays(6);
                                
                                var divider = preferredCpcGroup.Count * 40 - preferredCpcSelect.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                                
                                double sum = r.Where(e => e.ExecutionDate >= sDate && e.ExecutionDate <= eDate).ToList().Sum(e => e.RequiredQuantity);
                                var percentage = (sum/divider) * 100;
                                var mChildData = new ChildData
                                {
                                    Hours = sum.ToString(CultureInfo.InvariantCulture) + "/" + divider.ToString(),
                                    Percentage = percentage.ToString("0.00")
                                };
                                planed.Add(mChildData);
                            }  
                            childData.Add(r.Key,planed);
                        }
                    }

                    if (childData.Any())
                    {
                        var parentData = new List<ChildData>();
                        //var parentData = new List<string>();
                        foreach (var sDate in sDates)
                        {
                            var eDate = sDate.AddDays(6);
                            var p = i.Where(e => preferredCpcId.Contains(e.CpcId)).ToList();
                            var preferredCpcUser = preferredCpc.Select(e => e.Id).Distinct().ToList();
                            var divider = preferredCpcUser.Count * 40 - preferredCpc.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                            double sum = p.Where(e => e.ExecutionDate >= sDate && e.ExecutionDate <= eDate).ToList().Sum(e => e.RequiredQuantity);
                            var percentage = (sum/divider) * 100;
                            
                            var mParentData = new ChildData
                            {
                                Hours = sum +"/"+divider,
                                Percentage = percentage.ToString("0.00")
                            };
                            
                            parentData.Add(mParentData);
                            //parentData.Add(sum +"/"+divider.ToString());
                        }
                    
                        var mCpcData = new CpcDataDto
                        {
                            ParentData = parentData,
                            Childs = childData
                        };
      
                        resource.Add(i.Key,mCpcData);
                    }
                }
            }
            
        }

        var mResourceMatrix = new ResourceMatrix
        {
            Week = weeks,
            Resource = resource
        };
       
        
        return mResourceMatrix;
    }
    
    public async Task<ResourceMatrix> GetResourceMatrixFromPmolMonth(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);

        var mResourceMatrixPmolData = new List<ResourceMatrixPmolData>();

        string pmolData = @"SELECT
                              PMol.ExecutionDate
                             ,CpcResourceFamilyLocalizedData.Label
                             ,PMolPlannedWorkLabour.RequiredQuantity
                             ,CorporateProductCatalog.Title
                             ,CorporateProductCatalog.Id as CpcId
                            FROM dbo.PMolPlannedWorkLabour
                            INNER JOIN dbo.PMol
                              ON PMolPlannedWorkLabour.PmolId = PMol.Id
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON CorporateProductCatalog.Id = PMolPlannedWorkLabour.CoperateProductCatalogId
                            RIGHT OUTER JOIN dbo.CpcResourceFamilyLocalizedData
                              ON CpcResourceFamilyLocalizedData.CpcResourceFamilyId = CorporateProductCatalog.ResourceFamilyId
                              WHERE PMol.ExecutionDate IS NOT NULL 
                              AND CpcResourceFamilyLocalizedData.Label IS NOT NULL 
                              AND CpcResourceFamilyLocalizedData.LanguageCode = @lang 
                              AND PMolPlannedWorkLabour.RequiredQuantity IS NOT NULL 
                              AND PMol.IsDeleted = 0
                              AND PMolPlannedWorkLabour.IsDeleted = 0
                              ORDER BY CpcResourceFamilyLocalizedData.Label";
        
        if (resourceMatrixParameter.IsMyEnv)
        {
            var selectProject =
                @"SELECT ProjectDefinition.Id,ProjectDefinition.Title, ProjectDefinition.ProjectConnectionString,ProjectDefinition.SequenceCode,CabCompany.SequenceCode AS ContractingUnitId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE  ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.ProjectManagerId = (SELECT PersonId FROM CabPersonCompany LEFT OUTER JOIN CabPerson cp ON CabPersonCompany.PersonId = cp.Id WHERE Oid = @UserId AND cp.IsDeleted = 0 AND CabPersonCompany.IsDeleted = 0) ";

            var db = connection.Query<ProjectDefinition>(selectProject, new {resourceMatrixParameter.UserId}).ToList();

            foreach (var project in db)
            {
                await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                var data = pConnection
                    .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();

                mResourceMatrixPmolData.AddRange(data);
            }
        }else if (resourceMatrixParameter.IsCu)
        {
            if (resourceMatrixParameter.getResourceMatrixDto.BusinessUnit.Any())
            {
                var selectPmProject = @"SELECT
                                      ProjectDefinition.Id
                                     ,ProjectDefinition.Title
                                     ,ProjectDefinition.SequenceCode
                                     ,ProjectDefinition.ProjectConnectionString
                                     ,ProjectDefinition.ProjectManagerId
                                     ,ProjectDefinition.ProjectStatus
                                     ,CabCompany.SequenceCode AS ContractingUnitId
                                     ,ProjectDefinition.ProjectScopeStatusId
                                    FROM dbo.ProjectDefinition
                                    LEFT OUTER JOIN CabCompany
                                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                                    LEFT OUTER JOIN dbo.ProjectClassification
                                      ON ProjectDefinition.Id = ProjectClassification.ProjectId
                                    WHERE ProjectDefinition.IsDeleted = 0
                                    AND ProjectClassification.ProjectClassificationBuisnessUnit IN (SELECT Id FROM dbo.OrganizationTaxonomy WHERE BuSequenceId IN @BuId)
                                    AND ProjectDefinition.ProjectScopeStatusId != '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'";

                var sb1 = new StringBuilder(selectPmProject);
                
                var db = connection
                    .Query<ProjectDefinition>(
                        sb1.ToString(),
                        new
                        {
                            BuId = resourceMatrixParameter.getResourceMatrixDto.BusinessUnit
                        }).ToList();

                foreach (var project in db)
                {
                    await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                    var data = pConnection
                        .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();

                    mResourceMatrixPmolData.AddRange(data);
                }            
            }
        }

        else
        {
            var connectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
                resourceMatrixParameter.ProjectSequenceId, resourceMatrixParameter.TenantProvider);
            
            await using var pConnection = new SqlConnection(connectionString);

            mResourceMatrixPmolData = pConnection
                .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();
        }

        const string preferredCpcSql = @"SELECT
                                  HRHeader.Id
                                 ,HRHeader.CpcLabourItemId AS CpcId
                                 ,AbsenceHeader.StartDate 
                                 ,AbsenceHeader.EndDate
                                 ,DATEDIFF(DAY,AbsenceHeader.StartDate ,AbsenceHeader.EndDate) + 1 AS totalDays
                                FROM dbo.HRHeader
                                LEFT OUTER JOIN dbo.AbsenceHeader
                                  ON HRHeader.PersonId = AbsenceHeader.Person
                                  WHERE CpcLabourItemId IN @CpcId";
        
        var months = new List<string>();
        var resource = new Dictionary<string, CpcDataDto>();
        
        if (mResourceMatrixPmolData.Any())
        {
            var startDate = mResourceMatrixPmolData.Select(e => e.ExecutionDate).Min();
            var endDate = mResourceMatrixPmolData.Select(e => e.ExecutionDate).Max().AddMonths(4);
            
            var sDates = new List<DateTime>();

            for (var i = startDate; i <= endDate;i = i.AddMonths(1))
            {
                var month = i.ToString("MMMM") ;

                var monthStart = new DateTime(i.Year, i.Month, 1);
                var monthEnd = startDate.AddMonths(1).AddDays(-1);

                months.Add(month);
                
                sDates.Add(monthStart);
            }
            
            var mResourceMatrixPmolDataGroup = mResourceMatrixPmolData.GroupBy(e => e.Label).ToList();

            foreach (var i in mResourceMatrixPmolDataGroup)
            {
                var allCpcId = i.Select(e => e.CpcId).ToList();
                var cpcId = allCpcId.Distinct();
                var preferredCpc = connection.Query<PreferredCpcDto>(preferredCpcSql,new {cpcId}).ToList();
                var preferredCpcId = preferredCpc.Select(e => e.CpcId).ToList();

                if (preferredCpc.Any())
                {
                    var cpcGroup = i.GroupBy(e => e.Title).ToList();
                
                    var childData = new Dictionary<string, List<ChildData>>();
                    foreach (var r in cpcGroup)
                    {
                        var preferredCpcSelect = preferredCpc.Where(e => e.CpcId == r.First().CpcId).ToList();
                        if (preferredCpcSelect.Any())
                        {
                            var preferredCpcGroup = preferredCpcSelect.GroupBy(e => e.Id).ToList();
                            var planed = new List<ChildData>();
                        
                            foreach (var sDate in sDates)
                            {
                                var eDate = sDate.AddMonths(1).AddDays(-1);
                                
                                var divider = preferredCpcGroup.Count * 160 - preferredCpcSelect.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                                
                                double sum = r.Where(e => e.ExecutionDate >= sDate && e.ExecutionDate <= eDate).ToList().Sum(e => e.RequiredQuantity);
                                var percentage = (sum/divider) * 100;
                                var mChildData = new ChildData
                                {
                                    Hours = sum.ToString(CultureInfo.InvariantCulture) + "/" + divider.ToString(),
                                    Percentage = percentage.ToString("0.00")
                                };
                                planed.Add(mChildData);
                            }  
                            childData.Add(r.Key,planed);
                        }
                    }
                    
                    if (childData.Any())
                    {
                        var parentData = new List<ChildData>();
                        //var parentData = new List<string>();
                        foreach (var sDate in sDates)
                        {
                            var eDate = sDate.AddMonths(1).AddDays(-1);
                            var p = i.Where(e => preferredCpcId.Contains(e.CpcId)).ToList();
                            var preferredCpcUser = preferredCpc.Select(e => e.Id).Distinct().ToList();
                            var divider = preferredCpcUser.Count * 160 - preferredCpc.Where(e => e.StartDate >= sDate && e.StartDate <= sDate.AddMonths(1).AddDays(-1)).ToList().Sum(e => e.TotalDays) * 8;
                            double sum = p.Where(e => e.ExecutionDate >= sDate && e.ExecutionDate <= eDate).ToList().Sum(e => e.RequiredQuantity);
                            var percentage = (sum / divider) * 100;
                            
                            var mParentData = new ChildData
                            {
                                Hours = sum +"/"+divider,
                                Percentage = percentage.ToString("0.00")
                            };
                            
                            parentData.Add(mParentData);
                            //parentData.Add(sum +"/"+divider.ToString());
                        }
                    
                        var mCpcData = new CpcDataDto
                        {
                            ParentData = parentData,
                            Childs = childData
                        };
      
                        resource.Add(i.Key,mCpcData);
                    }
                }
            }
            
        }

        var mResourceMatrix = new ResourceMatrix
        {
            Week = months,
            Resource = resource
        };
       
        
        return mResourceMatrix;
    }
    
    public async Task<ResourceMatrix> GetResourceMatrixFromPmolQuarter(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);

        var mResourceMatrixPmolData = new List<ResourceMatrixPmolData>();

        string pmolData = @"SELECT
                              PMol.ExecutionDate
                             ,CpcResourceFamilyLocalizedData.Label
                             ,PMolPlannedWorkLabour.RequiredQuantity
                             ,CorporateProductCatalog.Title
                             ,CorporateProductCatalog.Id as CpcId
                            FROM dbo.PMolPlannedWorkLabour
                            INNER JOIN dbo.PMol
                              ON PMolPlannedWorkLabour.PmolId = PMol.Id
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON CorporateProductCatalog.Id = PMolPlannedWorkLabour.CoperateProductCatalogId
                            RIGHT OUTER JOIN dbo.CpcResourceFamilyLocalizedData
                              ON CpcResourceFamilyLocalizedData.CpcResourceFamilyId = CorporateProductCatalog.ResourceFamilyId
                              WHERE PMol.ExecutionDate IS NOT NULL 
                              AND CpcResourceFamilyLocalizedData.Label IS NOT NULL 
                              AND CpcResourceFamilyLocalizedData.LanguageCode = @lang 
                              AND PMolPlannedWorkLabour.RequiredQuantity IS NOT NULL 
                              AND PMol.IsDeleted = 0
                              AND PMolPlannedWorkLabour.IsDeleted = 0
                              ORDER BY CpcResourceFamilyLocalizedData.Label";
        
        if (resourceMatrixParameter.IsMyEnv)
        {
            var selectProject =
                @"SELECT ProjectDefinition.Id,ProjectDefinition.Title, ProjectDefinition.ProjectConnectionString,ProjectDefinition.SequenceCode,CabCompany.SequenceCode AS ContractingUnitId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE  ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.ProjectManagerId = (SELECT PersonId FROM CabPersonCompany LEFT OUTER JOIN CabPerson cp ON CabPersonCompany.PersonId = cp.Id WHERE Oid = @UserId AND cp.IsDeleted = 0 AND CabPersonCompany.IsDeleted = 0) ";

            var db = connection.Query<ProjectDefinition>(selectProject, new {resourceMatrixParameter.UserId}).ToList();

            foreach (var project in db)
            {
                await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                var data = pConnection
                    .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();

                mResourceMatrixPmolData.AddRange(data);
            }
        }else if (resourceMatrixParameter.IsCu)
        {
            if (resourceMatrixParameter.getResourceMatrixDto.BusinessUnit.Any())
            {
                var selectPmProject = @"SELECT
                                      ProjectDefinition.Id
                                     ,ProjectDefinition.Title
                                     ,ProjectDefinition.SequenceCode
                                     ,ProjectDefinition.ProjectConnectionString
                                     ,ProjectDefinition.ProjectManagerId
                                     ,ProjectDefinition.ProjectStatus
                                     ,CabCompany.SequenceCode AS ContractingUnitId
                                     ,ProjectDefinition.ProjectScopeStatusId
                                    FROM dbo.ProjectDefinition
                                    LEFT OUTER JOIN CabCompany
                                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                                    LEFT OUTER JOIN dbo.ProjectClassification
                                      ON ProjectDefinition.Id = ProjectClassification.ProjectId
                                    WHERE ProjectDefinition.IsDeleted = 0
                                    AND ProjectClassification.ProjectClassificationBuisnessUnit IN (SELECT Id FROM dbo.OrganizationTaxonomy WHERE BuSequenceId IN @BuId)
                                    AND ProjectDefinition.ProjectScopeStatusId != '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'";

                var sb1 = new StringBuilder(selectPmProject);
                
                var db = connection
                    .Query<ProjectDefinition>(
                        sb1.ToString(),
                        new
                        {
                            BuId = resourceMatrixParameter.getResourceMatrixDto.BusinessUnit
                        }).ToList();

                foreach (var project in db)
                {
                    await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                    var data = pConnection
                        .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();

                    mResourceMatrixPmolData.AddRange(data);
                }            
            }
        }

        else
        {
            var connectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
                resourceMatrixParameter.ProjectSequenceId, resourceMatrixParameter.TenantProvider);
            
            await using var pConnection = new SqlConnection(connectionString);

            mResourceMatrixPmolData = pConnection
                .Query<ResourceMatrixPmolData>(pmolData, new {lang = resourceMatrixParameter.Lang}).ToList();
        }

        const string preferredCpcSql = @"SELECT
                                  HRHeader.Id
                                 ,HRHeader.CpcLabourItemId AS CpcId
                                 ,AbsenceHeader.StartDate 
                                 ,AbsenceHeader.EndDate
                                 ,DATEDIFF(DAY,AbsenceHeader.StartDate ,AbsenceHeader.EndDate) + 1 AS totalDays
                                FROM dbo.HRHeader
                                LEFT OUTER JOIN dbo.AbsenceHeader
                                  ON HRHeader.PersonId = AbsenceHeader.Person
                                  WHERE CpcLabourItemId IN @CpcId";
        
        var quarter = new List<string>();
        var resource = new Dictionary<string, CpcDataDto>();
        
        if (mResourceMatrixPmolData.Any())
        {
            var startDate = mResourceMatrixPmolData.Select(e => e.ExecutionDate).Min();
            var endDate = mResourceMatrixPmolData.Select(e => e.ExecutionDate).Max().AddYears(1);
            
            var quarterData = GetAllQuarters(endDate, startDate);
            
            var sDates = quarterData.Select(e => e.StartDate).ToList();
            
            quarter = quarterData.Select(e => e.Quarter).ToList();
            
            var mResourceMatrixPmolDataGroup = mResourceMatrixPmolData.GroupBy(e => e.Label).ToList();

            foreach (var i in mResourceMatrixPmolDataGroup)
            {
                var allCpcId = i.Select(e => e.CpcId).ToList();
                var cpcId = allCpcId.Distinct();
                var preferredCpc = connection.Query<PreferredCpcDto>(preferredCpcSql,new {cpcId}).ToList();
                var preferredCpcId = preferredCpc.Select(e => e.CpcId).ToList();

                if (preferredCpc.Any())
                {
                    var cpcGroup = i.GroupBy(e => e.Title).ToList();
                
                    var childData = new Dictionary<string, List<ChildData>>();
                    
                    foreach (var r in cpcGroup)
                    {
                        var preferredCpcSelect = preferredCpc.Where(e => e.CpcId == r.First().CpcId).ToList();

                        if (preferredCpcSelect.Any())
                        {
                            var preferredCpcGroup = preferredCpcSelect.GroupBy(e => e.Id).ToList();
                            var planed = new List<ChildData>();
                        
                            foreach (var sDate in sDates)
                            {
                                var eDate = sDate.AddMonths(3).AddDays(-1);
                                
                                var divider = preferredCpcGroup.Count * 480 - preferredCpcSelect.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                                
                                double sum = r.Where(e => e.ExecutionDate >= sDate && e.ExecutionDate <= eDate).ToList().Sum(e => e.RequiredQuantity);
                                var percentage = (sum/divider) * 100;
                                var mChildData = new ChildData
                                {
                                    Hours = sum.ToString(CultureInfo.InvariantCulture) + "/" + divider.ToString(),
                                    Percentage = percentage.ToString("0.00")
                                };
                                planed.Add(mChildData);
                            }  
                            childData.Add(r.Key,planed);
                        }
                    }

                    if (childData.Any())
                    {
                        var parentData = new List<ChildData>();
                        //var parentData = new List<string>();
                        foreach (var sDate in sDates)
                        {
                            var eDate = sDate.AddMonths(3).AddDays(-1);
                            var p = i.Where(e => preferredCpcId.Contains(e.CpcId)).ToList();
                            var preferredCpcUser = preferredCpc.Select(e => e.Id).Distinct().ToList();
                            var divider = preferredCpcUser.Count * 480 - preferredCpc.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                            double sum = p.Where(e => e.ExecutionDate >= sDate && e.ExecutionDate <= eDate).ToList().Sum(e => e.RequiredQuantity);
                            var percentage = (sum / divider) * 100;
                            
                            var mParentData = new ChildData
                            {
                                Hours = sum +"/"+divider,
                                Percentage = percentage.ToString("0.00")
                            };
                            
                            parentData.Add(mParentData);
                            //parentData.Add(sum +"/"+divider.ToString());
                        }
                    
                        var mCpcData = new CpcDataDto
                        {
                            ParentData = parentData,
                            Childs = childData
                        };
      
                        resource.Add(i.Key,mCpcData);
                    }
                }
            }
            
        }

        var mResourceMatrix = new ResourceMatrix
        {
            Week = quarter,
            Resource = resource
        };
       
        
        return mResourceMatrix;
    }
    
    public async Task<ResourceMatrix> GetResourceMatrixFromPbs(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);
        
        var mResourceMatrixPbsData = new List<ResourceMatrixPbsData>();
        
        const string preferredCpcSql = @"SELECT
                                  HRHeader.Id
                                 ,HRHeader.CpcLabourItemId AS CpcId
                                 ,AbsenceHeader.StartDate 
                                 ,AbsenceHeader.EndDate
                                 ,DATEDIFF(DAY,AbsenceHeader.StartDate ,AbsenceHeader.EndDate) + 1 AS totalDays
                                FROM dbo.HRHeader
                                LEFT OUTER JOIN dbo.AbsenceHeader
                                  ON HRHeader.PersonId = AbsenceHeader.Person
                                  WHERE CpcLabourItemId IN @cpcId";
        
        string pbsData = @"SELECT
                              PbsProduct.StartDate
                             ,PbsProduct.EndDate
                             ,CpcResourceFamilyLocalizedData.Label
                             ,CorporateProductCatalog.Title
                             ,CorporateProductCatalog.Id AS CpcId
                             ,PbsLabour.Quantity
                            FROM dbo.PbsLabour
                            INNER JOIN dbo.PbsProduct
                              ON PbsLabour.PbsProductId = PbsProduct.Id
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON PbsLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
                            RIGHT OUTER JOIN dbo.CpcResourceFamilyLocalizedData
                              ON CpcResourceFamilyLocalizedData.CpcResourceFamilyId = CorporateProductCatalog.ResourceFamilyId
                            WHERE PbsProduct.StartDate IS NOT NULL
                            AND PbsProduct.EndDate IS NOT NULL
                            AND PbsProduct.IsDeleted = 0
                            AND CpcResourceFamilyLocalizedData.Label IS NOT NULL
                            AND CpcResourceFamilyLocalizedData.LanguageCode = @lang
                            AND PbsLabour.Quantity IS NOT NULL ORDER BY CpcResourceFamilyLocalizedData.Label";

        if (resourceMatrixParameter.IsMyEnv)
        {
            var selectProject =
                @"SELECT ProjectDefinition.Id,ProjectDefinition.Title, ProjectDefinition.ProjectConnectionString,ProjectDefinition.SequenceCode,CabCompany.SequenceCode AS ContractingUnitId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE  ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.ProjectManagerId = (SELECT PersonId FROM CabPersonCompany LEFT OUTER JOIN CabPerson cp ON CabPersonCompany.PersonId = cp.Id WHERE Oid = @UserId AND cp.IsDeleted = 0 AND CabPersonCompany.IsDeleted = 0) ";

            var db = connection.Query<ProjectDefinition>(selectProject, new{resourceMatrixParameter.UserId}).ToList();

            foreach (var project in db)
            {
                await using var pConnection = new SqlConnection(project.ProjectConnectionString);
            
                var data1 = pConnection.Query<ResourceMatrixPbsData>(pbsData,new {lang = resourceMatrixParameter.Lang}).ToList();
            
                mResourceMatrixPbsData.AddRange(data1);
            }
        }
        
        else if (resourceMatrixParameter.IsCu)
        {
            if (resourceMatrixParameter.getResourceMatrixDto.BusinessUnit.Any())
            {
                var selectPmProject = @"SELECT
                                      ProjectDefinition.Id
                                     ,ProjectDefinition.Title
                                     ,ProjectDefinition.SequenceCode
                                     ,ProjectDefinition.ProjectConnectionString
                                     ,ProjectDefinition.ProjectManagerId
                                     ,ProjectDefinition.ProjectStatus
                                     ,CabCompany.SequenceCode AS ContractingUnitId
                                     ,ProjectDefinition.ProjectScopeStatusId
                                    FROM dbo.ProjectDefinition
                                    LEFT OUTER JOIN CabCompany
                                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                                    LEFT OUTER JOIN dbo.ProjectClassification
                                      ON ProjectDefinition.Id = ProjectClassification.ProjectId
                                    WHERE ProjectDefinition.IsDeleted = 0
                                    AND ProjectClassification.ProjectClassificationBuisnessUnit IN (SELECT Id FROM dbo.OrganizationTaxonomy WHERE BuSequenceId IN @BuId)
                                    AND ProjectDefinition.ProjectScopeStatusId != '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'";

                var sb1 = new StringBuilder(selectPmProject);
                
                var db = connection
                    .Query<ProjectDefinition>(
                        sb1.ToString(),
                        new
                        {
                            BuId = resourceMatrixParameter.getResourceMatrixDto.BusinessUnit
                        }).ToList();

                foreach (var project in db)
                {
                    await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                    var data1 = pConnection.Query<ResourceMatrixPbsData>(pbsData,new {lang = resourceMatrixParameter.Lang}).ToList();
            
                    mResourceMatrixPbsData.AddRange(data1);
                }            
            }
        }
        else
        {
            var connectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
                resourceMatrixParameter.ProjectSequenceId, resourceMatrixParameter.TenantProvider);
            
            await using var pConnection = new SqlConnection(connectionString);

            mResourceMatrixPbsData = pConnection
                .Query<ResourceMatrixPbsData>(pbsData, new {lang = resourceMatrixParameter.Lang}).ToList();
        }
        
        var weeks = new List<string>();
        var resource = new Dictionary<string, CpcDataDto>();
        
        if (mResourceMatrixPbsData.Any())
        { 
            var startDate = mResourceMatrixPbsData.Select(e => e.StartDate).Min();
            var endDate = mResourceMatrixPbsData.Select(e => e.EndDate).Max().AddDays(30);
            
            var sDates = new List<DateTime>();
        
            for (var i = startDate; i <= endDate;i = i.AddDays(7))
            {
                var week = GetIso8601WeekOfYear(i);

                var weekStart = FirstDateOfWeekIso8601(i.Year, week);
                
                var weekId = "W" + week.ToString()+" ("+weekStart.ToString("dd/MM")+"-"+weekStart.AddDays(6).ToString("dd/MM")+")";

                weeks.Add(weekId);
                
                sDates.Add(weekStart);
            }
            
            var mResourceMatrixPmolDataGroup = mResourceMatrixPbsData.GroupBy(e => e.Label).ToList();

            foreach (var i in mResourceMatrixPmolDataGroup)
            {
                var allCpcId = i.Select(e => e.CpcId).ToList();
                var cpcId = allCpcId.Distinct();
                var preferredCpc = connection.Query<PreferredCpcDto>(preferredCpcSql,new {cpcId}).ToList();

                var preferredCpcId = preferredCpc.Select(e => e.CpcId).ToList();

                if (preferredCpc.Any())
                {
                    var cpcGroup = i.GroupBy(e => e.Title).ToList();
                    
                    var childData = new Dictionary<string, List<ChildData>>();

                    foreach (var r in cpcGroup)
                    {
                        var preferredCpcSelect = preferredCpc.Where(e => e.CpcId == r.First().CpcId).ToList();
                        if (preferredCpcSelect.Any())
                        {
                            var preferredCpcGroup = preferredCpcSelect.GroupBy(e => e.Id).ToList();
                            var planed = new List<ChildData>();
                        
                            foreach (var sDate in sDates)
                            {
                                var eDate = sDate.AddDays(6);

                                double sum1 = r.Where(e => e.StartDate >= sDate && e.EndDate <= eDate).ToList()
                                    .Sum(e => e.Quantity);// this week pbs

                                var sum2List = r.Where(e =>
                                    e.StartDate >= sDate && e.StartDate <= eDate && e.EndDate > eDate || e.EndDate >= sDate && e.EndDate <= eDate && e.StartDate < sDate || e.StartDate < sDate && e.EndDate > eDate ).ToList(); // pbs duration more than one week

                                double sum2 = 0;
                                foreach (var b in sum2List)
                                {
                                    double totalDays = 0;
                                    double thisWeekDays = 0; // without weekends

                                    for (var j = b.StartDate; j <= b.EndDate; j =j.AddDays(1))
                                    {
                                        var day = j.DayOfWeek.ToString();
                                        if (day != "Saturday" && day != "Sunday")
                                        {
                                            totalDays = totalDays + 1;
                                            if (j >= sDate && j <= sDate.AddDays(4))
                                            {
                                                thisWeekDays = thisWeekDays + 1;
                                            }
                                        }
                                    }
                                    double thisWeekPercentage = thisWeekDays / totalDays * 100;
                                    sum2 = sum2 + b.Quantity * thisWeekPercentage / 100;
                                }
                                
                                var divider = preferredCpcGroup.Count * 40 - preferredCpcSelect.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                                
                                //double sum = r.Where(e => e.EndDate >= sDate && e.StartDate <= sDate || e.StartDate >= sDate && e.StartDate <= sDate.AddDays(6)).ToList().Sum(e => e.Quantity);
                                var sum = sum1 + sum2;
                                var percentage = (sum/divider) * 100;
                                var mChildData = new ChildData
                                {
                                    Hours = sum.ToString("0.00") + "/" + divider.ToString(),
                                    Percentage = percentage.ToString("0.00")
                                };
                                planed.Add(mChildData);
                            }  
                            childData.Add(r.Key,planed);
                        }
                    }
                    
                    var parentData = new List<ChildData>();
                    //var parentData = new List<string>();
                    foreach (var sDate in sDates)
                    {
                        var eDate = sDate.AddDays(6);
                        var p = i.Where(e => preferredCpcId.Contains(e.CpcId)).ToList();
                        var preferredCpcUser = preferredCpc.Select(e => e.Id).Distinct().ToList();
                        var divider = preferredCpcUser.Count * 40 - preferredCpc.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                        double sum = p.Where(e => e.EndDate >= sDate && e.StartDate <= sDate || e.StartDate >= sDate && e.StartDate <= sDate.AddDays(6)).ToList().Sum(e => e.Quantity);
                        var percentage = (sum / divider) * 100;
                            
                        var mParentData = new ChildData
                        {
                            Hours = sum +"/"+divider,
                            Percentage = percentage.ToString("0.00")
                        };
                            
                        parentData.Add(mParentData);
                        //parentData.Add(sum +"/"+divider.ToString());
                    }
                    
                    var mCpcData = new CpcDataDto
                    {
                        ParentData = parentData,
                        Childs = childData
                    };
      
                    resource.Add(i.Key,mCpcData);
                }
                
            }
            
        }

        var mResourceMatrix = new ResourceMatrix
        {
            Week = weeks,
            Resource = resource
        };
       
        
        return mResourceMatrix;
    }
    
    public async Task<ResourceMatrix> GetResourceMatrixFromPbsMonth(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);
        
        var mResourceMatrixPbsData = new List<ResourceMatrixPbsData>();
        
        const string preferredCpcSql = @"SELECT
                                  HRHeader.Id
                                 ,HRHeader.CpcLabourItemId AS CpcId
                                 ,AbsenceHeader.StartDate 
                                 ,AbsenceHeader.EndDate
                                 ,DATEDIFF(DAY,AbsenceHeader.StartDate ,AbsenceHeader.EndDate) + 1 AS totalDays
                                FROM dbo.HRHeader
                                LEFT OUTER JOIN dbo.AbsenceHeader
                                  ON HRHeader.PersonId = AbsenceHeader.Person
                                  WHERE CpcLabourItemId IN @CpcId";
        
        string pbsData = @"SELECT
                              PbsProduct.StartDate
                             ,PbsProduct.EndDate
                             ,CpcResourceFamilyLocalizedData.Label
                             ,CorporateProductCatalog.Title
                             ,CorporateProductCatalog.Id AS CpcId
                             ,PbsLabour.Quantity
                            FROM dbo.PbsLabour
                            INNER JOIN dbo.PbsProduct
                              ON PbsLabour.PbsProductId = PbsProduct.Id
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON PbsLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
                            RIGHT OUTER JOIN dbo.CpcResourceFamilyLocalizedData
                              ON CpcResourceFamilyLocalizedData.CpcResourceFamilyId = CorporateProductCatalog.ResourceFamilyId
                            WHERE PbsProduct.StartDate IS NOT NULL
                            AND PbsProduct.EndDate IS NOT NULL
                            AND PbsProduct.IsDeleted = 0
                            AND CpcResourceFamilyLocalizedData.Label IS NOT NULL
                            AND CpcResourceFamilyLocalizedData.LanguageCode = @lang
                            AND PbsLabour.Quantity IS NOT NULL ORDER BY CpcResourceFamilyLocalizedData.Label";

        if (resourceMatrixParameter.IsMyEnv)
        {
            var selectProject =
                @"SELECT ProjectDefinition.Id,ProjectDefinition.Title, ProjectDefinition.ProjectConnectionString,ProjectDefinition.SequenceCode,CabCompany.SequenceCode AS ContractingUnitId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE  ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.ProjectManagerId = (SELECT PersonId FROM CabPersonCompany LEFT OUTER JOIN CabPerson cp ON CabPersonCompany.PersonId = cp.Id WHERE Oid = @UserId AND cp.IsDeleted = 0 AND CabPersonCompany.IsDeleted = 0) ";

            var db = connection.Query<ProjectDefinition>(selectProject, new{resourceMatrixParameter.UserId}).ToList();

            foreach (var project in db)
            {
                await using var pConnection = new SqlConnection(project.ProjectConnectionString);
            
                var data1 = pConnection.Query<ResourceMatrixPbsData>(pbsData,new {lang = resourceMatrixParameter.Lang}).ToList();
            
                mResourceMatrixPbsData.AddRange(data1);
            }
        }else if (resourceMatrixParameter.IsCu)
        {
            if (resourceMatrixParameter.getResourceMatrixDto.BusinessUnit.Any())
            {
                var selectPmProject = @"SELECT
                                      ProjectDefinition.Id
                                     ,ProjectDefinition.Title
                                     ,ProjectDefinition.SequenceCode
                                     ,ProjectDefinition.ProjectConnectionString
                                     ,ProjectDefinition.ProjectManagerId
                                     ,ProjectDefinition.ProjectStatus
                                     ,CabCompany.SequenceCode AS ContractingUnitId
                                     ,ProjectDefinition.ProjectScopeStatusId
                                    FROM dbo.ProjectDefinition
                                    LEFT OUTER JOIN CabCompany
                                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                                    LEFT OUTER JOIN dbo.ProjectClassification
                                      ON ProjectDefinition.Id = ProjectClassification.ProjectId
                                    WHERE ProjectDefinition.IsDeleted = 0
                                    AND ProjectClassification.ProjectClassificationBuisnessUnit IN (SELECT Id FROM dbo.OrganizationTaxonomy WHERE BuSequenceId IN @BuId)
                                    AND ProjectDefinition.ProjectScopeStatusId != '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'";

                var sb1 = new StringBuilder(selectPmProject);
                
                var db = connection
                    .Query<ProjectDefinition>(
                        sb1.ToString(),
                        new
                        {
                            BuId = resourceMatrixParameter.getResourceMatrixDto.BusinessUnit
                        }).ToList();

                foreach (var project in db)
                {
                    await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                    var data1 = pConnection.Query<ResourceMatrixPbsData>(pbsData,new {lang = resourceMatrixParameter.Lang}).ToList();
            
                    mResourceMatrixPbsData.AddRange(data1);
                }            
            }
        }
        else
        {
            var connectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
                resourceMatrixParameter.ProjectSequenceId, resourceMatrixParameter.TenantProvider);
            
            await using var pConnection = new SqlConnection(connectionString);

            mResourceMatrixPbsData = pConnection
                .Query<ResourceMatrixPbsData>(pbsData, new {lang = resourceMatrixParameter.Lang}).ToList();
        }
        
        var months = new List<string>();
        var resource = new Dictionary<string, CpcDataDto>();
        
        if (mResourceMatrixPbsData.Any())
        { 
            var startDate = mResourceMatrixPbsData.Select(e => e.StartDate).Min();
            var endDate = mResourceMatrixPbsData.Select(e => e.EndDate).Max().AddMonths(4);
            
            var sDates = new List<DateTime>();

            for (var i = startDate; i <= endDate;i = i.AddMonths(1))
            {
                var month = i.ToString("MMMM") ;

                var monthStart = new DateTime(i.Year, i.Month, 1);
                var monthEnd = startDate.AddMonths(1).AddDays(-1);

                months.Add(month);
                
                sDates.Add(monthStart);
            }
            
            var mResourceMatrixPmolDataGroup = mResourceMatrixPbsData.GroupBy(e => e.Label).ToList();

            foreach (var i in mResourceMatrixPmolDataGroup)
            {
                var allCpcId = i.Select(e => e.CpcId).ToList();
                var cpcId = allCpcId.Distinct();
                var preferredCpc = connection.Query<PreferredCpcDto>(preferredCpcSql,new {cpcId}).ToList();

                var preferredCpcId = preferredCpc.Select(e => e.CpcId).ToList();

                if (preferredCpc.Any())
                {
                    var cpcGroup = i.GroupBy(e => e.Title).ToList();
                
                    var childData = new Dictionary<string, List<ChildData>>();
                    
                    foreach (var r in cpcGroup)
                    {
                        var planed = new List<ChildData>();
                        
                        var preferredCpcSelect = preferredCpc.Where(e => e.CpcId == r.First().CpcId).ToList();

                        if (preferredCpcSelect.Any())
                        {
                            var preferredCpcGroup = preferredCpcSelect.GroupBy(e => e.Id).ToList();
                        
                            foreach (var sDate in sDates)
                            {
                                var eDate = sDate.AddMonths(1).AddDays(-1);
                                
                                double sum1 = r.Where(e => e.StartDate >= sDate && e.EndDate <= eDate).ToList()
                                    .Sum(e => e.Quantity) * 8;// this week pbs

                                var sum2List = r.Where(e =>
                                    e.StartDate >= sDate && e.StartDate <= eDate && e.EndDate > eDate || e.EndDate >= sDate && e.EndDate <= eDate && e.StartDate < sDate || e.StartDate < sDate && e.EndDate > eDate ).ToList(); // pbs duration more than one week

                                double sum2 = 0;
                                foreach (var b in sum2List)
                                {
                                    double totalDays = 0;
                                    double thisWeekDays = 0; // without weekends

                                    for (var j = b.StartDate; j <= b.EndDate; j =j.AddDays(1))
                                    {
                                        if (j.DayOfWeek.ToString() != "Saturday" && j.DayOfWeek.ToString() != "Sunday")
                                        {
                                            totalDays = totalDays + 1;
                                            if (j >= sDate && j <= eDate)
                                            {
                                                thisWeekDays = thisWeekDays + 1;
                                            }
                                        }
                                    }
                                    double thisWeekPercentage = thisWeekDays / totalDays * 100;
                                    sum2 = sum2 + b.Quantity * thisWeekPercentage;
                                }
                                var divider = preferredCpcGroup.Count * 160 - preferredCpcSelect.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                                
                                //double sum = r.Where(e => e.EndDate >= sDate && e.StartDate <= sDate || e.StartDate >= sDate && e.StartDate <= sDate.AddMonths(1).AddDays(-1)).ToList().Sum(e => e.Quantity);
                                var sum = sum1 + sum2;
                                var percentage = (sum/divider) * 100;
                                var mChildData = new ChildData
                                {
                                    Hours = sum.ToString(CultureInfo.InvariantCulture) + "/" + divider.ToString(),
                                    Percentage = percentage.ToString("0.00")
                                };
                                planed.Add(mChildData);
                            }  
                            childData.Add(r.Key,planed);
                        }
                    }
                    
                    var parentData = new List<ChildData>();
                    //var parentData = new List<string>();
                    foreach (var sDate in sDates)
                    {
                        var p = i.Where(e => preferredCpcId.Contains(e.CpcId)).ToList();
                        var preferredCpcUser = preferredCpc.Select(e => e.Id).Distinct().ToList();
                        var divider = preferredCpcUser.Count * 160 - preferredCpc.Where(e => e.StartDate >= sDate && e.StartDate <= sDate.AddMonths(1).AddDays(-1)).ToList().Sum(e => e.TotalDays) * 8;
                        double sum = p.Where(e => e.EndDate >= sDate && e.StartDate <= sDate || e.StartDate >= sDate && e.StartDate <= sDate.AddMonths(1).AddDays(-1)).ToList().Sum(e => e.Quantity);
                        var percentage = (sum / divider) * 100;
                            
                         var mParentData = new ChildData
                         {
                             Hours = sum +"/"+divider,
                             Percentage = percentage.ToString("0.00")
                         };
                        parentData.Add(mParentData);
                        //parentData.Add(sum +"/"+divider.ToString());
                    }
                    
                    var mCpcData = new CpcDataDto
                    {
                        ParentData = parentData,
                        Childs = childData
                    };
      
                    resource.Add(i.Key,mCpcData);
                }
            }
            
        }

        var mResourceMatrix = new ResourceMatrix
        {
            Week = months,
            Resource = resource
        };
       
        
        return mResourceMatrix;
    }
    
    public async Task<ResourceMatrix> GetResourceMatrixFromPbsQuarter(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);
        
        var mResourceMatrixPbsData = new List<ResourceMatrixPbsData>();
        
        const string preferredCpcSql = @"SELECT
                                  HRHeader.Id
                                 ,HRHeader.CpcLabourItemId AS CpcId
                                 ,AbsenceHeader.StartDate 
                                 ,AbsenceHeader.EndDate
                                 ,DATEDIFF(DAY,AbsenceHeader.StartDate ,AbsenceHeader.EndDate) + 1 AS totalDays
                                FROM dbo.HRHeader
                                LEFT OUTER JOIN dbo.AbsenceHeader
                                  ON HRHeader.PersonId = AbsenceHeader.Person
                                  WHERE CpcLabourItemId IN @CpcId";
        
        string pbsData = @"SELECT
                              PbsProduct.StartDate
                             ,PbsProduct.EndDate
                             ,CpcResourceFamilyLocalizedData.Label
                             ,CorporateProductCatalog.Title
                             ,CorporateProductCatalog.Id AS CpcId
                             ,PbsLabour.Quantity
                            FROM dbo.PbsLabour
                            INNER JOIN dbo.PbsProduct
                              ON PbsLabour.PbsProductId = PbsProduct.Id
                            LEFT OUTER JOIN dbo.CorporateProductCatalog
                              ON PbsLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
                            RIGHT OUTER JOIN dbo.CpcResourceFamilyLocalizedData
                              ON CpcResourceFamilyLocalizedData.CpcResourceFamilyId = CorporateProductCatalog.ResourceFamilyId
                            WHERE PbsProduct.StartDate IS NOT NULL
                            AND PbsProduct.EndDate IS NOT NULL
                            AND PbsProduct.IsDeleted = 0
                            AND CpcResourceFamilyLocalizedData.Label IS NOT NULL
                            AND CpcResourceFamilyLocalizedData.LanguageCode = @lang
                            AND PbsLabour.Quantity IS NOT NULL ORDER BY CpcResourceFamilyLocalizedData.Label";

        if (resourceMatrixParameter.IsMyEnv)
        {
            var selectProject =
                @"SELECT ProjectDefinition.Id,ProjectDefinition.Title, ProjectDefinition.ProjectConnectionString,ProjectDefinition.SequenceCode,CabCompany.SequenceCode AS ContractingUnitId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id WHERE  ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.ProjectManagerId = (SELECT PersonId FROM CabPersonCompany LEFT OUTER JOIN CabPerson cp ON CabPersonCompany.PersonId = cp.Id WHERE Oid = @UserId AND cp.IsDeleted = 0 AND CabPersonCompany.IsDeleted = 0) ";

            var db = connection.Query<ProjectDefinition>(selectProject, new{resourceMatrixParameter.UserId}).ToList();

            foreach (var project in db)
            {
                await using var pConnection = new SqlConnection(project.ProjectConnectionString);
            
                var data1 = pConnection.Query<ResourceMatrixPbsData>(pbsData,new {lang = resourceMatrixParameter.Lang}).ToList();
            
                mResourceMatrixPbsData.AddRange(data1);
            }
        }else if (resourceMatrixParameter.IsCu)
        {
            if (resourceMatrixParameter.getResourceMatrixDto.BusinessUnit.Any())
            {
                var selectPmProject = @"SELECT
                                      ProjectDefinition.Id
                                     ,ProjectDefinition.Title
                                     ,ProjectDefinition.SequenceCode
                                     ,ProjectDefinition.ProjectConnectionString
                                     ,ProjectDefinition.ProjectManagerId
                                     ,ProjectDefinition.ProjectStatus
                                     ,CabCompany.SequenceCode AS ContractingUnitId
                                     ,ProjectDefinition.ProjectScopeStatusId
                                    FROM dbo.ProjectDefinition
                                    LEFT OUTER JOIN CabCompany
                                      ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                                    LEFT OUTER JOIN dbo.ProjectClassification
                                      ON ProjectDefinition.Id = ProjectClassification.ProjectId
                                    WHERE ProjectDefinition.IsDeleted = 0
                                    AND ProjectClassification.ProjectClassificationBuisnessUnit IN (SELECT Id FROM dbo.OrganizationTaxonomy WHERE BuSequenceId IN @BuId)
                                    AND ProjectDefinition.ProjectScopeStatusId != '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'";

                var sb1 = new StringBuilder(selectPmProject);
                
                var db = connection
                    .Query<ProjectDefinition>(
                        sb1.ToString(),
                        new
                        {
                            BuId = resourceMatrixParameter.getResourceMatrixDto.BusinessUnit
                        }).ToList();

                foreach (var project in db)
                {
                    await using var pConnection = new SqlConnection(project.ProjectConnectionString);
                
                    var data1 = pConnection.Query<ResourceMatrixPbsData>(pbsData,new {lang = resourceMatrixParameter.Lang}).ToList();
            
                    mResourceMatrixPbsData.AddRange(data1);
                }            
            }
        }
        else
        {
            var connectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
                resourceMatrixParameter.ProjectSequenceId, resourceMatrixParameter.TenantProvider);
            
            await using var pConnection = new SqlConnection(connectionString);

            mResourceMatrixPbsData = pConnection
                .Query<ResourceMatrixPbsData>(pbsData, new {lang = resourceMatrixParameter.Lang}).ToList();
        }

        var quarter = new List<string>();
        var resource = new Dictionary<string, CpcDataDto>();
        
        if (mResourceMatrixPbsData.Any())
        { 
            var startDate = mResourceMatrixPbsData.Select(e => e.StartDate).Min();
            var endDate = mResourceMatrixPbsData.Select(e => e.EndDate).Max().AddYears(1);

            var quarterData = GetAllQuarters(endDate, startDate);
            
            var sDates = quarterData.Select(e => e.StartDate).ToList();
            
            quarter = quarterData.Select(e => e.Quarter).ToList();
        
            var mResourceMatrixPmolDataGroup = mResourceMatrixPbsData.GroupBy(e => e.Label).ToList();

            foreach (var i in mResourceMatrixPmolDataGroup)
            {
                var allCpcId = i.Select(e => e.CpcId).ToList();
                var cpcId = allCpcId.Distinct();
                var preferredCpc = connection.Query<PreferredCpcDto>(preferredCpcSql,new {cpcId}).ToList();

                var preferredCpcId = preferredCpc.Select(e => e.CpcId).ToList();

                if (preferredCpc.Any())
                {
                    var cpcGroup = i.GroupBy(e => e.Title).ToList();
                    
                    var childData = new Dictionary<string, List<ChildData>>();
                    
                    foreach (var r in cpcGroup)
                    {
                        var preferredCpcSelect = preferredCpc.Where(e => e.CpcId == r.First().CpcId).ToList();
                        if (preferredCpcSelect.Any())
                        {
                            var preferredCpcGroup = preferredCpcSelect.GroupBy(e => e.Id).ToList();
                            var planed = new List<ChildData>();
                        
                            foreach (var sDate in sDates)
                            {
                                var eDate = sDate.AddMonths(3).AddDays(-1);
                                
                                var divider = preferredCpcGroup.Count * 480 - preferredCpcSelect.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                                
                                double sum = r.Where(e => e.EndDate >= sDate && e.StartDate <= sDate || e.StartDate >= sDate && e.StartDate <= sDate.AddMonths(3).AddDays(-1)).ToList().Sum(e => e.Quantity);
                                var percentage = (sum/divider) * 100;
                                var mChildData = new ChildData
                                {
                                    Hours = sum.ToString(CultureInfo.InvariantCulture) + "/" + divider.ToString(),
                                    Percentage = percentage.ToString("0.00")
                                };
                                planed.Add(mChildData);
                            }  
                            childData.Add(r.Key,planed);
                        }
                    }
                    
                    var parentData = new List<ChildData>();
                    //var parentData = new List<string>();
                    foreach (var sDate in sDates)
                    {
                        var eDate = sDate.AddMonths(3).AddDays(-1);
                        var p = i.Where(e => preferredCpcId.Contains(e.CpcId)).ToList();
                        var preferredCpcUser = preferredCpc.Select(e => e.Id).Distinct().ToList();
                        var divider = preferredCpcUser.Count * 480 - preferredCpc.Where(e => e.StartDate >= sDate && e.StartDate <= eDate).ToList().Sum(e => e.TotalDays) * 8;
                        double sum = p.Where(e => e.EndDate >= sDate && e.StartDate <= sDate || e.StartDate >= sDate && e.StartDate <= sDate.AddMonths(3).AddDays(-1)).ToList().Sum(e => e.Quantity);
                        var percentage = (sum / divider) * 100;
                             
                         var mParentData = new ChildData
                         {
                             Hours = sum +"/"+divider,
                             Percentage = percentage.ToString("0.00")
                         };
                            
                        parentData.Add(mParentData);
                        //parentData.Add(sum +"/"+divider.ToString());
                    }
                    
                    var mCpcData = new CpcDataDto
                    {
                        ParentData = parentData,
                        Childs = childData
                    };
      
                    resource.Add(i.Key,mCpcData);
                }
                
            }
            
        }

        var mResourceMatrix = new ResourceMatrix
        {
            Week = quarter,
            Resource = resource
        };
        return mResourceMatrix;
    }
    
    public async Task<LabourMatrix> GetLabourMatrixByDate(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);

        var cuHr = connection.Query<HRHeader>(@"SELECT * FROM dbo.HRHeader").ToList().DistinctBy(e => e.PersonId);

        var allAbsence = connection.Query<AbsenceHeader>(@"SELECT * FROM dbo.AbsenceHeader").ToList();

        var pearson = connection
            .Query<CabPerson>(
                @"SELECT cp.FullName,cpc.Id FROM CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id")
            .ToList();

        var cuConnectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
            null, resourceMatrixParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        var labourData = cuConnection
            .Query<PbsAssignedLabour>(@"SELECT Id ,PbsLabourId ,CabPersonId ,IsDeleted ,CpcId ,EndDate ,PbsProduct ,Project ,StartDate ,ROUND(AssignedHoursPerDay,2) AS AssignedHoursPerDay ,DayOfWeek ,ProjectManager ,Week ,Date FROM dbo.PbsAssignedLabour WHERE Date BETWEEN @StartDate AND @EndDate",
                new { resourceMatrixParameter.PbsDate.StartDate, resourceMatrixParameter.PbsDate.EndDate }).ToList();
        
        var startDate = resourceMatrixParameter.PbsDate.StartDate;
        var endDate = resourceMatrixParameter.PbsDate.EndDate;

        var daysList = new List<string>();
        var weekList = new List<WeekData>();
        for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
        {
            daysList.Add(j.ToString("ddd") + "-" + j.ToString("MMM dd"));
            var weekData = new WeekData()
            {
                Number = GetIso8601WeekOfYear(j),
                Year = j.Year
            };
            weekList.Add(weekData);
        }

        var labourMatrix = new LabourMatrix()
        {
            Days = daysList,
            Week = WeekWithMonth(weekList)
        };

        var resource = new Resource();

        var unAssigned = new UnAssigned();

        var unAssignedChildrenList = new List<ChildrenData>();

        var absence = new UnAssigned();

        var absenceChildrenList = new List<ChildrenData>();

        foreach (var l in cuHr)
        {
            var hoursCal = new List<double>();
            var absenceHCal = new List<double>();
            var hours = new List<string>();
            var absenceH = new List<string>();

            for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
            {
                var checkAbsence =
                    allAbsence.FirstOrDefault(e => e.Person == l.PersonId && e.StartDate <= j && j <= e.EndDate);
                if (checkAbsence != null)
                {
                    absenceHCal.Add(8);
                    hoursCal.Add(0);
                    absenceH.Add("8");
                    hours.Add("0");
                }
                else
                {
                    var h = 8 - labourData.Where(e => e.Date == j && e.CabPersonId == l.PersonId)
                        .Select(e => e.AssignedHoursPerDay).Sum();
                    if (h < 0)
                    {
                        h = 0;
                    }

                    hours.Add(Convert.ToString(Math.Round(h,1), CultureInfo.InvariantCulture));
                    absenceH.Add("0");
                    absenceHCal.Add(h);
                    hoursCal.Add(0);
                }
            }

            if (hoursCal.Any(x => x > 0))
            {
                var unAssignedChildren = new ChildrenData()
                {
                    Pearson = pearson.Where(e => e.Id == l.PersonId).Select(e => e.FullName).FirstOrDefault(),
                    Hours = hours,
                    HoursCal = hoursCal
                };
                unAssignedChildrenList.Add(unAssignedChildren);
            }

            if (absenceHCal.Any(x => x > 0))
            {
                var absenceChildren = new ChildrenData()
                {
                    Pearson = pearson.Where(e => e.Id == l.PersonId).Select(e => e.FullName).FirstOrDefault(),
                    Hours = absenceH,
                    HoursCal = absenceHCal
                };
                absenceChildrenList.Add(absenceChildren);
            }
        }

        var unAssignedParent = SumLists(unAssignedChildrenList.Select(e => e.HoursCal).ToList());
        var unAssignedParentData = new ParentData()
        {
            Hours = unAssignedParent.HoursString,
            HoursCal = unAssignedParent.HoursDouble
        };

        var absenceParent = SumLists(absenceChildrenList.Select(e => e.HoursCal).ToList());
        var absenceParentData = new ParentData()
        {
            Hours = absenceParent.HoursString,
            HoursCal = absenceParent.HoursDouble
        };

        unAssigned.Parent = unAssignedParentData;
        unAssigned.Children = unAssignedChildrenList;
        resource.UnAssigned = new List<UnAssigned> { unAssigned };

        absence.Parent = absenceParentData;
        absence.Children = absenceChildrenList;
        resource.Absent = new List<UnAssigned> { absence };

        var projectManagerDataList = new List<ProjectManagerData>();

        var projectManager = labourData.GroupBy(e => e.ProjectManager).ToList().OrderBy(e => e.First().Date);

        foreach (var n in projectManager)
        {
            var projectManagerData = new ProjectManagerData()
            {
                ProjectManager = n.Key
            };

            var managerProjectDataList = new List<ManagerProjectData>();

            var project = n.GroupBy(e => e.Project).ToList().OrderBy(e => e.First().Project).ThenBy(e => e.First().Date);

            foreach (var r in project)
            {
                var managerProjectData = new ManagerProjectData()
                {
                    Project = r.Key,
                };

                var projectChildrenList = new List<ChildrenData>();

                var person = r.GroupBy(e => e.CabPersonId).ToList().OrderBy(e => e.First().Date);

                foreach (var j in person)
                {
                    var projectChildren = new ChildrenData()
                    {
                        Pearson = pearson.Where(e => e.Id == j.Key).Select(e => e.FullName).FirstOrDefault(),
                    };

                    var totalAssignedHoursPerDay = new List<string>();
                    var totalAssignedHoursPerDayCal = new List<double>();
                    var totalAssignedHoursPercentagePerDay = new List<double>();
                    for (var k = startDate; k < endDate.AddDays(1); k = k.AddDays(1))
                    {
                        var assignedHoursPerDay = j.Where(e => e.Date == k).Select(e => e.AssignedHoursPerDay).Sum();
                        totalAssignedHoursPerDay.Add(Convert.ToString(Math.Round(assignedHoursPerDay,1), CultureInfo.InvariantCulture));
                        totalAssignedHoursPerDayCal.Add(assignedHoursPerDay);
                        var assignedHoursPercentagePerDay = (double)assignedHoursPerDay / 8 * 100;
                        totalAssignedHoursPercentagePerDay.Add(assignedHoursPercentagePerDay);

                    }

                    projectChildren.Hours = totalAssignedHoursPerDay;
                    projectChildren.HoursCal = totalAssignedHoursPerDayCal;
                    projectChildren.Percentage = totalAssignedHoursPercentagePerDay;
                    projectChildrenList.Add(projectChildren);
                }

                var projectParent = SumLists(projectChildrenList.Select(e => e.HoursCal).ToList());
                var projectParentData = new ParentData()
                {
                    Hours = projectParent.HoursString,
                    HoursCal = projectParent.HoursDouble
                };
                managerProjectData.Parent = projectParentData;
                managerProjectData.Children = projectChildrenList;
                managerProjectDataList.Add(managerProjectData);
            }

            var managerParentDataHours = SumLists(managerProjectDataList.Select(e => e.Parent.HoursCal).ToList());
            var managerParentData = new ParentData()
            {
                Hours = managerParentDataHours.HoursString,
                HoursCal = managerParentDataHours.HoursDouble
            };
            projectManagerData.Parent = managerParentData;
            projectManagerData.Children = managerProjectDataList;
            projectManagerDataList.Add(projectManagerData);
        }

        resource.ProjectManager = projectManagerDataList;
        labourMatrix.Resource = resource;
        var weekParentDataHours = SumLists(projectManagerDataList.Select(e => e.Parent.HoursCal).ToList());
        var weekParentData = new ParentData()
        {
            Hours = weekParentDataHours.HoursString
        };
        labourMatrix.Parent = weekParentData;
        return labourMatrix;
    }
    
    public async Task<LabourMatrix> GetLabourMatrixByWeek(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);

        var cuHr = connection.Query<HRHeader>(@"SELECT * FROM dbo.HRHeader").ToList().DistinctBy(e => e.PersonId);

        var allAbsence = connection.Query<AbsenceHeader>(@"SELECT * FROM dbo.AbsenceHeader").ToList();

        var pearson = connection
            .Query<CabPerson>(
                @"SELECT cp.FullName,cpc.Id FROM CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id")
            .ToList();

        var cuConnectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
            null, resourceMatrixParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        var labourData = cuConnection
            .Query<PbsAssignedLabour>(@"SELECT Id ,PbsLabourId ,CabPersonId ,IsDeleted ,CpcId ,EndDate ,PbsProduct ,Project ,StartDate ,ROUND(AssignedHoursPerDay,2) AS AssignedHoursPerDay ,DayOfWeek ,ProjectManager ,Week ,Date FROM dbo.PbsAssignedLabour WHERE Date BETWEEN @StartDate AND @EndDate",
                new { resourceMatrixParameter.PbsDate.StartDate, resourceMatrixParameter.PbsDate.EndDate }).ToList();
        
        var startDate = resourceMatrixParameter.PbsDate.StartDate;
        var endDate = resourceMatrixParameter.PbsDate.EndDate;

        var daysList = new List<string>();
        var weekList = new List<WeekData>();
        for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
        {
            daysList.Add(j.ToString("ddd") + "-" + j.ToString("MMM dd"));
            var weekData = new WeekData()
            {
                Number = GetIso8601WeekOfYear(j),
                Year = j.Year
            };
            weekList.Add(weekData);
        }

        var labourMatrix = new LabourMatrix()
        {
            Week = WeekWithMonth(weekList)
        };

        var resource = new Resource();

        var unAssigned = new UnAssigned();

        var unAssignedChildrenList = new List<ChildrenData>();

        var absence = new UnAssigned();

        var absenceChildrenList = new List<ChildrenData>();

        foreach (var l in cuHr)
        {
            var hoursCal = new List<double>();
            var absenceHCal = new List<double>();
            var hours = new List<string>();
            var absenceH = new List<string>();

            for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
            {
                var checkAbsence =
                    allAbsence.FirstOrDefault(e => e.Person == l.PersonId && e.StartDate <= j && j <= e.EndDate);
                if (checkAbsence != null)
                {
                    absenceH.Add("8");
                    hours.Add("0");
                    absenceHCal.Add(8);
                    hoursCal.Add(0);
                }
                else
                {
                    var h = 8 - labourData.Where(e => e.Date == j && e.CabPersonId == l.PersonId)
                        .Select(e => e.AssignedHoursPerDay).Sum();
                    if (h < 0)
                    {
                        h = 0;
                    }

                    hours.Add(Convert.ToString(Math.Round(h,1), CultureInfo.InvariantCulture));
                    absenceH.Add("0");
                    hoursCal.Add(h);
                    absenceHCal.Add(0);
                }
            }

            if (hoursCal.Any(x => x > 0))
            {
                var weekHours = WeekSumLists(hoursCal);
                var unAssignedChildren = new ChildrenData()
                {
                    Pearson = pearson.Where(e => e.Id == l.PersonId).Select(e => e.FullName).FirstOrDefault(),
                    Hours = weekHours.HoursString,
                    HoursCal = weekHours.HoursDouble
                };
                unAssignedChildrenList.Add(unAssignedChildren);
            }

            if (absenceHCal.Any(x => x > 0))
            {
                var weekHours = WeekSumLists(absenceHCal);
                var absenceChildren = new ChildrenData()
                {
                    Pearson = pearson.Where(e => e.Id == l.PersonId).Select(e => e.FullName).FirstOrDefault(),
                    Hours = weekHours.HoursString,
                    HoursCal = weekHours.HoursDouble
                };
                absenceChildrenList.Add(absenceChildren);
            }
        }

        var unAssignedParent = SumLists(unAssignedChildrenList.Select(e => e.HoursCal).ToList());
        var unAssignedParentData = new ParentData()
        {
            Hours = unAssignedParent.HoursString,
            HoursCal = unAssignedParent.HoursDouble
        };

        var absenceParent = SumLists(absenceChildrenList.Select(e => e.HoursCal).ToList());
        var absenceParentData = new ParentData()
        {
            Hours = absenceParent.HoursString,
            HoursCal = absenceParent.HoursDouble
        };

        unAssigned.Parent = unAssignedParentData;
        unAssigned.Children = unAssignedChildrenList;
        resource.UnAssigned = new List<UnAssigned> { unAssigned };

        absence.Parent = absenceParentData;
        absence.Children = absenceChildrenList;
        resource.Absent = new List<UnAssigned> { absence };

        var projectManagerDataList = new List<ProjectManagerData>();

        var projectManager = labourData.GroupBy(e => e.ProjectManager).ToList().OrderBy(e => e.First().Date);

        foreach (var n in projectManager)
        {
            var projectManagerData = new ProjectManagerData()
            {
                ProjectManager = n.Key
            };

            var managerProjectDataList = new List<ManagerProjectData>();

            var project = n.GroupBy(e => e.Project).ToList().OrderBy(e => e.First().Project).ThenBy(e => e.First().Date);

            foreach (var r in project)
            {
                var managerProjectData = new ManagerProjectData()
                {
                    Project = r.Key,
                };

                var projectChildrenList = new List<ChildrenData>();

                var person = r.GroupBy(e => e.CabPersonId).ToList().OrderBy(e => e.First().Date);

                foreach (var j in person)
                {
                    var projectChildren = new ChildrenData()
                    {
                        Pearson = pearson.Where(e => e.Id == j.Key).Select(e => e.FullName).FirstOrDefault(),
                    };

                    var totalAssignedHoursPerDay = new List<double>();
                    for (var k = startDate; k < endDate.AddDays(1); k = k.AddDays(1))
                    {
                        var assignedHoursPerDay = j.Where(e => e.Date == k).Select(e => e.AssignedHoursPerDay).Sum();
                        totalAssignedHoursPerDay.Add(assignedHoursPerDay);
                    }

                    var weekHours = WeekSumLists(totalAssignedHoursPerDay);
                    projectChildren.Hours = weekHours.HoursString;
                    projectChildren.HoursCal = weekHours.HoursDouble;
                    var weekHoursPercentageList = weekHours.HoursDouble.Select(wh => (double)wh / 40 * 100).ToList();

                    projectChildren.Percentage = weekHoursPercentageList;
                    projectChildrenList.Add(projectChildren);
                }

                var projectParent = SumLists(projectChildrenList.Select(e => e.HoursCal).ToList());
                var projectParentData = new ParentData()
                {
                    Hours = projectParent.HoursString,
                    HoursCal = projectParent.HoursDouble
                };
                managerProjectData.Parent = projectParentData;
                managerProjectData.Children = projectChildrenList;
                managerProjectDataList.Add(managerProjectData);
            }

            var managerParentDataHours = SumLists(managerProjectDataList.Select(e => e.Parent.HoursCal).ToList());
            var managerParentData = new ParentData()
            {
                Hours = managerParentDataHours.HoursString,
                HoursCal = managerParentDataHours.HoursDouble
            };
            projectManagerData.Parent = managerParentData;
            projectManagerData.Children = managerProjectDataList;
            projectManagerDataList.Add(projectManagerData);
        }

        resource.ProjectManager = projectManagerDataList;
        labourMatrix.Resource = resource;
        var weekParentDataHours = SumLists(projectManagerDataList.Select(e => e.Parent.HoursCal).ToList());
        var weekParentData = new ParentData()
        {
            Hours = weekParentDataHours.HoursString,
            HoursCal = weekParentDataHours.HoursDouble
        };
        labourMatrix.Parent = weekParentData;
        return labourMatrix;
    }

    public async Task<LabourMatrix> GetLabourMatrixByMonth(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);

        var cuHr = connection.Query<HRHeader>(@"SELECT * FROM dbo.HRHeader").ToList().DistinctBy(e => e.PersonId);

        var allAbsence = connection.Query<AbsenceHeader>(@"SELECT * FROM dbo.AbsenceHeader").ToList();

        var pearson = connection
            .Query<CabPerson>(
                @"SELECT cp.FullName,cpc.Id FROM CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id")
            .ToList();

        var cuConnectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
            null, resourceMatrixParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        var labourData = cuConnection
            .Query<PbsAssignedLabour>(@"SELECT Id ,PbsLabourId ,CabPersonId ,IsDeleted ,CpcId ,EndDate ,PbsProduct ,Project ,StartDate ,ROUND(AssignedHoursPerDay,2) AS AssignedHoursPerDay ,DayOfWeek ,ProjectManager ,Week ,Date FROM dbo.PbsAssignedLabour WHERE Date BETWEEN @StartDate AND @EndDate",
                new { resourceMatrixParameter.PbsDate.StartDate, resourceMatrixParameter.PbsDate.EndDate }).ToList();
        
        var startDate = resourceMatrixParameter.PbsDate.StartDate;
        var endDate = resourceMatrixParameter.PbsDate.EndDate;

        var daysList = new List<string>();
        var allMonthList = new List<WeekData>();
        for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
        {
            daysList.Add(j.ToString("ddd") + "-" + j.ToString("MMM dd"));
            var monthData = new WeekData()
            {
                Number = j.Month,
                Year = j.Year,
                Name = j.ToString("MMMM")
            };
            allMonthList.Add(monthData);
        }

        var monthList = allMonthList.DistinctBy(e => e.Name).ToList();
        var labourMatrix = new LabourMatrix()
        {
            Week = monthList.Select(e => e.Name).ToList()
        };

        var resource = new Resource();

        var unAssigned = new UnAssigned();

        var unAssignedChildrenList = new List<ChildrenData>();

        var absence = new UnAssigned();

        var absenceChildrenList = new List<ChildrenData>();

        foreach (var l in cuHr)
        {
            var hoursCal = new List<double>();
            var absenceHCal = new List<double>();
            var hours = new List<string>();
            var absenceH = new List<string>();

            for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
            {
                var checkAbsence =
                    allAbsence.FirstOrDefault(e => e.Person == l.PersonId && e.StartDate <= j && j <= e.EndDate);
                if (checkAbsence != null)
                {
                    absenceH.Add("8");
                    hours.Add("0");
                    absenceHCal.Add(8);
                    hoursCal.Add(0);
                }
                else
                {
                    var h = 8 - labourData.Where(e => e.Date == j && e.CabPersonId == l.PersonId)
                        .Select(e => e.AssignedHoursPerDay).Sum();
                    if (h < 0)
                    {
                        h = 0;
                    }

                    hoursCal.Add(h);
                    absenceHCal.Add(0);
                }
            }

            if (hoursCal.Any(x => x > 0))
            {
                var monthHours = MonthSumLists(hoursCal,monthList);
                var unAssignedChildren = new ChildrenData()
                {
                    Pearson = pearson.Where(e => e.Id == l.PersonId).Select(e => e.FullName).FirstOrDefault(),
                    Hours = monthHours.HoursString,
                    HoursCal = monthHours.HoursDouble
                };
                unAssignedChildrenList.Add(unAssignedChildren);
            }

            if (absenceHCal.Any(x => x > 0))
            {
                var monthHours = MonthSumLists(absenceHCal,monthList);
                var absenceChildren = new ChildrenData()
                {
                    Pearson = pearson.Where(e => e.Id == l.PersonId).Select(e => e.FullName).FirstOrDefault(),
                    Hours = monthHours.HoursString,
                    HoursCal = monthHours.HoursDouble
                };
                absenceChildrenList.Add(absenceChildren);
            }
        }

        var unAssignedParent = SumLists(unAssignedChildrenList.Select(e => e.HoursCal).ToList());
        var unAssignedParentData = new ParentData()
        {
            Hours = unAssignedParent.HoursString,
            HoursCal = unAssignedParent.HoursDouble
        };

        var absenceParent = SumLists(absenceChildrenList.Select(e => e.HoursCal).ToList());
        var absenceParentData = new ParentData()
        {
            Hours = absenceParent.HoursString,
            HoursCal = absenceParent.HoursDouble
        };

        unAssigned.Parent = unAssignedParentData;
        unAssigned.Children = unAssignedChildrenList;
        resource.UnAssigned = new List<UnAssigned> { unAssigned };

        absence.Parent = absenceParentData;
        absence.Children = absenceChildrenList;
        resource.Absent = new List<UnAssigned> { absence };

        var projectManagerDataList = new List<ProjectManagerData>();

        var projectManager = labourData.GroupBy(e => e.ProjectManager).ToList().OrderBy(e => e.First().Date);

        foreach (var n in projectManager)
        {
            var projectManagerData = new ProjectManagerData()
            {
                ProjectManager = n.Key
            };

            var managerProjectDataList = new List<ManagerProjectData>();

            var project = n.GroupBy(e => e.Project).ToList().OrderBy(e => e.First().Project).ThenBy(e => e.First().Date);

            foreach (var r in project)
            {
                var managerProjectData = new ManagerProjectData()
                {
                    Project = r.Key,
                };

                var projectChildrenList = new List<ChildrenData>();

                var person = r.GroupBy(e => e.CabPersonId).ToList().OrderBy(e => e.First().Date);

                foreach (var j in person)
                {
                    var projectChildren = new ChildrenData()
                    {
                        Pearson = pearson.Where(e => e.Id == j.Key).Select(e => e.FullName).FirstOrDefault(),
                    };

                    var totalAssignedHoursPerDay = new List<double>();
                    for (var k = startDate; k < endDate.AddDays(1); k = k.AddDays(1))
                    {
                        var assignedHoursPerDay = j.Where(e => e.Date == k).Select(e => e.AssignedHoursPerDay).Sum();
                        totalAssignedHoursPerDay.Add(assignedHoursPerDay);
                    }

                    var monthHours = MonthSumLists(totalAssignedHoursPerDay,monthList);
                    projectChildren.Hours = monthHours.HoursString;
                    projectChildren.HoursCal = monthHours.HoursDouble;
                    projectChildren.Percentage = monthHours.Percentage;
                    projectChildrenList.Add(projectChildren);
                }

                var projectParent = SumLists(projectChildrenList.Select(e => e.HoursCal).ToList());
                var projectParentData = new ParentData()
                {
                    Hours = projectParent.HoursString,
                    HoursCal = projectParent.HoursDouble
                };
                managerProjectData.Parent = projectParentData;
                managerProjectData.Children = projectChildrenList;
                managerProjectDataList.Add(managerProjectData);
            }

            var managerParentDataHours = SumLists(managerProjectDataList.Select(e => e.Parent.HoursCal).ToList());
            var managerParentData = new ParentData()
            {
                Hours = managerParentDataHours.HoursString,
                HoursCal = managerParentDataHours.HoursDouble
            };
            projectManagerData.Parent = managerParentData;
            projectManagerData.Children = managerProjectDataList;
            projectManagerDataList.Add(projectManagerData);
        }

        resource.ProjectManager = projectManagerDataList;
        labourMatrix.Resource = resource;
        var weekParentDataHours = SumLists(projectManagerDataList.Select(e => e.Parent.HoursCal).ToList());
        var weekParentData = new ParentData()
        {
            Hours = weekParentDataHours.HoursString,
            HoursCal = weekParentDataHours.HoursDouble
        };
        labourMatrix.Parent = weekParentData;
        return labourMatrix;
    }
    public async Task<OrganizationMatrix> GetOrganizationMatrixDay(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);
        
        var pearson = connection
            .Query<CabPerson>(
                @"SELECT cp.FullName,cpc.Id FROM CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id")
            .ToList();

        var allProjects =
            connection.Query<ProjectDefinition>(@"SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0").ToList();
        
        var organizationTaxonomy = connection
            .Query<OrganizationTaxonomy>(
                @"SELECT * FROM dbo.OrganizationTaxonomy")
            .ToList(); 

        var cuConnectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
            null, resourceMatrixParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        var labourData = cuConnection
            .Query<PbsAssignedLabour>(@"SELECT * FROM dbo.PbsAssignedLabour WHERE Date BETWEEN @StartDate AND @EndDate",
                new { resourceMatrixParameter.PbsDate.StartDate, resourceMatrixParameter.PbsDate.EndDate }).ToList();
        
        var startDate = resourceMatrixParameter.PbsDate.StartDate;
        var endDate = resourceMatrixParameter.PbsDate.EndDate;

        var daysList = new List<string>();
        var weekList = new List<WeekData>();
        for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
        {
            daysList.Add(j.ToString("ddd") + "-" + j.ToString("MMM dd"));
            var weekData = new WeekData()
            {
                Number = GetIso8601WeekOfYear(j),
                Year = j.Year
            };
            weekList.Add(weekData);
        }

        var personWithProjectList = new List<OrganizationMatrixPerson>();

        var person = organizationTaxonomy.Where(e => e.OrganizationTaxonomyLevelId == "we10e768-3e06-po02-b337-ee367a82adwe").Select(e => e.PersonId).DistinctBy(e => e).ToList();
        foreach (var i in person)
        {
            var projectList = new List<List<string>>();
            for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
            {
                var projects = labourData.Where(e => e.Date == j && e.CabPersonId == i).Select(e => e.Project)
                    .DistinctBy(e => e).ToList().OrderBy(e => e).ToList();
                var projectTitle = allProjects.Where(e => projects.Contains(e.SequenceCode)).Select(e => e.Title).DistinctBy(e => e).ToList();
                projectList.Add(projectTitle);
            }
            var personWithProject = new OrganizationMatrixPerson()
            {
                PersonId = i,
                PersonName = pearson.Where(e => e.Id == i).Select(e => e.FullName).FirstOrDefault(),
                Project = projectList
            };
            personWithProjectList.Add(personWithProject);
        }

        var organizationMatrix = new OrganizationMatrix()
        {
            Week = WeekWithMonth(weekList),
            Days = daysList
        };

        var organizationMatrixCuList = new List<OrganizationMatrixCu>();
        
        var cu = organizationTaxonomy
            .Where(e => e.OrganizationTaxonomyLevelId == "2210e768-3e06-po02-b337-ee367a82adjj").ToList();

        foreach (var i in cu)
        {
            var bu = organizationTaxonomy
                .Where(e => e.OrganizationTaxonomyLevelId == "oo10e768-3e06-po02-b337-ee367a82admn" && e.ParentId == i.Id).ToList();
            
            var organizationMatrixBuList = new List<OrganizationMatrixBu>();

            foreach (var j in bu)
            {
                var team = organizationTaxonomy
                    .Where(e => e.OrganizationTaxonomyLevelId == "fg10e768-3e06-po02-b337-ee367a82adfg" && e.ParentId == j.Id).ToList();
                
                var organizationMatrixTeamList = new List<OrganizationMatrixTeam>();

                foreach (var k in team)
                {
                    var personList = organizationTaxonomy
                        .Where(e => e.OrganizationTaxonomyLevelId == "we10e768-3e06-po02-b337-ee367a82adwe" && e.ParentId == k.Id).ToList().Select(e => e.PersonId).ToList();

                    var personData = personWithProjectList.Where(e => personList.Contains(e.PersonId)).ToList();
                    var organizationMatrixTeam = new OrganizationMatrixTeam()
                    {
                        TeamTitle = k.Title,
                        Person = personData
                    }; 
                    organizationMatrixTeamList.Add(organizationMatrixTeam);
                }
                
                var organizationMatrixBu = new OrganizationMatrixBu()
                {
                    BuTitle = j.Title,
                    Team = organizationMatrixTeamList
                };
                organizationMatrixBuList.Add(organizationMatrixBu);
            }
            
            var organizationMatrixCu = new OrganizationMatrixCu()
            {
                CuTitle = i.Title,
                Bu = organizationMatrixBuList
            };
            organizationMatrixCuList.Add(organizationMatrixCu);
        }

        organizationMatrix.Cu = organizationMatrixCuList;

        return organizationMatrix;
    }

    public async Task<OrganizationMatrix> GetOrganizationMatrixWeek(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);
        
        var pearson = connection
            .Query<CabPerson>(
                @"SELECT cp.FullName,cpc.Id FROM CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id")
            .ToList();
        
        var allProjects =
            connection.Query<ProjectDefinition>(@"SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0").ToList();
        
        var organizationTaxonomy = connection
            .Query<OrganizationTaxonomy>(
                @"SELECT * FROM dbo.OrganizationTaxonomy")
            .ToList(); 

        var cuConnectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
            null, resourceMatrixParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        var labourData = cuConnection
            .Query<PbsAssignedLabour>(@"SELECT * FROM dbo.PbsAssignedLabour WHERE Date BETWEEN @StartDate AND @EndDate",
                new { resourceMatrixParameter.PbsDate.StartDate, resourceMatrixParameter.PbsDate.EndDate }).ToList();
        
        var startDate = resourceMatrixParameter.PbsDate.StartDate;
        var endDate = resourceMatrixParameter.PbsDate.EndDate;

        var daysList = new List<string>();
        var weekList = new List<WeekData>();
        for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
        {
            daysList.Add(j.ToString("ddd") + "-" + j.ToString("MMM dd"));
            var weekData = new WeekData()
            {
                Number = GetIso8601WeekOfYear(j),
                Year = j.Year
            };
            weekList.Add(weekData);
        }

        var personWithProjectList = new List<OrganizationMatrixPerson>();

        var person = organizationTaxonomy.Where(e => e.OrganizationTaxonomyLevelId == "we10e768-3e06-po02-b337-ee367a82adwe").Select(e => e.PersonId).DistinctBy(e => e).ToList();
        foreach (var i in person)
        {
            var projectList = new List<List<string>>();
            for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(7))
            {
                var projects = labourData.Where(e => e.Date >= j && e.Date <= j.AddDays(6) && e.CabPersonId == i).Select(e => e.Project)
                    .DistinctBy(e => e).ToList().OrderBy(e => e).ToList();
                var projectTitle = allProjects.Where(e => projects.Contains(e.SequenceCode)).Select(e => e.Title).DistinctBy(e => e).ToList();
                projectList.Add(projectTitle);
            }
            var personWithProject = new OrganizationMatrixPerson()
            {
                PersonId = i,
                PersonName = pearson.Where(e => e.Id == i).Select(e => e.FullName).FirstOrDefault(),
                Project = projectList
            };
            personWithProjectList.Add(personWithProject);
        }

        var organizationMatrix = new OrganizationMatrix()
        {
            Week = WeekWithMonth(weekList)
        };

        var organizationMatrixCuList = new List<OrganizationMatrixCu>();
        
        var cu = organizationTaxonomy
            .Where(e => e.OrganizationTaxonomyLevelId == "2210e768-3e06-po02-b337-ee367a82adjj").ToList();

        foreach (var i in cu)
        {
            var bu = organizationTaxonomy
                .Where(e => e.OrganizationTaxonomyLevelId == "oo10e768-3e06-po02-b337-ee367a82admn" && e.ParentId == i.Id).ToList();
            
            var organizationMatrixBuList = new List<OrganizationMatrixBu>();

            foreach (var j in bu)
            {
                var team = organizationTaxonomy
                    .Where(e => e.OrganizationTaxonomyLevelId == "fg10e768-3e06-po02-b337-ee367a82adfg" && e.ParentId == j.Id).ToList();
                
                var organizationMatrixTeamList = new List<OrganizationMatrixTeam>();

                foreach (var k in team)
                {
                    var personList = organizationTaxonomy
                        .Where(e => e.OrganizationTaxonomyLevelId == "we10e768-3e06-po02-b337-ee367a82adwe" && e.ParentId == k.Id).ToList().Select(e => e.PersonId).ToList();

                    var personData = personWithProjectList.Where(e => personList.Contains(e.PersonId)).ToList();
                    var organizationMatrixTeam = new OrganizationMatrixTeam()
                    {
                        TeamTitle = k.Title,
                        Person = personData
                    }; 
                    organizationMatrixTeamList.Add(organizationMatrixTeam);
                }
                
                var organizationMatrixBu = new OrganizationMatrixBu()
                {
                    BuTitle = j.Title,
                    Team = organizationMatrixTeamList
                };
                organizationMatrixBuList.Add(organizationMatrixBu);
            }
            
            var organizationMatrixCu = new OrganizationMatrixCu()
            {
                CuTitle = i.Title,
                Bu = organizationMatrixBuList
            };
            organizationMatrixCuList.Add(organizationMatrixCu);
        }

        organizationMatrix.Cu = organizationMatrixCuList;

        return organizationMatrix;
    }
    
     public async Task<OrganizationMatrix> GetOrganizationMatrixMonth(ResourceMatrixParameter resourceMatrixParameter)
    {
        await using var connection =
            new SqlConnection(resourceMatrixParameter.TenantProvider.GetTenant().ConnectionString);
        
        var pearson = connection
            .Query<CabPerson>(
                @"SELECT cp.FullName,cpc.Id FROM CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id")
            .ToList();
        var allProjects =
            connection.Query<ProjectDefinition>(@"SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0").ToList();
        
        var organizationTaxonomy = connection
            .Query<OrganizationTaxonomy>(
                @"SELECT * FROM dbo.OrganizationTaxonomy")
            .ToList(); 

        var cuConnectionString = ConnectionString.MapConnectionString(resourceMatrixParameter.ContractingUnitSequenceId,
            null, resourceMatrixParameter.TenantProvider);

        await using var cuConnection = new SqlConnection(cuConnectionString);

        var labourData = cuConnection
            .Query<PbsAssignedLabour>(@"SELECT * FROM dbo.PbsAssignedLabour WHERE Date BETWEEN @StartDate AND @EndDate",
                new { resourceMatrixParameter.PbsDate.StartDate, resourceMatrixParameter.PbsDate.EndDate }).ToList();
        
        var startDate = resourceMatrixParameter.PbsDate.StartDate;
        var endDate = resourceMatrixParameter.PbsDate.EndDate;

        var daysList = new List<string>();
        var allMonthList = new List<WeekData>();
        for (var j = startDate; j < endDate.AddDays(1); j = j.AddDays(1))
        {
            daysList.Add(j.ToString("ddd") + "-" + j.ToString("MMM dd"));
            var monthData = new WeekData()
            {
                Number = j.Month,
                Year = j.Year,
                Name = j.ToString("MMMM")
            };
            allMonthList.Add(monthData);
        }

        var monthList = allMonthList.DistinctBy(e => e.Name).ToList();
        var personWithProjectList = new List<OrganizationMatrixPerson>();

        var person = organizationTaxonomy.Where(e => e.OrganizationTaxonomyLevelId == "we10e768-3e06-po02-b337-ee367a82adwe").Select(e => e.PersonId).DistinctBy(e => e).ToList();
        foreach (var i in person)
        {
            var projectList = new List<List<string>>();
            foreach (var m in monthList)
            {
                var mStartDate = new DateTime(m.Year, m.Number, 1);
                var mEndDate = mStartDate.AddMonths(1).AddDays(-1);   
                var projects = labourData.Where(e => e.Date >= mStartDate && e.Date <= mEndDate && e.CabPersonId == i).Select(e => e.Project)
                    .DistinctBy(e => e).ToList().OrderBy(e => e).ToList();
                var projectTitle = allProjects.Where(e => projects.Contains(e.SequenceCode)).Select(e => e.Title).DistinctBy(e => e).ToList();
                projectList.Add(projectTitle);
            }
            
            var personWithProject = new OrganizationMatrixPerson()
            {
                PersonId = i,
                PersonName = pearson.Where(e => e.Id == i).Select(e => e.FullName).FirstOrDefault(),
                Project = projectList
            };
            personWithProjectList.Add(personWithProject);
        }

        var organizationMatrix = new OrganizationMatrix()
        {
            Week = monthList.Select(e => e.Name).ToList()
        };

        var organizationMatrixCuList = new List<OrganizationMatrixCu>();
        
        var cu = organizationTaxonomy
            .Where(e => e.OrganizationTaxonomyLevelId == "2210e768-3e06-po02-b337-ee367a82adjj").ToList();

        foreach (var i in cu)
        {
            var bu = organizationTaxonomy
                .Where(e => e.OrganizationTaxonomyLevelId == "oo10e768-3e06-po02-b337-ee367a82admn" && e.ParentId == i.Id).ToList();
            
            var organizationMatrixBuList = new List<OrganizationMatrixBu>();

            foreach (var j in bu)
            {
                var team = organizationTaxonomy
                    .Where(e => e.OrganizationTaxonomyLevelId == "fg10e768-3e06-po02-b337-ee367a82adfg" && e.ParentId == j.Id).ToList();
                
                var organizationMatrixTeamList = new List<OrganizationMatrixTeam>();

                foreach (var k in team)
                {
                    var personList = organizationTaxonomy
                        .Where(e => e.OrganizationTaxonomyLevelId == "we10e768-3e06-po02-b337-ee367a82adwe" && e.ParentId == k.Id).ToList().Select(e => e.PersonId).ToList();

                    var personData = personWithProjectList.Where(e => personList.Contains(e.PersonId)).ToList();
                    var organizationMatrixTeam = new OrganizationMatrixTeam()
                    {
                        TeamTitle = k.Title,
                        Person = personData
                    }; 
                    organizationMatrixTeamList.Add(organizationMatrixTeam);
                }
                
                var organizationMatrixBu = new OrganizationMatrixBu()
                {
                    BuTitle = j.Title,
                    Team = organizationMatrixTeamList
                };
                organizationMatrixBuList.Add(organizationMatrixBu);
            }
            
            var organizationMatrixCu = new OrganizationMatrixCu()
            {
                CuTitle = i.Title,
                Bu = organizationMatrixBuList
            };
            organizationMatrixCuList.Add(organizationMatrixCu);
        }

        organizationMatrix.Cu = organizationMatrixCuList;

        return organizationMatrix;
    }


    public List<QuarterDto> GetAllQuarters(DateTime endDate, DateTime startDate)
    {
        var quarters = new List<QuarterDto>();

        for (DateTime i = startDate; i < endDate;i = i.AddMonths(3))
        {
            int quarterNumber = (i.Month-1)/3+1;
            DateTime firstDayOfQuarter = new DateTime(i.Year, (quarterNumber-1)*3+1,1);
            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);

            var qu = new QuarterDto()
            {
                Quarter = firstDayOfQuarter.ToString("MMMM")+"-"+lastDayOfQuarter.ToString("MMMM")+"(Q" + quarterNumber.ToString()+")",
                StartDate = firstDayOfQuarter,
                EndDate = lastDayOfQuarter
            };
            quarters.Add(qu);
        }

        return quarters;
    }

    private static HoursData SumLists(IReadOnlyCollection<List<double>> lists)
    {
        var hoursData = new HoursData();
        if (lists == null || lists.Count == 0)
        {
             var zeroList = new List<double>()
             {
                 0,0,0,0,0
             };
             var zeroListSting = new List<string>()
             {
                 "0","0","0","0","0"
             };
             
             hoursData.HoursDouble = zeroList;
             hoursData.HoursString = zeroListSting;
             return hoursData;
        }

        var maxLength = lists.Max(list => list.Count);
        var sumList = new List<double>();
        var sumListString = new List<string>();
        
        for (var i = 0; i < maxLength; i++)
        {
            var sum = lists.Where(list => i < list.Count).Sum(list => list[i]);
            sumList.Add(Math.Round(sum,2));
            sumListString.Add(Convert.ToString(Math.Round(sum,1), CultureInfo.InvariantCulture));
        }

        hoursData.HoursDouble = sumList;
        hoursData.HoursString = sumListString;
        return hoursData;
    }

    private static HoursData WeekSumLists(IReadOnlyCollection<double> list)
    {
        var hoursData = new HoursData();
        if (list.Count == 0)
        {
            var zeroList = new List<double>()
            {
                0, 0, 0, 0,
            };
            var zeroListString = new List<string>()
            {
                "0", "0", "0", "0",
            };
            hoursData.HoursDouble = zeroList;
            hoursData.HoursString = zeroListString;
            return hoursData;
        }
        
        var sumList = new List<double>();
        var sumListString = new List<string>();

        const int subListSize = 7;

        for (var i = 0; i < list.Count; i += subListSize)
        {
            var subList = list.Skip(i).Take(subListSize).ToList();
            var sum = subList.Sum();
            sumList.Add(Math.Round(sum,2));
            sumListString.Add(Convert.ToString(Math.Round(sum,1), CultureInfo.InvariantCulture));
        }

        hoursData.HoursString = sumListString;
        hoursData.HoursDouble = sumList;

        return hoursData;
    }
    
    private static List<string> WeekWithMonth(List<WeekData> weekList)
    {
        var weeks = new List<string>();

        var distinctWeeks = weekList.DistinctBy(e => e.Number).ToList();
        foreach (var i in distinctWeeks)
        {
            var weekStartDate = FirstDateOfWeekIso8601(i.Year, i.Number);
            var weekEndDate = weekStartDate.AddDays(6);

            // if (weekStartDate.Month == weekEndDate.Month)
            // {
            //     weeks.Add("Week-"+GetIso8601WeekOfYear(weekStartDate).ToString() +"-"+ weekStartDate.ToString("MMMM"));
            // }
            // else
            // {
            //     weeks.Add("Week-"+GetIso8601WeekOfYear(weekStartDate).ToString() +"-"+ weekStartDate.ToString("MMMM")+"/"+weekEndDate.ToString("MMMM"));
            // }
            
            weeks.Add("W"+GetIso8601WeekOfYear(weekStartDate).ToString() +"("+ weekStartDate.ToString("dd/MM")+"-"+weekEndDate.ToString("dd/MM")+")");
        }
        return weeks;
    }
    
    private static HoursWithPercentage MonthSumLists(IReadOnlyCollection<double> list,List<WeekData> monthList)
    {
        var hoursWithPercentage = new HoursWithPercentage();
        if (list.Count == 0)
        {
            var zeroList = new List<double>()
            {
                0, 0, 0, 0,
            };
            var zeroListString = new List<string>()
            {
                "0", "0", "0", "0",
            };
            hoursWithPercentage.HoursDouble = zeroList;
            hoursWithPercentage.HoursString = zeroListString;
            return hoursWithPercentage;
        }

        var sumList = new List<double>();
        var sumListString = new List<string>();
        var percentageList = new List<double>();
        var subListSize = 0;
        
        foreach (var m in monthList)
        {
            var mStartDate = new DateTime(m.Year, m.Number, 1);
            var mEndDate = mStartDate.AddMonths(1);
            var dif = mEndDate - mStartDate;
            var daysDifference = (int)dif.TotalDays;
            
            var subList = list.Skip(subListSize).Take(daysDifference).ToList();
            var sum = subList.Sum();
            sumList.Add(Math.Round(sum,2));
            sumListString.Add(Convert.ToString(Math.Round(sum,1), CultureInfo.InvariantCulture));
            percentageList.Add((double)daysDifference * 8 /100);
            subListSize = daysDifference + subListSize;
        }

        hoursWithPercentage.HoursDouble = sumList;
        hoursWithPercentage.HoursString = sumListString;
        hoursWithPercentage.Percentage = percentageList;
        return hoursWithPercentage;
    }
}