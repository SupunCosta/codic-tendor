using System.Security.Claims;

namespace UPrinceV4.Web.Azure;

internal static class Claims
{
    internal const string ScopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";
    internal const string AppPermissionOrRolesClaimType = ClaimTypes.Role;
}