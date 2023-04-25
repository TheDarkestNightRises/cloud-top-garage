using Application.LogicContracts;
using AutoMapper;
using CarService.Dtos;
using CarService.Models;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Carservice.Controllers;
[ApiController]
[Route("[controller]")]

public class CarsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICarLogic _logic;

    private readonly IPublishEndpoint _publishEndpoint;

    public CarsController(IMapper mapper, ICarLogic logic, IPublishEndpoint publishEndpoint)
    {
        _mapper = mapper;
        _logic = logic;
        _publishEndpoint = publishEndpoint;
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

            await _publishEndpoint.Publish(new HelloEvent("Hello"));

            return Ok(carsMapped);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<CarReadDto>> CreateCar([FromBody] CarCreateDto carCreateDto)
    {
        try
        {
            Car car = _mapper.Map<Car>(carCreateDto);
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
            Car? car = await _logic.GetCarAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            CarReadDto carReadDto = _mapper.Map<CarReadDto>(car);
            return Ok(carReadDto);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCarAsync(int id)
    {
        try
        {
            await _logic.DeleteCarAsync(id);
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


    [HttpGet, Route("/Cars/{id}/image")]
    public async Task<IActionResult> GetCarImage(int id)
    {
        try
        {
            var carImage = await _logic.GetCarImageAsync(id);
            if (carImage is not null)
            {
                return File(carImage.Data, "image/jpeg");
            }
            return NotFound();
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

    [HttpPost, Route("cars/{id}/image")]
    public async Task<IActionResult> CreateCarImage([FromRoute] int id, [FromForm] IFormFile image)
    {
        try
        {
            // Convert the image to a byte array
            byte[] imageData = null;
            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                imageData = ms.ToArray();
            }
            var carImage = await _logic.CreateCarImage(id);
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