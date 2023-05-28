using System.ComponentModel.DataAnnotations;
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

    public CarsController(IMapper mapper, ICarLogic logic)
    {
        _mapper = mapper;
        _logic = logic;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarReadDto>>> GetAllCarsAsync([FromQuery] CarQueryDto carQueryDto)
    {
        // try
        // {
        var carQuery = _mapper.Map<CarQuery>(carQueryDto);
        var cars = await _logic.GetAllCarsAsync(carQuery);

        if (cars == null || cars.Count() == 0)
        {
            return NotFound();
        }

        var carsMapped = _mapper.Map<IEnumerable<CarReadDto>>(cars);


        return Ok(carsMapped);
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateCarAsync([FromBody] CarUpdateDto carUpdateDto)
    {
        var carToUpdate = _mapper.Map<Car>(carUpdateDto);
        await _logic.UpdateCarAsync(carToUpdate);
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<CarReadDto>> CreateCar([FromBody] CarCreateDto carCreateDto)
    {
        // Convert from DTO to a Model
        Car car = _mapper.Map<Car>(carCreateDto);

        // Delegate to the logic layer to create a car 
        Car created = await _logic.CreateCarAsync(car);

        // Map the created result into a ReadDto 
        CarReadDto createdDto = _mapper.Map<CarReadDto>(created);

        //Return 200 created 
        return Created($"/Cars/{created.Id}", createdDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CarReadDto>> GetCarById(int id)
    {
        Car? car = await _logic.GetCarAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        CarReadDto carReadDto = _mapper.Map<CarReadDto>(car);
        return Ok(carReadDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCarAsync(int id)
    {
        await _logic.DeleteCarAsync(id);
        return NoContent();
    }


    [HttpGet, Route("/Cars/{id}/image")]
    public async Task<IActionResult> GetCarImage(int id)
    {
        var carImage = await _logic.GetCarImageAsync(id);
        if (carImage is not null)
        {
            return File(carImage.Data, "image/jpeg");
        }
        return NotFound();
    }

    [HttpPost, Route("/Cars/{id}/image")]
    public async Task<IActionResult> CreateCarImage([FromRoute] int id, IFormFile image)
    {
        // Convert the image to a byte array
        byte[]? imageData = null;
        using (var ms = new MemoryStream())
        {
            await image.CopyToAsync(ms);
            imageData = ms.ToArray();
        }
        //Create Image model
        Image carImage = new Image
        {
            Data = imageData
        };
        var created = await _logic.CreateCarImage(carImage, id);
        return File(created.Data, "image/jpeg");
    }


}
