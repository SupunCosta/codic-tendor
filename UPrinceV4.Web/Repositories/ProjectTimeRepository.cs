using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class ProjectTimeRepository : IProjectTimeRepository
{
    public async Task<string> CreateProjectTime(ProjectTimeCreateDto timedto,
        ApplicationUser user, ITenantProvider iTenantProvider)
    {
        
        try
        {
            var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
            var context =
                new ProjectDefinitionDbContext(options1,iTenantProvider);
            var time = new ProjectTime();
            if (timedto.CalendarTemplateId == null)
            {
                var calId = context.CalendarTemplate.First(t => t.IsDefault == true).Id;
                time.CalendarTemplateId = calId;
            }
            else
            {
                time.CalendarTemplateId = timedto.CalendarTemplateId;
            }

            if (time.ExpectedEndDate != null && time.StartDate != null && time.ExpectedEndDate < time.StartDate)
                throw new Exception("Expected End date can't be earlier than start date");

            if (time.EndDate != null && time.StartDate != null && time.EndDate < time.StartDate)
                throw new Exception("End date can't be earlier than start date");

            time.EndDate = timedto.EndDate;
            time.ExpectedEndDate = timedto.ExpectedEndDate;
            time.Id = Guid.NewGuid().ToString();
            time.ProjectId = timedto.ProjectId;
            time.StartDate = timedto.StartDate;
            time.TenderStartDate = timedto.TenderStartDate;
            time.TenderEndDate = timedto.TenderEndDate;
            context.ProjectTime.Add(time);
            await context.SaveChangesAsync();
            return time.Id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public bool DeleteProjectTime(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Currency> GetProjectTimeById(ApplicationDbContext context, string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ProjectTime>> GetProjectTimes(ApplicationDbContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UpdateProjectTime(ProjectTimeUpdateDto timedto,
        ApplicationUser user, ITenantProvider iTenantProvider)
    {
        try
        {
            var time = new ProjectTime();
            var options1 = new DbContextOptions<ProjectDefinitionDbContext>();
            var context =
                new ProjectDefinitionDbContext(options1,iTenantProvider);
            if (timedto.Id != null)
            {


                if (timedto.CalendarTemplateId == null)
                {
                    var calId = context.CalendarTemplate.First(t => t.IsDefault == true).Id;
                    time.CalendarTemplateId = calId;
                }
                else
                {
                    time.CalendarTemplateId = timedto.CalendarTemplateId;
                }

                if (time.ExpectedEndDate != null && time.StartDate != null && time.ExpectedEndDate < time.StartDate)
                    throw new Exception("Expected End date can't be earlier than start date");

                if (time.EndDate != null && time.StartDate != null && time.EndDate < time.StartDate)
                    throw new Exception("End date can't be earlier than start date");

                time.EndDate = timedto.EndDate;
                time.ExpectedEndDate = timedto.ExpectedEndDate;
                time.Id = timedto.Id;
                time.ProjectId = timedto.ProjectId;
                time.StartDate = timedto.StartDate;
                //time.ChangeByUserId = user.Id;
                //time.Action = HistoryState.UPDATED.ToString();
                time.TenderStartDate = timedto.TenderStartDate;
                time.TenderEndDate = timedto.TenderEndDate;
                context.ProjectTime.Update(time);
                await context.SaveChangesAsync();
                return time.Id;
            }
            else
            {
                if (timedto.CalendarTemplateId == null)
                {
                    var calId = context.CalendarTemplate.First(t => t.IsDefault == true).Id;
                    time.CalendarTemplateId = calId;
                }
                else
                {
                    time.CalendarTemplateId = timedto.CalendarTemplateId;
                }

                if (time.ExpectedEndDate != null && time.StartDate != null && time.ExpectedEndDate < time.StartDate)
                    throw new Exception("Expected End date can't be earlier than start date");

                if (time.EndDate != null && time.StartDate != null && time.EndDate < time.StartDate)
                    throw new Exception("End date can't be earlier than start date");

                time.EndDate = timedto.EndDate;
                time.ExpectedEndDate = timedto.ExpectedEndDate;
                time.Id = Guid.NewGuid().ToString();
                time.ProjectId = timedto.ProjectId;
                time.StartDate = timedto.StartDate;
                time.TenderStartDate = timedto.TenderStartDate;
                time.TenderEndDate = timedto.TenderEndDate;
                context.ProjectTime.Add(time);
                await context.SaveChangesAsync();            
            }

            return time.Id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}