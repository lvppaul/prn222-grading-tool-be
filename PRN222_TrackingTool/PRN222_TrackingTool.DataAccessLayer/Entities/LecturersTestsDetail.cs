using System;
using System.Collections.Generic;

namespace PRN222_TrackingTool.DataAccessLayer.Entities;

public partial class LecturersTestsDetail
{
    public int TestId { get; set; }

    public int LecturerId { get; set; }

    public double? Score { get; set; }

    public string? Reason { get; set; }

    public bool? IsGrading { get; set; }

    public virtual User Lecturer { get; set; } = null!;

    public virtual Test Test { get; set; } = null!;
}
