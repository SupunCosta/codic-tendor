using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using ServiceStack;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.VisualPlaane;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.VP;

public class MyCalenderRepository : IMyCalenderRepository
{
    public Task<IEnumerable<TeamsWithPmolDto>> Teams(MyCalenderParameter myCalenderParameter)
    {
        throw new NotImplementedException();
    }
    public IEnumerable<VpProjectWithPm> VPProjectPm(string connection)
    {
        var projectWithPm = @"SELECT
                                  ProjectTeam.ProjectId
                                 ,ProjectTeam.ContractingUnitId
                                 ,ProjectTeamRole.CabPersonId
                                 ,ProjectDefinition.SequenceCode
                                 ,ProjectDefinition.Name
                                 ,CabPerson.FullName
                                 ,ProjectDefinition.ProjectConnectionString
                                 ,ProjectDefinition.Title AS ProjectTitle
                                 ,CabPersonCompany.Oid
                                FROM dbo.ProjectTeamRole
                                INNER JOIN dbo.ProjectTeam
                                  ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
                                INNER JOIN dbo.ProjectDefinition
                                  ON ProjectTeam.ProjectId = ProjectDefinition.Id
                                INNER JOIN dbo.CabPerson
                                  ON ProjectTeamRole.CabPersonId = CabPerson.Id
                                INNER JOIN dbo.CabPersonCompany
                                  ON CabPersonCompany.PersonId = CabPerson.Id
                                WHERE ProjectDefinition.IsDeleted = 0
                                AND ProjectTeamRole.RoleId IN ('476127cb-62db-4af7-ac8e-d4a722f8e142', '266a5f47-3489-484b-8dae-e4468c5329dn3') AND Oid IS NOT NULL";

        IEnumerable<VpProjectWithPm> projectWithPmList = null;
        using (var connectionDb = new SqlConnection(connection))
        {
            try
            {
                connectionDb.Open();
                projectWithPmList = connectionDb.Query<VpProjectWithPm>(projectWithPm);
            }
            catch (Exception e)
            {
            }
        }

        return projectWithPmList;
    }
    
    public async Task<IEnumerable<TeamsWithPmolDto>> MyCalenderListDataForPerson(MyCalenderParameter myCalenderParameter)
    {
        var teamsWithPmol = new List<TeamsWithPmolDto>();

        using (var connection =
               new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString))
        {
            myCalenderParameter.GetTeamDto.CabPersonId = connection
                .Query<string>("SELECT PersonId FROM dbo.CabPersonCompany WHERE Oid = @UserId", new {myCalenderParameter.UserId})
                .FirstOrDefault();
            
            var teampmol =
                @"SELECT * FROM dbo.OrganizationTeamPmol WHERE ExecutionDate BETWEEN @Startdate AND @Enddate ";

            IEnumerable<OrganizationTeamPmol> mOrganizationTeamPmol;

            mOrganizationTeamPmol = connection.Query<OrganizationTeamPmol>(teampmol,
                    new { Startdate = myCalenderParameter.GetTeamDto.StartDate, Enddate = myCalenderParameter.GetTeamDto.EndDate })
                .ToList();

            //var teamgroup = mOrganizationTeamPmol.GroupBy(e => e.OrganizationTeamId);

            var teamsWithPmolDto = new TeamsWithPmolDto();
            var pmolDatalist = new List<PmolData>();
            var team = new List<PmolTeamMeber>();

            var ispmol = false;
            
            foreach (var r in mOrganizationTeamPmol)
            {
                IEnumerable<PomlVehicle> vehicles;
                    IEnumerable<PomlVehicle> tools;
                    var connectionString = ConnectionString.MapConnectionString(r.ContractingUnit,
                        r.Project, myCalenderParameter.TenantProvider);

                    using (var dbconnection =
                           new SqlConnection(connectionString))
                    {
                        var pbsList = dbconnection.Query<PbsProduct>("SELECT * FROM dbo.PbsProduct");

                        var pmolquery = @"SELECT
                                          PMol.ProjectMoleculeId
                                         ,PMol.Name
                                         ,PMol.Id
                                         ,PMol.ExecutionEndTime
                                         ,PMol.ExecutionStartTime
                                         ,PMol.ExecutionDate
                                         ,PMol.Title
                                         ,PMol.ProjectSequenceCode
                                         ,PMol.StatusId
                                         ,PMol.TypeId
                                         ,PMol.LocationId
                                         ,PMol.ProductId
                                        FROM dbo.PMol
                                        LEFT OUTER JOIN PMolPlannedWorkLabour ppwl ON PMol.Id = ppwl.PmolId
                                        LEft OUTER JOIN PmolTeamRole ptr ON ppwl.Id = ptr.PmolLabourId
                                        WHERE PMol.Id = @Id AND ptr.CabPersonId = @CabPersonId AND ptr.IsDeleted = 0";

                        var sb = new StringBuilder(pmolquery);
                        if (myCalenderParameter.GetTeamDto.PmolStatus != null)
                            sb.Append(" AND PMol.StatusId = '" + myCalenderParameter.GetTeamDto.PmolStatus + "'");

                        var pmolData1 = new PmolData
                        {
                            ProjectSequenceCode = r.Project,
                            ProjectTitle = connection
                                .Query<string>("SELECT Title FROM dbo.ProjectDefinition WHERE SequenceCode = @Project",
                                    new { r.Project }).FirstOrDefault()
                        };
                        var pmol = dbconnection.Query<Pmol>(sb.ToString(),
                            new { Id = r.PmolId, myCalenderParameter.GetTeamDto.CabPersonId }).FirstOrDefault();
                        if (pmol != null)
                        {
                            ispmol = true;
                            pmolData1.Id = pmol.Id;
                            pmolData1.Name = pmol.Name;
                            pmolData1.ExecutionStartTime = pmol.ExecutionStartTime;
                            pmolData1.ExecutionEndTime = pmol.ExecutionEndTime;
                            pmolData1.ExecutionDate = pmol.ExecutionDate.ToString();
                            pmolData1.Title = pmol.Title;
                            pmolData1.ContractingUinit = r.ContractingUnit;
                            pmolData1.TeamId = r.OrganizationTeamId;
                            pmolData1.ProjectMoleculeId = pmol.ProjectMoleculeId;
                            pmolData1.StatusId = pmol.StatusId;
                            pmolData1.TypeId = pmol.TypeId;
                            pmolData1.ProductId =
                                pbsList.FirstOrDefault(e => e.Id == pmol.ProductId)?.ProductId;
                            pmolData1.IsRFQGenerated = connection
                                .Query<PmolRfq>("Select * from PmolRfq Where PmolId = @PmolId",
                                    new { PmolId = pmol.Id })
                                .Any();

                            var selectProduct = @"with name_tree as
                                                             (SELECT
                                                               PbsProduct.Id
                                                              ,PbsProduct.Name
                                                              ,PbsProduct.Title
                                                              ,PbsProduct.ParentId
                                                             FROM dbo.PbsProduct
                                                             WHERE PbsProduct.Id = @Id
                                                               UNION ALL
                                                               SELECT c.Id, c.Name,CONCAT(c.Title,' > ',p.Title),c.ParentId
                                                               FROM dbo.PbsProduct c
                                                               JOIN name_tree p on p.ParentId = c.ProductId)
                                                               select Title
                                                               from name_tree WHERE ParentId IS NULL";

                            pmolData1.ProductTaxonomy = dbconnection
                                .Query<string>(selectProduct, new { Id = pmol.ProductId }).FirstOrDefault();

                            if (pmol.LocationId != null && pmol.ExecutionStartTime != null)
                            {
                                var mapLocation = dbconnection
                                    .Query<Position>(
                                        "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId WHERE ml.Id = @Id",
                                        new { Id = pmol.LocationId }).FirstOrDefault();

                                if (mapLocation != null)
                                    if (mapLocation.Lat != "0" || mapLocation.Lon != "0")
                                    {
                                        // var forecast = await VPParameter._iShiftRepository.GetWeatherForecast(
                                        //     mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                                        //     pmol.ExecutionDate.GetValueOrDefault(), pmol.ExecutionStartTime,
                                        //     VPParameter.TenantProvider, VPParameter.Configuration);
                                        //
                                        //
                                        // pmolData1.Forecast = forecast;
                                    }
                            }

                            var vehical =
                                @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0";

                            var tool =
                                @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber,PMolPlannedWorkTools.RequiredQuantity,PMolPlannedWorkTools.AllocatedQuantity FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0 AND CorporateProductCatalog.ResourceFamilyId != '0c355800-91fd-4d99-8010-921a42f0ba04'";

                            {
                                vehicles = dbconnection.Query<PomlVehicle>(vehical, new { Id = r.PmolId }).ToList();
                                tools = dbconnection.Query<PomlVehicle>(tool, new { Id = r.PmolId }).ToList();
                            }
                            if (vehicles.Any()) pmolData1.PomlVehical = (List<PomlVehicle>)vehicles;

                            if (tools.Any()) pmolData1.PomlTool = (List<PomlVehicle>)tools;

                            pmolDatalist.Add(pmolData1);
                        }
                    }
            }
            if (ispmol)
            {
                var teamquery = @"SELECT
                                              CabPerson.Id
                                             ,CabPerson.FullName AS Name
                                            FROM dbo.CabPerson
                                            WHERE CabPerson.Id = @Id";

               // List<PmolTeamMeber> mPmolTeamMeber;

                team = connection.Query<PmolTeamMeber>(teamquery, new { Id = myCalenderParameter.GetTeamDto.CabPersonId}).ToList();

                if (team.Count != 0)
                {
                    if (pmolDatalist.Count != 0)
                    {
                        pmolDatalist = pmolDatalist.OrderBy(c => c.ExecutionStartTime).ToList();
                        teamsWithPmolDto.Pmol = pmolDatalist;
                        teamsWithPmolDto.ProjectSequenceCode =
                            teamsWithPmolDto.Pmol.FirstOrDefault().ProjectSequenceCode;
                    }
                    teamsWithPmolDto.Team = team;
                    teamsWithPmol.Add(teamsWithPmolDto);
                }
            }
        }

