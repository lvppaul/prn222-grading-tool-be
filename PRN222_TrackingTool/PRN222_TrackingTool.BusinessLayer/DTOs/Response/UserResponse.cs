using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.DTOs.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
