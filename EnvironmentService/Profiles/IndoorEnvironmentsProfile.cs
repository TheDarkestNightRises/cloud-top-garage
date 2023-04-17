namespace EnvironmentService.Profiles;

using AutoMapper;
using EnvironmentService.Dtos;
using EnvironmentService.Models;

public class IndoorEnvironmentsProfile : Profile
{
    public IndoorEnvironmentsProfile()
    {
        CreateMap<IndoorEnvironment,IndoorEnvironmentReadDto>();
        CreateMap<IndoorEnvironmentReadDto,IndoorEnvironment>();
    }
}