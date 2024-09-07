using AutoMapper;
using Web.Models.Dto;
using Wingrid.Models;
using Wingrid.Models.Dto;

namespace Wingrid
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Event, EventDto>().ReverseMap();
                config.CreateMap<Team, TeamDto>().ReverseMap();
                config.CreateMap<Fixture, FixtureDto>().ReverseMap();
                config.CreateMap<Entry, EntryDto>().ReverseMap();
                config.CreateMap<UserStatistics, StatisticsDto>().ReverseMap();
                config.CreateMap<ApplicationUser, UserDto>();
            });

            return mappingConfig;
        }
    }
}