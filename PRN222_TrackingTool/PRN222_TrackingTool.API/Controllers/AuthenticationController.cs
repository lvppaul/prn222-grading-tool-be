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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new AuthenticationResponse
                {
                    Success = false,
                    Message = "Validation errors occurred.",
                    Errors = errors
                });
            }
            var response = await _authenticationService.LoginAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return Unauthorized(response);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new AuthenticationResponse
                {
                    Success = false,
                    Message = "Validation errors occurred.",
                    Errors = errors
                });
            }
            var response = await _authenticationService.RegisterAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
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
