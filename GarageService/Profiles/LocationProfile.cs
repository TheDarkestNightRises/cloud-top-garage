namespace CarService.Profile;

using AutoMapper;
using GarageService.Dtos;
using GarageService.Models;


public class LocationProfile : Profile
{
    public LocationProfile()
    {
        //Source --> Target
        CreateMap<Location, LocationReadDto>();
        CreateMap<LocationReadDto, Location>();
        CreateMap<Location, LocationCreateDto>();
        CreateMap<LocationCreateDto, Location>();
    }
}