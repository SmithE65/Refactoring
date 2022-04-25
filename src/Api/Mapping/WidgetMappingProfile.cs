using Api.Dtos;
using AutoMapper;
using Core.Models;

namespace Api.Mapping;

public class WidgetMappingProfile : Profile
{
    public WidgetMappingProfile()
    {
        CreateMap<WidgetDto, Widget>();
        CreateMap<Widget, WidgetDto>();
        CreateMap<PropertyDto, Property>();
        CreateMap<Property, PropertyDto>();
    }
}
