using AutoMapper;
using CommandService.DTO;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            //Source -> Target
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<Command, CommandReadDTO>();
            CreateMap<CommandCreateDTO, Command>();
        }
    }
}
