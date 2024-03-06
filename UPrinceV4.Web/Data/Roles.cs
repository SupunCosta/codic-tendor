using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class Roles
{
    [Required] public int TenantId { get; set; }
    public string Id { get; set; }

    [Required] public string RoleName { get; set; }
    public string LanguageCode { get; set; }

    public string RoleId { get; set; }
    // public IList<UserRole> UserRole { get; set; }
}

public class RolesCreateDto
{
    [Required] public int TenantId { get; set; }

    [Required] public string RoleName { get; set; }
}

public class RolesUpdateDto
{
    public string Id { get; set; }
    [Required] public int TenantId { get; set; }

    [Required] public string RoleName { get; set; }
}

public class RoleReadDto
{
    public string Id { get; set; }
    public int TenantId { get; set; }
    public string Role { get; set; }
}

public class RoleDto
{
    public string Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string RoleId { get; set; }
    public string LanguageCode { get; set; }
    public string Label { get; set; }
}

public class RoleAddDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}

public class RoleUpdateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string LanguageCode { get; set; }
}