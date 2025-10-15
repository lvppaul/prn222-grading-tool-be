using PRN222_TrackingTool.BusinessLayer.DTOs.Request;
using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateAccessToken(User user, string roleName);
        string GenerateRefreshToken();
        Task<AuthenticationResponse?> LoginAsync(LoginRequest request);
        Task<AuthenticationResponse?> RefreshTokenAsync(string refreshToken);
        Task<AuthenticationResponse?> RegisterAsync(UserRequest request);
        Task<AuthenticationResponse?> LogoutAsync(LogoutRequest request);
    }
}
