using AutoMapper;
using GarageService.Application.LogicContracts;
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
            // Convert from DTO to Garage Query
            var garageQuery = _mapper.Map<GarageQuery>(garageQueryDto);

            // Delegate to the logic layer to retrieve all garages
            var garages = await _logic.GetAllGaragesAsync(garageQuery);

            // Return 404 if no garage was found
            if (garages == null || garages.Count() == 0)
            {
                return NotFound();
            }

            // Convert from Model to Read DTO
            var garagesMapped = _mapper.Map<IEnumerable<GarageReadDto>>(garages);

            //Return 200 retrieved
            return Ok(garagesMapped);
        }
        catch (Exception e)
        {
            // Return 500 if the system failed to fetch the garages
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<GarageReadDto>> GetGarageById(int id)
    {
        try
        {
            // Delegate to the logic layer to get the garage by id
            var garage = await _logic.GetGarageAsync(id);
            if (garage == null)
            {
                // Return 404 if no garage was found
                return NotFound();
            }
            // Convert from Model to Read DTO
            var garageReadDto = _mapper.Map<GarageReadDto>(garage);

            //Return 200 retrieved
            return Ok(garageReadDto);
        }
        catch (ArgumentException e)
        {
            // Return 400 if the client made a mistake and didnt follow the rules of getting a garage
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            // Return 500 if the system failed to fetch the garage
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGarageAsync(int id)
    {
        try
        {
            // Delegate to the logic layer to delete the garage by id
            await _logic.DeleteGarageAsync(id);
            // Return nothing since the object has been deleted
            return NoContent();
        }
        catch (ArgumentException e)
        {
            // Return 400 if the client made a mistake and didnt follow the rules of deleting a garage
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            // Return 500 if the system failed to fetch the garage
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}
