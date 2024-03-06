using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Models;

public class UserRoleRegistration
{
    [Required] public string UserId { get; set; }

    [Required] public string RoleId { get; set; }
}