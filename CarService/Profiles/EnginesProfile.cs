namespace CarService.Profile;
using AutoMapper;
using CarService.Dtos;
using CarService.Models;

public class EnginesProfile : Profile
{
    public EnginesProfile()
    {
        //Source --> Target
        CreateMap<Engine, EngineReadDto>();
        CreateMap<EngineReadDto, Engine>();
        CreateMap<EngineCreateDto, Engine>();
        CreateMap<Engine, EngineCreateDto>();
        //CreateMap<CarUpdateDto, Car>();
    }
}
