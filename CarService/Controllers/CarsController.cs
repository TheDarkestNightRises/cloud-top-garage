using Application.LogicContracts;
using AutoMapper;
using Carservice.Dtos;
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
    public async Task<ActionResult<IEnumerable<CarReadDto>>> GetAllCarsAsync()
    {
        try
        {
            var cars = _logic.GetAllCarsAsync();
            var carsMapped = _mapper.Map<IEnumerable<CarReadDto>>(cars);
            return Ok(carsMapped);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}