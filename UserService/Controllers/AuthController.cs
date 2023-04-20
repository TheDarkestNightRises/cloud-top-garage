using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.Logic;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    private readonly IMapper _mapper;

    public AuthController(IUserLogic userLogic, IMapper mapper)
    {
        _userLogic = userLogic;
        _mapper = mapper;
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] UserAuthDto userAuthDto)
    {
        try
        {
            var user = _userLogic.LoginUserAsync(userAuthDto.Email, userAuthDto.Password);
            string token = GenerateJtw(user);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


}
