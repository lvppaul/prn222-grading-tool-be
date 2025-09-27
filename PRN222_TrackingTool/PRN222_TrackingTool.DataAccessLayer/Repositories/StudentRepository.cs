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
    public class StudentRepository : GenericRepository<Students>, IStudentRepository
    {
        public StudentRepository(Prn222TrackingToolContext context) : base(context)
        {
        }

        public async Task<ICollection<Students>> GetStudentByEmailAsync(string email)
        {
            return await _context.Students
                .Where(s => s.Email.ToLower().ToLower() == email.ToLower())
                .ToListAsync();
        }

        public async Task<ICollection<Students>> GetStudentByName(string name)
        {
            return await _context.Students
                .Include(s => s.Test)
                .Include(s => s.LecturerStudentAssignments)
                    .ThenInclude(lsa => lsa.Test)
                .Include(s => s.LecturerStudentAssignments)
                    .ThenInclude(lsa => lsa.Lecturer)
                .Where(s => s.FullName.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public override async Task<List<Students>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.Test)
                .Include(s => s.LecturerStudentAssignments)
                    .ThenInclude(lsa => lsa.Test)
                .Include(s => s.LecturerStudentAssignments)
                    .ThenInclude(lsa => lsa.Lecturer)
                .ToListAsync();
        }

        public override async Task<Students> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Test)
                .Include(s => s.LecturerStudentAssignments)
                    .ThenInclude(lsa => lsa.Test)
                .Include(s => s.LecturerStudentAssignments)
                    .ThenInclude(lsa => lsa.Lecturer)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

    }
}
