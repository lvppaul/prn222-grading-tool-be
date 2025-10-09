using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN222_TrackingTool.BusinessLayer.DTOs.Request;
using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.BusinessLayer.Interfaces;

namespace PRN222_TrackingTool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation errors occurred.",
                        Payload = errors
                    });
                }

                var response = await _authenticationService.LoginAsync(request);

                if (response.Success)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Login successful.",
                        Payload = response.Data
                    });
                }
                else
                {
                    return Unauthorized(new ApiResponse<AuthenticationResponse>
                    {
                        Success = false,
                        Message = response.Message ?? "Invalid credentials.",
                        Payload = null
                    });
                }
            }
            catch (Exception ex)
            {
                // Có thể log lỗi chi tiết tại đây
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An unexpected error occurred while processing login.",
                    Payload = null
                });
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation errors occurred.",
                        Payload = errors
                    });
                }
                var response = await _authenticationService.RegisterAsync(request);
                if (response.Success)
                {
                    return Ok(new ApiResponse<AuthenticationResponse>
                    {
                        Success = true,
                        Message = "Registration successful.",
                        Payload = response
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = response.Message ?? "Registration failed.",
                        Payload = null
                    });
                }
            } catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<AuthenticationResponse>>
                {
                    Success = false,
                    Message = "An error occurred while processing registration.",
                    Payload = null
                });
            }

        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Refresh token is required.",
                        Payload = null
                    });
                }

                var result = await _authenticationService.RefreshTokenAsync(request.RefreshToken);

                if (result == null)
                {
                    return StatusCode(500, new ApiResponse<object>
                    {
                        Success = false,
                        Message = "An error occurred while processing the refresh token.",
                        Payload = null
                    });
                }

                if (!result.Success)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = result.Message ?? "Invalid refresh token.",
                        Payload = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Token refreshed successfully.",
                    Payload = result.Data
                });
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An unexpected error occurred while processing the refresh token.",
                    Payload = null
                });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var response = await _authenticationService.LogoutAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
