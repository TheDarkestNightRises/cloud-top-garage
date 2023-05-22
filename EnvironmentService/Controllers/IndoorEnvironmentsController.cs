using AutoMapper;
using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Dtos;
using EnvironmentService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentService.Controllers;

[ApiController]
[Route("[controller]")]
public class IndoorEnvironmentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IIndoorEnvironmentLogic _logic;

    public IndoorEnvironmentsController(IMapper mapper, IIndoorEnvironmentLogic logic)
    {
        _mapper = mapper;
        _logic = logic;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IndoorEnvironmentReadDto>>> GetAllIndoorEnvironmentsAsync()
    {
        try
        {
            var indoorEnvironments = await _logic.GetAllIndoorEnvironmentsAsync();
            var indoorEnvironmentsMapped = _mapper.Map<IEnumerable<IndoorEnvironmentReadDto>>(indoorEnvironments);
            return Ok(indoorEnvironmentsMapped);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch,Route("IndoorEnvironments/{id}/settings")]
    public async Task<IActionResult> UpdateSettingsAsync(int id, [FromBody] IndoorEnvironmentSettingsUpdateDto newSettings)
    {
        try
        {
            var settingsMapped = _mapper.Map<IndoorEnvironmentSettings>(newSettings);
            await _logic.UpdateSettingsAsync(id,settingsMapped);
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
}
