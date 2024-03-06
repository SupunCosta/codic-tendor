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
using UPrinceV4.Web.Repositories.Interfaces.PBS;
using UPrinceV4.Web.Util;

namespace UPrinceV4.Web.Repositories.PBS;

public class CompetenciesRepository : ICompetenciesRepository
{
    public async Task<IEnumerable<PbsSkillExperience>> GetCompetencies(
        CompetenciesRepositoryParameter competenciesRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        // ApplicationDbContext applicationDbContext = new ApplicationDbContext(options, competenciesRepositoryParameter.TenantProvider);
        var connectionString = ConnectionString.MapConnectionString(
            competenciesRepositoryParameter.ContractingUnitSequenceId,
            competenciesRepositoryParameter.ProjectSequenceId, competenciesRepositoryParameter.TenantProvider);
        using var context = new ShanukaDbContext(options, connectionString,
            competenciesRepositoryParameter.TenantProvider);
        var lang = competenciesRepositoryParameter.Lang;
        var competenceList = context.PbsSkillExperience
            .Include(p => p.PbsExperience)
            .Include(p => p.PbsProduct)
            .Include(p => p.PbsSkill).ToList();
        if (!(lang == Language.en.ToString() || string.IsNullOrEmpty(lang)))
            foreach (var com in competenceList)
            {
                if (com.PbsSkill != null)
                {
                    var localizedData = context.LocalizedData.FirstOrDefault(ld =>
                        ld.LocaleCode == com.PbsSkill.LocaleCode && ld.LanguageCode == lang);
                    if (localizedData != null) com.PbsSkill.Title = localizedData.Label;
                }

                if (com.PbsExperience != null)
                {
                    var localizedData = context.LocalizedData.FirstOrDefault(ld =>
                        ld.LocaleCode == com.PbsExperience.LocaleCode && ld.LanguageCode == lang);
                    if (localizedData != null) com.PbsExperience.Name = localizedData.Label;
                }
            }

        return competenceList;
    }

