using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BurndownChart;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class BurndownChartRepository:IBurndownChartRepository
{
    public async Task<List<GetBurndownChart>> GetBurnDownChartData(BurndownChartParameter BurndownChartParameter)
    {
        
        await using var connection = new SqlConnection(BurndownChartParameter.TenantProvider.GetTenant().ConnectionString);

        var data = new List<GetBurndownChart>();
        var AllLabour = new List<LabourData>();

        var db = new List<ProjectDefinition>();
        // var selectProject =
        //     @"SELECT ProjectDefinition.Id, ProjectDefinition.Title,ProjectDefinition.SequenceCode,ProjectDefinition.ProjectConnectionString ,ProjectDefinition.ProjectManagerId ,ProjectDefinition.ProjectStatus ,CabCompany.SequenceCode AS ContractingUnitId FROM dbo.ProjectDefinition LEFT OUTER JOIN CabCompany ON ProjectDefinition.ContractingUnitId = CabCompany.Id LEFT OUTER JOIN dbo.ProjectClassification ON ProjectDefinition.Id = ProjectClassification.ProjectId  WHERE ProjectDefinition.IsDeleted = 0  AND ProjectDefinition.ProjectScopeStatusId != '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da'";

        db = connection
            .Query<ProjectDefinition>("GetBurnDownChartDataSelectProject",commandType: CommandType.StoredProcedure).ToList();

        var resourceFamilies = connection.Query<CpcResourceFamilyLocalizedData>(
            "SELECT * FROM CpcResourceFamilyLocalizedData WHERE LanguageCode = @lang",
            new { lang = BurndownChartParameter.Lang });

        // var labourQuery = @"SELECT
        //                          PMolPlannedWorkLabour.RequiredQuantity
        //                         ,PMolPlannedWorkLabour.CoperateProductCatalogId
        //                         ,PMolPlannedWorkLabour.ConsumedQuantity
        //                         ,PMol.ExecutionDate
        //                         ,CorporateProductCatalog.ResourceFamilyId
        //                         FROM dbo.PMolPlannedWorkLabour
        //                         LEFT OUTER JOIN dbo.PMol
        //                         ON PMolPlannedWorkLabour.PmolId = PMol.Id
        //                     LEFT OUTER JOIN dbo.CorporateProductCatalog
        //                         ON PMolPlannedWorkLabour.CoperateProductCatalogId = CorporateProductCatalog.Id
        //                     WHERE PMolPlannedWorkLabour.IsDeleted = 0 AND PMol.IsDeleted = 0 AND CorporateProductCatalog.IsDeleted = 0  AND CorporateProductCatalog.ResourceFamilyId IS NOT NULL AND Pmol.ExecutionDate BETWEEN @StartDate AND @EndDate ";

        Parallel.ForEach(db, project =>
        {
            using var projectConnection = new SqlConnection(project.ProjectConnectionString);

            var labourData = projectConnection.Query<LabourData>("GetBurnDownChartDataLabourQuery",param: BurndownChartParameter.BurndownChartDto,commandType: CommandType.StoredProcedure)
                .ToList();

            AllLabour.AddRange(labourData);
        });
            
        
        data.Add(NodesCreate(BurndownChartParameter, AllLabour,null));

        Parallel.ForEach(AllLabour.GroupBy(x => x.ResourceFamilyId), item =>
        {
            // foreach (var item in AllLabour.GroupBy(x => x.ResourceFamilyId))
            // {
            var fam = resourceFamilies.FirstOrDefault(x => x.CpcResourceFamilyId == item.Key)?.Label;
            data.Add(NodesCreate(BurndownChartParameter, item.ToList(), fam));

        });


        return data.OrderBy(x => x.Title).ToList();
    }

    private  GetBurndownChart NodesCreate(BurndownChartParameter burndownChartParameter, IReadOnlyCollection<LabourData> allLabour,string family)
    {

        string title = null;
        var isPrimary = false;
        
        if (family == null)
        {
            title = "All";
            isPrimary = true;
        }
        else
        {
            title = family;
        }
        
        var primary = new GetBurndownChart
        {
            Title = title,
            IsPrimary = isPrimary,
            Data = new List<BurndownData>()
        };

        var day1 = new BurndownData()
        {
            Name = "Day 1",
            Consumed = allLabour.Sum(x => x.ConsumedQuantity),
            Planned = allLabour.Sum(x => x.RequiredQuantity)
        };
        primary.Data.Add(day1);

        var day2 = new BurndownData()
        {
            Name = "Day 2",
            Consumed = day1.Consumed - allLabour
                .Where(v => v.ExecutionDate == burndownChartParameter.BurndownChartDto.StartDate?.AddDays(0))
                .Sum(x => x.ConsumedQuantity),
            Planned = day1.Planned - allLabour
                .Where(v => v.ExecutionDate == burndownChartParameter.BurndownChartDto.StartDate?.AddDays(0))
                .Sum(x => x.RequiredQuantity)
        };
        primary.Data.Add(day2);

        var day3 = new BurndownData()
        {
            Name = "Day 3",
            Consumed = day2.Consumed - allLabour
                .Where(v => v.ExecutionDate == burndownChartParameter.BurndownChartDto.StartDate?.AddDays(1))
                .Sum(x => x.ConsumedQuantity),
            Planned = day2.Planned - allLabour
                .Where(v => v.ExecutionDate == burndownChartParameter.BurndownChartDto.StartDate?.AddDays(1))
                .Sum(x => x.RequiredQuantity)
        };
        primary.Data.Add(day3);

        var day4 = new BurndownData()
        {
            Name = "Day 4",
            Consumed = day3.Consumed - allLabour
                .Where(v => v.ExecutionDate == burndownChartParameter.BurndownChartDto.StartDate?.AddDays(2))
                .Sum(x => x.ConsumedQuantity),
            Planned = day3.Planned - allLabour
                .Where(v => v.ExecutionDate == burndownChartParameter.BurndownChartDto.StartDate?.AddDays(2))
                .Sum(x => x.RequiredQuantity)
        };
        primary.Data.Add(day4);

        var day5 = new BurndownData()
        {
            Name = "Day 5",
            Consumed = day4.Consumed - allLabour
                .Where(v => v.ExecutionDate == burndownChartParameter.BurndownChartDto.StartDate?.AddDays(3))
                .Sum(x => x.ConsumedQuantity),
            Planned = day4.Planned - allLabour
                .Where(v => v.ExecutionDate == burndownChartParameter.BurndownChartDto.StartDate?.AddDays(3))
                .Sum(x => x.RequiredQuantity)
        };
        primary.Data.Add(day5);

        return primary;
    }
}