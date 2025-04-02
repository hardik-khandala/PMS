using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Server.Models;
using Server.Models.DTOs;
using Server.Models.Enum;
using Server.Repository.IRepo;
using System.Text;

namespace Server.Repository
{
    public class ReportRepo : IReportRepo
    {
        private readonly AppDbContext _context;
        private readonly IConverter _converter;
        public ReportRepo(AppDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }


        public async Task<List<ReportView>> GetPerformanceReport(DateOnly? startDate, DateOnly? endDate, int? deptId)
        {
            //var query = _context.PerfomanceReviews
            //    .Include(r => r.Emp)
            //    .Include(r => r.Manager)
            //    .Include(r => r.Criteria)
            //    .Where(r => r.StatusId == Convert.ToInt32(Status.Approve) && r.IsDeleted == false);


            //if (deptId.HasValue && deptId > 0)
            //{
            //    query = query.Where(r => r.Emp.DeptId == deptId);
            //}

            //if(startDate.HasValue && endDate.HasValue)
            //{
            //    query = query.Where(r => r.StartDate >= startDate && r.EndDate <= endDate);
            //}

            //var res = await query
            //    .Select(r => new PerformanceReportDTO
            //    {
            //        ReviewId = r.ReviewId,
            //        EmpName = r.Emp.EmpName,
            //        ManagerName = r.Manager.EmpName,
            //        StartDate = r.StartDate,
            //        EndDate = r.EndDate,
            //        CriteriaName = r.Criteria.CriteriaName,
            //        SelfRating = r.SelfRating,
            //        Strength = r.Strength,
            //        Improvement = r.Improvement,
            //        ManagerRating = r.ManagerRating ?? 0,
            //        ManagerFeedback = r.ManagerFeedback ?? string.Empty
            //    })
            //    .ToListAsync();

            var parameters = new[]
            {
                new SqlParameter("@startDate", startDate ?? (object)DBNull.Value),
                new SqlParameter("@endDate", endDate ??(object)DBNull.Value),
                new SqlParameter("@deptId", deptId ?? (object)DBNull.Value)
            };

            var res = await _context.ReportViews
                .FromSqlRaw("EXEC GetReportData @startDate, @endDate, @deptId", parameters)
                .ToListAsync();

            return res;

        }
        public async Task<PerformanceReportDTO> GetReport(int reviewId)
        {
            var r = await _context.PerfomanceReviews
                .Include(r => r.Emp)
                .Include(r => r.Manager)
                .Include(r => r.Criteria)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);


            var res = new PerformanceReportDTO
            {
                ReviewId = r.ReviewId,
                EmpName = r.Emp.EmpName,
                ManagerName = r.Manager.EmpName,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                CriteriaName = r.Criteria.CriteriaName,
                SelfRating = r.SelfRating,
                Strength = r.Strength,
                Improvement = r.Improvement,
                ManagerRating = r.ManagerRating ?? 0,
                ManagerFeedback = r.ManagerFeedback ?? string.Empty
            };

            return res;
        }
        public string GenerateHtmlContent(PerformanceReportDTO report)
        {
            var sb = new StringBuilder();
            sb.Append(@"
            <html>
            <head>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        color: #333;
                        margin: 0;
                        padding: 0;
                        background-color: #f9f9f9;
                    }
                    h2 {
                        text-align: center;
                        color: #007bff;
                        padding: 20px;
                        background-color: #f2f2f2;
                        border-bottom: 2px solid #007bff;
                    }
                    table {
                        width: 80%;
                        margin: 0 auto;
                        border-collapse: collapse;
                        background-color: #fff;
                        border-radius: 8px;
                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    }
                    th, td {
                        border: 1px solid #ddd;
                        padding: 12px;
                        text-align: left;
                        font-size: 14px;
                    }
 
                    td, th {
                        background-color: #f9f9f9;
                    }
                    tr, th:nth-child(even) td {
                        background-color: #f1f1f1;
                    }
                    .highlight {
                        background-color: #f8d7da;
                        font-weight: bold;
                        color: #721c24;
                        padding: 8px;
                        border-radius: 5px;
                    }
                    .footer {
                        text-align: center;
                        font-size: 12px;
                        color: #999;
                        margin-top: 30px;
                    }
                </style>
            </head>
            <body>
                <h2>Performance Management System - Individual Report</h2>
                <table>
                    <tbody>");

                        sb.AppendFormat(@"
                        <tr><th>Employee Name</th><td>{0}</td></tr>
                        <tr><th>Manager Name</th><td>{1}</td></tr>
                        <tr><th>Start Date</th><td>{2:dd-MM-yyyy}</td></tr>
                        <tr><th>End Date</th><td>{3:dd-MM-yyyy}</td></tr>
                        <tr><th>Criteria</th><td>{4}</td></tr>
                        <tr><th>Self Rating</th><td>{5}</td></tr>
                        <tr><th>Manager Rating</th><td>{6}</td></tr>
                        <tr><th>Strength</th><td>{7}</td></tr>
                        <tr><th>Improvement</th><td>{8}</td></tr>
                        <tr><th>Manager Feedback</th><td>{9}</td></tr>",
                                report.EmpName, report.ManagerName, report.StartDate, report.EndDate,
                                report.CriteriaName, report.SelfRating, report.ManagerRating,
                                report.Strength, report.Improvement, report.ManagerFeedback);

                        sb.Append(@"
                            </tbody>
                        </table>
                    </body>
                    </html>");

                        sb.Append(@"
                    </tbody>
                </table>
    
                <div class='footer'>
                    <p>&copy; PMS. All Rights Reserved.</p>
                </div>
            </body>
            </html>");

            return sb.ToString();
        }

        public byte[] GeneratePerformanceReport(string htmlContent)
        {
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return _converter.Convert(pdf);
        }


    }
}
