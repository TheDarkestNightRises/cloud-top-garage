using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.Logic;
using UserService.Models;

namespace UserService.Controllers;
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserLogic _userLogic;
    private readonly IMapper _mapper;

    public UsersController(IUserLogic userLogic, IMapper mapper)
    {
        _userLogic = userLogic;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsers()
    {
        var users = await _userLogic.GetAllUsersAsync();
        var usersMapped = _mapper.Map<IEnumerable<UserReadDto>>(users);
        return Ok(usersMapped);
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateUserPassword([FromBody] UserUpdateDto userUpdateDto)
     {
        try
        {
            var userToUpdate = _mapper.Map<User>(userUpdateDto);
            await _userLogic.UpdateUserPassword(userToUpdate);
            return NoContent();
        }
        catch(Exception e)
        {
          Console.Write(e);
          return StatusCode(500, e.Message);
        }
     }
}
