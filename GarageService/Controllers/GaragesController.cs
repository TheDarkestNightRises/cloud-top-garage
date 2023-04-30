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

    [HttpPost]
    public async Task<ActionResult<CarReadDto>> CreateGarageAsync([FromBody] GarageCreateDto carCreateDto)
    {
        try
        {
            // Convert from DTO to a Model
            Garage garage = _mapper.Map<Garage>(carCreateDto);

            // Delegate to the logic layer to create a garage 
            Garage created = await _logic.CreateGarageAsync(garage);

            // Map the created result into a ReadDto 
            GarageReadDto createdDto = _mapper.Map<GarageReadDto>(created);

            //Return 200 created 
            return Created($"/Garages/{created.Id}", createdDto);
        }
        catch (ArgumentException e)
        {
            // Return 400 if the client made a mistake and didnt follow the rules of creating a garage
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            // Return 500 if the system failed to create a garage
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

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

    [HttpDelete ("{id}")]
    public async Task<ActionResult> DeleteGarageAsync(int id)
    {
        try
        {
            await _logic.DeleteGarageAsync(id);
            return NoContent();
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