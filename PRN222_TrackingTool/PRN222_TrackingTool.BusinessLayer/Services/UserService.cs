using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.BusinessLayer.DTOs.Update;
using PRN222_TrackingTool.BusinessLayer.Interfaces;
using PRN222_TrackingTool.BusinessLayer.Mappings;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using PRN222_TrackingTool.DataAccessLayer.Helper;
using PRN222_TrackingTool.DataAccessLayer.UnitOfWork;

namespace PRN222_TrackingTool.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users.Select(u => u.ToResponse()).ToList();
        }

        public async Task<PaginatedResult<UserResponse>> GetAllAsync(Pagination pagination)
        {
            var paged = await _unitOfWork.UserRepository.GetAllAsyncWithPagination(pagination);
            return new PaginatedResult<UserResponse>(
                paged.Items.Select(u => u.ToResponse()),
                paged.TotalCount,
                paged.PageNumber,
                paged.PageSize
            );
        }

        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return user?.ToResponse();
        }

        public async Task<UserResponse?> UpdateAsync(int id, UserUpdate updateDto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return null;

            bool changed = false;
            if(!string.IsNullOrWhiteSpace(updateDto.Email) && updateDto.Email != user.Email)
            {
                user.Email = updateDto.Email.Trim();
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Name) && updateDto.Name != user.Name)
            {
                user.Name = updateDto.Name.Trim();
                changed = true;
            }
            if (updateDto.RoleId.HasValue && updateDto.RoleId != user.RoleId)
            {
                user.RoleId = updateDto.RoleId;
                changed = true;
            }
            if (updateDto.IsActive.HasValue && updateDto.IsActive != user.IsActive)
            {
                user.IsActive = updateDto.IsActive;
                changed = true;
            }

            if (!changed) return user.ToResponse();

            _unitOfWork.UserRepository.PrepareUpdate(user);
            await _unitOfWork.SaveAsync();
            return user.ToResponse();
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return false;
            if (user.IsActive == true) return true;
            await _unitOfWork.UserRepository.SetActive(user);
            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return false;
            if (user.IsActive == false) return true;
            await _unitOfWork.UserRepository.DeActive(user);
            return true;
        }
    }
}
