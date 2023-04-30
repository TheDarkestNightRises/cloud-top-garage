namespace CarService.Profile;

using AutoMapper;
using GarageService.Dtos;
using GarageService.Models;


public class GaragesProfile : Profile
{
    public GaragesProfile()
    {
        //Source --> Target
        CreateMap<Garage, GarageReadDto>();
        CreateMap<GarageReadDto, Garage>();
        CreateMap<GarageQuery, GarageQueryDto>();
        CreateMap<GarageQueryDto, GarageQuery>();
        CreateMap<Garage, GarageCreateDto>();
        CreateMap<GarageCreateDto, Garage>();
    }
}