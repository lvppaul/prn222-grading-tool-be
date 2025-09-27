using PRN222_TrackingTool.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Interfaces
{
    public interface IStudentService
    {
        Task<ICollection<Students>> GetStudentByEmailAsync(string email);
        Task<ICollection<Students>> GetStudentByName(string name);
        Task<ICollection<Students>> GetAllAsync();
        Task<Students> GetByIdAsync(int id);
    }
}
