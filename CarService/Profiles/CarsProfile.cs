namespace Carservice.Profile;
using AutoMapper;
using Carservice.Dtos;
using Carservice.Models;

public class CarsProfile : Profile
{
    public CarsProfile()
    {
        //Source --> Target
        CreateMap<Car, CarReadDto>();
        CreateMap<CarReadDto, Car>();
    }
}