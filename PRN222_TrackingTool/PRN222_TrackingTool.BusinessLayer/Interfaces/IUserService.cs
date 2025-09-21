using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.BusinessLayer.DTOs.Update;
using PRN222_TrackingTool.DataAccessLayer.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetAllAsync();
        Task<PaginatedResult<UserResponse>> GetAllAsync(Pagination pagination);
        Task<UserResponse?> GetByIdAsync(int id);
        Task<UserResponse?> UpdateAsync(int id, UserUpdate updateDto);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
    }
}
