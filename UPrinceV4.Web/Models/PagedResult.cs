using System.Collections.Generic;

namespace UPrinceV4.Web.Models;

public class PagedResult<T>
{
    public int PageSize = 10;
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public IList<T> Results { get; set; }
}