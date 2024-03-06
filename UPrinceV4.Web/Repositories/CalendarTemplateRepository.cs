using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Repositories.Interfaces;

namespace UPrinceV4.Web.Repositories;

public class CalendarTemplateRepository : ICalendarTemplateRepository
{
    public async Task<string> CreateCalendarTemplate(ApplicationDbContext context,
        CalendarTemplateCreateDto calendarTemplateCreateDto)
    {
        if (calendarTemplateCreateDto.IsDefault)
        {
            var defaultValue = context.CalendarTemplate.Where(p => p.IsDefault == true);
            if (defaultValue.Count() != 0)
            {
                defaultValue.First().IsDefault = false;
                context.CalendarTemplate.Update(defaultValue.First());
            }
        }

        var calendar = new CalendarTemplate
        {
            Id = Guid.NewGuid().ToString(),
            IsDefault = calendarTemplateCreateDto.IsDefault,
            Name = calendarTemplateCreateDto.Name
        };
        context.CalendarTemplate.Add(calendar);
        await context.SaveChangesAsync();
        return calendar.Id;
    }

    public async Task<bool> DeleteCalendarTemplate(ApplicationDbContext context, string id)
    {
        var cal = (from a in context.CalendarTemplate
            where a.Id == id
            select a).Single();
        context.CalendarTemplate.Remove(cal);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<CalendarTemplate> GetCalendarTemplateById(ApplicationDbContext context, string id)
    {
        var calenderTemplate = context.CalendarTemplate.FirstOrDefault(p => p.Id == id);
        return calenderTemplate;
    }

    public async Task<IEnumerable<CalendarTemplate>> GetCalendarTemplates(ApplicationDbContext context)
    {
        var calenderTemplates = context.CalendarTemplate.ToList();
        return calenderTemplates;
    }

    public async Task<string> UpdateCalendarTemplate(ApplicationDbContext context,
        CalendarTemplate calendarTemplate)
    {
        if (calendarTemplate.IsDefault)
        {
            var defaultValue = context.CalendarTemplate.Where(p => p.IsDefault == true);
            if (defaultValue.Count() != 0)
            {
                defaultValue.First().IsDefault = false;
                context.CalendarTemplate.Update(defaultValue.First());
            }
        }

        var calendar = new CalendarTemplate
        {
            Id = calendarTemplate.Id,
            IsDefault = calendarTemplate.IsDefault,
            Name = calendarTemplate.Name
        };
        context.CalendarTemplate.Update(calendar);
        await context.SaveChangesAsync();
        return calendar.Id;
    }
}