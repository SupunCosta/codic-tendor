using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.PBS_;
using UPrinceV4.Web.Models;
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PBS;

public class PbsInstructionsRepository : IPbsInstructionsRepository
{
    public async Task<IEnumerable<PbsInstruction>> GetPbsInstructionist(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var instructionsList = context.PbsInstruction.Where(r => r.IsDeleted == false)
                .Include(p => p.PbsInstructionFamily)
                .Include(p => p.PbsInstructionLink).ToList();

            return instructionsList;
        }
    }

    public async Task<IEnumerable<Instructions>> InstructionsFilter(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);

        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var instructionsList = context.Instructions.Where(r => r.IsDeleted == false).AsEnumerable()
                .OrderByDescending(x => x.Title).ToList();

            foreach (var item in instructionsList)
            {
                item.PbsInstructionFamily = context.PbsInstructionFamily
                    .Where(x => x.Id == item.PbsInstructionFamilyId).FirstOrDefault();

                //item.PbsInstructionLink = context.PbsInstructionLink.Where(x => x.PbsInstructionId == item.Id).ToList();

                item.PbsInstructionLink = dbConnection
                    .Query<PbsInstructionLink>(
                        "Select PbsInstructionLink.*,PbsInstructionLink.Link As Value  From PbsInstructionLink WHERE PbsInstructionId = @Id",
                        new { item.Id }).ToList();

                if (!(pbsInstructionsRepositoryParameter.Lang == Language.en.ToString() ||
                      string.IsNullOrEmpty(pbsInstructionsRepositoryParameter.Lang)))
                    if (item.PbsInstructionFamily != null)
                    {
                        // var localizedData = context.LocalizedData.FirstOrDefault(ld =>
                        //     ld.LocaleCode == item.PbsInstructionFamily.LocaleCode &&
                        //     ld.LanguageCode == pbsInstructionsRepositoryParameter.Lang);
                        // if (localizedData != null)
                        //     item.PbsInstructionFamily.Family = localizedData.Label;

                        var localizedData = dbConnection.Query<string>(
                                "SELECT Family FROM dbo.PbsInstructionFamilyLocalizedData WHERE InstructionFamilyID = @Id AND LocaleCode = @lang",
                                new { item.PbsInstructionFamily.Id, lang = pbsInstructionsRepositoryParameter.Lang })
                            .FirstOrDefault();

                        if (localizedData != null)
                            item.PbsInstructionFamily.Family = localizedData;
                    }

                if (item.PbsInstructionFamilyId != null) item.InstructionFamilyName = item.PbsInstructionFamily.Family;

                if (item.InstructionType != null)
                {
                    if (item.InstructionType == "100")
                        item.InstructionTypeName = "Technical";
                    else if (item.InstructionType == "200")
                        item.InstructionTypeName = "Environment";
                    else if (item.InstructionType == "300")
                        item.InstructionTypeName = "Safety";
                    else if (item.InstructionType == "400") item.InstructionTypeName = "Health";
                }
            }

            if (pbsInstructionsRepositoryParameter.Filter != null)
            {
                if (pbsInstructionsRepositoryParameter.Filter.Title != null)
                {
                    pbsInstructionsRepositoryParameter.Filter.Title =
                        pbsInstructionsRepositoryParameter.Filter.Title.Replace("'", "''");
                    instructionsList = instructionsList
                        .Where(x => x.Title.ToLower()
                            .Contains(pbsInstructionsRepositoryParameter.Filter.Title.ToLower())).ToList();
                }
                    
                if (pbsInstructionsRepositoryParameter.Filter.InstructionType != null)
                    instructionsList = instructionsList
                        .Where(x => x.InstructionType == pbsInstructionsRepositoryParameter.Filter.InstructionType)
                        .ToList();
                if (pbsInstructionsRepositoryParameter.Filter.PbsInstructionFamilyId != null)
                    instructionsList = instructionsList
                        .Where(x => x.PbsInstructionFamilyId ==
                                    pbsInstructionsRepositoryParameter.Filter.PbsInstructionFamilyId).ToList();
                if (pbsInstructionsRepositoryParameter.Filter.PbsProductId != null)
                {
                    var pbsList = context.PbsInstruction
                        .Where(x => x.PbsProductId == pbsInstructionsRepositoryParameter.Filter.PbsProductId &&
                                    x.IsDeleted == false)
                        .ToList();

                    instructionsList = instructionsList
                        .Where(x => pbsList.All(c => c.InstructionsId != x.Id)).ToList();
                }

                if (pbsInstructionsRepositoryParameter.Filter.Sorter?.Attribute?.ToLower() == "title")
                {
                    if (pbsInstructionsRepositoryParameter.Filter.Sorter.Order?.ToLower() == "desc")
                        instructionsList = instructionsList.OrderByDescending(x => x.Title).ToList();
                    if (pbsInstructionsRepositoryParameter.Filter.Sorter.Order?.ToLower() == "asc")
                        instructionsList = instructionsList.OrderBy(x => x.Title).ToList();
                }

                if (pbsInstructionsRepositoryParameter.Filter.Sorter?.Attribute?.ToLower() == "instructiontype")
                {
                    if (pbsInstructionsRepositoryParameter.Filter.Sorter.Order?.ToLower() == "desc")
                        instructionsList = instructionsList.OrderByDescending(x => x.InstructionTypeName).ToList();
                    if (pbsInstructionsRepositoryParameter.Filter.Sorter.Order?.ToLower() == "asc")
                        instructionsList = instructionsList.OrderBy(x => x.InstructionTypeName).ToList();
                }

                if (pbsInstructionsRepositoryParameter.Filter.Sorter?.Attribute?.ToLower() == "family")
                {
                    if (pbsInstructionsRepositoryParameter.Filter.Sorter.Order?.ToLower() == "desc")
                        instructionsList = instructionsList.OrderByDescending(x => x.InstructionFamilyName).ToList();
                    if (pbsInstructionsRepositoryParameter.Filter.Sorter.Order?.ToLower() == "asc")
                        instructionsList = instructionsList.OrderBy(x => x.InstructionFamilyName).ToList();
                }
            }

            return instructionsList;
        }
    }

    public async Task<Instructions> GetPbsInstructionById(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);

        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var lang = pbsInstructionsRepositoryParameter.Lang;
            var instruction = context.Instructions
                .Where(r => r.IsDeleted == false &&
                            r.SequenceCode == pbsInstructionsRepositoryParameter.PbsInstructionId).FirstOrDefault();

            if (instruction != null)
            {
                instruction.PbsInstructionFamily = context.PbsInstructionFamily
                    .Where(x => x.Id == instruction.PbsInstructionFamilyId).FirstOrDefault();

                // instruction.PbsInstructionLink = context.PbsInstructionLink
                //     .Where(x => x.PbsInstructionId == instruction.Id).ToList();

                instruction.PbsInstructionLink = dbConnection
                    .Query<PbsInstructionLink>(
                        "Select PbsInstructionLink.*,PbsInstructionLink.Link As Value  From PbsInstructionLink WHERE PbsInstructionId = @Id",
                        new { instruction.Id }).ToList();
            }

            if (instruction != null)

                if (!(lang == Language.en.ToString() || string.IsNullOrEmpty(lang)))
                    if (instruction.PbsInstructionFamily != null)
                    {
                        var localizedData = context.LocalizedData.FirstOrDefault(ld =>
                            ld.LanguageCode == instruction.PbsInstructionFamily.LocaleCode &&
                            ld.LanguageCode == lang);
                        if (localizedData != null) instruction.PbsInstructionFamily.Family = localizedData.Label;
                    }

            return instruction;
        }
    }

    public async Task<IEnumerable<PbsInstructionLoadDto>> GetPbsInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        // 100 = technical, 200 = environmental, 300 = safety, 400 = health
        //var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        //var dbConnection = new SqlConnection(connectionString);

        var sql = @"SELECT
  Instructions.*
