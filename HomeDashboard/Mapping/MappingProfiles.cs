using AutoMapper;
using HomeDashboard.Library.Models.Calendar;
using HomeDashboard.Library.Models.Hue;
using kriez.HomeDashboard.Data.Models;

namespace HomeDashboard.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<HueLight, HueLightDto>();
            CreateMap<HueLightDto, HueLight>();

            CreateMap<HueScene, HueSceneDto>();
            CreateMap<HueSceneDto, HueScene>();

            CreateMap<CalendarItem, CalendarItemDto>()
                .AfterMap((src, dest) => dest.Color = src.Calendar.Color);


        }
    }
}
