using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserService.Dtos;
using UserService.Logic;
using UserService.Models;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public AuthController(IUserLogic userLogic, IMapper mapper, IConfiguration config)
    {
        _userLogic = userLogic;
        _mapper = mapper;
        _config = config;
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] UserAuthDto userAuthDto)
    {
        try
        {
            User user = await _userLogic.LoginUserAsync(userAuthDto.Email, userAuthDto.Password);
            string token = GenerateJtw(user);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private string GenerateJtw(User user)
    {
        var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

        JwtHeader header = new JwtHeader(Credentials);

        List<Claim> claims = GenerateClaims(user);

        JwtPayload payload = new JwtPayload(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            null,
            DateTime.UtcNow.AddMinutes(30));

        JwtSecurityToken token = new JwtSecurityToken(header, payload);

        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }

    private List<Claim> GenerateClaims(User user)
    {
        var claims = new[]
       {
            new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("DisplayName", user.Name),
        };
        return claims.ToList();
    }

}
