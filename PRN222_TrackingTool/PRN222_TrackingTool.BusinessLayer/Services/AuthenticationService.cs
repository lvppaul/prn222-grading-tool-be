using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PRN222_TrackingTool.BusinessLayer.DTOs.Request;
using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.BusinessLayer.Interfaces;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using PRN222_TrackingTool.DataAccessLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _secretKey = _configuration["JwtSettings:Key"];
            _issuer = _configuration["JwtSettings:Issuer"];
            _audience = _configuration["JwtSettings:Audience"];
            _expiryMinutes = int.Parse(_configuration["JwtSettings:ExpiryMinutes"] ?? "30");
        }

        public string GenerateAccessToken(User user, string roleName)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(_secretKey)) throw new InvalidOperationException("Secret key not configured.");

            var keyBytes = System.Text.Encoding.UTF8.GetBytes(_secretKey);
            if (keyBytes.Length < 32)
                throw new InvalidOperationException("Secret key should be at least 256 bits (32 chars UTF8).");

            var secretKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            if (!string.IsNullOrWhiteSpace(roleName))
                claims.Add(new Claim(ClaimTypes.Role, roleName));

            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = now,
                Expires = now.AddMinutes(_expiryMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = signingCredentials
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(tokenDescriptor);
        }

        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        public async Task<AuthenticationResponse?> LoginAsync(LoginRequest request)
        {
            if(request == null 
                || string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email and password are required"
                };
            }

            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email does not exists"
                };
            }

            var isValidPassword = await _unitOfWork.UserRepository.VerifyPasswordAsync(user, request.Password);
            if (!isValidPassword)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid password"
                };
            }
            if (user.IsActive == false)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "User is deactivated"
                };
            }
            // Generate tokens
            var accessToken = GenerateAccessToken(user, user.Role?.Name ?? string.Empty);

            // Always rotate refresh token on login (good practice)
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.ExpiredRefreshToken = DateTime.UtcNow.AddDays(7);

            _unitOfWork.UserRepository.PrepareUpdate(user);
            await _unitOfWork.SaveAsync();

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Login successful",
                Data = new AuthenticationData
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }

        public async Task<AuthenticationResponse?> LogoutAsync(LogoutRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Refresh token is required"
                };
            }

            // Giả định có method này trong UserRepository.
            // Nếu chưa có, bạn cần thêm: GetByRefreshTokenAsync(string token)
            var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(request.RefreshToken);
            if (user == null)
            {
                // Không tiết lộ trạng thái nội bộ
                return new AuthenticationResponse
                {
                    Success = true,
                    Message = "Logged out"
                };
            }

            if(user.ExpiredRefreshToken <= DateTime.UtcNow)
            {
                user.RefreshToken = null;
                user.ExpiredRefreshToken = null;

                _unitOfWork.UserRepository.PrepareUpdate(user);
                await _unitOfWork.SaveAsync();

                return new AuthenticationResponse
                {
                    Success = true,
                    Message = "Refresh token already expired"
                };
            }

            // Kiểm tra token hiện tại còn khớp không
            if (user.RefreshToken != request.RefreshToken)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            // Hủy refresh token (cách đơn giản: xóa hoặc đặt hết hạn)
            user.RefreshToken = null;
            user.ExpiredRefreshToken = null;

            _unitOfWork.UserRepository.PrepareUpdate(user);
            await _unitOfWork.SaveAsync();

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Logged out"
            };
        }

        public async Task<AuthenticationResponse?> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Refresh token is required"
                };
            }

            var user = await _unitOfWork.UserRepository.GetByRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            if (user.ExpiredRefreshToken == null || user.ExpiredRefreshToken < DateTime.UtcNow)
            {
                // Clear expired token
                user.RefreshToken = null;
                user.ExpiredRefreshToken = null;
                _unitOfWork.UserRepository.PrepareUpdate(user);
                await _unitOfWork.SaveAsync();

                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Refresh token has expired"
                };
            }

            // Rotate refresh token
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshExpiry = DateTime.UtcNow.AddDays(7);
            user.RefreshToken = newRefreshToken;
            user.ExpiredRefreshToken = newRefreshExpiry;

            // Issue new access token
            var newAccessToken = GenerateAccessToken(user, user.Role?.Name);

            _unitOfWork.UserRepository.PrepareUpdate(user);
            await _unitOfWork.SaveAsync();

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                Data = new AuthenticationData
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };
        }

        public async Task<AuthenticationResponse?> RegisterAsync(UserRequest request)
        {
            // Validate input Null or Empty
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Name))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email, password and name are required"
                };
            }

            // Validate password
            var passwordCheck = ValidatePassword(request.Password);
            if (!passwordCheck.Success)
                return passwordCheck;

            // Check email exists
            var existing = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (existing != null)
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email already registered"
                };

            var hashed = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = hashed,
                RoleId = request.RoleId,
                CreatedAt = DateTime.UtcNow
            };
            
            _unitOfWork.UserRepository.PrepareCreate(user);
            await _unitOfWork.SaveAsync();
            
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(user.RoleId.Value);
            if (role == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid role specified"
                };
            }

            var accessToken = GenerateAccessToken(user, role.Name);
            var refreshToken = GenerateRefreshToken();
            var expiry = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshToken;
            user.ExpiredRefreshToken = expiry;

            _unitOfWork.UserRepository.PrepareUpdate(user);
            await _unitOfWork.SaveAsync();

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Registration successful",
                Data = new AuthenticationData
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }

        public Task<ClaimsPrincipal?> ValidateToken(string token)
        {
            throw new NotImplementedException();
        }

        private AuthenticationResponse ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters long.");
            }

            if (!password.Any(char.IsUpper))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }

            if (!password.Any(char.IsLower))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }

            if (!password.Any(char.IsDigit))
            {
                errors.Add("Password must contain at least one digit.");
            }

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                errors.Add("Password must contain at least one special character.");
            }

            return new AuthenticationResponse
            {
                // error  count = 0 thì success = true
                Success = errors.Count == 0,
                // nếu có lỗi thì trả về danh sách lỗi, không thì trả về null
                Errors = errors.Count > 0 ? errors : null,
                // nếu có lỗi thì trả về thông báo lỗi chung, không thì trả về null
                Message = errors.Count > 0 ? "Password validation failed" : null
            };
        }
    }
}
