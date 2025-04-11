namespace CompanyManagementSystem.Web.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ResolvedAt { get; set; }
    }

    public enum TicketStatus
    {
        New,
        InProgress,
        Resolved
    }
} 