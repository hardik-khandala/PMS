using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Repository.IRepo;
using Server.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepo _reportRepo;
        private readonly S3Service _s3service;
        public ReportController(IReportRepo reportRepo, S3Service s3Service)
        {
            _reportRepo = reportRepo;
            _s3service = s3Service;
        }

        [HttpGet("GetAllReport/{pageNumber}")]
        public async Task<IActionResult> GetAllReport(int pageNumber,[FromQuery] int pageSize, [FromQuery] DateOnly? startDate = null,[FromQuery] DateOnly? endDate = null,[FromQuery] int? deptId = null)
        {
            var data = await _reportRepo.GetPerformanceReport(startDate, endDate, deptId);

            int total = data.Count();
            int totalPage = (int)Math.Ceiling(total / (double)pageSize);

            var result = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var response = new
            {
                Data = result,
                PageNumber = total == 0 ? 0 : pageNumber,
                TotalPage = totalPage,
            };
            return Ok(response);
        }

        [HttpGet("GenerateReport/{reviewId}")]
        public async Task<IActionResult> GenerateReport(int reviewId)
        {
            var data = await _reportRepo.GetReport(reviewId);
            if(data is null)
            {
                return NotFound("Not Found");
            }

            var htmlContent = _reportRepo.GenerateHtmlContent(data);

            var pdf = _reportRepo.GeneratePerformanceReport(htmlContent);

            MemoryStream newpdf = new MemoryStream(pdf);
            

            string fileName = $"PerformanceReport_{data.EmpName}_{DateTime.Now}.pdf";
            var url = await _s3service.UploadFileAsync(fileName, newpdf);

            return Ok(new { url = url, fileName = fileName });
        }
    }
}
