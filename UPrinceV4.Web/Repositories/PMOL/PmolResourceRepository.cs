using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.BOR;
using UPrinceV4.Web.Data.CAB;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Data.PMOL;
using UPrinceV4.Web.Data.VisualPlan;
using UPrinceV4.Web.Repositories.Interfaces;
using UPrinceV4.Web.Repositories.Interfaces.CPC;
using UPrinceV4.Web.Repositories.Interfaces.PMOL;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PMOL;

public class PmolResourceRepository : IPmolResourceRepository
{
    
    public async Task<string> CreateConsumable(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider);
            var consumable = new PmolPlannedWorkConsumable
            {
                CoperateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                CpcBasicUnitofMeasureId = parameter.ResourceCreateDto.CpcBasicUnitOfMeasureId,
                RequiredQuantity = parameter.ResourceCreateDto.Required,
                PmolId = parameter.ResourceCreateDto.PmolId,
                ConsumedQuantity = parameter.ResourceCreateDto.ConsumedQuantity,
                Type = parameter.ResourceCreateDto.Type,
                IsDeleted = false
            };
            if (parameter.ResourceCreateDto.Id == null)
            {
                if (parameter.ResourceCreateDto.Environment != "local") await CopyCpc(parameter, connectionString);
                consumable.Id = Guid.NewGuid().ToString();
                context.PMolPlannedWorkConsumable.Add(consumable);
                await context.SaveChangesAsync();
                var isExist = context.BorConsumable.FirstOrDefault(p =>
                    p.CorporateProductCatalogId == parameter.ResourceCreateDto.CorporateProductCatalogId &&
                    p.BorProductId == parameter.ResourceCreateDto.BorId);

                if (isExist == null)
                {
                    var borConsumable = new BorConsumable
                    {
                        BorProductId = parameter.ResourceCreateDto.BorId,
                        CorporateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                        Date = DateTime.UtcNow,
                        Required = parameter.ResourceCreateDto.Required,
                        TotalRequired = parameter.ResourceCreateDto.Required,
                        Id = Guid.NewGuid().ToString()
                    };
                    context.BorConsumable.Add(borConsumable);
                    await context.SaveChangesAsync();

                    var requiredConsumable = new BorRequiredConsumable
                    {
                        BorConsumableId = borConsumable.Id,
                        CpcId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                        Date = DateTime.UtcNow,
                        Quantity = parameter.ResourceCreateDto.Required,
                        Id = Guid.NewGuid().ToString()
                    };
                    context.BorRequiredConsumable.Add(requiredConsumable);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                consumable.Id = parameter.ResourceCreateDto.Id;
                context.PMolPlannedWorkConsumable.Update(consumable);
                await context.SaveChangesAsync();
            }

            return consumable.Id;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public async Task<string> CreateLabour(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);

            await using var connection = new SqlConnection(connectionString);

            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var labour = new PmolPlannedWorkLabour
                {
                    CoperateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                    CpcBasicUnitofMeasureId = parameter.ResourceCreateDto.CpcBasicUnitOfMeasureId,
                    RequiredQuantity = parameter.ResourceCreateDto.Required,
                    PmolId = parameter.ResourceCreateDto.PmolId,
                    ConsumedQuantity = parameter.ResourceCreateDto.ConsumedQuantity,
                    Type = parameter.ResourceCreateDto.Type,
                    IsDeleted = false
                };
                if (parameter.ResourceCreateDto.Id == null)
                {
                    if (parameter.ResourceCreateDto.Environment != "local") await CopyCpc(parameter, connectionString);

                    labour.Id = Guid.NewGuid().ToString();
                    context.PMolPlannedWorkLabour.Add(labour);
                    await context.SaveChangesAsync();
                    var isExist = context.BorLabour.FirstOrDefault(p =>
                        p.CorporateProductCatalogId == parameter.ResourceCreateDto.CorporateProductCatalogId &&
                        p.BorProductId == parameter.ResourceCreateDto.BorId);

                    if (isExist == null)
                    {
                        var borLabour = new BorLabour
                        {
                            BorProductId = parameter.ResourceCreateDto.BorId,
                            CorporateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                            Date = DateTime.UtcNow,
                            Required = parameter.ResourceCreateDto.Required,
                            TotalRequired = parameter.ResourceCreateDto.Required,
                            Id = Guid.NewGuid().ToString()
                        };
                        context.BorLabour.Add(borLabour);
                        await context.SaveChangesAsync();

                        var requiredlabour = new BorRequiredLabour
                        {
                            BorLabourId = borLabour.Id,
                            CpcId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                            Date = DateTime.UtcNow,
                            Quantity = parameter.ResourceCreateDto.Required,
                            Id = Guid.NewGuid().ToString()
                        };
                        context.BorRequiredLabour.Add(requiredlabour);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    labour.Id = parameter.ResourceCreateDto.Id;
                    context.PMolPlannedWorkLabour.Update(labour);
                    await context.SaveChangesAsync();
                }

                var pmol = connection.Query<Pmol>("SELECT * FROM dbo.PMol WHERE Id = @PmolId",
                    new { parameter.ResourceCreateDto.PmolId }).FirstOrDefault();

                var startTime = "06:30";
                var endTime = "08:30";

                if (pmol.ExecutionStartTime != null) startTime = pmol.ExecutionStartTime;
                if (pmol.ExecutionEndTime != null) endTime = pmol.ExecutionEndTime;

                await using var dbconnection = new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString);

                if (parameter.ResourceCreateDto.OrganizationTeamId == null)
                {
                    var organizationteam = dbconnection
                        .Query<OrganizationTeamPmol>("SELECT * FROM OrganizationTeamPmol WHERE PmolId = @PmolId",
                            new { parameter.ResourceCreateDto.PmolId }).FirstOrDefault();

                    string teamId = null;

                    var newTeam =
                        @"INSERT INTO dbo.OrganizationTaxonomy (Id, Title, ParentId, OrganizationTaxonomyLevelId, PersonId, RoleId, ModifiedDate,StartDate,EndDate,TemporaryTeamNameId) VALUES (@Id, @Title, @ParentId, @OrganizationTaxonomyLevelId, @PersonId, @RoleId, @ModifiedDate,@StartDate,@EndDate,@TemporaryTeamNameId)";

                    if (organizationteam == null)
                    {
                        if (pmol.ExecutionDate != null)
                        {
                            var parent = dbconnection
                                .Query<OrganizationTaxonomy>(
                                    "SELECT * FROM dbo.OrganizationTaxonomy WHERE Title = @Cu",
                                    new { Cu = parameter.ContractingUnitSequenceId })
                                .FirstOrDefault();

                            var selectBu = @"SELECT
                                  ProjectClassification.ProjectClassificationBuisnessUnit
                                FROM dbo.ProjectDefinition
                                LEFT OUTER JOIN dbo.ProjectClassification
                                  ON ProjectDefinition.Id = ProjectClassification.ProjectId
                                WHERE ProjectDefinition.SequenceCode = @Project";

                            var buId = dbconnection
                                .Query<string>(selectBu, new { Project = parameter.ProjectSequenceId })
                                .FirstOrDefault();

                            var baseDate = (DateTime)pmol.ExecutionDate;

                            var param1 = new
                            {
                                Id = Guid.NewGuid().ToString(),
                                parent.Title,
                                ParentId = buId,
                                OrganizationTaxonomyLevelId = "fg10e768-3e06-po02-b337-ee367a82adfg",
                                PersonId = "",
                                RoleId = "",
                                ModifiedDate = DateTime.UtcNow,
                                StartDate = baseDate.AddDays(-(int)baseDate.DayOfWeek).Date,
                                EndDate = baseDate.AddDays(-(int)baseDate.DayOfWeek).AddDays(7).Date,
                                TemporaryTeamNameId = "7bcb4e8d-8e8c-487d-team-6b91c89fAcce"
                            };

                            teamId = param1.Id;

                            await dbconnection.ExecuteAsync(newTeam, param1);

                            var sql =
                                @"INSERT INTO dbo.OrganizationTeamPmol ( Id ,OrganizationTeamId ,PmolId ,ExecutionDate ,StartTime ,EndTime ,ContractingUnit ,Project ) VALUES ( @Id ,@OrganizationTeamId ,@PmolId ,@ExecutionDate ,@StartTime ,@EndTime ,@ContractingUnit ,@Project ); ";

                            var param2 = new
                            {
                                Id = Guid.NewGuid(), OrganizationTeamId = param1.Id,
                                ContractingUnit = parameter.ContractingUnitSequenceId,
                                Project = parameter.ProjectSequenceId,
                                parameter.ResourceCreateDto.PmolId,
                                pmol.ExecutionDate, StartTime = startTime, EndTime = endTime
                            };

                            await dbconnection.ExecuteAsync(sql, param2);
                        }
                    }

                    else
                    {
                        teamId = organizationteam.Id;
                    }

                    var mTeamMemberDto = new List<TeamMemberDto>();

                    var nullCheck = parameter.ResourceCreateDto.TeamRoleList == null;
                    if (nullCheck == false)
                        foreach (var team in parameter.ResourceCreateDto.TeamRoleList)
                        {
                            var cabPerson = dbconnection
                                .Query<CabPerson>(
                                    "SELECT CabPerson.FullName ,CabPersonCompany.Id FROM dbo.CabPersonCompany INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE CabPerson.Id = @Id",
                                    new { Id = team.CabPersonId }).FirstOrDefault();
                            if (teamId != null)
                            {
                                var param = new
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Title = cabPerson.FullName,
                                    ParentId = teamId,
                                    OrganizationTaxonomyLevelId = "we10e768-3e06-po02-b337-ee367a82adwe",
                                    PersonId = cabPerson.Id,
                                    RoleId = "2210e768-msms-Item-team2-ee367a82ad22",
                                    ModifiedDate = DateTime.UtcNow,
                                    StartDate = "",
                                    EndDate = "",
                                    TemporaryTeamNameId = ""
                                };
                                await dbconnection.ExecuteAsync(newTeam, param);
                            }

                            var pmolTeamRole = new PmolTeamRole
                            {
                                CabPersonId = team.CabPersonId,
                                ConsumedQuantity = team.ConsumedQuantity,
                                PmolLabourId = labour.Id,
                                RequiredQuantity = team.RequiredQuantity,
                                RoleId = team.RoleId,
                                Type = parameter.ResourceCreateDto.Type
                            };

                            if (team.Id == null)
                            {
                                if (team.RoleId == "Foreman")
                                {
                                    var isForemanExists = connection.Query<PmolTeamRole>(
                                        @"SELECT PmolTeamRole.* FROM PmolTeamRole LEFT OUTER JOIN PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.RoleId = 'Foreman' AND PmolTeamRole.IsDeleted = 0 ",
                                        new { parameter.ResourceCreateDto.PmolId }).Any();

                                    if (!isForemanExists)
                                    {
                                        //throw new Exception("Foreman Already Exists");
                                        pmolTeamRole.Id = Guid.NewGuid().ToString();
                                        context.PmolTeamRole.Add(pmolTeamRole);
                                        await context.SaveChangesAsync();

                                        await connection.ExecuteAsync(
                                            "Update PMol Set ForemanId = @ForemanId Where Id = @Id",
                                            new
                                            {
                                                ForemanId = team.CabPersonId, Id = parameter.ResourceCreateDto.PmolId
                                            });
                                    }
                                }
                                else
                                {
                                    pmolTeamRole.Id = Guid.NewGuid().ToString();
                                    context.PmolTeamRole.Add(pmolTeamRole);
                                    await context.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                pmolTeamRole.Id = team.Id;
                                context.PmolTeamRole.Update(pmolTeamRole);
                                await context.SaveChangesAsync();
                            }

                            if (team.Id == null)
                            {
                                var mTeam = new TeamMemberDto()
                                {
                                    PersonId = team.CabPersonId,
                                    CabPersonCompanyId = cabPerson.Id,
                                    IsExist = false,
                                    RoleId = team.RoleId
                                };
                                mTeamMemberDto.Add(mTeam);
                            }
                            else
                            {
                                var mTeam = new TeamMemberDto()
                                {
                                    PersonId = team.CabPersonId,
                                    CabPersonCompanyId = cabPerson.Id,
                                    IsExist = true,
                                    RoleId = team.RoleId
                                };
                                mTeamMemberDto.Add(mTeam);
                            }
                        }

                    if (pmol.ExecutionDate != null && mTeamMemberDto.Count > 0)
                    {
                        var vpParameter = new VPParameter();
                        vpParameter.TenantProvider = parameter.TenantProvider;

                        var mAddMutipleTeamMembers = new AddMutipleTeamMembers()
                        {
                            ProjectSequenceCode = parameter.ProjectSequenceId,
                            ContractingUnit = parameter.ContractingUnitSequenceId,
                            PmolId = pmol.Id,
                            ExecutionDate = (DateTime)pmol.ExecutionDate,
                            Team = mTeamMemberDto
                        };
                        vpParameter.AddMutipleTeamMembers = mAddMutipleTeamMembers;
                        vpParameter.Configuration = parameter.Configuration;

                        await parameter.VpRepository.AddMultipleMembersToPmol(vpParameter, true);
                    }
                }
                else
                {
                    var organizationTeam = dbconnection.Query<OrganizationTeamPmol>("SELECT * FROM OrganizationTeamPmol WHERE PmolId = @Id",
                        new { Id = parameter.ResourceCreateDto.PmolId}).FirstOrDefault();

                    if (organizationTeam == null)
                    {
                        var sql =
                            @"INSERT INTO dbo.OrganizationTeamPmol ( Id ,OrganizationTeamId ,PmolId ,ExecutionDate ,StartTime ,EndTime ,ContractingUnit ,Project ) VALUES ( @Id ,@OrganizationTeamId ,@PmolId ,@ExecutionDate ,@StartTime ,@EndTime ,@ContractingUnit ,@Project ); ";

                        var param2 = new
                        {
                            Id = Guid.NewGuid(), OrganizationTeamId = parameter.ResourceCreateDto.OrganizationTeamId,
                            ContractingUnit = parameter.ContractingUnitSequenceId,
                            Project = parameter.ProjectSequenceId,
                            parameter.ResourceCreateDto.PmolId,
                            pmol.ExecutionDate, StartTime = startTime, EndTime = endTime
                        };

                        await dbconnection.ExecuteAsync(sql, param2);
                    }
                    
                }

                return labour.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateMaterial(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var material = new PmolPlannedWorkMaterial
                {
                    CoperateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                    CpcBasicUnitofMeasureId = parameter.ResourceCreateDto.CpcBasicUnitOfMeasureId,
                    RequiredQuantity = parameter.ResourceCreateDto.Required,
                    PmolId = parameter.ResourceCreateDto.PmolId,
                    ConsumedQuantity = parameter.ResourceCreateDto.ConsumedQuantity,
                    Type = parameter.ResourceCreateDto.Type,
                    IsDeleted = false
                };
                if (parameter.ResourceCreateDto.Id == null)
                {
                    if (parameter.ResourceCreateDto.Environment != "local") await CopyCpc(parameter, connectionString);

                    material.Id = Guid.NewGuid().ToString();
                    context.PMolPlannedWorkMaterial.Add(material);
                    await context.SaveChangesAsync();


                    var isExist = context.BorMaterial.FirstOrDefault(p =>
                        p.CorporateProductCatalogId == parameter.ResourceCreateDto.CorporateProductCatalogId &&
                        p.BorProductId == parameter.ResourceCreateDto.BorId);

                    if (isExist == null)
                    {
                        var mBorMaterial = new BorMaterial
                        {
                            BorProductId = parameter.ResourceCreateDto.BorId,
                            CorporateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                            Date = DateTime.UtcNow,
                            Required = parameter.ResourceCreateDto.Required,
                            TotalRequired = parameter.ResourceCreateDto.Required,
                            Id = Guid.NewGuid().ToString()
                        };
                        context.BorMaterial.Add(mBorMaterial);
                        await context.SaveChangesAsync();

                        var requiredMaterial = new BorRequiredMaterial
                        {
                            BorMaterialId = mBorMaterial.Id,
                            CpcId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                            Date = DateTime.UtcNow,
                            Quantity = parameter.ResourceCreateDto.Required,
                            Id = Guid.NewGuid().ToString()
                        };
                        context.BorRequiredMaterial.Add(requiredMaterial);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    material.Id = parameter.ResourceCreateDto.Id;
                    context.PMolPlannedWorkMaterial.Update(material);
                    await context.SaveChangesAsync();
                }

                return material.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateTools(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var tool = new PmolPlannedWorkTools
                {
                    CoperateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                    CpcBasicUnitofMeasureId = parameter.ResourceCreateDto.CpcBasicUnitOfMeasureId,
                    RequiredQuantity = parameter.ResourceCreateDto.Required,
                    PmolId = parameter.ResourceCreateDto.PmolId,
                    ConsumedQuantity = parameter.ResourceCreateDto.ConsumedQuantity,
                    Type = parameter.ResourceCreateDto.Type,
                    IsDeleted = false
                };
                if (parameter.ResourceCreateDto.Id == null)
                {
                    if (parameter.ResourceCreateDto.Environment != "local") await CopyCpc(parameter, connectionString);

                    tool.Id = Guid.NewGuid().ToString();
                    context.PMolPlannedWorkTools.Add(tool);
                    await context.SaveChangesAsync();

                    var isExist = context.BorTools.FirstOrDefault(p =>
                        p.CorporateProductCatalogId == parameter.ResourceCreateDto.CorporateProductCatalogId &&
                        p.BorProductId == parameter.ResourceCreateDto.BorId);

                    if (isExist == null)
                    {
                        var borTools = new BorTools
                        {
                            BorProductId = parameter.ResourceCreateDto.BorId,
                            CorporateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                            Date = DateTime.UtcNow,
                            Required = parameter.ResourceCreateDto.Required,
                            TotalRequired = parameter.ResourceCreateDto.Required,
                            Id = Guid.NewGuid().ToString()
                        };
                        context.BorTools.Add(borTools);
                        await context.SaveChangesAsync();

                        var requiredtool = new BorRequiredTools
                        {
                            BorToolsId = borTools.Id,
                            CpcId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                            Date = DateTime.UtcNow,
                            Quantity = parameter.ResourceCreateDto.Required,
                            Id = Guid.NewGuid().ToString()
                        };
                        context.BorRequiredTools.Add(requiredtool);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    tool.Id = parameter.ResourceCreateDto.Id;
                    context.PMolPlannedWorkTools.Update(tool);
                    await context.SaveChangesAsync();
                }

                return tool.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadConsumable(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                               SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkConsumable con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);

                foreach (var pR in result)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Any()) pR.Title = pR.ResourceNumber + "-" + resultNickNames.First().NickName;
                }

                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadMaterial(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query =
                @"SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkMaterial con
                                INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId IS NULL)
                                AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0";

            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);

                foreach (var pR in result)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Any()) pR.Title = pR.ResourceNumber + "-" + resultNickNames.First().NickName;
                }


                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadLabour(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                               SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                               FROM PMolPlannedWorkLabour con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                               LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                               WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                               AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);

