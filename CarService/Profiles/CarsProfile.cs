namespace CarService.Profile;
using AutoMapper;
using CarService.Dtos;
using CarService.Models;

public class CarsProfile : Profile
{
    public CarsProfile()
    {
        //Source --> Target
        CreateMap<Car, CarReadDto>();
        CreateMap<CarReadDto, Car>();
        CreateMap<CarCreateDto, Car>();
        CreateMap<Car, CarReadDto>();
        CreateMap<CarUpdateDto, Car>();
    }
}