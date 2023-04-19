using Application.LogicContracts;
using AutoMapper;
using CarService.Dtos;
using CarService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carservice.Controllers;
[ApiController]
[Route("[controller]")]

public class CarsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICarLogic _logic;

    public CarsController(IMapper mapper, ICarLogic logic)
    {
        _mapper = mapper;
        _logic = logic;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarReadDto>>> GetAllCarsAsync([FromQuery] CarQueryDto carQueryDto)
    {
        try
        {
            var carQuery = _mapper.Map<CarQuery>(carQueryDto);
            var cars = await _logic.GetAllCarsAsync(carQuery);

            if (cars == null || cars.Count() == 0)
            {
                return NotFound();
            }

            var carsMapped = _mapper.Map<IEnumerable<CarReadDto>>(cars);
            return Ok(carsMapped);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CarReadDto>> CreateCar([FromBody] CarRegisterDto carRegisterDto)
    {
        try
        {
            Car car = _mapper.Map<Car>(carRegisterDto);
            Car created = await _logic.CreateAsync(car);
            CarReadDto createdDto = _mapper.Map<CarReadDto>(created);
            return Created($"/Cars/{created.Id}", createdDto);
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

    [HttpGet("{id}")]
    public async Task<ActionResult<CarReadDto>> GetCarById(int id)
    {
        try
        {
            Car car = await _logic.GetCarAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            CarReadDto carReadDto = _mapper.Map<CarReadDto>(car);
            return Ok(carReadDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCarAsync(int id)
    {
        try
        {
            await _logic.DeleteCarAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}