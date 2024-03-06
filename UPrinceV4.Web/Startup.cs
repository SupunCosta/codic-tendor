using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Quartz;
using UPrinceV4.Shared;
using UPrinceV4.Shared.FileSystem;
using UPrinceV4.Shared.TenantProviders;
using UPrinceV4.Shared.TenantSources;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories;
using UPrinceV4.Web.Repositories.BM;
using UPrinceV4.Web.Repositories.BOR;
using UPrinceV4.Web.Repositories.CAB;
using UPrinceV4.Web.Repositories.Category;
using UPrinceV4.Web.Repositories.CPC;
using UPrinceV4.Web.Repositories.GD;
using UPrinceV4.Web.Repositories.HR;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.BM;
using UPrinceV4.Web.Repositories.Interfaces.BOR;
using UPrinceV4.Web.Repositories.Interfaces.CAB;
using UPrinceV4.Web.Repositories.Interfaces.Category;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.GD;
using UPrinceV4.Web.Repositories.Interfaces.HR;
using UPrinceV4.Web.Repositories.Interfaces.INV;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Repositories.Interfaces.PC;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Repositories.Interfaces.PO;
using UPrinceV4.Web.Repositories.Interfaces.PriceCalaculator;
using UPrinceV4.Web.Repositories.Interfaces.PS;
using UPrinceV4.Web.Repositories.Interfaces.RFQ;
using UPrinceV4.Web.Repositories.Interfaces.ThAutomation;
using UPrinceV4.Web.Repositories.INV;
using UPrinceV4.Web.Repositories.Mecop;
using UPrinceV4.Web.Repositories.PBS;
using UPrinceV4.Web.Repositories.PC;
using UPrinceV4.Web.Repositories.PdfToExcel;
using UPrinceV4.Web.Repositories.PMOL;
using UPrinceV4.Web.Repositories.PO;
using UPrinceV4.Web.Repositories.PriceCalculator;
using UPrinceV4.Web.Repositories.PS;
using UPrinceV4.Web.Repositories.RFQ;
using UPrinceV4.Web.Repositories.StandardMail;
using UPrinceV4.Web.Repositories.Stock;
using UPrinceV4.Web.Repositories.ThAutomation;
using UPrinceV4.Web.Repositories.TimeRegistration;
using UPrinceV4.Web.Repositories.VAT;
using UPrinceV4.Web.Repositories.VP;
using UPrinceV4.Web.Repositories.WBS;
using UPrinceV4.Web.Repositories.WF;
using UPrinceV4.Web.Repositories.WH;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(opts => opts.SuppressModelStateInvalidFilter = true);
        services.AddDbContext<UPrinceCustomerContex>(options => { options.EnableSensitiveDataLogging(); });
        services.AddDbContext<ApplicationDbContext>(options => { options.EnableSensitiveDataLogging(); },
            ServiceLifetime.Transient);

        services.AddDbContext<ApplicationDbContextStartUp>(options => { options.EnableSensitiveDataLogging(); },
            ServiceLifetime.Transient);

        services.AddDbContext<ShanukaDbContext>(options => { options.EnableSensitiveDataLogging(); },
            ServiceLifetime.Transient);
        services
            .AddScoped<IUPrinceCustomerContactPreferenceRepository, UPrinceCustomerContactPreferenceRepository>();
        services.AddScoped<IUPrinceCustomerJobRoleRepository, UPrinceCustomerJobRoleRepository>();
        services.AddScoped<IUPrinceCustomerLegalAddressRepository, UPrinceCustomerLegalAddressRepository>();
        services.AddScoped<IUPrinceCustomerLocationRepository, UPrinceCustomerLocationRepository>();
        services.AddScoped<IUPrinceCustomerPrimaryContactRepository, UPrinceCustomerPrimaryContactRepository>();
        services.AddScoped<IUPrinceCustomerProfileRepository, UPrinceCustomerProfileRepository>();
        services.AddScoped<IUPrinceCustomerRepository, UPrinceCustomerRepository>();
        services.AddScoped<ITimeClockRepository, TimeClockRepository>();
        services.AddScoped<IQrCodeRepository, QrCodeRepository>();
        services.AddScoped<ILocaleRepository, LocaleRepository>();
        services.AddScoped<IProjectDefinitionRepository, ProjectDefinitionRepository>();
        services.AddScoped<ICalendarTemplateRepository, CalendarTemplateRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IProjectFinanceRepository, ProjectFinanceRepository>();
        services.AddScoped<IProjectHistoryLogRepository, ProjectHistoryLogRepository>();
        services.AddScoped<IProjectManagementLevelRepository, ProjectManagementLevelRepository>();
        services.AddScoped<IProjectStateRepository, ProjectStateRepository>();
        services.AddScoped<IProjectTeamRepository, ProjectTeamRepository>();
        services.AddScoped<IProjectTemplateRepository, ProjectTemplateRepository>();
        services.AddScoped<IProjectTimeRepository, ProjectTimeRepository>();
        services.AddScoped<IProjectToleranceStateRepository, ProjectToleranceStateRepository>();
        services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IAppConfigurationRepository, AppConfigurationRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<ITimeClockActivityTypeRepository, TimeClockActivityTypeRepository>();
        services.AddScoped<IWorkflowStateRepository, WorkflowStateRepository>();
        services.AddScoped<IProjectTimeRepository, ProjectTimeRepository>();
        services.AddScoped<IProjectFinanceRepository, ProjectFinanceRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IPersonCompanyRepository, PersonCompanyRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICoporateProductCatalogRepository, CoporateProductCatalogRepository>();
        services.AddScoped<ICabHistoryLogRepository, CabHistoryLogRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICpcVendorRepository, CpcVendorRepository>();
        services.AddScoped<ICpcNicknameRepository, CpcNicknameRepository>();
        services.AddScoped<IUniqueContactDetailsRepository, UniqueContactDetailsRepository>();
        services.AddScoped<IPbsRepository, PbsRepository>();
        services.AddScoped<IPbsExperienceRepository, PbsExperienceRepository>();
        services.AddScoped<IPbsSkillRepository, PbsSkillRepository>();
        services.AddScoped<ICompetenciesRepository, CompetenciesRepository>();
        services.AddScoped<IPbsInstructionFamilyRepository, PbsInstructionFamilyRepository>();
        services.AddScoped<IPbsInstructionsRepository, PbsInstructionsRepository>();
        services.AddScoped<IPmolJournalRepository, PmolJournalRepository>();
        services.AddScoped<IPsRepository, PsRepository>();
        services.AddScoped<IPmolJournalRepository, PmolJournalRepository>();
        // Repository
        services.AddSingleton(new ApiResponse(200, true));
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IPersonCompanyRepository, PersonCompanyRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ICoporateProductCatalogRepository, CoporateProductCatalogRepository>();
        services.AddScoped<ICabHistoryLogRepository, CabHistoryLogRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICpcVendorRepository, CpcVendorRepository>();
        services.AddScoped<ICpcNicknameRepository, CpcNicknameRepository>();
        services.AddScoped<IUniqueContactDetailsRepository, UniqueContactDetailsRepository>();
        services.AddScoped<IRiskStatusRepository, RiskStatusRepository>();
        services.AddScoped<IRiskTypeRepository, RiskTypeRepository>();
        services.AddScoped<IRiskRepository, RiskRepository>();
        services.AddScoped<IQualityRepository, QualityRepository>();
        services.AddScoped<IPbsRiskRepository, PbsRiskRepository>();
        services.AddScoped<IPbsQualityRepository, PbsQualityRepository>();
        services.AddScoped<IPbsResourceRepository, PbsResourceRepository>();
        services.AddScoped<IBorRepository, BorRepository>();
        services.AddScoped<IPmolRepository, PmolRepository>();
        services.AddScoped<IBorResourceRepository, BorResourceRepository>();
        services.AddScoped<IPmolResourceRepository, PmolResourceRepository>();
        services.AddScoped<IPriceListRepository, PriceListRepository>();
        services.AddScoped<IProjectCostRepository, ProjectCostRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IVATRepository, VATRepository>();
        services.AddScoped<IPORepository, PORepository>();
        services.AddScoped<IManagerCPCRepository, ManagerCPCRepository>();
        services.AddScoped<IDropDownRepository, DropDownRepository>();
        services.AddScoped<IWareHouseRepository, WareHouseRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IWorkFlowRepository, WorkFlowRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IVPRepository, VPRepository>();
        services.AddScoped<IOrganizationSettingsRepository, OrganizationSettingsRepository>();
        services.AddScoped<IAbsenceRepository, AbsenceRepository>();
        services.AddScoped<IHRRepository, HRRepository>();
        services.AddScoped<IContractorReopsitory, ContractorRepository>();
        services.AddScoped<IStandardMailRepositary, StandardMailRepositary>();
        services.AddScoped<IPdfToExcelRepository, PdfToExcelReposoitory>();
        services.AddScoped<IGraphRepository, GraphRepository>();
        services.AddScoped<ISendGridRepositorie, SendGridRepositorie>();
        services.AddScoped<ISendGridMailRepositorie, SendGridMailRepositorie>();
        services.AddScoped<IForemanMigrationRepository, ForemanMigrationRepository>();
        services.AddScoped<IGDRepository, GDRepository>();
        services.AddScoped<ICpcResourceFamilyRepository, CpcResourceFamilyRepository>();
        services.AddScoped<ICiawRepository, CiawRepository>();
        services.AddScoped<IRFQRepository, RFQRepository>();
        services.AddScoped<ITimeRegistrationRepository, TimeRegistrationRepository>();
        services.AddScoped<IThAutomationRepository, ThAutomationRepository>();
        services.AddScoped<IPriceCalculatorRepository, PriceCalculatorRepository>();
        services.AddScoped<IResourceMatrixRepository, ResourceMatrixRepository>();
        services.AddScoped<IBurndownChartRepository, BurndownChartRepository>();
        services.AddScoped<IMyCalenderRepository, MyCalenderRepository>();
        services.AddScoped<IWbsRepository, WbsRepository>();
        services.AddScoped<IMecopRepository, MecopRepository>();
        services.AddScoped<IIssueRepository, IssueRepository>();

        
        // Response
        services.AddSingleton(new ApiResponse(200, true));
        services.AddSingleton(new ApiBadRequestResponse(null));
        services.AddSingleton(new ApiOkResponse(200, "Ok"));

        // RepositoryParameter
        services.AddSingleton(new CompanyRepositoryParameter());
        services.AddSingleton(new PersonCompanyRepositoryParameter());
        services.AddSingleton(new PersonRepositoryParameter());
        services.AddSingleton(new CountryRepositoryParameter());
        services.AddSingleton(new CpcParameters());
        services.AddSingleton(new CpcParameters());
        services.AddSingleton(new CabHistoryLogRepositoryParameter());
        services.AddSingleton(new CpcParameters());
        services.AddSingleton(new CpcParameters());
        services.AddSingleton(new CpcVendorParameters());
        services.AddSingleton(new CpcNicknameParameters());
        services.AddSingleton(
            new UniqueContactDetailsRepositoryParameter());
        services.AddSingleton(new PbsParameters());
        services.AddSingleton(new QualityRepositoryParameter());
        services.AddSingleton(new CompetenciesRepositoryParameter());
        services.AddSingleton(new PbsSkillRepositoryParameter());
        services.AddSingleton(new PbsExperienceRepositoryParameter());
        services.AddSingleton(new BorParameter());

        services.AddSingleton(
            new UniqueContactDetailsRepositoryParameter());
        services.AddSingleton(new RiskRepositoryParameter());
        services.AddSingleton(new RiskStatusRepositoryParameter());
        services.AddSingleton(new RiskTypeRepositoryParameter());
        services.AddSingleton(
            new PbsInstructionFamilyRepositoryParameter());
        services.AddSingleton(new PbsInstructionsRepositoryParameter());
        services.AddSingleton(new PbsRiskParameters());
        services.AddSingleton(new PbsQualityParameters());
        services.AddSingleton(new PbsResourceParameters());
        services.AddSingleton(new PmolParameter());
        services.AddSingleton(new ProjectTeamRoleParameter());
        services.AddSingleton(new PmolJournalParameter());
        services.AddSingleton(new PmolResourceParameter());
        services.AddSingleton(new PriceListParameter());
        services.AddSingleton(new PmolResourceParameter());
        services.AddSingleton(new PsParameter());
        services.AddSingleton(new ProjectCostRepositoryParameter());
        services.AddSingleton(new VATParameter());
        services.AddSingleton(new CategoryParameter());
        services.AddSingleton(new ProjectDefinitionParameter());
        services.AddSingleton(new StandardMailParameters());
        services.AddSingleton(new PdfToExcelParameter());
        services.AddSingleton(new GraphParameter());
        services.AddSingleton(new SendGridParameter());
        services.AddSingleton(new SendGridMailParameter());
        services.AddSingleton(new ForemanMigrationParameter());
        services.AddSingleton(new GDParameter());
        services.AddSingleton(new CpcResourceFamilyParameters());
        services.AddSingleton(new CiawParameter());
        services.AddSingleton(new RFQParameter());
        services.AddSingleton(new TimeRegistrationParameter());
        services.AddSingleton(new ThAutomationParameter());
        services.AddSingleton(new PriceCalculatorParameter());
        services.AddSingleton(new ResourceMatrixParameter());
        services.AddSingleton(new BurndownChartParameter());
        services.AddSingleton(new MyCalenderParameter());
        services.AddSingleton(new WbsParameter());
        services.AddSingleton(new MecopParameter());
        services.AddSingleton(new IssueParameter());

        services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager();

        services.AddHttpContextAccessor();
        services.AddMvc(options => { options.Filters.Add<OperationCancelledExceptionFilter>(); });
        services.AddRazorPages();

        services.AddControllersWithViews();

        services.AddSingleton<ITenantSource, FileTenantSource>();
        services.AddScoped<ITenantProvider, WebTenantProvider>();

        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

        services.AddTransient<IStartupFilter, MigrationStartupFilter<UPrinceV4Context>>();

        services.AddScoped<IFileClient>(service =>
        {
            var provider = service.GetRequiredService<ITenantProvider>();
            var tenant = provider.GetTenant();

            return tenant.StorageType switch
            {
                "GoogleDrive" => new GoogleDriveFileClient(tenant.ConnectionString),
                "AzureBlob" => new AzureBlobStorageFileClient(tenant.ConnectionString),
                _ => null
            };
        });
        
        services.AddMicrosoftIdentityWebApiAuthentication(this.Configuration)
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(options => 
                Configuration.GetSection("DownstreamApis:MicrosoftGraph").Bind(options))
            .AddInMemoryTokenCaches();
        

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        services.AddResponseCaching();
        
        services.AddApplicationInsightsTelemetry(); //yq7y4cxdnng3imzpi64zzd0gecgv1z5hzu8fdjb5
        
        services.AddHttpLogging(logging =>
        {
            // if (_currentEnvironment.IsDevelopment())
            // {
                logging.LoggingFields = HttpLoggingFields.All;
            //}
        });
        services.AddRateLimiter(_ =>
        {
            _.OnRejected = (context, _) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int) retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken: _);

                return new ValueTask();
            };
            _.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var userAgent = httpContext.Request.Headers.UserAgent.ToString();

                    return RateLimitPartition.GetFixedWindowLimiter
                    (userAgent, _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 25,
                            Window = TimeSpan.FromSeconds(2)
                        });
                }),
                PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var userAgent = httpContext.Request.Headers.UserAgent.ToString();
            
                    return RateLimitPartition.GetFixedWindowLimiter
                    (userAgent, _ =>
                        new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 20,    
                            Window = TimeSpan.FromSeconds(30)
                        });
                }));
        });
        
        services.AddQuartz();
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "UPrince API",
                Description = "UPrince API",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Mickiesoft",
                    Url = new Uri("https://mickiesoft.com/contact/")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "OAuth2.0 Auth Code with PKCE",
                Name = "oauth2",
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(Configuration["AuthorizationUrl"]),
                        TokenUrl = new Uri(Configuration["TokenUrl"]),
                        Scopes = new Dictionary<string, string>
                        {
                            { Configuration["ApiScope"], "api://de2ed8fc-23b5-4c87-aef6-c00e38556018/access_as_user" }
                        }
                    }
                } 
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    new[] { Configuration["ApiScope"] }
                }
            });
            options.CustomSchemaIds(type => type.ToString());
        });
    }


    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "swaggerAADdemo v1");
            c.OAuthClientId(Configuration["OpenIdClientId"]);
            c.OAuthUsePkce();
            c.OAuthScopeSeparator(" ");
        });
      
        
        //app.UseStatusCodePagesWithReExecute("/Error/{0}");
        // app.UseExceptionHandler("/Error");
        app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
        app.UseMiddleware<MissingTenantMiddleware>(Configuration["MissingTenantUrl"]);
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // Map attribute-routed API controllers
            endpoints.MapDefaultControllerRoute(); // Map conventional MVC controllers using the default route
            endpoints.MapRazorPages();
        });

        app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod());

        // allow response caching directives in the API Controllers
        app.UseResponseCaching();
        
        app.UseHttpsRedirection();
        app.UseHttpLogging();
        
        app.UseRateLimiter();
    }
    
    public static Action<IApplicationBuilder> HandleApiException(ILoggerFactory loggerFactory)
    {
        return appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (exceptionHandlerFeature != null)
                {
                    var logger = loggerFactory.CreateLogger("Serilog Global exception logger");
                    logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                }

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An unexpected fault happened. Try again later.");

            });
        };
    }
}




public class MigrationStartupFilter<TContext> : IStartupFilter where TContext : DbContext
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                foreach (var context in scope.ServiceProvider.GetServices<TContext>())
                {
                    context.Database.SetCommandTimeout(160);
                    context.Database.Migrate();
                }
            }

            next(app);
        };
    }
}

public static class SqlCacheExtensions
{
    public static void ConfigureSqlCacheFromCommand(IConfiguration configuration)
    {
        //var process = new System.Diagnostics.Process()
        //{
        //    StartInfo = new ProcessStartInfo
        //    {
        //        FileName = "cmd.exe",
        //        Arguments = $"/c dotnet sql-cache create \"{configuration.GetConnectionString("TokenCacheDbConnStr")}\" dbo TokenCache",
        //        RedirectStandardOutput = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = false,
        //        WindowStyle = ProcessWindowStyle.Normal,
        //        RedirectStandardInput = true,
        //        RedirectStandardError = true
        //    }
        //};
        //process.Start();
        //string input = process.StandardError.ReadToEnd();
        //string result = process.StandardOutput.ReadToEnd();
        //process.WaitForExit();
    }
}