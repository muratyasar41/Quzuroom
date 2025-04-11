namespace CompanyManagementSystem.Web.Models
{
    public class Version
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = string.Empty; // "Ticket" veya "User"
        public int EntityId { get; set; }
        public string VersionNumber { get; set; } = string.Empty;
        public string Changes { get; set; } = string.Empty;
        public string ChangedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string PreviousVersion { get; set; } = string.Empty;
        public string CurrentState { get; set; } = string.Empty; // JSON formatında mevcut durum
    }
} 