
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
        try
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
        catch (Exception e)
        {
            // Return 500 if the system failed to fetch the garages
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}
