using AutoMapper;
using AirlineWeb.Dtos;
using AirlineWeb.Models;

namespace AirlineWeb.Profiles;

public class FlightsProfile : Profile
{
    public FlightsProfile()
    {
        CreateMap<FlightDetailCreateDto, FlightDetail>();
        CreateMap<FlightDetailUpdateDto, FlightDetail>();
        CreateMap<FlightDetail, FlightDetailReadDto>();
    }
}