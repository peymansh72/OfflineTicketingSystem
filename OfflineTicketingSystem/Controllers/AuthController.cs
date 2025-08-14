using OfflineTicketingSystem.Data.Entities;
using OfflineTicketingSystem.DTOs.Requests;
using OfflineTicketingSystem.DTOs.Responses;

namespace OfflineTicketingSystem.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Enums;
using DTOs;
using Helper;
using Services;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
     private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email.ToLower()))
            {
                return BadRequest("Email is already taken.");
            }
            
            var user = new UserEntity()
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email.ToLower(),
                PasswordHash = PasswordHasher.Hash(registerDto.Password),
                Role = Role.Employee 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginRequestDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            if (!PasswordHasher.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            return new UserDTO
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = _tokenService.CreateToken(user)
            };
        }
}