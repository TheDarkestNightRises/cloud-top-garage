using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EnvironmentService.Application.LogicContracts;

namespace EnvironmentService.Controllers;

[ApiController]
[Route("[controller]")]
public class IndoorEnvironmentsController: ControllerBase
{
   private readonly IMapper _mapper;
    private readonly IIndoorEnvironmentLogic _logic;

    public IndoorEnvironmentsController(IMapper mapper, IIndoorEnvironmentLogic logic)
    {
        _mapper = mapper;
        _logic = logic;
    }
}