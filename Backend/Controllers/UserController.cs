using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;

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

    [HttpPost("login")]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email });
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, UserDto dto)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.DateOfBirth = dto.DateOfBirth;
        user.Address = dto.Address;
        user.PostalCode = dto.PostalCode;
        user.City = dto.City;
        user.Country = dto.Country;
        user.PhoneNumber = dto.PhoneNumber;
        user.Email = dto.Email;

        if (!string.IsNullOrEmpty(dto.PasswordHash))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
        }

        _context.SaveChanges();

        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        _context.Users.Remove(user);
        _context.SaveChanges();

        return NoContent();
    }
}