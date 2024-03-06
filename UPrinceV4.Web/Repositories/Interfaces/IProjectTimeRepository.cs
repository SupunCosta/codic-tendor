using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IProjectTimeRepository
{
    Task<IEnumerable<ProjectTime>> GetProjectTimes(ApplicationDbContext context);
    Task<Currency> GetProjectTimeById(ApplicationDbContext context, string id);

    Task<string> CreateProjectTime( ProjectTimeCreateDto projectTimeCreateDto,
        ApplicationUser user, ITenantProvider iTenantProvider);

    Task<string> UpdateProjectTime(ProjectTimeUpdateDto projectTimeUpdateDto,
        ApplicationUser user, ITenantProvider iTenantProvider);

    bool DeleteProjectTime(ApplicationDbContext context, string id);
}