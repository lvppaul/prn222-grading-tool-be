using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN222_TrackingTool.BusinessLayer.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System;
using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using PRN222_TrackingTool.BusinessLayer.Mappings;

namespace PRN222_TrackingTool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [Authorize]
        // GET: api/student/by-email?email=abc@domain.com
        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { message = "Email is required" });

            var students = await _studentService.GetStudentByEmailAsync(email);
            if (students == null || students.Count == 0)
                return NotFound(new { message = "No students found with that email" });

            return Ok(students);
        }

        //[Authorize]
        // GET: api/student/by-name?name=Nguyen
        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Name is required" });

            var students = await _studentService.GetStudentByName(name);
            if (students == null || students.Count == 0)
                return NotFound(new { message = "No students found with that name" });

            var result = students.Select(s => s.ToResponse()).ToList();

            return Ok(result);
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            var result = students.Select(s => s.ToResponse()).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid student ID" });
            var student = await _studentService.GetByIdAsync(id);
            if (student == null)
                return NotFound(new { message = "Student not found" });
            var result = student.ToResponse();
            return Ok(result);
        }
    }
}