    public async Task<PbsSkillExperience> GetCompetenceById(
        CompetenciesRepositoryParameter competenciesRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            competenciesRepositoryParameter.ContractingUnitSequenceId,
            competenciesRepositoryParameter.ProjectSequenceId, competenciesRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         competenciesRepositoryParameter.TenantProvider))
        {
            var lang = competenciesRepositoryParameter.Lang;
            var id = competenciesRepositoryParameter.Id;
            var competence = context.PbsSkillExperience.Where(p => p.Id.Equals(id))
                .Include(p => p.PbsExperience)
                .Include(p => p.PbsProduct)
                .Include(p => p.PbsSkill).ToList().FirstOrDefault();

            if (!(lang == Language.en.ToString() || string.IsNullOrEmpty(lang)))
            {
                if (competence != null && competence.PbsSkill != null)
                {
                    var localizedData = context.LocalizedData.FirstOrDefault(ld =>
                        ld.LocaleCode == competence.PbsSkill.LocaleCode && ld.LanguageCode == lang);
                    if (localizedData != null) competence.PbsSkill.Title = localizedData.Label;
                }

                if (competence != null && competence.PbsExperience != null)
                {
                    var localizedData = context.LocalizedData.FirstOrDefault(ld =>
                        ld.LocaleCode == competence.PbsExperience.LocaleCode && ld.LanguageCode == lang);
                    if (localizedData != null) competence.PbsExperience.Name = localizedData.Label;
                }
            }

            return competence;
        }
    }

    public async Task<string> AddCompetence(CompetenciesRepositoryParameter competenciesRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            competenciesRepositoryParameter.ContractingUnitSequenceId,
            competenciesRepositoryParameter.ProjectSequenceId, competenciesRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         competenciesRepositoryParameter.TenantProvider))
        {
            var skillExp = competenciesRepositoryParameter.PbsSkillExperience;
            var dbContext = competenciesRepositoryParameter.ApplicationDbContext;
            var existingSkillExp = context.PbsSkillExperience.FirstOrDefault(s => s.Id.Equals(skillExp.Id));

            if (existingSkillExp != null)
            {
                existingSkillExp.PbsExperienceId = skillExp.PbsExperienceId;
                existingSkillExp.PbsProductId = skillExp.PbsProductId;
                existingSkillExp.PbsSkillId = skillExp.PbsSkillId;
                context.Update(existingSkillExp);
                await context.SaveChangesAsync();
                return existingSkillExp.Id;
            }

            skillExp.Id = Guid.NewGuid().ToString();
            context.Add(skillExp);
            await context.SaveChangesAsync();
            return skillExp.Id;
        }
    }

    public async Task<bool> DeleteCompetencies(CompetenciesRepositoryParameter competenciesRepositoryParameter)
    {
        var options = new DbContextOptions<ShanukaDbContext>();
        var connectionString = ConnectionString.MapConnectionString(
            competenciesRepositoryParameter.ContractingUnitSequenceId,
            competenciesRepositoryParameter.ProjectSequenceId, competenciesRepositoryParameter.TenantProvider);
        await using (var context = new ShanukaDbContext(options, connectionString,
                         competenciesRepositoryParameter.TenantProvider))
        {
            var isUpdated = false;
            foreach (var id in competenciesRepositoryParameter.IdList)
            {
                var competence = context.PbsSkillExperience
                    .FirstOrDefault(p => p.Id.Equals(id));
                if (competence != null)
                {
                    context.PbsSkillExperience.Remove(competence);
                    await context.SaveChangesAsync();
                    isUpdated = true;
                }
            }

            return isUpdated;
        }
    }

    public async Task<CompetenciesDropdown> GetCompetenciesDropdownData(
        CompetenciesRepositoryParameter competenciesRepositoryParameter)
    {
        var competenciesDropdown = new CompetenciesDropdown
        {
            PbsSkills = competenciesRepositoryParameter.IPbsSkillRepository.GetSkillList(competenciesRepositoryParameter
                .PbsSkillRepositoryParameter),
            PbsExperience = competenciesRepositoryParameter.IPbsExperienceRepository.GetExperienceList(
                competenciesRepositoryParameter
                    .PbsExperienceRepositoryParameter)
        };

        return competenciesDropdown;
    }

    public async Task<IEnumerable<PbsSkillExperienceDto>> GetCompetenceByPbsId(
        CompetenciesRepositoryParameter competenciesRepositoryParameter)
    {
        var lang = competenciesRepositoryParameter.Lang;

        var sql = @"SELECT
                            PbsSkillExperience.PbsProductId
                            ,PbsSkillExperience.Id
                            ,PbsSkillLocalizedData.PbsSkillId AS SkillId
                            ,PbsSkillLocalizedData.Label AS Skill
                            ,PbsExperienceLocalizedData.PbsExperienceId AS ExperienceId
                            ,PbsExperienceLocalizedData.Label AS Experience
                             FROM dbo.PbsSkillExperience
                             LEFT OUTER JOIN dbo.PbsSkillLocalizedData
                            ON PbsSkillExperience.PbsSkillId = PbsSkillLocalizedData.PbsSkillId
                            LEFT OUTER JOIN dbo.PbsExperienceLocalizedData
                            ON PbsSkillExperience.PbsExperienceId = PbsExperienceLocalizedData.PbsExperienceId
                            WHERE PbsSkillLocalizedData.LanguageCode = @lang
                            AND PbsExperienceLocalizedData.LanguageCode = @lang 
                            AND PbsSkillExperience.PbsProductId = @pbsId
                            order by Id";
        var connectionString = ConnectionString.MapConnectionString(
            competenciesRepositoryParameter.ContractingUnitSequenceId,
            competenciesRepositoryParameter.ProjectSequenceId, competenciesRepositoryParameter.TenantProvider);
        var parameters = new { lang, pbsId = competenciesRepositoryParameter.PbsId };
        await using (var dbConnection = new SqlConnection(connectionString))
        {
            var result = dbConnection.Query<PbsSkillExperienceDto>(sql, parameters);
            

            return result;
        }
    }
}