,PbsInstruction.Id As PbsInstructionId
,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
 ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
 ,PbsInstructionFamilyLocalizedData.Family AS Text
FROM dbo.PbsInstruction
LEFT OUTER JOIN dbo.Instructions
  ON PbsInstruction.InstructionsId = Instructions.Id
LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
  ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
OR Instructions.PbsInstructionFamilyId IS NULL)
AND PbsInstruction.PbsProductId = @PbsProductId
AND Instructions.InstructionType = @InstructionType AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false'";

        var sqlLink =
            @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

        var instructionLoadDtos = new List<PbsInstructionLoadDto>();

        await using (var connection = new SqlConnection(connectionString))
        {
            var cache = new Dictionary<string, PbsInstructionLoadDto>();

            instructionLoadDtos = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    },
                    new
                    {
                        lang = pbsInstructionsRepositoryParameter.Lang,
                        pbsInstructionsRepositoryParameter.PbsProductId,
                        pbsInstructionsRepositoryParameter.InstructionType
                    },
                    splitOn: "Key").ToList();

            foreach (var instructionLoadDtoss in instructionLoadDtos)
                instructionLoadDtoss.PbsInstructionLink = connection
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();

            
        }

        return instructionLoadDtos;
    }

    public async Task<IEnumerable<PbsInstructionLoadDto>> GetAllPbsInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        //var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        var instructionDto = new List<PbsInstructionLoadDto>();
        instructionDto =
            (List<PbsInstructionLoadDto>)await GetPbsInstructionsByPbsProductId(
                pbsInstructionsRepositoryParameter);
        var sql = @"with name_tree as 
                                   (
                                    select Id, PbsProductId, PbsTaxonomyNodeId
                                    from PbsProductTaxonomy
                                    where PbsProductId = @PbsProductId
                                    union all
                                    select c.Id, c.PbsProductId, c.PbsTaxonomyNodeId
                                    from PbsProductTaxonomy c
                                    join name_tree p on p.PbsTaxonomyNodeId = c.PbsProductId
                                    )
                                    select PbsTaxonomyNodeId
                                    from name_tree
                                    where PbsTaxonomyNodeId is not null ";

        var param = new { pbsInstructionsRepositoryParameter.PbsProductId };
        IEnumerable<string> Idlist = null;
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            Idlist = dbConnection.Query<string>(sql, param);
            
        }

        foreach (var id in Idlist)
        {
            pbsInstructionsRepositoryParameter.PbsProductId = id;
            var dto =
                await GetPbsInstructionsByPbsProductId(pbsInstructionsRepositoryParameter);
            foreach (var instruction in dto) instructionDto.Add(instruction);
        }

        return instructionDto;
    }


    public async Task<PbsInstructionLoadAllPmolDto> GetAllPbsInstructionsByPbsProductAllTypes(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        var sql = @"with name_tree as 
                                   (
                                    select Id, PbsProductId, PbsTaxonomyNodeId
                                    from PbsProductTaxonomy
                                    where PbsProductId = @PbsProductId
                                    union all
                                    select c.Id, c.PbsProductId, c.PbsTaxonomyNodeId
                                    from PbsProductTaxonomy c
                                    join name_tree p on p.PbsTaxonomyNodeId = c.PbsProductId
                                    )
                                    select PbsTaxonomyNodeId
                                    from name_tree
                                    where PbsTaxonomyNodeId is not null ";


        var sql2 = @"SELECT
  Instructions.*
,PbsInstruction.Id As PbsInstructionId
,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
 ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
 ,PbsInstructionFamilyLocalizedData.Family AS Text
FROM dbo.PbsInstruction
LEFT OUTER JOIN dbo.Instructions
  ON PbsInstruction.InstructionsId = Instructions.Id
LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
  ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
OR Instructions.PbsInstructionFamilyId IS NULL)
AND PbsInstruction.PbsProductId IN @PbsProductId
AND Instructions.InstructionType = @InstructionType AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false' ";

        var sqlLink =
            @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

        var param = new { pbsInstructionsRepositoryParameter.PbsProductId };


        //List<PbsInstructionLoadDto> instructionLoadDtos = new List<PbsInstructionLoadDto>();
        var pbsInstructionLoadAllPmolDto = new PbsInstructionLoadAllPmolDto();
        await using var connection = new SqlConnection(connectionString);
        var Idlist = connection.Query<string>(sql, param).ToList();
        Idlist.Add(pbsInstructionsRepositoryParameter.PbsProductId);

        var instructionDto = connection
            .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                sql2,
                (pbsInstruction, pbsInstructionFamily) =>
                {
                    pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                    return pbsInstruction;
                },
                new
                {
                    lang = pbsInstructionsRepositoryParameter.Lang, PbsProductId = Idlist,
                    InstructionType = 100
                },
                splitOn: "Key").ToList();

        Parallel.ForEach(instructionDto, instructionLoadDtoss =>
        {
            using var connectionX = new SqlConnection(connectionString);
            instructionLoadDtoss.PbsInstructionLink = connectionX
                .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                .ToList();
        });


        //100 = technical, 200 = environmental, 300 = safety, 400 = health
        pbsInstructionLoadAllPmolDto.Technical = instructionDto;


        var instructionenvironmentalDto = connection
            .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                sql2,
                (pbsInstruction, pbsInstructionFamily) =>
                {
                    pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                    return pbsInstruction;
                },
                new
                {
                    lang = pbsInstructionsRepositoryParameter.Lang, PbsProductId = Idlist,
                    InstructionType = 200
                },
                splitOn: "Key").ToList();

        Parallel.ForEach(instructionenvironmentalDto, instructionLoadDtoss =>
        {
            using var connectionY = new SqlConnection(connectionString);
            instructionLoadDtoss.PbsInstructionLink = connectionY
                .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                .ToList();
        });
        pbsInstructionLoadAllPmolDto.Environmental = instructionenvironmentalDto;

        var instructionsafetyDto = connection
            .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                sql2,
                (pbsInstruction, pbsInstructionFamily) =>
                {
                    pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                    return pbsInstruction;
                },
                new
                {
                    lang = pbsInstructionsRepositoryParameter.Lang, PbsProductId = Idlist,
                    InstructionType = 300
                },
                splitOn: "Key").ToList();

        Parallel.ForEach(instructionenvironmentalDto, instructionLoadDtoss =>
        {
            using var connectionZ = new SqlConnection(connectionString);
            instructionLoadDtoss.PbsInstructionLink = connectionZ
                .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                .ToList();
        });

        pbsInstructionLoadAllPmolDto.Safety = instructionsafetyDto;

        var instructionhealthDto = connection
            .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                sql2,
                (pbsInstruction, pbsInstructionFamily) =>
                {
                    pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                    return pbsInstruction;
                },
                new
                {
                    lang = pbsInstructionsRepositoryParameter.Lang, PbsProductId = Idlist,
                    InstructionType = 400
                },
                splitOn: "Key").ToList();

        Parallel.ForEach(instructionhealthDto, instructionLoadDtoss =>
        {
            using var connectionB = new SqlConnection(connectionString);
            instructionLoadDtoss.PbsInstructionLink = connectionB
                .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                .ToList();
        });

        pbsInstructionLoadAllPmolDto.Health = instructionhealthDto;


        

        return pbsInstructionLoadAllPmolDto;
    }

    public async Task<Instructions> AddPbsInstruction(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        var dbConnection = new SqlConnection(connectionString);

        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var instructions = pbsInstructionsRepositoryParameter.PbsInstruction;
            var existingInstruction = context.Instructions.Where(r => r.Id == instructions.Id)
                .ToList().FirstOrDefault();

            var dbContext = pbsInstructionsRepositoryParameter.ApplicationDbContext;

            var updateQuery = @"UPDATE dbo.Instructions 
                                SET
                                    Id = @Id 
                                    ,InstructionsDetails = @InstructionsDetails
                                    ,InstructionType = @InstructionType
                                    ,PbsInstructionFamilyId = @PbsInstructionFamilyId
                                    ,IsDeleted = 0 
                                    ,IsSaved = 0 
                                    ,SequenceCode = @SequenceCode
                                    ,Name = @Name
                                WHERE
                                    Id = @Id
                                    ";
            if (existingInstruction != null)
            {
                // var instLinks = context.PbsInstructionLink.Where(x => x.PbsInstructionId == existingInstruction.Id)
                //     .ToList();
                var instLinks = dbConnection
                    .Query<PbsInstructionLink>(
                        "Select PbsInstructionLink.*,PbsInstructionLink.Link As Value  From PbsInstructionLink WHERE PbsInstructionId = @Id",
                        new { existingInstruction.Id }).ToList();
                existingInstruction.Name = instructions.Name;
                existingInstruction.PbsInstructionFamilyId = instructions.PbsInstructionFamilyId;
                if (instLinks.Any())
                    foreach (var link in instLinks)
                    {
                        context.PbsInstructionLink.Remove(link);
                        context.SaveChanges();
                    }

                existingInstruction.InstructionsDetails = instructions.InstructionsDetails;
                existingInstruction.InstructionType = instructions.InstructionType;
                existingInstruction.PbsInstructionLink = instructions.PbsInstructionLink;

                foreach (var link in instructions.PbsInstructionLink)
                {
                    // if (link.Id == null)
                    //     link.Id = Guid.NewGuid().ToString();
                    // link.PbsInstructionId = existingInstruction.Id;
                    // context.PbsInstructionLink.Update(link);
                    // context.SaveChanges();

                    var query = @"INSERT INTO dbo.PbsInstructionLink
                                (
                                  Id
                                 ,Title
                                 ,Link
                                 ,PbsInstructionId
                                 ,Type
                                )
                                VALUES
                                (
                                  @Id
                                 ,@Title
                                 ,@Link
                                 ,@PbsInstructionId
                                 ,@Type
                                )";

                    var param = new
                    {
                        link.Id,
                        link.Title,
                        link.Link,
                        PbsInstructionId = existingInstruction.Id,
                        link.Type
                    };

                    await dbConnection.ExecuteAsync(query, param);
                }

                // context.Instructions.Update(existingInstruction);
                // context.SaveChanges();
                await dbConnection.ExecuteAsync(updateQuery, existingInstruction);

                return existingInstruction;
            }

            var insert = @"INSERT INTO dbo.Instructions
                            (
                              Id
                             ,InstructionsDetails
                             ,InstructionType
                             ,PbsInstructionFamilyId
                             ,IsDeleted
                             ,IsSaved
                             ,SequenceCode
                             ,Name
                            )
                            VALUES
                            (
                              @Id
                             ,@InstructionsDetails
                             ,@InstructionType
                             ,@PbsInstructionFamilyId
                             ,@IsDeleted
                             ,@IsSaved
                             ,@SequenceCode
                             ,@Name
                            )";
            instructions.Id = Guid.NewGuid().ToString();
            instructions.SequenceCode = new IdGenerator().GenerateId(dbContext, "I", "InstructionId");
            instructions.PbsInstructionFamilyId = instructions.PbsInstructionFamilyId;
            foreach (var link in instructions.PbsInstructionLink)
            {
                var query = @"INSERT INTO dbo.PbsInstructionLink
                                (
                                  Id
                                 ,Title
                                 ,Link
                                 ,PbsInstructionId
                                 ,Type
                                )
                                VALUES
                                (
                                  @Id
                                 ,@Title
                                 ,@Link
                                 ,@PbsInstructionId
                                 ,@Type
                                )";

                var param = new
                {
                    link.Id,
                    link.Title,
                    link.Link,
                    PbsInstructionId = instructions.Id,
                    link.Type
                };

                await dbConnection.ExecuteAsync(query, param);
                // link.Id = Guid.NewGuid().ToString();
                // link.PbsInstructionId = instructions.Id;
                // context.PbsInstructionLink.Add(link);
                // context.SaveChanges();
            }

            // context.Instructions.Add(instructions);
            // await context.SaveChangesAsync();

            await dbConnection.ExecuteAsync(insert, instructions);
            return instructions;
        }
    }

    public async Task<bool> DeletePbsInstruction(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var isUpdated = false;
            foreach (var id in pbsInstructionsRepositoryParameter.IdList)
            {
                var instruction = context.PbsInstruction.FirstOrDefault(p => p.Id == id);
                if (instruction != null)
                {
                    instruction.IsDeleted = true;
                    context.PbsInstruction.Update(instruction);
                    await context.SaveChangesAsync();
                    isUpdated = true;
                }
            }

            return isUpdated;
        }
    }

    public async Task<bool> DeleteInstruction(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var isUpdated = false;
            foreach (var id in pbsInstructionsRepositoryParameter.IdList)
            {
                var instruction = context.Instructions.FirstOrDefault(p => p.Id == id);
                if (instruction != null)
                {
                    instruction.IsDeleted = true;
                    context.Instructions.Update(instruction);
                    await context.SaveChangesAsync();
                    isUpdated = true;
                }
            }

            return isUpdated;
        }
    }

    public async Task<PbsInstructionsDropdown> GetPbsInstructionDropdownData(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var instructionsDropdown = new PbsInstructionsDropdown();
        //PbsInstructionFamilyDapper
        var data = pbsInstructionsRepositoryParameter.IPbsInstructionFamilyRepository.GetInstructionFamilyList(
            pbsInstructionsRepositoryParameter.PbsInstructionFamilyRepositoryParameter);
        var PbsInstructionFamily = new List<PbsInstructionFamilyDapper>();
        foreach (var dto in data)
        {
            var dapper = new PbsInstructionFamilyDapper
            {
                Key = dto.Id,
                Text = dto.Family,
                Type = dto.Type
            };
            PbsInstructionFamily.Add(dapper);
        }

        instructionsDropdown.PbsInstructionFamily = PbsInstructionFamily.Distinct();
        return instructionsDropdown;
    }

    public async Task<string> UploadImage(PbsInstructionsRepositoryParameter personRepositoryParameter)
    {
        var client = new FileClient();
        var url = client.PersistPhotoInNewFolder(
            personRepositoryParameter.Image.Files.FirstOrDefault()?.FileName,
            personRepositoryParameter.TenantProvider
            , personRepositoryParameter.Image.Files.FirstOrDefault(), personRepositoryParameter.FolderName);
        return url;
    }

    public async Task<IEnumerable<PbsInstructionLoadDto>> GetPictureOfInstallationInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        // 100 = technical, 200 = environmental, 300 = safety, 400 = health
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var lang = pbsInstructionsRepositoryParameter.Lang;

            var sql2 = @"SELECT
  Instructions.*
