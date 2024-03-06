using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface ICalendarTemplateRepository
{
    Task<IEnumerable<CalendarTemplate>> GetCalendarTemplates(ApplicationDbContext context);
    Task<CalendarTemplate> GetCalendarTemplateById(ApplicationDbContext context, string id);

    Task<string> CreateCalendarTemplate(ApplicationDbContext context,
        CalendarTemplateCreateDto calendarTemplateCreateDto);

    Task<string> UpdateCalendarTemplate(ApplicationDbContext context, CalendarTemplate calendarTemplate);
    Task<bool> DeleteCalendarTemplate(ApplicationDbContext context, string id);
}