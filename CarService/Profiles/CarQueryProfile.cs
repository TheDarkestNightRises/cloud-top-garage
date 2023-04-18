using AutoMapper;
using CarService.Dtos;
using CarService.Models;

namespace CarService.Profile;

public class CarQueryProfile : Profile
{
    public CarQueryProfile()
    {
        //Source --> Target
        CreateMap<CarQueryDto, CarQuery>();
    }
}