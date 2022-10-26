using AutoMapper;

namespace NZWalks.API.Profiles
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Models.DTOs.Region, Models.DTOs.Region>()
                .ReverseMap();
        }
    }
}
