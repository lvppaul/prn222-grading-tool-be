using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Mappings
{
    public static class LecturerStudentAssignmentMapper
    {
        public static LecturerStudentAssignmentResponse ToResponse(this LecturerStudentAssignment entity)
        {
            if (entity == null) return null;

            return new LecturerStudentAssignmentResponse
            {
                Id = entity.Id,
                Score = entity.Score,
                Reason = entity.Reason,
                IsReExam = entity.IsReExam,
                IsFinal = entity.IsFinal,
                AssignedAt = entity.AssignedAt,
            };
        }
    }
}
