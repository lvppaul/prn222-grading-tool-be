using PRN222_TrackingTool.DataAccessLayer.Entities;
using PRN222_TrackingTool.DataAccessLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task SetActive(User user);
        Task DeActive(User user);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<PaginatedResult<User>> GetAllAsyncWithPagination(Pagination pagination);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByNameAsync(string name);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<bool> VerifyPasswordAsync(User user, string password);

        void PrepareCreate(User user);
        void PrepareUpdate(User user);
        Task<int> UpdateAsync(User user);
    }
}
