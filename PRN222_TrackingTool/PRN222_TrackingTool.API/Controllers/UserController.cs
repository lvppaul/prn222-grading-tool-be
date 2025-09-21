using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN222_TrackingTool.BusinessLayer.DTOs.Update;
using PRN222_TrackingTool.BusinessLayer.Interfaces;
using PRN222_TrackingTool.DataAccessLayer.Helper;

namespace PRN222_TrackingTool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("pagination")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var pagination = new Pagination(pageNumber, pageSize);
            var result = await _userService.GetAllAsync(pagination);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdate updateDto)
        {
            if (updateDto == null) return BadRequest();
            var updated = await _userService.UpdateAsync(id, updateDto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [Authorize]
        [HttpPost("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var ok = await _userService.ActivateAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

        [Authorize]
        [HttpPost("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var ok = await _userService.DeactivateAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
