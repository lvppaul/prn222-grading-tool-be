using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.DTOs.Response
{
    public class AuthenticationResponse
    {
        public bool Success { get; set; }

        public string? Message { get; set; }
        // Trong trường họp null thì không serialize field này
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AuthenticationData? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Errors { get; set; }
    }

    public class AuthenticationData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
