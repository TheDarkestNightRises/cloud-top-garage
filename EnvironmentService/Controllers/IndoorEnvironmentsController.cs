using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentService.Controllers;

[ApiController]
[Route("[controller]")]
public class IndoorEnvironmentsController: ControllerBase
{
   private readonly IMapper _mapper;

      public IndoorEnvironmentsController(IMapper mapper, ILogic logic)
    {
        _mapper = mapper;
        _logic = logic;
    }
}