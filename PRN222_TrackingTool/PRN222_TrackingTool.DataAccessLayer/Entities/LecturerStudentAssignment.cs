using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.DataAccessLayer.Entities
{
    public partial class LecturerStudentAssignment
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }   // FK -> Users.UserId
        public int StudentId { get; set; }    // FK -> Students.StudentId
        public int TestId { get; set; }       // FK -> Tests.TestId

        public double? Score { get; set; }
        public string? Reason { get; set; }
        public bool? IsReExam { get; set; }
        public bool? IsFinal { get; set; }
        public DateTime AssignedAt { get; set; }

        public virtual User Lecturer { get; set; }
        public virtual Students Student { get; set; }
        public virtual Test Test { get; set; }
    }
}