,PbsInstruction.Id As PbsInstructionId
,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
 ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
 ,PbsInstructionFamilyLocalizedData.Family AS Text
FROM dbo.PbsInstruction
LEFT OUTER JOIN dbo.Instructions
  ON PbsInstruction.InstructionsId = Instructions.Id
LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
  ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
OR Instructions.PbsInstructionFamilyId IS NULL)
AND PbsInstruction.PbsProductId  = @PbsProductId
AND Instructions.InstructionType = @InstructionType AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false' AND PbsInstructionFamilyLocalizedData.InstructionFamilyID ='626253e2-981c-4dcd-b084-a801adfdab1e'";

            var sqlLink =
                @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

            var param = new { pbsInstructionsRepositoryParameter.PbsProductId };


            //List<PbsInstructionLoadDto> instructionLoadDtos = new List<PbsInstructionLoadDto>();
            var pbsInstructionLoadAllPmolDto = new PbsInstructionLoadAllPmolDto();
            await using var connection = new SqlConnection(connectionString);
            var instructionDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    },
                    new
                    {
                        lang = pbsInstructionsRepositoryParameter.Lang,
                        pbsInstructionsRepositoryParameter.PbsProductId,
                        pbsInstructionsRepositoryParameter.InstructionType
                    },
                    splitOn: "Key").ToList();

            Parallel.ForEach(instructionDto, instructionLoadDtoss =>
            {
                using var connectionX = new SqlConnection(connectionString);
                instructionLoadDtoss.PbsInstructionLink = connectionX
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();
            });

            // var pbsInstructions = context.PbsInstruction
            //     .Where(r => r.IsDeleted == false && r.PbsProductId ==
            //                                      pbsInstructionsRepositoryParameter.PbsProductId
            //                                      && r.InstructionType.Equals(pbsInstructionsRepositoryParameter
            //                                          .InstructionType)
            //                                      && r.PbsInstructionFamily.Family.Equals("Installation"))
            //     .Include(p => p.PbsInstructionFamily)
            //     .Include(p => p.PbsInstructionLink).ToList();

            //var riskDto = new RiskReadDto();
            // if (pbsInstructions.Any())
            //     //riskDto.SequenceCode = instruction.SequenceCode;
            //     //riskDto.Name = instruction.Name;
            //     //riskDto.Title = instruction.Title;
            //     //riskDto.HeaderTitle = instruction.HeaderTitle;
            //     //riskDto.RiskDetails = instruction.RiskDetails;
            //     foreach (var pbsInstruction in pbsInstructions)
            //         if (!(lang == Language.en.ToString() || string.IsNullOrEmpty(lang)))
            //             if (pbsInstruction.PbsInstructionFamily != null)
            //             {
            //                 var localizedData = context.LocalizedData.FirstOrDefault(ld =>
            //                     ld.LocaleCode == pbsInstruction.PbsInstructionFamily.LocaleCode &&
            //                     ld.LanguageCode == lang);
            //                 if (localizedData != null)
            //                     pbsInstruction.PbsInstructionFamily.Family = localizedData.Label;
            //             }
            //
            // var instructionLoadDtos = new List<PbsInstructionLoadDto>();
            // foreach (var pbsInstruction in pbsInstructions)
            // {
            //     var dto = new PbsInstructionLoadDto
            //     {
            //         Id = pbsInstruction.Id,
            //         SequenceCode = pbsInstruction.SequenceCode,
            //         Name = pbsInstruction.Name,
            //         InstructionType = pbsInstruction.InstructionType,
            //         InstructionsDetails = pbsInstruction.InstructionsDetails,
            //         PbsProductId = pbsInstruction.PbsProductId,
            //         PbsProduct = pbsInstruction.PbsProduct,
            //         PbsInstructionFamilyId = pbsInstruction.PbsInstructionFamilyId
            //     };
            //
            //     if (pbsInstruction.PbsInstructionFamily != null)
            //     {
            //         dto.PbsInstructionFamily = new PbsInstructionFamilyLoadDto
            //         {
            //             Key = pbsInstruction.PbsInstructionFamily.Id,
            //             Text = pbsInstruction.PbsInstructionFamily.Family
            //         };
            //         dto.PbsInstructionFamilyName = pbsInstruction.PbsInstructionFamily.Family;
            //     }
            //
            //     var instructionLinkDtos = new List<PbsInstructionLinkDto>();
            //     foreach (var link in pbsInstruction.PbsInstructionLink)
            //     {
            //         var ldto = new PbsInstructionLinkDto
            //         {
            //             Id = link.Id,
            //             Title = link.Title,
            //             Type = link.Type,
            //             Value = link.Link,
            //             PbsInstructionId = link.PbsInstructionId
            //         };
            //         instructionLinkDtos.Add(ldto);
            //     }
            //
            //     dto.PbsInstructionLink = instructionLinkDtos;
            //     instructionLoadDtos.Add(dto);
            // }

            return instructionDto;
        }
    }

    public async Task<IEnumerable<PbsInstructionLoadDto>> GetPipingInstrumentationDiagramByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        // 100 = technical, 200 = environmental, 300 = safety, 400 = health
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var lang = pbsInstructionsRepositoryParameter.Lang;

            var sql2 = @"SELECT
  Instructions.*
