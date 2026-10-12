using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace PetHaven.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected ulong GetCurrentUserId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return ulong.TryParse(idClaim, out var id) ? id : 0;
    }

    protected string GetCurrentUserRole()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    }

    protected bool IsAdminOrStaff()
    {
        var role = GetCurrentUserRole();
        return role.Equals("admin", StringComparison.OrdinalIgnoreCase) || 
               role.Equals("staff", StringComparison.OrdinalIgnoreCase);
    }
}
