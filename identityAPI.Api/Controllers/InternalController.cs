using identityAPI.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace identityAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternalController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<InternalController> _logger;

        public InternalController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<InternalController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint interno para que WebAPI consulte conteos de usuarios y roles
        /// </summary>
        [HttpGet("counts")]
        [AllowAnonymous] // Temporalmente público, en producción debería estar protegido por API Key o red interna
        public async Task<ActionResult<InternalCountsDto>> GetCounts()
        {
            try
            {
                var totalUsers = await _userManager.Users.CountAsync();
                var totalRoles = await _roleManager.Roles.CountAsync();

                _logger.LogInformation("Internal counts requested: {TotalUsers} users, {TotalRoles} roles", 
                    totalUsers, totalRoles);

                return Ok(new InternalCountsDto
                {
                    TotalUsers = totalUsers,
                    TotalRoles = totalRoles
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching internal counts");
                return StatusCode(500, new { error = "Error fetching counts" });
            }
        }
    }

    public class InternalCountsDto
    {
        public int TotalUsers { get; set; }
        public int TotalRoles { get; set; }
    }
}
