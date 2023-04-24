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

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<GarageReadDto>>> GetAllCarsAsync()
    // {
    //     try
    //     {
    //         var garages = await _logic.GetAllGaragesAsync();
    //         var garagesMapped = _mapper.Map<IEnumerable<GarageReadDto>>(garages);
    //         return Ok(garagesMapped);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(500, e.Message);
    //     }
    // }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GarageReadDto>>> GetAllGaragesAsync([FromQuery] GarageQueryDto garageQueryDto)
    {
        try
        {
            var garageQuery = _mapper.Map<GarageQuery>(garageQueryDto);
            var garages = await _logic.GetAllGaragesAsync(garageQuery);

            if (garages == null || garages.Count() == 0)
            {
                return NotFound();
            }

            var garagesMapped = _mapper.Map<IEnumerable<GarageReadDto>>(garages);

            return Ok(garagesMapped);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<GarageReadDto>> GetGarageById(int id)
    {
        try
        {
            var garage = await _logic.GetGarageAsync(id);
            if (garage == null)
            {
                return NotFound();
            }
            var garageReadDto = _mapper.Map<GarageReadDto>(garage);
            return Ok(garageReadDto);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}