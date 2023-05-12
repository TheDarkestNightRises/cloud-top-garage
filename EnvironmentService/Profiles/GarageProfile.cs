namespace EnvironmentService.Profiles;
using AutoMapper;
using EnvironmentService.Dtos;
using EnvironmentService.Models;

public class GarageProfile : Profile
{
    public GarageProfile()
    {
        CreateMap<Garage, GarageReadDto>();
        CreateMap<GarageReadDto, Garage>();
    }
}