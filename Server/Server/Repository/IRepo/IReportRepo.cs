using Server.Models;
using Server.Models.DTOs;

namespace Server.Repository.IRepo
{
    public interface IReportRepo
    {
        Task<List<ReportView>> GetPerformanceReport(DateOnly? startDate, DateOnly? endDate, int? deptId);
        Task<PerformanceReportDTO> GetReport(int reviewId);
        byte[] GeneratePerformanceReport(string htmlContent);
        string GenerateHtmlContent(PerformanceReportDTO report);
    }
}
