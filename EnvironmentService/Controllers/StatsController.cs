
using AutoMapper;
using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Dtos;
using EnvironmentService.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class StatsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStatLogic _statLogic;
    public StatsController(IMapper mapper, IStatLogic statLogic)
    {
        _mapper = mapper;
        _statLogic = statLogic;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatReadDto>>> GetAllStatsAsync([FromQuery] StatQueryDto statQueryDto)
    {
        // Delegate to the logic layer to retrieve all garages 
        var statQuery = _mapper.Map<StatQuery>(statQueryDto);

        var stats = await _statLogic.GetAllStatsAsync(statQuery);

        // Return 404 if no garage was found
        if (stats == null || stats.Count() == 0)
        {
            return NotFound();
        }

        // Convert from Model to Read DTO
        var statsMapped = _mapper.Map<IEnumerable<StatReadDto>>(stats);

        //Return 200 retrieved
        return Ok(statsMapped);
    }

    [HttpGet("{garageId}/lastest")]
    public async Task<ActionResult<IEnumerable<StatReadDto>>> GetLastestStatAsync([FromRoute] int garageId)
    {
        var stat = await _statLogic.GetLastestStatAsync(garageId);

        // Return 404 if no garage was found
        if (stat is null)
        {
            return NotFound();
        }

        // Convert from Model to Read DTO
        var statMapped = _mapper.Map<StatReadDto>(stat);

        //Return 200 retrieved
        return Ok(statMapped);
    }
}
