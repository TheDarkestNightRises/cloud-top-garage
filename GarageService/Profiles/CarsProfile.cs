namespace CarService.Profile;

using AutoMapper;
using GarageService.Dtos;
using GarageService.Models;


public class CarsProfile : Profile
{
    public CarsProfile()
    {
        //Source --> Target
        CreateMap<Car, CarReadDto>();
        CreateMap<CarReadDto, Car>();
    }
}