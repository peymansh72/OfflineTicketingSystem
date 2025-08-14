namespace OfflineTicketingSystem.DTOs.Responses;

public class TicketDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } 
    public string Priority { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public Guid? AssignedToUserId { get; set; }
    public string? AssignedToAdminName { get; set; }
}