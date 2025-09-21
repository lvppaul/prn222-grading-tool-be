using Microsoft.EntityFrameworkCore;
using PRN222_TrackingTool.DataAccessLayer.Context;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using PRN222_TrackingTool.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.DataAccessLayer.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(Prn222TrackingToolContext context) : base(context)
        {
        }

        public override async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles
                .Where(r => r.IsDeleted == false && r.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
