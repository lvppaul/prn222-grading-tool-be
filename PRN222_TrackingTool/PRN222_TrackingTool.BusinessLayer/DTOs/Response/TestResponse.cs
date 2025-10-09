using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.DTOs.Response
{
    public class TestResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string OriginalFilename { get; set; }
        public int StudentId { get; set; }
        public int? ExamId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
