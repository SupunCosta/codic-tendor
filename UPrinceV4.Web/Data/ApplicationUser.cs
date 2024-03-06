using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class ApplicationUser
{
    public int TenantId { get; set; }
    public string Id { get; set; }

    [Unique] public string OId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Company { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
    public string Comment { get; set; }
    public string Avatar { get; set; }
    public string Country { get; set; }

    public IList<UserRole> UserRole { get; set; }
}

public class ApplicationUserDto
{
    public string UserName { get; set; }
}