,PbsInstruction.Id As PbsInstructionId
,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
 ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
 ,PbsInstructionFamilyLocalizedData.Family AS Text
FROM dbo.PbsInstruction
LEFT OUTER JOIN dbo.Instructions
  ON PbsInstruction.InstructionsId = Instructions.Id
LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
  ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
OR Instructions.PbsInstructionFamilyId IS NULL)
AND PbsInstruction.PbsProductId  = @PbsProductId
AND Instructions.InstructionType = @InstructionType AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false' AND PbsInstructionFamilyLocalizedData.InstructionFamilyID ='915cf56a-09ba-48b3-bcc5-2ac1fe64f4ca'";

            var sqlLink =
                @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

            var param = new { pbsInstructionsRepositoryParameter.PbsProductId };


            //List<PbsInstructionLoadDto> instructionLoadDtos = new List<PbsInstructionLoadDto>();
            var pbsInstructionLoadAllPmolDto = new PbsInstructionLoadAllPmolDto();
            await using var connection = new SqlConnection(connectionString);
            var instructionDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    },
                    new
                    {
                        lang = pbsInstructionsRepositoryParameter.Lang,
                        pbsInstructionsRepositoryParameter.PbsProductId,
                        pbsInstructionsRepositoryParameter.InstructionType
                    },
                    splitOn: "Key").ToList();

            Parallel.ForEach(instructionDto, instructionLoadDtoss =>
            {
                using var connectionX = new SqlConnection(connectionString);
                instructionLoadDtoss.PbsInstructionLink = connectionX
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();
            });
            // var pbsInstructions = context.PbsInstruction
            //     .Where(r => r.IsDeleted == false && r.PbsProductId ==
            //                                      pbsInstructionsRepositoryParameter.PbsProductId
            //                                      && r.InstructionType.Equals(pbsInstructionsRepositoryParameter
            //                                          .InstructionType)
            //                                      && r.PbsInstructionFamily.Family.Equals(
            //                                          "Piping and Instrumentation"))
            //     .Include(p => p.PbsInstructionFamily)
            //     .Include(p => p.PbsInstructionLink).ToList();
            //
            // //var riskDto = new RiskReadDto();
            // if (pbsInstructions.Any())
            //     //riskDto.SequenceCode = instruction.SequenceCode;
            //     //riskDto.Name = instruction.Name;
            //     //riskDto.Title = instruction.Title;
            //     //riskDto.HeaderTitle = instruction.HeaderTitle;
            //     //riskDto.RiskDetails = instruction.RiskDetails;
            //     foreach (var pbsInstruction in pbsInstructions)
            //         if (!(lang == Language.en.ToString() || string.IsNullOrEmpty(lang)))
            //             if (pbsInstruction.PbsInstructionFamily != null)
            //             {
            //                 var localizedData = context.LocalizedData.FirstOrDefault(ld =>
            //                     ld.LocaleCode == pbsInstruction.PbsInstructionFamily.LocaleCode &&
            //                     ld.LanguageCode == lang);
            //                 if (localizedData != null)
            //                     pbsInstruction.PbsInstructionFamily.Family = localizedData.Label;
            //             }
            //
            // var instructionLoadDtos = new List<PbsInstructionLoadDto>();
            // foreach (var pbsInstruction in pbsInstructions)
            // {
            //     var dto = new PbsInstructionLoadDto
            //     {
            //         Id = pbsInstruction.Id,
            //         SequenceCode = pbsInstruction.SequenceCode,
            //         Name = pbsInstruction.Name,
            //         InstructionType = pbsInstruction.InstructionType,
            //         InstructionsDetails = pbsInstruction.InstructionsDetails,
            //         PbsProductId = pbsInstruction.PbsProductId,
            //         PbsProduct = pbsInstruction.PbsProduct,
            //         PbsInstructionFamilyId = pbsInstruction.PbsInstructionFamilyId
            //     };
            //
            //     if (pbsInstruction.PbsInstructionFamily != null)
            //     {
            //         dto.PbsInstructionFamily = new PbsInstructionFamilyLoadDto
            //         {
            //             Key = pbsInstruction.PbsInstructionFamily.Id,
            //             Text = pbsInstruction.PbsInstructionFamily.Family
            //         };
            //         dto.PbsInstructionFamilyName = pbsInstruction.PbsInstructionFamily.Family;
            //     }
            //
            //     var instructionLinkDtos = new List<PbsInstructionLinkDto>();
            //     foreach (var link in pbsInstruction.PbsInstructionLink)
            //     {
            //         var ldto = new PbsInstructionLinkDto
            //         {
            //             Id = link.Id,
            //             Title = link.Title,
            //             Type = link.Type,
            //             Value = link.Link,
            //             PbsInstructionId = link.PbsInstructionId
            //         };
            //         instructionLinkDtos.Add(ldto);
            //     }
            //
            //     dto.PbsInstructionLink = instructionLinkDtos;
            //     instructionLoadDtos.Add(dto);
            // }

            return instructionDto;
        }
    }

    public async Task<IEnumerable<PbsInstructionLoadDto>> GetWeldingProceduresSpecificationsPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        // 100 = technical, 200 = environmental, 300 = safety, 400 = health
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var lang = pbsInstructionsRepositoryParameter.Lang;

            var sql2 = @"SELECT
  Instructions.*
