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
    public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsersAsync()
    {
        var users = await _userLogic.GetAllUsersAsync();
        if (users == null)
        {
            return NotFound();
        }
        var usersMapped = _mapper.Map<IEnumerable<UserReadDto>>(users);
        return Ok(usersMapped);
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateUserAsync([FromBody] UserUpdateDto userUpdateDto)
    {
        var userToUpdate = _mapper.Map<User>(userUpdateDto);
        await _userLogic.UpdateUserAsync(userToUpdate);
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
    {
        var user = _mapper.Map<User>(userCreateDto);
        var userCreated = await _userLogic.CreateUserAsync(user);
        UserReadDto userReadDto = _mapper.Map<UserReadDto>(userCreated);
        return Created($"/users/{userReadDto.Id}", userReadDto);
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<UserReadDto>> GetUserByEmailAsync(string email)
    {
        var user = await _userLogic.GetUserByEmailAsync(email);
        if (user == null)
        {
            // Return 404 if no user was found
            return NotFound();
        }
        // Convert from Model to Read DTO
        var userReadDto = _mapper.Map<UserReadDto>(user);

        //Return 200 retrieved
        return Ok(userReadDto);
    }

}
