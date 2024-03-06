using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectManagementLevelRepository
{
    Task<IEnumerable<ProjectManagementLevelDto>> GetProjectManagementLevels(ApplicationDbContext context,
        string lang, ITenantProvider iTenantProvider);

    Task<Currency> GetProjectManagementLevelById(ApplicationDbContext context, string id);

    Task<string> CreateProjectManagementLevel(ApplicationDbContext context,
        ProjectManagementLevelCreateDto projectManagementLevelCreateDto);

    Task<string> UpdateProjectManagementLevel(ApplicationDbContext context,
        ProjectManagementLevelCreateDto projectManagementLevelCreateDto);

    Task<bool> DeleteProjectManagementLevel(ApplicationDbContext context, string id);
}