                foreach (var pR in result)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Any()) pR.Title = pR.ResourceNumber + "-" + resultNickNames.First().NickName;
                }

                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadTools(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId ,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkTools con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);

                foreach (var pR in result)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Any()) pR.Title = pR.ResourceNumber + "-" + resultNickNames.First().NickName;
                }

                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadToolsForDayPlanning(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId ,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkTools con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=cor.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR cor.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Planned' AND con.IsDeleted=0 AND cor.ResourceFamilyId = '0c355800-91fd-4d99-8010-921a42f0ba04'
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);

                foreach (var pR in result)
                {
                    var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                        new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                    if (resultNickNames.Any()) pR.Title = pR.ResourceNumber + "-" + resultNickNames.First().NickName;
                }

                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> ReadConsumableFromBor(PmolResourceParameter parameter, string PmolId)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                               select CorporateProductCatalogId, SUM(TotalRequired) AS Required, CorporateProductCatalog.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, CpcBasicUnitOfMeasureLocalizedData.Id AS CpcBasicUnitOfMeasureId 
                                from BorConsumable 
                                LEFT OUTER JOIN CorporateProductCatalog ON CorporateProductCatalog.Id = BorConsumable.CorporateProductCatalogId
								LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
								where BorProductId = @id
								AND ( CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR  CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                                group by CorporateProductCatalogId,CorporateProductCatalog.ResourceNumber, CorporateProductCatalog.ResourceTitle,  CpcBasicUnitOfMeasureLocalizedData.Label, CpcBasicUnitOfMeasureLocalizedData.Id;
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            IEnumerable<PmolResourceByBorIdDto> result;
            using (var dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.Query<PmolResourceByBorIdDto>(query, parameters);
                if (!parameter.isWeb)
                    if (lang != "nl")
                        foreach (var pR in result)
                        {
                            var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                                new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                            if (resultNickNames.Count() > 0) pR.Title = resultNickNames.First().NickName;
                        }

                
            }

            foreach (var dto in result)
            {
                var createDto = new PmolResourceCreateDto();
                createDto.CorporateProductCatalogId = dto.CorporateProductCatalogId;
                createDto.CpcBasicUnitOfMeasureId = dto.CpcBasicUnitOfMeasureId;
                createDto.Environment = "local";
                createDto.Id = null;
                createDto.PmolId = PmolId;
                createDto.Required = dto.Required;
                createDto.ConsumedQuantity = 0;
                createDto.Type = "Planned";
                parameter.ResourceCreateDto = createDto;
                await CreateConsumable(parameter);
            }

            return "";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> ReadLabourFromBor(PmolResourceParameter parameter, string PmolId)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                               select CorporateProductCatalogId, SUM(TotalRequired) AS Required, CorporateProductCatalog.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, CpcBasicUnitOfMeasureLocalizedData.Id AS CpcBasicUnitOfMeasureId
                                from BorLabour 
                                LEFT OUTER JOIN CorporateProductCatalog ON CorporateProductCatalog.Id = BorLabour.CorporateProductCatalogId
								LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
								where BorProductId = @id
								AND ( CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR  CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                                group by CorporateProductCatalogId,CorporateProductCatalog.ResourceNumber, CorporateProductCatalog.ResourceTitle,  CpcBasicUnitOfMeasureLocalizedData.Label, CpcBasicUnitOfMeasureLocalizedData.Id;
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };
            IEnumerable<PmolResourceByBorIdDto> result;

            using (var dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.Query<PmolResourceByBorIdDto>(query, parameters);
                if (!parameter.isWeb)
                    if (lang != "nl")
                        foreach (var pR in result)
                        {
                            var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                                new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                            if (resultNickNames.Count() > 0) pR.Title = resultNickNames.First().NickName;
                        }

                
            }

            foreach (var dto in result)
            {
                var createDto = new PmolResourceCreateDto();
                createDto.CorporateProductCatalogId = dto.CorporateProductCatalogId;
                createDto.CpcBasicUnitOfMeasureId = dto.CpcBasicUnitOfMeasureId;
                createDto.Environment = "local";
                createDto.Id = null;
                createDto.PmolId = PmolId;
                createDto.Required = dto.Required;
                createDto.ConsumedQuantity = 0;
                createDto.Type = "Planned";
                parameter.ResourceCreateDto = createDto;
                await CreateLabour(parameter);
            }

            return "";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> ReadMaterialFromBor(PmolResourceParameter parameter, string PmolId)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                                 select CorporateProductCatalogId, SUM(TotalRequired) AS Required, CorporateProductCatalog.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, CpcBasicUnitOfMeasureLocalizedData.Id AS CpcBasicUnitOfMeasureId
                                from BorMaterial 
                                LEFT OUTER JOIN CorporateProductCatalog ON CorporateProductCatalog.Id = BorMaterial.CorporateProductCatalogId
								LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
								where BorProductId = @id
								AND ( CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR  CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                                group by CorporateProductCatalogId,CorporateProductCatalog.ResourceNumber, CorporateProductCatalog.ResourceTitle,  CpcBasicUnitOfMeasureLocalizedData.Label, CpcBasicUnitOfMeasureLocalizedData.Id;
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };
            IEnumerable<PmolResourceByBorIdDto> result;

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.Query<PmolResourceByBorIdDto>(query, parameters);
                if (!parameter.isWeb)
                    if (lang != "nl")
                        foreach (var pR in result)
                        {
                            var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                                new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                            if (resultNickNames.Count() > 0) pR.Title = resultNickNames.First().NickName;
                        }

                
            }

            foreach (var dto in result)
            {
                var createDto = new PmolResourceCreateDto();
                createDto.CorporateProductCatalogId = dto.CorporateProductCatalogId;
                createDto.CpcBasicUnitOfMeasureId = dto.CpcBasicUnitOfMeasureId;
                createDto.Environment = "local";
                createDto.Id = null;
                createDto.PmolId = PmolId;
                createDto.Required = dto.Required;
                createDto.ConsumedQuantity = 0;
                createDto.Type = "Planned";
                parameter.ResourceCreateDto = createDto;
                await CreateMaterial(parameter);
            }

            return "";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> ReadToolsFromBor(PmolResourceParameter parameter, string PmolId)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                               select CorporateProductCatalogId, SUM(TotalRequired) AS Required, CorporateProductCatalog.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, CpcBasicUnitOfMeasureLocalizedData.Id AS CpcBasicUnitOfMeasureIdg
                                from BorTools
                                LEFT OUTER JOIN CorporateProductCatalog ON CorporateProductCatalog.Id = BorTools.CorporateProductCatalogId
								LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData on CorporateProductCatalog.CpcBasicUnitOfMeasureId = CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId
								where BorProductId = @id
								AND ( CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR  CorporateProductCatalog.CpcBasicUnitOfMeasureId is null)
                                group by CorporateProductCatalogId,CorporateProductCatalog.ResourceNumber, CorporateProductCatalog.ResourceTitle,  CpcBasicUnitOfMeasureLocalizedData.Label, CpcBasicUnitOfMeasureLocalizedData.Id
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };
            IEnumerable<PmolResourceByBorIdDto> result;

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                result = dbConnection.Query<PmolResourceByBorIdDto>(query, parameters);
                if (!parameter.isWeb)
                    if (lang != "nl")
                        foreach (var pR in result)
                        {
                            var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                                new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                            if (resultNickNames.Count() > 0) pR.Title = resultNickNames.First().NickName;
                        }

                
            }

            foreach (var dto in result)
            {
                var createDto = new PmolResourceCreateDto();
                createDto.CorporateProductCatalogId = dto.CorporateProductCatalogId;
                createDto.CpcBasicUnitOfMeasureId = dto.CpcBasicUnitOfMeasureId;
                createDto.Environment = "local";
                createDto.Id = null;
                createDto.PmolId = PmolId;
                createDto.Required = dto.Required;
                createDto.ConsumedQuantity = 0;
                createDto.Type = "Planned";
                parameter.ResourceCreateDto = createDto;
                await CreateTools(parameter);
            }

            return "";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadExtraConsumable(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                               SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkConsumable con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=con.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR con.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Extra' AND con.IsDeleted=0
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);
                if (!parameter.isWeb)
                    if (lang != "nl")
                        foreach (var pR in result)
                        {
                            var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                                new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                            if (resultNickNames.Count() > 0) pR.Title = resultNickNames.First().NickName;
                        }

                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadExtraMaterial(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"  
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkMaterial con
                                INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=con.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR con.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Extra' AND con.IsDeleted=0
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);
                if (!parameter.isWeb)
                    if (lang != "nl")
                        foreach (var pR in result)
                        {
                            var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                                new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                            if (resultNickNames.Count() > 0) pR.Title = resultNickNames.First().NickName;
                        }

                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadExtraLabour(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                               SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                               FROM PMolPlannedWorkLabour con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                               LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=con.CpcBasicUnitofMeasureId
                               WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR con.CpcBasicUnitofMeasureId is null)
                               AND con.PmolId = @id AND con.Type='Extra' AND con.IsDeleted=0
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);
                if (!parameter.isWeb)
                    if (lang != "nl")
                        foreach (var pR in result)
                        {
                            var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                                new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                            if (resultNickNames.Count() > 0) pR.Title = resultNickNames.First().NickName;
                        }

                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolResourceReadDto>> ReadExtraTools(PmolResourceParameter parameter)
    {
        try
        {
            var lang = parameter.Lang;

            var query = @"
                                SELECT cor.Id AS CorporateProductCatalogId, con.RequiredQuantity AS Required, cor.Title AS Title, CpcBasicUnitOfMeasureLocalizedData.Label AS Unit, con.CpcBasicUnitofMeasureId AS cpcBasicUnitOfMeasureId ,
                                con.ConsumedQuantity AS ConsumedQuantity, con.Id AS Id, con.PmolId AS PmolId, cor.ResourceNumber
                                FROM PMolPlannedWorkTools con
                               INNER JOIN CorporateProductCatalog cor ON con.CoperateProductCatalogId=cor.Id
                                LEFT OUTER JOIN CpcBasicUnitOfMeasureLocalizedData ON CpcBasicUnitOfMeasureLocalizedData.CpcBasicUnitOfMeasureId=con.CpcBasicUnitofMeasureId
                                WHERE (CpcBasicUnitOfMeasureLocalizedData.LanguageCode=@lang OR con.CpcBasicUnitofMeasureId is null)
                                AND con.PmolId = @id AND con.Type='Extra' AND con.IsDeleted=0
                             ";
            var sqlNickName =
                @"SELECT * FROM dbo.CpcResourceNickname WHERE CoperateProductCatalogId = @CoperateProductCatalogId AND LocaleCode LIKE '%" +
                lang + "%'";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id, lang };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                var result = dbConnection.Query<PmolResourceReadDto>(query, parameters);
                if (!parameter.isWeb)
                    if (lang != "nl")
                        foreach (var pR in result)
                        {
                            var resultNickNames = dbConnection.Query<CpcResourceNickname>(sqlNickName,
                                new { CoperateProductCatalogId = pR.CorporateProductCatalogId });
                            if (resultNickNames.Count() > 0) pR.Title = resultNickNames.First().NickName;
                        }

                

                return result;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> DeleteConsumable(PmolResourceParameter parameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var query = "update PMolPlannedWorkConsumable set IsDeleted=1 where Id = @Id";
            foreach (var id in parameter.IdList)
                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = connection.ExecuteAsync(query, new { Id = id }).Result;
                    connection.Close();
                }

            return "Ok";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> DeleteLabour(PmolResourceParameter parameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var query = "update PMolPlannedWorkLabour set IsDeleted=1 where Id = @Id";

            var pmolQuery =
                "SELECT PMol.Id,PMol.ProjectMoleculeId AS PmolId,PmolTeamRole.CabPersonId,PMol.ExecutionDate,PmolTeamRole.RoleId FROM dbo.PmolTeamRole INNER JOIN dbo.PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id INNER JOIN dbo.PMol ON PMolPlannedWorkLabour.PmolId = PMol.Id WHERE PMolPlannedWorkLabour.Id = @Id";


            foreach (var id in parameter.IdList)
            {
                await using var connection = new SqlConnection(connectionString);
                var affectedRows = connection.ExecuteAsync(query, new { Id = id }).Result;

                var teamRole = connection.Query<PmolTeamRoleDelete>(pmolQuery, new { Id = id });

                if (teamRole.Any(x => x.RoleId == "Foreman"))
                {
                    await connection.ExecuteAsync("UPDATE PMol SET ForemanId = null Where Id = @Id ",
                        new { Id = teamRole.FirstOrDefault()?.Id });
                }

                connection.Close();
            }
            return "Ok";

        }
        
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> DeleteMaterial(PmolResourceParameter parameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var query = "update PMolPlannedWorkMaterial set IsDeleted=1 where Id = @Id";
            foreach (var id in parameter.IdList)
                await using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = connection.ExecuteAsync(query, new { Id = id }).Result;
                    connection.Close();
                }

            return "Ok";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> DeleteTools(PmolResourceParameter parameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var query = "update PMolPlannedWorkTools set IsDeleted=1 where Id = @Id";
            foreach (var id in parameter.IdList)
                await using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = connection.ExecuteAsync(query, new { Id = id }).Result;
                    connection.Close();
                }

            return "Ok";
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public async Task<string> CreateTeamRole(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var role = new PmolTeamRole
                {
                    CabPersonId = parameter.pmolTeamCreateDto.CabPersonId,
                    ConsumedQuantity = parameter.pmolTeamCreateDto.ConsumedQuantity,
                    PmolLabourId = parameter.pmolTeamCreateDto.PmolLabourId,
                    RequiredQuantity = parameter.pmolTeamCreateDto.RequiredQuantity,
                    RoleId = parameter.pmolTeamCreateDto.RoleId,
                    Type = parameter.pmolTeamCreateDto.Type
                };
                if (parameter.pmolTeamCreateDto.Id == null)
                {
                    role.Id = Guid.NewGuid().ToString();
                    context.PmolTeamRole.Add(role);
                    await context.SaveChangesAsync();
                }
                else
                {
                    role.Id = parameter.pmolTeamCreateDto.Id;
                    context.PmolTeamRole.Update(role);
                    await context.SaveChangesAsync();
                }

                return role.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolTeamRoleReadDto>> ReadExtraTeamMember(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options, parameter.TenantProvider);
            var lang = parameter.Lang;
            IEnumerable<PmolTeamRoleReadDto> teamRoleDto;

            // var query = @"
            //                    select PmolTeamRole.CabPersonId, PmolTeamRole.ConsumedQuantity, PmolTeamRole.Id,PmolTeamRole.RequiredQuantity,PmolTeamRole.RoleId, Role.RoleName
            //                     from PmolTeamRole
            //                     Left Outer Join Role on PmolTeamRole.RoleId=Role.Id
            //                     where PmolTeamRole.Type = 'Extra' 
            //                     AND PmolTeamRole.PmolLabourId=@id AND PmolTeamRole.IsDeleted = 0
            //                  ";

            var query = @"
                               select PmolTeamRole.CabPersonId, PmolTeamRole.ConsumedQuantity, PmolTeamRole.Id,PmolTeamRole.RequiredQuantity,PmolTeamRole.RoleId
                                from PmolTeamRole
                                where PmolTeamRole.Type = 'Extra' 
                                AND PmolTeamRole.PmolLabourId=@id AND PmolTeamRole.IsDeleted = 0
                             ";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                teamRoleDto = dbConnection.Query<PmolTeamRoleReadDto>(query, parameters);
                
            }

            var cabPersons = applicationDbContext.CabPerson.ToList();

            foreach (var dto in teamRoleDto)
            {
                var person = cabPersons.Where(p => p.Id == dto.CabPersonId).FirstOrDefault();
                dto.CabPerson = person.FirstName + " " + person.Surname;
                dto.RoleName = applicationDbContext.Role
                    .Where(x => x.RoleId == dto.RoleId && x.LanguageCode == parameter.Lang).Select(v => v.RoleName)
                    .FirstOrDefault();
            }

            return teamRoleDto;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolTeamRoleReadDto>> ReadPlannedTeamMember(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options, parameter.TenantProvider);
            var lang = parameter.Lang;

            IEnumerable<PmolTeamRoleReadDto> teamRoleDto;
            // var query = @"
            //                    select PmolTeamRole.CabPersonId, PmolTeamRole.ConsumedQuantity, PmolTeamRole.Id,PmolTeamRole.RequiredQuantity,PmolTeamRole.RoleId, Role.RoleName
            //                     from PmolTeamRole
            //                     Left Outer Join Role on PmolTeamRole.RoleId=Role.Id
            //                     where PmolTeamRole.Type = 'Planned' 
            //                     AND PmolTeamRole.PmolLabourId=@id AND PmolTeamRole.IsDeleted = 0
            //                  ";

            var query = @"
                               select PmolTeamRole.CabPersonId, PmolTeamRole.ConsumedQuantity, PmolTeamRole.Id,PmolTeamRole.RequiredQuantity,PmolTeamRole.RoleId
                                from PmolTeamRole
                                where PmolTeamRole.Type = 'Planned' 
                                AND PmolTeamRole.PmolLabourId=@id AND PmolTeamRole.IsDeleted = 0
                             ";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { id = parameter.Id };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                teamRoleDto = dbConnection.Query<PmolTeamRoleReadDto>(query, parameters);
                
            }

            var cabPersons = applicationDbContext.CabPerson.ToList();
            foreach (var dto in teamRoleDto)
            {
                var person = cabPersons.Where(p => p.Id == dto.CabPersonId).FirstOrDefault();
                dto.CabPerson = person.FirstName + " " + person.Surname;
                dto.RoleName = applicationDbContext.Role
                    .Where(x => x.RoleId == dto.RoleId && x.LanguageCode == parameter.Lang).Select(v => v.RoleName)
                    .FirstOrDefault();

                await using var connection = new SqlConnection(connectionString);
                dto.StartDateTime = connection
                    .Query<PmolLabourTime>(
                        "Select * From PmolLabourTime Where LabourId = @LabourId AND StartDateTime IS NOT NULL Order By StartDateTime ASC",
                        new { LabourId = dto.Id })
                    .FirstOrDefault()?.StartDateTime;
                dto.EndDateTime = connection
                    .Query<PmolLabourTime>(
                        "Select * From PmolLabourTime Where LabourId = @LabourId  AND Type = '8'",
                        new { LabourId = dto.Id })
                    .FirstOrDefault()?.EndDateTime;
            }

            return teamRoleDto;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> DeleteTeamMember(PmolResourceParameter parameter)
    {
        try
        {
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var cuConnectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                null, parameter.TenantProvider);
            var query = "update PmolTeamRole set IsDeleted = 1 where Id = @Id";
            var deletequery = "DELETE FROM PmolLabourTeams WHERE PersonId = @PersonId AND PmolId = @pmolId";
            var personQuery = "SELECT Id from CabPersonCompany WHERE PersonId = @PersonId";
            var pmolQuery =
                "SELECT PMol.Id,PMol.ProjectMoleculeId AS PmolId,PmolTeamRole.CabPersonId,PMol.ExecutionDate,PmolTeamRole.RoleId FROM dbo.PmolTeamRole INNER JOIN dbo.PMolPlannedWorkLabour ON PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id INNER JOIN dbo.PMol ON PMolPlannedWorkLabour.PmolId = PMol.Id WHERE PmolTeamRole.Id = @Id";

            PmolTeamRoleDelete teamRole = null;
            string personCompanyId;
            foreach (var id in parameter.IdList)
            {
                await using var connection = new SqlConnection(connectionString);

                var affectedRows = connection.ExecuteAsync(query, new { Id = id }).Result;
                teamRole = connection.Query<PmolTeamRoleDelete>(pmolQuery, new { Id = id }).FirstOrDefault();


                if (teamRole != null)
                {
                    await using (var connection2 =
                                 new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString))
                    {
                        personCompanyId = connection2
                            .Query<string>(personQuery, new { PersonId = teamRole.CabPersonId })
                            .FirstOrDefault();
                    }

                    await connection.ExecuteAsync(deletequery,
                        new { PersonId = personCompanyId, pmolId = teamRole.PmolId });

                    if (teamRole.RoleId == "Foreman")
                    {
                        await connection.ExecuteAsync("UPDATE PMol SET ForemanId = null Where Id = @Id ",
                            new { Id = teamRole.Id });
                    }
                }


                var removePersonFromPmol = new RemovePersonFromPmol()
                {
                    CuConnectionString = cuConnectionString,
                    ProjectConnectionString = connectionString,
                    PmolId = teamRole.Id,
                    CabPersonId = teamRole.CabPersonId,
                    ExecutionDate = teamRole.ExecutionDate.Value
                };

                parameter.RemovePersonFromPmol = removePersonFromPmol;
                await RemovePersonFromPmol(parameter);
            }


            return "Ok";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateMaterialForMobile(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var material = new PmolPlannedWorkMaterial();
                material.CoperateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId;
                material.CpcBasicUnitofMeasureId = parameter.ResourceCreateDto.CpcBasicUnitOfMeasureId;
                material.RequiredQuantity = parameter.ResourceCreateDto.Required;
                material.PmolId = parameter.ResourceCreateDto.PmolId;
                material.ConsumedQuantity = parameter.ResourceCreateDto.ConsumedQuantity;
                material.Type = parameter.ResourceCreateDto.Type;
                material.IsDeleted = false;
                material.Id = parameter.ResourceCreateDto.Id;
                var isExist = context.PMolPlannedWorkMaterial.Any(p => p.Id == parameter.ResourceCreateDto.Id);
                if (isExist == false)
                {
                    if (parameter.ResourceCreateDto.Environment != "local") CopyCpc(parameter, connectionString);

                    context.PMolPlannedWorkMaterial.Add(material);
                    await context.SaveChangesAsync();
                }
                else
                {
                    context.PMolPlannedWorkMaterial.Update(material);
                    await context.SaveChangesAsync();
                }

                return material.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateLabourForMobile(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var resource = new PmolPlannedWorkLabour
                {
                    CoperateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                    CpcBasicUnitofMeasureId = parameter.ResourceCreateDto.CpcBasicUnitOfMeasureId,
                    RequiredQuantity = parameter.ResourceCreateDto.Required,
                    PmolId = parameter.ResourceCreateDto.PmolId,
                    ConsumedQuantity = parameter.ResourceCreateDto.ConsumedQuantity,
                    Type = parameter.ResourceCreateDto.Type,
                    IsDeleted = false,
                    Id = parameter.ResourceCreateDto.Id
                };
                var IsExist = context.PMolPlannedWorkLabour.Any(p => p.Id == parameter.ResourceCreateDto.Id);
                if (IsExist == false)
                {
                    if (parameter.ResourceCreateDto.Environment != "local") await CopyCpc(parameter, connectionString);

                    context.PMolPlannedWorkLabour.Add(resource);
                    await context.SaveChangesAsync();
                }
                else
                {
                    context.PMolPlannedWorkLabour.Update(resource);
                    await context.SaveChangesAsync();
                }

                return resource.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateConsumableForMobile(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var resource = new PmolPlannedWorkConsumable
                {
                    CoperateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                    CpcBasicUnitofMeasureId = parameter.ResourceCreateDto.CpcBasicUnitOfMeasureId,
                    RequiredQuantity = parameter.ResourceCreateDto.Required,
                    PmolId = parameter.ResourceCreateDto.PmolId,
                    ConsumedQuantity = parameter.ResourceCreateDto.ConsumedQuantity,
                    Type = parameter.ResourceCreateDto.Type,
                    IsDeleted = false,
                    Id = parameter.ResourceCreateDto.Id
                };
                var IsExist = context.PMolPlannedWorkConsumable.Any(p => p.Id == parameter.ResourceCreateDto.Id);
                if (IsExist == false)
                {
                    if (parameter.ResourceCreateDto.Environment != "local") await CopyCpc(parameter, connectionString);

                    context.PMolPlannedWorkConsumable.Add(resource);
                    await context.SaveChangesAsync();
                }
                else
                {
                    context.PMolPlannedWorkConsumable.Update(resource);
                    await context.SaveChangesAsync();
                }

                return resource.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateToolsForMobile(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var resource = new PmolPlannedWorkTools
                {
                    CoperateProductCatalogId = parameter.ResourceCreateDto.CorporateProductCatalogId,
                    CpcBasicUnitofMeasureId = parameter.ResourceCreateDto.CpcBasicUnitOfMeasureId,
                    RequiredQuantity = parameter.ResourceCreateDto.Required,
                    PmolId = parameter.ResourceCreateDto.PmolId,
                    ConsumedQuantity = parameter.ResourceCreateDto.ConsumedQuantity,
                    Type = parameter.ResourceCreateDto.Type,
                    IsDeleted = false,
                    Id = parameter.ResourceCreateDto.Id
                };
                var IsExist = context.PMolPlannedWorkTools.Any(p => p.Id == parameter.ResourceCreateDto.Id);
                if (IsExist == false)
                {
                    if (parameter.ResourceCreateDto.Environment != "local") await CopyCpc(parameter, connectionString);

                    context.PMolPlannedWorkTools.Add(resource);
                    await context.SaveChangesAsync();
                }
                else
                {
                    context.PMolPlannedWorkTools.Update(resource);
                    await context.SaveChangesAsync();
                }

                return resource.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolTeamRoleReadDto>> ReadPlannedTeamMemberForMobile(
        PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options, parameter.TenantProvider);
            var lang = parameter.Lang;

            IEnumerable<PmolTeamRoleReadDto> teamRoleDto;
            var query = @"
                               	select PmolTeamRole.CabPersonId, PmolTeamRole.ConsumedQuantity, PmolTeamRole.Id,PmolTeamRole.RequiredQuantity,PmolTeamRole.RoleId, Role.RoleName
	                            from PmolTeamRole
	                            INNER JOIN PMolPlannedWorkLabour on PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id
	                            Left Outer Join Role on PmolTeamRole.RoleId=Role.Id
	                            WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.Type = 'Planned'
	                            AND PmolTeamRole.IsDeleted=0
                             ";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { PmolId = parameter.Id };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                teamRoleDto = dbConnection.Query<PmolTeamRoleReadDto>(query, parameters);
                
            }

            var Cabpersons = applicationDbContext.CabPerson.ToList();
            foreach (var dto in teamRoleDto)
            {
                var person = Cabpersons.Where(p => p.Id == dto.CabPersonId).FirstOrDefault();
                dto.CabPerson = person.FirstName + " " + person.Surname;
            }

            return teamRoleDto;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<IEnumerable<PmolTeamRoleReadDto>> ReadExtraTeamMemberForMobile(
        PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var applicationDbContext = new ApplicationDbContext(options, parameter.TenantProvider);
            var lang = parameter.Lang;
            IEnumerable<PmolTeamRoleReadDto> teamRoleDto;
            var query = @"
                               	select PmolTeamRole.CabPersonId, PmolTeamRole.ConsumedQuantity, PmolTeamRole.Id,PmolTeamRole.RequiredQuantity,PmolTeamRole.RoleId, Role.RoleName
	                            from PmolTeamRole
	                            INNER JOIN PMolPlannedWorkLabour on PmolTeamRole.PmolLabourId = PMolPlannedWorkLabour.Id
	                            Left Outer Join Role on PmolTeamRole.RoleId=Role.Id
	                            WHERE PMolPlannedWorkLabour.PmolId = @PmolId AND PmolTeamRole.Type = 'Extra'
	                            AND PmolTeamRole.IsDeleted=0
                             ";

            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            var parameters = new { PmolId = parameter.Id };

            await using (var dbConnection = new SqlConnection(connectionString))
            {
                teamRoleDto = dbConnection.Query<PmolTeamRoleReadDto>(query, parameters);
                
            }

            foreach (var dto in teamRoleDto)
            {
                var person = applicationDbContext.CabPerson.Where(p => p.Id == dto.CabPersonId)
                    .FirstOrDefault();
                dto.CabPerson = person.FirstName + " " + person.Surname;
            }

            return teamRoleDto;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateLabourForMobileApp(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var existingLabour =
                    context.PMolPlannedWorkLabour.FirstOrDefault(l =>
                        l.Id.Equals(parameter.ResourceCreateMobileDto.Id));
                string labourId = null;
                if (existingLabour == null)
                {
                    var labour = new PmolPlannedWorkLabour
                    {
                        CoperateProductCatalogId = parameter.ResourceCreateMobileDto.CorporateProductCatalogId,
                        CpcBasicUnitofMeasureId = parameter.ResourceCreateMobileDto.CpcBasicUnitOfMeasureId,
                        RequiredQuantity = parameter.ResourceCreateMobileDto.Required,
                        PmolId = parameter.ResourceCreateMobileDto.PmolId,
                        ConsumedQuantity = parameter.ResourceCreateMobileDto.ConsumedQuantity,
                        Type = parameter.ResourceCreateMobileDto.Type,
                        IsDeleted = false
                    };
                    if (parameter.ResourceCreateMobileDto.Environment != "local")
                        await CopyCpc(parameter, connectionString);

                    labour.Id = parameter.ResourceCreateMobileDto.Id ?? Guid.NewGuid().ToString();

                    context.PMolPlannedWorkLabour.Add(labour);
                    await context.SaveChangesAsync();
                    labourId = labour.Id;
                }
                else
                {
                    existingLabour.CoperateProductCatalogId =
                        parameter.ResourceCreateMobileDto.CorporateProductCatalogId;
                    existingLabour.CpcBasicUnitofMeasureId =
                        parameter.ResourceCreateMobileDto.CpcBasicUnitOfMeasureId;
                    existingLabour.RequiredQuantity = parameter.ResourceCreateMobileDto.Required;
                    existingLabour.PmolId = parameter.ResourceCreateMobileDto.PmolId;
                    existingLabour.ConsumedQuantity = parameter.ResourceCreateMobileDto.ConsumedQuantity;
                    existingLabour.Type = parameter.ResourceCreateMobileDto.Type;
                    existingLabour.IsDeleted = false;
                    context.PMolPlannedWorkLabour.Update(existingLabour);
                    await context.SaveChangesAsync();
                    labourId = existingLabour.Id;
                }

                var pmolTeamRole = new PmolTeamRole
                {
                    CabPersonId = parameter.ResourceCreateMobileDto.cabPersonId,
                    ConsumedQuantity = parameter.ResourceCreateMobileDto.ConsumedQuantity,
                    PmolLabourId = labourId,
                    RequiredQuantity = parameter.ResourceCreateMobileDto.Required,
                    RoleId = parameter.ResourceCreateMobileDto.RoleId,
                    Type = parameter.ResourceCreateMobileDto.Type,
                    Id = Guid.NewGuid().ToString()
                };
                context.PmolTeamRole.Add(pmolTeamRole);
                await context.SaveChangesAsync();

                return labourId;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task<string> CreateLabourTeamForMobileApp(PmolResourceParameter parameter)
    {
        try
        {
            var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
            {
                var pmolTeamRole = new PmolTeamRole
                {
                    CabPersonId = parameter.pmolTeamCreateDto.CabPersonId,
                    ConsumedQuantity = parameter.pmolTeamCreateDto.ConsumedQuantity,
                    PmolLabourId = parameter.pmolTeamCreateDto.PmolLabourId,
                    RequiredQuantity = parameter.pmolTeamCreateDto.RequiredQuantity,
                    RoleId = parameter.pmolTeamCreateDto.RoleId,
                    Type = parameter.pmolTeamCreateDto.Type
                };

                if (parameter.pmolTeamCreateDto.Id == null)
                {
                    pmolTeamRole.Id = Guid.NewGuid().ToString();
                    context.PmolTeamRole.Add(pmolTeamRole);
                    await context.SaveChangesAsync();
                }
                else
                {
                    pmolTeamRole.Id = parameter.pmolTeamCreateDto.Id;
                    context.PmolTeamRole.Update(pmolTeamRole);
                    await context.SaveChangesAsync();
                }

                return pmolTeamRole.Id;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public async Task<string> UpdateLabourTeamForMobileApp(PmolResourceParameter parameter)
    {
        try
        {
            //var options = new DbContextOptions<ShanukaDbContext>();
            var connectionString = ConnectionString.MapConnectionString(parameter.ContractingUnitSequenceId,
                parameter.ProjectSequenceId, parameter.TenantProvider);
            await using (var connection = new SqlConnection(connectionString))
            {
                connection.Query("UPDATE dbo.PmolTeamRole SET ConsumedQuantity = @ConsumedQuantity WHERE Id = @Id",
                    new
                    {
                        parameter.pmolTeamCreateDto.ConsumedQuantity, parameter.pmolTeamCreateDto.Id
                    });
                connection.Close();
            }

            return parameter.pmolTeamCreateDto.Id;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private async Task<string> CopyCpc(PmolResourceParameter parameter, string connectionString)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        bool isCpcExist;
        await using (var context = new ShanukaDbContext(options, connectionString, parameter.TenantProvider))
        {
            isCpcExist = context.CorporateProductCatalog.Any(c =>
                c.ResourceNumber == parameter.ResourceCreateDto.ResourceNumber);
        }

        if (isCpcExist == false)
        {
            var cpcParameter = new CpcParameters
            {
                Id = parameter.ResourceCreateDto.ResourceNumber
            };
            if (parameter.ResourceCreateDto.Environment == "cu")
                cpcParameter.ContractingUnitSequenceId = parameter.ContractingUnitSequenceId;

            cpcParameter.ContextAccessor = parameter.ContextAccessor;
            cpcParameter.TenantProvider = parameter.TenantProvider;
            var cpc = await parameter.ICoporateProductCatalogRepository
                .GetCorporateProductCatalogById(cpcParameter);

            var cpcCreateDto = new CoperateProductCatalogCreateDto();
            if (cpc.CpcBasicUnitOfMeasure != null) cpcCreateDto.CpcBasicUnitOfMeasureId = cpc.CpcBasicUnitOfMeasure.Key;

            if (cpc.CpcBrand != null) cpcCreateDto.CpcBrandId = cpc.CpcBrand.Key;

            if (cpc.CpcPressureClass != null) cpcCreateDto.CpcPressureClassId = cpc.CpcPressureClass.Key;

            if (cpc.ResourceFamily != null) cpcCreateDto.ResourceFamilyId = cpc.ResourceFamily.Key;

            if (cpc.CpcUnitOfSizeMeasure != null) cpcCreateDto.CpcUnitOfSizeMeasureId = cpc.CpcUnitOfSizeMeasure.Key;

            cpcCreateDto.CpcMaterialId = cpc.CpcMaterialId;
            cpcCreateDto.Id = null;
            cpcCreateDto.CpcId = cpc.Id;
            cpcCreateDto.InventoryPrice = cpc.InventoryPrice;
            cpcCreateDto.MaxOrderQuantity = cpc.MaxOrderQuantity;
            cpcCreateDto.MinOrderQuantity = cpc.MinOrderQuantity;

            cpcCreateDto.ResourceNumber = cpc.ResourceNumber;
            cpcCreateDto.ResourceTitle = cpc.ResourceTitle;
            cpcCreateDto.ResourceTypeId = cpc.ResourceType.key;
            cpcCreateDto.Size = cpc.Size;
            cpcCreateDto.Status = cpc.Status;
            cpcCreateDto.WallThickness = cpc.WallThickness;

            var resouceList = new List<CpcResourceNicknameCreateDto>();
            foreach (var resource in cpc.CpcResourceNickname)
            {
                var dto = new CpcResourceNicknameCreateDto
                {
                    CoperateProductCatalogId = resource.CoperateProductCatalogId,
                    Id = null,
                    Language = resource.Language,
                    LocaleCode = resource.LocaleCode,
                    NickName = resource.NickName
                };
                resouceList.Add(dto);
            }

            cpcCreateDto.CpcResourceNickname = resouceList;

            var imageList = new List<CpcImageCreateDto>();
            foreach (var image in cpc.CpcImage)
            {
                var dto = new CpcImageCreateDto
                {
                    Id = null,
                    Image = image.Image
                };
                imageList.Add(dto);
            }

            cpcCreateDto.CpcImage = imageList;

            var vendorList = new List<CpcVendorCreateDto>();
            foreach (var vendor in cpc.CpcVendor)
            {
                var dto = new CpcVendorCreateDto
                {
                    CompanyId = vendor.CompanyId,
                    CompanyName = vendor.Company.Name,
                    CoperateProductCatalogId = vendor.CoperateProductCatalogId,
                    Id = null,
                    MaxOrderQuantity = vendor.MaxOrderQuantity,
                    MinOrderQuantity = vendor.MinOrderQuantity,
                    PreferredParty = vendor.PreferredParty,
                    PurchasingUnit = vendor.PurchasingUnit,
                    ResourceLeadTime = vendor.ResourceLeadTime,
                    ResourceNumber = vendor.ResourceNumber,
                    ResourcePrice = vendor.ResourcePrice,
                    ResourceTitle = vendor.ResourceTitle,
                    RoundingValue = vendor.RoundingValue
                };
                vendorList.Add(dto);
            }

            cpcCreateDto.CpcVendor = vendorList;

            cpcParameter.CpcDto = cpcCreateDto;
            cpcParameter.isCopy = true;
            cpcParameter.ProjectSequenceId = parameter.ProjectSequenceId;
            cpcParameter.ContractingUnitSequenceId = parameter.ContractingUnitSequenceId;
            await parameter.ICoporateProductCatalogRepository.CreateCoporateProductCatalog(cpcParameter,
                parameter.ContextAccessor);
        }

        return "";
    }
    
    public async Task<string> RemovePersonFromPmol(PmolResourceParameter parameter)
    {
        try
        {
            await using var Projectconnection = new SqlConnection(parameter.RemovePersonFromPmol.ProjectConnectionString);
            
            await using var cuConnection = new SqlConnection(parameter.RemovePersonFromPmol.CuConnectionString);
            await using var connection = new SqlConnection(parameter.TenantProvider.GetTenant().ConnectionString);

          var teamquery =
            @"SELECT CabPerson.Id ,CabPerson.FirstName AS Name,OrganizationTaxonomy.RoleId FROM dbo.OrganizationTaxonomy LEFT OUTER JOIN dbo.CabPersonCompany ON OrganizationTaxonomy.PersonId = CabPersonCompany.Id INNER JOIN dbo.CabPerson ON CabPersonCompany.PersonId = CabPerson.Id WHERE OrganizationTaxonomy.ParentId = @Id";

        var update =
            @"UPDATE dbo.OrganizationTeamPmol SET OrganizationTeamId = @OrganizationTeamId WHERE PmolId = @Id ;";

        var insertNewTeam =
            @"INSERT INTO dbo.OrganizationTaxonomy ( Id ,Title,ParentId ,OrganizationTaxonomyLevelId,PersonId ,RoleId,ModifiedDate,StartDate,EndDate,TemporaryTeamNameId ) VALUES ( @Id ,@Title,@ParentId ,@OrganizationTaxonomyLevelId ,(SELECT Id FROM CabPersonCompany WHERE PersonId = @PersonId) ,@RoleId,@ModifiedDate,@StartDate,@EndDate,@TemporaryTeamNameId);";

        var team = new List<PmolTeamMeber>();

        var orgTeamId = connection
            .Query<string>("SELECT OrganizationTeamId FROM OrganizationTeamPmol  WHERE PmolId = @Id",
                new { Id = parameter.RemovePersonFromPmol.PmolId }).FirstOrDefault();
        

        string teamId = null;
        
       
            if (orgTeamId != null)
            {
                await cuConnection.ExecuteAsync(
                    "Delete From PmolAssignTime Where CabPersonId = @CabPersonId AND PmolId = @PmolId",
                    parameter.RemovePersonFromPmol);
                
                var checkTeamCombinationSql =
                    @"SELECT PersonId FROM OrganizationTaxonomy ot WHERE ot.ParentId = @TeamId";

                var checkTeamCombinationData = connection.Query<string>(checkTeamCombinationSql,
                    new { TeamId = orgTeamId}).ToList();

                var cabPersonCompanyId = connection.Query<string>(
                    "SELECT Id FROM dbo.CabPersonCompany cpc WHERE cpc.PersonId = @PersonId",
                    new { PersonId = parameter.RemovePersonFromPmol.CabPersonId }).FirstOrDefault();

                checkTeamCombinationData.Remove(cabPersonCompanyId);

                if (!checkTeamCombinationData.Any())
                {
                    await connection.ExecuteAsync("Delete From OrganizationTeamPmol Where  PmolId = @PmolId",
                        parameter.RemovePersonFromPmol);

                    return "ok";
                }

                checkTeamCombinationData.Sort();

                var endDate =parameter.RemovePersonFromPmol.ExecutionDate
                    .AddDays(-(int)parameter.RemovePersonFromPmol.ExecutionDate.DayOfWeek).AddDays(7);

                string selectTeamSql = @"with name_tree as
                                        (SELECT
                                          OrganizationTaxonomy.Id
                                         ,OrganizationTaxonomy.Title
                                         ,OrganizationTaxonomy.ParentId
                                         ,OrganizationTaxonomy.OrganizationTaxonomyLevelId
                                         ,OrganizationTaxonomy.ModifiedDate
                                         ,OrganizationTaxonomy.EndDate
                                         ,OrganizationTaxonomy.TemporaryTeamNameId
                                        FROM dbo.OrganizationTaxonomy
                                        WHERE OrganizationTaxonomy.Id = (SELECT ot.ParentId FROM OrganizationTaxonomy ot WHERE Id = @TeamId)
                                          UNION ALL
                                          SELECT c.Id, c.Title,c.ParentId,c.OrganizationTaxonomyLevelId,c.ModifiedDate,c.EndDate,c.TemporaryTeamNameId
                                          FROM dbo.OrganizationTaxonomy c
                                          JOIN name_tree p on p.Id = c.ParentId)
                                          select OrganizationTaxonomyLevelId, Title, Id,ParentId,ModifiedDate,EndDate,TemporaryTeamNameId
                                          from name_tree
                                          where OrganizationTaxonomyLevelId = 'fg10e768-3e06-po02-b337-ee367a82adfg'
                                          AND (EndDate = @EndDate OR EndDate IS NULL)
                                          ORDER BY Title DESC";

                var selectTeamData = connection
                    .Query<OrganizationTaxonomy>(selectTeamSql,
                        new { TeamId = orgTeamId, EndDate = endDate }).ToList();

                string existingTeamId = null;

                foreach (var i in selectTeamData)
                {
                    var teamData = connection.Query<string>(checkTeamCombinationSql, new { TeamId = i.Id }).ToList();

                    teamData.Sort();

                    if (teamData.SequenceEqual(checkTeamCombinationData))
                    {
                        existingTeamId = i.Id;
                        break;
                    }
                }

                if (existingTeamId != null)
                {
                    teamId = existingTeamId;
                    // await connection.ExecuteAsync("UPDATE dbo.OrganizationTeamPmol SET OrganizationTeamId = @existingTeamId WHERE PmolId IN (SELECT PmolId FROM OrganizationTeamPmol WHERE OrganizationTeamId = @TeamId)",new{existingTeamId,VPParameter.PmolAssignDayPanningDto.TeamId});
                    await connection.ExecuteAsync(
                        "UPDATE dbo.OrganizationTeamPmol SET OrganizationTeamId = @existingTeamId WHERE PmolId = @Id",
                        new { existingTeamId, Id = parameter.RemovePersonFromPmol.PmolId });
                }else
                {
                   
                    var parent = connection
                        .Query<OrganizationTaxonomy>("SELECT *FROM dbo.OrganizationTaxonomy WHERE Id = @Id;",
                            new { Id = orgTeamId }).FirstOrDefault();
                    var teamNew = checkTeamCombinationData.Select(x => new PmolTeamMeber
                    {
                        Id = x,
                        Name = connection.Query<string>(
                            "Select FullName From CabPerson Left Outer Join CabPersonCompany cpc On CabPerson.Id = cpc.PersonId Where cpc.Id = @Id",
                            new { Id = x }).FirstOrDefault()
                    }).ToList();
                
                

                    var lacaldate = Convert.ToDateTime(parameter.RemovePersonFromPmol.ExecutionDate);

                    var param1 = new
                    {
                        Id = Guid.NewGuid(),
                        Title = parent.Title,
                        parent.ParentId,
                        OrganizationTaxonomyLevelId = "fg10e768-3e06-po02-b337-ee367a82adfg",
                        PersonId = "",
                        RoleId = "",
                        ModifiedDate = DateTime.UtcNow,
                        StartDate = parameter.RemovePersonFromPmol.ExecutionDate
                            .AddDays(-(int)parameter.RemovePersonFromPmol.ExecutionDate.DayOfWeek).Date,
                        EndDate = parameter.RemovePersonFromPmol.ExecutionDate
                            .AddDays(-(int)parameter.RemovePersonFromPmol.ExecutionDate.DayOfWeek).AddDays(7).Date,
                        TemporaryTeamNameId = "7bcb4e8d-8e8c-487d-team-6b91c89fAcce"
                    };
                    
                    await connection.ExecuteAsync(insertNewTeam, param1);

                    teamId = param1.Id.ToString();
                    await connection.ExecuteAsync(update,
                        new { OrganizationTeamId = param1.Id, Id = parameter.RemovePersonFromPmol.PmolId });

                    foreach (var i in teamNew)
                    {
                        var param = new
                        {
                            Id = Guid.NewGuid(),
                            Title = i.Name,
                            ParentId = param1.Id,
                            OrganizationTaxonomyLevelId = "we10e768-3e06-po02-b337-ee367a82adwe",
                            PersonId = i.Id,
                            i.RoleId,
                            ModifiedDate = DateTime.UtcNow,
                            StartDate = "",
                            EndDate = "",
                            TemporaryTeamNameId = " "
                        };

                        await connection.ExecuteAsync(insertNewTeam, param);
                    }
                }
            }
            
            return "Ok";
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}