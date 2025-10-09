using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
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

        [Authorize(Roles = "Examiner")]
        [HttpGet("pagination")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagination = new Pagination(pageNumber, pageSize);
                var result = await _userService.GetAllAsync(pagination);
                return Ok(new ApiResponse<PaginatedResult<UserResponse>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Payload = result
                });
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving users.",
                    Payload = null
                });
            }
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid user ID.",
                Payload = null
            });

            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null) return NotFound();
                return Ok(new ApiResponse<UserResponse>
                {
                    Success = true,
                    Message = "User retrieved successfully.",
                    Payload = user
                });
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user.",
                    Payload = null
                });
            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdate updateDto)
        {
            if (updateDto == null) return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Update data is required.",
                Payload = null
            });
            try
            {
                var updated = await _userService.UpdateAsync(id, updateDto);
                if (updated == null) return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User not found.",
                    Payload = null
                });
                return Ok(new ApiResponse<UserResponse>
                {
                    Success = true,
                    Message = "User updated successfully.",
                    Payload = updated
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while updating the user.",
                    Payload = null
                });
            }
        }

        [Authorize]
        [HttpPost("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            if(id <= 0) return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid user ID.",
                Payload = null
            });
            try
            {
                var ok = await _userService.ActivateAsync(id);
                if (!ok) return NotFound();
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while activating the user.",
                    Payload = null
                });
            }
        }

        [Authorize]
        [HttpPost("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            if(id <= 0) return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid user ID.",
                Payload = null
            });
            try
            {
                var ok = await _userService.DeactivateAsync(id);
                if (!ok) return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User not found.",
                    Payload = null
                });
                return NoContent();
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while deactivating the user.",
                    Payload = null
                });
            }
        }
    }
}
