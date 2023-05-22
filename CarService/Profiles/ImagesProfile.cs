namespace CarService.Profile;
using AutoMapper;
using CarService.Dtos;
using CarService.Models;

public class ImagesProfile : Profile
{
    public ImagesProfile()
    {
        //Source --> Target
        CreateMap<Image, ImageReadDto>();
        CreateMap<ImageReadDto, Image>();
    }
}
