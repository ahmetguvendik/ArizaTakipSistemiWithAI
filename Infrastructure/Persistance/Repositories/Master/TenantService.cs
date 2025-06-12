using Application.Repositories.Master;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Persistance.Repositories.Master;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetConnectionString()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            throw new Exception("HTTP context mevcut değil.");

        // Tek scheme var: MyCookieAuth
        var result = httpContext.AuthenticateAsync("MyCookieAuth").GetAwaiter().GetResult();

        if (result.Succeeded)
        {
            var connStr = result.Principal?.Claims.FirstOrDefault(x => x.Type == "ConnectionString")?.Value;

            if (!string.IsNullOrEmpty(connStr))
                return connStr;
        }

        throw new Exception("Tenant bağlantı bilgisi alınamadı.");
    }

}