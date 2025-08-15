using OfflineTicketingSystem.Data.Entities;
using OfflineTicketingSystem.DTOs.Requests;
using OfflineTicketingSystem.DTOs.Responses;

namespace OfflineTicketingSystem.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Enums;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TicketsController :ControllerBase
{
    private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        // POST /api/tickets – Create a new ticket (Employee only)
        [HttpPost("Create New Ticket")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> CreateTicket([FromBody] TicketCreateDTO createDto)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var ticket = new TicketEntity()
            {
                Id = Guid.NewGuid(),
                Title = createDto.Title,
                Description = createDto.Description,
                Priority = createDto.Priority,
                Status = Status.Open,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedByUserId = userId.Value
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            var ticketDto = await MapToTicketDto(ticket);

            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticketDto);
        }

        // GET /api/tickets/my – List tickets created by the current user (Employee)
        [HttpGet("my")]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var tickets = await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .Where(t => t.CreatedByUserId == userId.Value)
                .OrderByDescending(t => t.UpdatedAt)
                .ToListAsync();

            var ticketDtos = tickets.Select(async t => await MapToTicketDto(t)).Select(t => t.Result).ToList();
            return Ok(ticketDtos);
        }

        // GET /api/tickets – List all tickets (Admin only)
        [HttpGet ("Admin")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .OrderByDescending(t => t.UpdatedAt)
                .ToListAsync();
            
            var ticketDtos = tickets.Select(async t => await MapToTicketDto(t)).Select(t => t.Result).ToList();
            return Ok(ticketDtos);
        }

        // GET /api/tickets/{id} – Get a specific ticket’s details (Bonus)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(Guid id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.CreatedByUser)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound();

            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            // Allow access if user is Admin, or if user is the creator of the ticket
            if (currentUserRole != Role.Admin.ToString() && ticket.CreatedByUserId != currentUserId)
            {
                return Forbid();
            }

            return Ok(await MapToTicketDto(ticket));
        }

        // PUT /api/tickets/{id} – Update ticket status and assignment (Admin only)
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] TicketUpdateDTO updateDto)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            // Validate AssignedToUserId if provided
            if (updateDto.AssignedToUserId.HasValue)
            {
                var adminUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == updateDto.AssignedToUserId && u.Role == Role.Admin);
                if (adminUser == null)
                {
                    return BadRequest("Assigned user must be a valid admin.");
                }
                ticket.AssignedToUserId = updateDto.AssignedToUserId;
            }
            else
            {
                ticket.AssignedToUserId = null;
            }
            
            ticket.Status = updateDto.Status;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return Ok(await MapToTicketDto(ticket));
        }

        // DELETE /api/tickets/{id} – Delete a ticket (Admin only) (Bonus)
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET /api/tickets/stats – Show ticket counts by status (Admin only)
        [HttpGet("stats")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetTicketStats()
        {
            var stats = new TicketStatsDTO()
            {
                OpenCount = await _context.Tickets.CountAsync(t => t.Status == Status.Open),
                InProgressCount = await _context.Tickets.CountAsync(t => t.Status == Status.InProgress),
                ClosedCount = await _context.Tickets.CountAsync(t => t.Status == Status.Closed),
                TotalCount = await _context.Tickets.CountAsync()
            };

            return Ok(stats);
        }

        // Helper methods
        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
        
        private string? GetCurrentUserRole()
        {
            return User.FindFirstValue(ClaimTypes.Role);
        }

        private async Task<TicketDTO> MapToTicketDto(TicketEntity ticket)
        {
            // Eager loading should be done before calling this method
            var createdByUser = ticket.CreatedByUser ?? await _context.Users.FindAsync(ticket.CreatedByUserId);
            var assignedToUser = ticket.AssignedToUser;
            if (ticket.AssignedToUserId.HasValue && assignedToUser == null)
            {
                assignedToUser = await _context.Users.FindAsync(ticket.AssignedToUserId);
            }

            return new TicketDTO()
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status.ToString(),
                Priority = ticket.Priority.ToString(),
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                CreatedByUserId = ticket.CreatedByUserId,
                CreatedByUserName = createdByUser?.FullName ?? "N/A",
                AssignedToUserId = ticket.AssignedToUserId,
                AssignedToAdminName = assignedToUser?.FullName
            };
        }
}