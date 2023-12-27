using AutoMapper;
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
            });

            return mappingConfig;
        }
    }
}