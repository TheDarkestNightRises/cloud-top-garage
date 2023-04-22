namespace CarService.Profile;

using AutoMapper;
using GarageService.Dtos;
using GarageService.Models;


public class UsersProfile : Profile
{
    public UsersProfile()
    {
        //Source --> Target
        CreateMap<User, UserReadDto>();
        CreateMap<UserReadDto, User>();
    }
}