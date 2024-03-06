using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Models;

public class CreateUser
{
    private readonly UserDbContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ITenantProvider _tenantProvider;

    public CreateUser(IHttpContextAccessor contextAccessor,
        ITenantProvider tenantProvider,
        UserDbContext customerContext)
    {
        _contextAccessor = contextAccessor;
        _tenantProvider = tenantProvider;
        _context = customerContext;
    }

    public ApplicationUser createUser()
    {
        string Email;
        var user = new ApplicationUser();
        var oid = _contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
        user.Id = oid;
        user.OId = oid;
        // user.Email = _contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
        //     claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

        var PEmail = _contextAccessor.HttpContext?.User.Identities.First().Claims.Where(claim =>
            claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

        if (PEmail.Any())
            user.Email = PEmail.FirstOrDefault().Value;
        else
            user.Email = _contextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn").Value;

        try
        {
            user.FirstName = _contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
            user.LastName = _contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
            // user.Company = _contextAccessor.HttpContext.User.Identities.First().Claims
            //     .First(claim => claim.Type == "extension_Company").Value;
            // user.Country = _contextAccessor.HttpContext.User.Identities.First().Claims
            //     .First(claim => claim.Type == "ctry").Value;
            user.Company = _contextAccessor.HttpContext.User.Identities.First().Claims
                .FirstOrDefault(claim => claim.Type == "extension_Company")?.Value;
            user.Country = _contextAccessor.HttpContext.User.Identities.First().Claims
                .FirstOrDefault(claim => claim.Type == "ctry")?.Value;
        }
        catch (Exception)
        {
            // ignored
        }

        user.TenantId = _tenantProvider.GetTenant().Id;
        _context.ApplicationUser.Add(user);
        _context.SaveChanges();
        AssignRoleAndCu(user.Id, user.OId);
        return user;
    }

    public ApplicationUser UpdateUser(ApplicationUser user)
    {
        var oid = _contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
            claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
        // ApplicationUser user = _context.ApplicationUser.FirstOrDefault(u => u.OId == oid);
        user.OId = oid;
        // user.Email = _contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
        //     claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
        var PEmail = _contextAccessor.HttpContext?.User.Identities.First().Claims.Where(claim =>
            claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

        if (PEmail.Any())
            user.Email = PEmail.FirstOrDefault().Value;
        else
            user.Email = _contextAccessor.HttpContext?.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn").Value;
        try
        {
            user.FirstName = _contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
            user.LastName = _contextAccessor.HttpContext.User.Identities.First().Claims.First(claim =>
                claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
            user.Company = _contextAccessor.HttpContext.User.Identities.First().Claims
                .FirstOrDefault(claim => claim.Type == "extension_Company")?.Value;
            user.Country = _contextAccessor.HttpContext.User.Identities.First().Claims
                .FirstOrDefault(claim => claim.Type == "ctry")?.Value;
        }
        catch (Exception)
        {
        }

        _context.ApplicationUser.Update(user);
        _context.SaveChanges();
        UpdateRoleAndCu(user.Id, oid);
        return user;
    }

    public void AssignRoleAndCu(string Id, string Oid)
    {
        try
        {
            var userRole = new UserRole();
            userRole.Id = Guid.NewGuid().ToString();
            userRole.RoleId = "4837043c-119c-47e1-bbf2-1f32557fdf30";
            userRole.ApplicationUserId = Id;
            userRole.ApplicationUserOid = Oid;
            _context.UserRole.Add(userRole);
            _context.SaveChanges();

            var contractingUnitUserRole = new ContractingUnitUserRole();
            contractingUnitUserRole.Id = Guid.NewGuid().ToString();
            contractingUnitUserRole.UserRoleId = userRole.Id;
            contractingUnitUserRole.CabCompanyId = "0134f1d9-172f-4d8e-bac4-33ac29f4be68"; //if BM
            // contractingUnitUserRole.CabCompanyId = "e0386eac-c9a0-4f93-8baf-d24948bedda9";
            _context.ContractingUnitUserRole.Add(contractingUnitUserRole);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public void UpdateRoleAndCu(string Id, string Oid)
    {
        try
        {
            var user = _context.UserRole.Where(u => u.ApplicationUserId == Id).FirstOrDefault();
            user.ApplicationUserOid = Oid;
            _context.UserRole.Update(user);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}