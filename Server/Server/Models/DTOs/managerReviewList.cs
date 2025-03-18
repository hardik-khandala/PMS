namespace Server.Models.DTOs
{
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
}
