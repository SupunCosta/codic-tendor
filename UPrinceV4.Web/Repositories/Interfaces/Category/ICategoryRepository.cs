using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data.Category;

namespace UPrinceV4.Web.Repositories.Interfaces.Category;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryLevelCreateDto>> GetCategory(CategoryParameter categoryParameter);
    Task<string> CreateCategory(CategoryParameter categoryParameter);
    Task<string> CreatePost(CategoryParameter categoryParameter);
    Task<PostDto> GetPostById(CategoryParameter categoryParameter);
}

public class CategoryParameter
{
    public IHttpContextAccessor ContextAccessor { get; set; }
    public string Lang { get; set; }

    public ITenantProvider TenantProvider { get; set; }

    // public PODto PODto { get; set; }
    public string Title { get; set; }


    public string Id { get; set; }
    public string UserId { get; set; }
    public List<string> idList { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
    public CategoryLevelCreateDto CategoryDto { get; set; }
    public PostDto PostDto { get; set; }
}