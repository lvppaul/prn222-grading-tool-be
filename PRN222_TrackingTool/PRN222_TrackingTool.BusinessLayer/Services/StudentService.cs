using PRN222_TrackingTool.BusinessLayer.Interfaces;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using PRN222_TrackingTool.DataAccessLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<Students>> GetAllAsync()
        {
            try
            {
                var result = await _unitOfWork.StudentRepository.GetAllAsync();
                return result;
            }
            catch (Exception)
            {
                // Log exception nếu có logger
                return Array.Empty<Students>();
            }
        }

        public async Task<Students> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            try
            {
                var result = await _unitOfWork.StudentRepository.GetByIdAsync(id);
                return result;
            }
            catch (Exception)
            {
                // Log exception nếu có logger
                return null;
            }
        }

        public async Task<ICollection<Students>> GetStudentByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Array.Empty<Students>();

            // Chuẩn hoá
            email = email.Trim();

            var result = await _unitOfWork.StudentRepository.GetStudentByEmailAsync(email);
            return result ?? Array.Empty<Students>();
        }

        public async Task<ICollection<Students>> GetStudentByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Array.Empty<Students>();

            name = name.Trim();
            var result = await _unitOfWork.StudentRepository.GetStudentByName(name);
            return result ?? Array.Empty<Students>();
        }
    }
}
