using Microsoft.AspNetCore.Mvc;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Components;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public CategoryMenuViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    //public Task<IViewComponentResult> InvokeAsync()
    //{
    //    var model = _context.Categories.OrderBy(c => c.Name).ToList();

    //    return Task.FromResult((IViewComponentResult)View("Default", model));
    //}
}