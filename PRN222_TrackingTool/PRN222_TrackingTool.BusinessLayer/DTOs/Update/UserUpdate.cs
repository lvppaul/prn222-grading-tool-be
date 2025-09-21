using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.DTOs.Update
{
    public class UserUpdate
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}
