using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class UserRole
{
    [Key] public string Id { get; set; }
    public string RoleId { get; set; }

    //public virtual Roles Role { get; set; }
    public string ApplicationUserId { get; set; }

    //public virtual ApplicationUser User { get; set; }
    public string ApplicationUserOid { get; set; }
}