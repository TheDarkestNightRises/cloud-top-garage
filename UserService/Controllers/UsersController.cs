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
    public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
    {
        try
        {
            var userToUpdate = _mapper.Map<User>(userUpdateDto);
            await _userLogic.UpdateUser(userToUpdate);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.Write(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
    {
        try
        {
            var user = _mapper.Map<User>(userCreateDto);
            var userCreated = await _userLogic.CreateUser(user);
            UserReadDto userReadDto = _mapper.Map<UserReadDto>(userCreated);
            return Created($"/users/{userReadDto.Id}", userReadDto);
        }
        catch (Exception e)
        {
            Console.Write(e);
            return StatusCode(500, e.Message);
        }
    }
}
