using AutoMapper;
using Wingrid.Services.Collector.Models;
using Wingrid.Services.Collector.Models.Dto;

namespace Wingrid.Services.Collector
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