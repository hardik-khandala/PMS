using System.ComponentModel.DataAnnotations;

namespace Server.Models.DTOs
{
    public class selfReviewDTO
    {
        [Required(ErrorMessage = "Start Date is Required")]
        public DateOnly StartDate { get; set; }
        [Required(ErrorMessage = "End Date is Required")]
        public DateOnly EndDate { get; set; }
        [Required(ErrorMessage = "Self Rating Required"), Range(1, 5, ErrorMessage = "Range shoud be between 1 to 5")]
        public int SelfRating { get; set; }
        [Required(ErrorMessage ="Strength is required")]
        public string Strength { get; set; }
        [Required(ErrorMessage = "Improvement is required")]
        public string Improvement { get; set; }
        [Required]
        public int CriteriaId { get; set; }

    }
    public class managerReviewDTO
    {
        public int ReviewId { get; set; }
        [Required(ErrorMessage = "Manager Rating Required"), Range(1, 5, ErrorMessage = "Range shoud be between 1 to 5")]
        public int ManagerRating { get; set; }
        [Required(ErrorMessage = "Manager Feedback is required")]
        public string ManagerFeedback { get; set; }
    }

    public class PerformanceReportDTO
    {
        public int ReviewId { get; set; }
        public string EmpName { get; set; }
        public string ManagerName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string CriteriaName { get; set; }
        public int SelfRating { get; set; }
        public string Strength { get; set; }
        public string Improvement { get; set; }
        public int ManagerRating { get; set; }
        public string ManagerFeedback { get; set; }
    }
}
