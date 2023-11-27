using AutoMapper;
using Wingrid.Services.EventAPI.Models;
using Wingrid.Services.EventAPI.Models.Dto;

namespace Wingrid.Services.EventAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                config.CreateMap<Event, EventDto>();
                config.CreateMap<EventDto, Event>();
            });
            
            return mappingConfig;
        }
    }
}