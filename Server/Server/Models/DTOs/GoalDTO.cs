namespace Server.Models.DTOs
{
    public class GoalDTO
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public int StatusId { get; set; }
        public DateOnly DueDate { get; set; }
    }
}
