namespace CarService.Profile;

using AutoMapper;
using CarService.Dtos;
using CarService.Models;


public class GaragesProfile : Profile
{
    public GaragesProfile()
    {
        //Source --> Target
        CreateMap<Garage, GarageDto>();
        CreateMap<GarageDto, Garage>();
    }
}