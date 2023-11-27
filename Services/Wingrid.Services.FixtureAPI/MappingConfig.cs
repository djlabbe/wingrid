using AutoMapper;
using Wingrid.Services.FixtureAPI.Models;
using Wingrid.Services.FixtureAPI.Models.Dto;

namespace Wingrid.Services.FixtureAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                config.CreateMap<Fixture, FixtureDto>().ReverseMap();
                config.CreateMap<Entry, EntryDto>().ReverseMap();
            });
            
            return mappingConfig;
        }
    }
}