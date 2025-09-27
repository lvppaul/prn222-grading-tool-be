using PRN222_TrackingTool.BusinessLayer.DTOs.Response;
using PRN222_TrackingTool.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222_TrackingTool.BusinessLayer.Mappings
{
    public static class TestMapper
    {
            //public static Test ToEntity(this TestRequest request)
            //{
            //    if (request == null) return null;

            //    return new Test
            //    {
            //        Code = request.Code,
            //        Title = request.Title,
            //        Content = request.Content,
            //        Link = request.Link,
            //        StudentId = request.StudentId,
            //        ExamId = request.ExamId,
            //        IsDeleted = false
            //    };
            //}

            public static TestResponse ToResponse(this Test entity)
            {
                if (entity == null) return null;

                return new TestResponse
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Title = entity.Title,
                    Content = entity.Content,
                    Link = entity.Link,
                    OriginalFilename = entity.OriginalFilename,
                    StudentId = entity.StudentId,   // vì StudentId trong entity là nullable
                    ExamId = entity.ExamId,
                    IsDeleted = entity.IsDeleted
                };
            }
    }
}
