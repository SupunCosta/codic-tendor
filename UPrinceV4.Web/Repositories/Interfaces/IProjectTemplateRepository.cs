using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectTemplateRepository
{
    Task<IEnumerable<ProjectTemplateDto>> GetProjectTemplates(ApplicationDbContext context, string lang,
        ITenantProvider iTenantProvider);

    Task<Currency> GetProjectTemplateById(ApplicationDbContext context, string id);

    Task<string> CreateProjectTemplate(ApplicationDbContext context,
        ProjectTemplateCreateDto projectTemplateCreateDto);

    Task<string> UpdateProjectTemplate(ApplicationDbContext context,
        ProjectTemplateCreateDto projectTemplateCreateDto);

    bool DeleteProjectTemplate(ApplicationDbContext context, string id);
}