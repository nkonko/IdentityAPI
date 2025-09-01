using identityAPI.Core.Models;
using identityAPI.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace identityAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
            => Ok(await _roleService.GetRolesAsync());

        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] RoleCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var role = await _roleService.CreateRoleAsync(dto);
            return CreatedAtAction(nameof(GetRoles), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _roleService.UpdateRoleAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetRolePermissions(string id)
        {
            var permissions = await _roleService.GetRolePermissionsAsync(id);
            return Ok(permissions);
        }

        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AddRolePermissions(string id, [FromBody] IEnumerable<PermissionDto> dtos)
        {
            var result = await _roleService.AddRolePermissionsAsync(id, dtos);
            return result ? NoContent() : BadRequest();
        }
    }
}
