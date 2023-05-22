using AutoMapper;
using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Dtos;
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
}
