namespace EnvironmentService.Profiles;

using AutoMapper;
using EnvironmentService.Dtos;
using EnvironmentService.Models;

public class IndoorEnvironmentsSettingsProfile : Profile
{
    public IndoorEnvironmentsSettingsProfile()
    {
        CreateMap<IndoorEnvironmentSettings, IndoorEnvironmentSettingsReadDto>();
        CreateMap<IndoorEnvironmentSettingsReadDto, IndoorEnvironmentSettings>();
        CreateMap<IndoorEnvironmentSettingsUpdateDto,IndoorEnvironmentSettings>();

    }
}
