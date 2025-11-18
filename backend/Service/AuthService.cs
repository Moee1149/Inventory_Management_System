using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.Entity;
using Backend.Entity.Models;
using Backend.IService;
using Backend.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Service;

public class AuthService(AppDbContext _context, IConfiguration configuration) : IAuthService
{
    private PasswordHasher<User> hasher = new PasswordHasher<User>();
    public async Task<ServiceResult<string>> LoginUser(UserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return ServiceResult<string>.Fail("Field is Required", 400);
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
        {
            return ServiceResult<string>.Fail("User Not Found", 404);
        }
        var result = hasher.VerifyHashedPassword(user, user.HashedPassword, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return ServiceResult<string>.Fail("Email or Password incorrect", 401);
        }
        string token = CreateToken(user);
        return ServiceResult<string>.Ok(token, 200, "User Login Successfull");
    }

    public async Task<ServiceResult<User>> RegisterNewUser(UserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
           string.IsNullOrWhiteSpace(request.Password) ||
           string.IsNullOrWhiteSpace(request.Email))
        {
            return ServiceResult<User>.Fail("Field is Required", 400);
        }

        var userAlreadyExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (userAlreadyExists is not null)
        {
            return ServiceResult<User>.Fail("User Already Exists", 409);
        }
        User user = new User
        {
            Email = request.Email,
            Name = request.Name,
            Role = request.Role ?? "user"
        };
        var password = hasher.HashPassword(user, request.Password);
        user.HashedPassword = password;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return ServiceResult<User>.Ok(user, 200, "User registered successfully");
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var tokenDescriptor = new JwtSecurityToken(
           issuer: configuration.GetValue<string>("AppSettings:Issuer"),
           audience: configuration.GetValue<string>("AppSettings:Audience"),
           claims: claims,
           expires: DateTime.UtcNow.AddDays(1),
           signingCredentials: creds
       );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}