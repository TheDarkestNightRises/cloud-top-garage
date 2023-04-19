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

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<CarReadDto>>> GetAllCarsAsync()
    // {
    //     try
    //     {
    //         var cars = await _logic.GetAllCarsAsync();
    //         var carsMapped = _mapper.Map<IEnumerable<CarReadDto>>(cars);
    //         return Ok(carsMapped);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(500, e.Message);
    //     }
    // }

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
    public async Tast<ActionResult< 

    //     [HttpPost]
    // public async Task<ActionResult<CommentDto>> CreateAsync([FromBody] CommentCreationDto dto)
    // {
    //     try
    //     {
    //         Comment comment = dto.AsBaseFromCreation();
    //         Comment created = await commentLogic.CreateAsync(comment);
    //         CommentDto createdDto = created.AsDto();
            
    //         return Created($"/Comments/{created.Lecture.Url}", createdDto);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(500, e.Message);
    //     }
    // }
}