namespace OfflineTicketingSystem.DTOs.Requests;
using Data.Enums;

public class TicketUpdateDTO
{
    public required Status Status { get; set; }
        
    public Guid? AssignedToUserId { get; set; }
}