namespace CarService.Profile;

using AutoMapper;
using CarService.Dtos;
using CarService.Models;


public class CarQueryProfile : Profile
{
    public CarQueryProfile()
    {
        //Source --> Target
        CreateMap<CarQueryDto, CarQuery>();
    }
}