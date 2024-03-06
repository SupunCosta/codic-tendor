using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UPrinceV4.Shared;

public class MissingTenantMiddleware
{
    private readonly string _missingTenantUrl;
    private readonly RequestDelegate _next;

    public MissingTenantMiddleware(RequestDelegate next, string missingTenantUrl)
    {
        _next = next;
        _missingTenantUrl = missingTenantUrl;
    }

    public async Task Invoke(HttpContext httpContext, ITenantProvider provider)
    {
        // if (provider.GetTenant() == null)
        // {
        //     httpContext.Response.Redirect(_missingTenantUrl);
        //     return;
        // }

        await _next.Invoke(httpContext);
    }
}