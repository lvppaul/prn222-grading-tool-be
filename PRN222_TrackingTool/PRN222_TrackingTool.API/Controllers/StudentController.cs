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
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Email is required",
                        Payload = null
                    });

                var students = await _studentService.GetStudentByEmailAsync(email);
                if (students == null || students.Count == 0)
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "No students found with that email",
                        Payload = null
                    });

                var result = students.Select(s => s.ToResponse()).ToList();
                return Ok(new ApiResponse<List<StudentResponse>>
                {
                    Success = true,
                    Message = "Students retrieved successfully",
                    Payload = result
                });
            }
            catch (Exception)
            {
                // Log exception here if needed
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving students by email",
                    Payload = null
                });
            }
        }

        //[Authorize]
        // GET: api/student/by-name?name=Nguyen
        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Name is required",
                        Payload = null
                    });

                var students = await _studentService.GetStudentByName(name);
                if (students == null || students.Count == 0)
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "No students found with that name",
                        Payload = null
                    });

                var result = students.Select(s => s.ToResponse()).ToList();
                return Ok(new ApiResponse<List<StudentResponse>>
                {
                    Success = true,
                    Message = "Students retrieved successfully",
                    Payload = result
                });
            }
            catch (Exception)
            {
                // Log exception here if needed
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving students by name",
                    Payload = null
                });
            }
        }

        [Authorize]
        [HttpGet("GetAllStudent")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var students = await _studentService.GetAllAsync();
                var result = students.Select(s => s.ToResponse()).ToList();
                return Ok(new ApiResponse<List<StudentResponse>>
                {
                    Success = true,
                    Message = "All students retrieved successfully",
                    Payload = result
                });
            }
            catch (Exception)
            {
                // Log exception here if needed
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving all students",
                    Payload = null
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid student ID",
                        Payload = null
                    });
                    
                var student = await _studentService.GetByIdAsync(id);
                if (student == null)
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Student not found",
                        Payload = null
                    });
                    
                var result = student.ToResponse();
                return Ok(new ApiResponse<StudentResponse>
                {
                    Success = true,
                    Message = "Student retrieved successfully",
                    Payload = result
                });
            }
            catch (Exception)
            {
                // Log exception here if needed
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "An error occurred while retrieving student by ID",
                    Payload = null
                });
            }
        }
    }
}