,PbsInstruction.Id As PbsInstructionId
,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
 ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
 ,PbsInstructionFamilyLocalizedData.Family AS Text
FROM dbo.PbsInstruction
LEFT OUTER JOIN dbo.Instructions
  ON PbsInstruction.InstructionsId = Instructions.Id
LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
  ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
OR Instructions.PbsInstructionFamilyId IS NULL)
AND PbsInstruction.PbsProductId  = @PbsProductId
AND Instructions.InstructionType = @InstructionType AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false' AND PbsInstructionFamilyLocalizedData.InstructionFamilyID ='0e5f0b46-0602-4e36-8227-256d487ec127'";

            var sqlLink =
                @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

            var param = new { pbsInstructionsRepositoryParameter.PbsProductId };


            //List<PbsInstructionLoadDto> instructionLoadDtos = new List<PbsInstructionLoadDto>();
            var pbsInstructionLoadAllPmolDto = new PbsInstructionLoadAllPmolDto();
            await using var connection = new SqlConnection(connectionString);
            var instructionDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    },
                    new
                    {
                        lang = pbsInstructionsRepositoryParameter.Lang,
                        pbsInstructionsRepositoryParameter.PbsProductId,
                        pbsInstructionsRepositoryParameter.InstructionType
                    },
                    splitOn: "Key").ToList();

            Parallel.ForEach(instructionDto, instructionLoadDtoss =>
            {
                using var connectionX = new SqlConnection(connectionString);
                instructionLoadDtoss.PbsInstructionLink = connectionX
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();
            });
            // var pbsInstructions = context.PbsInstruction
            //     .Where(r => r.IsDeleted == false && r.PbsProductId ==
            //                                      pbsInstructionsRepositoryParameter.PbsProductId
            //                                      && r.InstructionType.Equals(pbsInstructionsRepositoryParameter
            //                                          .InstructionType)
            //                                      && r.PbsInstructionFamily.Family.Equals("Welding Procedures"))
            //     .Include(p => p.PbsInstructionFamily)
            //     .Include(p => p.PbsInstructionLink).ToList();
            //
            // //var riskDto = new RiskReadDto();
            // if (pbsInstructions.Any())
            //     //riskDto.SequenceCode = instruction.SequenceCode;
            //     //riskDto.Name = instruction.Name;
            //     //riskDto.Title = instruction.Title;
            //     //riskDto.HeaderTitle = instruction.HeaderTitle;
            //     //riskDto.RiskDetails = instruction.RiskDetails;
            //     foreach (var pbsInstruction in pbsInstructions)
            //         if (!(lang == Language.en.ToString() || string.IsNullOrEmpty(lang)))
            //             if (pbsInstruction.PbsInstructionFamily != null)
            //             {
            //                 var localizedData = context.LocalizedData.FirstOrDefault(ld =>
            //                     ld.LocaleCode == pbsInstruction.PbsInstructionFamily.LocaleCode &&
            //                     ld.LanguageCode == lang);
            //                 if (localizedData != null)
            //                     pbsInstruction.PbsInstructionFamily.Family = localizedData.Label;
            //             }
            //
            // var instructionLoadDtos = new List<PbsInstructionLoadDto>();
            // foreach (var pbsInstruction in pbsInstructions)
            // {
            //     var dto = new PbsInstructionLoadDto
            //     {
            //         Id = pbsInstruction.Id,
            //         SequenceCode = pbsInstruction.SequenceCode,
            //         Name = pbsInstruction.Name,
            //         InstructionType = pbsInstruction.InstructionType,
            //         InstructionsDetails = pbsInstruction.InstructionsDetails,
            //         PbsProductId = pbsInstruction.PbsProductId,
            //         PbsProduct = pbsInstruction.PbsProduct,
            //         PbsInstructionFamilyId = pbsInstruction.PbsInstructionFamilyId
            //     };
            //
            //     if (pbsInstruction.PbsInstructionFamily != null)
            //     {
            //         dto.PbsInstructionFamily = new PbsInstructionFamilyLoadDto
            //         {
            //             Key = pbsInstruction.PbsInstructionFamily.Id,
            //             Text = pbsInstruction.PbsInstructionFamily.Family
            //         };
            //         dto.PbsInstructionFamilyName = pbsInstruction.PbsInstructionFamily.Family;
            //     }
            //
            //     var instructionLinkDtos = new List<PbsInstructionLinkDto>();
            //     foreach (var link in pbsInstruction.PbsInstructionLink)
            //     {
            //         var ldto = new PbsInstructionLinkDto
            //         {
            //             Id = link.Id,
            //             Title = link.Title,
            //             Type = link.Type,
            //             Value = link.Link,
            //             PbsInstructionId = link.PbsInstructionId
            //         };
            //         instructionLinkDtos.Add(ldto);
            //     }
            //
            //     dto.PbsInstructionLink = instructionLinkDtos;
            //     instructionLoadDtos.Add(dto);
            // }

            return instructionDto;
        }
    }

    public async Task<IEnumerable<PbsInstructionLoadDto>> GetIsoMetricDrawingsPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        // 100 = technical, 200 = environmental, 300 = safety, 400 = health
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var lang = pbsInstructionsRepositoryParameter.Lang;

            var sql2 = @"SELECT
  Instructions.*
