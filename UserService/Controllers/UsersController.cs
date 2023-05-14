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
        try
        {
            var users = await _userLogic.GetAllUsersAsync();
            if (users == null)
            {
                return NotFound();
            }
            var usersMapped = _mapper.Map<IEnumerable<UserReadDto>>(users);
            return Ok(usersMapped);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateUserAsync([FromBody] UserUpdateDto userUpdateDto)
    {
        try
        {
            var userToUpdate = _mapper.Map<User>(userUpdateDto);
            await _userLogic.UpdateUser(userToUpdate);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
    {
        try
        {
            var user = _mapper.Map<User>(userCreateDto);
            var userCreated = await _userLogic.CreateUser(user);
            UserReadDto userReadDto = _mapper.Map<UserReadDto>(userCreated);
            return Created($"/users/{userReadDto.Id}", userReadDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<UserReadDto>> GetUserByEmailAsync(string email)
    {
        try
        {
            // Delegate to the logic layer to get the user by email
            var user = await _userLogic.GetUserByEmail(email);
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
        catch (Exception e)
        {
            // Return 500 if the system failed to fetch the user
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}
