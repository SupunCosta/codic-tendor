using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Dapper;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Quartz.Impl;
using ServiceStack;
using UPrinceV4.Shared;
using UPrinceV4.Web.Controllers.CAB;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.DossierData;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.PO;
using UPrinceV4.Web.Data.ProjectLocationDetails;
using UPrinceV4.Web.Data.PS;
using UPrinceV4.Web.Data.ThAutomation;
using UPrinceV4.Web.Data.Translations;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Data.WF;
using UPrinceV4.Web.Jobs;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BM;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Repositories.Interfaces.PO;
using UPrinceV4.Web.Repositories.Interfaces.PS;
using UPrinceV4.Web.Repositories.Interfaces.ThAutomation;
using UPrinceV4.Web.Util;
using Z.Dapper.Plus;

namespace UPrinceV4.Web.Controllers.Tenants;

[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
[ApiController]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public class ContractingUnitTenantsController : CommonConfigurationController
{
    private readonly BorParameter _borParameter;
    private readonly IBorRepository _borRepository;
    private readonly CompetenciesRepositoryParameter _competenciesRepositoryParameter;
    private readonly ContractorParameter _contractorParameter;
    private readonly CpcParameters _CpcParameters;
    private readonly ApplicationDbContext _dbContext;


    private readonly IBorRepository _iBorRepository;
    private readonly IBorResourceRepository _iBorResourceRepository;

    private readonly ICompetenciesRepository _iCompetenciesRepository;
    private readonly IContractorReopsitory _iContractorReopsitory;
    private readonly ICoporateProductCatalogRepository _iCoporateProductCatalogRepository;

    private readonly IPbsInstructionsRepository _iPbsInstructionsRepository;

    private readonly IPbsQualityRepository _IPbsQualityRepository;
    private readonly IPbsRepository _IPbsRepository;

    private readonly IPbsRiskRepository _IPbsRiskRepository;
    private readonly IPersonRepository _ipersonRepository;

    private readonly IPmolJournalRepository _IPmolJournalRepository;

    private readonly IPmolRepository _iPmolRepository;
    private readonly IPmolResourceRepository _iPmolResourceRepository;
    private readonly IProjectDefinitionRepository _iProjectDefinitionRepository;
    private readonly IProjectFinanceRepository _iProjectFinanceRepository;
    private readonly IProjectTeamRepository _iProjectTeamRepository;
    private readonly IProjectTimeRepository _iProjectTimeRepository;

    private readonly IVPRepository _iVPRepository;

    private readonly PbsQualityParameters _PbsQualityParameters;
    private readonly PersonRepositoryParameter _personRepositoryParameter;
    private readonly PmolJournalParameter _PmolJournalParameter;
    private readonly PmolParameter _pmolParameter;
    private readonly IPsRepository _psRepository;

    private readonly ITenantProvider _TenantProvider;
    private readonly IThAutomationRepository _iThAutomationRepository;
    private readonly IStockRepository _iStockRepository;
    private ICompanyRepository _iCompanyRepository;
    private readonly IPersonCompanyRepository _iPersonCompanyRepository;
    public ICabHistoryLogRepository _iCabHistoryLogRepository;
    private readonly IPbsResourceRepository _IPbsResourceRepository;
    private readonly ILogger<CentralAddressBookController> _logger;
    private readonly IConfiguration _configuration;


    public ContractingUnitTenantsController(ApplicationDbContext uPrinceCustomerContext,
        IHttpContextAccessor contextAccessor, ApiResponse apiResponse, ApiBadRequestResponse apiBadRequestResponse,
        ApiOkResponse apiOkResponse, ITenantProvider iTenantProvider,
        ICoporateProductCatalogRepository iCoporateProductCatalogRepository,
        IPmolRepository iPmolRepository, BorParameter borParameter,
        IBorRepository borRepository, ICompetenciesRepository iCompetenciesRepository, IVPRepository iVPRepository,
        CompetenciesRepositoryParameter competenciesRepositoryParameter,
        IPbsInstructionsRepository iPbsInstructionsRepository, CpcParameters cpcParameters,
        IBorRepository iBorRepository, IPbsRepository iPbsRepository,
        PersonRepositoryParameter personRepositoryParameter, IPmolResourceRepository iPmolResourceRepository,
        IPersonRepository ipersonRepository,
        IPbsQualityRepository iPbsQualityRepository, PbsQualityParameters pbsQualityParameters,
        IPbsRiskRepository iPbsRiskRepository, IPmolJournalRepository IPmolJournalRepository,
        IProjectDefinitionRepository IProjectDefinitionRepository, IProjectFinanceRepository IProjectFinanceRepository,
        IProjectTeamRepository IProjectTeamRepository, IProjectTimeRepository IProjectTimeRepository,
        IBorResourceRepository IBorResourceRepository,
        PmolJournalParameter PmolJournalParameter, IContractorReopsitory iContractorReopsitory,
        IPsRepository iPsRepository, IThAutomationRepository iThAutomationRepository, IStockRepository iStockRepository,
        ICompanyRepository iCompanyRepository, IPersonCompanyRepository iPersonCompanyRepository,
        ICabHistoryLogRepository iCabHistoryLogRepository,
        IPbsResourceRepository iPbsResourceRepository, ILogger<CentralAddressBookController> logger,
        IConfiguration iConfiguration) : base(uPrinceCustomerContext, contextAccessor, apiResponse,
        apiBadRequestResponse, apiOkResponse, iTenantProvider)
    {
        _dbContext = uPrinceCustomerContext;
        _iTenantProvider = iTenantProvider;
        _iPmolRepository = iPmolRepository;
        _borParameter = borParameter;

        _borRepository = borRepository;
        _iPmolResourceRepository = iPmolResourceRepository;

        _iPbsInstructionsRepository = iPbsInstructionsRepository;


        _iBorRepository = iBorRepository;
        _borParameter = new BorParameter();

        _IPbsRepository = iPbsRepository;

        _personRepositoryParameter = personRepositoryParameter;
        _ipersonRepository = ipersonRepository;

        _IPbsQualityRepository = iPbsQualityRepository;
        _PbsQualityParameters = pbsQualityParameters;

        _IPbsRiskRepository = iPbsRiskRepository;

        _iCompetenciesRepository = iCompetenciesRepository;
        _competenciesRepositoryParameter = competenciesRepositoryParameter;

        _IPmolJournalRepository = IPmolJournalRepository;

        _PmolJournalParameter = PmolJournalParameter;

        _pmolParameter = new PmolParameter();
        _iVPRepository = iVPRepository;
        _iCoporateProductCatalogRepository = iCoporateProductCatalogRepository;
        _CpcParameters = cpcParameters;
        _contractorParameter = new ContractorParameter();
        _iContractorReopsitory = iContractorReopsitory;
        _psRepository = iPsRepository;
        _iProjectDefinitionRepository = IProjectDefinitionRepository;
        _iProjectFinanceRepository = IProjectFinanceRepository;
        _iProjectTeamRepository = IProjectTeamRepository;
        _iProjectTimeRepository = IProjectTimeRepository;
        _iBorResourceRepository = IBorResourceRepository;
        _iThAutomationRepository = iThAutomationRepository;
        _iStockRepository = iStockRepository;
        _iCompanyRepository = iCompanyRepository;
        _iPersonCompanyRepository = iPersonCompanyRepository;
        _iCabHistoryLogRepository = iCabHistoryLogRepository;
        _IPbsResourceRepository = iPbsResourceRepository;
        _logger = logger;
        _configuration = iConfiguration;
    }


    [AllowAnonymous]
    [HttpGet("StartJob")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> StartJob(CancellationToken ct)
    {
        StdSchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = await factory.GetScheduler();

        // and start it off
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<DatabaseCopyJob>()
            .WithIdentity("DatabaseCopyJob", "DatabaseCopyJob")
            .UsingJobData("SequenceCode", "Test")
            .UsingJobData("ConnectionString", _iTenantProvider.GetTenant().ConnectionString)
            .Build();
        // JobKey jobKey = new JobKey("testJob");
        // JobDetail job = newJob(DatabaseCopyJob.class)
        // .withIdentity(jobKey)
        //     .storeDurably()
        //     .build();
        var replace = true;
        var durable = true;
        await scheduler.AddJob(job, replace, durable, ct);

        await scheduler.TriggerJob(new JobKey("DatabaseCopyJob", "DatabaseCopyJob"), ct);

        return Ok(new ApiOkResponse("ss"));
    }

    public ITenantProvider _iTenantProvider { get; }


    [AllowAnonymous]
    [HttpGet("Migration")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<DatabasesEx>> Migration([FromHeader(Name = "env")] string env,
        [FromHeader(Name = "sql")] string sql)
    {
        // var json = "[{\"DatabaseName\":\"COM0001\"},{\"DatabaseName\":\"P0001\"},{\"DatabaseName\":\"P0002\"},{\"DatabaseName\":\"P0003\"},{\"DatabaseName\":\"P0004\"},{\"DatabaseName\":\"P0005\"},{\"DatabaseName\":\"P0007\"},{\"DatabaseName\":\"P0008\"},{\"DatabaseName\":\"P0009\"},{\"DatabaseName\":\"P0010\"},{\"DatabaseName\":\"P0011\"},{\"DatabaseName\":\"P0013\"},{\"DatabaseName\":\"P0014\"},{\"DatabaseName\":\"P0015\"},{\"DatabaseName\":\"P0016\"},{\"DatabaseName\":\"P0017\"},{\"DatabaseName\":\"P0018\"},{\"DatabaseName\":\"P0019\"},{\"DatabaseName\":\"P0020\"},{\"DatabaseName\":\"P0021\"},{\"DatabaseName\":\"P0022\"},{\"DatabaseName\":\"P0023\"},{\"DatabaseName\":\"P0024\"},{\"DatabaseName\":\"P0025\"},{\"DatabaseName\":\"P0026\"},{\"DatabaseName\":\"P0027\"},{\"DatabaseName\":\"P0028\"},{\"DatabaseName\":\"P0029\"},{\"DatabaseName\":\"P0030\"},{\"DatabaseName\":\"P0031\"},{\"DatabaseName\":\"P0032\"},{\"DatabaseName\":\"P0033\"},{\"DatabaseName\":\"P0034\"},{\"DatabaseName\":\"P0035\"},{\"DatabaseName\":\"P0036\"},{\"DatabaseName\":\"P0037\"},{\"DatabaseName\":\"P0038\"},{\"DatabaseName\":\"P0039\"},{\"DatabaseName\":\"P0040\"},{\"DatabaseName\":\"P0041\"},{\"DatabaseName\":\"P0042\"},{\"DatabaseName\":\"P0043\"},{\"DatabaseName\":\"P0044\"},{\"DatabaseName\":\"P0045\"},{\"DatabaseName\":\"P0046\"},{\"DatabaseName\":\"P0047\"},{\"DatabaseName\":\"P0048\"},{\"DatabaseName\":\"P0049\"},{\"DatabaseName\":\"P0050\"},{\"DatabaseName\":\"P0051\"},{\"DatabaseName\":\"P0052\"},{\"DatabaseName\":\"P0053\"},{\"DatabaseName\":\"P0054\"},{\"DatabaseName\":\"P0055\"},{\"DatabaseName\":\"P0056\"},{\"DatabaseName\":\"P0057\"},{\"DatabaseName\":\"P0058\"},{\"DatabaseName\":\"P0059\"},{\"DatabaseName\":\"P0060\"},{\"DatabaseName\":\"P0061\"},{\"DatabaseName\":\"P0062\"},{\"DatabaseName\":\"P0063\"},{\"DatabaseName\":\"P0064\"},{\"DatabaseName\":\"P0065\"},{\"DatabaseName\":\"P0066\"},{\"DatabaseName\":\"P0067\"},{\"DatabaseName\":\"P0068\"},{\"DatabaseName\":\"P0069\"},{\"DatabaseName\":\"P0070\"},{\"DatabaseName\":\"P0071\"},{\"DatabaseName\":\"P0072\"},{\"DatabaseName\":\"P0073\"},{\"DatabaseName\":\"P0074\"},{\"DatabaseName\":\"P0075\"},{\"DatabaseName\":\"P0076\"},{\"DatabaseName\":\"P0077\"},{\"DatabaseName\":\"P0078\"},{\"DatabaseName\":\"P0079\"},{\"DatabaseName\":\"P0080\"},{\"DatabaseName\":\"P0081\"},{\"DatabaseName\":\"P0082\"},{\"DatabaseName\":\"P0083\"},{\"DatabaseName\":\"P0084\"},{\"DatabaseName\":\"P0085\"},{\"DatabaseName\":\"P0086\"},{\"DatabaseName\":\"P0087\"},{\"DatabaseName\":\"P0088\"},{\"DatabaseName\":\"P0089\"},{\"DatabaseName\":\"P0090\"},{\"DatabaseName\":\"P0091\"},{\"DatabaseName\":\"P0092\"},{\"DatabaseName\":\"P0093\"},{\"DatabaseName\":\"P0094\"},{\"DatabaseName\":\"P0095\"},{\"DatabaseName\":\"P0096\"},{\"DatabaseName\":\"P0097\"},{\"DatabaseName\":\"P0098\"},{\"DatabaseName\":\"P0099\"},{\"DatabaseName\":\"P0100\"},{\"DatabaseName\":\"P0101\"},{\"DatabaseName\":\"P0102\"},{\"DatabaseName\":\"P0103\"},{\"DatabaseName\":\"P0104\"},{\"DatabaseName\":\"P0105\"},{\"DatabaseName\":\"P0106\"},{\"DatabaseName\":\"P0107\"},{\"DatabaseName\":\"UPrinceV4Einstein\"},{\"DatabaseName\":\"UPrinceV4ProjectTemplate\"}]";
        //var json = "[{\"DatabaseName\":\"COM0001\"},{\"DatabaseName\":\"COM0053\"},{\"DatabaseName\":\"P0001\"},{\"DatabaseName\":\"P0002\"},{\"DatabaseName\":\"P0003\"},{\"DatabaseName\":\"P0004\"},{\"DatabaseName\":\"P0005\"},{\"DatabaseName\":\"P0007\"},{\"DatabaseName\":\"P0008\"},{\"DatabaseName\":\"P0010\"},{\"DatabaseName\":\"P0011\"},{\"DatabaseName\":\"P0013\"},{\"DatabaseName\":\"P0014\"},{\"DatabaseName\":\"P0015\"},{\"DatabaseName\":\"P0016\"},{\"DatabaseName\":\"P0017\"},{\"DatabaseName\":\"P0018\"},{\"DatabaseName\":\"P0019\"},{\"DatabaseName\":\"P0021\"},{\"DatabaseName\":\"P0022\"},{\"DatabaseName\":\"P0023\"},{\"DatabaseName\":\"P0024\"},{\"DatabaseName\":\"P0025\"},{\"DatabaseName\":\"P0026\"},{\"DatabaseName\":\"P0027\"},{\"DatabaseName\":\"P0028\"},{\"DatabaseName\":\"P0029\"},{\"DatabaseName\":\"P0030\"},{\"DatabaseName\":\"P0031\"},{\"DatabaseName\":\"P0032\"},{\"DatabaseName\":\"P0033\"},{\"DatabaseName\":\"P0034\"},{\"DatabaseName\":\"P0035\"},{\"DatabaseName\":\"P0036\"},{\"DatabaseName\":\"P0037\"},{\"DatabaseName\":\"P0038\"},{\"DatabaseName\":\"P0039\"},{\"DatabaseName\":\"P0040\"},{\"DatabaseName\":\"P0041\"},{\"DatabaseName\":\"P0042\"},{\"DatabaseName\":\"P0043\"},{\"DatabaseName\":\"P0044\"},{\"DatabaseName\":\"P0045\"},{\"DatabaseName\":\"P0046\"},{\"DatabaseName\":\"P0047\"},{\"DatabaseName\":\"P0048\"},{\"DatabaseName\":\"P0049\"},{\"DatabaseName\":\"P0050\"},{\"DatabaseName\":\"P0051\"},{\"DatabaseName\":\"P0052\"},{\"DatabaseName\":\"P0053\"},{\"DatabaseName\":\"P0054\"},{\"DatabaseName\":\"UPrinceV4ProjectTemplate\"},{\"DatabaseName\":\"UPrinceV4UAT\"}]";
        var result = new List<Databases>();
        //uprincev4uatdb
        //uprincev4einstein
        // var env = "uprincev4einstein";
        var exceptionLst = new List<DatabasesEx>();
        using (var connection = new SqlConnection("Server=tcp:" + env +
                                                  ".database.windows.net,1433;Initial Catalog=master;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        {
            result = connection
                .Query<Databases>(
                    @"select [name] as DatabaseName from sys.databases WHERE name NOT IN('master', 'MsalTokenCacheDatabase', 'UPrinceV4EinsteinCatelog', 'UPrinceV4UATCatelog') order by name")
                .ToList();
        }


        foreach (var project in result)
        {
            var connectionString = "Server=tcp:" + env + ".database.windows.net,1433;Initial Catalog=" +
                                   project.DatabaseName +
                                   ";Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            {
                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        await connection.ExecuteAsync(sql);
                    }
                    catch (Exception ex)
                    {
                        var mDatabasesEx = new DatabasesEx();
                        mDatabasesEx.DatabaseName = project.DatabaseName;
                        mDatabasesEx.Exception = ex;
                        exceptionLst.Add(mDatabasesEx);
                    }
                }
            }
        }

        return exceptionLst;
    }

    [AllowAnonymous]
    [HttpGet("SpMigration")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<DatabasesEx>> SpMigration([FromHeader(Name = "env")] string env,
        [FromHeader(Name = "sql")] string sql)
    {
        // var json = "[{\"DatabaseName\":\"COM0001\"},{\"DatabaseName\":\"P0001\"},{\"DatabaseName\":\"P0002\"},{\"DatabaseName\":\"P0003\"},{\"DatabaseName\":\"P0004\"},{\"DatabaseName\":\"P0005\"},{\"DatabaseName\":\"P0007\"},{\"DatabaseName\":\"P0008\"},{\"DatabaseName\":\"P0009\"},{\"DatabaseName\":\"P0010\"},{\"DatabaseName\":\"P0011\"},{\"DatabaseName\":\"P0013\"},{\"DatabaseName\":\"P0014\"},{\"DatabaseName\":\"P0015\"},{\"DatabaseName\":\"P0016\"},{\"DatabaseName\":\"P0017\"},{\"DatabaseName\":\"P0018\"},{\"DatabaseName\":\"P0019\"},{\"DatabaseName\":\"P0020\"},{\"DatabaseName\":\"P0021\"},{\"DatabaseName\":\"P0022\"},{\"DatabaseName\":\"P0023\"},{\"DatabaseName\":\"P0024\"},{\"DatabaseName\":\"P0025\"},{\"DatabaseName\":\"P0026\"},{\"DatabaseName\":\"P0027\"},{\"DatabaseName\":\"P0028\"},{\"DatabaseName\":\"P0029\"},{\"DatabaseName\":\"P0030\"},{\"DatabaseName\":\"P0031\"},{\"DatabaseName\":\"P0032\"},{\"DatabaseName\":\"P0033\"},{\"DatabaseName\":\"P0034\"},{\"DatabaseName\":\"P0035\"},{\"DatabaseName\":\"P0036\"},{\"DatabaseName\":\"P0037\"},{\"DatabaseName\":\"P0038\"},{\"DatabaseName\":\"P0039\"},{\"DatabaseName\":\"P0040\"},{\"DatabaseName\":\"P0041\"},{\"DatabaseName\":\"P0042\"},{\"DatabaseName\":\"P0043\"},{\"DatabaseName\":\"P0044\"},{\"DatabaseName\":\"P0045\"},{\"DatabaseName\":\"P0046\"},{\"DatabaseName\":\"P0047\"},{\"DatabaseName\":\"P0048\"},{\"DatabaseName\":\"P0049\"},{\"DatabaseName\":\"P0050\"},{\"DatabaseName\":\"P0051\"},{\"DatabaseName\":\"P0052\"},{\"DatabaseName\":\"P0053\"},{\"DatabaseName\":\"P0054\"},{\"DatabaseName\":\"P0055\"},{\"DatabaseName\":\"P0056\"},{\"DatabaseName\":\"P0057\"},{\"DatabaseName\":\"P0058\"},{\"DatabaseName\":\"P0059\"},{\"DatabaseName\":\"P0060\"},{\"DatabaseName\":\"P0061\"},{\"DatabaseName\":\"P0062\"},{\"DatabaseName\":\"P0063\"},{\"DatabaseName\":\"P0064\"},{\"DatabaseName\":\"P0065\"},{\"DatabaseName\":\"P0066\"},{\"DatabaseName\":\"P0067\"},{\"DatabaseName\":\"P0068\"},{\"DatabaseName\":\"P0069\"},{\"DatabaseName\":\"P0070\"},{\"DatabaseName\":\"P0071\"},{\"DatabaseName\":\"P0072\"},{\"DatabaseName\":\"P0073\"},{\"DatabaseName\":\"P0074\"},{\"DatabaseName\":\"P0075\"},{\"DatabaseName\":\"P0076\"},{\"DatabaseName\":\"P0077\"},{\"DatabaseName\":\"P0078\"},{\"DatabaseName\":\"P0079\"},{\"DatabaseName\":\"P0080\"},{\"DatabaseName\":\"P0081\"},{\"DatabaseName\":\"P0082\"},{\"DatabaseName\":\"P0083\"},{\"DatabaseName\":\"P0084\"},{\"DatabaseName\":\"P0085\"},{\"DatabaseName\":\"P0086\"},{\"DatabaseName\":\"P0087\"},{\"DatabaseName\":\"P0088\"},{\"DatabaseName\":\"P0089\"},{\"DatabaseName\":\"P0090\"},{\"DatabaseName\":\"P0091\"},{\"DatabaseName\":\"P0092\"},{\"DatabaseName\":\"P0093\"},{\"DatabaseName\":\"P0094\"},{\"DatabaseName\":\"P0095\"},{\"DatabaseName\":\"P0096\"},{\"DatabaseName\":\"P0097\"},{\"DatabaseName\":\"P0098\"},{\"DatabaseName\":\"P0099\"},{\"DatabaseName\":\"P0100\"},{\"DatabaseName\":\"P0101\"},{\"DatabaseName\":\"P0102\"},{\"DatabaseName\":\"P0103\"},{\"DatabaseName\":\"P0104\"},{\"DatabaseName\":\"P0105\"},{\"DatabaseName\":\"P0106\"},{\"DatabaseName\":\"P0107\"},{\"DatabaseName\":\"UPrinceV4Einstein\"},{\"DatabaseName\":\"UPrinceV4ProjectTemplate\"}]";
        //var json = "[{\"DatabaseName\":\"COM0001\"},{\"DatabaseName\":\"COM0053\"},{\"DatabaseName\":\"P0001\"},{\"DatabaseName\":\"P0002\"},{\"DatabaseName\":\"P0003\"},{\"DatabaseName\":\"P0004\"},{\"DatabaseName\":\"P0005\"},{\"DatabaseName\":\"P0007\"},{\"DatabaseName\":\"P0008\"},{\"DatabaseName\":\"P0010\"},{\"DatabaseName\":\"P0011\"},{\"DatabaseName\":\"P0013\"},{\"DatabaseName\":\"P0014\"},{\"DatabaseName\":\"P0015\"},{\"DatabaseName\":\"P0016\"},{\"DatabaseName\":\"P0017\"},{\"DatabaseName\":\"P0018\"},{\"DatabaseName\":\"P0019\"},{\"DatabaseName\":\"P0021\"},{\"DatabaseName\":\"P0022\"},{\"DatabaseName\":\"P0023\"},{\"DatabaseName\":\"P0024\"},{\"DatabaseName\":\"P0025\"},{\"DatabaseName\":\"P0026\"},{\"DatabaseName\":\"P0027\"},{\"DatabaseName\":\"P0028\"},{\"DatabaseName\":\"P0029\"},{\"DatabaseName\":\"P0030\"},{\"DatabaseName\":\"P0031\"},{\"DatabaseName\":\"P0032\"},{\"DatabaseName\":\"P0033\"},{\"DatabaseName\":\"P0034\"},{\"DatabaseName\":\"P0035\"},{\"DatabaseName\":\"P0036\"},{\"DatabaseName\":\"P0037\"},{\"DatabaseName\":\"P0038\"},{\"DatabaseName\":\"P0039\"},{\"DatabaseName\":\"P0040\"},{\"DatabaseName\":\"P0041\"},{\"DatabaseName\":\"P0042\"},{\"DatabaseName\":\"P0043\"},{\"DatabaseName\":\"P0044\"},{\"DatabaseName\":\"P0045\"},{\"DatabaseName\":\"P0046\"},{\"DatabaseName\":\"P0047\"},{\"DatabaseName\":\"P0048\"},{\"DatabaseName\":\"P0049\"},{\"DatabaseName\":\"P0050\"},{\"DatabaseName\":\"P0051\"},{\"DatabaseName\":\"P0052\"},{\"DatabaseName\":\"P0053\"},{\"DatabaseName\":\"P0054\"},{\"DatabaseName\":\"UPrinceV4ProjectTemplate\"},{\"DatabaseName\":\"UPrinceV4UAT\"}]";
        var result = new List<Databases>();
        //uprincev4uatdb
        //uprincev4einstein
        // var env = "uprincev4einstein";
        var exceptionLst = new List<DatabasesEx>();
        using (var connection = new SqlConnection("Server=tcp:" + env +
                                                  ".database.windows.net,1433;Initial Catalog=master;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        {
            result = connection
                .Query<Databases>(
                    @"select [name] as DatabaseName from sys.databases WHERE name NOT IN('master', 'MsalTokenCacheDatabase', 'UPrinceV4EinsteinCatelog') order by name")
                .ToList();
        }


        var queries = sql.Split("GO");
        foreach (var project in result)
        {
            var connectionString = "Server=tcp:" + env + ".database.windows.net,1433;Initial Catalog=" +
                                   project.DatabaseName +
                                   ";Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            {
                await using var connection = new SqlConnection(connectionString);

                foreach (var query in queries)
                {
                    try
                    {
                        await connection.ExecuteAsync(query);
                    }
                    catch (Exception ex)
                    {
                        var mDatabasesEx = new DatabasesEx();
                        mDatabasesEx.DatabaseName = project.DatabaseName;
                        mDatabasesEx.Exception = ex;
                        exceptionLst.Add(mDatabasesEx);
                    }
                }
            }
        }

        return exceptionLst;
    }

    [AllowAnonymous]
    [HttpGet("AllWF")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<AllWF>> AllWF([FromHeader(Name = "env")] string env)
    {
        var result = new List<ProjectDefinition>();

        var exceptionLst = new List<DatabasesEx>();
        using (var connection = new SqlConnection("Server=tcp:" + env +
                                                  ".database.windows.net,1433;Initial Catalog=UPrinceV4Einstein;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
        {
            result = connection
                .Query<ProjectDefinition>(
                    @"SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0 ORDER BY SequenceCode DESC")
                .ToList();
        }

        var wf = new List<AllWF>();
        foreach (var project in result)
        {
            using (var connection = new SqlConnection(project.ProjectConnectionString))
            {
                try
                {
                    var id = connection.Query<AllWF>("SELECT SequenceId,@SequenceCode AS Project From dbo.WFHeader ",
                        new { project.SequenceCode }).ToList();
                    if (id != null) wf.AddRange(id);
                }
                catch (Exception ex)
                {
                    var mDatabasesEx = new DatabasesEx();
                    mDatabasesEx.DatabaseName = project.SequenceCode;
                    mDatabasesEx.Exception = ex;
                    exceptionLst.Add(mDatabasesEx);
                }
            }
        }

        return wf;
    }

    [AllowAnonymous]
    [HttpGet("EfMigrate")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<string> EfMigrate()
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString =
            "Server=tcp:uprincev4staging.database.windows.net,1433;Initial Catalog=P0003; Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;";
        using (var context = new ShanukaDbContext(options, connectionString, _iTenantProvider))
        {
            context.Database.Migrate();
        }


        return "";
    }

    [AllowAnonymous]
    [HttpGet("POExcel")]
    public async Task<ActionResult> POExcel([FromHeader(Name = "AuthToken")] string AuthToken,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromHeader(Name = "CU")] string CU,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var _pmolParameter = new PmolParameter();
            var lang = langCode(langX);
            _pmolParameter.ContractingUnitSequenceId = Project;
            _pmolParameter.ProjectSequenceId = CU;
            //_pmolParameter.AuthToken = Request.Headers["AuthToken"].FirstOrDefault();


            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                _pmolParameter.Lang = lang;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = _iTenantProvider;

                var pOResourcesAddDtoDictionaryR = new Dictionary<string, POAllResourcesExcelDto>();
                var connectionString = ConnectionString.MapConnectionString(CU, Project, _iTenantProvider);
                //List<POBORResources> resource = new List<POBORResources>();


                var PrjectCustomer =
                    @"select ProjectDefinition.SequenceCode as ID, ProjectTime.EndDate, CabPerson.FullName AS ContactPersonName,
                                    CabEmail.EmailAddress AS ContactPersonEmail , CabCompany.Name AS Customer
                                    from ProjectDefinition
                                    left outer join  ProjectTime on ProjectDefinition.Id=ProjectTime.ProjectId
                                    left outer join  ProjectTeam on ProjectDefinition.Id = ProjectTeam.ProjectId
                                    left outer join  ProjectTeamRole on ProjectTeam.Id = ProjectTeamRole.ProjectTeamId
                                    left outer join  CabPerson on ProjectTeamRole.CabPersonId = CabPerson.Id
                                    left outer join CabPersonCompany on CabPerson.Id = CabPersonCompany.PersonId
                                    left outer join CabCompany on CabCompany.Id = CabPersonCompany.CompanyId
                                    left outer join CabEmail on CabPersonCompany.EmailId = CabEmail.Id
                                    where ProjectDefinition.SequenceCode IN @Ids
                                    AND RoleId = '907b7af0-b132-4951-a2dc-6ab82d4cd40d'";

                var query =
                    @"SELECT DISTINCT POHeader.ProjectSequenceCode FROM dbo.POHeader WHERE ProjectSequenceCode IS NOT NULL";

                List<string> projectSequenceList;

                using (var connection = new SqlConnection(connectionString))
                {
                    projectSequenceList = connection.Query<string>(query).ToList();

                    var resourceX = connection
                        .Query<POAllResourcesExcelDto, POResourcesDocument, POHeaderExcelDto,
                            POAllResourcesExcelDto>
                        (@"SELECT
  POResources.*
 ,CpcResourceTypeLocalizedData.Label AS ResourceType
 ,POResources.PTitle AS ProjectCPCTitle
 ,POResources.CTitle AS CUCPCTitle
 ,POResourcesDocument.Id AS POResourcesDocumentId
 ,POResourcesDocument.Link
 ,POHeader.*
 
FROM dbo.POResources
LEFT OUTER JOIN dbo.POResourcesDocument
  ON POResources.Id = POResourcesDocument.POResourcesId
INNER JOIN dbo.POHeader
  ON POResources.PurchesOrderId = POHeader.Id
INNER JOIN dbo.CpcResourceTypeLocalizedData
  ON POResources.ResourcesType = CpcResourceTypeLocalizedData.CpcResourceTypeId
WHERE CpcResourceTypeLocalizedData.LanguageCode = @lang",
                            (pOResources, pOResourcesDocument, pOHeaderDto) =>
                            {
                                POAllResourcesExcelDto orderEntry;

                                if (!pOResourcesAddDtoDictionaryR.TryGetValue(pOResources.Id, out orderEntry))
                                {
                                    orderEntry = pOResources;
                                    orderEntry.PDocuments = new List<string>();
                                    pOResourcesAddDtoDictionaryR.Add(orderEntry.Id, orderEntry);
                                }

                                orderEntry.POHeaderDto = pOHeaderDto;

                                orderEntry.PDocuments.Add(pOResourcesDocument.Link);
                                if (orderEntry.PDocuments.Count > 0)
                                {
                                    if (orderEntry.PDocuments.First() == null)
                                    {
                                        orderEntry.PDocuments = null;
                                        return orderEntry;
                                    }

                                    return orderEntry;
                                }

                                orderEntry.PDocuments = null;
                                return orderEntry;
                            },
                            new { lang }, splitOn: "POResourcesDocumentId,ID").ToList();

                    IEnumerable<projectForPsDto> result;
                    using (var connectionX = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString))
                    {
                        result = connectionX.Query<projectForPsDto>(PrjectCustomer,
                            new { Ids = projectSequenceList });
                    }


                    foreach (var r in resourceX)
                    {
                        var customer = new PsCustomerReadDto();
                        var project = result.Where(p => p.Id == r.POHeaderDto.ProjectSequenceCode).FirstOrDefault();
                        if (project != null)
                        {
                            customer.ContactPersonName = project.ContactPersonName;
                            customer.ContactPersonEmail = project.ContactPersonEmail;
                            customer.CustomerName = project.Customer;
                            r.Customer = customer;
                        }

                        var POParameter = new POParameter();
                        POParameter.TenantProvider = _iTenantProvider;
                        POParameter.LocationId = r.POHeaderDto.LocationId;
                        if (r.POHeaderDto.LocationId != null)
                            r.POHeaderDto.MapLocation = await ReadLocation(POParameter);
                    }

                    return Ok(new ApiOkResponse(resourceX));
                }
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (OperationCanceledException ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [HttpGet("ReadLocation")]
    public async Task<MapLocation> ReadLocation(POParameter POParameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            //string connectionString = ConnectionString.MapConnectionString(pmolParameter.ContractingUnitSequenceId, pmolParameter.ProjectSequenceId, pmolParameter.TenantProvider);
            var context = new ShanukaDbContext(options, POParameter.TenantProvider.GetTenant().ConnectionString,
                POParameter.TenantProvider);


            var MapLocation = context.MapLocation.Where(L => L.Id == POParameter.LocationId).Include(m => m.Address)
                .Include(m => m.Position)
                .Include(m => m.Viewport).ThenInclude(v => v.BtmRightPoint)
                .Include(m => m.Viewport).ThenInclude(v => v.TopLeftPoint)
                .Include(b => b.BoundingBox).ThenInclude(v => v.BtmRightPoint)
                .Include(b => b.BoundingBox).ThenInclude(v => v.TopLeftPoint)
                .Include(d => d.DataSources).ThenInclude(d => d.Geometry).ToList().FirstOrDefault();

            return MapLocation;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    [AllowAnonymous]
    [HttpGet("ReadPmolId/{id}")]
    public async Task<ActionResult> ReadPmolId([FromHeader(Name = "AuthToken")] string AuthToken, string id,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                var lang = langCode(langX);

                _pmolParameter.ContractingUnitSequenceId = CU;
                _pmolParameter.ProjectSequenceId = Project;
                _pmolParameter.Lang = lang;
                _pmolParameter.Id = id;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = ItenantProvider;
                _pmolParameter.borRepository = _borRepository;
                _pmolParameter.borParameter = _borParameter;
                var mPmolDto = new PmolDto();
                mPmolDto.Header = await _iPmolRepository.GetPmolById(_pmolParameter);

                var pbsInstructionsRepositoryParameter =
                    new PbsInstructionsRepositoryParameter();
                pbsInstructionsRepositoryParameter.ContractingUnitSequenceId = CU;
                pbsInstructionsRepositoryParameter.ProjectSequenceId = Project;
                pbsInstructionsRepositoryParameter.Lang = lang;
                pbsInstructionsRepositoryParameter.TenantProvider = ItenantProvider;
                pbsInstructionsRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
                pbsInstructionsRepositoryParameter.PbsProductId = mPmolDto.Header.ProductId;

                mPmolDto.Instruction =
                    await _iPbsInstructionsRepository.GetAllPbsInstructionsByPbsProductAllTypes(
                        pbsInstructionsRepositoryParameter);

                var mPmolResourceParameter = new PmolResourceParameter();
                mPmolResourceParameter.ContractingUnitSequenceId = CU;
                mPmolResourceParameter.ProjectSequenceId = Project;
                mPmolResourceParameter.Lang = lang;
                mPmolResourceParameter.Id = mPmolDto.Header.Id;
                mPmolResourceParameter.ContextAccessor = ContextAccessor;
                mPmolResourceParameter.TenantProvider = ItenantProvider;

                var mxPmolResourceParameter = new PmolResourceParameter();
                mxPmolResourceParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
                mxPmolResourceParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
                mxPmolResourceParameter.Lang = lang;
                mxPmolResourceParameter.Id = mPmolDto.Header.Id;
                mxPmolResourceParameter.ContextAccessor = ContextAccessor;
                mxPmolResourceParameter.TenantProvider = ItenantProvider;

                var mPmolResourceReadAllDto = new PmolResourceReadAllDto();
                mPmolResourceReadAllDto.Labour = await _iPmolResourceRepository.ReadLabour(mxPmolResourceParameter);
                mPmolResourceReadAllDto.Consumable =
                    await _iPmolResourceRepository.ReadConsumable(mxPmolResourceParameter);
                mPmolResourceReadAllDto.Material =
                    await _iPmolResourceRepository.ReadMaterial(mxPmolResourceParameter);
                mPmolResourceReadAllDto.Tools = await _iPmolResourceRepository.ReadTools(mxPmolResourceParameter);

                mPmolDto.PlannedResource = mPmolResourceReadAllDto;

                var mPmolResourceReadAllDtoExtra = new PmolResourceReadAllDto();
                mPmolResourceReadAllDtoExtra.Labour =
                    await _iPmolResourceRepository.ReadExtraLabour(mxPmolResourceParameter);
                mPmolResourceReadAllDtoExtra.Consumable =
                    await _iPmolResourceRepository.ReadExtraConsumable(mxPmolResourceParameter);
                mPmolResourceReadAllDtoExtra.Material =
                    await _iPmolResourceRepository.ReadExtraMaterial(mxPmolResourceParameter);
                mPmolResourceReadAllDtoExtra.Tools =
                    await _iPmolResourceRepository.ReadExtraTools(mxPmolResourceParameter);

                mPmolDto.ExtraResource = mPmolResourceReadAllDtoExtra;


                _PbsQualityParameters.ContractingUnitSequenceId = CU;
                _PbsQualityParameters.ProjectSequenceId = Project;
                _PbsQualityParameters.TenantProvider = ItenantProvider;
                _PbsQualityParameters.PbsProductId = mPmolDto.Header.ProductId;
                _PbsQualityParameters.Lang = lang;

                mPmolDto.Quality =
                    await _IPbsQualityRepository.GetAllPbsQualityByPbsProductId(_PbsQualityParameters);

                var _PbsRiskParameters = new PbsRiskParameters();
                _PbsRiskParameters.ContractingUnitSequenceId = CU;
                _PbsRiskParameters.ProjectSequenceId = Project;

                _PbsRiskParameters.TenantProvider = ItenantProvider;
                _PbsRiskParameters.PbsProductId = mPmolDto.Header.ProductId;
                _PbsRiskParameters.Lang = lang;

                mPmolDto.Risk = await _IPbsRiskRepository.GetAllPbsRiskByPbsProductId(_PbsRiskParameters);


                _competenciesRepositoryParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
                _competenciesRepositoryParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
                //string message = ApiErrorMessages.GetErrorMessage(ItenantProvider, ErrorMessageKey.Ok, lang).Message;

                _competenciesRepositoryParameter.TenantProvider = ItenantProvider;
                _competenciesRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
                _competenciesRepositoryParameter.Lang = lang;
                _competenciesRepositoryParameter.PbsId = mPmolDto.Header.ProductId;

                mPmolDto.Competencies =
                    await _iCompetenciesRepository.GetCompetenceByPbsId(_competenciesRepositoryParameter);

                mPmolDto.MapLocation = await _iPmolRepository.GetLocationByPmolId(_pmolParameter);


                _PmolJournalParameter.ContractingUnitSequenceId = CU;
                _PmolJournalParameter.ProjectSequenceId = Project;
                _PmolJournalParameter.Lang = lang;
                _PmolJournalParameter.Id = mPmolDto.Header.Id;
                _PmolJournalParameter.ContextAccessor = ContextAccessor;
                _PmolJournalParameter.TenantProvider = ItenantProvider;

                mPmolDto.Journal = await _IPmolJournalRepository.ReadJournal(_PmolJournalParameter);
                _pmolParameter.Id = mPmolDto.Header.Id;
                mPmolDto.ExtraWork = await _iPmolRepository.GetExtraWorkByPmolId(_pmolParameter);
                mPmolDto.StopHandshake = await _iPmolRepository.GetStopHandShakesByPmolId(_pmolParameter);

                return Ok(new ApiOkResponse(mPmolDto));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("ReadPmolProjectsPM")]
    public async Task<ActionResult> ReadPmolProjectsPM([FromHeader(Name = "AuthToken")] string AuthToken)
    {
        try
        {
            var _pmolParameter = new PmolParameter();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                _pmolParameter.Lang = lang;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = _iTenantProvider;
                var shortcutPaneData = await _iPmolRepository.ReadPmolProjectsPM(_pmolParameter);

                return Ok(new ApiOkResponse(shortcutPaneData));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("ReadPmolProjectsPMWithDetail")]
    public async Task<ActionResult> ReadPmolProjectsPMWithDetail([FromHeader(Name = "AuthToken")] string AuthToken)
    {
        try
        {
            var _pmolParameter = new PmolParameter();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //_pmolParameter.AuthToken = Request.Headers["AuthToken"].FirstOrDefault();


            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                _pmolParameter.Lang = lang;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = _iTenantProvider;
                var shortcutPaneData = await _iPmolRepository.ReadPmolProjectsPMWithDetail(_pmolParameter);

                return Ok(new ApiOkResponse(shortcutPaneData));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("ReadBorByProjectsPM")]
    public async Task<ActionResult> ReadBorByProjectsPM([FromHeader(Name = "AuthToken")] string AuthToken)
    {
        try
        {
            var _pmolParameter = new PmolParameter();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //_pmolParameter.AuthToken = Request.Headers["AuthToken"].FirstOrDefault();


            //return Ok(new ApiOkResponse(shortcutPaneData));

            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                _pmolParameter.Lang = lang;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = _iTenantProvider;
                var shortcutPaneData = await _iPmolRepository.ReadBorByProjectsPM(_pmolParameter);

                return Ok(new ApiOkResponse(shortcutPaneData));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("ReadPbsByProjectsPM")]
    public async Task<ActionResult> ReadPbsByProjectsPM([FromHeader(Name = "AuthToken")] string AuthToken)
    {
        try
        {
            var _pmolParameter = new PmolParameter();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();
            //_pmolParameter.AuthToken = Request.Headers["AuthToken"].FirstOrDefault();
            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                _pmolParameter.Lang = lang;
                _pmolParameter.ContextAccessor = ContextAccessor;
                _pmolParameter.TenantProvider = _iTenantProvider;
                var shortcutPaneData = await _iPmolRepository.ReadPbsByProjectsPM(_pmolParameter);

                return Ok(new ApiOkResponse(shortcutPaneData));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("Tenant")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<TenantDto> Tenant()
    {
        return ItenantProvider.GetTenantDto();
    }

    [Authorize]
    [HttpGet("GetContractingUnits")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<UPrinceCustomerContractingUnit>> GetContractingUnits()
    {
        var tenant = ItenantProvider.GetTenant();
        var query = "select * from UPrinceCustomerContractingUnit";
        await using (var dbConnection = new SqlConnection(tenant.CatelogConnectionString))
        {
            var result = await dbConnection.QueryAsync<UPrinceCustomerContractingUnit>(query);
            return result.ToList();
        }
    }

    [Authorize]
    [HttpGet("GetContractingUnitsById")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetContractingUnitsById(string id)
    {
        var tenant = ItenantProvider.GetTenant();
        var query = @"select Name from CabCompany where SequenceCode = @id";
        var parameters = new { id };

        await using (var dbConnection = new SqlConnection(tenant.ConnectionString))
        {
            var result = await dbConnection.QueryAsync<string>(query, parameters);
            return Ok(new ApiOkResponse(result));
        }
    }

    [Authorize]
    [HttpGet("GetProjectsByContractingUnitId")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<ProjectDefinitionDto>> GetProjectsByContractingUnitId(string id)
    {
        var sql =
            "SELECT ProjectDefinition.Id ,ProjectDefinition.Description AS Description  ,ProjectDefinition.SequenceCode AS SequenceCode  ,ProjectDefinition.Name AS Name " +
            " ,ProjectDefinition.ProjManagementLevelId AS projectManagementLevelId  ,ProjectDefinition.ProjTemplateId AS projectTemplateId  ,ProjectDefinition.ProjToleranceStateId " +
            "AS projectToleranceStateId ,ProjectDefinition.ProjTypeId AS projectTypeId  ,ProjectDefinition.Title AS Title " +
            "FROM dbo.ProjectDefinition WHERE ProjectDefinition.ContractingUnitId = '" + id + "'";

        return _iTenantProvider.orgSqlConnection().Query<ProjectDefinitionDto>(sql).ToList();
        ;
    }


    [Authorize]
    [HttpPost("GetCpcByProjectSequenceCode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<BorCoperateProductCatalogFilterDto> GetCpcByProjectSequenceCode(
        [FromBody] CpcFilter filter, string code)
    {
        var sqlProject =
            "SELECT ProjectDefinition.Id , ProjectDefinition.ProjectConnectionString,  ProjectDefinition.ContractingUnitId AS ContractingUnitId,ProjectDefinition.Description AS Description  ,ProjectDefinition.SequenceCode AS SequenceCode  ,ProjectDefinition.Name AS Name " +
            " ,ProjectDefinition.ProjManagementLevelId AS projectManagementLevelId  ,ProjectDefinition.ProjTemplateId AS projectTemplateId  ,ProjectDefinition.ProjToleranceStateId " +
            "AS projectToleranceStateId ,ProjectDefinition.ProjTypeId AS projectTypeId  ,ProjectDefinition.Title AS Title " +
            "FROM dbo.ProjectDefinition WHERE ProjectDefinition.IsDeleted = 0 AND ProjectDefinition.SequenceCode = '" +
            code + "'";

        ProjectDefinitionDto resultProject = null;
        
        resultProject = _iTenantProvider.orgSqlConnection().Query<ProjectDefinitionDto>(sqlProject).FirstOrDefault();
        

        var lang = langCode(Request.Headers["lang"].FirstOrDefault());

        var borCoperateProductCatalogFilterDto = new BorCoperateProductCatalogFilterDto();
        var sql =
            "SELECT CorporateProductCatalog.ID,  CorporateProductCatalog.ResourceTitle ,CorporateProductCatalog.Status AS Status ,CorporateProductCatalog.ResourceNumber AS ResourceNumber " +
            " ,CpcResourceTypeLocalizedData.Label AS ResourceType  , CorporateProductCatalog.Title AS Title" +
            "  , CONCAT(CorporateProductCatalog.ResourceNumber, ' - ',  CorporateProductCatalog.ResourceTitle) AS HeaderTitle  FROM dbo.CpcResourceTypeLocalizedData " +
            " ,dbo.CorporateProductCatalog  INNER JOIN dbo.CpcResourceType ON CorporateProductCatalog.ResourceTypeId = CpcResourceType.Id " +
            "WHERE CpcResourceType.Id = CorporateProductCatalog.ResourceTypeId AND CpcResourceTypeLocalizedData.CpcResourceTypeId = CpcResourceType.Id " +
            "AND CorporateProductCatalog.IsDeleted=0 AND CpcResourceTypeLocalizedData.LanguageCode = '" + lang +
            "'";

        var sb = new StringBuilder(sql);
        if (filter != null)
        {
            if (filter.Title != null)
            {
                filter.Title = filter.Title.Replace("'", "''");
                var words = filter.Title.Split(" ");
                foreach (var element in words)
                    sb.Append(" AND CorporateProductCatalog.Title LIKE '%" + element + "%'");
            }

            if (filter.ResourceTypeId != null)
                sb.Append(" AND CpcResourceTypeLocalizedData.CpcResourceTypeId = '" + filter.ResourceTypeId +
                          "'");

            if (filter.Status != null) sb.Append(" AND CorporateProductCatalog.Status =" + filter.Status);

            if (filter.Sorter.Attribute == null) sb.Append(" ORDER BY CorporateProductCatalog.ResourceNumber");

            if (filter.Sorter != null)
            {
                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("asc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " ASC");

                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("desc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " DESC");
            }
        }

        IEnumerable<CoperateProductCatalogFilterDto> result = null;
        using (IDbConnection dbConnection = new SqlConnection(resultProject.ProjectConnectionString))
        {
            result = dbConnection.Query<CoperateProductCatalogFilterDto>(sb.ToString()).ToList();
            

            borCoperateProductCatalogFilterDto.Local = result.ToList();
        }
        
            borCoperateProductCatalogFilterDto.Central =
                _iTenantProvider.orgSqlConnection().Query<CoperateProductCatalogFilterDto>(sb.ToString()).ToList();
        

        var query = @"select * from UPrinceCustomerContractingUnit where SequenceCode = @id";
        var parameters = new { id = resultProject.ContractingUnitId };
        UPrinceCustomerContractingUnit resultContractingUnitx = _iTenantProvider.orgSqlConnection().Query<UPrinceCustomerContractingUnit>(query, parameters)
            .FirstOrDefault();

        using (IDbConnection dbConnection = new SqlConnection(resultContractingUnitx.ConnectionString))
        {
            borCoperateProductCatalogFilterDto.ContractingUnit =
                dbConnection.Query<CoperateProductCatalogFilterDto>(sb.ToString()).ToList();
        }

        return borCoperateProductCatalogFilterDto;
    }

    [Authorize]
    [HttpPost("GetProjectCpcByProjectSequenceCode")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<BorCoperateProductCatalogFilterDto> GetProjectCpcByProjectSequenceCode(
        [FromBody] CpcFilter filter, string code)
    {
        var sqlProject =
            "SELECT ProjectDefinition.Id , ProjectDefinition.ProjectConnectionString,  ProjectDefinition.ContractingUnitId AS ContractingUnitId,ProjectDefinition.Description AS Description  ,ProjectDefinition.SequenceCode AS SequenceCode  ,ProjectDefinition.Name AS Name " +
            " ,ProjectDefinition.ProjManagementLevelId AS projectManagementLevelId  ,ProjectDefinition.ProjTemplateId AS projectTemplateId  ,ProjectDefinition.ProjToleranceStateId " +
            "AS projectToleranceStateId ,ProjectDefinition.ProjTypeId AS projectTypeId  , ProjectDefinition.Title AS Title " +
            "FROM dbo.ProjectDefinition WHERE ProjectDefinition.SequenceCode = '" + code + "'";

        //var tenant1 = ItenantProvider.GetTenant();
        ProjectDefinitionDto resultProject = null;
        // using (IDbConnection dbConnection = new SqlConnection(tenant1.ConnectionString))
        // {
            resultProject = ItenantProvider.orgSqlConnection().Query<ProjectDefinitionDto>(sqlProject).FirstOrDefault();
        //     
        // }

        var lang = langCode(Request.Headers["lang"].FirstOrDefault());

        var borCoperateProductCatalogFilterDto = new BorCoperateProductCatalogFilterDto();
        var sql =
            @"SELECT CorporateProductCatalog.ID,  CorporateProductCatalog.ResourceTitle ,CorporateProductCatalog.Status AS Status ,CorporateProductCatalog.ResourceNumber AS ResourceNumber 
                                , CpcResourceTypeLocalizedData.Label AS ResourceType  , CorporateProductCatalog.Title AS Title
                                 , CONCAT(CorporateProductCatalog.ResourceNumber, ' - ', CorporateProductCatalog.ResourceTitle) AS HeaderTitle,
                                    CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId AS BasicUnitOfMeasureId , CpcBasicUnitOfMeasureLocalizedData.Label AS BasicUnitOfMeasure
                                    FROM dbo.CpcResourceTypeLocalizedData
                                     ,dbo.CorporateProductCatalog INNER JOIN dbo.CpcResourceType ON CorporateProductCatalog.ResourceTypeId = CpcResourceType.Id
                                         LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
                                        WHERE CpcResourceType.Id = CorporateProductCatalog.ResourceTypeId AND CpcResourceTypeLocalizedData.CpcResourceTypeId = CpcResourceType.Id
                                        AND CorporateProductCatalog.IsDeleted = 0 AND CpcResourceTypeLocalizedData.LanguageCode = @lang
                            AND (CpcBasicUnitOfMeasureLocalizedData.LanguageCode = @lang  OR CorporateProductCatalog.CpcBasicUnitOfMeasureId IS NULL)";

        var sb = new StringBuilder(sql);
        if (filter != null)
        {
            if (filter.Title != null)
            {
                filter.Title = filter.Title.Replace("'", "''");
                var words = filter.Title.Split(" ");
                foreach (var element in words)
                    sb.Append(" AND CorporateProductCatalog.Title LIKE '%" + element + "%'");
            }

            if (filter.ResourceTypeId != null)
                sb.Append(" AND CpcResourceTypeLocalizedData.CpcResourceTypeId = '" + filter.ResourceTypeId +
                          "'");

            if (filter.Status != null) sb.Append(" AND CorporateProductCatalog.Status =" + filter.Status);

            if (filter.Sorter != null)
            {
                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("asc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " ASC");

                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("desc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " DESC");
            }
        }

        IEnumerable<CoperateProductCatalogFilterDto> result = null;
        using (IDbConnection dbConnection = new SqlConnection(resultProject.ProjectConnectionString))
        {
            var param = new { lang };
            result = dbConnection.QueryAsync<CoperateProductCatalogFilterDto>(sb.ToString(), param).Result.ToList();
           // 

            borCoperateProductCatalogFilterDto.Local = result.ToList();
        }

        return borCoperateProductCatalogFilterDto;
    }


    [Authorize]
    [HttpPost("BORCPCFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<BorCPCFilterDto> BORCPCfilter(
        [FromBody] CpcBORFilter filter)
    {
        var contractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
        var projectSequenceId = Request.Headers["Project"].FirstOrDefault();

        var cuConnectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, null, _iTenantProvider);

        var projectonnectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId,
                _iTenantProvider);

        var lang = langCode(Request.Headers["lang"].FirstOrDefault());

        var borCoperateProductCatalogFilterDto = new BorCPCFilterDto();
        var sql =
            @"SELECT
  CorporateProductCatalog.Id
 ,CorporateProductCatalog.Title AS Title
FROM dbo.CorporateProductCatalog
WHERE CorporateProductCatalog.IsDeleted = 0 AND CorporateProductCatalog.Id NOT IN @cpcIds ";


        var cpcExistingItem = filter.ResourceTypeId switch
        {
            // ResourceType = "BorMaterial";
            "c46c3a26-39a5-42cc-n7k1-89655304eh6" => @"SELECT
                                        BorMaterial.CorporateProductCatalogId
                                      FROM dbo.BorMaterial
                                      WHERE BorMaterial.BorProductId = @BorProductId",
            "c46c3a26-39a5-42cc-b07s-89655304eh6" => @"SELECT
                                        BorLabour.CorporateProductCatalogId
                                      FROM dbo.BorLabour
                                      WHERE BorLabour.BorProductId = @BorProductId",
            "c46c3a26-39a5-42cc-m06g-89655304eh6" => @"SELECT
                                        BorConsumable.CorporateProductCatalogId
                                      FROM dbo.BorConsumable
                                      WHERE BorConsumable.BorProductId = @BorProductId",
            "c46c3a26-39a5-42cc-n9wn-89655304eh6" => @"SELECT
                                        BorTools.CorporateProductCatalogId
                                      FROM dbo.BorTools
                                      WHERE BorTools.BorProductId = @BorProductId",
            _ => @"SELECT
                                        BorMaterial.CorporateProductCatalogId
                                      FROM dbo.@ResourceType
                                      WHERE @ResourceType.BorProductId = @BorProductId"
        };

        IEnumerable<string> cpcExistingItemList = null;
        using (IDbConnection dbConnection = new SqlConnection(projectonnectionString))
        {
            var param = new { lang, BorProductId = filter.BORId };
            cpcExistingItemList = dbConnection.Query<string>(cpcExistingItem, param).ToList();
            
        }

        var paramNew = new { lang, cpcIds = cpcExistingItemList };

        var sb = new StringBuilder(sql);
        if (filter != null)
        {
            if (filter.Title != null)
            {
                filter.Title = filter.Title.Replace("'", "''");
                var words = filter.Title.Split(" ");
                foreach (var element in words)
                    sb.Append(" AND CorporateProductCatalog.Title LIKE '%" + element + "%'");
            }

            if (filter.ResourceTypeId != null) sb.Append(" AND ResourceTypeId = '" + filter.ResourceTypeId + "'");

            if (filter.Status != null) sb.Append(" AND CorporateProductCatalog.Status =" + filter.Status);

            if (filter.Sorter != null)
            {
                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("asc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " ASC");

                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("desc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " DESC");
            }
        }

        IEnumerable<CoperateProductCatalogFilterDto> result = null;
        using (IDbConnection dbConnection = new SqlConnection(projectonnectionString))
        {
            borCoperateProductCatalogFilterDto.Local =
                dbConnection.Query<CPCFilterDto>(sb.ToString(), paramNew).ToList();
            
        }

        using (IDbConnection dbConnection = new SqlConnection(cuConnectionString))
        {
            borCoperateProductCatalogFilterDto.ContractingUnit =
                dbConnection.Query<CPCFilterDto>(sb.ToString(), paramNew).ToList();
            borCoperateProductCatalogFilterDto.ContractingUnit.RemoveAll(x => x.Title.StartsWith("PAr"));
            
        }

        using (IDbConnection dbConnection = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString))
        {
            borCoperateProductCatalogFilterDto.Central =
                dbConnection.Query<CPCFilterDto>(sb.ToString(), paramNew).ToList();
            
        }

        return borCoperateProductCatalogFilterDto;
    }

    [Authorize]
    [HttpPost("PBSCPCFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<PBSCPCFilterDto> PBSCPCFilter(
        [FromBody] PBSBORFilter filter)
    {
        var contractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
        var projectSequenceId = Request.Headers["Project"].FirstOrDefault();

        var cuConnectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, null, _iTenantProvider);

        var projectonnectionString =
            ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId,
                _iTenantProvider);

        var lang = langCode(Request.Headers["lang"].FirstOrDefault());

        var borCoperateProductCatalogFilterDto = new PBSCPCFilterDto();
        var sql =
            @"SELECT
  CorporateProductCatalog.Id
 ,CorporateProductCatalog.Title AS Title
FROM dbo.CorporateProductCatalog
WHERE CorporateProductCatalog.IsDeleted = 0 AND CorporateProductCatalog.Id NOT IN @cpcIds ";

        var cpcExistingItem = filter.ResourceTypeId switch
        {
            "c46c3a26-39a5-42cc-n7k1-89655304eh6" => @"SELECT
                                        PbsMaterial.CoperateProductCatalogId
                                      FROM dbo.PbsMaterial
                                      WHERE PbsMaterial.PbsProductId = @PBSId",
            "c46c3a26-39a5-42cc-b07s-89655304eh6" => @"SELECT
                                        PbsLabour.CoperateProductCatalogId
                                      FROM dbo.PbsLabour
                                      WHERE PbsLabour.PbsProductId = @PBSId",
            "c46c3a26-39a5-42cc-m06g-89655304eh6" => @"SELECT
                                        PbsConsumable.CoperateProductCatalogId
                                      FROM dbo.PbsConsumable
                                      WHERE PbsConsumable.PbsProductId = @PBSId",
            "c46c3a26-39a5-42cc-n9wn-89655304eh6" => @"SELECT
                                        PbsTools.CoperateProductCatalogId
                                      FROM dbo.PbsTools
                                      WHERE PbsTools.PbsProductId = @PBSId",
            _ => ""
        };

        IEnumerable<string> cpcExistingItemList = null;
        using (IDbConnection dbConnection = new SqlConnection(projectonnectionString))
        {
            var param = new { filter.PBSId };
            cpcExistingItemList = dbConnection.Query<string>(cpcExistingItem, param).ToList();
            
        }


        var paramNew = new { lang, cpcIds = cpcExistingItemList };


        var sb = new StringBuilder(sql);
        if (filter != null)
        {
            if (filter.Title != null)
            {
                filter.Title = filter.Title.Replace("'", "''");
                var words = filter.Title.Split(" ");
                foreach (var element in words)
                    sb.Append(" AND CorporateProductCatalog.Title LIKE '%" + element + "%'");
            }

            if (filter.ResourceTypeId != null) sb.Append(" AND ResourceTypeId = '" + filter.ResourceTypeId + "'");

            if (filter.Status != null) sb.Append(" AND CorporateProductCatalog.Status =" + filter.Status);

            if (filter.Sorter != null)
            {
                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("asc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " ASC");

                if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("desc"))
                    sb.Append(" ORDER BY " + filter.Sorter.Attribute + " DESC");
            }
        }

        using (IDbConnection dbConnection = new SqlConnection(projectonnectionString))
        {
            borCoperateProductCatalogFilterDto.Local =
                dbConnection.Query<CPCFilterDto>(sb.ToString(), paramNew).ToList();
            
        }

        using (IDbConnection dbConnection = new SqlConnection(cuConnectionString))
        {
            borCoperateProductCatalogFilterDto.ContractingUnit =
                dbConnection.Query<CPCFilterDto>(sb.ToString(), paramNew).ToList();
            borCoperateProductCatalogFilterDto.ContractingUnit.RemoveAll(x => x.Title.StartsWith("PAr"));
            
        }

        using (IDbConnection dbConnection = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString))
        {
            borCoperateProductCatalogFilterDto.Central =
                dbConnection.Query<CPCFilterDto>(sb.ToString(), paramNew).ToList();
            
        }

        return borCoperateProductCatalogFilterDto;
    }

    [Authorize]
    [HttpGet("CreateDatabase")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<int> CreateDatabase()
    {
        try
        {
            var projectTemplateDbName = "UPrinceV4ProjectTemplate";
            var idGenerator = new IdGenerator();
            var newDbName = idGenerator.GenerateId(UPrinceCustomerContext, "P", "ProjectSequenceCode ");
            var connectionString = "Server=tcp:upuatprod.database.windows.net,1433;Persist Security Info=False;" +
                                   "User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;" +
                                   "TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;Initial Catalog=" +
                                   newDbName + ";";

            var projectDefinitionDatabaseName = new ProjectDefinitionDatabaseName
            {
                Id = Guid.NewGuid().ToString(),
                SequenceCode = newDbName,
                IsUsed = false,
                ConnectionString = connectionString
            };
            UPrinceCustomerContext.ProjectDefinitionDatabaseName.Add(projectDefinitionDatabaseName);
            UPrinceCustomerContext.SaveChanges();
            var copyQuery = "CREATE DATABASE " + newDbName + " AS COPY OF " + projectTemplateDbName;
            using (var connection = new SqlConnection(ItenantProvider.GetTenant().ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(copyQuery, connection);
                command.CommandTimeout = 300;
                command.ExecuteNonQuery();
            }

            return 0;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [Authorize]
    [HttpPost("PMOLCPCFilter")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PMOLCPCFilter([FromBody] CpcPmolFilter filter)
    {
        try
        {
            var contractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            var projectSequenceId = Request.Headers["Project"].FirstOrDefault();

            var connectionString =
                ConnectionString.MapConnectionString(contractingUnitSequenceId, projectSequenceId,
                    _iTenantProvider);

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var borCoperateProductCatalogFilterDto = new PMolCPCFilterDto();
            var sql =
                @"SELECT
  CorporateProductCatalog.Id
 ,CorporateProductCatalog.Title AS Title
FROM dbo.CorporateProductCatalog WHERE CorporateProductCatalog.Id NOT IN @cpcIds";

            var cpcExistingItem = filter.ResourceTypeId switch
            {
                "c46c3a26-39a5-42cc-n7k1-89655304eh6" => @"SELECT
                                        PMolPlannedWorkMaterial.CoperateProductCatalogId
                                      FROM dbo.PMolPlannedWorkMaterial
                                      WHERE PMolPlannedWorkMaterial.Type = @Type AND PMolPlannedWorkMaterial.PmolId = @PmolId AND IsDeleted = 0 AND PMolPlannedWorkMaterial.CoperateProductCatalogId IS NOT NULL",
                "c46c3a26-39a5-42cc-b07s-89655304eh6" => @"SELECT
                                        PMolPlannedWorkLabour.CoperateProductCatalogId
                                      FROM dbo.PMolPlannedWorkLabour
                                      WHERE PMolPlannedWorkLabour.Type = @Type AND PMolPlannedWorkLabour.PmolId = @PmolId AND IsDeleted = 0 AND PMolPlannedWorkLabour.CoperateProductCatalogId IS NOT NULL",
                "c46c3a26-39a5-42cc-m06g-89655304eh6" => @"SELECT
                                        PMolPlannedWorkConsumable.CoperateProductCatalogId
                                      FROM dbo.PMolPlannedWorkConsumable
                                      WHERE PMolPlannedWorkConsumable.Type = @Type AND PMolPlannedWorkConsumable.PmolId = @PmolId AND IsDeleted = 0 AND PMolPlannedWorkConsumable.CoperateProductCatalogId IS NOT NULL",
                "c46c3a26-39a5-42cc-n9wn-89655304eh6" => @"SELECT
                                        PMolPlannedWorkTools.CoperateProductCatalogId
                                      FROM dbo.PMolPlannedWorkTools
                                      WHERE PMolPlannedWorkTools.Type = @Type AND PMolPlannedWorkTools.PmolId = @PmolId AND IsDeleted = 0 AND PMolPlannedWorkTools.CoperateProductCatalogId IS NOT NULL",
                _ => ""
            };

            IEnumerable<string> cpcExistingItemList = null;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                var param = new { filter.PmolId, Type = filter.type };
                cpcExistingItemList = dbConnection.Query<string>(cpcExistingItem, param).ToList();
                
            }


            var paramNew = new { lang, cpcIds = cpcExistingItemList };


            var sb = new StringBuilder(sql);
            if (filter != null)
            {
                if (filter.Title != null)
                {
                    filter.Title = filter.Title.Replace("'", "''");
                    var words = filter.Title.Split(" ");
                    foreach (var element in words)
                        sb.Append(" AND CorporateProductCatalog.Title LIKE '%" + element + "%'");
                }

                if (filter.ResourceTypeId != null) sb.Append(" AND ResourceTypeId = '" + filter.ResourceTypeId + "'");

                if (filter.Status != null) sb.Append(" AND CorporateProductCatalog.Status =" + filter.Status);

                if (filter.Sorter != null)
                {
                    if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("asc"))
                        sb.Append(" ORDER BY " + filter.Sorter.Attribute + " ASC");

                    if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("desc"))
                        sb.Append(" ORDER BY " + filter.Sorter.Attribute + " DESC");
                }
            }

            // var sqlNickName =
            //     @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId IN @CoperateProductCatalogId AND LocaleCode LIKE '%" +
            //     lang + "%'";

            IEnumerable<PMOLCPCFilterDto> result = null;
            // var resultNew = new List<PMOLCPCFilterDto>();
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                // var param = new {lang, filter.PmolId};
                result = dbConnection.Query<PMOLCPCFilterDto>(sb.ToString(), paramNew).ToList();

                // if (lang != "nl")
                // {
                //     var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                //         new {CoperateProductCatalogId = result.Select(r => r.Id).ToList()});
                //
                //     
                //     Parallel.ForEach(result, pR =>
                //     {
                //         var cpcResourceNicknames = resultNickNames as CpcResourceNickname[] ?? resultNickNames.ToArray();
                //         if (cpcResourceNicknames.Length > 0)
                //         {
                //             var t = cpcResourceNicknames
                //                 .FirstOrDefault(rN => rN.CoperateProductCatalogId == pR.Id)?.NickName;
                //             if (t != null)
                //             {
                //                 pR.Title = pR.ResourceNumber + " - " + t;;
                //             }
                //         }
                //     });
                // }

                

                borCoperateProductCatalogFilterDto.Local = result.ToList();
            }

            // if (filter != null)
            //     if (lang != "nl")
            //     {
            //         if (filter.Title != null)
            //         {
            //             var words = filter.Title.Split(" ");
            //
            //             Parallel.ForEach(words, element =>
            //             {
            //                 if (element.Length > 0)
            //                     resultNew.AddRange(result
            //                         .Where(X => X.Title.ToLower().Contains(element.ToLower())).ToList()); 
            //             });
            //
            //             if (filter.Title == "") resultNew = result.ToList();
            //         }
            //
            //         borCoperateProductCatalogFilterDto.Local =
            //             resultNew.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
            //     }

            return Ok(borCoperateProductCatalogFilterDto);
        }
        catch (Exception ex)
        {
            return Ok(new ApiResponse(400, false, ex.Message));
        }
    }


    [Authorize]
    [HttpGet("CreateDatabase/{fromDb}/{toDb}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<int> CopyDatabase(string fromDb, string toDb)
    {
        try
        {
            var projectTemplateDbName = fromDb;
            var idGenerator = new IdGenerator();
            var newDbName = toDb;
            var connectionString = "Server=tcp:upuatprod.database.windows.net,1433;Persist Security Info=False;" +
                                   "User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;" +
                                   "TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;Initial Catalog=" +
                                   newDbName + ";";

            var projectDefinitionDatabaseName = new ProjectDefinitionDatabaseName
            {
                Id = Guid.NewGuid().ToString(),
                SequenceCode = newDbName,
                IsUsed = false,
                ConnectionString = connectionString
            };
            UPrinceCustomerContext.ProjectDefinitionDatabaseName.Add(projectDefinitionDatabaseName);
            UPrinceCustomerContext.SaveChanges();
            var copyQuery = "CREATE DATABASE " + newDbName + " AS COPY OF " + projectTemplateDbName;
            using (var connection = new SqlConnection(ItenantProvider.GetTenant().ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(copyQuery, connection);
                command.CommandTimeout = 300;
                command.ExecuteNonQuery();
            }

            return 0;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [Authorize]
    [HttpGet("CreateDatabases")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<int> CreateDatabases(int numberOfDbs)
    {
        try
        {
            var projectTemplateDbName = "UPrinceV4ProjectTemplate";
            for (var i = 1; i <= numberOfDbs; i++)
            {
                var idGenerator = new IdGenerator();
                var newDbName = idGenerator.GenerateId(UPrinceCustomerContext, "P", "ProjectSequenceCode ");
                var connectionString =
                    "Server=tcp:upuatprod.database.windows.net,1433;Persist Security Info=False;" +
                    "User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;" +
                    "TrustServerCertificate=False;Connection Timeout=30; MultipleActiveResultSets=true;Initial Catalog=" +
                    newDbName + ";";
                var projectDefinitionDatabaseName = new ProjectDefinitionDatabaseName
                {
                    Id = Guid.NewGuid().ToString(),
                    SequenceCode = newDbName,
                    IsUsed = false,
                    ConnectionString = connectionString
                };
                UPrinceCustomerContext.ProjectDefinitionDatabaseName.Add(projectDefinitionDatabaseName);
                UPrinceCustomerContext.SaveChanges();

                var copyQuery = "CREATE DATABASE " + newDbName + " AS COPY OF " + projectTemplateDbName;
                using (var connection = new SqlConnection(ItenantProvider.GetTenant().ConnectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(copyQuery, connection);
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }
            }

            return 0;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [Authorize]
    [HttpPost("SendInvitation")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<string> SendInvitation(string id, string recipientEmail, string displayName,
        string messageBody, string ccEmail)
    {
        try
        {
            var tenant = ItenantProvider.GetTenant();
            var resultJson = AzureInvitation.SendInvitation(recipientEmail, displayName, messageBody, ccEmail, true,
                tenant.CatelogConnectionString);
            var result = JObject.Parse(resultJson);
            var user = result["invitedUser"];
            var userId = (string)user["id"];
            var personCompany = _dbContext.CabPersonCompany.FirstOrDefault(p => p.Id.Equals(id));
            personCompany.Oid = userId;
            _dbContext.CabPersonCompany.Update(personCompany);
            _dbContext.SaveChanges();
            return resultJson;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //getproject and contracting unit by user

    [Authorize]
    [HttpGet("GetProjectsByUser")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<ProjectDefinitionDto>> GetProjectsByUser()
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();

            var tenant = ItenantProvider.GetTenant();
            var sql = @"SELECT
                              ProjectDefinition.Id ,ProjectDefinition.Description AS Description  
                              ,ProjectDefinition.SequenceCode AS SequenceCode  ,ProjectDefinition.Name AS Name 
                              ,ProjectDefinition.ProjManagementLevelId AS projectManagementLevelId  
                              ,ProjectDefinition.ProjTemplateId AS projectTemplateId  
                              ,ProjectDefinition.ProjToleranceStateId  AS projectToleranceStateId 
                              ,ProjectDefinition.ProjTypeId AS projectTypeId  , ProjectDefinition.Title AS Title
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectUserRole
                              ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                            INNER JOIN dbo.UserRole
                              ON ProjectUserRole.UsrRoleId = UserRole.Id
                            INNER JOIN dbo.ApplicationUser
                              ON UserRole.ApplicationUserId = ApplicationUser.Id
                            WHERE ApplicationUser.OId = '" + user.OId + "'";

            IEnumerable<ProjectDefinitionDto> result = null;
            using (IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString))
            {
                result = dbConnection.Query<ProjectDefinitionDto>(sql).ToList();
                

                return result;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [Authorize]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("GetProjectsByUserProjectFilter")]
    public async Task<ActionResult> GetProjectsByUserProjectFilter(
        [FromBody] ProjectFilter filter, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.FirstOrDefault().Claims
                .First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                .Value;
            // var Email = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            //     claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            // var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var tenant = ItenantProvider.GetTenant();

            if (!myEnv)
            {
                var sql = @"SELECT DISTINCT
                          ProjectDefinition.SequenceCode AS SequenceCode
                         ,ProjectDefinition.Name AS Name
                         ,ProjectDefinition.Id AS ProjectDefinitionId
                         ,ProjectTemplate.Name AS ProjectTemplateName
                         ,ProjectToleranceState.Name AS ProjectToleranceStateName
                         ,ProjectType.Name AS ProjectTypeName
                         ,ProjectManagementLevel.Name AS ProjectManagementLevelName
                         ,ProjectDefinition.Title AS Title
                         ,ProjectStateLocalizedData.Label AS ProjectStatusName
                         ,CabPerson.FullName AS CustomerName
                         ,cp.FullName AS ProjectManagerName
                         ,cab.FullName AS SiteManagerName
                         ,ProjectClassificationSector.Name AS SectorName
                         ,Address.FreeformAddress AS ProjectAddress
                         ,ProjectCiawSite.CiawSiteCode AS CiawNumber
                         ,cc.Name AS CustomerCompanyName
                        FROM dbo.ProjectDefinition
                        LEFT OUTER JOIN dbo.ProjectTemplate
                          ON ProjectDefinition.ProjTemplateId = ProjectTemplate.TemplateId
                        LEFT OUTER JOIN dbo.ProjectToleranceState
                          ON ProjectDefinition.ProjToleranceStateId = ProjectToleranceState.ProjectToleranceStateId
                        LEFT OUTER JOIN dbo.ProjectType
                          ON ProjectDefinition.ProjTypeId = ProjectType.ProjectTypeId
                        LEFT OUTER JOIN dbo.ProjectManagementLevel
                          ON ProjectDefinition.ProjManagementLevelId = ProjectManagementLevel.ProjectManagementLevelId
                        LEFT OUTER JOIN dbo.ProjectUserRole
                          ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                        LEFT OUTER JOIN dbo.UserRole
                          ON ProjectUserRole.UsrRoleId = UserRole.Id
                        LEFT OUTER JOIN dbo.ApplicationUser
                          ON UserRole.ApplicationUserOid = ApplicationUser.Oid
                        LEFT OUTER JOIN dbo.CabCompany
                          ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                        LEFT OUTER JOIN dbo.ProjectScopeStatus
                          ON ProjectDefinition.ProjectScopeStatusId = ProjectScopeStatus.StatusId
                        LEFT OUTER JOIN dbo.CabPerson
                          ON ProjectDefinition.CustomerId = CabPerson.Id
                          LEFT OUTER JOIN dbo.CabPersonCompany cpc
                          ON CabPerson.Id = cpc.PersonId
                          LEFT OUTER JOIN dbo.CabCompany cc
                          ON cpc.CompanyId = cc.Id
                        LEFT OUTER JOIN dbo.CabPerson cp
                          ON ProjectDefinition.ProjectManagerId = cp.Id
                        LEFT OUTER JOIN dbo.CabPerson cab
                          ON ProjectDefinition.SiteManagerId = cab.Id
                        LEFT OUTER JOIN dbo.ProjectStateLocalizedData
                          ON ProjectDefinition.ProjectStatus = ProjectStateLocalizedData.ProjectStateId
                        LEFT OUTER JOIN dbo.ProjectClassification
                          ON ProjectDefinition.Id = ProjectClassification.ProjectId
                        LEFT OUTER JOIN dbo.ProjectClassificationSector
                          ON ProjectClassification.ProjectClassificationSectorId = ProjectClassificationSector.TypeId
                        LEFT OUTER JOIN MapLocation ml
                          ON ProjectDefinition.LocationId = ml.Id
                        LEFT OUTER JOIN Address
                          ON ml.AddressId = Address.Id
                        LEFT OUTER JOIN ProjectCiawSite
                          ON ProjectDefinition.Id = ProjectCiawSite.ProjectId
                        WHERE (ProjectTemplate.LanguageCode = @lang
                        OR ProjectDefinition.ProjTemplateId IS NULL)
                        AND ProjectToleranceState.LanguageCode = @lang
                        AND ProjectType.LanguageCode = @lang
                        AND ProjectManagementLevel.LanguageCode = @lang
                        AND (ProjectStateLocalizedData.LanguageCode = @lang
                        OR ProjectDefinition.ProjectStatus IS NULL)
                        AND (ProjectClassificationSector.LanguageCode = @lang
                        OR ProjectClassification.ProjectClassificationSectorId IS NULL)
                        AND ApplicationUser.Oid = @oid
                        AND CabCompany.SequenceCode = @cuId
                        AND ProjectDefinition.IsDeleted = 0 ";
                // AND ( ProjectStatus NOT IN ('d7e13082-77f4-44ad-8ad8-b0b9dad94ac1') OR ProjectStatus IS NULL) ";

                var sql2 = @"SELECT DISTINCT
                          ProjectDefinition.SequenceCode AS SequenceCode
                         ,ProjectDefinition.Name AS Name
                         ,ProjectDefinition.Id AS ProjectDefinitionId
                         ,ProjectTemplate.Name AS ProjectTemplateName
                         ,ProjectToleranceState.Name AS ProjectToleranceStateName
                         ,ProjectType.Name AS ProjectTypeName
                         ,ProjectManagementLevel.Name AS ProjectManagementLevelName
                         ,ProjectDefinition.Title AS Title
                         ,ProjectStateLocalizedData.Label AS ProjectStatusName
                         ,CabPerson.FullName AS CustomerName
                         ,cp.FullName AS ProjectManagerName
                         ,cab.FullName AS SiteManagerName
                         ,ProjectClassificationSector.Name AS SectorName
                         ,Address.FreeformAddress AS ProjectAddress
                         ,ProjectCiawSite.CiawSiteCode AS CiawNumber
                         ,cc.Name AS CustomerCompanyName
                        FROM dbo.ProjectDefinition
                        LEFT OUTER JOIN dbo.ProjectTemplate
                          ON ProjectDefinition.ProjTemplateId = ProjectTemplate.TemplateId
                        LEFT OUTER JOIN dbo.ProjectToleranceState
                          ON ProjectDefinition.ProjToleranceStateId = ProjectToleranceState.ProjectToleranceStateId
                        LEFT OUTER JOIN dbo.ProjectType
                          ON ProjectDefinition.ProjTypeId = ProjectType.ProjectTypeId
                        LEFT OUTER JOIN dbo.ProjectManagementLevel
                          ON ProjectDefinition.ProjManagementLevelId = ProjectManagementLevel.ProjectManagementLevelId
                        LEFT OUTER JOIN dbo.ProjectUserRole
                          ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                        LEFT OUTER JOIN dbo.UserRole
                          ON ProjectUserRole.UsrRoleId = UserRole.Id
                        LEFT OUTER JOIN dbo.ApplicationUser
                          ON UserRole.ApplicationUserOid = ApplicationUser.Oid
                        LEFT OUTER JOIN dbo.CabCompany
                          ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                        LEFT OUTER JOIN dbo.ProjectScopeStatus
                          ON ProjectDefinition.ProjectScopeStatusId = ProjectScopeStatus.StatusId
                        LEFT OUTER JOIN dbo.CabPerson
                          ON ProjectDefinition.CustomerId = CabPerson.Id
                          LEFT OUTER JOIN dbo.CabPersonCompany cpc
                          ON CabPerson.Id = cpc.PersonId
                          LEFT OUTER JOIN dbo.CabCompany cc
                          ON cpc.CompanyId = cc.Id
                        LEFT OUTER JOIN dbo.CabPerson cp
                          ON ProjectDefinition.ProjectManagerId = cp.Id
                        LEFT OUTER JOIN dbo.CabPerson cab
                          ON ProjectDefinition.SiteManagerId = cab.Id
                        LEFT OUTER JOIN dbo.ProjectStateLocalizedData
                          ON ProjectDefinition.ProjectStatus = ProjectStateLocalizedData.ProjectStateId
                        LEFT OUTER JOIN dbo.ProjectClassification
                          ON ProjectDefinition.Id = ProjectClassification.ProjectId
                        LEFT OUTER JOIN dbo.ProjectClassificationSector
                          ON ProjectClassification.ProjectClassificationSectorId = ProjectClassificationSector.TypeId
                        LEFT OUTER JOIN MapLocation ml
                          ON ProjectDefinition.LocationId = ml.Id
                        LEFT OUTER JOIN Address
                          ON ml.AddressId = Address.Id
                        LEFT OUTER JOIN ProjectCiawSite
                          ON ProjectDefinition.Id = ProjectCiawSite.ProjectId
                        WHERE (ProjectTemplate.LanguageCode = @lang
                        OR ProjectDefinition.ProjTemplateId IS NULL)
                        AND ProjectToleranceState.LanguageCode = @lang
                        AND ProjectType.LanguageCode = @lang
                        AND ProjectManagementLevel.LanguageCode = @lang
                        AND (ProjectStateLocalizedData.LanguageCode = @lang
                        OR ProjectDefinition.ProjectStatus IS NULL)
                        AND (ProjectClassificationSector.LanguageCode = @lang
                        OR ProjectClassification.ProjectClassificationSectorId IS NULL)
                        AND ApplicationUser.Oid = @oid
                        AND ProjectDefinition.IsDeleted = 0 ";

                var sb = ContractingUnitSequenceId != null ? new StringBuilder(sql) : new StringBuilder(sql2);

                if (filter.History != true)
                    sb.Append(
                        " AND ProjectScopeStatus.StatusId <> '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da' AND ProjectScopeStatus.LanguageCode = @lang");
                else
                    sb.Append(
                        " AND ProjectScopeStatus.StatusId = '7bcb4e8d-8e8c-487d-8170-6b91c89fc3da' AND ProjectScopeStatus.LanguageCode = @lang");

                if (filter.ManagementLevelId != null)
                    sb.Append(" AND ProjectDefinition.ProjManagementLevelId ='" + filter.ManagementLevelId + "'");

                if (filter.ProjectTypeId != null)
                    sb.Append(" AND ProjectDefinition.ProjTypeId ='" + filter.ProjectTypeId + "'");

                if (filter.Title != null)
                {
                    filter.Title = filter.Title.Replace("'", "''");
                    sb.Append(" AND ProjectDefinition.Title like '%" + filter.Title + "%' ");
                }


                if (filter.ToleranceStateId != null)
                    sb.Append(" AND ProjectDefinition.ProjToleranceStateId ='" + filter.ToleranceStateId + "'");

                if (filter.TemplateId != null)
                    sb.Append(" AND ProjectDefinition.ProjTemplateId ='" + filter.TemplateId + "'");

                if (filter.ProjectManagerId != null)
                    sb.Append(" AND ProjectDefinition.ProjectManagerId ='" + filter.ProjectManagerId + "'");

                if (filter.ProjectStatus != null)
                    sb.Append(" AND ProjectDefinition.ProjectStatus ='" + filter.ProjectStatus + "'");

                if (filter.ProjectClassificationSectorId != null)
                    sb.Append(" AND ProjectClassification.ProjectClassificationSectorId ='" +
                              filter.ProjectClassificationSectorId + "'");

                if (filter.CustomerId != null)
                    sb.Append(" AND ProjectDefinition.CustomerId ='" + filter.CustomerId + "'");

                if (filter.CustomerCompanyId != null)
                    sb.Append(" AND cc.Id ='" + filter.CustomerCompanyId + "'");

                if (filter.SiteManagerId != null)
                    sb.Append(" AND ProjectDefinition.SiteManagerId ='" + filter.SiteManagerId + "'");

                if (filter.projectAddress != null)
                {
                    filter.projectAddress = filter.projectAddress.Replace("'", "''");
                    sb.Append(" AND Address.FreeformAddress like '%" + filter.projectAddress + "%' ");
                }

                if (filter.ciawNumber != null)
                {
                    filter.ciawNumber = filter.ciawNumber.Replace("'", "''");
                    sb.Append(" AND ProjectCiawSite.CiawSiteCode like '%" + filter.ciawNumber + "%' ");
                }


                if (filter.Sorter.Attribute == null)
                {
                    //sb.Append(" ORDER BY cast((select SUBSTRING(ProjectDefinition.SequenceCode, PATINDEX('%[0-9]%', ProjectDefinition.SequenceCode), LEN(ProjectDefinition.SequenceCode))) as int) desc ");
                    sb.Append(" ORDER BY SequenceCode DESC ");
                }
                else
                {
                    if (filter.Sorter.Attribute == "projectManagerId")
                        sb.Append(" ORDER BY ProjectManagerName " + filter.Sorter.Order);
                    else if (filter.Sorter.Attribute == "customerId")
                        sb.Append(" ORDER BY CustomerName " + filter.Sorter.Order);
                    else if (filter.Sorter.Attribute == "siteManagerId")
                        sb.Append(" ORDER BY SiteManagerName " + filter.Sorter.Order);
                    else if (filter.Sorter.Attribute == "projectClassificationSectorId")
                        sb.Append(" ORDER BY SectorName " + filter.Sorter.Order);
                    else if (filter.Sorter.Attribute == "projectStatus")
                        sb.Append(" ORDER BY ProjectStatusName " + filter.Sorter.Order);
                    else if (filter.Sorter.Attribute == "projectAddress")
                        sb.Append(" ORDER BY ProjectAddress " + filter.Sorter.Order);
                    else if (filter.Sorter.Attribute == "ciawNumber")
                        sb.Append(" ORDER BY CiawNumber " + filter.Sorter.Order);
                    else
                        sb.Append(" ORDER BY " + filter.Sorter.Attribute + " " + filter.Sorter.Order);

                    // if (filter.Sorter.Attribute != null && filter.Sorter.Order.ToLower().Equals("desc") &&
                    //     filter.Sorter.Attribute != "projectManager")
                    //     sb.Append(" ORDER BY " + filter.Sorter.Attribute + " DESC");
                }

                var param = new { lang, oid = objectIdentifier, cuId = ContractingUnitSequenceId };
                IEnumerable<AllProjectAttributes> result = null;
                using (IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString))
                {
                    var q = sb.ToString();
                    result = dbConnection.Query<AllProjectAttributes>(q, param).ToList();

                    var projectPm = _iVPRepository.VPProjectPm(tenant.ConnectionString);
                    //result = dbConnection.Query<AllProjectAttributes>(q).ToList();
                    

                    if (!result.Any())
                    {
                        var mApiResponse = new ApiOkResponse(null, "noavailableproject");
                        mApiResponse.Status = false;
                        return Ok(mApiResponse);
                    }

                    return Ok(new ApiOkResponse(result));
                }
            }

            else
            {
                var sql = @"SELECT DISTINCT
                              ProjectDefinition.SequenceCode AS SequenceCode
                             ,ProjectDefinition.Name AS Name
                             ,ProjectDefinition.Id AS ProjectDefinitionId
                             ,ProjectDefinition.Title AS Title
                             ,cc.SequenceCode AS ContractingUnitId
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectClassification
                              ON ProjectClassification.ProjectId = ProjectDefinition.Id
                            LEFT OUTER JOIN CabCompany cc 
                              ON ProjectDefinition.ContractingUnitId = cc.Id
                            WHERE ProjectClassification.ProjectClassificationBuisnessUnit = @bu";

                IEnumerable<AllProjectAttributes> result = null;
                var sb = new StringBuilder(sql);

                if (filter.Title != null)
                {
                    filter.Title = filter.Title.Replace("'", "''");
                    sb.Append(" AND ProjectDefinition.Title like '%" + filter.Title + "%' ORDER BY SequenceCode ");
                }
                else
                {
                    sb.Append(" ORDER BY SequenceCode ");
                }

                using (IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString))
                {
                    if (filter.BuId != null)
                        result = dbConnection.Query<AllProjectAttributes>(sb.ToString(), new { bu = filter.BuId })
                            .ToList();

                    

                    if (result == null)
                    {
                        var mApiResponse = new ApiOkResponse(null, "noavailableproject");
                        mApiResponse.Status = false;
                        return Ok(mApiResponse);
                    }

                    return Ok(new ApiOkResponse(result));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [Authorize]
    [HttpPost("ProjectFilterProjectPlan")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ProjectFilterProjectPlan(
        [FromBody] ProjectFilterProjectPlanDto filter, [FromHeader(Name = "isMyEnv")] bool myEnv)
    {
        try
        {
            var tenant = ItenantProvider.GetTenant();

            var sql = @"SELECT DISTINCT
                              ProjectDefinition.SequenceCode AS SequenceCode
                             ,ProjectDefinition.Name AS Name
                             ,ProjectDefinition.Id AS ProjectDefinitionId
                             ,ProjectDefinition.Title AS Title
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectClassification
                              ON ProjectClassification.ProjectId = ProjectDefinition.Id
                            WHERE ProjectClassification.ProjectClassificationBuisnessUnit = @bu";

            IEnumerable<AllProjectAttributes> result = null;
            var sb = new StringBuilder(sql);

            if (filter.Title != null)
                sb.Append(" AND ProjectDefinition.Title like '%" + filter.Title + "%' ORDER BY SequenceCode ");
            else
                sb.Append(" ORDER BY SequenceCode ");
            using (IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString))
            {
                if (filter.BuId != null)
                    result = dbConnection.Query<AllProjectAttributes>(sb.ToString(), new { bu = filter.BuId })
                        .ToList();

                

                if (result == null)
                {
                    var mApiResponse = new ApiOkResponse(null, "noavailableproject");
                    mApiResponse.Status = false;
                    return Ok(mApiResponse);
                }

                return Ok(new ApiOkResponse(result));
            }
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
            throw;
        }
    }


    [Authorize]
    [HttpGet("GetContractingUnitsByUser")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<ContractingUnitDto>> GetContractingUnitsByUser()
    {
        var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
        var user = UPrinceCustomerContext.ApplicationUser
            .FirstOrDefault(u => u.OId == objectIdentifier);

        var tenant = ItenantProvider.GetTenant();
        var sql = @"SELECT
                              CabCompany.Id AS Id ,CabCompany.Name AS Name  
                              ,CabCompany.SequenceCode AS SequenceCode  ,CONCAT(CabCompany.SequenceCode
                              , ' - ', CabCompany.Name) AS Title
                            FROM dbo.CabCompany
                            LEFT OUTER JOIN dbo.ContractingUnitUserRole
                              ON CabCompany.Id = ContractingUnitUserRole.CabCompanyId
                            INNER JOIN dbo.UserRole
                              ON ContractingUnitUserRole.UserRoleId = UserRole.Id
                            INNER JOIN dbo.ApplicationUser
                              ON UserRole.ApplicationUserId = ApplicationUser.Id
                            WHERE ApplicationUser.OId = '" + user.OId + "'";

        IEnumerable<ContractingUnitDto> result = null;
        using IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString);
        result = dbConnection.Query<ContractingUnitDto>(sql).ToList();
        
        return result;
    }

    [Authorize]
    [HttpGet("GetUserRoleByUser")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<RoleReadDto> GetUserRoleByUser()
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var tenant = ItenantProvider.GetTenant();
            var sql = @"SELECT
                              Role.Id AS Id
                             ,Role.RoleName AS Role
                             ,Role.TenantId AS TenantId
                            FROM dbo.Role
                            LEFT OUTER JOIN dbo.UserRole
                              ON Role.Id = UserRole.RoleId
                            WHERE UserRole.ApplicationUserOid = '" + user.OId + "'";

            IEnumerable<RoleReadDto> result = null;
            using (IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString))
            {
                result = dbConnection.Query<RoleReadDto>(sql).ToList();
                

                return result.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [Authorize]
    [HttpGet("GetProjectsAssignedToUser")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IEnumerable<ProjectAssignedToUserDto>> GetProjectsAssignedToUser()
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier)
                .FirstOrDefault();
            var tenant = ItenantProvider.GetTenant();

            //string sql = @"SELECT
            //                ProjectDefinition.SequenceCode AS ProjectDefinitionSequenceCode
            //                ,CabCompany.SequenceCode AS CabCompanySequenceCode
            //                FROM dbo.ProjectTeamRole
            //                INNER JOIN dbo.ProjectTeam
            //                ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
            //                INNER JOIN dbo.CabPersonCompany
            //                ON CabPersonCompany.PersonId = ProjectTeamRole.CabPersonId
            //                INNER JOIN dbo.ProjectDefinition
            //                ON ProjectTeam.ProjectId = ProjectDefinition.Id
            //                INNER JOIN dbo.CabCompany
            //                ON ProjectTeam.ContractingUnitId = CabCompany.Id
            //                WHERE CabPersonCompany.Oid = '" + user.OId + "'";

            var sql = @"SELECT
                                ProjectDefinition.SequenceCode AS ProjectDefinitionSequenceCode
                                ,CabCompany.SequenceCode AS CabCompanySequenceCode
                                FROM dbo.ProjectTeamRole
                                INNER JOIN dbo.ProjectTeam
                                ON ProjectTeamRole.ProjectTeamId = ProjectTeam.Id
                                INNER JOIN dbo.CabPersonCompany
                                ON CabPersonCompany.PersonId = ProjectTeamRole.CabPersonId
                                INNER JOIN dbo.ProjectDefinition
                                ON ProjectTeam.ProjectId = ProjectDefinition.Id
                                INNER JOIN dbo.CabCompany
                                ON ProjectTeam.ContractingUnitId = CabCompany.Id
                                ";

            IEnumerable<ProjectAssignedToUserDto> result = null;
            using (IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString))
            {
                result = dbConnection.Query<ProjectAssignedToUserDto>(sql).ToList();
                

                return result;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [Authorize]
    [HttpGet("GetUserByEmail/{email}")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<string> GetUserByEmail(string email)
    {
        try
        {
            //var objectIdentifier = ContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
            //objectIdentifier
            //var token = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //token = objectIdentifier.Result;
            //token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ilg1ZVhrNHh5b2pORnVtMWtsMll0djhkbE5QNC1jNTdkTzZRR1RWQndhTmsifQ.eyJleHAiOjE2MDUxMTI1NTIsIm5iZiI6MTYwNTAyNjE1MiwidmVyIjoiMS4wIiwiaXNzIjoiaHR0cHM6Ly91cHJpbmNldXNlcm1hbmFnZW1lbnRwcm9kLmIyY2xvZ2luLmNvbS83ODkyYzA4YS1mZmNkLTRhNjEtYTk5Ni03Mjg1NGI2NTdkNzQvdjIuMC8iLCJzdWIiOiJjNDY3ZDEzNy02NTg1LTQ1ZDMtYTZmYy04NTlmNzgwNGIzZDMiLCJhdWQiOiJlZDg2M2UzZi01MGI1LTRkNDMtYWQ3YS00Y2U0NzIzYWQzNzMiLCJub25jZSI6IjBmNDdiOGRkLWEzZjctNGIwNS1hY2ZlLTA3YjM5NTIyYmFjMCIsImlhdCI6MTYwNTAyNjE1MiwiYXV0aF90aW1lIjoxNjA1MDI2MTUyLCJvaWQiOiJjNDY3ZDEzNy02NTg1LTQ1ZDMtYTZmYy04NTlmNzgwNGIzZDMiLCJjaXR5IjoiUmF0aG1hbGFuYSIsImNvdW50cnkiOiJTcmkgTGFua2EiLCJnaXZlbl9uYW1lIjoic2hhbnVrYSIsImZhbWlseV9uYW1lIjoiZ2F5YXNoYW4iLCJleHRlbnNpb25fQ29tcGFueSI6Ik1pY2tpZXNvZnQiLCJuYW1lIjoidW5rbm93biIsImVtYWlscyI6WyJzaGFudWthZ2F5YXNoYW5AZ21haWwuY29tIl0sInRmcCI6IkIyQ18xX1dlYl92NF9zaWdudXAifQ.fe5r-bKWhtarIb6VcIq7JzkC_oZ0ja1l7oVY0V8zVJB8_l_DE3vm1DoeaeNR7GHqiJOBQBWQXcARprrmtt_IoYrTjfiyJt-52COVXetbZhufMJsJyoMerDKBKu-TsPCrKIOuHcesWfUrjMo31dpHCWTDGTsQ8PSR_EIRV_EHJRkgXKC3qi8-Mrolif2Gv7m4URPDRJPM0nR-FvrNBEnJ57pn5k1qavn4GoNPnElrc-YPm0_9RCUxPGDbjqSFa6HwvpcWKulSi0GLlV8A6j--0vHL9mz5B8BS7O_III3wvAi_cbdkdbNDvQol_11U2CxeSxpLrGPrp-DVDRMbqooUpg";
            //var user = await UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var tenant = ItenantProvider.GetTenant();
            var details = AzureInvitation.GetUserByEmail(email, tenant.CatelogConnectionString);
            return details.Result;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    [HttpGet("GenerateToken")]
    public string GenerateToken()
    {
        var connectionString = ItenantProvider.GetTenant().CatelogConnectionString;
        var query = @"SELECT
                            UPrinceCustomerTenantsInfo.ClientId AS ClientId
                            ,UPrinceCustomerTenantsInfo.ClientSecretKey AS ClientSecretKey
                            ,UPrinceCustomerTenantsInfo.TenantId AS TenantId
                            FROM dbo.UPrinceCustomerTenantsInfo
                            WHERE UPrinceCustomerTenantsInfo.CatelogConnectionString = @connectionString";

        var parameters = new { connectionString = ItenantProvider.GetTenant().CatelogConnectionString };
        InvitationConfigData invitationConfigData = null;
        using (var dbConnection = new SqlConnection(connectionString))
        {
            invitationConfigData = dbConnection.Query<InvitationConfigData>(query, parameters)
                .FirstOrDefault();
            
        }

        var audienceURL = "https://graph.microsoft.com/.default";
        var TokenUrl = "https://login.microsoftonline.com/" + invitationConfigData.TenantId + "/oauth2/v2.0/token";

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.CacheControl] = "no-cache";
        webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        webClient.Headers[HttpRequestHeader.Cookie] =
            "flight-uxoptin=true; stsservicecookie=ests; x-ms-gateway-slice=productionb; stsservicecookie=ests";
        var para = "client_id=" + invitationConfigData.ClientId +
                   "&grant_type=client_credentials&client_secret=" + invitationConfigData.ClientSecretKey +
                   "&scope=" + audienceURL;
        var response = webClient.UploadString(TokenUrl, "POST", para);
        dynamic jsonObj = JsonConvert.DeserializeObject(response);
        string token = jsonObj.access_token;
        return token;
    }


    [HttpGet("GenerateTokenWithGraphClient")]
    public async Task<string> GenerateTokenWithGraphClient()
    {
        try
        {
            var authentication = new
            {
                Authority =
                    "https://uprinceusermanagementprod.b2clogin.com/7892c08a-ffcd-4a61-a996-72854b657d74/B2C_1_Web_v4_signup",
                Directory = "7892c08a-ffcd-4a61-a996-72854b657d74", /* tenant id */
                Application = "93049a78-e107-4ed4-9ae8-c1ce48f51179", /* client id */
                ClientSecret = "O.-~2iFz1v.zu2EcCj35CpLZmO3U.L8yzL"
            };

            var app = ConfidentialClientApplicationBuilder.Create(authentication.Application)
                .WithClientSecret(authentication.ClientSecret)
                .WithAuthority(AzureCloudInstance.AzurePublic, authentication.Directory)
                .Build();

            //var scopes = new[] { "https://graph.microsoft.com/.default" };
            //var scopes = new[] { "user.read", "files.read.all", "Files.ReadWrite.All" };

            // var authenticationResult = await app.AcquireTokenForClient(scopes)
            //     .ExecuteAsync();

            // var graphServiceClient = new GraphServiceClient(
            //     new DelegateAuthenticationProvider(x =>
            //     {
            //         x.Headers.Authorization = new AuthenticationHeaderValue(
            //             "Bearer", authenticationResult.AccessToken);
            //
            //         return Task.FromResult(0);
            //     }));

            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var tenantId = "7892c08a-ffcd-4a61-a996-72854b657d74";
            var clientId = "93049a78-e107-4ed4-9ae8-c1ce48f51179";
            var clientSecret = "O.-~2iFz1v.zu2EcCj35CpLZmO3U.L8yzL";

            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret);
            var graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);


            //var search = await graphServiceClient.Me.Drive.Root
            //    .Search("finance")
            //    .Request(queryOptions)
            //    .GetAsync();

            //return search.ToString();
            var users = await graphServiceClient.Users.GetAsync();
            var userByEmail = await graphServiceClient.Users["sampath@mickiesoft.com"].GetAsync();
            var userString = "";
            foreach (var user in users.Value)
                userString += user.Mail + ", ";
            return userString;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }


    [HttpGet("GetTranslations/{languageCode}/translations.json")]
    [AllowAnonymous]
    public async Task<Dictionary<string, string>> GetJson(string languageCode)
    {
        var param = new { lang = languageCode };
        //var tenant = ItenantProvider.GetTenant();
        // var sql =
        //     @"SELECT WebTranslation.[Key] ,WebTranslation.Value FROM dbo.WebTranslation INNER JOIN dbo.Language ON WebTranslation.LanguageId = Language.Id WHERE Language.Code = @lang";


        //using IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString);
        return ItenantProvider.orgSqlConnection()
            .QueryAsync<WebTranslation>("select * from GetTranslationsView WHERE code = @lang", param).Result
            .ToDictionary(
                row => string.Concat(row.Key[..1].ToLower(), row.Key.AsSpan(1)),
                row => row.Value);
    }

    [HttpGet("GetTimeClockHistoryList")]
    public IEnumerable<string> GetTimeClockHistoryList([FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
           // var connectionString = ConnectionString.MapConnectionString(CU, null, ItenantProvider);
            IEnumerable<string> dataId;
            //DapperPlusManager.Entity<Data.ShiftDto>().Table("Shifts")

            //                         .Identity(x => x.Id);

            DapperPlusManager.Entity<ShiftDto>()
                .Table("Shifts")
                .Map(m => m.Id, nameof(ShiftDto.Id))
                .Map(m => m.UserId, nameof(ShiftDto.UserId))
                .Map(m => m.EndDateTime, nameof(ShiftDto.EndDateTime))
                .Map(m => m.StartDateTime, nameof(ShiftDto.StartDateTime))
                .Map(m => m.Status, nameof(ShiftDto.Status))
                .Map(m => m.WorkflowStateId, nameof(ShiftDto.WorkflowStateId))
                .Key(x => x.Id);

            //Id
            //                 ,UserId
            //                 ,
            //                 ,EndDateTime
            //                 ,
            //                 ,
            using (var connection = new SqlConnection(
                       "Server=tcp:princev4einstein.database.windows.net,1433;Initial Catalog=COM0001;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                dataId = connection.Query<string>(
                    @"SELECT TimeClockHistory.Id FROM dbo.TimeClockHistory WHERE TimeClockHistory.HistoryLog = 'OptimizeDatabase'");
                foreach (var id in dataId)
                {
                    var shiftJson = connection
                        .Query<string>(
                            @"SELECT TimeClockHistory.DataJson FROM dbo.TimeClockHistory WHERE TimeClockHistory.Id = @Id",
                            new { Id = id }).FirstOrDefault();
                    IEnumerable<ShiftDto>
                        shift = JsonConvert.DeserializeObject<List<ShiftDto>>(shiftJson);
                    if (shift != null)
                        if (shift.Count() > 0)
                            connection.BulkMerge(shift);
                    //var shiftInsert = @"
                    //                INSERT INTO dbo.Shifts
                    //                (
                    //                  Id
                    //                 ,UserId
                    //                 ,StartDateTime
                    //                 ,EndDateTime
                    //                 ,Status
                    //                 ,WorkflowStateId
                    //                )
                    //                VALUES
                    //                (
                    //                  @Id
                    //                 ,@UserId
                    //                 ,@StartDateTime
                    //                 ,@EndDateTime
                    //                 ,@Status
                    //                 ,@WorkflowStateId
                    //                );";

                    //foreach (var s in shift)
                    //{
                    //    var param = new { Id = s.Id, UserId = s.UserId, StartDateTime = s.StartDateTime, EndDateTime = s.EndDateTime, Status = s.Status, WorkflowStateId = s.WorkflowStateId };
                    //    try
                    //    {
                    //        connection.ExecuteAsync(shiftInsert, param);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);

                    //    }


                    //}
                }
            }

            return dataId;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [AllowAnonymous]
    [HttpGet("GetPmolTeams")]
    public async Task<ActionResult> GetPmolTeams([FromHeader(Name = "AuthToken")] string AuthToken)
    {
        try
        {
            var _vpParameter = new VPParameter();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _vpParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _vpParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();


            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                _vpParameter.Lang = lang;
                _vpParameter.ContextAccessor = ContextAccessor;
                _vpParameter.TenantProvider = _iTenantProvider;
                var shortcutPaneData = await _iVPRepository.TeamsForPowerBi(_vpParameter);

                return Ok(new ApiOkResponse(shortcutPaneData));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("GetMissingCPC")]
    public async Task<ActionResult> GetMissingCPC([FromHeader(Name = "AuthToken")] string AuthToken)
    {
        try
        {
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _CpcParameters.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _CpcParameters.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();


            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                _CpcParameters.Lang = lang;
                _CpcParameters.ContextAccessor = ContextAccessor;
                _CpcParameters.TenantProvider = _iTenantProvider;
                var shortcutPaneData = await _iCoporateProductCatalogRepository.MissingCPC(_CpcParameters);

                return Ok(new ApiOkResponse(shortcutPaneData));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("GetContractorById/{Id}")]
    public async Task<ActionResult> GetContractorById(string Id,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _contractorParameter.ContractingUnitSequenceId = CU;
            _contractorParameter.ProjectSequenceId = Project;
            _contractorParameter.Lang = lang;
            _contractorParameter.Id = Id;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;

            var s = await _iContractorReopsitory.GetContractorByIdForMail(_contractorParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("GetContractorByIdForSubscribMail/{Id}")]
    public async Task<ActionResult> GetContractorByIdForSubscribMail(string Id,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var lang = langCode(langX);

            _contractorParameter.ContractingUnitSequenceId = CU;
            _contractorParameter.ProjectSequenceId = Project;
            _contractorParameter.Lang = lang;
            _contractorParameter.Id = Id;
            _contractorParameter.ContextAccessor = ContextAccessor;
            _contractorParameter.TenantProvider = ItenantProvider;

            var s = await _iContractorReopsitory.GetContractorByIdForSubscribMail(_contractorParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("UpdateProjectPm")]
    public async Task<ActionResult> UpdateProjectPm(string Id,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var _pmolParameter = new PmolParameter();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _iTenantProvider;

            var s = await _iPmolRepository.UpdateProjectPm(_pmolParameter);
            return Ok(new ApiOkResponse(s));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("UpdateProjectCustomer")]
    public async Task<ActionResult> UpdateProjectCustomer(string Id,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var _pmolParameter = new PmolParameter();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _pmolParameter.ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();
            _pmolParameter.ProjectSequenceId = Request.Headers["Project"].FirstOrDefault();

            _pmolParameter.Lang = lang;
            _pmolParameter.ContextAccessor = ContextAccessor;
            _pmolParameter.TenantProvider = _iTenantProvider;

            await using var connection = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString);


            var projectList =
                connection.Query<string>("SELECT SequenceCode FROM dbo.ProjectDefinition WHERE IsDeleted = 0");

            foreach (var project in projectList)
            {
                _pmolParameter.ProjectSequenceId = project;
                var customer = await _psRepository.GetCustomer(_pmolParameter);

                if (customer != null)
                    connection.ExecuteAsync(
                        "UPDATE dbo.ProjectDefinition SET CustomerId = @CustomerId WHERE SequenceCode = @SequenceCode",
                        new { CustomerId = customer.CabPersonId, SequenceCode = project });
            }

            return Ok(new ApiOkResponse(projectList));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [Authorize]
    [HttpGet("AssignAdminToProject")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AssignAdminToProject(string Id)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.FirstOrDefault().Claims
                .First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                .Value;
            // var Email = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            //     claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            // var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var tenant = ItenantProvider.GetTenant();
            objectIdentifier = "96add707-03e1-49e2-b598-e6c30b04562f";
            //if (objectIdentifier == "3259340e-47c5-4363-b891-8e773f430be6")
            if (objectIdentifier == "96add707-03e1-49e2-b598-e6c30b04562f")

            {
                var sql = @"SELECT DISTINCT
                              ProjectDefinition.SequenceCode AS SequenceCode
                             ,ProjectDefinition.Name AS Name
                             ,ProjectDefinition.Id AS ProjectDefinitionId
                             ,ProjectDefinition.ContractingUnitId AS ContractingUnitId
                             ,ProjectTemplate.Name AS ProjectTemplateName
                             ,ProjectToleranceState.Name AS ProjectToleranceStateName
                             ,ProjectType.Name AS ProjectTypeName
                             ,ProjectManagementLevel.Name AS ProjectManagementLevelName
                             ,ProjectDefinition.Title AS Title
                             ,ProjectStateLocalizedData.Label AS ProjectStatusName
                             ,CabPerson.FullName AS CustomerName
                             ,cp.FullName AS ProjectManagerName
                             ,ProjectClassificationSector.Name AS SectorName
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectTemplate
                              ON ProjectDefinition.ProjTemplateId = ProjectTemplate.TemplateId
                            LEFT OUTER JOIN dbo.ProjectToleranceState
                              ON ProjectDefinition.ProjToleranceStateId = ProjectToleranceState.ProjectToleranceStateId
                            LEFT OUTER JOIN dbo.ProjectType
                              ON ProjectDefinition.ProjTypeId = ProjectType.ProjectTypeId
                            LEFT OUTER JOIN dbo.ProjectManagementLevel
                              ON ProjectDefinition.ProjManagementLevelId = ProjectManagementLevel.ProjectManagementLevelId
                            LEFT OUTER JOIN dbo.ProjectUserRole
                              ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                            LEFT OUTER JOIN dbo.UserRole
                              ON ProjectUserRole.UsrRoleId = UserRole.Id
                            LEFT OUTER JOIN dbo.ApplicationUser
                              ON UserRole.ApplicationUserOid = ApplicationUser.OId
                            INNER JOIN dbo.CabCompany
                              ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                            LEFT OUTER JOIN dbo.ProjectScopeStatus
                              ON ProjectDefinition.ProjectScopeStatusId = ProjectScopeStatus.StatusId
                            LEFT OUTER JOIN dbo.CabPerson
                              ON ProjectDefinition.CustomerId = CabPerson.Id
                            LEFT OUTER JOIN dbo.CabPerson cp
                              ON ProjectDefinition.ProjectManagerId = cp.Id
                            LEFT OUTER JOIN dbo.ProjectStateLocalizedData
                              ON ProjectDefinition.ProjectStatus = ProjectStateLocalizedData.ProjectStateId
                            LEFT OUTER JOIN dbo.ProjectClassification
                              ON ProjectDefinition.Id = ProjectClassification.ProjectId
                            LEFT OUTER JOIN dbo.ProjectClassificationSector
                              ON ProjectClassification.ProjectClassificationSectorId = ProjectClassificationSector.TypeId
                            WHERE (ProjectTemplate.LanguageCode = @lang
                            OR ProjectDefinition.ProjTemplateId IS NULL)
                            AND ProjectToleranceState.LanguageCode = @lang
                            AND ProjectType.LanguageCode = @lang
                            AND ProjectManagementLevel.LanguageCode = @lang
                            AND (ProjectStateLocalizedData.LanguageCode = @lang
                            OR ProjectDefinition.ProjectStatus IS NULL)
                            AND (ProjectClassificationSector.LanguageCode = @lang
                            OR ProjectClassification.ProjectClassificationSectorId IS NULL)
                            AND ApplicationUser.OId = @oid
                            AND ProjectDefinition.IsDeleted = 0";


                var sb = new StringBuilder(sql);

                var param = new { lang, oid = objectIdentifier, cuId = ContractingUnitSequenceId };
                IEnumerable<AllProjectAttributes> result = null;
                using IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString);
                var q = sb.ToString();
                result = dbConnection.Query<AllProjectAttributes>(q, param).ToList();


                if (!result.Any())
                {
                    var mApiResponse = new ApiOkResponse(null, "noavailableproject");
                    mApiResponse.Status = false;
                    return Ok(mApiResponse);
                }

                var cabPersonId = dbConnection
                    .Query<string>("SELECT PersonId FROM dbo.CabPersonCompany  WHERE Oid = @Oid",
                        new { Oid = objectIdentifier })
                    .FirstOrDefault();

                var projectList = result.Select(x => x.SequenceCode).ToList();

                var excludeProjects = dbConnection.Query<ProjectDefinition>(
                    "SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0 AND SequenceCode NOT IN @Ids",
                    new { Ids = projectList });

                var existingUserRole = dbConnection
                    .Query<UserRole>("Select * from UserRole WHERE ApplicationUserOid = @Id AND RoleId = @RoleId",
                        new { Id = objectIdentifier, RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82" })
                    .FirstOrDefault();
                string userRoleId = null;
                if (existingUserRole != null)
                {
                    userRoleId = existingUserRole.Id;
                }
                else
                {
                    var userRoleSql = @"INSERT INTO dbo.UserRole
                                            (
                                              Id
                                             ,RoleId
                                             ,ApplicationUserOid
                                            )
                                            VALUES
                                            (
                                              @Id
                                             ,@RoleId
                                             ,@ApplicationUserOid
                                            )";

                    var userRole = new UserRole
                    {
                        ApplicationUserOid = objectIdentifier,
                        Id = Guid.NewGuid().ToString(),
                        RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82"
                    };

                    await dbConnection.ExecuteAsync(userRoleSql, userRole);
                    userRoleId = userRole.Id;
                }

                foreach (var project in excludeProjects)
                {
                    var insertQuery = @"INSERT INTO dbo.ProjectTeamRole
                    (
                        Id
                        ,ProjectTeamId
                        ,CabPersonId
                        ,RoleId
                        ,status
                        ,IsAccessGranted
                    )
                    VALUES
                    (
                        @Id
                        ,@ProjectTeamId
                        ,@CabPersonId
                        ,@RoleId
                        ,@status
                        ,@IsAccessGranted
                    )";

                    var isTeamExist = dbConnection
                        .Query<ProjectTeam>("Select * FROM ProjectTeam Where ProjectId = @ProjectId",
                            new { ProjectId = project.Id }).FirstOrDefault();

                    if (isTeamExist != null)
                    {
                        var param1 = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectTeamId = isTeamExist.Id,
                            CabPersonId = cabPersonId,
                            RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82", //admin
                            status = "2",
                            IsAccessGranted = true
                        };

                        await dbConnection.ExecuteAsync(insertQuery, param1);
                    }
                    else
                    {
                        var insertProjectTeam = @"INSERT INTO dbo.ProjectTeam
                                                (
                                                  Id
                                                 ,ContractingUnitId
                                                 ,ProjectId
                                                )
                                                VALUES
                                                (
                                                 @Id
                                                 ,@ContractingUnitId
                                                 ,@ProjectId
                                                )";

                        var param2 = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            project.ContractingUnitId,
                            ProjectId = project.Id
                        };

                        await dbConnection.ExecuteAsync(insertProjectTeam, param2);

                        var param1 = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectTeamId = param2.Id,
                            CabPersonId = cabPersonId,
                            RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82", //admin
                            status = "2",
                            IsAccessGranted = true
                        };

                        await dbConnection.ExecuteAsync(insertQuery, param1);
                    }


                    var roleSql = @"INSERT INTO dbo.ProjectUserRole
                                            (
                                              Id
                                             ,ProjectDefinitionId
                                             ,UsrRoleId
                                            )
                                            VALUES 
                                            (
                                             @Id
                                             ,@ProjectDefinitionId
                                             ,@UsrRoleId
                                            )";

                    var existingProjectRole = dbConnection
                        .Query<ProjectUserRole>(
                            "Select * From ProjectUserRole Where UsrRoleId = @UsrRoleId AND ProjectDefinitionId = @ProjectDefinitionId",
                            new { UsrRoleId = userRoleId, ProjectDefinitionId = project.Id })
                        .FirstOrDefault();

                    if (existingProjectRole == null)
                    {
                        var projectUserRole = new ProjectUserRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectDefinitionId = project.Id,
                            UsrRoleId = userRoleId
                        };

                        await dbConnection.ExecuteAsync(roleSql, projectUserRole);
                    }
                }

                return Ok(new ApiOkResponse(result));
            }

            {
                var mApiResponse = new ApiOkResponse(null, "noaccess");
                mApiResponse.Status = false;
                return Ok(mApiResponse);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [Authorize]
    [HttpPost("AssignMultipleAdminToProject")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AssignAdminToProject([FromBody] List<string> idList)
    {
        try
        {
            var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.FirstOrDefault().Claims
                .First(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                .Value;
            // var Email = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            //     claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            // var user = UPrinceCustomerContext.ApplicationUser.Where(u => u.OId == objectIdentifier).Include(u => u.UserRole).ThenInclude(u => u.Role).FirstOrDefault();
            var ContractingUnitSequenceId = Request.Headers["CU"].FirstOrDefault();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var tenant = ItenantProvider.GetTenant();

            foreach (var item in idList)
            {
                using IDbConnection dbConnection = new SqlConnection(tenant.ConnectionString);

                var oid = dbConnection
                    .Query<string>(
                        @"SELECT cpc.Oid FROM CabEmail ce LEFT OUTER JOIN CabPersonCompany cpc ON ce.Id = cpc.EmailId  WHERE ce.EmailAddress = @email",
                        new { email = item })
                    .FirstOrDefault();

                if (oid != null)
                {
                    objectIdentifier = oid;


                    //if (objectIdentifier == "3259340e-47c5-4363-b891-8e773f430be6")

                    var sql = @"SELECT DISTINCT
                              ProjectDefinition.SequenceCode AS SequenceCode
                             ,ProjectDefinition.Name AS Name
                             ,ProjectDefinition.Id AS ProjectDefinitionId
                             ,ProjectDefinition.ContractingUnitId AS ContractingUnitId
                             ,ProjectTemplate.Name AS ProjectTemplateName
                             ,ProjectToleranceState.Name AS ProjectToleranceStateName
                             ,ProjectType.Name AS ProjectTypeName
                             ,ProjectManagementLevel.Name AS ProjectManagementLevelName
                             ,ProjectDefinition.Title AS Title
                             ,ProjectStateLocalizedData.Label AS ProjectStatusName
                             ,CabPerson.FullName AS CustomerName
                             ,cp.FullName AS ProjectManagerName
                             ,ProjectClassificationSector.Name AS SectorName
                            FROM dbo.ProjectDefinition
                            LEFT OUTER JOIN dbo.ProjectTemplate
                              ON ProjectDefinition.ProjTemplateId = ProjectTemplate.TemplateId
                            LEFT OUTER JOIN dbo.ProjectToleranceState
                              ON ProjectDefinition.ProjToleranceStateId = ProjectToleranceState.ProjectToleranceStateId
                            LEFT OUTER JOIN dbo.ProjectType
                              ON ProjectDefinition.ProjTypeId = ProjectType.ProjectTypeId
                            LEFT OUTER JOIN dbo.ProjectManagementLevel
                              ON ProjectDefinition.ProjManagementLevelId = ProjectManagementLevel.ProjectManagementLevelId
                            LEFT OUTER JOIN dbo.ProjectUserRole
                              ON ProjectDefinition.Id = ProjectUserRole.ProjectDefinitionId
                            LEFT OUTER JOIN dbo.UserRole
                              ON ProjectUserRole.UsrRoleId = UserRole.Id
                            LEFT OUTER JOIN dbo.ApplicationUser
                              ON UserRole.ApplicationUserOid = ApplicationUser.OId
                            INNER JOIN dbo.CabCompany
                              ON ProjectDefinition.ContractingUnitId = CabCompany.Id
                            LEFT OUTER JOIN dbo.ProjectScopeStatus
                              ON ProjectDefinition.ProjectScopeStatusId = ProjectScopeStatus.StatusId
                            LEFT OUTER JOIN dbo.CabPerson
                              ON ProjectDefinition.CustomerId = CabPerson.Id
                            LEFT OUTER JOIN dbo.CabPerson cp
                              ON ProjectDefinition.ProjectManagerId = cp.Id
                            LEFT OUTER JOIN dbo.ProjectStateLocalizedData
                              ON ProjectDefinition.ProjectStatus = ProjectStateLocalizedData.ProjectStateId
                            LEFT OUTER JOIN dbo.ProjectClassification
                              ON ProjectDefinition.Id = ProjectClassification.ProjectId
                            LEFT OUTER JOIN dbo.ProjectClassificationSector
                              ON ProjectClassification.ProjectClassificationSectorId = ProjectClassificationSector.TypeId
                            WHERE (ProjectTemplate.LanguageCode = @lang
                            OR ProjectDefinition.ProjTemplateId IS NULL)
                            AND ProjectToleranceState.LanguageCode = @lang
                            AND ProjectType.LanguageCode = @lang
                            AND ProjectManagementLevel.LanguageCode = @lang
                            AND (ProjectStateLocalizedData.LanguageCode = @lang
                            OR ProjectDefinition.ProjectStatus IS NULL)
                            AND (ProjectClassificationSector.LanguageCode = @lang
                            OR ProjectClassification.ProjectClassificationSectorId IS NULL)
                            AND ApplicationUser.OId = @oid
                            AND ProjectDefinition.IsDeleted = 0";


                    var sb = new StringBuilder(sql);

                    var param = new { lang, oid = objectIdentifier, cuId = ContractingUnitSequenceId };
                    IEnumerable<AllProjectAttributes> result = null;
                    var q = sb.ToString();
                    result = dbConnection.Query<AllProjectAttributes>(q, param).ToList();


                    // if (!result.Any())
                    // {
                    //     var mApiResponse = new ApiOkResponse(null, "noavailableproject");
                    //     mApiResponse.Status = false;
                    //     return Ok(mApiResponse);
                    // }

                    var cabPersonId = dbConnection
                        .Query<string>("SELECT PersonId FROM dbo.CabPersonCompany  WHERE Oid = @Oid",
                            new { Oid = objectIdentifier })
                        .FirstOrDefault();

                    var projectList = result.Select(x => x.SequenceCode).ToList();

                    var excludeProjects = dbConnection.Query<ProjectDefinition>(
                        "SELECT * FROM dbo.ProjectDefinition WHERE IsDeleted = 0 AND SequenceCode NOT IN @Ids",
                        new { Ids = projectList });

                    var existingUserRole = dbConnection
                        .Query<UserRole>("Select * from UserRole WHERE ApplicationUserOid = @Id AND RoleId = @RoleId",
                            new { Id = objectIdentifier, RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82" })
                        .FirstOrDefault();
                    string userRoleId = null;
                    if (existingUserRole != null)
                    {
                        userRoleId = existingUserRole.Id;
                    }
                    else
                    {
                        var userRoleSql = @"INSERT INTO dbo.UserRole
                                            (
                                              Id
                                             ,RoleId
                                             ,ApplicationUserOid
                                            )
                                            VALUES
                                            (
                                              @Id
                                             ,@RoleId
                                             ,@ApplicationUserOid
                                            )";

                        var userRole = new UserRole
                        {
                            ApplicationUserOid = objectIdentifier,
                            Id = Guid.NewGuid().ToString(),
                            RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82"
                        };

                        await dbConnection.ExecuteAsync(userRoleSql, userRole);
                        userRoleId = userRole.Id;
                    }

                    foreach (var project in excludeProjects)
                    {
                        var insertQuery = @"INSERT INTO dbo.ProjectTeamRole
                    (
                        Id
                        ,ProjectTeamId
                        ,CabPersonId
                        ,RoleId
                        ,status
                        ,IsAccessGranted
                    )
                    VALUES
                    (
                        @Id
                        ,@ProjectTeamId
                        ,@CabPersonId
                        ,@RoleId
                        ,@status
                        ,@IsAccessGranted
                    )";

                        var isTeamExist = dbConnection
                            .Query<ProjectTeam>("Select * FROM ProjectTeam Where ProjectId = @ProjectId",
                                new { ProjectId = project.Id }).FirstOrDefault();

                        if (isTeamExist != null)
                        {
                            var param1 = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectTeamId = isTeamExist.Id,
                                CabPersonId = cabPersonId,
                                RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82", //admin
                                status = "2",
                                IsAccessGranted = true
                            };

                            await dbConnection.ExecuteAsync(insertQuery, param1);
                        }
                        else
                        {
                            var insertProjectTeam = @"INSERT INTO dbo.ProjectTeam
                                                (
                                                  Id
                                                 ,ContractingUnitId
                                                 ,ProjectId
                                                )
                                                VALUES
                                                (
                                                 @Id
                                                 ,@ContractingUnitId
                                                 ,@ProjectId
                                                )";

                            var param2 = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                project.ContractingUnitId,
                                ProjectId = project.Id
                            };

                            await dbConnection.ExecuteAsync(insertProjectTeam, param2);

                            var param1 = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectTeamId = param2.Id,
                                CabPersonId = cabPersonId,
                                RoleId = "0e06111a-a513-45e0-a431-170dbd4b0d82", //admin
                                status = "2",
                                IsAccessGranted = true
                            };

                            await dbConnection.ExecuteAsync(insertQuery, param1);
                        }


                        var roleSql = @"INSERT INTO dbo.ProjectUserRole
                                            (
                                              Id
                                             ,ProjectDefinitionId
                                             ,UsrRoleId
                                            )
                                            VALUES 
                                            (
                                             @Id
                                             ,@ProjectDefinitionId
                                             ,@UsrRoleId
                                            )";

                        var existingProjectRole = dbConnection
                            .Query<ProjectUserRole>(
                                "Select * From ProjectUserRole Where UsrRoleId = @UsrRoleId AND ProjectDefinitionId = @ProjectDefinitionId",
                                new { UsrRoleId = userRoleId, ProjectDefinitionId = project.Id })
                            .FirstOrDefault();

                        if (existingProjectRole == null)
                        {
                            var projectUserRole = new ProjectUserRole
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectDefinitionId = project.Id,
                                UsrRoleId = userRoleId
                            };

                            await dbConnection.ExecuteAsync(roleSql, projectUserRole);
                        }
                    }
                }
            }

            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [AllowAnonymous]
    [HttpGet("TestDb/{Id}")]
    public async Task<ActionResult> TestDb(string Id, string env,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            if (Id == "mickiesoft")
            {
                var result = new List<Databases>();
                //uprincev4uatdb
                //uprincev4einstein
                // var env = "uprincev4einstein";
                var exceptionLst = new List<DatabasesEx>();
                using (var connection = new SqlConnection("Server=tcp:" + env +
                                                          ".database.windows.net,1433;Initial Catalog=master;Persist Security Info=False;User ID=uprincedbuser;Password=UPrince2017!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    result = connection
                        .Query<Databases>(
                            @"select [name] as DatabaseName from sys.databases WHERE name NOT IN('master', 'MsalTokenCacheDatabase', 'UPrinceV4EinsteinCatelog', 'UPrinceV4UATCatelog') order by name")
                        .ToList();

                    foreach (var item in result)
                        await connection.ExecuteAsync("Drop Database @db", new { db = item.DatabaseName });


                    return Ok(new ApiOkResponse("ok"));
                }
            }

            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpPost("AddCpcTrucks")]
    public async Task<ActionResult> AddCpcTrucks([FromHeader(Name = "AuthToken")] string AuthToken,
        [FromBody] AddCpcTruckDto AddCpcTruckDto)
    {
        try
        {
            var _cpcParameter = new CpcParameters();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            _cpcParameter.ContractingUnitSequenceId = "COM-0001";
            //_pmolParameter.AuthToken = Request.Headers["AuthToken"].FirstOrDefault();


            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                AddCpcTruckDto.ResourceFamilyId ??= "2210e768-human-kknk-truck-ee367a82ad17";

                _cpcParameter.Lang = lang;
                _cpcParameter.ContextAccessor = ContextAccessor;
                _cpcParameter.TenantProvider = _iTenantProvider;
                _cpcParameter.Context = UPrinceCustomerContext;
                _cpcParameter.isCopy = false;


                var cpcDto = new CoperateProductCatalogCreateDto
                {
                    CpcBasicUnitOfMeasureId = "kljfjk479-org2-mixer-2WERT-nl0dbe6a5w16",
                    Id = Guid.NewGuid().ToString(),
                    ResourceFamilyId = AddCpcTruckDto.ResourceFamilyId,
                    ResourceTypeId = "c46c3a26-39a5-42cc-n9wn-89655304eh6",
                    ResourceTitle = AddCpcTruckDto.Name,
                    Size = AddCpcTruckDto.Size,
                    Status = 1,
                    SerialNumber = AddCpcTruckDto.SerialNumber
                };

                _cpcParameter.CpcDto = cpcDto;
                var shortcutPaneData =
                    await _iCoporateProductCatalogRepository.CreateCoporateProductCatalog(_cpcParameter,
                        ContextAccessor);
                var thParam = new ThAutomationParameter
                {
                    ContractingUnitSequenceId = "COM-0001",
                    Lang = lang,
                    ContextAccessor = ContextAccessor,
                    TenantProvider = _iTenantProvider,
                    uPrinceCustomerContext = UPrinceCustomerContext,
                    iStockRepository = _iStockRepository,
                    ThStockCreate = new ThStockCreate()
                    {
                        CPCId = shortcutPaneData.Id
                    }
                };

                var stock = await _iThAutomationRepository.ThStockCreate(thParam);
                shortcutPaneData.StockId = stock;

                return Ok(new ApiOkResponse(shortcutPaneData));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [AllowAnonymous]
    [HttpPost("CreateSbProject")]
    public async Task<ActionResult> CreateThProject([FromHeader(Name = "AuthToken")] string AuthToken,
        [FromBody] CreateThProjectDto CreateThProjectDto)
    {
        try
        {
            await using var dbConnection = new SqlConnection(ItenantProvider.GetTenant().ConnectionString);

            var _pbsDto = new ProjectDefinitionCreateDto();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());
            //_pmolParameter.AuthToken = Request.Headers["AuthToken"].FirstOrDefault();

            // var objectIdentifier = ContextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
            // //     claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            // var user = UPrinceCustomerContext.ApplicationUser
            //     .FirstOrDefault(u => u.OId == "objectIdentifier");
            var user = new ApplicationUser
            {
                Email = "gg@gmail.com"
            };
            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                if (CreateThProjectDto.CustomerId != null && CreateThProjectDto.OrganizationId != null)
                {
                    var isOrganizationExist = dbConnection
                        .Query<ThCustomerOrganizations>(
                            "Select * From ThCustomerOrganizations Where OrganizationId = @OrganizationId",
                            new { CreateThProjectDto.OrganizationId }).FirstOrDefault();
                    string projectId;

                    if (isOrganizationExist != null)
                    {
                        projectId = isOrganizationExist.ProjectId;


                        var insertSql =
                            @"INSERT INTO dbo.ThCustomerOrganizations ( Id ,CustomerId ,OrganizationId ,ProjectId, PoId ) VALUES ( @Id ,@CustomerId ,@OrganizationId ,@ProjectId,@PoId )";

                        var insertParams = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreateThProjectDto.CustomerId,
                            CreateThProjectDto.OrganizationId,
                            ProjectId = projectId,
                            CreateThProjectDto.PoId
                        };
                        await dbConnection.ExecuteAsync(insertSql, insertParams);
                    }
                    else
                    {
                        _pbsDto.CreateThProjectDto = CreateThProjectDto;
                        _pbsDto.Name = CreateThProjectDto.ProjectName + " - " + CreateThProjectDto.OrganizationName;
                        _pbsDto.ProjectManagementLevelId = "4fb90b13-93e5-457c-908e-fea699ad23f1";
                        _pbsDto.ProjectScopeStatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1";
                        _pbsDto.ProjectToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18";
                        _pbsDto.ProjectTypeId = "a77e863a-d329-4f5b-ba1b-7dc3bf09b0b8";
                        _pbsDto.ProjectStatus = "65f2f0fd-ea7a-440e-9fd4-346628ef1299";
                        _pbsDto.Description = "";

                        _pbsDto.Location = new MapLocation
                        {
                            Position = new Position
                            {
                                Lat = CreateThProjectDto.Latitude,
                                Lon = CreateThProjectDto.Longitude
                            }
                        };

                        _pbsDto.ProjectTime = new ProjectTimeCreateDto
                        {
                            StartDate = CreateThProjectDto.StartDateTime
                        };
                        _pbsDto.ProjectTeam = new ProjectTeamCreateDto
                        {
                            ContractingUnitId = "e0386eac-c9a0-4f93-8baf-d24948bedda9",
                            TeamRoleList = new List<ProjectTeamRoleCreateDto>()
                        };
                        _pbsDto._iConfiguration = _configuration;

                        var project = await _iProjectDefinitionRepository.CreateProjectForTh(UPrinceCustomerContext,
                            _pbsDto, _iProjectTimeRepository, _iProjectFinanceRepository, _iProjectTeamRepository,
                            ItenantProvider, user, _IPbsRepository);
                        projectId = project.ProjectId;

                        var insertSql =
                            @"INSERT INTO dbo.ThCustomerOrganizations ( Id ,CustomerId ,OrganizationId ,ProjectId, PoId ) VALUES ( @Id ,@CustomerId ,@OrganizationId ,@ProjectId,@PoId )";

                        var inserParams = new
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreateThProjectDto.CustomerId,
                            CreateThProjectDto.OrganizationId,
                            ProjectId = projectId,
                            CreateThProjectDto.PoId
                        };
                        await dbConnection.ExecuteAsync(insertSql, inserParams);
                    }

                    var projectData = dbConnection
                        .Query<ProjectDefinition>("Select * from ProjectDefinition Where Id = @Id",
                            new { Id = projectId }).FirstOrDefault();

                    await using var connection = new SqlConnection(projectData?.ProjectConnectionString);

                    var pbsParameters = new PbsParameters
                    {
                        ProjectSequenceId = projectData?.SequenceCode,
                        ContractingUnitSequenceId = "COM-0001",
                        TenantProvider = ItenantProvider,
                        ChangedUser = user
                    };

                    var dtoNew = new PbsProductCreateDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = CreateThProjectDto.ProjectName,
                        PbsType = "regular",
                        PbsProductItemTypeId = "48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
                        Scope = "0",
                        PbsProductStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                        PbsToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18",
                        Contract = CreateThProjectDto.CustomerName,
                        StartDate = CreateThProjectDto.StartDateTime?.Date
                    };

                    if (CreateThProjectDto.Velocity != null && CreateThProjectDto.Capacity != null)
                    {
                        var duration = Convert.ToDouble(CreateThProjectDto.Capacity) /
                            Convert.ToDouble(CreateThProjectDto.Velocity) * 60;

                        CreateThProjectDto.EndDateTime = CreateThProjectDto.StartDateTime?.AddMinutes(duration);
                    }

                    dtoNew.EndDate = CreateThProjectDto.EndDateTime;

                    pbsParameters.PbsDto = dtoNew;

                    var pbs = await _IPbsRepository.CreatePbs(pbsParameters).ConfigureAwait(false);


                    var dynamicAttribute1 = new PbsDynamicAttributes
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = dtoNew.Id,
                        Key = "Velocity",
                        Value = CreateThProjectDto.Velocity
                    };

                    var dynamicAttribute3 = new PbsDynamicAttributes
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = dtoNew.Id,
                        Key = "Capacity",
                        Value = CreateThProjectDto.Capacity
                    };
                    var dynamicAttribute4 = new PbsDynamicAttributes
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = dtoNew.Id,
                        Key = "StartTime",
                        Value = CreateThProjectDto.StartDateTime?.ToString("HH:mm")
                    };
                    var dynamicAttribute5 = new PbsDynamicAttributes
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = dtoNew.Id,
                        Key = "EndTime",
                        Value = CreateThProjectDto.EndDateTime?.ToString("HH:mm")
                    };

                    var dynamicInsert =
                        "INSERT INTO dbo.PbsDynamicAttributes (Id, ProductId, [Key], Value) VALUES (@Id, @ProductId, @Key, @Value)";

                    await connection.ExecuteAsync(dynamicInsert, dynamicAttribute1);
                    await connection.ExecuteAsync(dynamicInsert, dynamicAttribute3);
                    await connection.ExecuteAsync(dynamicInsert, dynamicAttribute4);
                    await connection.ExecuteAsync(dynamicInsert, dynamicAttribute5);


                    _borParameter.ContractingUnitSequenceId = "COM-0001";
                    _borParameter.ProjectSequenceId = projectData?.SequenceCode;
                    _borParameter.Lang = lang;
                    _borParameter.ContextAccessor = ContextAccessor;
                    _borParameter.TenantProvider = ItenantProvider;
                    _borParameter.IBorResourceRepository = _iBorResourceRepository;
                    _borParameter.ICoporateProductCatalogRepository = _iCoporateProductCatalogRepository;
                    //_CpcParameters.Oid = userId;
                    _borParameter.CpcParameters = _CpcParameters;

                    var borDto = new BorDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        BorStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                        BorTypeId = "88282458-0b40-poa3-b0f9-c2e40344c888",
                        Name = CreateThProjectDto.ProjectName,
                        StartDate = CreateThProjectDto.StartDateTime,
                        BorResources = new BorResource(),
                        IsTh = true,
                        Product = new BorProductDto
                        {
                            Id = pbs.Id,
                            ProductId = pbs.ProductId
                        }
                    };
                    _borParameter.BorDto = borDto;

                    var borItemId = await _iBorRepository.CreateBor(_borParameter);

                    return Ok(new ApiOkResponse(pbs));
                }

                return BadRequest(new ApiResponse(400, false, "need both customerId and organizationId"));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpPost("UpdateSbProject")]
    public async Task<ActionResult> UpdateSbProject([FromHeader(Name = "AuthToken")] string AuthToken,
        [FromBody] UpdateThProjectDto UpdateThProjectDto)
    {
        try
        {
            await using var dbConnection = new SqlConnection(ItenantProvider.GetTenant().ConnectionString);

            var _pbsDto = new ProjectDefinitionCreateDto();
            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            var user = new ApplicationUser
            {
                Email = "gg@gmail.com"
            };

            if (AuthToken == "c6d04456-b40e-11eb-8529-0242ac130003")
            {
                var project = dbConnection.Query<ProjectDefinition>(
                    "Select * From ProjectDefinition Where SequenceCode = @Id",
                    new
                    {
                        Id = UpdateThProjectDto.ProjectSequenceCode
                    }).FirstOrDefault();

                if (project != null)
                {
                    var _pbsUpdateDtoDto = new ProjectDefinitionUpdateDto();

                    _pbsUpdateDtoDto.Id = project.Id;
                    _pbsUpdateDtoDto.Name = project.Name;
                    _pbsUpdateDtoDto.ProjectManagementLevelId = "4fb90b13-93e5-457c-908e-fea699ad23f1";
                    _pbsUpdateDtoDto.ProjectScopeStatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1";
                    _pbsUpdateDtoDto.ProjectToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18";
                    _pbsUpdateDtoDto.ProjectTypeId = "a77e863a-d329-4f5b-ba1b-7dc3bf09b0b8";
                    _pbsUpdateDtoDto.ProjectStatus = "65f2f0fd-ea7a-440e-9fd4-346628ef1299";
                    _pbsUpdateDtoDto.Description = "";
                    _pbsUpdateDtoDto.Cu = project?.ContractingUnitId;

                    _pbsUpdateDtoDto.Location = new MapLocation
                    {
                        Position = new Position
                        {
                            Lat = UpdateThProjectDto.Latitude,
                            Lon = UpdateThProjectDto.Longitude
                        }
                    };

                    var timeId = dbConnection
                        .Query<string>("Select Id From ProjectTime Where ProjectId = @ProjectId",
                            new { ProjectId = project.Id }).FirstOrDefault();

                    _pbsUpdateDtoDto.ProjectTime = new ProjectTimeUpdateDto()
                    {
                        Id = timeId,
                        ProjectId = project.Id,
                        StartDate = UpdateThProjectDto.StartDateTime
                    };

                    await _iProjectDefinitionRepository.UpdateProjectDefinitionForTh(
                        _pbsUpdateDtoDto, _iProjectTimeRepository,
                        ItenantProvider, user);


                    await using var connection = new SqlConnection(project.ProjectConnectionString);

                    var pbsdata = connection.Query<PbsProduct>("select * from PbsProduct where ProductId = @Id",
                        new { Id = UpdateThProjectDto.ProductId }).FirstOrDefault();
                    var pbsParameters = new PbsParameters
                    {
                        ProjectSequenceId = project.SequenceCode,
                        ContractingUnitSequenceId = "COM-0001",
                        TenantProvider = ItenantProvider,
                        ChangedUser = user
                    };

                    var dtoNew = new PbsProductCreateDto
                    {
                        Id = pbsdata.Id,
                        Name = pbsdata.Name,
                        PbsType = "regular",
                        PbsProductItemTypeId = "48a7dd9c-55ac-4e7c-a2f3-653811c0eb14",
                        Scope = "0",
                        PbsProductStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                        PbsToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18",
                        StartDate = UpdateThProjectDto.StartDateTime?.Date,
                        Contract = pbsdata.Contract
                    };
                    if (UpdateThProjectDto.Velocity != null && UpdateThProjectDto.Capacity != null)
                    {
                        var duration = Convert.ToDouble(UpdateThProjectDto.Capacity) /
                            Convert.ToDouble(UpdateThProjectDto.Velocity) * 60;

                        UpdateThProjectDto.EndDateTime = UpdateThProjectDto.StartDateTime?.AddMinutes(duration);
                    }

                    dtoNew.EndDate = UpdateThProjectDto.EndDateTime;

                    pbsParameters.PbsDto = dtoNew;

                    var pbs = await _IPbsRepository.CreatePbs(pbsParameters).ConfigureAwait(false);


                    await connection.ExecuteAsync(
                        "Delete From PbsDynamicAttributes Where ProductId = @ProductId",
                        new { ProductId = pbsdata.Id });

                    var dynamicAttribute1 = new PbsDynamicAttributes
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = dtoNew.Id,
                        Key = "Velocity",
                        Value = UpdateThProjectDto.Velocity
                    };

                    var dynamicAttribute3 = new PbsDynamicAttributes
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = dtoNew.Id,
                        Key = "Capacity",
                        Value = UpdateThProjectDto.Capacity
                    };
                    var dynamicAttribute4 = new PbsDynamicAttributes
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = dtoNew.Id,
                        Key = "StartTime",
                        Value = UpdateThProjectDto.StartDateTime?.ToString("HH:mm")
                    };
                    var dynamicAttribute5 = new PbsDynamicAttributes
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = dtoNew.Id,
                        Key = "EndTime",
                        Value = UpdateThProjectDto.EndDateTime?.ToString("HH:mm")
                    };

                    var dynamicInsert =
                        "INSERT INTO dbo.PbsDynamicAttributes (Id, ProductId, [Key], Value) VALUES (@Id, @ProductId, @Key, @Value)";

                    await connection.ExecuteAsync(dynamicInsert, dynamicAttribute1);
                    await connection.ExecuteAsync(dynamicInsert, dynamicAttribute3);
                    await connection.ExecuteAsync(dynamicInsert, dynamicAttribute4);
                    await connection.ExecuteAsync(dynamicInsert, dynamicAttribute5);

                    return Ok(new ApiOkResponse(pbs));
                }

                return BadRequest(new ApiResponse(400, false, "project does not exist"));
            }

            return BadRequest(new ApiResponse(400, false, "Please send token"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }


    [HttpGet("GetPsResources/{PsId}")]
    public async Task<ActionResult> GetPsResources(string PsId, [FromHeader(Name = "CU")] string CU,
        [FromHeader(Name = "Project")] string Project)
    {
        try
        {
           // var tenant = ItenantProvider.GetTenant();
            var connectionString = ConnectionString.MapConnectionString(Request.Headers["CU"].FirstOrDefault(),
                Request.Headers["Project"].FirstOrDefault(), ItenantProvider);

            var param = new { PsId };

            var sql =
                @"SELECT MIN(Id) AS Id, PsId, CpcId, CpcResourceNumber, CpcTitle, ConsumedQuantity, CostToMou, TotalCost, SoldQuantity, SpToMou, Status, CpcResourceTypeId, ConsumedQuantityMou, MouId, ProjectCostId, ProductTitle, GeneralLedgerId,
                    SoldQuantity * SpToMou as Total
                    FROM PsResource where PsId = @PsId
                    GROUP BY PsId, CpcId, CpcResourceNumber, CpcTitle, ConsumedQuantity, CostToMou, TotalCost, SoldQuantity, SpToMou, Status, CpcResourceTypeId, ConsumedQuantityMou, MouId, ProjectCostId, ProductTitle, GeneralLedgerId";

            await using var connection = new SqlConnection(connectionString);

            var psResources = connection.Query<PsResource>(sql, param).ToList();

            var totalAmount = psResources.Sum(x => x.Total);

            return Ok(new ApiOkResponse(psResources));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpPost("DeleteSbProject")]
    public async Task<ActionResult<DeleteThProjectDto>> DeleteSbProject(
        [FromHeader(Name = "AuthToken")] string AuthToken,
        [FromBody] DeleteThProjectDto DeleteThProjectDto)
    {
        var connectionString = ConnectionString.MapConnectionString("COM-0001",
            DeleteThProjectDto.ProjectSequenceCode, ItenantProvider);

        await using var connection = new SqlConnection(connectionString);

        var pbs = connection.Query<string>("Select Id From PbsProduct where ProductId = @Id",
            new { Id = DeleteThProjectDto.ProductId }).FirstOrDefault();
        await connection.ExecuteAsync("Delete From Bor where PbsProductId = @Id", new { Id = pbs });
        await connection.ExecuteAsync("Delete From PbsProduct where ProductId = @Id ",
            new { Id = DeleteThProjectDto.ProductId });

        return DeleteThProjectDto;
    }

    [AllowAnonymous]
    [HttpGet("DeleteProject/{Id}")]
    public async Task<ActionResult<string>> TestPostApi(string Id,
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX)
    {
        try
        {
            var updatequery = "Update ProjectDefinition Set IsDeleted = 1 Where SequenceCode = @SequenceCode";
            await using var connection = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString);

            await connection.ExecuteAsync(updatequery,
                new
                {
                    SequenceCode = Id
                });
            return Ok(new ApiOkResponse(Id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpPost("CreateAgreementItem")]
    public async Task<ActionResult> PotteauAdd(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] DossierDto dto)
    {
        try
        {
            await using var connection = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString);

            var sql = @"INSERT INTO dbo.Potteau (Id,JsonRequest) VALUES (@Id,@JsonRequest)";

            var jsonString = JsonConvert.SerializeObject(dto, Formatting.Indented);

            await connection.ExecuteAsync(sql, new { Id = Guid.NewGuid().ToString(), JsonRequest = jsonString });

            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }

    [AllowAnonymous]
    [HttpGet("AddPbsTreeIndexAllProjects")]
    [ProducesResponseType(typeof(ApiOkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<List<DatabasesEx>> AddPbsTreeIndexAllProjects([FromHeader(Name = "env")] string env,
        [FromHeader(Name = "sql")] string sql)
    {
        var result = new List<ProjectDefinition>();

        var exceptionLst = new List<DatabasesEx>();
        await using (var connection = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString))

            result = connection
                .Query<ProjectDefinition>(
                    @"SELECT * FROM ProjectDefinition WHERE IsDeleted = 0 ORDER BY SequenceCode ASC").ToList();


        var insertQuery =
            "INSERT INTO dbo.PbsTreeIndex ( Id ,PbsProductId ,TreeIndex ) VALUES ( @Id ,@PbsProductId ,@TreeIndex );";

        // var deleteQuery = @"DELETE  FROM PbsTreeIndex";
        foreach (var project in result)
        {
            try
            {
                await using var projectConnection = new SqlConnection(project.ProjectConnectionString);

                //await projectConnection.ExecuteAsync(deleteQuery);
                var treeData =
                    projectConnection.Query<PbsTreeIndex>("Select * From PbsTreeIndex WHERE PbsProductId LIKE '%PBS%'");
                int val = 0;
                var pbsData = projectConnection.Query<PbsProduct>(
                    @"SELECT * FROM PbsProduct WHERE NodeType = 'P' AND IsDeleted = 0 ORDER BY ProductId DESC");
                if (treeData.Any())
                {
                    val = treeData.MaxBy(x => x.TreeIndex).TreeIndex + 1;
                }

                foreach (var i in pbsData)

                {
                    if (!treeData.Any(x => x.PbsProductId == i.ProductId))
                    {
                        var param = new
                        {
                            Id = Guid.NewGuid(),
                            PbsProductId = i.ProductId,
                            TreeIndex = val
                        };

                        await projectConnection.ExecuteAsync(insertQuery, param);
                        val++;
                    }
                }
            }
            catch (Exception ex)
            {
                var mDatabasesEx = new DatabasesEx
                {
                    DatabaseName = project.SequenceCode,
                    Exception = ex
                };
                exceptionLst.Add(mDatabasesEx);
            }
        }

        return exceptionLst;
    }

    [AllowAnonymous]
    [HttpPost("CreateDossier")]
    public async Task<ActionResult> CreateDossier(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] DossierDto dto)
    {
        var _log4net = LogManager.GetLogger(typeof(ContractingUnitTenantsController));
        var jsonString = JsonConvert.SerializeObject(dto, Formatting.Indented);

        try
        {
            _log4net.Info("CreateDossier Started");

            var resultList = new List<string>();

            var lang = langCode(Request.Headers["lang"].FirstOrDefault());

            lang ??= "en";

            await using var connection = new SqlConnection(_iTenantProvider.GetTenant().ConnectionString);

            if (dto != null)
            {
                if (dto.SequenceId == null)
                {
                    throw new Exception("SequenceId cannot be null");
                }

                var teamList = new List<ProjectTeamRoleCreateDto>();

                var dossier = connection
                    .Query<Dossier>("Select * From Dossier Where Name = @Name", new { Name = dto.Name })
                    .FirstOrDefault();

                if (dossier == null)
                {
                    const string dossierInsertSql = "Insert Into Dossier Values (@Id,@Name,@SequenceId)";

                    var dossierParam = new Dossier
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = dto.Name,
                        SequenceId = dto.SequenceId
                    };
                    await connection.ExecuteAsync(dossierInsertSql, dossierParam);

                    dossier = dossierParam;
                }

                if (dto.ProjectDefinition?.Customer != null && dto.ProjectDefinition.Organization != null)
                {
                    var org = connection.Query<CabCompany>("Select * From CabCompany WHERE Name = @Name",
                        new { Name = dto.ProjectDefinition.Customer.FirstName }).FirstOrDefault();

                    if (org == null)
                    {
                        var company = new CabCompany
                        {
                            Name = dto.ProjectDefinition.Customer.FirstName,
                            IsSaved = true,
                            CabVat = new CabVat(),
                            CabBankAccount = new CabBankAccount(),
                            Email = new CabEmail(),
                            LandPhone = new CabLandPhone(),
                            MobilePhone = new CabMobilePhone(),
                            WhatsApp = new CabWhatsApp(),
                            Skype = new CabSkype(),
                            Address = new CabAddress
                            {
                                City = dto.ProjectDefinition.Organization.City,
                                Street = dto.ProjectDefinition.Organization.Street,
                                StreetNumber = dto.ProjectDefinition.Organization.StreetNumber,
                                PostalCode = dto.ProjectDefinition.Organization.PostalCode,
                            }
                        };

                        var companyParam = new CompanyRepositoryParameter()
                        {
                            CompanyId = company.Id,
                            ApplicationDbContext = UPrinceCustomerContext,
                            Company = company,
                            TenantProvider = _iTenantProvider,
                            CabHistoryLogRepository = _iCabHistoryLogRepository,
                            CabHistoryLogRepositoryParameter = new CabHistoryLogRepositoryParameter()
                            {
                                ApplicationDbContext = UPrinceCustomerContext,
                                Company = new CompanyDto(),
                                ChangedUser = new ApplicationUser()
                            }
                        };

                        var companyId = await _iCompanyRepository.AddCompany(companyParam);
                        company.Id = companyId;

                        org = company;
                    }

                    var cabCreateDto = new DossierCabCreateDto()
                    {
                        FirstName = dto.ProjectDefinition.Customer.FirstName,
                        FamilyName = dto.ProjectDefinition.Customer.FamilyName,
                        //Mail = dto.ProjectDefinition.Customer.Mail,
                        FullName = dto.ProjectDefinition.Customer.FirstName,
                        // MobileNumber = dto.ProjectDefinition.Customer.MobileNumber
                    };
                    var customer = await CabCreation(cabCreateDto, connection, org, false);

                    var cusAdd = new ProjectTeamRoleCreateDto
                    {
                        CabPersonId = customer.Id,
                        RoleId = "910b7af0-b132-4951-a2dc-6ab82d4cd40d", //customer
                        IsAccessGranted = false,
                        status = "1"
                    };

                    teamList.Add(cusAdd);

                    string pmId = null;
                    string siteManagerId = null;

                    // if (dto.ProjectDefinition.ProjectManager != null)
                    // {
                    //     var pmCabCreateDto = new DossierCabCreateDto()
                    //     {
                    //         FirstName = dto.ProjectDefinition.ProjectManager.FirstName,
                    //         FamilyName = dto.ProjectDefinition.ProjectManager.FamilyName,
                    //         Mail = dto.ProjectDefinition.ProjectManager.Mail,
                    //         FullName = dto.ProjectDefinition.ProjectManager.FirstName + " " +
                    //                    dto.ProjectDefinition.ProjectManager.FamilyName,
                    //         MobileNumber = dto.ProjectDefinition.ProjectManager.MobileNumber
                    //
                    //     };
                    //     var pm = await CabCreation(pmCabCreateDto, connection, org,false);
                    //
                    //     var pmAdd = new ProjectTeamRoleCreateDto
                    //     {
                    //         CabPersonId = pm.Id,
                    //         RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3", //project manager
                    //         IsAccessGranted = true,
                    //         status = "1"
                    //     };
                    //     pmId = pm.Id;
                    //     teamList.Add(pmAdd);
                    // }

                    // if (dto.Contract != null)
                    // {
                    //     var siteLeaderCabCreateDto = new DossierCabCreateDto()
                    //     {
                    //         FirstName = dto.Contract.SiteLeader,
                    //         FullName = dto.Contract.SiteLeader,
                    //
                    //     };
                    //     var siteLeader = await CabCreation(siteLeaderCabCreateDto, connection, org, true);
                    //
                    //     var siteLeaderAdd = new ProjectTeamRoleCreateDto
                    //     {
                    //         CabPersonId = siteLeader.Id,
                    //         RoleId = "266a5f47-3489-484b-8dae-e4468c5329dn3", //site manager
                    //         IsAccessGranted = true,
                    //         status = "1"
                    //     };
                    //
                    //     teamList.Add(siteLeaderAdd);
                    //
                    //     siteManagerId = siteLeader.Id;
                    // }
                    //
                    // var cusWarfCreateDto = new DossierCabCreateDto()
                    // {
                    //     FirstName = dto.ProjectDefinition.RoleCustomerWarfcontact.FirstName,
                    //     FamilyName = dto.ProjectDefinition.RoleCustomerWarfcontact.FamilyName,
                    //     //Mail = dto.ProjectDefinition.RoleCustomerWarfcontact.Mail,
                    //     FullName = dto.ProjectDefinition.RoleCustomerWarfcontact.FirstName + " " +
                    //                dto.ProjectDefinition.RoleCustomerWarfcontact.FamilyName,
                    //     //MobileNumber = dto.ProjectDefinition.RoleCustomerWarfcontact.MobileNumber
                    //
                    // };
                    // var warfCustomer = await CabCreation(cusWarfCreateDto, connection, org, false);
                    //
                    // var cusWarfAdd = new ProjectTeamRoleCreateDto
                    // {
                    //     CabPersonId = warfCustomer.Id,
                    //     RoleId = "907b7af0-b132-4951-a2dc-6ab82d4cd40d", //customer invoice contact
                    //     IsAccessGranted = false,
                    //     status = "1"
                    // };
                    //
                    // teamList.Add(cusWarfAdd);

                    if (dto.ProjectDefinition.Roles != null)
                    {
                        foreach (var item in dto.ProjectDefinition.Roles)
                        {
                            var roleCreateDto = new DossierCabCreateDto()
                            {
                                FirstName = item.Name.Split(" ").FirstOrDefault(),
                                FamilyName = item.Name.Split(" ").LastOrDefault(),
                                FullName = item.Name
                            };
                            item.PersonId = CabCreation(roleCreateDto, connection, org, false).Result.Id;

                            var roleAdd = new ProjectTeamRoleCreateDto
                            {
                                CabPersonId = item.PersonId,
                                RoleId = "4837043c-119c-47e1-bbf2-1f32557fdf30", //user
                                IsAccessGranted = true,
                                status = "1"
                            };

                            teamList.Add(roleAdd);
                        }
                    }

                    var project = connection
                        .Query<ProjectCreateReturnResponse>(
                            "Select Id As ProjectId,SequenceCode AS SequenceId,ProjectConnectionString From ProjectDefinition Where Name = @Name",
                            new
                            {
                                Name = dto.SequenceId + " - " + dto.ProjectDefinition.Customer.FirstName + " " +
                                       dto.ProjectDefinition.Customer.FamilyName + " - " + dto.Name + " - " +
                                       dto.Contract?.BusinessUnit
                            }).FirstOrDefault();

                    if (project == null)
                    {
                        var createProjectDto = new ProjectDefinitionCreateDto();
                        createProjectDto.Name = dto.SequenceId + " - " + dto.ProjectDefinition.Customer.FirstName +
                                                " " + dto.ProjectDefinition.Customer.FamilyName + " - " + dto.Name +
                                                " - " + dto.Contract?.BusinessUnit;
                        createProjectDto.ProjectManagementLevelId = "4fb90b13-93e5-457c-908e-fea699ad23f1";
                        createProjectDto.ProjectScopeStatusId = "94282458-0b40-40a3-b0f9-c2e40344c8f1";
                        createProjectDto.ProjectToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18";
                        createProjectDto.ProjectTypeId = "a77e863a-d329-4f5b-ba1b-7dc3bf09b0b8";
                        createProjectDto.ProjectStatus = "65f2f0fd-ea7a-440e-9fd4-346628ef1299";

                        createProjectDto.Description = dto.ProjectDefinition.Description ?? "";
                        createProjectDto.CustomerId = customer.Id;
                        //createProjectDto.ProjectManagerId = siteManagerId;
                        //createProjectDto.SiteManagerId = siteManagerId;


                        if (dto.ProjectDefinition.Address != null)
                        {
                            var mapClient = new AzureMapClient(_configuration);

                            var positions =
                                await mapClient.GetPositionsByAddress(dto.ProjectDefinition.Address.ProjectAddress);

                            if (positions != null)
                            {
                                createProjectDto.Location = new MapLocation
                                {
                                    Position = new Position
                                    {
                                        Lat = positions.Position.Lat.ToString(),
                                        Lon = positions.Position.Lon.ToString()
                                    },
                                    Address = positions.Address
                                };
                            }
                        }

                        createProjectDto.ProjectTime = new ProjectTimeCreateDto();
                        createProjectDto.ProjectTeam = new ProjectTeamCreateDto
                        {
                            ContractingUnitId = "e0386eac-c9a0-4f93-8baf-d24948bedda9",
                            TeamRoleList = teamList
                        };
                        createProjectDto._iConfiguration = _configuration;

                        var projectCreate = await _iProjectDefinitionRepository.CreateProjectForTh(
                            UPrinceCustomerContext,
                            createProjectDto, _iProjectTimeRepository, _iProjectFinanceRepository,
                            _iProjectTeamRepository,
                            _iTenantProvider, new ApplicationUser(), _IPbsRepository);

                        var bu = connection
                            .Query<OrganizationTaxonomy>(
                                "SELECT * FROM OrganizationTaxonomy WHERE OrganizationTaxonomyLevelId = 'oo10e768-3e06-po02-b337-ee367a82admn' AND BuName = @BuName",
                                new { BuName = dto.Contract?.BusinessUnit }).FirstOrDefault();

                        if (bu != null)
                        {
                            const string insertBu =
                                @"INSERT INTO dbo.ProjectClassification ( Id ,ProjectId ,ProjectClassificationBuisnessUnit ) VALUES ( @Id ,@ProjectId ,@ProjectClassificationBuisnessUnit )";

                            var buParam = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectId = projectCreate.ProjectId,
                                ProjectClassificationBuisnessUnit = bu.Id
                            };

                            await connection.ExecuteAsync(insertBu, buParam);
                        }

                        project = projectCreate;
                    }

                    // var connectionString = ConnectionString.MapConnectionString("COM-0001",
                    //     project.SequenceId, _iTenantProvider);


                    await using var projectConnection = new SqlConnection(project.ProjectConnectionString);


                    var pbsParameters = new PbsParameters
                    {
                        ProjectSequenceId = project.SequenceId,
                        ContractingUnitSequenceId = "COM-0001",
                        TenantProvider = _iTenantProvider,
                        ChangedUser = new ApplicationUser()
                    };
                    foreach (var item in dto.PBS)
                    {
                        var resExist = item.Resources.Any(x => x.LabourHours.ToDouble() > 0);

                        if (resExist)
                        {
                            if (item.Title != null)
                            {
                                var cuConnectionString = ConnectionString.MapConnectionString(
                                    pbsParameters.ContractingUnitSequenceId,
                                    null, pbsParameters.TenantProvider);

                                await using var cuConnection = new SqlConnection(cuConnectionString);

                                var vendorCpc = cuConnection
                                    .Query<string>(
                                        "SELECT CoperateProductCatalogId FROM CpcVendor WHERE ResourceTitle = @title",
                                        new { title = item.Title }).FirstOrDefault();
                                if (vendorCpc != null)
                                {
                                    var pbs = projectConnection.Query<PbsProduct>(
                                        "Select * From PbsProduct Where Name = @Name",
                                        new { Name = item.Title }).FirstOrDefault();

                                    if (pbs == null)
                                    {
                                        var dtoNew = new PbsProductCreateDto
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Name = item.Title,
                                            PbsType = "regular",
                                            PbsProductItemTypeId = "aa0c8e3c-f716-4f92-afee-851d485164da", //internal
                                            Scope = "0",
                                            PbsProductStatusId = "d60aad0b-2e84-482b-ad25-618d80d49477",
                                            PbsToleranceStateId = "004eb795-8bba-47e8-9049-d14774ab0b18",
                                            Contract = dto.Contract.SequenceId,
                                        };

                                        pbsParameters.PbsDto = dtoNew;

                                        var pbsCreate = await _IPbsRepository.CreatePbs(pbsParameters)
                                            .ConfigureAwait(false);

                                        pbs = pbsCreate;
                                    }
                                    else
                                    {
                                        await projectConnection.ExecuteAsync(
                                            "Update PbsProduct Set Contract = @Contract Where Id = @Id ",
                                            new
                                            {
                                                Id = pbs.Id,
                                                Contract = pbs.Contract + "," + dto.Contract.SequenceId
                                            });
                                    }

                                    // item.Resources = item.Resources.Where(x => x.LabourHours.ToDouble() > 0)
                                    //     .ToList();
                                    // foreach (var res in item.Resources)
                                    // {
                                    //     try
                                    //     {
                                    //         
                                    //         var cuConnectionString = ConnectionString.MapConnectionString(
                                    //             pbsParameters.ContractingUnitSequenceId,
                                    //             null, pbsParameters.TenantProvider);
                                    //
                                    //         await using var cuConnection = new SqlConnection(cuConnectionString);
                                    //
                                    //         var cpc = cuConnection
                                    //             .Query<CorporateProductCatalog>(
                                    //                 "Select * From CorporateProductCatalog Where ResourceTitle = @ResourceTitle ",
                                    //                 new { ResourceTitle = res.Title }).FirstOrDefault();
                                    //
                                    //         string cpcId = null;
                                    //         if (cpc == null)
                                    //         {
                                    //             var _cpcParameter = new CpcParameters
                                    //             {
                                    //                 Lang = lang,
                                    //                 ContextAccessor = ContextAccessor,
                                    //                 TenantProvider = _iTenantProvider,
                                    //                 Context = UPrinceCustomerContext,
                                    //                 isCopy = false,
                                    //                 ContractingUnitSequenceId = "COM-0001",
                                    //             };
                                    //
                                    //
                                    //             var cpcDto = new CoperateProductCatalogCreateDto
                                    //             {
                                    //                 CpcBasicUnitOfMeasureId = "cce5fd6a-91e1-4fc0-b3f6-2c462eaf2500",
                                    //                 Id = Guid.NewGuid().ToString(),
                                    //                 ResourceFamilyId = "2210e768-human-kknk-jhhk-ee367a82ad17",
                                    //                 ResourceTypeId = "c46c3a26-39a5-42cc-b07s-89655304eh6",
                                    //                 ResourceTitle = res.Title,
                                    //                 Status = 1,
                                    //             };
                                    //
                                    //             _cpcParameter.CpcDto = cpcDto;
                                    //             var data =
                                    //                 await _iCoporateProductCatalogRepository.CreateCoporateProductCatalog(
                                    //                     _cpcParameter,
                                    //                     ContextAccessor);
                                    //
                                    //             cpcId = data.Id;
                                    //
                                    //             var pbsResourceParam = new PbsResourceParameters()
                                    //             {
                                    //                 ProjectSequenceId = project.SequenceId,
                                    //                 Lang = lang,
                                    //                 ContractingUnitSequenceId = "COM-0001",
                                    //                 TenantProvider = _iTenantProvider,
                                    //                 ICoporateProductCatalogRepository = _iCoporateProductCatalogRepository,
                                    //                 PbsLabourCreateDto = new LabourForPbsCreateDto()
                                    //                 {
                                    //                     Environment = "cu",
                                    //                     PbsProductId = pbs.Id,
                                    //                     CoperateProductCatalogId = cpcId,
                                    //                     Quantity = Math.Floor(res.LabourHours.ToDouble())
                                    //                 }
                                    //             };
                                    //
                                    //             await _IPbsResourceRepository.CreatePbsLabour(pbsResourceParam);
                                    //
                                    //         }
                                    //         else
                                    //         {
                                    //             var pbsresourceExist = projectConnection.Query<LabourForPbs>(
                                    //                     "Select * From PbsLabour Where PbsProductId = @PbsProductId AND CoperateProductCatalogId = @CoperateProductCatalogId",
                                    //                     new { PbsProductId = pbs.Id, CoperateProductCatalogId = cpc.Id })
                                    //                 .FirstOrDefault();
                                    //
                                    //             if (pbsresourceExist == null)
                                    //             {
                                    //                 var pbsResourceParam = new PbsResourceParameters()
                                    //                 {
                                    //                     ProjectSequenceId = project.SequenceId,
                                    //                     Lang = lang,
                                    //                     ContractingUnitSequenceId = "COM-0001",
                                    //                     TenantProvider = _iTenantProvider,
                                    //                     ICoporateProductCatalogRepository = _iCoporateProductCatalogRepository,
                                    //                     PbsLabourCreateDto = new LabourForPbsCreateDto()
                                    //                     {
                                    //                         Environment = "cu",
                                    //                         PbsProductId = pbs.Id,
                                    //                         CoperateProductCatalogId = cpc.Id,
                                    //                         Quantity = Math.Floor(res.LabourHours.ToDouble())
                                    //                     }
                                    //                 };
                                    //
                                    //                 await _IPbsResourceRepository.CreatePbsLabour(pbsResourceParam);
                                    //             }
                                    //             else
                                    //             {
                                    //                 await projectConnection.ExecuteAsync(
                                    //                     "Update PbsLabour Set Quantity = @Quantity Where Id = @Id ",
                                    //                     new
                                    //                     {
                                    //                         Id = pbsresourceExist.Id,
                                    //                         Quantity = pbsresourceExist.Quantity +
                                    //                                    Math.Floor(res.LabourHours.ToDouble())
                                    //                     });
                                    //             }
                                    //         }
                                    //
                                    //
                                    //     }
                                    //     catch (Exception e)
                                    //     {
                                    //
                                    //         throw new Exception(e + item.Title + " - " + res.Title);
                                    //     }
                                    // }

                                    var sumHours = item.Resources.Sum(x => x.LabourHours.ToDouble());

                                    var pbsresourceExist = projectConnection.Query<LabourForPbs>(
                                            "Select * From PbsLabour Where PbsProductId = @PbsProductId AND CoperateProductCatalogId = @CoperateProductCatalogId",
                                            new { PbsProductId = pbs.Id, CoperateProductCatalogId = vendorCpc })
                                        .FirstOrDefault();

                                    if (pbsresourceExist == null)
                                    {
                                        var pbsResourceParam = new PbsResourceParameters()
                                        {
                                            ProjectSequenceId = project.SequenceId,
                                            Lang = lang,
                                            ContractingUnitSequenceId = "COM-0001",
                                            TenantProvider = _iTenantProvider,
                                            ICoporateProductCatalogRepository = _iCoporateProductCatalogRepository,
                                            PbsLabourCreateDto = new LabourForPbsCreateDto()
                                            {
                                                Environment = "cu",
                                                PbsProductId = pbs.Id,
                                                CoperateProductCatalogId = vendorCpc,
                                                Quantity = Math.Floor(sumHours)
                                            }
                                        };

                                        await _IPbsResourceRepository.CreatePbsLabour(pbsResourceParam);
                                    }
                                    else
                                    {
                                        await projectConnection.ExecuteAsync(
                                            "Update PbsLabour Set Quantity = @Quantity Where Id = @Id ",
                                            new
                                            {
                                                Id = pbsresourceExist.Id,
                                                Quantity = pbsresourceExist.Quantity +
                                                           Math.Floor(sumHours)
                                            });
                                    }
                                }
                                else
                                {
                                    _log4net.Error("Phase not known in CPC_Vendor_Resource Title - " + item.Title);
                                    resultList.Add("Phase not known in CPC_Vendor_Resource Title - " + item.Title);
                                }
                            }
                        }
                    }

                    const string dossierProjectsInsertSql =
                        "Insert Into DossierProjects Values (@Id,@ProjectId,@DossierId)";

                    var dossierProjectsParam = new DossierProjects()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DossierId = dossier.Id,
                        ProjectId = project.ProjectId
                    };

                    var dossierExist = connection
                        .Query<DossierProjects>(
                            "Select * From DossierProjects Where DossierId = @DossierId AND ProjectId = @ProjectId ",
                            dossierProjectsParam).Any();

                    if (!dossierExist)
                    {
                        await connection.ExecuteAsync(dossierProjectsInsertSql, dossierProjectsParam);
                    }

                    var sql = @"INSERT INTO dbo.Potteau (Id,JsonRequest) VALUES (@Id,@JsonRequest)";


                    await connection.ExecuteAsync(sql,
                        new { Id = Guid.NewGuid().ToString(), JsonRequest = jsonString });
                }
            }

            _log4net.Info(jsonString);

            _log4net.Info("CreateDossier Ended");

            return Ok(new ApiOkResponse(resultList));
        }
        catch (Exception ex)
        {
            _log4net.Info("Exception thrown");
            _log4net.Error(ex.ToString());
            _log4net.Info(jsonString);
            _log4net.Info("CreateDossier Ended");


            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
        finally
        {
            _log4net.Info("final");
        }
    }

    private async Task<CabPerson> CabCreation(DossierCabCreateDto dto, SqlConnection connection, CabCompany org,
        bool isSiteLeader)
    {
        // var customer = connection
        //     .Query<CabPerson>(
        //         "Select cp.* From CabPersonCompany cpc LEFT OUTER JOIN CabPerson cp ON cpc.PersonId = cp.Id LEFT OUTER JOIN CabEmail ce ON cpc.EmailId = ce.Id WHERE ce.EmailAddress = @email",
        //         new { email = dto.ProjectDefinition.Customer.Mail }).FirstOrDefault();

        try
        {
            CabPerson customer;

            if (isSiteLeader)
            {
                customer = connection
                    .Query<CabPerson>("Select * From CabPerson Where CallName = @CallName",
                        new { CallName = dto.FullName })
                    .FirstOrDefault();
            }
            else
            {
                customer = connection
                    .Query<CabPerson>("Select * From CabPerson Where FullName = @FullName",
                        new { FullName = dto.FullName })
                    .FirstOrDefault();
            }


            if (customer == null)
            {
                var person = new CabPerson

                {
                    IsSaved = true,
                    Email = new CabEmail(),
                    FirstName = dto.FullName,
                    LandPhone = new CabLandPhone(),
                    MobilePhone = new CabMobilePhone(),
                    WhatsApp = new CabWhatsApp(),
                    Skype = new CabSkype(),
                    Surname = dto.FullName,
                    FullName = dto.FullName,
                    CallName = dto.FullName,
                    BusinessEmail = new CabEmail(),
                    BusinessPhone = new CabMobilePhone(),
                    Address = new CabAddress()
                };

                _personRepositoryParameter.PersonId = person.Id;
                _personRepositoryParameter.ApplicationDbContext = UPrinceCustomerContext;
                _personRepositoryParameter.Person = person;
                var cabHistoryLogRepositoryParameter = new CabHistoryLogRepositoryParameter()
                {
                    Person = new PersonDto(),
                    ApplicationDbContext = UPrinceCustomerContext,
                    ChangedUser = new ApplicationUser()
                };

                _personRepositoryParameter.TenantProvider = ItenantProvider;
                _personRepositoryParameter.CabHistoryLogRepository = _iCabHistoryLogRepository;
                _personRepositoryParameter.CabHistoryLogRepositoryParameter = cabHistoryLogRepositoryParameter;
                _personRepositoryParameter.NationalityId = person.NationalityId;

                var personId = await _ipersonRepository.AddPerson(_personRepositoryParameter);
                person.Id = personId;


                string personCompanyId = null;
                CabPersonCompany personCompany = null;
                personCompany = new CabPersonCompany
                {
                    IsSaved = true,
                    CompanyId = org.Id,
                    PersonId = person.Id,
                    Email = new CabEmail { EmailAddress = dto.Mail },
                    LandPhone = new CabLandPhone(),
                    MobilePhone = new CabMobilePhone { MobilePhoneNumber = dto.MobileNumber },
                    WhatsApp = new CabWhatsApp(),
                    Skype = new CabSkype()
                };

                var _personCompanyRepositoryParameter = new PersonCompanyRepositoryParameter
                {
                    PersonCompany = personCompany,
                    ApplicationDbContext = UPrinceCustomerContext,
                    Logger = _logger,
                };


//adding history
                cabHistoryLogRepositoryParameter.CabDataDto = new CabDataDto
                {
                    Person = new PersonDto()
                    {
                        Id = personId
                    }
                };
                _personCompanyRepositoryParameter.TenantProvider = ItenantProvider;
                _personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter = cabHistoryLogRepositoryParameter;
                _personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter.TenantProvider = ItenantProvider;
                _personCompanyRepositoryParameter.CabHistoryLogRepository = _iCabHistoryLogRepository;
                _personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter.Company =
                    new CompanyDto { Id = org.Id };
                _personCompanyRepositoryParameter.CabHistoryLogRepositoryParameter.ChangedUser = new ApplicationUser();

                personCompanyId =
                    await _iPersonCompanyRepository.AddPersonCompany(_personCompanyRepositoryParameter);

                customer = person;
            }

            return customer;
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString() + " - cab create");
        }
    }

    [AllowAnonymous]
    [HttpPost("TestAzureMap")]
    public async Task<ActionResult> TestAzureMap(
        [FromHeader(Name = "CU")] string CU, [FromHeader(Name = "Project")] string Project,
        [FromHeader(Name = "lang")] string langX, [FromBody] GeoPositions dto)
    {
        try
        {
            // var mapClient = new AzureMapClient(_configuration);
            //
            // double val = 0;
            // double val2 = 0;
            //
            // //bool isValid = int.TryParse(dto.Address, out int intValue) && intValue % 100 == 0;
            //
            //
            // // var kk = double.TryParse(dto.Address,NumberStyles.Float, new CultureInfo("en-US"), out val);
            // //
            // // var nn = double.TryParse(dto.Address,NumberStyles.Float, new CultureInfo("nl-NL"), out val2);
            //
            // var input = dto.Address;
            //
            // const string pattern = @"^([^/]+)";
            //
            // var match = Regex.Match(input, pattern);
            //
            // string result = null;
            // if (match.Success)
            // {
            //      result = match.Groups[1].Value.Trim();
            //
            // }
            // else
            // {
            //     Console.WriteLine("No match found");
            // }

            var childLabourItems = new List<Person>
            {
                new Person() { SSN = "1234567890", Name = "John Doe" },
                new Person() { SSN = "1234567891", Name = "Jane Doe" },
                new Person() { SSN = "1234567892", Name = "Emily Johnson" },
                new Person() { SSN = "1234567893", Name = "William Brown" },
                new Person() { SSN = "1234567894", Name = "Sarah Davis" },
            };

            var parentLabourItems = new List<Person>
            {
                // new Person() { SSN="1234567890", Name="John Doe" },
                // new Person() { SSN="1234567891", Name="Jane Doe" }
            };

            if (childLabourItems.Any())
            {
                var notExistCpc = childLabourItems.ExceptBy(
                    parentLabourItems.Select(x => x.SSN), e => e.SSN);

                //var result = await mapClient.GetPositionsByAddress(dto.Address);
            }

            return Ok(new ApiOkResponse("ok"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse(400, false, ex.ToString()));
        }
    }
}

public class Person
{
    public required string SSN { get; set; }
    public required string Name { get; set; }

    public override string ToString() => $"{Name} <{SSN}>";
}

public class DossierDto
{
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public DossierProjectDefinitionDto ProjectDefinition { get; set; }
    public DossierContractDto Contract { get; set; }
    public List<DossierPbsDto> PBS { get; set; }
}

public class DossierProjectDefinitionDto
{
    public string Description { get; set; }
    public string Name { get; set; }
    public DossierUserDto Customer { get; set; }
    public DossierUserDto ProjectManager { get; set; }
    public DossierOrgDto Organization { get; set; }
    public DossierUserDto RoleCustomerWarfcontact { get; set; }
    public List<DossierRoleDto> Roles { get; set; }
    public DossierAddressDto Address { get; set; }
}

public class DossierUserDto
{
    public string FirstName { get; set; }

    public string FamilyName { get; set; }

    //public string MobileNumber { get; set; }
    //public string Mail { get; set; }
    public string PersonId { get; set; }
}

public class DossierOrgDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
}

public class DossierRoleDto
{
    public string Name { get; set; }
    public string RoleId { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string PersonId { get; set; }
}

public class DossierAddressDto
{
    public string ProjectAddress { get; set; }
}

public class DossierContractDto
{
    public string SequenceId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string BusinessUnit { get; set; }
    public string FinancialTaxonomyMachineTaxonomy { get; set; }
    public string SiteLeader { get; set; }
}

public class DossierPbsDto
{
    public string Title { get; set; }
    public List<DossierResourcesDto> Resources { get; set; }
}

public class DossierResourcesDto
{
    public string Title { get; set; }
    public string LabourHours { get; set; }
}

public class DossierCabCreateDto
{
    public string FirstName { get; set; }
    public string FamilyName { get; set; }
    public string MobileNumber { get; set; }
    public string Mail { get; set; }
    public string FullName { get; set; }
}