,PbsInstruction.Id As PbsInstructionId
,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
 ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
 ,PbsInstructionFamilyLocalizedData.Family AS Text
FROM dbo.PbsInstruction
LEFT OUTER JOIN dbo.Instructions
  ON PbsInstruction.InstructionsId = Instructions.Id
LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
  ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
OR Instructions.PbsInstructionFamilyId IS NULL)
AND PbsInstruction.PbsProductId  = @PbsProductId
AND Instructions.InstructionType = @InstructionType AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false' AND PbsInstructionFamilyLocalizedData.InstructionFamilyID ='975cb86b-d47d-4b8c-b77f-b3c67adb3fb0'";

            var sqlLink =
                @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

            var param = new { pbsInstructionsRepositoryParameter.PbsProductId };


            //List<PbsInstructionLoadDto> instructionLoadDtos = new List<PbsInstructionLoadDto>();
            var pbsInstructionLoadAllPmolDto = new PbsInstructionLoadAllPmolDto();
            await using var connection = new SqlConnection(connectionString);
            var instructionDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    },
                    new
                    {
                        lang = pbsInstructionsRepositoryParameter.Lang,
                        pbsInstructionsRepositoryParameter.PbsProductId,
                        pbsInstructionsRepositoryParameter.InstructionType
                    },
                    splitOn: "Key").ToList();

            Parallel.ForEach(instructionDto, instructionLoadDtoss =>
            {
                using var connectionX = new SqlConnection(connectionString);
                instructionLoadDtoss.PbsInstructionLink = connectionX
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();
            });
            // var pbsInstructions = context.PbsInstruction
            //     .Where(r => r.IsDeleted == false && r.PbsProductId ==
            //                                      pbsInstructionsRepositoryParameter.PbsProductId
            //                                      && r.InstructionType.Equals(pbsInstructionsRepositoryParameter
            //                                          .InstructionType)
            //                                      && r.PbsInstructionFamily.Family.Equals("Isometric"))
            //     .Include(p => p.PbsInstructionFamily)
            //     .Include(p => p.PbsInstructionLink).ToList();
            //
            // //var riskDto = new RiskReadDto();
            // if (pbsInstructions.Any())
            //     //riskDto.SequenceCode = instruction.SequenceCode;
            //     //riskDto.Name = instruction.Name;
            //     //riskDto.Title = instruction.Title;
            //     //riskDto.HeaderTitle = instruction.HeaderTitle;
            //     //riskDto.RiskDetails = instruction.RiskDetails;
            //     foreach (var pbsInstruction in pbsInstructions)
            //         if (!(lang == Language.en.ToString() || string.IsNullOrEmpty(lang)))
            //             if (pbsInstruction.PbsInstructionFamily != null)
            //             {
            //                 var localizedData = context.LocalizedData.FirstOrDefault(ld =>
            //                     ld.LocaleCode == pbsInstruction.PbsInstructionFamily.LocaleCode &&
            //                     ld.LanguageCode == lang);
            //                 if (localizedData != null)
            //                     pbsInstruction.PbsInstructionFamily.Family = localizedData.Label;
            //             }
            //
            // var instructionLoadDtos = new List<PbsInstructionLoadDto>();
            // foreach (var pbsInstruction in pbsInstructions)
            // {
            //     var dto = new PbsInstructionLoadDto
            //     {
            //         Id = pbsInstruction.Id,
            //         SequenceCode = pbsInstruction.SequenceCode,
            //         Name = pbsInstruction.Name,
            //         InstructionType = pbsInstruction.InstructionType,
            //         InstructionsDetails = pbsInstruction.InstructionsDetails,
            //         PbsProductId = pbsInstruction.PbsProductId,
            //         PbsProduct = pbsInstruction.PbsProduct,
            //         PbsInstructionFamilyId = pbsInstruction.PbsInstructionFamilyId
            //     };
            //
            //     if (pbsInstruction.PbsInstructionFamily != null)
            //     {
            //         dto.PbsInstructionFamily = new PbsInstructionFamilyLoadDto
            //         {
            //             Key = pbsInstruction.PbsInstructionFamily.Id,
            //             Text = pbsInstruction.PbsInstructionFamily.Family
            //         };
            //         dto.PbsInstructionFamilyName = pbsInstruction.PbsInstructionFamily.Family;
            //     }
            //
            //     var instructionLinkDtos = new List<PbsInstructionLinkDto>();
            //     foreach (var link in pbsInstruction.PbsInstructionLink)
            //     {
            //         var ldto = new PbsInstructionLinkDto
            //         {
            //             Id = link.Id,
            //             Title = link.Title,
            //             Type = link.Type,
            //             Value = link.Link,
            //             PbsInstructionId = link.PbsInstructionId
            //         };
            //         instructionLinkDtos.Add(ldto);
            //     }
            //
            //     dto.PbsInstructionLink = instructionLinkDtos;
            //     instructionLoadDtos.Add(dto);
            // }

            return instructionDto;
        }
    }

    public async Task<IEnumerable<PbsInstructionLoadDto>> GetHealthSafetyEnvironmenInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        // 100 = technical, 200 = environmental, 300 = safety, 400 = health
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var lang = pbsInstructionsRepositoryParameter.Lang;

            var sql2 = @"SELECT
  Instructions.*
