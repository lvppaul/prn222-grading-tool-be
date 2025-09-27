using PRN222_TrackingTool.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.DataAccessLayer.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Students>
    {
        Task<ICollection<Students>> GetStudentByEmailAsync(string email);
        Task<ICollection<Students>> GetStudentByName(string name);
    }
}
