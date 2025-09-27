using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.DTOs.Response
{
    public class LecturerStudentAssignmentResponse
    {
        public int Id { get; set; }
        public double? Score { get; set; }
        public string? Reason { get; set; }
        public bool? IsReExam { get; set; }
        public bool? IsFinal { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