,PbsInstruction.Id As PbsInstructionId
,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
 ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
 ,PbsInstructionFamilyLocalizedData.Family AS Text
FROM dbo.PbsInstruction
LEFT OUTER JOIN dbo.Instructions
  ON PbsInstruction.InstructionsId = Instructions.Id
LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
  ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
OR Instructions.PbsInstructionFamilyId IS NULL)
AND PbsInstruction.PbsProductId  = @PbsProductId
AND Instructions.InstructionType != '100' AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false' ";

            var sqlLink =
                @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

            var param = new { pbsInstructionsRepositoryParameter.PbsProductId };


            //List<PbsInstructionLoadDto> instructionLoadDtos = new List<PbsInstructionLoadDto>();
            var pbsInstructionLoadAllPmolDto = new PbsInstructionLoadAllPmolDto();
            await using var connection = new SqlConnection(connectionString);
            var instructionDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    },
                    new
                    {
                        lang = pbsInstructionsRepositoryParameter.Lang,
                        pbsInstructionsRepositoryParameter.PbsProductId,
                        pbsInstructionsRepositoryParameter.InstructionType
                    },
                    splitOn: "Key").ToList();

            Parallel.ForEach(instructionDto, instructionLoadDtoss =>
            {
                using var connectionX = new SqlConnection(connectionString);
                instructionLoadDtoss.PbsInstructionLink = connectionX
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();
            });
            // var pbsInstructions = context.PbsInstruction
            //     .Where(r => r.IsDeleted == false && r.PbsProductId ==
            //                                      pbsInstructionsRepositoryParameter.PbsProductId
            //                                      && (r.InstructionType.Equals("200") ||
            //                                          r.InstructionType.Equals("300") ||
            //                                          r.InstructionType.Equals("400")))
            //     .Include(p => p.PbsInstructionFamily)
            //     .Include(p => p.PbsInstructionLink).ToList();
            //
            // //var riskDto = new RiskReadDto();
            // if (pbsInstructions.Any())
            //     //riskDto.SequenceCode = instruction.SequenceCode;
            //     //riskDto.Name = instruction.Name;
            //     //riskDto.Title = instruction.Title;
            //     //riskDto.HeaderTitle = instruction.HeaderTitle;
            //     //riskDto.RiskDetails = instruction.RiskDetails;
            //     foreach (var pbsInstruction in pbsInstructions)
            //         if (string.IsNullOrEmpty(pbsInstructionsRepositoryParameter.Lang) ||
            //             pbsInstructionsRepositoryParameter.Lang.Contains("en"))
            //             if (pbsInstruction.PbsInstructionFamily != null)
            //             {
            //                 var localizedData = context.LocalizedData.FirstOrDefault(ld =>
            //                     ld.LocaleCode == pbsInstruction.PbsInstructionFamily.LocaleCode &&
            //                     ld.LanguageCode == lang);
            //                 if (localizedData != null)
            //                     pbsInstruction.PbsInstructionFamily.Family = localizedData.Label;
            //             }
            //
            // var instructionLoadDtos = new List<PbsInstructionLoadDto>();
            // foreach (var pbsInstruction in pbsInstructions)
            // {
            //     var dto = new PbsInstructionLoadDto
            //     {
            //         Id = pbsInstruction.Id,
            //         SequenceCode = pbsInstruction.SequenceCode,
            //         Name = pbsInstruction.Name,
            //         InstructionType = pbsInstruction.InstructionType,
            //         InstructionsDetails = pbsInstruction.InstructionsDetails,
            //         PbsProductId = pbsInstruction.PbsProductId,
            //         PbsProduct = pbsInstruction.PbsProduct,
            //         PbsInstructionFamilyId = pbsInstruction.PbsInstructionFamilyId
            //     };
            //
            //     if (pbsInstruction.PbsInstructionFamily != null)
            //     {
            //         dto.PbsInstructionFamily = new PbsInstructionFamilyLoadDto
            //         {
            //             Key = pbsInstruction.PbsInstructionFamily.Id,
            //             Text = pbsInstruction.PbsInstructionFamily.Family
            //         };
            //         dto.PbsInstructionFamilyName = pbsInstruction.PbsInstructionFamily.Family;
            //     }
            //
            //     var instructionLinkDtos = new List<PbsInstructionLinkDto>();
            //     foreach (var link in pbsInstruction.PbsInstructionLink)
            //     {
            //         var ldto = new PbsInstructionLinkDto
            //         {
            //             Id = link.Id,
            //             Title = link.Title,
            //             Type = link.Type,
            //             Value = link.Link,
            //             PbsInstructionId = link.PbsInstructionId
            //         };
            //         instructionLinkDtos.Add(ldto);
            //     }
            //
            //     dto.PbsInstructionLink = instructionLinkDtos;
            //     instructionLoadDtos.Add(dto);
            // }

            return instructionDto;
        }
    }

    public Task<IEnumerable<PbsInstructionLoadDto>> GetAllInstructionsByPbsProductIdAll(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        throw new NotImplementedException();
    }

    public async Task<string> CreatePbsInstruction(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using var context = new ShanukaDbContext(options, connectionString,
            pbsInstructionsRepositoryParameter.TenantProvider);

        var exist = context.PbsInstruction
            .Any(x => x.InstructionsId == pbsInstructionsRepositoryParameter.CreateInstruction.InstructionsId &&
                      x.PbsProductId == pbsInstructionsRepositoryParameter.CreateInstruction.PbsProductId &&
                      x.IsDeleted == false);
        if (!exist)
        {
            var pbsInst = new PbsInstruction
            {
                Id = pbsInstructionsRepositoryParameter.CreateInstruction.Id,
                InstructionsId = pbsInstructionsRepositoryParameter.CreateInstruction.InstructionsId,
                PbsProductId = pbsInstructionsRepositoryParameter.CreateInstruction.PbsProductId,
                IsDeleted = false,
                IsSaved = false
            };

            await context.PbsInstruction.AddAsync(pbsInst);
            await context.SaveChangesAsync();
        }

        return pbsInstructionsRepositoryParameter.CreateInstruction.Id;
    }

    public async Task<IEnumerable<PbsInstructionLoadDto>> ReadAllInstructionsByPbsProductId(
        PbsInstructionsRepositoryParameter pbsInstructionsRepositoryParameter)
    {
        // 100 = technical, 200 = environmental, 300 = safety, 400 = health
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            pbsInstructionsRepositoryParameter.ContractingUnitSequenceId,
            pbsInstructionsRepositoryParameter.ProjectSequenceId,
            pbsInstructionsRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         pbsInstructionsRepositoryParameter.TenantProvider))
        {
            var lang = pbsInstructionsRepositoryParameter.Lang;

            var sql2 = @"SELECT
  Instructions.*
,PbsInstruction.Id As PbsInstructionId
,PbsInstructionFamilyLocalizedData.Family AS PbsInstructionFamilyName
 ,PbsInstructionFamilyLocalizedData.InstructionFamilyID AS [Key]
 ,PbsInstructionFamilyLocalizedData.Family AS Text
FROM dbo.PbsInstruction
LEFT OUTER JOIN dbo.Instructions
  ON PbsInstruction.InstructionsId = Instructions.Id
LEFT OUTER JOIN dbo.PbsInstructionFamilyLocalizedData
  ON Instructions.PbsInstructionFamilyId = PbsInstructionFamilyLocalizedData.InstructionFamilyID
WHERE (PbsInstructionFamilyLocalizedData.LocaleCode = @lang
OR Instructions.PbsInstructionFamilyId IS NULL)
AND PbsInstruction.PbsProductId  = @PbsProductId 
AND Instructions.IsDeleted = 'false' AND PbsInstruction.IsDeleted = 'false' ";

            var sqlLink =
                @"SELECT PbsInstructionLink.*,PbsInstructionLink.Link As Value FROM dbo.PbsInstructionLink WHERE PbsInstructionLink.PbsInstructionId = @PbsInstructionId";

            var param = new { pbsInstructionsRepositoryParameter.PbsProductId };


            //List<PbsInstructionLoadDto> instructionLoadDtos = new List<PbsInstructionLoadDto>();
            var pbsInstructionLoadAllPmolDto = new PbsInstructionLoadAllPmolDto();
            await using var connection = new SqlConnection(connectionString);
            var instructionDto = connection
                .Query<PbsInstructionLoadDto, PbsInstructionFamilyLoadDto, PbsInstructionLoadDto>(
                    sql2,
                    (pbsInstruction, pbsInstructionFamily) =>
                    {
                        pbsInstruction.PbsInstructionFamily = pbsInstructionFamily;

                        return pbsInstruction;
                    },
                    new
                    {
                        lang = pbsInstructionsRepositoryParameter.Lang,
                        pbsInstructionsRepositoryParameter.PbsProductId,
                        pbsInstructionsRepositoryParameter.InstructionType
                    },
                    splitOn: "Key").ToList();

            Parallel.ForEach(instructionDto, instructionLoadDtoss =>
            {
                using var connectionX = new SqlConnection(connectionString);
                instructionLoadDtoss.PbsInstructionLink = connectionX
                    .Query<PbsInstructionLinkDto>(sqlLink, new { PbsInstructionId = instructionLoadDtoss.Id })
                    .ToList();
            });


            return instructionDto;
        }
    }
}