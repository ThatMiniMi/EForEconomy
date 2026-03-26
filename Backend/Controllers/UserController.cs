using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using BCrypt.Net;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public IActionResult Register(UserDto dto)
    {
        if (_context.Users.Any(u => u.Email == dto.Email))
        {
            return BadRequest("Email already in use.");
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Address = dto.Address,
            PostalCode = dto.PostalCode,
            City = dto.City,
            Country = dto.Country,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email });
    }
}