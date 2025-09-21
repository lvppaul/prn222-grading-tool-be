using Microsoft.EntityFrameworkCore;
using PRN222_TrackingTool.DataAccessLayer.Context;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using PRN222_TrackingTool.DataAccessLayer.Helper;
using PRN222_TrackingTool.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.DataAccessLayer.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(Prn222TrackingToolContext context) : base(context)
        {
        }

        public async Task SetActive(User user)
        {
            user.IsActive = true;
            await UpdateAsync(user);
        }

        public async Task DeActive(User user)
        {
            user.IsActive = false;
            await UpdateAsync(user);
        }

        public override async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<PaginatedResult<User>> GetAllAsyncWithPagination(Pagination pagination)
        {
            // Get total count
            var totalCount = await _context.Users.CountAsync();

            // Get paginated items with Role included
            var items = await _context.Users
                .Include(u => u.Role)
                .OrderBy(u => u.Id) // Thêm ordering để đảm bảo consistent pagination
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();

            return new PaginatedResult<User>(items, totalCount, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            if(string.IsNullOrEmpty(refreshToken))
                return null;

            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.IsActive == true);
        }

        public async Task<bool> VerifyPasswordAsync(User user, string password)
        {
            return await Task.FromResult(BCrypt.Net.BCrypt.Verify(password, user.Password));
        }
    }
}
