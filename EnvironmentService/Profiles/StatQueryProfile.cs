using AutoMapper;
using EnvironmentService.Dtos;
using EnvironmentService.Models;

namespace EnvironmentService.Profiles;

public class StatQueryProfile : Profile
{
    public StatQueryProfile()
    {
        CreateMap<StatQueryDto, StatQuery>();
    }
}