        return teamsWithPmol;
    }
    
    public async Task<IEnumerable<TeamsWithPmolDto>> MyCalenderListData(MyCalenderParameter myCalenderParameter)
    {
        var projectManager = VPProjectPm(myCalenderParameter.TenantProvider.GetTenant().ConnectionString);

        var pp = projectManager.Where(r => r.Oid == myCalenderParameter.UserId).ToList();

        if (myCalenderParameter.CalenderGetTeamDto.StartDate ==  null)
        {
            myCalenderParameter.CalenderGetTeamDto.StartDate = DateTime.Today
                .AddDays(-(int)DateTime.Today.DayOfWeek).AddDays(1).Date;
            myCalenderParameter.CalenderGetTeamDto.EndDate = DateTime.Today
                .AddDays(-(int)DateTime.Today.DayOfWeek).AddDays(7).Date;
        }

        var teamsWithPmol = new List<TeamsWithPmolDto>();

        using (var connection =
               new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString))
        {
            const string data = @"SELECT
                                  OrganizationTeamPmol.PmolId
                                 ,OrganizationTeamPmol.Project
                                 ,OrganizationTaxonomy.Title
                                 ,OrganizationTaxonomy.PersonId
                                 ,OrganizationTeamPmol.ContractingUnit
                                 ,OrganizationTeamPmol.OrganizationTeamId
                                FROM dbo.OrganizationTeamPmol
                                RIGHT OUTER JOIN dbo.OrganizationTaxonomy
                                  ON OrganizationTaxonomy.ParentId = OrganizationTeamPmol.OrganizationTeamId
                                WHERE OrganizationTeamPmol.ExecutionDate BETWEEN @Startdate AND @Enddate AND OrganizationTeamPmol.Project IN @Project
                                GROUP BY OrganizationTeamPmol.PmolId
                                        ,OrganizationTeamPmol.Project
                                        ,OrganizationTaxonomy.Title
                                        ,OrganizationTaxonomy.PersonId
                                        ,OrganizationTeamPmol.ContractingUnit
                                        ,OrganizationTeamPmol.OrganizationTeamId
                                ORDER BY OrganizationTeamPmol.PmolId
                                ";
            
            var person = connection.Query<MyCalenderListDataDto>(data,
                new
                {
                    myCalenderParameter.CalenderGetTeamDto.StartDate, myCalenderParameter.CalenderGetTeamDto.EndDate,
                    Project = pp.Select(e => e.SequenceCode).ToList()
                }).ToList();
            
            if (!person.Any())
            {
                if (myCalenderParameter.GetTeamDto !=null)
                {
                    var mteamsWithPmol = await MyCalenderListDataForPerson(myCalenderParameter);
                    return mteamsWithPmol;
                }
            }

            var groupByPerson = person.GroupBy(e => e.PersonId).ToList();
            
            foreach (var p in groupByPerson)
            {
                var teamsWithPmolDto = new TeamsWithPmolDto();
                var pmolDatalist = new List<PmolData>();
                var team = new List<PmolTeamMeber>();
                IEnumerable<PomlVehicle> vehicals;
                IEnumerable<PomlVehicle> tools;

                foreach (var r in p)
                {
                    var connectionString = ConnectionString.MapConnectionString(r.ContractingUnit,
                            r.Project, myCalenderParameter.TenantProvider);

                    await using var dbconnection =
                        new SqlConnection(connectionString);
                    
                    var pbsList = dbconnection.Query<PbsProduct>("SELECT * FROM dbo.PbsProduct");
                    //var rfqPmolList = connection.Query<PmolRfq>("Select * from PmolRfq");

                    var vehical =
                        @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0";

                    var tool =
                        @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber,PMolPlannedWorkTools.RequiredQuantity,PMolPlannedWorkTools.AllocatedQuantity FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0 AND CorporateProductCatalog.ResourceFamilyId != '0c355800-91fd-4d99-8010-921a42f0ba04'";

                    var pmolquery =
                        @"SELECT PMol.ProjectMoleculeId ,PMol.Name ,PMol.Id ,PMol.ExecutionEndTime ,PMol.ExecutionStartTime ,PMol.ExecutionDate ,PMol.Title ,PMol.ProjectSequenceCode,PMol.StatusId,PMol.TypeId , PMol.LocationId,PMol.ProductId FROM dbo.PMol WHERE Id = @Id";

                    var sb = new StringBuilder(pmolquery);

                    if (myCalenderParameter.CalenderGetTeamDto.PmolStatus != null)
                        sb.Append(" AND PMol.StatusId = '" + myCalenderParameter.CalenderGetTeamDto.PmolStatus + "'");
                    var pmolData1 = new PmolData
                    {
                        ProjectSequenceCode = r.Project,
                        ProjectTitle = connection
                            .Query<string>(
                                "SELECT Title FROM dbo.ProjectDefinition WHERE SequenceCode = @Project",
                                new { r.Project }).FirstOrDefault()
                    };
                    {
                        vehicals = dbconnection.Query<PomlVehicle>(vehical, new { Id = r.PmolId }).ToList();
                        tools = dbconnection.Query<PomlVehicle>(tool, new { Id = r.PmolId }).ToList();
                    }
                    if (vehicals.Any()) pmolData1.PomlVehical = (List<PomlVehicle>)vehicals;

                    if (tools.Any()) pmolData1.PomlTool = (List<PomlVehicle>)tools;

                    var pmol = dbconnection.Query<Pmol>(sb.ToString(), new { Id = r.PmolId }).FirstOrDefault();

                    if (pmol != null)
                    {
                        pmolData1.Id = pmol.Id;
                        pmolData1.Name = pmol.Name;
                        pmolData1.ExecutionStartTime = pmol.ExecutionStartTime;
                        pmolData1.ExecutionEndTime = pmol.ExecutionEndTime;
                        pmolData1.ExecutionDate = pmol.ExecutionDate.ToString();
                        pmolData1.Title = pmol.Title;
                        pmolData1.ContractingUinit = r.ContractingUnit;
                        pmolData1.TeamId = r.OrganizationTeamId;
                        pmolData1.ProjectMoleculeId = pmol.ProjectMoleculeId;
                        pmolData1.StatusId = pmol.StatusId;
                        pmolData1.TypeId = pmol.TypeId;
                        pmolData1.ProjectSequenceId = r.Project;
                        pmolData1.ProductId =
                            pbsList.FirstOrDefault(e => e.Id == pmol.ProductId)?.ProductId;
                        pmolData1.IsRFQGenerated = connection
                            .Query<PmolRfq>("Select * from PmolRfq Where PmolId = @PmolId",
                                new { PmolId = pmol.Id }).Any();

                        var selectProduct = @"with name_tree as
                                                             (SELECT
                                                               PbsProduct.Id
                                                              ,PbsProduct.Name
                                                              ,PbsProduct.Title
                                                              ,PbsProduct.ParentId
                                                             FROM dbo.PbsProduct
                                                             WHERE PbsProduct.Id = @Id
                                                               UNION ALL
                                                               SELECT c.Id, c.Name,CONCAT(c.Title,' > ',p.Title),c.ParentId
                                                               FROM dbo.PbsProduct c
                                                               JOIN name_tree p on p.ParentId = c.ProductId)
                                                               select Title
                                                               from name_tree WHERE ParentId IS NULL";

                        pmolData1.ProductTaxonomy = dbconnection
                            .Query<string>(selectProduct, new { Id = pmol.ProductId }).FirstOrDefault();


                        if (pmol.LocationId != null && pmol.ExecutionStartTime != null)
                        {
                            var mapLocation = dbconnection
                                .Query<Position>(
                                    "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId WHERE ml.Id = @Id",
                                    new { Id = pmol.LocationId }).FirstOrDefault();

                            if (mapLocation != null)
                                if (mapLocation.Lat != "0" || mapLocation.Lon != "0")
                                {
                                    // var forecast = await myCalenderParameter.iShiftRepository.GetWeatherForecast(
                                    //     mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                                    //     pmol.ExecutionDate.GetValueOrDefault(), pmol.ExecutionStartTime,
                                    //     myCalenderParameter.TenantProvider, myCalenderParameter.Configuration);
                                    //
                                    //
                                    // pmolData1.Forecast = forecast;
                                }
                        }

                        pmolDatalist.Add(pmolData1);
                    }
                }
                
                var teamquery = @"SELECT CabPerson.Id ,CabPerson.FullName AS Name FROM dbo.CabPerson LEFT OUTER JOIN dbo.CabPersonCompany ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Id = @Id AND (CabPersonCompany.Oid IS NULL OR CabPersonCompany.Oid != @UserId)";

                  //  List<PmolTeamMeber> mPmolTeamMeber;

                    team = connection.Query<PmolTeamMeber>(teamquery, new { Id = p.Key,UserId = myCalenderParameter.UserId }).ToList();

                    if (team.Count != 0)
                    {
                        teamsWithPmolDto.TeamId = p.First().OrganizationTeamId;
                        
                        var cu = @"with name_tree as
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
                                          where OrganizationTaxonomyLevelId = '2210e768-3e06-po02-b337-ee367a82adjj'";
                        var cuId = connection.Query<OrganizationTaxonomy>(cu, new {Id = p.First().OrganizationTeamId }).FirstOrDefault();

                        if (pmolDatalist.Count != 0)
                        {
                            pmolDatalist = pmolDatalist.OrderBy(c => c.ExecutionStartTime).ToList();
                            teamsWithPmolDto.Pmol = pmolDatalist;
                            teamsWithPmolDto.ProjectSequenceCode =
                                teamsWithPmolDto.Pmol.FirstOrDefault().ProjectSequenceCode;
                        }

                        teamsWithPmolDto.ContractingUinit = cuId?.Title;
                        teamsWithPmolDto.Team = team;
                        teamsWithPmol.Add(teamsWithPmolDto);
                    }
                
            }

        }

        if (myCalenderParameter.GetTeamDto != null)
        {
            var personWithPmol = await MyCalenderListDataForPerson(myCalenderParameter);
            teamsWithPmol.AddRange(personWithPmol);
        }
        
        teamsWithPmol = teamsWithPmol.OrderBy(c => c.TeamTitle).ToList();
        return teamsWithPmol;
    }
    
    public async Task<IEnumerable<TeamsWithPmolDto>> MyCalenderListDataForCu(MyCalenderParameter myCalenderParameter)
    {
        var teamsWithPmol = new List<TeamsWithPmolDto>();
        if (myCalenderParameter.MyCalenderGetTeamDto.Type == "1")
        {
            teamsWithPmol =  await MyCalenderListDataForCuPerson(myCalenderParameter);
        }
        
        if (myCalenderParameter.MyCalenderGetTeamDto.Type == "2")
        {
            teamsWithPmol =  await MyCalenderListDataForCuTeam(myCalenderParameter);
        }
        
        if (myCalenderParameter.MyCalenderGetTeamDto.Type == "3")
        {
            teamsWithPmol =  await MyCalenderListDataForCuBu(myCalenderParameter);
        }
        
        if (myCalenderParameter.MyCalenderGetTeamDto.Type == "4")
        {
            teamsWithPmol =  await MyCalenderListDataForCuCu(myCalenderParameter);
        }

        return teamsWithPmol;
    }

    public async Task<List<TeamsWithPmolDto>> MyCalenderListDataForCuPerson(MyCalenderParameter myCalenderParameter)
    {
        var teamsWithPmol = new List<TeamsWithPmolDto>();

        using (var connection = new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString))
        {
            string personSql1 = @"SELECT
                                  *,@PersonId AS PersonId
                                FROM dbo.OrganizationTeamPmol
                                WHERE OrganizationTeamPmol.ExecutionDate BETWEEN @Startdate AND @Enddate
                                AND OrganizationTeamPmol.OrganizationTeamId IN (SELECT
                                    OrganizationTaxonomy.Id
                                  FROM dbo.OrganizationTaxonomy
                                  LEFT OUTER JOIN OrganizationTaxonomy ot
                                    ON OrganizationTaxonomy.Id = ot.ParentId
                                  RIGHT OUTER JOIN OrganizationTaxonomy ot1
                                    ON OrganizationTaxonomy.ParentId = ot1.Id
                                  WHERE ot.PersonId = @PersonId
                                  AND ot1.Id = @BuId)
                                ORDER BY OrganizationTeamPmol.PmolId";

            string personSql2 = @"SELECT
                                  OrganizationTeamPmol.PmolId
                                 ,OrganizationTeamPmol.Project
                                 ,OrganizationTaxonomy.Title
                                 ,OrganizationTaxonomy.PersonId
                                 ,OrganizationTeamPmol.ContractingUnit
                                 ,OrganizationTeamPmol.OrganizationTeamId
                                FROM dbo.OrganizationTeamPmol
                                RIGHT OUTER JOIN dbo.OrganizationTaxonomy
                                  ON OrganizationTaxonomy.ParentId = OrganizationTeamPmol.OrganizationTeamId
                                WHERE OrganizationTeamPmol.ExecutionDate BETWEEN @Startdate AND @Enddate AND OrganizationTaxonomy.PersonId = @PersonId
                                ORDER BY OrganizationTaxonomy.Title";

            var pmolData = new List<MyCalenderPmolData>();
            if (myCalenderParameter.MyCalenderGetTeamDto.BuId != null)
            {
                pmolData = connection.Query<MyCalenderPmolData>(personSql1,
                    new
                    {
                        myCalenderParameter.MyCalenderGetTeamDto.StartDate,
                        myCalenderParameter.MyCalenderGetTeamDto.EndDate,
                        myCalenderParameter.MyCalenderGetTeamDto.BuId,
                        PersonId = myCalenderParameter.MyCalenderGetTeamDto.Id
                    }).ToList();
            }

            else
            {
                pmolData = connection.Query<MyCalenderPmolData>(personSql2,
                    new
                    {
                        myCalenderParameter.MyCalenderGetTeamDto.StartDate,
                        myCalenderParameter.MyCalenderGetTeamDto.EndDate,
                        CuId = myCalenderParameter.ContractingUnitSequenceId,
                        PersonId = myCalenderParameter.MyCalenderGetTeamDto.Id
                    }).ToList();
            }

            

            var teamsWithPmolDto = new TeamsWithPmolDto();
            var pmolDatalist = new List<PmolData>();
            var team = new List<PmolTeamMeber>();
            IEnumerable<PomlVehicle> vehicals;
            IEnumerable<PomlVehicle> tools;

            foreach (var r in pmolData)
            {
                var connectionString = ConnectionString.MapConnectionString(r.ContractingUnit,
                    r.Project, myCalenderParameter.TenantProvider);

                await using var dbconnection =
                    new SqlConnection(connectionString);

                var pbsList = dbconnection.Query<PbsProduct>("SELECT * FROM dbo.PbsProduct");
                var rfqPmolList = connection.Query<PmolRfq>("Select * from PmolRfq");

                var vehical =
                    @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0";

                var tool =
                    @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber,PMolPlannedWorkTools.RequiredQuantity,PMolPlannedWorkTools.AllocatedQuantity FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0 AND CorporateProductCatalog.ResourceFamilyId != '0c355800-91fd-4d99-8010-921a42f0ba04'";

                var pmolquery =
                    @"SELECT PMol.ProjectMoleculeId ,PMol.Name ,PMol.Id ,PMol.ExecutionEndTime ,PMol.ExecutionStartTime ,PMol.ExecutionDate ,PMol.Title ,PMol.ProjectSequenceCode,PMol.StatusId,PMol.TypeId , PMol.LocationId,PMol.ProductId FROM dbo.PMol WHERE Id = @Id";

                var sb = new StringBuilder(pmolquery);
                
                var pmolData1 = new PmolData
                {
                    ProjectSequenceCode = r.Project,
                    ProjectTitle = connection
                        .Query<string>(
                            "SELECT Title FROM dbo.ProjectDefinition WHERE SequenceCode = @Project",
                            new {r.Project}).FirstOrDefault()
                };
                {
                    vehicals = dbconnection.Query<PomlVehicle>(vehical, new {Id = r.PmolId}).ToList();
                    tools = dbconnection.Query<PomlVehicle>(tool, new {Id = r.PmolId}).ToList();
                }
                if (vehicals.Any()) pmolData1.PomlVehical = (List<PomlVehicle>) vehicals;

                if (tools.Any()) pmolData1.PomlTool = (List<PomlVehicle>) tools;

                var pmol = dbconnection.Query<Pmol>(sb.ToString(), new {Id = r.PmolId}).FirstOrDefault();

                if (pmol != null)
                {
                    pmolData1.Id = pmol.Id;
                    pmolData1.Name = pmol.Name;
                    pmolData1.ExecutionStartTime = pmol.ExecutionStartTime;
                    pmolData1.ExecutionEndTime = pmol.ExecutionEndTime;
                    pmolData1.ExecutionDate = pmol.ExecutionDate.ToString();
                    pmolData1.Title = pmol.Title;
                    pmolData1.ContractingUinit = r.ContractingUnit;
                    pmolData1.TeamId = r.OrganizationTeamId;
                    pmolData1.ProjectMoleculeId = pmol.ProjectMoleculeId;
                    pmolData1.StatusId = pmol.StatusId;
                    pmolData1.TypeId = pmol.TypeId;
                    pmolData1.ProjectSequenceId = r.Project;
                    pmolData1.ProductId =
                        pbsList.FirstOrDefault(e => e.Id == pmol.ProductId)?.ProductId;
                    pmolData1.IsRFQGenerated = connection
                        .Query<PmolRfq>("Select * from PmolRfq Where PmolId = @PmolId",
                            new {PmolId = pmol.Id}).Any();

                    var selectProduct = @"with name_tree as
                                                                 (SELECT
                                                                   PbsProduct.Id
                                                                  ,PbsProduct.Name
                                                                  ,PbsProduct.Title
                                                                  ,PbsProduct.ParentId
                                                                 FROM dbo.PbsProduct
                                                                 WHERE PbsProduct.Id = @Id
                                                                   UNION ALL
                                                                   SELECT c.Id, c.Name,CONCAT(c.Title,' > ',p.Title),c.ParentId
                                                                   FROM dbo.PbsProduct c
                                                                   JOIN name_tree p on p.ParentId = c.ProductId)
                                                                   select Title
                                                                   from name_tree WHERE ParentId IS NULL";

                    pmolData1.ProductTaxonomy = dbconnection
                        .Query<string>(selectProduct, new {Id = pmol.ProductId}).FirstOrDefault();


                    if (pmol.LocationId != null && pmol.ExecutionStartTime != null)
                    {
                        var mapLocation = dbconnection
                            .Query<Position>(
                                "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId WHERE ml.Id = @Id",
                                new {Id = pmol.LocationId}).FirstOrDefault();

                        if (mapLocation != null)
                            if (mapLocation.Lat != "0" || mapLocation.Lon != "0")
                            {
                                // var forecast = await myCalenderParameter.iShiftRepository.GetWeatherForecast(
                                //     mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                                //     pmol.ExecutionDate.GetValueOrDefault(), pmol.ExecutionStartTime,
                                //     myCalenderParameter.TenantProvider, myCalenderParameter.Configuration);
                                //
                                //
                                // pmolData1.Forecast = forecast;
                            }
                    }

                    pmolDatalist.Add(pmolData1);
                }
            }

            var teamquery =
                @"SELECT CabPerson.Id ,CabPerson.FullName AS Name FROM dbo.CabPerson LEFT OUTER JOIN dbo.CabPersonCompany ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Id = @Id";

            List<PmolTeamMeber> mPmolTeamMeber;

            team = connection.Query<PmolTeamMeber>(teamquery,
                new {Id = pmolData.FirstOrDefault()?.PersonId, UserId = myCalenderParameter.UserId}).ToList();

            if (team.Count != 0)
            {
                var cu = @"with name_tree as
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
                                          where OrganizationTaxonomyLevelId = '2210e768-3e06-po02-b337-ee367a82adjj'";
                var cuId = connection
                    .Query<OrganizationTaxonomy>(cu, new {Id = myCalenderParameter.MyCalenderGetTeamDto.BuId})
                    .FirstOrDefault();

                if (pmolDatalist.Count != 0)
                {
                    pmolDatalist = pmolDatalist.OrderBy(c => c.ExecutionStartTime).ToList();
                    teamsWithPmolDto.Pmol = pmolDatalist;
                    teamsWithPmolDto.ProjectSequenceCode =
                        teamsWithPmolDto.Pmol.FirstOrDefault().ProjectSequenceCode;
                }

                teamsWithPmolDto.ContractingUinit = cuId?.Title;
                teamsWithPmolDto.Team = team;
                teamsWithPmol.Add(teamsWithPmolDto);
            }
        }

        teamsWithPmol = teamsWithPmol.OrderBy(c => c.TeamTitle).ToList();
        return teamsWithPmol;
    }

    public async Task<List<TeamsWithPmolDto>> MyCalenderListDataForCuTeam(MyCalenderParameter myCalenderParameter)
    {
        var teamsWithPmol = new List<TeamsWithPmolDto>();

        using (var connection = new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString))
        {
            string personSql = @"SELECT
                                  OrganizationTeamPmol.PmolId
                                 ,OrganizationTeamPmol.Project
                                 ,OrganizationTaxonomy.Title
                                 ,OrganizationTaxonomy.PersonId
                                 ,OrganizationTeamPmol.ContractingUnit
                                 ,OrganizationTeamPmol.OrganizationTeamId
                                FROM dbo.OrganizationTeamPmol
                                RIGHT OUTER JOIN dbo.OrganizationTaxonomy
                                  ON OrganizationTaxonomy.ParentId = OrganizationTeamPmol.OrganizationTeamId
                                WHERE OrganizationTeamPmol.ExecutionDate BETWEEN @Startdate AND @Enddate AND OrganizationTaxonomy.PersonId IN (SELECT PersonId FROM OrganizationTaxonomy WHERE ParentId = @Id)
                                ORDER BY OrganizationTaxonomy.Title";

            var pmolData = connection.Query<MyCalenderPmolData>(personSql,
                new
                {
                    myCalenderParameter.MyCalenderGetTeamDto.StartDate,
                    myCalenderParameter.MyCalenderGetTeamDto.EndDate,
                    Id = myCalenderParameter.MyCalenderGetTeamDto.Id
                }).ToList();

            var groupByPerson = pmolData.GroupBy(e => e.PersonId).ToList();

            foreach (var i in groupByPerson)
            {
                var teamsWithPmolDto = new TeamsWithPmolDto();
                var pmolDatalist = new List<PmolData>();
                var team = new List<PmolTeamMeber>();
                IEnumerable<PomlVehicle> vehicals;
                IEnumerable<PomlVehicle> tools;

                foreach (var r in i)
                {
                    var connectionString = ConnectionString.MapConnectionString(r.ContractingUnit,
                        r.Project, myCalenderParameter.TenantProvider);

                    await using var dbconnection =
                        new SqlConnection(connectionString);

                    var pbsList = dbconnection.Query<PbsProduct>("SELECT * FROM dbo.PbsProduct");
                    var rfqPmolList = connection.Query<PmolRfq>("Select * from PmolRfq");

                    var vehical =
                        @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0";

                    var tool =
                        @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber,PMolPlannedWorkTools.RequiredQuantity,PMolPlannedWorkTools.AllocatedQuantity FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0 AND CorporateProductCatalog.ResourceFamilyId != '0c355800-91fd-4d99-8010-921a42f0ba04'";

                    var pmolquery =
                        @"SELECT PMol.ProjectMoleculeId ,PMol.Name ,PMol.Id ,PMol.ExecutionEndTime ,PMol.ExecutionStartTime ,PMol.ExecutionDate ,PMol.Title ,PMol.ProjectSequenceCode,PMol.StatusId,PMol.TypeId , PMol.LocationId,PMol.ProductId FROM dbo.PMol WHERE Id = @Id";

                    var sb = new StringBuilder(pmolquery);
                    
                    var pmolData1 = new PmolData
                    {
                        ProjectSequenceCode = r.Project,
                        ProjectTitle = connection
                            .Query<string>(
                                "SELECT Title FROM dbo.ProjectDefinition WHERE SequenceCode = @Project",
                                new {r.Project}).FirstOrDefault()
                    };
                    {
                        vehicals = dbconnection.Query<PomlVehicle>(vehical, new {Id = r.PmolId}).ToList();
                        tools = dbconnection.Query<PomlVehicle>(tool, new {Id = r.PmolId}).ToList();
                    }
                    if (vehicals.Any()) pmolData1.PomlVehical = (List<PomlVehicle>) vehicals;

                    if (tools.Any()) pmolData1.PomlTool = (List<PomlVehicle>) tools;

                    var pmol = dbconnection.Query<Pmol>(sb.ToString(), new {Id = r.PmolId}).FirstOrDefault();

                    if (pmol != null)
                    {
                        pmolData1.Id = pmol.Id;
                        pmolData1.Name = pmol.Name;
                        pmolData1.ExecutionStartTime = pmol.ExecutionStartTime;
                        pmolData1.ExecutionEndTime = pmol.ExecutionEndTime;
                        pmolData1.ExecutionDate = pmol.ExecutionDate.ToString();
                        pmolData1.Title = pmol.Title;
                        pmolData1.ContractingUinit = r.ContractingUnit;
                        pmolData1.TeamId = r.OrganizationTeamId;
                        pmolData1.ProjectMoleculeId = pmol.ProjectMoleculeId;
                        pmolData1.StatusId = pmol.StatusId;
                        pmolData1.TypeId = pmol.TypeId;
                        pmolData1.ProjectSequenceId = r.Project;
                        pmolData1.ProductId =
                            pbsList.FirstOrDefault(e => e.Id == pmol.ProductId)?.ProductId;
                        pmolData1.IsRFQGenerated = connection
                            .Query<PmolRfq>("Select * from PmolRfq Where PmolId = @PmolId",
                                new {PmolId = pmol.Id}).Any();

                        var selectProduct = @"with name_tree as
                                                                 (SELECT
                                                                   PbsProduct.Id
                                                                  ,PbsProduct.Name
                                                                  ,PbsProduct.Title
                                                                  ,PbsProduct.ParentId
                                                                 FROM dbo.PbsProduct
                                                                 WHERE PbsProduct.Id = @Id
                                                                   UNION ALL
                                                                   SELECT c.Id, c.Name,CONCAT(c.Title,' > ',p.Title),c.ParentId
                                                                   FROM dbo.PbsProduct c
                                                                   JOIN name_tree p on p.ParentId = c.ProductId)
                                                                   select Title
                                                                   from name_tree WHERE ParentId IS NULL";

                        pmolData1.ProductTaxonomy = dbconnection
                            .Query<string>(selectProduct, new {Id = pmol.ProductId}).FirstOrDefault();


                        if (pmol.LocationId != null && pmol.ExecutionStartTime != null)
                        {
                            var mapLocation = dbconnection
                                .Query<Position>(
                                    "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId WHERE ml.Id = @Id",
                                    new {Id = pmol.LocationId}).FirstOrDefault();

                            if (mapLocation != null)
                                if (mapLocation.Lat != "0" || mapLocation.Lon != "0")
                                {
                                    // var forecast = await myCalenderParameter.iShiftRepository.GetWeatherForecast(
                                    //     mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                                    //     pmol.ExecutionDate.GetValueOrDefault(), pmol.ExecutionStartTime,
                                    //     myCalenderParameter.TenantProvider, myCalenderParameter.Configuration);
                                    //
                                    //
                                    // pmolData1.Forecast = forecast;
                                }
                        }

                        pmolDatalist.Add(pmolData1);
                    }
                }

                var teamquery =
                    @"SELECT CabPerson.Id ,CabPerson.FullName AS Name FROM dbo.CabPerson LEFT OUTER JOIN dbo.CabPersonCompany ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Id = @Id ";

                List<PmolTeamMeber> mPmolTeamMeber;

                team = connection.Query<PmolTeamMeber>(teamquery,
                    new {Id = i.Key, UserId = myCalenderParameter.UserId}).ToList();

                if (team.Count != 0)
                {
                    var cu = @"with name_tree as
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
                                          where OrganizationTaxonomyLevelId = '2210e768-3e06-po02-b337-ee367a82adjj'";
                    var cuId = connection
                        .Query<OrganizationTaxonomy>(cu, new {Id =  i.First().OrganizationTeamId})
                        .FirstOrDefault();

                    if (pmolDatalist.Count != 0)
                    {
                        pmolDatalist = pmolDatalist.OrderBy(c => c.ExecutionStartTime).ToList();
                        teamsWithPmolDto.Pmol = pmolDatalist;
                        teamsWithPmolDto.ProjectSequenceCode =
                            teamsWithPmolDto.Pmol.FirstOrDefault().ProjectSequenceCode;
                    }

                    teamsWithPmolDto.ContractingUinit = cuId?.Title;
                    teamsWithPmolDto.Team = team;
                    teamsWithPmol.Add(teamsWithPmolDto);
                }
            }
        }

        teamsWithPmol = teamsWithPmol.OrderBy(c => c.TeamTitle).ToList();
        return teamsWithPmol;
    }
    
    public async Task<List<TeamsWithPmolDto>> MyCalenderListDataForCuBu(MyCalenderParameter myCalenderParameter)
    {
        var teamsWithPmol = new List<TeamsWithPmolDto>();

        using (var connection = new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString))
        {
            string personSql = @"SELECT
                                   OrganizationTeamPmol.PmolId
                                 ,OrganizationTeamPmol.Project
                                 ,OrganizationTaxonomy.Title
                                 ,OrganizationTaxonomy.PersonId
                                 ,OrganizationTeamPmol.ContractingUnit
                                 ,OrganizationTeamPmol.OrganizationTeamId
                                FROM dbo.OrganizationTeamPmol
                                 RIGHT OUTER JOIN dbo.OrganizationTaxonomy
                                  ON OrganizationTaxonomy.ParentId = OrganizationTeamPmol.OrganizationTeamId
                                WHERE OrganizationTeamPmol.ExecutionDate BETWEEN @Startdate AND @Enddate
                                AND OrganizationTeamPmol.OrganizationTeamId IN (SELECT
                                    OrganizationTaxonomy.Id
                                  FROM dbo.OrganizationTaxonomy
                                  LEFT OUTER JOIN OrganizationTaxonomy ot
                                    ON OrganizationTaxonomy.Id = ot.ParentId
                                  RIGHT OUTER JOIN OrganizationTaxonomy ot1
                                    ON OrganizationTaxonomy.ParentId = ot1.Id
                                  WHERE ot1.Id = @Id)
                                  AND OrganizationTaxonomy.PersonId IN (SELECT
                                    ot.PersonId
                                  FROM dbo.OrganizationTaxonomy
                                  LEFT OUTER JOIN OrganizationTaxonomy ot
                                    ON OrganizationTaxonomy.Id = ot.ParentId
                                  RIGHT OUTER JOIN OrganizationTaxonomy ot1
                                    ON OrganizationTaxonomy.ParentId = ot1.Id
                                  WHERE ot1.Id = @Id  AND (OrganizationTaxonomy.TemporaryTeamNameId != '7bcb4e8d-8e8c-487d-team-6b91c89fAcce' OR OrganizationTaxonomy.TemporaryTeamNameId IS NULL))
                                ORDER BY OrganizationTaxonomy.Title";

            var pmolData = connection.Query<MyCalenderPmolData>(personSql,
                new
                {
                    myCalenderParameter.MyCalenderGetTeamDto.StartDate,
                    myCalenderParameter.MyCalenderGetTeamDto.EndDate,
                    Id = myCalenderParameter.MyCalenderGetTeamDto.Id
                }).ToList();

            var groupByPerson = pmolData.GroupBy(e => e.PersonId).ToList();

            foreach (var i in groupByPerson)
            {
                var teamsWithPmolDto = new TeamsWithPmolDto();
                var pmolDatalist = new List<PmolData>();
                var team = new List<PmolTeamMeber>();
                IEnumerable<PomlVehicle> vehicals;
                IEnumerable<PomlVehicle> tools;

                foreach (var r in i)
                {
                    var connectionString = ConnectionString.MapConnectionString(r.ContractingUnit,
                        r.Project, myCalenderParameter.TenantProvider);

                    await using var dbconnection =
                        new SqlConnection(connectionString);

                    var pbsList = dbconnection.Query<PbsProduct>("SELECT * FROM dbo.PbsProduct");
                    var rfqPmolList = connection.Query<PmolRfq>("Select * from PmolRfq");

                    var vehical =
                        @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0";

                    var tool =
                        @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber,PMolPlannedWorkTools.RequiredQuantity,PMolPlannedWorkTools.AllocatedQuantity FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0 AND CorporateProductCatalog.ResourceFamilyId != '0c355800-91fd-4d99-8010-921a42f0ba04'";

                    var pmolquery =
                        @"SELECT PMol.ProjectMoleculeId ,PMol.Name ,PMol.Id ,PMol.ExecutionEndTime ,PMol.ExecutionStartTime ,PMol.ExecutionDate ,PMol.Title ,PMol.ProjectSequenceCode,PMol.StatusId,PMol.TypeId , PMol.LocationId,PMol.ProductId FROM dbo.PMol WHERE Id = @Id";

                    var sb = new StringBuilder(pmolquery);

                    var pmolData1 = new PmolData
                    {
                        ProjectSequenceCode = r.Project,
                        ProjectTitle = connection
                            .Query<string>(
                                "SELECT Title FROM dbo.ProjectDefinition WHERE SequenceCode = @Project",
                                new {r.Project}).FirstOrDefault()
                    };
                    {
                        vehicals = dbconnection.Query<PomlVehicle>(vehical, new {Id = r.PmolId}).ToList();
                        tools = dbconnection.Query<PomlVehicle>(tool, new {Id = r.PmolId}).ToList();
                    }
                    if (vehicals.Any()) pmolData1.PomlVehical = (List<PomlVehicle>) vehicals;

                    if (tools.Any()) pmolData1.PomlTool = (List<PomlVehicle>) tools;

                    var pmol = dbconnection.Query<Pmol>(sb.ToString(), new {Id = r.PmolId}).FirstOrDefault();

                    if (pmol != null)
                    {
                        pmolData1.Id = pmol.Id;
                        pmolData1.Name = pmol.Name;
                        pmolData1.ExecutionStartTime = pmol.ExecutionStartTime;
                        pmolData1.ExecutionEndTime = pmol.ExecutionEndTime;
                        pmolData1.ExecutionDate = pmol.ExecutionDate.ToString();
                        pmolData1.Title = pmol.Title;
                        pmolData1.ContractingUinit = r.ContractingUnit;
                        pmolData1.TeamId = r.OrganizationTeamId;
                        pmolData1.ProjectMoleculeId = pmol.ProjectMoleculeId;
                        pmolData1.StatusId = pmol.StatusId;
                        pmolData1.TypeId = pmol.TypeId;
                        pmolData1.ProjectSequenceId = r.Project;
                        pmolData1.ProductId =
                            pbsList.FirstOrDefault(e => e.Id == pmol.ProductId)?.ProductId;
                        pmolData1.IsRFQGenerated = connection
                            .Query<PmolRfq>("Select * from PmolRfq Where PmolId = @PmolId",
                                new {PmolId = pmol.Id}).Any();

                        var selectProduct = @"with name_tree as
                                                                 (SELECT
                                                                   PbsProduct.Id
                                                                  ,PbsProduct.Name
                                                                  ,PbsProduct.Title
                                                                  ,PbsProduct.ParentId
                                                                 FROM dbo.PbsProduct
                                                                 WHERE PbsProduct.Id = @Id
                                                                   UNION ALL
                                                                   SELECT c.Id, c.Name,CONCAT(c.Title,' > ',p.Title),c.ParentId
                                                                   FROM dbo.PbsProduct c
                                                                   JOIN name_tree p on p.ParentId = c.ProductId)
                                                                   select Title
                                                                   from name_tree WHERE ParentId IS NULL";

                        pmolData1.ProductTaxonomy = dbconnection
                            .Query<string>(selectProduct, new {Id = pmol.ProductId}).FirstOrDefault();


                        if (pmol.LocationId != null && pmol.ExecutionStartTime != null)
                        {
                            var mapLocation = dbconnection
                                .Query<Position>(
                                    "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId WHERE ml.Id = @Id",
                                    new {Id = pmol.LocationId}).FirstOrDefault();

                            if (mapLocation != null)
                                if (mapLocation.Lat != "0" || mapLocation.Lon != "0")
                                {
                                    // var forecast = await myCalenderParameter.iShiftRepository.GetWeatherForecast(
                                    //     mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                                    //     pmol.ExecutionDate.GetValueOrDefault(), pmol.ExecutionStartTime,
                                    //     myCalenderParameter.TenantProvider, myCalenderParameter.Configuration);
                                    //
                                    //
                                    // pmolData1.Forecast = forecast;
                                }
                        }

                        pmolDatalist.Add(pmolData1);
                    }
                }

                var teamquery =
                    @"SELECT CabPerson.Id ,CabPerson.FullName AS Name FROM dbo.CabPerson LEFT OUTER JOIN dbo.CabPersonCompany ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Id = @Id ";

                List<PmolTeamMeber> mPmolTeamMeber;

                team = connection.Query<PmolTeamMeber>(teamquery,
                    new {Id = i.Key, UserId = myCalenderParameter.UserId}).ToList();

                if (team.Count != 0)
                {
                    var cu = @"with name_tree as
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
                                          where OrganizationTaxonomyLevelId = '2210e768-3e06-po02-b337-ee367a82adjj'";
                    var cuId = connection
                        .Query<OrganizationTaxonomy>(cu, new {Id = myCalenderParameter.MyCalenderGetTeamDto.BuId})
                        .FirstOrDefault();

                    if (pmolDatalist.Count != 0)
                    {
                        pmolDatalist = pmolDatalist.OrderBy(c => c.ExecutionStartTime).ToList();
                        teamsWithPmolDto.Pmol = pmolDatalist;
                        teamsWithPmolDto.ProjectSequenceCode =
                            teamsWithPmolDto.Pmol.FirstOrDefault().ProjectSequenceCode;
                    }

                    teamsWithPmolDto.ContractingUinit = cuId?.Title;
                    teamsWithPmolDto.Team = team;
                    teamsWithPmol.Add(teamsWithPmolDto);
                }
            }
        }

        teamsWithPmol = teamsWithPmol.OrderBy(c => c.TeamTitle).ToList();
        return teamsWithPmol;
    }
    
     public async Task<List<TeamsWithPmolDto>> MyCalenderListDataForCuCu(MyCalenderParameter myCalenderParameter)
    {
        var teamsWithPmol = new List<TeamsWithPmolDto>();

        using (var connection = new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString))
        {
            string personSql = @"SELECT
                                   OrganizationTeamPmol.PmolId
                                 ,OrganizationTeamPmol.Project
                                 ,OrganizationTaxonomy.Title
                                 ,OrganizationTaxonomy.PersonId
                                 ,OrganizationTeamPmol.ContractingUnit
                                 ,OrganizationTeamPmol.OrganizationTeamId
                                FROM dbo.OrganizationTeamPmol
                                 RIGHT OUTER JOIN dbo.OrganizationTaxonomy
                                  ON OrganizationTaxonomy.ParentId = OrganizationTeamPmol.OrganizationTeamId
                                WHERE OrganizationTeamPmol.ExecutionDate BETWEEN @Startdate AND @Enddate
                                AND OrganizationTeamPmol.OrganizationTeamId IN (SELECT
                                    OrganizationTaxonomy.Id
                                  FROM dbo.OrganizationTaxonomy
                                  LEFT OUTER JOIN OrganizationTaxonomy ot
                                    ON OrganizationTaxonomy.Id = ot.ParentId
                                  RIGHT OUTER JOIN OrganizationTaxonomy ot1
                                    ON OrganizationTaxonomy.ParentId = ot1.Id
                                     LEFT OUTER JOIN OrganizationTaxonomy ot2
                                     ON ot1.ParentId = ot2.Id
                                  WHERE ot2.Id = @Id)
                                ORDER BY OrganizationTaxonomy.Title";

            var pmolData = connection.Query<MyCalenderPmolData>(personSql,
                new
                {
                    myCalenderParameter.MyCalenderGetTeamDto.StartDate,
                    myCalenderParameter.MyCalenderGetTeamDto.EndDate,
                    myCalenderParameter.MyCalenderGetTeamDto.Id
                }).ToList();

            var groupByPerson = pmolData.GroupBy(e => e.PersonId).ToList();

            foreach (var i in groupByPerson)
            {
                var teamsWithPmolDto = new TeamsWithPmolDto();
                var pmolDatalist = new List<PmolData>();
                var team = new List<PmolTeamMeber>();
                IEnumerable<PomlVehicle> vehicals;
                IEnumerable<PomlVehicle> tools;

                foreach (var r in i)
                {
                    var connectionString = ConnectionString.MapConnectionString(r.ContractingUnit,
                        r.Project, myCalenderParameter.TenantProvider);

                    await using var dbconnection =
                        new SqlConnection(connectionString);

                    var pbsList = dbconnection.Query<PbsProduct>("SELECT * FROM dbo.PbsProduct");
                    var rfqPmolList = connection.Query<PmolRfq>("Select * from PmolRfq");

                    var vehical =
                        @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE CorporateProductCatalog.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04' AND PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0";

                    var tool =
                        @"SELECT PMolPlannedWorkTools.CoperateProductCatalogId ,CorporateProductCatalog.Title ,CorporateProductCatalog.ResourceNumber,PMolPlannedWorkTools.RequiredQuantity,PMolPlannedWorkTools.AllocatedQuantity FROM dbo.PMolPlannedWorkTools INNER JOIN dbo.PMol ON PMolPlannedWorkTools.PmolId = PMol.Id INNER JOIN dbo.CorporateProductCatalog ON PMolPlannedWorkTools.CoperateProductCatalogId = CorporateProductCatalog.Id WHERE PMol.Id = @Id AND PMolPlannedWorkTools.IsDeleted = 0 AND CorporateProductCatalog.ResourceFamilyId != '0c355800-91fd-4d99-8010-921a42f0ba04'";

                    var pmolquery =
                        @"SELECT PMol.ProjectMoleculeId ,PMol.Name ,PMol.Id ,PMol.ExecutionEndTime ,PMol.ExecutionStartTime ,PMol.ExecutionDate ,PMol.Title ,PMol.ProjectSequenceCode,PMol.StatusId,PMol.TypeId , PMol.LocationId,PMol.ProductId FROM dbo.PMol WHERE Id = @Id";

                    var sb = new StringBuilder(pmolquery);
                    
                    var pmolData1 = new PmolData
                    {
                        ProjectSequenceCode = r.Project,
                        ProjectTitle = connection
                            .Query<string>(
                                "SELECT Title FROM dbo.ProjectDefinition WHERE SequenceCode = @Project",
                                new {r.Project}).FirstOrDefault()
                    };
                    {
                        vehicals = dbconnection.Query<PomlVehicle>(vehical, new {Id = r.PmolId}).ToList();
                        tools = dbconnection.Query<PomlVehicle>(tool, new {Id = r.PmolId}).ToList();
                    }
                    if (vehicals.Any()) pmolData1.PomlVehical = (List<PomlVehicle>) vehicals;

                    if (tools.Any()) pmolData1.PomlTool = (List<PomlVehicle>) tools;

                    var pmol = dbconnection.Query<Pmol>(sb.ToString(), new {Id = r.PmolId}).FirstOrDefault();

                    if (pmol != null)
                    {
                        pmolData1.Id = pmol.Id;
                        pmolData1.Name = pmol.Name;
                        pmolData1.ExecutionStartTime = pmol.ExecutionStartTime;
                        pmolData1.ExecutionEndTime = pmol.ExecutionEndTime;
                        pmolData1.ExecutionDate = pmol.ExecutionDate.ToString();
                        pmolData1.Title = pmol.Title;
                        pmolData1.ContractingUinit = r.ContractingUnit;
                        pmolData1.TeamId = r.OrganizationTeamId;
                        pmolData1.ProjectMoleculeId = pmol.ProjectMoleculeId;
                        pmolData1.StatusId = pmol.StatusId;
                        pmolData1.TypeId = pmol.TypeId;
                        pmolData1.ProjectSequenceId = r.Project;
                        pmolData1.ProductId =
                            pbsList.FirstOrDefault(e => e.Id == pmol.ProductId)?.ProductId;
                        pmolData1.IsRFQGenerated = connection
                            .Query<PmolRfq>("Select * from PmolRfq Where PmolId = @PmolId",
                                new {PmolId = pmol.Id}).Any();

                        var selectProduct = @"with name_tree as
                                                                 (SELECT
                                                                   PbsProduct.Id
                                                                  ,PbsProduct.Name
                                                                  ,PbsProduct.Title
                                                                  ,PbsProduct.ParentId
                                                                 FROM dbo.PbsProduct
                                                                 WHERE PbsProduct.Id = @Id
                                                                   UNION ALL
                                                                   SELECT c.Id, c.Name,CONCAT(c.Title,' > ',p.Title),c.ParentId
                                                                   FROM dbo.PbsProduct c
                                                                   JOIN name_tree p on p.ParentId = c.ProductId)
                                                                   select Title
                                                                   from name_tree WHERE ParentId IS NULL";

                        pmolData1.ProductTaxonomy = dbconnection
                            .Query<string>(selectProduct, new {Id = pmol.ProductId}).FirstOrDefault();


                        if (pmol.LocationId != null && pmol.ExecutionStartTime != null)
                        {
                            var mapLocation = dbconnection
                                .Query<Position>(
                                    "SELECT * FROM Position LEFT OUTER JOIN MapLocation ml ON Position.Id = ml.PositionId WHERE ml.Id = @Id",
                                    new {Id = pmol.LocationId}).FirstOrDefault();

                            if (mapLocation != null)
                                if (mapLocation.Lat != "0" || mapLocation.Lon != "0")
                                {
                                    // var forecast = await myCalenderParameter.iShiftRepository.GetWeatherForecast(
                                    //     mapLocation.Lat.ToDouble(), mapLocation.Lon.ToDouble(),
                                    //     pmol.ExecutionDate.GetValueOrDefault(), pmol.ExecutionStartTime,
                                    //     myCalenderParameter.TenantProvider, myCalenderParameter.Configuration);
                                    //
                                    //
                                    // pmolData1.Forecast = forecast;
                                }
                        }

                        pmolDatalist.Add(pmolData1);
                    }
                }

                var teamquery =
                    @"SELECT CabPerson.Id ,CabPerson.FullName AS Name FROM dbo.CabPerson LEFT OUTER JOIN dbo.CabPersonCompany ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPersonCompany.Id = @Id AND (CabPersonCompany.Oid IS NULL OR CabPersonCompany.Oid != @UserId)";

                List<PmolTeamMeber> mPmolTeamMeber;

                team = connection.Query<PmolTeamMeber>(teamquery,
                    new {Id = i.Key, UserId = myCalenderParameter.UserId}).ToList();

                if (team.Count != 0)
                {
                    var cu = @"with name_tree as
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
                                          where OrganizationTaxonomyLevelId = '2210e768-3e06-po02-b337-ee367a82adjj'";
                    var cuId = connection
                        .Query<OrganizationTaxonomy>(cu, new {Id = myCalenderParameter.MyCalenderGetTeamDto.BuId})
                        .FirstOrDefault();

                    if (pmolDatalist.Count != 0)
                    {
                        pmolDatalist = pmolDatalist.OrderBy(c => c.ExecutionStartTime).ToList();
                        teamsWithPmolDto.Pmol = pmolDatalist;
                        teamsWithPmolDto.ProjectSequenceCode =
                            teamsWithPmolDto.Pmol.FirstOrDefault().ProjectSequenceCode;
                    }

                    teamsWithPmolDto.ContractingUinit = cuId?.Title;
                    teamsWithPmolDto.Team = team;
                    teamsWithPmol.Add(teamsWithPmolDto);
                }
            }
        }

        teamsWithPmol = teamsWithPmol.OrderBy(c => c.TeamTitle).ToList();
        return teamsWithPmol;
    }

    public async Task<IEnumerable<MyCalanderProjectDto>> MyCalenderProjectFlter(MyCalenderParameter myCalenderParameter)
    {
        if (myCalenderParameter.IsMyEnv)
        {
            await using var connection = new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString);
            
            var projects = @"SELECT
                          ProjectTeam.ProjectId
                         ,ProjectTeam.ContractingUnitId
                         ,ProjectTeamRole.CabPersonId
                         ,ProjectDefinition.SequenceCode
                         ,ProjectDefinition.Name
                         ,CabPerson.FullName
                         ,ProjectDefinition.ProjectConnectionString
                         ,ProjectDefinition.Title AS ProjectTitle
                         ,CabPersonCompany.Oid
                        FROM dbo.ProjectTeamRole
                        INNER JOIN dbo.ProjectTeam
                          ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
                        INNER JOIN dbo.ProjectDefinition
                          ON ProjectTeam.ProjectId = ProjectDefinition.Id
                        INNER JOIN dbo.CabPerson
                          ON ProjectTeamRole.CabPersonId = CabPerson.Id
                        INNER JOIN dbo.CabPersonCompany
                          ON CabPersonCompany.PersonId = CabPerson.Id
                        WHERE ProjectDefinition.IsDeleted = 0
                        AND CabPersonCompany.Oid = @Oid
                        AND ProjectDefinition.Title LIKE '%"+ myCalenderParameter.ProjectSearchMyCalender.Title +"%' ORDER BY ProjectDefinition.Title";

            var projectWithPmList = connection.Query<MyCalanderProjectDto>(projects, new {Oid = myCalenderParameter.UserId}).ToList();

            return projectWithPmList;
        }

        else
        {
            await using var connection = new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString);
            
            var projects = @"SELECT
                          ProjectDefinition.Id AS ProjectId
                         ,ProjectDefinition.ContractingUnitId
                         ,ProjectDefinition.SequenceCode
                         ,ProjectDefinition.Name
                         ,ProjectDefinition.ProjectConnectionString
                         ,ProjectDefinition.Title AS ProjectTitle
                        FROM dbo.ProjectDefinition
                        WHERE ProjectDefinition.IsDeleted = 0
                        AND ProjectDefinition.Title LIKE '%"+ myCalenderParameter.ProjectSearchMyCalender.Title +"%' ORDER BY ProjectDefinition.Title";

            var projectWithPmList = connection.Query<MyCalanderProjectDto>(projects, new {Oid = myCalenderParameter.UserId}).ToList();

            return projectWithPmList;
        }
        
    }

    public async Task<IEnumerable<PbsTreeStructure>> GetMyCalenderPbsTaxonomy(
        MyCalenderParameter myCalenderParameter)
    {
        await using var dbConnection = new SqlConnection(myCalenderParameter.TenantProvider.GetTenant().ConnectionString);

        var cu = dbConnection
            .Query<string>(
                "SELECT  cc.SequenceCode FROM ProjectDefinition LEFT OUTER JOIN CabCompany cc ON ProjectDefinition.ContractingUnitId = cc.Id WHERE ProjectDefinition.SequenceCode = @Id",
                new { Id = myCalenderParameter.Id }).FirstOrDefault();
        
        var connectionString = ConnectionString.MapConnectionString(cu,
            myCalenderParameter.Id, myCalenderParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);


        var productQuery = 
            @"SELECT t1.Id ,t1.Title ,t1.StartDate ,t1.EndDate ,t1.ProductId AS PbsSequenceId , t1.PbsProductStatusId ,t2.ProductId AS ParentId ,t4.TreeIndex FROM PbsProduct t1 LEFT OUTER JOIN PbsProduct t2 ON t1.ParentId = t2.ProductId LEFT OUTER JOIN PbsTreeIndex t4 ON t1.ProductId = t4.PbsProductId WHERE t1.NodeType = 'P' AND t1.IsDeleted = 0";


        
            var pbsData = connection.Query<PbsTreeStructure>(productQuery);
            
            return pbsData;
    }

    public  async Task<Pmol> MyCalenderCreatePmol(MyCalenderParameter MyCalenderParameter)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            MyCalenderParameter.ProjectCreateMycal.ProjectSequenceId, MyCalenderParameter.TenantProvider);
        
        await using var connection = new SqlConnection(connectionString);
        
        await using var dbConnection = new SqlConnection(MyCalenderParameter.TenantProvider.GetTenant().ConnectionString);

        var personId = dbConnection.Query<string>("SELECT PersonId FROM CabPersonCompany WHERE Oid = @oid",
            new { oid = MyCalenderParameter.UserId }).FirstOrDefault();

        if (MyCalenderParameter.ProjectCreateMycal.ForemanId != null)
        {
            personId = MyCalenderParameter.ProjectCreateMycal.ForemanId;
        }
        
        var pbs = connection.Query<PbsProduct>("Select * From PbsProduct Where Id = @ProductId",
            new { ProductId = MyCalenderParameter.ProjectCreateMycal.ProductId }).FirstOrDefault();
        if (pbs != null)
        {
            
            var _borParameter = new BorParameter();
            _borParameter.ContractingUnitSequenceId = "COM-0001";
            _borParameter.ProjectSequenceId = MyCalenderParameter.ProjectCreateMycal.ProjectSequenceId;
            _borParameter.Lang = MyCalenderParameter.Lang;
            _borParameter.ContextAccessor = MyCalenderParameter.ContextAccessor;
            _borParameter.TenantProvider = MyCalenderParameter.TenantProvider;
            _borParameter.IBorResourceRepository = MyCalenderParameter._iBorResourceRepository;
            _borParameter.ICoporateProductCatalogRepository = MyCalenderParameter._iCoporateProductCatalogRepository;
            var _CpcParameters = new CpcParameters();
            _CpcParameters.Oid = MyCalenderParameter.UserId;
            _borParameter.CpcParameters = _CpcParameters;

            var borDto = new BorDto
            {
                Id = Guid.NewGuid().ToString(),
                BorStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                BorTypeId = "88282458-0b40-poa3-b0f9-c2e40344c888",
                Name = pbs.Name,
                StartDate = pbs.StartDate,
                BorResources = new BorResource(),
                IsTh = true,
                Product = new BorProductDto
                {
                    Id = pbs.Id,
                    ProductId = pbs.ProductId
                }
            };
            _borParameter.BorDto = borDto;

            var borItemId = await MyCalenderParameter._iBorRepository.CreateBor(_borParameter);

            var _pmolParameter = new PmolParameter();
            _pmolParameter.ContractingUnitSequenceId = "COM-0001";
            _pmolParameter.ProjectSequenceId = MyCalenderParameter.ProjectCreateMycal.ProjectSequenceId ;
            _pmolParameter.Lang = MyCalenderParameter.Lang;
            _pmolParameter.ContextAccessor = MyCalenderParameter.ContextAccessor;
            _pmolParameter.TenantProvider = MyCalenderParameter.TenantProvider;
            _pmolParameter.IPmolResourceRepository = MyCalenderParameter.iPmolResourceRepository;
            _pmolParameter.Configuration = MyCalenderParameter.Configuration;
            _pmolParameter.UserId = MyCalenderParameter.UserId;
            _pmolParameter.isMyCal = true;

            string status = "94282458-0b40-40a3-b0f9-c2e40344c8f1";//In Development

            if (MyCalenderParameter.ProjectCreateMycal.IsFinished)
            {
                status = "7143ff01-d173-4a20-8c17-cacdfecdb84c";//In Review
            }

            var pmolDto = new PmolCreateDto()
            {
                Id = Guid.NewGuid().ToString(),
                Bor = new BorGetByIdDto()
                {
                    Id = borDto.Id,

                },
                Comment = MyCalenderParameter.ProjectCreateMycal.Comment,
                ExecutionDate = MyCalenderParameter.ProjectCreateMycal.ExecutionDate,
                ExecutionStartTime = MyCalenderParameter.ProjectCreateMycal.ExecutionStartTime,
                ExecutionEndTime = MyCalenderParameter.ProjectCreateMycal.ExecutionEndTime,
                ForemanId = personId,
                IsFinished = MyCalenderParameter.ProjectCreateMycal.IsFinished,
                Name = MyCalenderParameter.ProjectCreateMycal.Name,
                PmolType = "regular",
                ProductId = pbs.ProductId,
                TypeId = MyCalenderParameter.ProjectCreateMycal.PmolTypeId,
                StatusId = status,
                PmolLotId = MyCalenderParameter.ProjectCreateMycal.Cbc?.FirstOrDefault()?.LotId
            };
            
            _pmolParameter.PmolDto = pmolDto;

            var pmol = await MyCalenderParameter.iPmolRepository.CreateHeader(_pmolParameter, false);

            var pmolResourceParam = new PmolResourceParameter();
            pmolResourceParam.ContractingUnitSequenceId = "COM-0001";
            pmolResourceParam.ProjectSequenceId = MyCalenderParameter.ProjectCreateMycal.ProjectSequenceId ;
            pmolResourceParam.Lang = MyCalenderParameter.Lang;
            pmolResourceParam.ContextAccessor = MyCalenderParameter.ContextAccessor;
            pmolResourceParam.TenantProvider = MyCalenderParameter.TenantProvider;
            pmolResourceParam.ICoporateProductCatalogRepository = MyCalenderParameter._iCoporateProductCatalogRepository;

            if (MyCalenderParameter.ProjectCreateMycal.IsFinished)
            {
                MyCalenderParameter.ProjectCreateMycal.ConsumedQuantity =
                    MyCalenderParameter.ProjectCreateMycal.PlannedQuantity;
            }

            pmolResourceParam.ResourceCreateDto = new PmolResourceCreateDto()
            {
                BorId = borDto.Id,
                ConsumedQuantity = MyCalenderParameter.ProjectCreateMycal.ConsumedQuantity.ToDouble(),
                Required = MyCalenderParameter.ProjectCreateMycal.PlannedQuantity.ToDouble(),
                CorporateProductCatalogId = MyCalenderParameter.ProjectCreateMycal.LabourCpcId,
                CpcBasicUnitOfMeasureId = MyCalenderParameter.ProjectCreateMycal.MouId,
                Environment = "local",
                PmolId = pmol.Id,
                Type = "Planned",
                OrganizationTeamId = MyCalenderParameter.ProjectCreateMycal.TeamId,
                TeamRoleList = new List<PmolTeamRoleCreateDto>()
                {

                }

            };

            var team = new PmolTeamRoleCreateDto()
            {
                CabPersonId = personId,
                ConsumedQuantity = MyCalenderParameter.ProjectCreateMycal.ConsumedQuantity.ToDouble(),
                RoleId = "Foreman",
                RequiredQuantity = MyCalenderParameter.ProjectCreateMycal.PlannedQuantity.ToDouble(),

            };
            pmolResourceParam.ResourceCreateDto.TeamRoleList.Add(team);
            pmolResourceParam.VpRepository = MyCalenderParameter.VpRepository;
            pmolResourceParam.Configuration = MyCalenderParameter.Configuration;

            await MyCalenderParameter.iPmolResourceRepository.CreateLabour(pmolResourceParam);

            if (MyCalenderParameter.ProjectCreateMycal.Cbc != null)
            {
                foreach (var item in MyCalenderParameter.ProjectCreateMycal.Cbc)
                {
                    item.PmolId = pmol.Id;
                    _pmolParameter.PmolCbcResources = item;
            
                    await MyCalenderParameter.iPmolRepository.AddPmolCbcResource(_pmolParameter);
                }
               
            }


            return pmol;

        }
        else
        {
            throw new Exception("Pbs not exist");
        }

    }

}