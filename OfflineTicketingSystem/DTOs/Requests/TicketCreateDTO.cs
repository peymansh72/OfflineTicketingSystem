namespace OfflineTicketingSystem.DTOs.Requests;
using Data.Enums;
using System.ComponentModel.DataAnnotations;

public class TicketCreateDTO
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Priority Priority { get; set; }
}