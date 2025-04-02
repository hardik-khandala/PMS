namespace Server.Models.DTOs
{
    public class GoalListDTO
    {
        public int GoalId { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Status { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly DueDate { get; set; }
    }

    public class CriteriaListDTO
    {
        public int CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public string Details { get; set; }
    }

    public class managerListDTO
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }

    public class managerReviewList
    {
        public int ReviewId { get; set; }
        public string EmpName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Title { get; set; }
        public int SelfRating { get; set; }
        public string Strength { get; set; }
        public string Improvement { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class selfReviewListDTO
    {
        public int ReviewId { get; set; }
        public string ManagerName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Title { get; set; }
        public int SelfRating { get; set; }
        public int? ManagerRating { get; set; }
        public string ManagerFeedback { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class notificationDTO
    {
        public int NotificationId { get; set; }

        public string Message { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class RoleDTO
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }

    public class DepartmentDTO
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
    }

    public class CriteriaDTO
    {
        public string CriteriaName { get; set; }
        public string Details { get; set; }
    }

    public class DashboardData
    {
        public int TotalEmployee { get; set; }
        public int TotalReview { get; set; }
        public int TotalGoal { get; set; }
        public int TotalReport { get; set; }
    }
    public class StatusDTO
    {
        public string Status { get; set; }
    }
}
