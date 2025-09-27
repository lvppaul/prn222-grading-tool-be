﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.DataAccessLayer.Entities
{
    public partial class Students
    {
        public int Id { get; set; }
        public string? StudentCode { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    
        public ICollection<LecturerStudentAssignment> LecturerStudentAssignments { get; set; }
        = new List<LecturerStudentAssignment>();

        public virtual Test? Test { get; set; }
    }
}
