using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.DTOs.Request
{
    public class LogoutRequest
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
