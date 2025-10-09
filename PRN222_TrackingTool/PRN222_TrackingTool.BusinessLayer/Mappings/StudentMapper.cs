using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Mappings
{
    public static class StudentMapper
    {
        public static StudentResponse ToResponse(this Students entity)
        {
            if (entity == null) return null;

            return new StudentResponse
            {
                Id = entity.Id,
                StudentCode = entity.StudentCode,
                FullName = entity.FullName,
                Email = entity.Email,
                Test = entity.Test?.ToResponse(),
                Assignments = entity.LecturerStudentAssignments?
                    .Select(a => a.ToResponse())
                    .ToList()
            };
        }
    }
}
