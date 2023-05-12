using AutoMapper;
using EnvironmentService.Dtos;
using EnvironmentService.Models;

public class StatProfile : Profile
{
    public StatProfile()
    {
        CreateMap<Stat, StatReadDto>();
        CreateMap<StatReadDto, Stat>();
    }
}