using System;
using System.Collections.Generic;

namespace PRN222_TrackingTool.DataAccessLayer.Entities;

public partial class Test
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? OriginalFilename { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Link { get; set; }

    public int? StudentId { get; set; }

    public int? ExamId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual ICollection<LecturerStudentAssignment> LecturerStudentAssignments { get; set; } = new List<LecturerStudentAssignment>();

    public virtual Students? Student { get; set; }

}
