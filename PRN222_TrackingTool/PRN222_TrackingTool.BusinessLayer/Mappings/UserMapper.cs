using PRN222_TrackingTool.BusinessLayer.DTOs.Request;
using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Mappings
{
    public static class UserMapper
    {
        public static User ToEntity(this UserRequest request)
        {
            if (request == null) return null;
            return new User
            {
                Name = request.Name.Trim(),
                Password = request.Password, // In real applications, hash the password
                Email = request.Email,
                RoleId = (int)DataAccessLayer.Enums.Roles.Lecturer,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
            };
        }

        public static UserResponse ToResponse(this User entity)
        {
            if (entity == null) return null;
            return new UserResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                RoleName = entity.Role?.Name ?? string.Empty,
                IsActive = entity.IsActive
            };
        }
    }
}
