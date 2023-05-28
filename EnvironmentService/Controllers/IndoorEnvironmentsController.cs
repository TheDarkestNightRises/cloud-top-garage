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

        var indoorEnvironments = await _logic.GetAllIndoorEnvironmentsAsync();
        var indoorEnvironmentsMapped = _mapper.Map<IEnumerable<IndoorEnvironmentReadDto>>(indoorEnvironments);
        return Ok(indoorEnvironmentsMapped);
    }

    [HttpPatch("{id}/settings")]
    public async Task<IActionResult> UpdateSettingsAsync(int id, [FromBody] IndoorEnvironmentSettingsUpdateDto newSettings)
    {
        var settingsMapped = _mapper.Map<IndoorEnvironmentSettings>(newSettings);
        await _logic.UpdateSettingsAsync(id, settingsMapped);
        return NoContent();
    }
}
