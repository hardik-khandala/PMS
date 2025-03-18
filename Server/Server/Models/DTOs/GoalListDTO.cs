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
}
