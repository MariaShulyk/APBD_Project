using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ProjectAPBD.Context;
using ProjectAPBD.Helpers;
using ProjectAPBD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectAPBD.ReqModels;

namespace ProjectAPBD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(IConfiguration _configuration, ContextDB _context) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult RegisterUser(RegisterReqModel registerModel)
    {
        var hashedPassword = SecurityHelpers.GetHashedPasswordAndSalt(registerModel.Password);
        var emp = new Employee()
        {
            Login = registerModel.Login,
            Password = hashedPassword.Item1,
            Salt = hashedPassword.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExpiration = DateTime.Now.AddDays(1)
        };
        _context.Employees.Add(emp);
        _context.SaveChanges();

        return Ok($"Employee {registerModel.Login} was added to the system");
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public IActionResult Refresh(RefreshTokenReqModel refreshToken)
    {
        Employee emp = _context.Employees.Where(u => u.RefreshToken == refreshToken.RefreshToken).FirstOrDefault();
        if (emp == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (emp.RefreshTokenExpiration < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        emp.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        emp.RefreshTokenExpiration = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            refreshToken = emp.RefreshToken
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginReqModel loginModel)
    {
        Employee emp = _context.Employees.Where(u => u.Login == loginModel.Login).FirstOrDefault();

        if (emp == null)
        {
            return Unauthorized("Wrong username or wrong password");
        }

        string passwordHashFromDb = emp.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginModel.Password, emp.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized("Wrong username or wrong password");
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        emp.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        emp.RefreshTokenExpiration = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = emp.RefreshToken
        });
    }
}