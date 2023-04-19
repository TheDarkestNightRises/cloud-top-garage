using GarageService.Application.LogicContracts;
using AutoMapper;
using GarageService.Dtos;
using GarageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carservice.Controllers;
[ApiController]
[Route("[controller]")]

public class GaragesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IGarageLogic _logic;

    public GaragesController(IMapper mapper, IGarageLogic logic)
    {
        _mapper = mapper;
        _logic = logic;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GarageReadDto>>> GetAllCarsAsync()
    {
        try
        {
            var garages = await _logic.GetAllGaragesAsync();
            var garagesMapped = _mapper.Map<IEnumerable<GarageReadDto>>(garages);
            return Ok(garagesMapped);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    
}