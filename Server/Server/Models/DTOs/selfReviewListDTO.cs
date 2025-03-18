using System.Runtime.CompilerServices;

namespace Server.Models.DTOs
{
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
}
