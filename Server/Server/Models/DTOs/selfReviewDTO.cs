namespace Server.Models.DTOs
{
    public class selfReviewDTO
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int SelfRating { get; set; }
        public string Strength { get; set; }
        public string Improvement { get; set; }
        public int CriteriaId { get; set; }

    }
}
