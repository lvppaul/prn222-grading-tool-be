using System;
using System.Collections.Generic;

namespace PRN222_TrackingTool.DataAccessLayer.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? ExpiredRefreshToken { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<LecturersTestsDetail> LecturersTestsDetails { get; set; } = new List<LecturersTestsDetail>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
