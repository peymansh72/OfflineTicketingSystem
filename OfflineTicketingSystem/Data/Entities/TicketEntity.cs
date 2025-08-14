namespace OfflineTicketingSystem.Data.Entities;
using Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Tickets")]
public class TicketEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Foreign Key for the user who created the ticket
    public Guid CreatedByUserId { get; set; }
    [ForeignKey("CreatedByUserId")]
    public virtual UserEntity CreatedByUser { get; set; } = null!;

    // Nullable Foreign Key for the admin assigned to the ticket
    public Guid? AssignedToUserId { get; set; }
    [ForeignKey("AssignedToUserId")]
    public virtual UserEntity? AssignedToUser { get; set; }
}