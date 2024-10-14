using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    // This action can be accessed by users with the "User" role
    [Authorize(Roles = "User")]
    [HttpGet("user-access")]
    public IActionResult UserAccess()
    {
        return Ok("Access granted: This endpoint is accessible to Users.");
    }

    // This action can be accessed by users with the "Driver" role
    [Authorize(Roles = "Driver")]
    [HttpGet("driver-access")]
    public IActionResult DriverAccess()
    {
        var userId = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return Ok("Access granted: This endpoint is accessible to Drivers.");
    }

    // This action allows anonymous access
    [AllowAnonymous]
    [HttpGet("public-access")]
    public IActionResult PublicAccess()
    {
        var userId = HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return Ok("Access granted: This endpoint is accessible to everyone.");
    }
}
