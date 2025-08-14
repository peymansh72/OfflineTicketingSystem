namespace OfflineTicketingSystem.Data.Entities;

using Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
﻿using Microsoft.AspNetCore.Identity;

[Table("Users")]
public class UserEntity : IdentityUser<Guid>
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public Role Role { get; set; }

    // Navigation properties
    public virtual ICollection<TicketEntity> CreatedTickets { get; set; } = new List<TicketEntity>();
    public virtual ICollection<TicketEntity> AssignedTickets { get; set; } = new List<TicketEntity>();
}